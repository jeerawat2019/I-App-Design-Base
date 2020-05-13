using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MCore.Controls;
//using Gigasoft.ProEssentials;
using Gigasoft.ProEssentials.Enums;

namespace MCore.Comp.XFunc
{
    public partial class Linear2DPage : UserControl, IComponentBinding<BasicLinear>
    {
        private BasicLinear _basicLinear = null;
        private enum eSubSet { TrainPointsSubset, FitLineSubset, NumSubSets }
        private const double NullDataValue = -99999.0;

        public Linear2DPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Define the name for this page
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "2D Graph";
        }
        /// <summary>
        /// Bind 
        /// </summary>
        /// <param name="axis"></param>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BasicLinear Bind
        {
            get { return _basicLinear; }
            set
            {
                _basicLinear = value;
                _basicLinear.OnUpdateGraph += new MethodInvoker(RefreshGraph);
                RefreshGraph();
            }
        }
        void OnVisibleChanged(object sender, System.EventArgs e)
        {
            RefreshGraph();
        }
        private void RefreshGraph()
        {
            if (!Visible || _basicLinear == null)
            {
                return;
            }
            graphXY.PeFunction.Reset();

            int numVars = _basicLinear.Variables.Count;
            if (numVars < 2)
                return;

            if (Height >= 150)
            {
                graphXY.PeString.MainTitle = _basicLinear.Name;
            }
            else
            {
                graphXY.PeString.MainTitle = string.Empty;
            }

            graphXY.PeData.Subsets = (int)eSubSet.NumSubSets* (numVars -1);
            // Enable Double precision x and y data //
            graphXY.PeData.UsingXDataii = true;
            graphXY.PeData.UsingYDataii = true;
            graphXY.PeData.NullDataValueX = NullDataValue;
            graphXY.PeData.NullDataValue = NullDataValue;
            double yCoef = _basicLinear.Variables[0].Coef;
            double xCoef = 0;
            for (int nVar = 1; nVar < numVars; nVar++)
            {
                xCoef = _basicLinear.Variables[nVar].Coef;
                int nSubOff = (nVar-1) * 2; 
                // Train Points
                if (_basicLinear.Variables[nVar].IsIndex)
                {
                    graphXY.PePlot.Methods[nSubOff + (int)eSubSet.TrainPointsSubset] = SGraphPlottingMethods.Line;
                    graphXY.PeString.SubsetLabels[nSubOff + (int)eSubSet.TrainPointsSubset] = string.Format("{0} Data Points", _basicLinear.Variables[nVar].Name);
                    graphXY.PePlot.Methods[nSubOff + (int)eSubSet.FitLineSubset] = SGraphPlottingMethods.Line;
                    graphXY.PePlot.SubsetLineTypes[nSubOff + (int)eSubSet.FitLineSubset] = LineType.ThinSolid;
                }
                else
                {
                    graphXY.PePlot.Methods[nSubOff + (int)eSubSet.TrainPointsSubset] = SGraphPlottingMethods.Point;
                    graphXY.PePlot.SubsetPointTypes[nSubOff + (int)eSubSet.TrainPointsSubset] = PointType.DotSolid;
                    graphXY.PeString.SubsetLabels[nSubOff + (int)eSubSet.TrainPointsSubset] = string.Format("{0} Train Points", _basicLinear.Variables[nVar].Name);
                    // Fit Line
                    graphXY.PePlot.Methods[nSubOff + (int)eSubSet.FitLineSubset] = SGraphPlottingMethods.Line;
                    graphXY.PePlot.SubsetLineTypes[nSubOff + (int)eSubSet.FitLineSubset] = LineType.Dot;
                }
                graphXY.PeString.SubsetLabels[nSubOff + (int)eSubSet.FitLineSubset] = string.Format("{0} = {1} [{2}] + {3}",
                    _basicLinear.Variables[0].Name, xCoef.ToString("0.######"), _basicLinear.Variables[nVar].Name, yCoef.ToString("0.######"));
            }


            double[] yTrainPoints = _basicLinear.Variables[0].TrainPoints;
            double[] xTrainPoints = null;
            graphXY.PeString.YAxisLabel = _basicLinear.Variables[0].Name;
            graphXY.PeString.XAxisLabel = GetUnits(_basicLinear.Variables[1].Name);
            graphXY.PeString.SubTitle = string.Empty;

            int maxPoints = _basicLinear.NumTrainPoints;
            if (maxPoints < 2)
                return;

            graphXY.PeData.Points = maxPoints;


            double x = NullDataValue;
            double y = NullDataValue;

            for (int nVar = 1; nVar < numVars; nVar++)
            {
                xCoef = _basicLinear.Variables[nVar].Coef;
                int nSubOff = (nVar - 1) * 2;
                xTrainPoints = _basicLinear.Variables[nVar].TrainPoints;
                for (int i = 0; i < maxPoints; i++)
                {
                    x = NullDataValue;
                    y = NullDataValue;
                    if (xTrainPoints != null)
                    {
                        x = xTrainPoints[i];
                        y = yTrainPoints[i];
                    }
                    graphXY.PeData.Xii[nSubOff + (int)eSubSet.TrainPointsSubset, i] = x;
                    graphXY.PeData.Yii[nSubOff + (int)eSubSet.TrainPointsSubset, i] = y;

                    x = NullDataValue;
                    y = NullDataValue;
                    if (xTrainPoints != null)
                    {
                        x = xTrainPoints[i];
                        y = x * xCoef + yCoef;
                    }

                    graphXY.PeData.Xii[nSubOff + (int)eSubSet.FitLineSubset, i] = x;
                    graphXY.PeData.Yii[nSubOff + (int)eSubSet.FitLineSubset, i] = y;
                }
            }
        }
        private string GetUnits(string text)
        {
            string[] split = text.Split('(', ')');
            if (split.Length > 1)
            {
                return split[1];
            }
            return text;
        }
    }
}
