using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using MCore.Comp;

namespace MCore.Controls
{
    public partial class MPIDCtl : UserControl
    {
        private bool _editing = false;
        private bool _noSelect = false;
        private Point _initialLoc = Point.Empty;
        private MouseEventHandler _delMouseClick = null;

        public enum eTypes { Void, String, Boolean, Int32, UInt32, Int64, UInt64, Object };
        private eTypes _sReturnType = eTypes.Void;
        private Type _returnType = null;
        private IDListBase _primaryStatement = null;
        private List<IDPropertyList> _argStatements = null;
        private CompBase _rootNode = null;
        private CompBase _compObj = null;
      
        private bool _logChanges = true;

        private bool _autoLastType = true;
        /// <summary>
        /// Flag to indicate if we are to add a log entry if user changes the value
        /// </summary>
        public bool LogChanges
        {
            get { return _logChanges; }
            set { _logChanges = value; }
        }
        public List<IDPropertyList> ArgStatements
        {
            get { return _argStatements; }
        }

        public CompBase RootNode
        {
            get { return _rootNode; }
        }
        public MouseEventHandler DelMouseClick
        {
            get { return _delMouseClick; }
        }

        public IDListBase PrimaryStatement
        {
            get { return _primaryStatement; }
        }

        public int NextControlNo = 2;

        public Label LblRoot
        {
            get { return lblRoot; }
        }
        /// <summary>
        /// The return type
        /// </summary>
        public eTypes ReturnType
        {
            get
            {

                return _sReturnType;
            }
            set
            {
                _sReturnType = value;
            }
        }

        public bool AutoLastType
        {
            get
            {
                return _autoLastType;
            }

            set
            {
                _autoLastType = value;
            }
        }


        private string _scopeID = string.Empty;
        public string ScopeID
        {
            get { return _scopeID; }
            set { _scopeID = value; }
        }

        public MPIDCtl()
        {
            InitializeComponent();
            _initialLoc = lblRoot.Location;
            _delMouseClick = new MouseEventHandler(this.OnMouseClick);

            //_rootElement = new IDNodeBase(this, lblRoot, Controls, _returnType, _delMouseClick);
        }

        public string RemoveScopeID(string id)
        {
            if (!string.IsNullOrEmpty(ScopeID) && id.StartsWith(ScopeID))
            {
                return id.Substring(ScopeID.Length + 1);
            }
            return id;
        }


        public string BuildFullID(string id)
        {
            if (!string.IsNullOrEmpty(ScopeID))
            {
                id = string.Format("{0}.{1}", ScopeID, id);
            }
           
            return id.Trim('<', ',', '(', ')');
        }
        private Object _objTarget = null;
        private Func<string> _propertyGetter = null;
        private Action<string> _propertySetter = null;
        private Expression<Func<string>> _propertyLambda = null;
        /// <summary>
        /// Provide two-way binding
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyLambda"></param>
        public void BindTwoWay(Expression<Func<string>> propertyLambda)
        {
            UnBind();
            _propertyLambda = propertyLambda;
            MemberExpression member = propertyLambda.Body as MemberExpression;
            PropertyInfo propInfo = member.Member as PropertyInfo;
            // Get the getter delegate
            _propertyGetter = propertyLambda.Compile();
            // Get the target object
            Func<Object> delObj = Expression.Lambda<Func<Object>>(member.Expression).Compile();
            _objTarget = delObj();
            _propertySetter = Delegate.CreateDelegate(typeof(Action<string>), _objTarget, "set_" + propInfo.Name) as Action<string>;
            if (_objTarget is CompBase)
            {
                _compObj = _objTarget as CompBase;
                U.RegisterOnChanged(propertyLambda, OnChanged);
            }
            OnChanged(_propertyGetter());
        }
        public void UnBind()
        {
            if (_objTarget != null && _objTarget is CompBase)
            {
                U.UnRegisterOnChanged(_propertyLambda, OnChanged);
            }
            DisposeAllListsAndControls();
            _objTarget = null;
        }
        private string _id = string.Empty;
        /// <summary>
        /// Get/Set the Id
        /// </summary>
        public string ID
        {
            get { return _id; }
            set
            {
                _id = value;
            }
        }
        private void OnChanged(string id)
        {
            if (_editing)
                return;
            _id = id;
            // Clean up
            _returnType = Type.GetType("System." + _sReturnType.ToString());

            if (U.ComponentExists(ScopeID))
            {
                _rootNode = U.GetComponent(ScopeID);
            }
            else
            {
                _rootNode = U.RootComp;
            }

            BuildAllSections();
            RepositionControls();
        }


        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            IDNodeBase selNode = (sender as Control).Tag as IDNodeBase;
            _noSelect = false;
            DropList(selNode);
        }

