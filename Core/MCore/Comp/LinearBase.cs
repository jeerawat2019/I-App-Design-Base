using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace MCore.Comp.XFunc
{
    public class LinearBase : CompBase
    {
        #region Public Browsable Properties

        /// <summary>
        /// An optional offset to be applied to the evaluated result
        /// </summary>
        [Browsable(true)]
        [Category("Linear")]
        [Description("An optional offset to be applied to the evaluated result")]
        public double Offset
        {
            get { return GetPropValue(() => Offset); }
            set { SetPropValue(() => Offset, value); }
        }


        #endregion Public Browsable Properties

        #region Public Non-Browsable Properties

        public event MethodInvoker OnUpdateGraph = null;

        /// <summary>
        /// Retrieve or replace the Data
        /// </summary>
        [XmlIgnore]
        public virtual DataTable Data
        {
            get
            {
                return new DataTable();
            }
            set
            {

            }
        }

        #endregion Public Non-Browsable Properties


        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public LinearBase()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public LinearBase(string name)
            : base(name)
        {            
        }

        #endregion Constructors

        #region override functions

        /// <summary>
        /// Reset the component
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            Offset = 0;
            U.LogInfo("{0}Offset has been reset", Nickname);
        }

        #endregion override functions


        #region virtual functions

        /// <summary>
        /// Teach the points
        /// </summary>
        /// <param name="variableVals"></param>
        /// <returns>True if the point passed outlier test</returns>
        public virtual bool Teach(params double[] variableVals)
        {
            return false;
        }

        /// <summary>
        /// Teach the points
        /// </summary>
        /// <param name="variableVals"></param>
        /// <returns>True if the point passed outlier test</returns>
        public virtual void Teach(params double[][] variableVals)
        {
        }

        /// <summary>
        /// Calculate the new point based on inputs
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public virtual double Evaluate(params double[] inputs)
        {
            return Offset;
        }

        /// <summary>
        /// Update the graph(s)
        /// </summary>
        public virtual void UpdateGraph()
        {
            if (OnUpdateGraph != null)
            {
                OnUpdateGraph();
            }
        }

        /// <summary>
        /// Find the Minimum position
        /// </summary>
        /// <returns>Returns the minimum point.</returns>
        public virtual double[] FindMin()
        {
            return null;
        }
        /// <summary>
        /// Find the Maximum position
        /// </summary>
        /// <returns>Returns the maximum point.</returns>
        public virtual double[] FindMax()
        {
            return null;
        }

        #endregion virtual functions

    }
}
