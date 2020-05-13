using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml.Serialization;

using MDouble;

namespace MCore.Comp.Geometry
{
    [TypeConverterAttribute(typeof(ExpandableObjectConverter))]
    public class G3DPoint
    {

        /// <summary>
        /// Class to manage 3D millimeter point
        /// </summary>
        #region Privates

        private Millimeters _mmX = 0.0;
        private Millimeters _mmY = 0.0;
        private Millimeters _mmZ = 0.0;
        private Radians _yaw = 0.0;
        private eMode _xyyawMode = eMode.Relative;
        private eMode _zMode = eMode.Relative;

        #endregion Privates

        
        #region Public Properties

        public enum eMode { Relative, Absolute };

        public delegate void G3DPointEventHandler(G3DPoint g3DPoint);

        public event MethodInvoker OnChangedX;
        public event MethodInvoker OnChangedY;
        public event MethodInvoker OnChangedZ;
        public event MethodInvoker OnChangedYaw;

        /// <summary>
        /// X location
        /// </summary>
        [Browsable(true)]
        public Millimeters X
        {
            get { return _mmX; }
            set 
            {
                _mmX = value;
                if (OnChangedX != null)
                    OnChangedX();
            }
        }

        /// <summary>
        /// Y location
        /// </summary>
        [Browsable(true)]
        public Millimeters Y
        {
            get { return _mmY; }
            set 
            { 
                _mmY = value;
                if (OnChangedY != null)
                    OnChangedY();
            }
        }

        /// <summary>
        /// Z location
        /// </summary>
        [Browsable(true)]
        public Millimeters Z
        {
            get { return _mmZ; }
            set 
            { 
                _mmZ = value;
                if (OnChangedZ != null)
                    OnChangedZ();
            }
        }

        /// <summary>
        /// Theta angle
        /// </summary>
        [Browsable(true)]
        public Radians Yaw
        {
            get { return _yaw; }
            set
            {
                _yaw = value;
                if (OnChangedYaw != null)
                    OnChangedYaw();
            }
        }

        /// <summary>
        /// XYTheta mode
        /// </summary>
        [Browsable(true)]
        public eMode XYYawMode
        {
            get { return _xyyawMode; }
            set { _xyyawMode = value; }
        }

        /// <summary>
        /// Z mode
        /// </summary>
        [Browsable(true)]
        public eMode ZMode
        {
            get { return _zMode; }
            set { _zMode = value; }
        }

        /// <summary>
        /// Returns true if empty
        /// </summary>
        [Browsable(true)]
        public bool IsEmpty
        {
            get { return X == 0 && Y == 0 && Z == 0; }
        }
        /// <summary>
        /// Addition
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public static G3DPoint operator +(G3DPoint val1, G3DPoint val2)
        {
            G3DPoint newVal = new G3DPoint();
            newVal.X = val1.X + val2.X;
            newVal.Y = val1.Y + val2.Y;
            newVal.Z = val1.Z + val2.Z;
            return newVal;
        }
        #endregion Public Properties

        #region Constructors
        /// <summary>
        /// Default constructor for serialization
        /// </summary>
        public G3DPoint()
        {
        }
        /// <summary>
        /// Copy Constructor 1
        /// </summary>
        /// <param name="val"></param>
        public G3DPoint(G3DPoint val)
        {
            if (val != null)
            {
                X.Val = val.X.Val;
                Y.Val = val.Y.Val;
                Z.Val = val.Z.Val;
                Yaw.Val = val.Yaw.Val;
                XYYawMode = val.XYYawMode;
                ZMode = val.ZMode;
            }
        }
        /// <summary>
        /// Copy Constructor 2
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public G3DPoint(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        /// <summary>
        /// Copy Constructor 3
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public G3DPoint(float x, float y, float z)
        {
            X = (double)x;
            Y = (double)y;
            Z = (double)z;
        }
        /// <summary>
        /// Copy Constructor 4
        /// </summary>
        /// <param name="pt"></param>
        public G3DPoint(PointF pt)
        {
            X = pt.X;
            Y = pt.Y;
        }
        #endregion Constructors

        #region overrides

        /// <summary>
        /// Show the values
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{{X={0},Y={1},Z={2},Yaw={3}}}",
                _mmX.ToString(),
                _mmY.ToString(),
                _mmZ.ToString(),
                _yaw.ToString());
        }

        /// <summary>
        /// Returns the XY distance to the given point
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public Millimeters XYDistanceTo(G3DPoint pt)
        {
            double diffX = pt.X - X;
            double diffY = pt.Y - Y;
            return Math.Sqrt(diffX * diffX + diffY * diffY);
        }

        /// <summary>
        /// Returns the yaw angle to the given point
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public Radians YawTo(G3DPoint pt)
        {
            double diffX = pt.X - X;
            double diffY = pt.Y - Y;
            return Math.Atan2(diffY, diffX);
        }

        #endregion overrides
        /// <summary>
        /// Reset all values to default
        /// </summary>
        public void Reset()
        {
            _mmX = 0.0;
            _mmY = 0.0;
            _mmZ = 0.0;
            _yaw = 0.0;
            _xyyawMode = eMode.Relative;
            _zMode = eMode.Relative;
        }
        /// <summary>
        /// Convert to PointF
        /// </summary>
        /// <returns></returns>
        public PointF ToPointF
        {
            get
            {
                PointF newVal = new PointF();
                newVal.X = (float)X.Val;
                newVal.Y = (float)Y.Val;
                return newVal;
            }
        }

        /// <summary>
        /// Move this point a distance along Theta direction
        /// </summary>
        /// <param name="mmDist"></param>
        public void Move(Millimeters mmDist)
        {
            X += mmDist * Math.Cos(Yaw);
            Y += mmDist * Math.Sin(Yaw);
        }
    }
}
