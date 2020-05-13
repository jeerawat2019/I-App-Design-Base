using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MCore.Controls;

namespace MCore.Comp.IOSystem.Input
{
    public partial class Array2DInputCtl : UserControl, IComponentBinding<Array2DInput>
    {
        private Array2DInput _arr2D = null;
        public Array2DInputCtl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Bind to the Inputs component
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Array2DInput Bind
        {
            get { return _arr2D; }
            set
            {
                _arr2D = value;
                if (_arr2D.NeedsInstantiation)
                {
                    _arr2D.Instantiate();
                }
                UploadCoarse();
                UploadFine();
            }
        }
        /// <summary>
        /// Set the name for this control
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Array2D";
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog() { InitialDirectory = @"D:\Astra", DefaultExt=".csv" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                _arr2D.ReadFromFile(ofd.FileName);
                RefreshImage();
            }
        }
        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog() { InitialDirectory = @"D:\Astra", DefaultExt=".csv" };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                _arr2D.SaveToFile(sfd.FileName);
            }

        }
        public void RefreshImage()
        {
            gbSubImages.Enabled = _arr2D.NumImages > 1;
            lblCurImage.Text = Convert.ToString(_arr2D.CurrentImage);

            if (tabPasses.SelectedIndex == 0)
            {
                double max = hScrollMax.Value;
                double min = hScrollMin.Value;
                tbMax.Text = Convert.ToString(max);
                tbMin.Text = Convert.ToString(min);
                pbImage.Image = _arr2D.BuildBitmap(max, min);
            }
            else
            {
                _arr2D.Precision = Convert.ToDouble(tbFinePrec.Text);
                _arr2D.ZRefMargin = Convert.ToDouble(tbFineZOff.Text);
                double ZHtRefRawAbs = _arr2D.ConvertGreyScaleToRawAbs(_ZRefGreyScale);
                double min2 = ZHtRefRawAbs - _arr2D.ZRefMarginRaw;
                double max2 = min2 + _arr2D.GetRawTargetRange();
                pbImage.Image = _arr2D.BuildBitmap(max2, min2);
            }
        }

        private void OnVChanged0(object sender, EventArgs e)
        {
            RefreshImage();
        }
        private void OnVChanged1(object sender, EventArgs e)
        {
            tbFinePrec.Text = Convert.ToString(hScrollPrec.Value / 100.0);
            tbFineZOff.Text = Convert.ToString(hScrollZOff.Value);
            RefreshImage();
        }
        private void btnSetCoarse_Click(object sender, EventArgs e)
        {
            try
            {
                _arr2D.MaxAcceptableValue = Convert.ToDouble(tbMax.Text);
                _arr2D.MinAcceptableValue = Convert.ToDouble(tbMin.Text);
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "Bad Entry");
            }
        }
        byte _ZRefGreyScale = 0;
        private void OnClickCoarse(object sender, EventArgs e)
        {
            Point pt = pbImage.PointToClient(MousePosition);
            lblRefPoint.Text = string.Format("Ref Color({0},{1})=", pt.X, pt.Y);
            _ZRefGreyScale = _arr2D.CoarseRawToGreyscale(pt.X, pt.Y);
            lblRefColor.Text = _ZRefGreyScale.ToString();
                        
            tabPasses.SelectedIndex = 1;
            //RefreshImage();
        }

        private void btnSetFine_Click(object sender, EventArgs e)
        {
            try
            {
                _arr2D.Precision = Convert.ToDouble(tbFinePrec.Text);
                _arr2D.ZRefMargin = Convert.ToDouble(tbFineZOff.Text);
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "Bad Entry");
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (_arr2D.CurrentImage > 0)
            {
                _arr2D.CurrentImage--;
                RefreshImage();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_arr2D.CurrentImage < _arr2D.NumImages-1)
            {
                _arr2D.CurrentImage++;
                RefreshImage();
            }
        }

        private void OnTabPageChanged(object sender, EventArgs e)
        {
            RefreshImage();
        }

        private void btnUploadCoarse_Click(object sender, EventArgs e)
        {
            UploadCoarse();
            RefreshImage();
        }
        private void UploadCoarse()
        {
            hScrollMax.Value = (int)Math.Round(_arr2D.MaxAcceptableValue);
            tbMax.Text = Convert.ToString(_arr2D.MaxAcceptableValue);
            hScrollMin.Value = (int)Math.Round(_arr2D.MinAcceptableValue);
            tbMin.Text = Convert.ToString(_arr2D.MinAcceptableValue);
        }
        private void btnUploadFine_Click(object sender, EventArgs e)
        {
            UploadFine();
            RefreshImage();
        }
        private void UploadFine()
        {
            tbFinePrec.Text = Convert.ToString(_arr2D.Precision);
            hScrollPrec.Value = (int)Math.Round(_arr2D.Precision * 100.0);
            tbFineZOff.Text = Convert.ToString(_arr2D.ZRefMargin);
            hScrollZOff.Value = (int)Math.Round(_arr2D.ZRefMargin);
        }

        private void OnPrecChanged(object sender, EventArgs e)
        {
            RefreshImage();
        }

    }
}
