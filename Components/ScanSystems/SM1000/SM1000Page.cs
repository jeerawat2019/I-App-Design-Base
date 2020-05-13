using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using MCore.Controls;
using MDouble;
using CtiUtils;
using LanmarkControls.LECBroadcast;
using LanmarkControls.LECSession;

namespace MCore.Comp.ScanSystem
{
    public partial class SM1000Page : UserControl, IComponentBinding<SM1000>
    {
        private bool _bGuiConnect = true;
        private SM1000 _sm1000 = null;
        private Broadcast cA = null;
        private const string strBroadcastAddress = "224.168.100.2";	// from AdminConfig.xml
        private const int iBroadcastPortNumber = 11000;			//
        private int piClientId = -1;

        private ECUtils ECUtil
        {
            get { return _sm1000.ECUtil; }
        }
        private Session ECSession
        {
            get { return _sm1000.ECSession; }
        }
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SM1000Page()
        {
            InitializeComponent();
            rbController.Tag = 5;
            rbLaser.Tag = 6;
            rbLens.Tag = 2;
            rbCorrection.Tag = 0xd;
            rbUser.Tag = 0xf;
            rbPerformance.Tag = 0x10;
            rbAdmin.Tag = 0xa;
            rbList.Tag = 0;
            rbJobList.Tag = -1;
            rbJobCopy.Tag = -2;
        }

        #region Overrides
        /// <summary>
        /// Generic Title for the property page
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Scan Master";
        }
        #endregion Overrides

        /// <summary>
        /// Bind to the Controller
        /// </summary>
        /// <param name="SM1000"></param>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SM1000 Bind
        {
            get { return _sm1000; }
            set
            {
                _sm1000 = value;
                cA = Broadcast.Instance;
                ECSession_OnConnectionEvent(_sm1000.RemoteIP, ECSession.SessionActive);
                ECSession.OnConnectionEvent += new ConnectionEventHandler(ECSession_OnConnectionEvent);
                ECSession.OnDataEvent += new DataEventHandler(ECSession_OnDataEvent);
                mdXPos.BindTwoWay(() => _sm1000.XPos);
                mdYPos.BindTwoWay(() => _sm1000.YPos);

            }
        }