        /// <summary>
        /// Fill the combo box and select the first entry
        /// </summary>
        /// <param name="pathElement"></param>
        private void DropList(IDNodeBase selNode)
        {
            if (selNode == null)
            {
                return;
            }
            //_editing = true;
            IDListBase listBase = selNode.ListBase;
            cbChildrenList.Location = selNode.Location;
            cbChildrenList.Tag = selNode;
            cbChildrenList.Items.Clear();

            // Add choice to de-select if target is path
            //if (listBase is IDPathList)
            //{
            //    cbChildrenList.Items.Add("");
            //}

            // Add component children
            selNode.AddComponents(cbChildrenList);

            selNode.AddMethodsOrProperties(cbChildrenList);

            _noSelect = true;
            int sel = cbChildrenList.FindString(selNode.Text);
            if (sel >= 0)
            {
                cbChildrenList.SelectedIndex = sel;
            }

            cbChildrenList.BringToFront();
            cbChildrenList.Show();
            cbChildrenList.Select();
            if (selNode.IsLiteral)
            {
                cbChildrenList.DropDownStyle = ComboBoxStyle.DropDown;
                cbChildrenList.Text = selNode.Text;
                cbChildrenList.SelectionStart = 0;
                cbChildrenList.SelectionLength = selNode.Text.Length;
            }
            else
            {
                if (selNode.ListBase is IDPropertyList && selNode.IsFirst)
                {
                    cbChildrenList.DropDownStyle = ComboBoxStyle.DropDown;
                    cbChildrenList.Text = "";
                    //cbChildrenList.Items.Insert(0, "");
                }
                else
                {
                    cbChildrenList.DropDownStyle = ComboBoxStyle.DropDownList;
                }
                cbChildrenList.DroppedDown = true;
            }

            _noSelect = false;
        }

