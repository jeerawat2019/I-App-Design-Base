using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Xml.Serialization;
using MySql.Data.MySqlClient;
using System.Data;

using MCore;

namespace MCore.Comp.DBCommunications
{
    public class MySQLComm : DBCommBase
    {


        #region Private Member

        private MySqlConnection _sqlConn = null;

        #endregion Private Member

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public MySQLComm()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public MySQLComm(string name)
            : base(name)
        {
        }
        #endregion Constructors


        /// <summary>
        /// Initialize this component
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            _sqlConn = new MySqlConnection();
        }


        /// <summary>
        /// Destroy the object
        /// </summary>
        public override void Destroy()
        {
            base.Destroy();
        }

        [StateMachineEnabled]
        public override void ConnectDb()
        {
            _sqlConn.Close();
            BuildDbConnectionString();
            _sqlConn.ConnectionString = DbConnString;
            _sqlConn.Open();
        }

        [StateMachineEnabled]
        public override void DisconnectDb()
        {
            _sqlConn.Close();
        }

        [StateMachineEnabled]
        public override void BuildInitConnectionString()
        {
            DbInitConnString = "SERVER=" + ServerName + ";" + "UID=" + DbUser + ";" + "PASSWORD=" + DbPassword + ";" + "DEFAULT COMMAND TIMEOUT=600" + ";" + "CONVERT ZERO DATETIME=True" + ";";
        }


        [StateMachineEnabled]
        public override void BuildDbConnectionString()
        {
            DbConnString = "SERVER=" + ServerName + ";" + "DATABASE=" + DbName + ";" + "UID=" + DbUser + ";" + "PASSWORD=" + DbPassword + ";" + "DEFAULT COMMAND TIMEOUT=600" + ";" + "CONVERT ZERO DATETIME=True" + ";";
        }

        [StateMachineEnabled]
        public override void CreateDb()
        {
            if (DbCreateCommand == "")
            {
                return;
            }

            _sqlConn.Close();
            BuildInitConnectionString();
            _sqlConn.ConnectionString = DbInitConnString;
            _sqlConn.Open();
            MySqlCommand cmd = new MySqlCommand(DbCreateCommand, _sqlConn);
            cmd.ExecuteNonQuery();
            _sqlConn.Close();
           
        }

        [StateMachineEnabled]
        public override void ConstructDb()
        {
            if(DbContructCommand == "")
            {
                return;
            }

            _sqlConn.Close();
            BuildDbConnectionString();
            _sqlConn.ConnectionString = DbConnString;
            _sqlConn.Open();
            MySqlCommand cmd = new MySqlCommand(DbContructCommand, _sqlConn);
            cmd.ExecuteNonQuery();
            _sqlConn.Close();
        }
 
        public override void UpgradeDb()
        {
            if (DbUpgradeCommand == "")
            {
                return;
            }
            _sqlConn.Close();
            BuildDbConnectionString();
            _sqlConn.ConnectionString = DbConnString;
            _sqlConn.Open();
            MySqlCommand cmd = new MySqlCommand(DbUpgradeCommand, _sqlConn);
            cmd.ExecuteNonQuery();
            _sqlConn.Close();
        }

        public override System.Data.DataTableCollection QueryDb(string QueryCommand)
        {
            if (QueryCommand == "")
            {
                return null;
            }

            _sqlConn.Close();
            BuildDbConnectionString();
            _sqlConn.ConnectionString = DbConnString;
            _sqlConn.Open();

            MySqlDataAdapter da = new MySqlDataAdapter(QueryCommand,_sqlConn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            _sqlConn.Close();
            return ds.Tables;
        }

        public override void ExecuteNoneQuery(string Command)
        {
            if (Command == "")
            {
                return;
            }
            _sqlConn.Close();
            BuildDbConnectionString();
            _sqlConn.ConnectionString = DbConnString;
            _sqlConn.Open();
            MySqlCommand cmd = new MySqlCommand(Command, _sqlConn);
            cmd.ExecuteNonQuery();
            _sqlConn.Close();
        }
    }
         
}

        
        



