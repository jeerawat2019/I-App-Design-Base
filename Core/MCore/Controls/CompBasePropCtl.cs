using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MCore.Comp;

namespace MCore.Controls
{
    public partial class CompBasePropCtl : UserControl, IComponentBinding<CompBase>
    {

        private CompBase _compBase = null;

        [Browsable(true)]
        public AttributeCollection BrowsableAttributes
        {
            get { return propertyGrid.BrowsableAttributes; }
            set { propertyGrid.BrowsableAttributes = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CompBasePropCtl()
        {
            InitializeComponent();

        }
        /// <summary>
        /// Bind to component
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CompBase Bind
        {
            get { return _compBase; }
            set
            {
                _compBase = value;
                cbClassChooser.Hide();
                propertyGrid.SelectedObject = _compBase;
                if (_compBase != null)
                {
                    try
                    {
                        var children = propertyGrid.Controls.OfType<Control>();
                        ToolStrip toolStrip = children.Where((c) => c is ToolStrip).Single() as ToolStrip;
                        if (toolStrip != null)
                        {
                            ToolStripLabel tsLabel = new ToolStripLabel();
                            tsLabel.DisplayStyle = ToolStripItemDisplayStyle.Text;
                            tsLabel.Name = "tsLabel";
                            tsLabel.Visible = true;
                            tsLabel.AutoSize = true;
                            tsLabel.Text = "Properties of " + _compBase.ID;
                            toolStrip.Items.Add(tsLabel);
                        }
                    }
                    catch { }                    
                }

                CompBase.OnChangedName += new CompBase.NameChangedEventHandler(OnChangedCompName);
            }
        }


        private void OnChangedCompName(CompBase comp, string oldID)
        {
            try
            {
                if (string.IsNullOrEmpty(oldID))
                {
                    cbClassChooser.Items.Clear();
                    Type[] types = CompRoot.GetSiblingTypes(comp.GetType());
                    if (types != null)
                    {
                        cbClassChooser.Items.AddRange(types);
                        cbClassChooser.SelectedItem = comp.GetType();
                        cbClassChooser.Show();
                        cbClassChooser.DroppedDown = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        /// <summary>
        /// This will be the name for the tab page
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Properties";
        }

        private void OnClassTypeChanged(object sender, EventArgs e)
        {
            if (cbClassChooser.Visible)
            {
                Type newType = cbClassChooser.SelectedItem as Type;
                if (newType.Name != _compBase.GetType().Name)
                {
                    // Change the type
                    CompBase newComp = Activator.CreateInstance(newType, _compBase.Name) as CompBase;
                    _compBase.Parent.ReplaceChild(newComp);
                    Bind = newComp;
                }
                else
                {
                    cbClassChooser.Hide();
                }
            }
        }
    }
}