        /// <summary>
        /// We just closed drop down.
        /// Did we finish? or is there more to select?
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDropDownClosed(object sender, EventArgs e)
        {
            if (_noSelect || cbChildrenList.SelectedItem == null)
                return;

            IDNodeBase selNode = cbChildrenList.Tag as IDNodeBase;

            string newText = cbChildrenList.SelectedItem.ToString();

            IDNodeBase dropNode = selNode.ListBase.NewSelection(selNode, newText);
            cbChildrenList.Hide();
            RepositionControls();
            StoreID();
            DropList(dropNode);
        }
        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && cbChildrenList.Visible)
            {
                IDNodeBase selNode = cbChildrenList.Tag as IDNodeBase;
                if (cbChildrenList.SelectedItem == null)
                {
                    // Literal
                    IDNodeBase node = selNode.ListBase.TrimNodes(selNode);
                    node.Text = cbChildrenList.Text;
                    node.IsLiteral = true;
                    cbChildrenList.Hide();
                    RepositionControls();
                    StoreID();
                }
                else
                {
                    cbChildrenList.DroppedDown = false;
                }

            }
            else if (e.KeyChar == (char)Keys.Escape)
            {
                _noSelect = true;
                cbChildrenList.Hide();
                BuildAllSections();
                RepositionControls();
            }
        }


        private void OnComboBoxLeave(object sender, EventArgs e)
        {
            OnKeyPress(null, new KeyPressEventArgs((char)Keys.Enter));

        }

        private int _xPos = 0;
        public int XPos
        {
            get { return _xPos; }
            set { _xPos = value; }
        }

        private void StoreID()
        {
            string id = string.Empty;
            if (_primaryStatement != null)
            {
                id = _primaryStatement.BuildID();
                if (_argStatements != null)
                {
                    foreach(IDPropertyList list in _argStatements)
                    {
                        id += list.BuildID();
                    }                    
                }
            }
            if (!string.IsNullOrEmpty(id) && _primaryStatement is IDMethodList && !id.EndsWith(")"))
            {
                // Incomplete method
                return;
            }
            _editing = true;
            ID = id;
            if (_propertySetter != null)
            {
                string oldVal = _propertyGetter();
                if (oldVal != id)
                {
                    _propertySetter(id);
                    if (LogChanges && _compObj != null)
                    {
                        U.LogChange(_compObj.Nickname, oldVal, id);
                    }
                }
            }
            _editing = false;
        }

        private void RepositionControls()
        {
            _xPos = 0;
            _primaryStatement.RepositionAll();
            if (_argStatements != null)
                _argStatements.ForEach(c => c.RepositionAll());
        }

        private void DisposeAllListsAndControls()
        {
            // Clear everything
            if (_primaryStatement != null)
            {
                _primaryStatement.Clear();
                _primaryStatement = null;
            }
            DisposeAllArgs();
            NextControlNo = 2;
        }

        public void DisposeAllArgs()
        {
            if (_argStatements != null)
            {
                _argStatements.ForEach(c => c.Clear());
                _argStatements.Clear();
                _argStatements = null;
            }
        }


        private void BuildAllSections()
        {
            DisposeAllListsAndControls();

            if (_returnType.Equals(typeof(void)))
            {
                _primaryStatement = new IDMethodList(this);
            }
            else
            {
                _primaryStatement = new IDPropertyList(this, _returnType, -1);
            }

            if (string.IsNullOrEmpty(_id))
            {
                return;
            }

            string id = "";
            if (!_id.Contains("(Object)")) 
            {
               id = SplitMethodOrProperty(_primaryStatement, _id);
               BuildMethodArgs(id);
            }
            else
            {
                
                id = _id.Replace("(Object)", "");
                _primaryStatement.InsertPathNode(id);
            }

           
            // Now we validate Methods and arguments
            if (_argStatements != null)
            {
                IDMethodList mList = _primaryStatement as IDMethodList;
                string methodID = BuildFullID(mList.BuildID());
                Type[] args = new Type[_argStatements.Count];
                for (int i=0; i < _argStatements.Count; i++)
                {
                    args[i] = _argStatements[i].PropNode.type;
                }
                if (!mList.ConfirmArgTypes(args))
                {
                    U.LogPopup("Unable to find matching method for '{0}'", _id);
                }
            }
        }
        public string SplitMethodOrProperty(IDListBase curList, string id)
        {
            // Could be any of the following
            // CompNode.CompNode.Method(...         
            // CompNode.CompNode.Property,...
            // Method(...
            // Property,...

            // No Literals expected here
            while (!string.IsNullOrEmpty(id))
            {
                string sNode = id;
                int iDelim = id.IndexOfAny(new char[] { '.', ',', '(' });
                char chDelim = '\0';
                if (iDelim < 0)
                {
                    id = string.Empty;
                }
                else
                {
                    chDelim = id[iDelim];
                    sNode = id.Substring(0, iDelim);
                    id = id.Substring(iDelim + 1);
                }
                if (chDelim == '.')
                {
                    // We have here a Path element
                    curList.InsertPathNode(sNode);
                }
                else // Must be last element
                {
                    curList.SetLast(sNode.TrimEnd(')'));
                    break;
                }
            }
            return id;
        }
        public string SplitLiteral(IDPropertyList propList, string id)
        {
            // Should be in the form of 
            // "(type)1.00,
            // "(type),
            // Extract the type
            Type tgtType = null;
            int tyEnd = id.IndexOf(')');
            if (tyEnd > 0)
            {
                string sType = id.Substring(1, tyEnd - 1);
                id = id.Substring(tyEnd + 1);
                try
                {
                    tgtType = Type.GetType("System." + sType);
                }
                catch { }
                if (tgtType == null)
                {
                    try
                    {
                        tgtType = U.TryGetType(sType);
                    }
                    catch  { }
                }
                if (tgtType == null)
                {
                    U.LogPopup("expected to find a type declaration for '{0}'", id);
                    tgtType = typeof(string);
                }
            }
            else
            {
                U.LogPopup("expected to find a closing ')' in type declaration for '{0}'", id);
            }
                
            // Look for end of value
            string value = string.Empty;
            int endOfVal = id.IndexOf(',');
            if (endOfVal >= 0)
            {
                value = id.Substring(0, endOfVal);
                id = id.Substring(endOfVal);
            }
            else
            {
                value = id.TrimEnd(')');
                id = string.Empty;
            }
            propList.SetLiteralValue(value, tgtType);

            return id.TrimStart(',');
        }
        public void BuildMethodArgs(string id)
        {
            // Could be any of the following
            // CompNode.CompNode.Property,
            // ()
            // (Property,
            // ((type)literal,
            // ((type)
            DisposeAllArgs();
            if (string.IsNullOrEmpty(id) || id[0] == ')')
            {
                return;
            }
            int nArg = 0;
            _argStatements = new List<IDPropertyList>();

            while (!string.IsNullOrEmpty(id))
            {
                _argStatements.Add(new IDPropertyList(this, null, nArg));
                IDPropertyList curList = _argStatements[nArg++];
                if (id[0] == '(')
                {
                    id = SplitLiteral(curList, id);
                }
                else
                {
                    id = SplitMethodOrProperty(curList, id);
                }
            }
        }


    }

    public class IDListBase
    {
        protected Type _tgtType = null;
        protected List<IDNodeBase> _list = new List<IDNodeBase>();
        protected MPIDCtl _mpIDCtl = null;
        public Type TgtType
        {
            get { return _tgtType; }
        }
        public CompBase ParentCompOf(IDNodeBase node)
        {
            if (_list.Count > 0)
            {
                int index = _list.IndexOf(node);
                if (index < 0)
                {
                    foreach (IDNodeBase nd in _list)
                    {
                        index = _list.IndexOf(nd);
                        if (index < 0)
                        {
                            U.LogPopup("Node not found '{0}'.", nd.Text);
                        }
                    }
                }
                if (index > 0)
                {
                    return (_list[index - 1] as IDPath).Comp;
                }
            }
            return _mpIDCtl.RootNode;
        }
        public bool IsFirst(IDNodeBase node)
        {
            return object.ReferenceEquals(_list.First(), node); 
        }
        public bool IsPrimaryStatement
        {
            get { return object.ReferenceEquals(_mpIDCtl.PrimaryStatement, this); }
        }
        public IDListBase(MPIDCtl mpIDCtl)
        {
            _mpIDCtl = mpIDCtl;
        }

        public void Clear()
        {
            _list.ForEach( c => c.Clear());
            _list.Clear();
        }
        /// <summary>
        /// Inserts a path node just before last node
        /// </summary>
        /// <param name="text"></param>
        public void InsertPathNode(string text)
        {
            IDPath newPathNode = new IDPath(_mpIDCtl, this) { Text = text };
            _list.Insert(_list.Count - 1, newPathNode);
            SetComp(newPathNode);
            _list.Last().IsLiteral = false;

        }
        public void SetComp(IDPath nodeStop)
        {
            if (!string.IsNullOrEmpty(nodeStop.Text))
            {
                string id = _mpIDCtl.BuildFullID(BuildID(nodeStop).TrimStart(',', '('));
                if (U.ComponentExists(id))
                {
                    nodeStop.Comp = U.GetComponent(id);
                }
            }
        }
        public virtual void SetLast(string text)
        {
            _list.Last().Text = text;
        }
        public string BuildID()
        {
            return BuildID(null);
        }
        public string BuildID(IDNodeBase nodeStop)
        {
            string id = string.Empty;
            foreach (IDNodeBase node in _list)
            {
                id += node.StoreText;
                if (object.ReferenceEquals(node, nodeStop))
                {
                    break;
                }
            }
            return id.TrimStart('>');
        }
        public IDNodeBase TrimNodes(IDNodeBase node)
        {
            int index = _list.IndexOf(node);
            while (index >= 0 && index < _list.Count - 1)
            {
                _list.RemoveAt(index);
                node.Clear();
                node = _list[index];
            }
            return node;
        }

        /// <summary>
        /// Just selected something new
        /// </summary>
        /// <param name="newText"></param>
        public IDNodeBase NewSelection(IDNodeBase node, string newText)
        {
            // Remove trailing nodes until we reach the last node
            // NEVER remove the last node
            node = TrimNodes(node);
            // Now we are targeted at last node (Will be Property (incl literal) or Method)
            int index = newText.IndexOfAny(new char[] { '(', ';'});
            if (index >= 0)
            {
                // Method or property (or literal)
                node.Text = newText.Substring(0, index);
                if (newText[index] == '(')
                {
                    if (!(this is IDMethodList))
                    {
                        U.LogPopup("Expected method to be in IDMethodList '{0}'", newText);
                    }
                    // We are method.  Let's resetup all args
                    _mpIDCtl.BuildMethodArgs(newText.Substring(index + 1));
                }
                return null;
            }
            else
            {
                // Path , Insert as Path
                InsertPathNode(newText);
                node.Text = string.Empty;
                return node;
            }
        }
        public void RepositionAll()
        {
            _list.ForEach(c => c.Reposition());
        }
    }


    class IDMethodList : IDListBase
    {
        // IDPath.IDPath.Method
        public IDMethodList(MPIDCtl mpIDCtl)
            : base(mpIDCtl)
        {
            _tgtType = typeof(void);
            _list.Add(new IDMethod(_mpIDCtl, this));
        }

        public CompBase MethodComponent
        {
            get
            {
                if (_list.Count == 1)
                {
                    return _mpIDCtl.RootNode;
                }
                else
                {
                    return (_list[_list.Count - 2] as IDPath).Comp;
                }
            }
        }
        public bool ConfirmArgTypes(Type [] argTypes)
        {
            try
            {
                MethodInfo mi = MethodComponent.GetType().GetMethod(_list.Last().Text, argTypes);
                return mi != null;
            }
            catch
            {
                return false;
            }
        }
    }

    public class IDPropertyList : IDListBase
    {
        int _argNum = 0;
        public int ArgNum
        {
            get { return _argNum; }
        }

        public IDProperty PropNode
        {
            get { return _list.Last() as IDProperty; }
        }

        // IDPath.IDPath.Property
        // Literal
        public IDPropertyList(MPIDCtl mpIDCtl, Type ty, int argNum)
            : base(mpIDCtl)
        {
            _tgtType = ty;
            _argNum = argNum;
            _list.Add(new IDProperty(_mpIDCtl, this, ty));
        }
        public override void SetLast(string text)
        {
            base.SetLast(text);
            // Assume it is not literal.  Get type from property
            PropertyInfo pi = U.GetPropertyInfo(_mpIDCtl.BuildFullID(BuildID()));
            if (pi != null)
            {
                if (_mpIDCtl.AutoLastType)
                {
                    _tgtType = pi.PropertyType;
                }
                else
                {
                    _tgtType = Type.GetType("System." + _mpIDCtl.ReturnType.ToString());
                }

                IDProperty idProp = (_list.Last() as IDProperty);
                idProp.type = _tgtType;
            }
            else
            {
                U.LogPopup("Could not get Property info for '{0}'", text);
            }
        }

        public void SetLiteralValue(string text, Type tgtType)
        {
            // Either setting the end of property link or is a literal.  Only used when parsing
            _list.Last().IsLiteral = true;
            _tgtType = tgtType;
            if (string.IsNullOrEmpty(text))
            {
                text = U.GetDefaultText(tgtType);
            }
            _list.Last().Text = text;
            IDProperty idProp = (_list.Last() as IDProperty);
            idProp.type = _tgtType;
        }
    }

    public class IDNodeBase
    {
        public string Text = string.Empty;
        protected Label _lblText = new Label();
        protected MPIDCtl _mpIDCtl = null;
        private IDListBase _listBase = null;
        public CompBase ParentComp
        {
            get 
            { 
                return _listBase.ParentCompOf(this); 
            }
        }
        public virtual bool IsLiteral
        {
            get { return false; }
            set { }
        }
        public Point Location
        {
            get { return _lblText.Location; }
        }
        public IDListBase ListBase
        {
            get { return _listBase; }
        }
        public bool IsFirst
        {
            get { return _listBase.IsFirst(this); }
        }

        public string Postfix
        {
            get
            {
                string postfix = string.Empty;
                // Only some get postfix
                //if (Method and no args, postfix = "()"
                // If Method and last arg, postFix = ")"
                if (_mpIDCtl.PrimaryStatement is IDMethodList)
                {
                    if ((this is IDMethod) && _mpIDCtl.ArgStatements == null)
                    {
                        postfix = "()";
                    }
                    else if (_listBase is IDPropertyList && (_listBase as IDPropertyList).ArgNum == _mpIDCtl.ArgStatements.Count - 1)
                    {
                        if (this is IDProperty)
                        {
                            postfix = ")";
                        }
                    }
                }
                return postfix;
            }
        }
        public string DisplayText
        {
            get 
            {
                string prefix = "";
                // Everything gets a prefix
                if (_listBase.IsFirst(this))
                {
                    if (_listBase.IsPrimaryStatement)
                    {
                        prefix = ">";
                    }
                    // Must be property if nor primary
                    else if ((_listBase as IDPropertyList).ArgNum == 0)
                    {
                        prefix = "(";
                    }
                    else
                    {
                        prefix = ",";
                    }
                }
                else
                {
                    prefix = ".";
                }

                return string.Format("{0}{1}{2}", prefix, Text, Postfix);
            }
        }
        public virtual string StoreText
        {
            get { return DisplayText; }
        }
        public IDNodeBase(MPIDCtl mpIDCtl, IDListBase listBase)
        {
            _mpIDCtl = mpIDCtl;
            _listBase = listBase;

            int index = _mpIDCtl.NextControlNo++;

            _lblText.AutoSize = false;
            _lblText.Name = string.Format("lblName{0}", index);
            _lblText.Size = new System.Drawing.Size(0, 13);
            _lblText.Font = _mpIDCtl.LblRoot.Font;
            _lblText.FlatStyle = System.Windows.Forms.FlatStyle.System;
            _lblText.Margin = new System.Windows.Forms.Padding(0);
            _lblText.TabIndex = index;
            _lblText.Text = "";
            _lblText.Tag = this;
            _lblText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            _lblText.MouseClick += _mpIDCtl.DelMouseClick;
            //_lblText.MouseEnter += new EventHandler(OnMouseEnter);
            //_lblText.MouseLeave += new EventHandler(OnMouseLeave);
            mpIDCtl.Controls.Add(_lblText);
        }
        public void Clear()
        {
            _mpIDCtl.Controls.Remove(_lblText);
            _lblText.Dispose();
        }
        public void Reposition()
        {
            _lblText.Visible = true;
            _lblText.Text = DisplayText;
            _lblText.Location = new Point(_mpIDCtl.XPos, 4);
            using (Graphics g = _lblText.CreateGraphics())
            {
                SizeF size = g.MeasureString(_lblText.Text, _lblText.Font, 200);
                _lblText.Width = (int)Math.Ceiling(size.Width) - 5;
                _mpIDCtl.XPos += _lblText.Width;
            }
        }
        public void AddComponents(ComboBox cb)
        {
            if (ParentComp != null)
            {
                ParentComp.ForEach(c => cb.Items.Add(c.Name));
            }
        }
        public void AddMethodsOrProperties(ComboBox cb)
        {
            if(ParentComp == null)
            {
                return;
            }

            // Add Methods
            string[] methodList = U.GetMethodList(ParentComp, typeof(StateMachineEnabled), _listBase.TgtType);
            for (int i = 0; i < methodList.Length; i++)
            {
                string strBase = methodList[i];
                if (_listBase.TgtType != typeof(void))
                {
                    methodList[i] = strBase + ";";
                }
            }
            cb.Items.AddRange(methodList);
        }
    }

    public class IDPath : IDNodeBase
    {
        CompBase _comp = null;
        public CompBase Comp
        {
            get { return _comp; }
            set { _comp = value; }
        }
        
        public IDPath(MPIDCtl mpIDCtl, IDListBase listBase)
            : base(mpIDCtl, listBase)
        {
        }
    }
    class IDMethod : IDNodeBase
    {
        public IDMethod(MPIDCtl mpIDCtl, IDListBase listBase)
            : base(mpIDCtl, listBase)
        {
        }
    }
    public class IDProperty : IDNodeBase
    {
        public Type type = null;
        public IDProperty(MPIDCtl mpIDCtl, IDListBase listBase, Type ty)
            : base(mpIDCtl, listBase)
        {
            type = ty; 
            //int index = ty.Name.LastIndexOf('.');
            //Text = index >= 0 ? ty.Name.Substring(index+1) : ty.Name;
        }
        public override string StoreText
        {
            get
            {
                if (IsLiteral)
                {
                    if (string.IsNullOrEmpty(Text))
                    {
                        Text = U.GetDefaultText(type);
                    }
                    string storeText = base.StoreText;
                    string tyName = type.FullName;
                    if (tyName.StartsWith("System."))
                    {
                        tyName = tyName.Substring(7);
                    }

                    return storeText.Insert(1, string.Format("({0})", tyName));
                }
                return base.StoreText; 
            }
        }
        private bool _literal = false;
        public override bool IsLiteral
        {
            get { return _literal; }
            set { _literal = value; }
                //string id = _mpIDCtl.BuildFullID(ListBase.BuildID());
                //return U.GetPropertyInfo(id, type) == null;
            
        }
    }
}
