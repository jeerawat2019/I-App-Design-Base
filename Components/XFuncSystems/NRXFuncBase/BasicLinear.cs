using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using System.Drawing;

namespace MCore.Comp.XFunc
{
    public class BasicLinear : LinearBase
    {

        private List<XFerVariable> _variables = new List<XFerVariable>();
        private int _maxTrainPoints = 100;
        private int _index = 0;
        private double _outlierMargin = double.MaxValue;
        private double _outlierMin = double.MinValue;
        private double _outlierMax = double.MaxValue;
        public const string XFUNCINDEX = "Index";

        /// <summary>
        /// The variables (including Result which is first)
        /// </summary>
        [Browsable(false)]
        [XmlElement("Variables")]
        public XFerVariable[] DefineVariables
        {
            get
            {
                return _variables.ToArray();
            }
            set
            {
                lock (_variables)
                {
                    _variables.Clear();
                    _variables.AddRange(value);
                }
            }
        }

        /// <summary>
        /// The variables (including Result which is first)
        /// </summary>
        [Category("XFunc")]
        [XmlIgnore]
        public bool Valid
        {
            get { return NumTrainPoints >= 2; }
        }
        /// <summary>
        /// The variables (including Result which is first)
        /// </summary>
        [Category("XFunc")]
        [XmlIgnore]
        public List<XFerVariable> Variables
        {
            get
            {
                return _variables;
            }
        }

        /// <summary>
        /// Retrieve or replace the Data
        /// </summary>
        [XmlIgnore]
        public override DataTable Data
        {
            get
            {
                DataTable dt = new DataTable();
                lock (_variables)
                {
                    int nVars = _variables.Count;
                    for (int iVar = 0; iVar < nVars; iVar++)
                    {
                        DataColumn dc = new DataColumn(_variables[iVar].Name, typeof(double));
                        dt.Columns.Add(dc);
                    }
                    dt.Columns.Add("Error", typeof(double));

                    for (int i = 0; i < numTrainPoints; i++)
                    {
                        double[] dRow = new double[nVars + 1];
                        for (int iVar = 0; iVar < nVars; iVar++)
                        {
                            dRow[iVar] = Variables[iVar].TrainPoints[i];
                        }
                        dRow[nVars] = GetError(dRow);
                        object[] object_array = new object[dRow.Length];
                        dRow.CopyTo(object_array, 0);
                        dt.LoadDataRow(object_array, true);
                    }
                }
                return dt;
            }
            set
            {
                value.AcceptChanges();
                lock (_variables)
                {
                    int nVars = _variables.Count;
                    for (int iVar = 0; iVar < nVars; iVar++)
                    {
                        double[] dTrainPts = new double[value.Rows.Count];
                        for (int i = 0; i < value.Rows.Count; i++)
                        {
                            dTrainPts[i] = (double)value.Rows[i][iVar];
                        }
                        _variables[iVar].TrainPoints = dTrainPts;
                    }
                }
            }
        }
        /// <summary>
        /// The variables (including Result which is first)
        /// </summary>
        [Category("XFunc")]
        [XmlIgnore]
        public bool ResetMap
        {
            get { return false; }
            set
            {
                if (value)
                {
                    Reset();
                }
            }
        }

        /// <summary>
        /// Get the number of train points
        /// </summary>
        [XmlIgnore]
        protected int numTrainPoints
        {
            get
            {
                if (_variables.Count > 0)
                {
                    return _variables[0].NumTrainPoints;
                }
                return 0;
            }
        }

        /// <summary>
        /// Get the number of train points
        /// </summary>
        [Category("XFunc")]
        [XmlIgnore]
        public int NumTrainPoints
        {
            get
            {
                lock (_variables)
                {
                    return numTrainPoints;
                }
            }
        }

        /// <summary>
        /// The Maximum number of training points
        /// </summary>
        [Category("XFunc")]
        [Description("The Maximum number of training points")]
        public int MaxTrainPoints
        {
            get { return _maxTrainPoints; }
            set { _maxTrainPoints = value; }
        }

        /// <summary>
        /// The index for the Teach item
        /// </summary>
        /// <remarks>Only used if a column is marked "Index"</remarks>
        [Category("XFunc")]
        [Description("The index for the Teach item")]
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }

        /// <summary>
        /// The Outlier margin
        /// </summary>
        [Category("XFunc")]
        [Description("The Outlier margin")]
        public double OutlierMargin
        {
            get { return _outlierMargin; }
            set { _outlierMargin = value; }
        }

        /// <summary>
        /// The Outlier minimum
        /// </summary>
        [Category("XFunc")]
        [Description("The Outlier Minimum")]
        public double OutlierMin
        {
            get { return _outlierMin; }
            set { _outlierMin = value; }
        }

