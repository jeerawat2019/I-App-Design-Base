using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MCore.Controls;
using Gigasoft.ProEssentials.Enums;

namespace MCore.Comp.XFunc
{
    public partial class Linear3DPage : UserControl, IComponentBinding<BasicLinear>
    {
        private BasicLinear _basicLinear = null;
        private const double NullDataValue = -99999.0;
        private enum eSubSet { TrainPointsSubset, FitLineSubset, NumSubSets }
        public Linear3DPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Define the name for this page
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "3D Graph";
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

        double[] _xTrainPoints = null;
        double[] _yTrainPoints = null;
        double[] _zTrainPoints = null;
        double _rowTolerance = 0;

        private int SortByY(int i1, int i2)
        {
            double dX1 = _xTrainPoints[i1];
            double dX2 = _xTrainPoints[i2];
            double dY1 = _yTrainPoints[i1];
            double dY2 = _yTrainPoints[i2];

            double distX = Math.Abs(dX2 - dX1);
            
            if (distX > _rowTolerance)
            {
                return dX1.CompareTo(dX2);
            }

            return dY1.CompareTo(dY2);

        }

        private int SortByX(int i1, int i2)
        {
            double dX1 = _xTrainPoints[i1];
            double dX2 = _xTrainPoints[i2];
            return dX1.CompareTo(dX2);
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
            graphXYZ.PeFunction.Reset();
            graphXYZ.PeString.MainTitle = _basicLinear.Name;
            graphXYZ.PeString.SubTitle = string.Empty;
            graphXYZ.PeString.YAxisLabel = "Z";
            graphXYZ.PeString.XAxisLabel = "X";
            graphXYZ.PeString.ZAxisLabel = "Y";
            graphXYZ.PeFont.MainTitle.Bold = true;
            graphXYZ.PeFont.SubTitle.Bold = true;
            graphXYZ.PeFont.Label.Bold = true;
            // Set various other properties //
            graphXYZ.PeColor.BitmapGradientMode = true;
            graphXYZ.PeColor.QuickStyle = QuickStyle.DarkLine;
            // Mechanism to control polygon border color //
            graphXYZ.PeColor.BarBorderColor = Color.FromArgb(255, 0, 0, 0);

            graphXYZ.PeColor.XZBackColor = Color.Empty;
            graphXYZ.PeColor.YBackColor = Color.Empty;
            graphXYZ.PeConfigure.PrepareImages = true;
            graphXYZ.PeConfigure.CacheBmp = true;
            graphXYZ.PeUserInterface.Allow.FocalRect = false;
            graphXYZ.PePlot.Option.ShadingStyle = ShadingStyle.White;
            graphXYZ.PePlot.Option.ShowBoundingBox = ShowBoundingBox.Never;

            graphXYZ.PeConfigure.TextShadows = TextShadows.BoldText;

            int numVars = _basicLinear.Variables.Count;
            if (numVars < 3)
                return;

            // Enable Double precision x and y data //
            graphXYZ.PeData.UsingXDataii = true;
            graphXYZ.PeData.UsingYDataii = true;
            graphXYZ.PeData.UsingZDataii = true;
            graphXYZ.PeData.NullDataValue = NullDataValue;
            graphXYZ.PeData.NullDataValueX = NullDataValue;
            graphXYZ.PeData.NullDataValueZ = NullDataValue;

            int maxPoints = _basicLinear.NumTrainPoints;

            _zTrainPoints = _basicLinear.Variables[0].TrainPoints;
            _xTrainPoints = _basicLinear.Variables[1].TrainPoints;
            _yTrainPoints = _basicLinear.Variables[2].TrainPoints;

            if (maxPoints > 0)
            {

                int nRows = 1;
                int nCols = maxPoints;
                bool matrix = false;
                // Don't consider using poly surface if less than 4 points.
                if (maxPoints >= 4)
                {
                    List<int> row = new List<int>();
                    double maxX = int.MinValue;
                    double maxY = int.MinValue;
                    double minX = int.MaxValue;
                    double minY = int.MaxValue;
                   
                    // Keep reference to original points
                    double[] zTrainPointsSource = _zTrainPoints;
                    double[] xTrainPointsSource = _xTrainPoints;
                    double[] yTrainPointsSource = _yTrainPoints;

                    // These points will be normalized between 0 to 1
                    _zTrainPoints = new double[maxPoints];
                    _xTrainPoints = new double[maxPoints];
                    _yTrainPoints = new double[maxPoints];
                    // First fine min, max, range of original values
                    for (int i = 0; i < maxPoints; i++)
                    {
                        _xTrainPoints[i] = xTrainPointsSource[i];
                        _yTrainPoints[i] = yTrainPointsSource[i];
                        _zTrainPoints[i] = zTrainPointsSource[i];
                        maxX = Math.Max(maxX, _xTrainPoints[i]);
                        maxY = Math.Max(maxY, _yTrainPoints[i]);
                        minX = Math.Min(minX, _xTrainPoints[i]);
                        minY = Math.Min(minY, _yTrainPoints[i]);
                        row.Add(i);
                    }
                    double rangeX = maxX - minX;
                    double rangeY = maxY - minY;
                    // Now normalize from 0 to 1
                    for (int i = 0; i < maxPoints; i++)
                    {
                        _xTrainPoints[i] = (_xTrainPoints[i] - minX) / rangeX;
                        _yTrainPoints[i] = (_yTrainPoints[i] - minY) / rangeY;
                    }
                    // Sort according to X or Y if X is the same
                    row.Sort(SortByX);

                    nRows = 1;
                    // Assume a row tolerance
                    _rowTolerance = 1.0 / Math.Sqrt(maxPoints) / 10.0;
                    // Count rows and columns
                    double rowCeiling = _rowTolerance;
                    nCols = 1;
                    int colCount = 0;
                    foreach (int i in row)
                    {
                        if (_xTrainPoints[i] > rowCeiling)
                        {
                            nRows++;
                            colCount = 0;
                            rowCeiling = _xTrainPoints[i] + _rowTolerance;
                        }
                        else
                        {
                            colCount++;
                            nCols = Math.Max(nCols, colCount);
                        }
                    }
                    // Include Y in sort according to _rowTolerance
                    row.Sort(SortByY);

                    // Copy original values in correct order
                    for (int i = 0; i < maxPoints; i++)
                    {
                        int n = row[i];
                        _zTrainPoints[i] = zTrainPointsSource[n];
                        _xTrainPoints[i] = xTrainPointsSource[n];
                        _yTrainPoints[i] = yTrainPointsSource[n];
                    }
                    // Only show surface if grid is full with no left overs
                    if (nRows > 1 && nCols > 1 && (nCols * nRows) == maxPoints)
                    {
                        matrix = true;
                    }
                }


                if (matrix)
                {
                    // Poly Surface plot
                    graphXYZ.PeData.Subsets = nRows;
                    graphXYZ.PeData.Points = nCols;
                    graphXYZ.PePlot.PolyMode = PolyMode.SurfacePolygons;
                    graphXYZ.PePlot.Method = ThreeDGraphPlottingMethod.One;
                    graphXYZ.PeColor.SubsetColors[(int)(SurfaceColors.WireFrame)] = Color.FromArgb(96, 0, 148, 0);
                    graphXYZ.PeColor.SubsetColors[(int)(SurfaceColors.SolidSurface)] = Color.FromArgb(96, 0, 148, 0);
                }
                else
                {
                    // Scatter plot
                    graphXYZ.PeData.Subsets = 1;
                    graphXYZ.PeData.Points = maxPoints;
                    graphXYZ.PePlot.PolyMode = PolyMode.Scatter;
                    graphXYZ.PePlot.SubsetPointTypes[0] = PointType.Square;
                    graphXYZ.PeColor.SubsetColors[0] = Color.FromArgb(198, 198, 0);
                    graphXYZ.PePlot.Method = ThreeDGraphPlottingMethod.Zero; // Points
                    graphXYZ.PePlot.PolyMode = PolyMode.Scatter;
                    graphXYZ.PeColor.SubsetColors[(int)(SurfaceColors.WireFrame)] = Color.FromArgb(96, 0, 148, 0);
                    graphXYZ.PeColor.SubsetColors[(int)(SurfaceColors.SolidSurface)] = Color.FromArgb(96, 0, 148, 0);
                }

                try
                {
                    Gigasoft.ProEssentials.Api.PEvsetW(graphXYZ.PeSpecial.HObject, Gigasoft.ProEssentials.DllProperties.XDataII, _xTrainPoints, maxPoints);
                    Gigasoft.ProEssentials.Api.PEvsetW(graphXYZ.PeSpecial.HObject, Gigasoft.ProEssentials.DllProperties.YDataII, _zTrainPoints, maxPoints);
                    Gigasoft.ProEssentials.Api.PEvsetW(graphXYZ.PeSpecial.HObject, Gigasoft.ProEssentials.DllProperties.ZDataII, _yTrainPoints, maxPoints);

                    double yCoef = _basicLinear.Variables[0].Coef;
                    double xCoef = _basicLinear.Variables[1].Coef;
                    double zCoef = _basicLinear.Variables[2].Coef;
                    graphXYZ.PeString.SubTitle = string.Format("{0} = {1} [{2}] + {3} [{4}] + {5}",
                        _basicLinear.Variables[0].Name,
                        xCoef.ToString("0.######"), _basicLinear.Variables[1].Name,
                        zCoef.ToString("0.######"), _basicLinear.Variables[2].Name,
                        yCoef.ToString("0.######"));

                    graphXYZ.PeUserInterface.Scrollbar.MouseDraggingX = true;
                    graphXYZ.PeUserInterface.Scrollbar.MouseDraggingY = true;

                    // Improves Metafile Export //
                    graphXYZ.PeSpecial.DpiX = 600;
                    graphXYZ.PeSpecial.DpiY = 600;

                    graphXYZ.PeConfigure.RenderEngine = RenderEngine.GdiPlus;
                    graphXYZ.PeConfigure.AntiAliasGraphics = true;
                    graphXYZ.PeConfigure.AntiAliasText = true; 
                    graphXYZ.PeString.YAxisLabel = _basicLinear.Variables[0].Name;
                    graphXYZ.PeString.XAxisLabel = _basicLinear.Variables[1].Name;
                    graphXYZ.PeString.ZAxisLabel = _basicLinear.Variables[2].Name;

                }
                catch (Exception ex)
                {
                    U.LogError(ex, "Problem with Linear3DPage graph generation of '{0}'", _basicLinear.Nickname);
                }
            }
            graphXYZ.Refresh();
        }
    }
}