        private delegate void delUintUintString(uint u1, uint u2, string strVal);
        void ECSession_OnDataEvent(uint puiPayloadHigh, uint puiPayloadLow, string pstrData)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new delUintUintString(ECSession_OnDataEvent), new object[] { puiPayloadHigh, puiPayloadLow, pstrData });
                return;
            }
            tbResults.Text = pstrData;
        }

        private delegate void delStringBool(string strVal, bool bVal);

        private void ECSession_OnConnectionEvent(string pstrName, bool pbConnected)
        {
            if (pstrName != _sm1000.RemoteIP)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new delStringBool(ECSession_OnConnectionEvent), new object[] { pstrName, pbConnected });
                return;
            }
            _bGuiConnect = false;
            chkBoxConnected.Checked = pbConnected;
            _bGuiConnect = true;
            if (pbConnected)
            {
                chkBoxConnected.Text = string.Format("Connected to '{0}'", _sm1000.DeviceName);
            }
            else
            {
                chkBoxConnected.Text = "Disconnected";
                _sm1000.Detach();
            }           
        }

        private void btnShowAddresses_Click(object sender, EventArgs e)
        {
            Cursor origCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            _sm1000.Detach();
            listAddresses.Items.Clear();
            string strLocalIPAddresses;
            List<string> listAddr = new List<string>();
            ECUtil.GetLocalIPAddress(out strLocalIPAddresses);
            if (!string.IsNullOrEmpty(strLocalIPAddresses))
            {
                string[] strAddresses = strLocalIPAddresses.Split(',');
                foreach (string strAddress in strAddresses)
                {
                    if (!listAddr.Contains(strAddress) && strAddress != "0.0.0.0")
                    {
                        uint err = cA.clientAttachBroadcast(strBroadcastAddress, strAddress, iBroadcastPortNumber, ref piClientId);
                        if (0 == err && piClientId != -1)
                        {
                            for (int i = 0; i < 6; i++)
                            {
                                int iServerCount;
                                string strServerList;
                                uint iRet = cA.getLECServerList(piClientId, out iServerCount, out strServerList);
                                if (iRet == 0 && iServerCount > 0)
                                {
                                    string strSysInfo = string.Empty;
                                    string strName = string.Empty;
                                    ECUtil.ReadFromXML(strServerList, 0, "LEC", "name", out strName);
                                    ListBoxItem lbi = new ListBoxItem(strName);
                                    ECUtil.ReadFromXML(strServerList, 0, "LEC", "localip", out lbi.localIP);
                                    if (lbi.localIP == strAddress)
                                    {
                                        uint err2 = cA.getBroadcastData(piClientId, strName, 0x00000001, out strSysInfo);
                                        if (0 == err2)
                                        {
                                            ECUtil.ReadFromXML(strSysInfo, 0, "IP", null, out lbi.remoteIP);
                                            string strTemp = "";
                                            ECUtil.ReadFromXML(strSysInfo, 0, "Port", null, out strTemp);
                                            lbi.portNumber = Convert.ToInt32(strTemp);
                                            listAddresses.Items.Add(lbi);
                                            listAddr.Add(strAddress);
                                        }
                                    }
                                    break;
                                }
                                Thread.Sleep(1000);
                            }
                            cA.clientDetachBroadcast(piClientId);
                        }
                    }
                }
            }
            _sm1000.Attach();
            Cursor.Current = origCursor;
        }

        private void OnSelIndexChanged(object sender, EventArgs e)
        {
            ListBoxItem lbi = listAddresses.SelectedItem as ListBoxItem;
            if (lbi != null)
            {
                _sm1000.AttachToDevice(lbi.name, lbi.localIP, lbi.remoteIP, lbi.portNumber);
            }
        }

        private void OnConnectedChanged(object sender, EventArgs e)
        {
            if (_bGuiConnect == false)
                return;

            if (chkBoxConnected.Checked)
            {
                _sm1000.Attach();
            }
            else
            {
                _sm1000.Detach();
            }

        }

        private void OnExecute(object sender, EventArgs e)
        {
            tbResults.Text = "";
            var checkedButton = gbDataTypes.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
            object oTag = checkedButton.Tag;
            string value2 = string.Format("{0},{1}", _sm1000.XPos.Val, _sm1000.YPos.Val);

            if (oTag == null)
            {
                if (checkedButton.Text == "Get")
                {
                    _sm1000.RequestData(tbArg.Text);
                }
                else
                {
                    _sm1000.SendData(tbArg.Text, value2);
                }
                tbResults.Text = "Done";
            }
            else if (oTag is string)
            {
                string sTag = oTag as string;
                switch(checkedButton.Text)
                {
                    case "LaserOn(us)":
                        _sm1000.SendData(sTag, tbArg.Text);
                        break;
                    case "LaserSigOn":
                    case "LaserSigOff":
                        _sm1000.SendData(sTag, string.Empty);
                        break;
                    case "LaserEnable":
                        _sm1000.SendSetData("EnableLaser", "true");
                        break;
                    case "LaserDisable":
                        _sm1000.SendSetData("EnableLaser", "false");
                        break;
                    default:
                        _sm1000.SendData(oTag as string, value2);
                        break;
                }
                tbResults.Text = "Done";
            }
            else
            {
                // Get Fixed configuration data
                string str = _sm1000.RequestFixedData((int)oTag, tbArg.Text);

                if (!string.IsNullOrEmpty(str))
                {
                    tbResults.Text = str;
                }
            }
        }

    }
    class ListBoxItem
    {
        public string name = string.Empty;
        public string localIP = string.Empty;
        public string remoteIP = string.Empty;
        public int portNumber = 0;
        public ListBoxItem(string name) 
        {
            this.name = name;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1} - {2} - {3}", name, localIP, remoteIP, portNumber);
        }
    }
}
