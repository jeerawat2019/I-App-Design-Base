using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MCore.Controls;

namespace MCore.Comp.VisionSystem
{
    /// <summary>
    /// Vision Job Run Page
    /// </summary>
    public partial class VisionJobRunPage : UserControl, IComponentBinding<VisionJobBase>
    {
        private bool _ignoreSelection = true;
        private VisionJobBase _visionJob = null;
       // public event MethodInvoker OnNewImage = null;
        
        private object _acquiredImage = null;
        /// <summary>
        /// Get the newly acquired bitmap image
        /// </summary>
        public object AcquiredImage
        {
            get { return _acquiredImage; }
            set { _acquiredImage = value;}
        }

        /// <summary>
        /// Owner to receive image
        /// </summary>
        public CompBase ImageOwner
        {
            get;
            set;
        }

        enum eCol { DataName, DataValue, Min, Max, Mode, Actual, KeyName };
        /// <summary>
        /// Constructor
        /// </summary>
        public VisionJobRunPage()
        {
            InitializeComponent();           
        }


        #region IComponentBinding<VisionJobBase> Members

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public VisionJobBase Bind
        {
            get { return _visionJob; }
            set
            {
                _ignoreSelection = true;
                _visionJob = value;
                mdStrobeDuration.BindTwoWay(() => _visionJob.StrobeDuration);
                dgCogJobResult.Rows.Clear();
                RefreshComboNames();
                this.mode.Items.AddRange(Enum.GetNames(typeof(SpecBase.eMode)));
                if (_visionJob.RefResults != null && _visionJob.RefResults.dataMap != null)
                {

                    int resultCount = _visionJob.RefResults.Results.Count;
                    foreach (ExchangeDataMapBase dataItem in _visionJob.RefResults.dataMap)
                    {
                        bool found = true;
                        if (resultCount == 0)
                        {
                            if (!this.visionName.Items.Contains(dataItem.Key))
                            {
                                if (string.IsNullOrEmpty(dataItem.Key))
                                {
                                    dataItem.Key = ResultsExchange.NONE;
                                }
                                else
                                {
                                    this.visionName.Items.Add(dataItem.Key);
                                }
                            }
                        }
                        else if (!this.visionName.Items.Contains(dataItem.Key))
                        {
                            // data item no longer exists,  Notify user and reset
                            //U.LogPopup("vision data name '{0}' for data '{1}' does not exist in the vision results.  Please select a valid name",
                            //    dataItem.Key, dataItem.DataName);
                            //dataItem.Key = ResultsExchange.NONE;
                            found = false;
                        }
                        // DataName, DataValue, Min, Max, Mode, Actual, KeyName 
                        int iRow = dgCogJobResult.Rows.Add(
                                dataItem.DataName, 
                                dataItem.DataToString, 
                                dataItem.MinString, 
                                dataItem.MaxString, 
                                dataItem.Mode.ToString(), 
                                dataItem.Actual.ToString(), 
                                dataItem.Key 
                            );
                        DataGridViewCell cell = dgCogJobResult[(int)eCol.KeyName, iRow];
                        cell.Tag = found;
                    }
                }
                UpdateData();
                _ignoreSelection = false;
            }
        }

        #endregion

        private void RefreshComboNames()
        {
            this.visionName.Items.Clear();
            this.visionName.ValueType = typeof(string);
            List<string> list = new List<string>();
            list.Add(ResultsExchange.NONE);
            if (_visionJob.RefResults != null)
            {
                int resultCount = _visionJob.RefResults.Results.Count;
                if (resultCount > 0)
                {
                    foreach (string resultName in _visionJob.RefResults.Results.Keys)
                    {
                        list.Add(resultName);
                    }
                }
            }
            this.visionName.Items.AddRange(list.ToArray());
        }

        public void UpdateData()
        {
            _ignoreSelection = true;
            bool overallSuccess = true;
            Color bkGrnd = Color.YellowGreen;
            lblResultsError.Text = _visionJob.ResultsError;
            RefreshComboNames();
            if (!_visionJob.ResultsSuccess)
            {
                overallSuccess = false;
                bkGrnd = Color.LightSalmon;
            }
            if (_visionJob.RefResults != null && _visionJob.RefResults.dataMap != null && _visionJob.RefResults.Results.Count > 0)
            {
                try
                {
                    for (int iRow = 0; iRow < _visionJob.RefResults.dataMap.Length; iRow++)
                    {
                        bool found = true;
                        bkGrnd = Color.Green;
                        ExchangeDataMapBase dataItem = _visionJob.RefResults.dataMap[iRow];
                        if (dataItem.Mode != SpecBase.eMode.None && !dataItem.IsInSpec)
                        {
                            overallSuccess = false;
                            bkGrnd = Color.LightSalmon;
                        }

                        DataGridViewRow row = dgCogJobResult.Rows[iRow];
                        row.Cells[(int)eCol.DataValue].Value = dataItem.DataToString;
                        if (!this.visionName.Items.Contains(dataItem.Key))
                        {
                            // data item no longer exists,  Notify user and reset
                            //U.LogPopup("vision data name '{0}' for data '{1}' does not exist in the vision results.  Please select a valid name",
                            //    dataItem.Key, dataItem.DataName);
                            //dataItem.Key = ResultsExchange.NONE;
                            found = false;
                        }
                        row.Cells[(int)eCol.Min].Value = dataItem.MinString;
                        row.Cells[(int)eCol.Max].Value = dataItem.MaxString;
                        row.Cells[(int)eCol.Mode].Value = dataItem.Mode.ToString();
                        row.Cells[(int)eCol.Actual].Value = dataItem.Actual.ToString();
                        row.Cells[(int)eCol.Actual].Style.BackColor = bkGrnd;
                        row.Cells[(int)eCol.KeyName].Value = found ? dataItem.Key : ResultsExchange.NONE;
                        row.Cells[(int)eCol.KeyName].Tag = found;

                    }
                }
                catch (Exception ex)
                {
                    U.LogPopup(ex, "Problem updating the Results for '{0}'", _visionJob.Nickname);
                }

            }
            if (overallSuccess)
            {
                cbSuccess.Text = "Success";
            }
            else
            {
                cbSuccess.Text = "Fail";
            }
            cbSuccess.BackColor = bkGrnd;
            //Image image = _visionJob.LastJobImage;
            //if (image != null)
            //{
            //    lock (image)
            //    {
            //        AcquiredImage = new Bitmap(image);
            //    }
            //}
            btnRunJob.Enabled = AcquiredImage != null || _visionJob.LastObjJobImage != null;
            _ignoreSelection = false;
        }