        /// <summary>
        /// The Outlier Maximum
        /// </summary>
        [Category("XFunc")]
        [Description("The Outlier Maximum")]
        public double OutlierMax
        {
            get { return _outlierMax; }
            set { _outlierMax = value; }
        }

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public BasicLinear()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public BasicLinear(string name)
            : base(name)
        {
        }

        #endregion Constructors

        public override void Initialize()
        {
            base.Initialize();
            Regression();

        }


        /// <summary>
        /// Get the Error of the point to the regresion model
        /// </summary>
        /// <param name="vars"></param>
        /// <returns></returns>
        public double GetError(double[] vars)
        {
            return Math.Abs(Evaluate(vars) - vars[0]);
        }

        /// <summary>
        /// Teach the points if not outlier
        /// </summary>
        /// <param name="variableVals"></param>
        /// <returns>True if the point passed outlier test</returns>
        public override void Teach(params double[][] variableVals)
        {
            int nVarsInTeach = variableVals.GetLength(0);
            int nTrainsInTeach = variableVals[0].GetLength(0);
            for (int iVar = 0; iVar < Math.Min(nVarsInTeach, Variables.Count); iVar++)
            {
                for (int i = 0; i < nTrainsInTeach; i++)
                {
                    if (Variables[iVar].IsIndex)
                    {
                        variableVals[iVar][i] = (double)_index+i;
                    }
                    Variables[iVar].AddTrainPoint(variableVals[iVar][i]);
                }                
            }
            _index = numTrainPoints;

            // Regress

            Regression();

            if (_index <= MaxTrainPoints)
            {
                return;
            }
            
            // Get errors

            double[] error = new double[_index];
            double[] errorSorted = new double[_index];
            List<double[]> dVals = new List<double[]>();
            for(int iVar=0; iVar < nVarsInTeach; iVar++)
            {
                dVals.Add(Variables[iVar].TrainPoints);
            }
            for (int i = 0; i < _index; i++)
            {
                double[] dSingleVal = new double[nVarsInTeach];
                for(int iVar=0; iVar < nVarsInTeach; iVar++)
                {
                    dSingleVal[iVar] = dVals[iVar][i];
                }
                error[i] = GetError(dSingleVal);
                errorSorted[i] = error[i];
            }

            Array.Sort(errorSorted);

            double cutoff = errorSorted[MaxTrainPoints];
            for (int iVar = 0; iVar < nVarsInTeach; iVar++)
            {
                Variables[iVar].ClearTraining();
            }
            for (int i = 0; i < _index; i++)
            {
                if (error[i] < cutoff)
                {
                    for (int iVar = 0; iVar < nVarsInTeach; iVar++)
                    {
                        Variables[iVar].AddTrainPoint(dVals[iVar][i]);
                    }
                }
            }
            _index = numTrainPoints;
            Regression();

            // Test
            error = new double[_index];
            errorSorted = new double[_index];
            dVals = new List<double[]>();
            for(int iVar=0; iVar < nVarsInTeach; iVar++)
            {
                dVals.Add(Variables[iVar].TrainPoints);
            }
            for (int i = 0; i < _index; i++)
            {
                double[] dSingleVal = new double[nVarsInTeach];
                for(int iVar=0; iVar < nVarsInTeach; iVar++)
                {
                    dSingleVal[iVar] = dVals[iVar][i];
                }
                error[i] = GetError(dSingleVal);
                errorSorted[i] = error[i];
            }
            Array.Sort(errorSorted);

        }
        /// <summary>
        /// Teach the points if not outlier
        /// </summary>
        /// <param name="variableVals"></param>
        /// <returns>True if the point passed outlier test</returns>
        public override bool Teach(params double[] variableVals)
        {
            // Replace with index if appropriate

            for (int i = 0; i < Math.Min(variableVals.Length, Variables.Count); i++)
            {
                if (Variables[i].IsIndex)
                {
                    variableVals[i] = (double)_index;
                }
            }

            if (OutlierMargin != 0.0 && numTrainPoints > MaxTrainPoints / 2)
            {
                double diff = GetError(variableVals);
                if (diff > OutlierMargin)
                {
                    Debug.WriteLine(string.Format("{0} outlier rejected = {1}", Name, diff)); 
                    return false;
                }
            }
            for (int i = 0; i < Math.Min(variableVals.Length, Variables.Count); i++)
            {
                Variables[i].AddTrainPoint(variableVals[i], MaxTrainPoints);
            }
            Regression();
            _index++;
            
            return true;
        }

