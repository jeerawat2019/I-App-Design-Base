using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;
using AppMachine.Comp.CommonParam;


using MCore;
using MCore.Comp;
using MDouble;

namespace AppMachine.Comp.Part
{
    public class AppPart : CompBase
    {


        #region Standard Pattern
        /// <summary>
        /// Lot Id
        /// </summary>
        [Category("Product Info"), Browsable(true), Description("Lot Id")]
        public String LotId
        {
            get { return GetPropValue(() => LotId, ""); }
            set { SetPropValue(() => LotId, value); }
        }

        /// <summary>
        /// Product Name
        /// </summary>
        [Category("Product Info"), Browsable(true), Description("Product Name")]
        public string ProductName
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => ProductName, "None"); }
            [StateMachineEnabled]
            set { SetPropValue(() => ProductName, value); }
        }


        /// <summary>
        /// Product Code
        /// </summary>
        [Category("Product Info"), Browsable(true), Description("Product Code")]
        public string ProductCode
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => ProductCode, "None"); }
            [StateMachineEnabled]
            set { SetPropValue(() => ProductCode, value); }
        }


        /// <summary>
        /// Part Id
        /// </summary>
        [Category("General"), Browsable(true), Description("Part Id")]
        public int PartId
        {
            get { return GetPropValue(() => PartId); }
            set { SetPropValue(() => PartId, value); }
        }


        /// <summary>
        /// Part Status
        /// </summary>
        [Category("Status"), Browsable(true), Description("Part Status")]
        public AppEnums.ePartStatus PartStatus
        {
            get { return GetPropValue(() => PartStatus, AppEnums.ePartStatus.None); }
            set { SetPropValue(() => PartStatus, value); }
        }


        #endregion 



        //Additional Part Data
        /* Put Addtional Part Property Here (Example in bleow)
     
        #region Part Data
        
        /// <summary>
        /// Strip Id
        /// </summary>
        [Category("Product Info"), Browsable(true), Description("Strip Id")]
        public String StripId
        {
            get { return GetPropValue(() => StripId, ""); }
            set { SetPropValue(() => StripId, value); }
        }


        /// <summary>
        /// Part Id
        /// </summary>
        [Category("General"), Browsable(true), Description("Part Id")]
        public int PartId
        {
            get { return GetPropValue(() => PartId); }
            set { SetPropValue(() => PartId, value); }
        }


        /// <summary>
        /// Part Status
        /// </summary>
        [Category("Status"), Browsable(true), Description("Part Status")]
        public AppEnums.ePartStatus PartStatus
        {
            get { return GetPropValue(() => PartStatus, AppEnums.ePartStatus.None); }
            set { SetPropValue(() => PartStatus, value); }
        }
        #endregion

         * 
        /// <summary>
        /// Time Stamp
        /// </summary>
        [Category("Start Time Stamp"), Browsable(true), Description("Start Time Stamp")]
        public DateTime StartTimeStamp
        {
            get { return GetPropValue(() => StartTimeStamp, DateTime.Now); }
            set { SetPropValue(() => StartTimeStamp, value); }
        }


        /// <summary>
        /// Completed Time Stamp
        /// </summary>
        [Category("Completed Time Stamp"), Browsable(true), Description("Completed Time Stamp")]
        public DateTime CompletedTimeStamp
        {
            get { return GetPropValue(() => CompletedTimeStamp, DateTime.Now); }
            set { SetPropValue(() => CompletedTimeStamp, value); }
        }


        //Add More Application Requirement in Below
        /// <summary>
        /// Vision Alignment Score
        /// </summary>
        [Category("Data"), Browsable(true), Description("Vision Alignment Score")]
        public MDoubleNoUnits VisionAlignScore
        {
            get { return GetPropValue(() => VisionAlignScore); }
            set { SetPropValue(() => VisionAlignScore, value); }
        }


        //Add More Application Requirement in Below
        /// <summary>
        /// Vision Laser Lap Score
        /// </summary>
        [Category("Data"), Browsable(true), Description("Vision Laser Lap Score")]
        public MDoubleNoUnits VisionLaserLapScore
        {
            get { return GetPropValue(() => VisionLaserLapScore); }
            set { SetPropValue(() => VisionLaserLapScore, value); }
        }


        //Add More Application Requirement in Below
        /// <summary>
        /// Vision Laser Mark
        /// </summary>
        [Category("Data"), Browsable(true), Description("Vision Laser Mark Score")]
        public MDoubleNoUnits VisionLaserMarkScore
        {
            get { return GetPropValue(() => VisionLaserMarkScore); }
            set { SetPropValue(() => VisionLaserMarkScore, value); }
        }


        /// <summary>
        /// Vision Align Image
        /// </summary>
        [Category("Data"), Browsable(false), Description("Vision Align Image")]
        [XmlIgnore]
        public Image VisionAlignImage
        {
            get { return GetPropValue(() => VisionAlignImage); }
            set { SetPropValue(() => VisionAlignImage, value); }
        }


        /// <summary>
        /// Vision Laser Lap Image
        /// </summary>
        [Category("Data"), Browsable(false), Description("Vision Laser Lap Image")]
        [XmlIgnore]
        public Image VisionLaserLapImage
        {
            get { return GetPropValue(() => VisionLaserLapImage); }
            set { SetPropValue(() => VisionLaserLapImage, value); }
        }


        /// <summary>
        /// Vision Laser Mark Image
        /// </summary>
        [Category("Data"), Browsable(false), Description("Vision Laser Mark Image")]
        [XmlIgnore]
        public Image VisionLaserMarkImage
        {
            get { return GetPropValue(() => VisionLaserMarkImage); }
            set { SetPropValue(() => VisionLaserMarkImage, value); }
        }

        #endregion

        #region Part Data Status

        /// <summary>
        /// Vision Align Status
        /// </summary>
        [Category("Status"), Browsable(true), Description("Vision Align Status")]
        public AppEnums.ePartStatus VisionAlignStatus
        {
            get { return GetPropValue(() => VisionAlignStatus, AppEnums.ePartStatus.None); }
            set { SetPropValue(() => VisionAlignStatus, value); }
        }


        /// <summary>
        /// Vision Laser Lap Status
        /// </summary>
        [Category("Status"), Browsable(true), Description(" Vision Laser Lap Status")]
        public AppEnums.ePartStatus VisionLaserLapStatus
        {
            get { return GetPropValue(() => VisionLaserLapStatus, AppEnums.ePartStatus.None); }
            set { SetPropValue(() => VisionLaserLapStatus, value); }
        }

        /// <summary>
        /// Vision Laser Mark Status
        /// </summary>
        [Category("Status"), Browsable(true), Description(" Vision Laser Mark Status")]
        public AppEnums.ePartStatus VisionLaserMarkStatus
        {
            get { return GetPropValue(() => VisionLaserMarkStatus, AppEnums.ePartStatus.None); }
            set { SetPropValue(() => VisionLaserMarkStatus, value); }
        }


        /// <summary>
        /// Laser Mark Status
        /// </summary>
        [Category("Status"), Browsable(true), Description("Laser Mark Status")]
        public bool LaserMarkStatus
        {
            get { return GetPropValue(() => LaserMarkStatus, false); }
            set { SetPropValue(() => LaserMarkStatus, value); }
        }

        #endregion
        */

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public AppPart()
        {

        }

        /// <summary>
        /// Manual Creation Constructor
        /// </summary>
        /// <param name="name"></param>
        public AppPart(string name)
            : base(name)
        {

        }

        /// <summary>
        /// Manual Creation Constructor
        /// </summary>
        /// <param name="name"></param>
        public AppPart(string name, int partId)
            : base(name)
        {
            PartId = partId;
        }
        #endregion Constructors


        #region Standard Pattern
        public override CompBase Clone(string name, bool bRecursivley)
        {
            AppPart clonedPart = base.Clone(name, bRecursivley) as AppPart;
            AppUtility.CloneAllProperties(this, clonedPart);
            return clonedPart;
        }
        #endregion

        //Add More Application Requirement in Below
        /* Add more Part method Here (Example In below)
        
        public void SavePartData(Double recordData, Expression<Func<MDoubleNoUnits>> dataProperty, double specData, Expression<Func<AppEnums.ePartStatus>> statusPorperty)
        {
            var dataMember = (MemberExpression)dataProperty.Body;

            SetPropValue(dataProperty, recordData);
            if (recordData == -1)
            {
                SetPropValue(statusPorperty, AppEnums.ePartStatus.Missing);
                PartStatus = AppEnums.ePartStatus.Missing;
            }
            else if (recordData >= 0 && recordData < specData)
            {
                SetPropValue(statusPorperty, AppEnums.ePartStatus.Fail);
                PartStatus = AppEnums.ePartStatus.Fail;
            }
            else if (recordData > specData)
            {
                SetPropValue(statusPorperty, AppEnums.ePartStatus.Pass);
                if (PartStatus == AppEnums.ePartStatus.None ||
                   PartStatus == AppEnums.ePartStatus.Pass)
                {
                    PartStatus = AppEnums.ePartStatus.Pass;
                }
            }
        }

        public void ResetPartStatus()
        {
            PartStatus = AppEnums.ePartStatus.None;
            VisionAlignStatus = AppEnums.ePartStatus.None;
            VisionLaserLapStatus = AppEnums.ePartStatus.None;
            VisionLaserMarkStatus = AppEnums.ePartStatus.None;
            LaserMarkStatus = false;
        }


        public void LogImage()
        {

            string ImageLogSubPart = "";
            if (this.PartStatus == AppEnums.ePartStatus.Pass && AppCommonParam.This.EnableSaveGoodPartImage)
            {
                ImageLogSubPart = string.Format("{0}\\{1} Lot {2}\\{3} {4}\\{5} [{6}]\\", "Strips Image Log",this.ProductName,this.LotId, this.Parent.Name,this.CompletedTimeStamp.ToString("dd-MM-yyyy HH.mm.ss"),this.Name,"Good");
            }
            else if(this.PartStatus == AppEnums.ePartStatus.Fail && AppCommonParam.This.EnableSaveFailPartImage)
            {
                ImageLogSubPart = string.Format("{0}\\{1} Lot {2}\\{3} {4}\\{5} [{6}]\\", "Strips Image Log", this.ProductName,this.LotId, this.Parent.Name, this.CompletedTimeStamp.ToString("dd-MM-yyyy HH.mm.ss"),this.Name,"Fail");
            }
            else if (this.PartStatus == AppEnums.ePartStatus.Fail && AppCommonParam.This.EnableSaveMissingPartImage)
            {
                ImageLogSubPart = string.Format("{0}\\{1} Lot {2}\\{3} {4}\\{5} [{6}]\\", "Strips Image Log", this.ProductName,this.LotId, this.Parent.Name, this.CompletedTimeStamp.ToString("dd-MM-yyyy HH.mm.ss"),this.Name,"Missing");
            }

            if(ImageLogSubPart == "")
            {
                return;
            }

            String logPath = U.GetLogFilePath(ImageLogSubPart);
            U.EnsureDirectory(logPath);
            if(this.VisionAlignImage != null)
            { 
                string saveImagefileName = String.Format("\\Align-{0} {1}_{2}.bmp",this.VisionAlignStatus.ToString(),this.Parent.Name, this.Name);
                string fullPath = logPath + saveImagefileName;
                try
                {
                    Bitmap bmp = new Bitmap(this.VisionAlignImage);
                    bmp.Save(fullPath);
                }
                catch(Exception ex)
                {
                    ex.ToString();
                }
            }

            if (this.VisionLaserLapImage != null)
            {
                string saveImagefileName = String.Format("\\Lap-{0} {1}_{2}.bmp", this.VisionLaserLapStatus.ToString(), this.Parent.Name, this.Name);
                string fullPath = logPath + saveImagefileName;
                try
                {
                    Bitmap bmp = new Bitmap(this.VisionLaserLapImage);
                    bmp.Save(fullPath);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }


            if (this.VisionLaserMarkImage != null)
            {
                string saveImagefileName = String.Format("\\Mark-{0} {1}_{2}.bmp", this.VisionLaserMarkStatus.ToString(), this.Parent.Name, this.Name);
                string fullPath = logPath + saveImagefileName;
                try
                {
                    Bitmap bmp = new Bitmap(this.VisionLaserMarkImage);
                    bmp.Save(fullPath);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
        }

        public Image ResizeImage(Image image, double resizeRatio)
        {
            int newWidth;
            int newHeight;

            if (image == null)
            {
                return null;
            }

            int originalWidth = image.Width;
            int originalHeight = image.Height;
            newWidth = (int)(originalWidth * resizeRatio);
            newHeight = (int)(originalHeight * resizeRatio);

            Image newImage = new Bitmap(newWidth, newHeight);
            if (resizeRatio != 1)
            {
                using (Graphics graphicsHandle = Graphics.FromImage(newImage))
                {
                    graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight);
                }
            }
            else
            {
                return image;
            }
            return newImage;
        }
        */

    }
}
