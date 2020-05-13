using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Forms;

using MCore;
using MCore.Comp;
using MCore.Comp.MotionSystem;
using MDouble;

using AppMachine.Comp.CommonParam;
using AppMachine.Comp.Recipe;
using AppMachine.Control;

namespace AppMachine.Comp.Motion
{
    public partial class AppAxisTeachCtrl : AppUserControlBase
    {
        private RealAxis _axis = null;
        private MDouble.Millimeters _position = null;
        public delegate void DelParamMillimeters(MDouble.Millimeters mm);
        public event DelParamMillimeters evOnTeached = null;

        private Expression<Func<Millimeters>> _property = null;

        public AppAxisTeachCtrl():base()
        {
            //InitializeComponent();
        }

        public AppAxisTeachCtrl(String name,RealAxis axis):base(axis)
        {
            _axis = axis;
            
            InitializeComponent();
            lblItemName.Text = name;
        }

        public void RegisterProperty(Expression<Func<Millimeters>> property)
        {
            _property = property;
            CompBase comp = U.GetComponentFromPropExpression(_property);
            //object obj = (comp as AppProductRecipe).GetPropValue(property);
            object obj = comp.GetPropValue(_property);
            _position = (obj as MDouble.Millimeters);
            U.RegisterOnChanged(property, OnTeachPositionChanged);
        }

        public void UnRegisterProperty()
        {
            U.UnRegisterOnChanged(_property, OnTeachPositionChanged);
            evOnTeached = null;
        }


        private void OnTeachPositionChanged(MDouble.Millimeters property)
        {
            _position.Val = property.Val;
        }


        Random rand = new Random();
        private void btnTeach_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Teach Position Value will be Change." + Environment.NewLine +
                                                  "Click OK to teach or Cancel to ignore teach.", "Confirm Teach", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            if (result != DialogResult.OK)
            {
                return;
            }

            //_axis.SetCurrentPosition(rand.NextDouble()*100);
            _position.Val = _axis.CurrentPosition;
            if(evOnTeached!=null)
            {
                evOnTeached(_position);
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Please check any safety before move."+Environment.NewLine+
                                                  "Click OK to move or Cancel to ignore move.", "Safety Check", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            if (result != DialogResult.OK)
            {
                return;
            }
            try
            {
                _axis.MoveAbs(_position, _axis.DefaultSpeed);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error in \"{0}\" Go: {1}",lblItemName.Text, ex.Message), "Error");
            }
           
        }
    }
}