        /// <summary>
        /// Calculate the new point based on inputs
        /// </summary>
        /// <param name="vars"></param>
        /// <returns></returns>
        public override double Evaluate(params double[] vars)
        {
            double offset = base.Evaluate(null);
            double result = Variables[0].Coef;
            int MA = Variables.Count();
            int iInput = vars.Length >= MA ? 1 : 0;
            for (int i = 1; i < MA; i++)
            {
                result += Variables[i].Calculate(vars[iInput++]);
            }
            return result + offset;
        }
        /// <summary>
        /// Reset the component
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            foreach (XFerVariable variable in DefineVariables)
            {
                variable.Coef = 1.0;
                variable.ClearTraining();
            }
            Variables[0].Coef = 0;
            _index = 0;
            UpdateGraph();

        }

        /// <summary>
        /// Find the Minimum position
        /// </summary>
        /// <returns>Returns the minimum point.</returns>
        private double[] FindMinMax(bool bMax)
        {
            int MA = Variables.Count();
            double[] dPt = new double[MA - 1];
            double[] dVals = Variables[0].TrainPoints;
            int iMinMax = 0;
            if (bMax)
            {
                double dMaxVal = double.MinValue;
                for(int i = 0; i<dVals.Length; i++)
                {
                    if (dVals[i] > dMaxVal)
                    {
                        dMaxVal = dVals[i];
                        iMinMax = i;
                    }
                }
            }
            else
            {
                double dMinVal = double.MaxValue;
                for (int i = 0; i < dVals.Length; i++)
                {
                    if (dVals[i] < dMinVal)
                    {
                        dMinVal = dVals[i];
                        iMinMax = i;
                    }
                }
            }
            for (int n = 1; n < MA; n++)
            {
                dPt[n - 1] = Variables[n].TrainPoints[iMinMax];
            }
            return dPt;
        }
        
        /// <summary>
        /// Find the Minimum position
        /// </summary>
        /// <returns>Returns the minimum point.</returns>
        public override double[] FindMin()
        {
            return FindMinMax(false);
        }
        /// <summary>
        /// Find the Maximum position
        /// </summary>
        /// <returns>Returns the maximum point.</returns>
        public override double[] FindMax()
        {
            return FindMinMax(true);
        }
        /// <summary>
        /// Compute the result based on inputs and defining points
        /// </summary>
        protected void Regression()
        {
            lock (_variables)
            {
                int MA = _variables.Count();
                if (MA < 2)
                    return;
                int count = _variables[0].TrainPoints.Count();
                if (count < MA)
                    return;

                try
                {
                    double[] coef = new double[MA];

                    double[,] inputs = new double[MA - 1, count];
                    double[] responses = _variables[0].TrainPoints;

                    for (int i = 1; i < MA; i++)
                    {
                        double[] input = _variables[i].TrainPoints;
                        for (int n = 0; n < count; n++)
                        {
                            inputs[i - 1, n] = input[n];
                        }
                    }

                    MatrixMath.LinearRegression(responses, inputs, ref coef);

                    for (int i = 0; i < MA; i++)
                    {
                        _variables[i].Coef = coef[i];
                    }

                }
                catch (Exception ex)
                {
                    U.LogError(ex, "Linear Regression Fit error {0}", Name);
                }
            }
        }

    }
    /// <summary>
    /// Class to contain the train values
    /// </summary>
    public class XFerVariable
    {
        private string _name = string.Empty;
        private Queue<double> _trainPoints = new Queue<double>();
        private double _coef = 0.0;

        /// <summary>
        /// Input Name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Returns true if the variable is an index
        /// </summary>
        public bool IsIndex
        {
            get { return _name == BasicLinear.XFUNCINDEX; }
        }
        /// <summary>
        /// Returns the number of train points
        /// </summary>
        public int NumTrainPoints
        {
            get { return _trainPoints.Count; }
        }

        /// <summary>
        /// Coefficient
        /// </summary>
        public double Coef
        {
            get { return _coef; }
            set { _coef = value; }
        }

        /// <summary>
        /// The trained points
        /// </summary>
        public double[] TrainPoints
        {
            get
            {
                return _trainPoints.ToArray();
            }
            set
            {
                _trainPoints.Clear();
                foreach (double d in value)
                {
                    _trainPoints.Enqueue(d);
                }
            }
        }
        /// <summary>
        /// Serialization constructor
        /// </summary>
        public XFerVariable()
        {
        }
        /// <summary>
        /// Manual constructor
        /// </summary>
        public XFerVariable(string name, double defaultCoeff)
        {
            _name = name;
            _coef = defaultCoeff;
        }
        public void AddTrainPoint(double d, int maxTrainPoints)
        {
            _trainPoints.Enqueue(d);
            while (_trainPoints.Count > maxTrainPoints)
            {
                _trainPoints.Dequeue();
            }
        }
        public void AddTrainPoint(double d)
        {
            _trainPoints.Enqueue(d);
        }
        public double Calculate(double d)
        {
            return Coef * d;
        }

        public void ClearTraining()
        {
            _trainPoints.Clear();
        }
    }
}