        private void btnAcquisition_Click(object sender, EventArgs e)
        {
            _visionJob.ImageOwner = ImageOwner;
            if (Control.ModifierKeys != Keys.None)
            {
                OpenFileDialog ofd = new OpenFileDialog() { DefaultExt = "bmp", Multiselect = false, InitialDirectory = _visionJob.LastAcquisitionFileFolder };
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _visionJob.LastAcquisitionFileFolder = Path.GetDirectoryName(ofd.FileName);
                    try
                    {
                        Bitmap bm = Bitmap.FromFile(ofd.FileName) as Bitmap;
                        AcquiredImage = new Bitmap(bm);
                        _visionJob.AssignAcquireImageToWindows(bm);
                        bm.Dispose();
                    }
                    catch (Exception ex)
                    {
                        U.LogPopup(ex, "Problem loading bitmap from file.");
                        return;
                    }
                    btnRunJob.Enabled = AcquiredImage != null || _visionJob.LastObjJobImage != null;
                }
                return;
            }
            object oImage = _visionJob.TriggerForImage();
            if (oImage != null)
            {
                AcquiredImage = oImage;                
            }
            
            //    // Get from last Job
            //    Image image = _visionJob.LastJobImage;
            //    if (image != null)
            //    {
            //        lock (image)
            //        {
            //            AcquiredImage = new Bitmap(image);
            //        }
            //    }
            //}
            btnRunJob.Enabled = AcquiredImage != null;
        }

        private void btnRunJob_Click(object sender, EventArgs e)
        {
            EnableCommandInput(false);
            try
            {
                _visionJob.ImageOwner = ImageOwner;
                _visionJob.RunJob(AcquiredImage, false);
                UpdateData();
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "Run Job Error");

            }
            finally
            {
                EnableCommandInput(true);
            }
        }

        private void btnEditJob_Click(object sender, EventArgs e)
        {
            EnableCommandInput(false);
            try
            {
                _visionJob.EditVisionFile();
            }
            finally
            {
                EnableCommandInput(true);
            }
        }

        private void PopulateResult()
        {
            if (_visionJob.RefResults != null)
            {
                foreach (string resultKey in _visionJob.RefResults.Results.Keys)
                {
                    dgCogJobResult.Rows.Add(new object[] { resultKey, _visionJob.RefResults.Results[resultKey] });
                }
            }
        }


        private void EnableCommandInput(bool enabled)
        {
            if (enabled)
            {
                flpButtons.Enabled = true;
            }
            else
            {
                flpButtons.Enabled = false;
            }
        }

        public override string ToString()
        {
            return "Vision Job";
        }

        private void OnCurrentCellDirty(object sender, EventArgs e)
        {
            if (!_ignoreSelection && dgCogJobResult.CurrentCell != null)
            {
                eCol col = (eCol)dgCogJobResult.CurrentCell.ColumnIndex;
                int iRow = dgCogJobResult.CurrentCell.RowIndex;
                ExchangeDataMapBase dataMap = _visionJob.RefResults.dataMap[iRow];
                string editedValue = dgCogJobResult.CurrentCell.EditedFormattedValue.ToString();
                switch(col)
                {
                    case eCol.Min:
                        dataMap.MinString = editedValue;
                        break;
                    case eCol.Max:
                        dataMap.MaxString = editedValue;
                        break;
                    case eCol.Mode:
                        dataMap.Mode = (SpecBase.eMode)Enum.Parse(typeof(SpecBase.eMode), editedValue);
                        break;
                    case eCol.KeyName:
                        dataMap.Key = editedValue;
                        break;
                }
            }
        }

        private void OnDataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Exception != null)
            {
                if (e.Exception.Message != "DataGridViewComboBoxCell value is not valid.")
                {
                    U.LogPopup(e.Exception.Message);
                }
            }
        }

        private void OnRefreshClick(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void OnCellFormating(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == (int)eCol.KeyName)
            {
                DataGridViewCell cell = dgCogJobResult[e.ColumnIndex, e.RowIndex];
                if (!(bool)cell.Tag)
                {
                    e.CellStyle.BackColor = Color.Red;
                }
            }
        }


    }
}
