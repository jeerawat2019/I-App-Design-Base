using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;

namespace MCore.Comp.DBCommunications
{
    public partial class DBCommBase : CompBase
    {


        #region Property
        /// <summary>
        /// Get the Server Name
        /// </summary>
        /// <returns></returns>
        [Browsable(true)]
        [Category("Server")]
        public string ServerName
        {
            get { return GetPropValue(() => ServerName, "127.0.0.1"); }
            set { SetPropValue(() => ServerName, value); }
        }


        [Browsable(true)]
        [Category("Server")]
        public string DbName
        {
            get { return GetPropValue(() => DbName, "localdb"); }
            set { SetPropValue(() => DbName, value); }
        }


        [Browsable(true)]
        [Category("Server")]
        public string DbUser
        {
            get { return GetPropValue(() => DbUser, "root"); }
            set { SetPropValue(() => DbUser, value); }
        }


        [Browsable(true)]
        [Category("Server")]
        public string DbPassword
        {
            get { return GetPropValue(() => DbPassword, "12345"); }
            set { SetPropValue(() => DbPassword, value); }
        }

        [Browsable(true)]
        [Category("Server")]
        public string DbInitConnString
        {
            get { return GetPropValue(() => DbInitConnString); }
            set { SetPropValue(() => DbInitConnString, value); }
        }

        [Browsable(true)]
        [Category("Server")]
        public string DbConnString
        {
            get { return GetPropValue(() => DbConnString); }
            set { SetPropValue(() => DbConnString, value); }
        }

        [Browsable(true)]
        [Category("Server")]
        public string DbCreateCommand
        {
            get { return GetPropValue(() => DbCreateCommand); }
            set { SetPropValue(() => DbCreateCommand, value); }
        }

        [Browsable(true)]
        [Category("Server")]
        public string DbContructCommand
        {
            get { return GetPropValue(() => DbContructCommand); }
            set { SetPropValue(() => DbContructCommand, value); }
        }

        [Browsable(true)]
        [Category("Server")]
        public string DbUpgradeCommand
        {
            get { return GetPropValue(() => DbUpgradeCommand); }
            set { SetPropValue(() => DbUpgradeCommand, value); }
        }


        [Browsable(true)]
        [Category("Server")]
        public string DbLastQueryCommand
        {
            get { return GetPropValue(() => DbLastQueryCommand); }
            set { SetPropValue(() => DbLastQueryCommand, value); }
        }

        #endregion Property



         #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public DBCommBase()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="childName"></param>
        public DBCommBase(string name) 
            : base (name)
        {
        }
        #endregion


        public virtual void ConnectDb()
        {
        }

        public virtual void DisconnectDb()
        {
        }

        public virtual void CreateDb()
        {
        }

        public virtual void ConstructDb()
        {
        }


        public virtual void UpgradeDb()
        {
        }

        public virtual void BuildInitConnectionString()
        {
        }

        public virtual void BuildDbConnectionString()
        {
        }


        public virtual DataTableCollection QueryDb(string QueryCommand)
        {
            return null;
        }

        public virtual void ExecuteNoneQuery(string Command)
        {
            
        }
    }
}
