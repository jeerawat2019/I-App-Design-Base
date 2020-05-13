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
    public partial class QueryTerminal : UserControl, IComponentBinding<MySQLComm>
    {

        private MySQLComm _mySQLComm = null;

        public QueryTerminal()
        {
            InitializeComponent();
        }


        public override string ToString()
        {
            return "Query Terminal";
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
                strQueryCommand.BindTwoWay(() => _mySQLComm.DbLastQueryCommand);
            }
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            if(strQueryCommand.Text == "")
            {
                return;
            }

            try
            {
                DataTableCollection dtResults = _mySQLComm.QueryDb(strQueryCommand.Text);
              dgQueryResult.DataSource = dtResults[0];
            }
            catch(Exception ex)
            {
                U.LogAlarmPopup(ex, "MySQL Query Database Error", null);
                return;
            }

            MessageBox.Show(String.Format("MySQL Query Database Completed"));
        }
    }
}
