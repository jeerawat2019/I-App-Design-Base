using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MCore.Controls;

namespace MCore.Comp.DBCommunications
{
    public partial class ConstructTerminal : UserControl, IComponentBinding<MySQLComm>
    {

        private MySQLComm _mySQLComm = null;

        public ConstructTerminal()
        {
            InitializeComponent();
        }


        public override string ToString()
        {
            return "Construct Terminal";
        }

        /// <summary>
        /// Bind 
        /// </summary>
        /// <param name="axis"></param>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MySQLComm Bind
        {
            get { return _mySQLComm; }
            set 
            { 
                _mySQLComm = value;              
                strCreate.BindTwoWay(() => _mySQLComm.DbCreateCommand);
                strConstruct.BindTwoWay(() => _mySQLComm.DbContructCommand);
                strUpdate.BindTwoWay(() => _mySQLComm.DbUpgradeCommand);
            }
        }

        private void btnCreateExecute_Click(object sender, EventArgs e)
        {
            try
            {
                _mySQLComm.CreateDb();
            }
            catch(Exception ex)
            {
                U.LogAlarmPopup(ex, "MySQL Create Database Error", null);
                return;
            }

            MessageBox.Show(String.Format("MySQL Create Database {0} Completed",_mySQLComm.DbName));
        }

        private void btnConstructExecute_Click(object sender, EventArgs e)
        {
            try
            {
                _mySQLComm.ConstructDb();
            }
            catch (Exception ex)
            {
                U.LogAlarmPopup(ex, "MySQL Construction Database Error", null);
                return;
            }

            MessageBox.Show(String.Format("MySQL Construction Database {0} Completed", _mySQLComm.DbName));
        }

        private void btnUpdateExecute_Click(object sender, EventArgs e)
        {
            try
            {
                _mySQLComm.UpgradeDb();
            }
            catch (Exception ex)
            {
                U.LogAlarmPopup(ex, "MySQL Update Database Error", null);
                return;
            }

            MessageBox.Show(String.Format("MySQL Update Database {0} Completed", _mySQLComm.DbName));
        }

    }
}
