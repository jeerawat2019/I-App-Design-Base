using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

using MCore.Comp;

using MCore.Comp.SMLib.Path;

namespace MCore.Comp.SMLib.Flow
{
    public abstract class SMFlowBase : CompBase
    {

        public delegate void DelHightedOnChange(bool hightligted);
        public event DelHightedOnChange evHighlightedOnChaged = null;


        #region privates

        private SMPathOut _incomingPath = null;

        #endregion privates

        #region Serialize properties

        /// <summary>
        /// Clockwise direction
        /// </summary>
        public enum eDir { Up, Right, Down, Left, Num };



        /// <summary>
        /// Returns true if is vertical
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static bool IsVertical(eDir dir)
        {
            return dir == eDir.Up || dir == eDir.Down;
        }

        [XmlArrayItem(Type = typeof(SMPathOutPlug), ElementName = "PathOutPlug")]
        [XmlArrayItem(Type = typeof(SMPathOut), ElementName = "PathOut")]
        [XmlArrayItem(Type = typeof(SMPathOutStop), ElementName = "PathOutStop")]
        [XmlArrayItem(Type = typeof(SMPathOutError), ElementName = "PathOutError")]
        [XmlArrayItem(Type = typeof(SMPathOutBool), ElementName = "PathOutBool")]
        public SMPathOut[] PathArray
        {
            get
            {
                return _pathList.ToArray();
            }
            set
            {
                _pathList = value.ToList();
            }
        }

        protected List<SMPathOut> _pathList = new List<SMPathOut>(4) { null, null, null, null };

        /// <summary>
        /// Location of this item (Grid units)
        /// </summary>
        public PointF GridLoc 
        {
            get { return GetPropValue(() => GridLoc, Point.Empty); }
            set { SetPropValue(() => GridLoc, value); }
        }

        /// <summary>
        ///  The Text displayed for this Flow Item
        ///  Not used for SMStateMachine
        /// </summary>
        public string Text
        {
            get { return GetPropValue(() => Text, string.Empty); }
            set { SetPropValue(() => Text, value); }
        }
        #endregion Serialize properties

        #region public properties

        public string BestDisplayText
        {
            get
            {
                if (!string.IsNullOrEmpty(Text))
                {
                    return Text;
                }
                return Name;
            }
        }
        /// <summary>
        /// Get the Incoming Path
        /// </summary>
        [XmlIgnore]
        [Browsable(false)]
        public SMPathOut IncomingPath
        {
            get { return _incomingPath; }
            set { _incomingPath = value; }
        }

        /// <summary>
        /// Get the path element
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        [XmlIgnore]
        public SMPathOut this[eDir dir]
        {
            get
            {
                return _pathList[(int)dir];
            }
            set
            {
                _pathList[(int)dir] = value;
            }
        }
        /// <summary>
        /// Get the dir of the PathOut element
        /// </summary>
        /// <param name="pathOutCheck"></param>
        /// <returns></returns>
        [XmlIgnore]
        public eDir this[SMPathOut pathOutCheck]
        {
            get
            {
                for (eDir dir = eDir.Up; dir < eDir.Num; dir++)
                {
                    if (object.ReferenceEquals(this[dir], pathOutCheck))
                    {
                        return dir;
                    }
                }
                return eDir.Num;
            }
         }
        /// <summary>
        /// Get the SMPathOut of the specified type
        /// </summary>
        /// <param name="pathOutCheck"></param>
        /// <returns></returns>
        [XmlIgnore]
        public SMPathOut this[Type ty]
        {
            get
            {
                return _pathList.Find(c => c.GetType().Name == ty.Name);
            }
        }
        /// <summary>
        /// Get the path Id using Text as much as possible
        /// </summary>
        public string PathText
        {
            get
            {
                string text = string.Empty;
                CompBase comp = this;
                while (comp is SMFlowBase)
                {
                    string thisText = (comp as SMFlowBase).Text;
                    if (string.IsNullOrEmpty(thisText))
                    {
                        thisText = comp.Name;
                    }
                    text = text.Insert(0, "." + thisText);
                    comp = comp.Parent;
                }
                return text.Insert(0, comp.Nickname);
                
            }
        }
        private bool _highlighted = false;
        /// <summary>
        /// Returns true if the flow item is highlighted because it is currently active during run of state machine
        /// </summary>
        [XmlIgnore]
        public bool Highlighted
        {
            get
            {
                return _highlighted;
            }
            set
            {
                _highlighted = value;
                if(evHighlightedOnChaged != null)
                {
                    evHighlightedOnChaged(value);
                }
            }
        }

        /// <summary>
        /// Get the Parent Object
        /// </summary>
        [XmlIgnore]
        public SMFlowContainer ParentContainer
        {
            get
            {
                return Parent as SMFlowContainer; 
            }
            set
            {
                Parent = value;
            }
        }


        private SMStateMachine _stateMachine = null;

        /// <summary>
        /// Get the Parent Object
        /// </summary>
        [XmlIgnore]
        public SMStateMachine StateMachine
        {
            get
            {
                CompBase comp = this;
                do
                {
                    if (comp is SMStateMachine)
                    {
                        _stateMachine = comp as SMStateMachine;
                        return _stateMachine;
                    }
                    comp = comp.Parent;
                }
                while (comp != null);
                if(_stateMachine != null)
                {
                    return _stateMachine;
                }
                throw new Exception("Could not locate the State Machine object");
            }
            set
            {
                _stateMachine = value;
            }
        }
        #endregion public properties

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SMFlowBase()
        {
        }

        /// <summary>
        /// Dynamic Creation Constructor for SMStateMachine
        /// </summary>
        /// <param name="name"></param>
        public SMFlowBase(string name)
            : base(name)
        {
        }

        #endregion Constructors

        /// <summary>
        /// Find the Flow Item at the specified end point
        /// </summary>
        /// <param name="gridLoc"></param>
        /// <returns></returns>
        public SMFlowBase FindFlowItem(PointF gridLoc)
        {
            try
            {
                if (Count > 0)
                {
                    foreach (SMFlowBase flowItem in ChildArray)
                    {
                        if (flowItem.GridLoc.Equals(gridLoc))
                        {
                            return flowItem;
                        }
                    }
                }
            }
            catch
            {
            }
            return null;
        }

        #region Overrides
        public override void Initialize()
        {
            base.Initialize();
            this[eDir.Up].Initialize(this, true);
            this[eDir.Down].Initialize(this, true);
            this[eDir.Left].Initialize(this, false);
            this[eDir.Right].Initialize(this, false);
 //           _pathList.Where(c => !(c is SMPathOutPlug)).ToList().ForEach(c => c.Initialize());
        }


        /// <summary>
        /// Copy certain non-unique properties
        /// </summary>
        /// <param name="compTo"></param>
        public override void ShallowCopyTo(CompBase compTo)
        {
            base.ShallowCopyTo(compTo);
            _shallowCopyTo(compTo);
        }

        private void _shallowCopyTo(CompBase compTo)
        {
            SMFlowBase flowTo = compTo as SMFlowBase;
            flowTo.Text = Text;
            flowTo.GridLoc = GridLoc;

            for (int i = 0; i < 4; i++)
            {
                flowTo._pathList[i] = _pathList[i].Clone() as SMPathOut;
            }
        }
        /// <summary>
        /// Clone this component
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bRecursivley"></param>
        /// <returns></returns>
        public override CompBase Clone(string name, bool bRecursivley)
        {
            SMFlowBase newComp = base.Clone(name, bRecursivley) as SMFlowBase;
            _shallowCopyTo(newComp);
            return newComp;
        }

        #endregion Overrides

        /// <summary>
        /// Rebuild this Flow Item
        /// </summary>
        public virtual void Rebuild() { }

        /// <summary>
        /// Check if the scope is doable
        /// </summary>
        /// <param name="proposedScope"></param>
        /// <returns></returns>
        public virtual string ScopeCheck(string proposedScope)
        {
            return proposedScope;
        }


        /// <summary>
        /// Returns true if the grid pt is close to this Grid Location
        /// </summary>
        /// <param name="gridPt"></param>
        /// <returns></returns>
        public bool Contains(PointF gridPt)
        {
            float x = Math.Abs(gridPt.X - GridLoc.X);
            float y =  Math.Abs(gridPt.Y - GridLoc.Y);

            return x < 0.4f && y < 0.25f;
        }
        /// <summary>
        /// Evaluate specific path for target
        /// </summary>
        /// <param name="pathOut"></param>
        public void DetermineTarget(SMPathOut pathOut)
        {
            if (!(pathOut is SMPathOutPlug))
            {
                // Allow us to loop back to self only if there is more than one segment
                SMFlowBase searcher = pathOut.Next != null ? this : null;
                SMFlowBase flowItem = ParentContainer.FindTarget(searcher, FindEndPoint(pathOut));
                if (flowItem != null)
                {
                    pathOut.TargetID = flowItem.Name;
                }
                else
                {
                    pathOut.TargetID = string.Empty;
                }
            }
        }

        #region Virtuals

        /// <summary>
        /// Runs this flow item
        /// </summary>
        /// <returns>Returns the path to take from here</returns>
        public virtual SMPathOut Run()
        {
            return null;
        }

        /// <summary>
        /// Evaluate all the paths for targets
        /// </summary>
        public virtual void DetermineAllPathTargets()
        {
            DetermineTarget(this[eDir.Up]);
            DetermineTarget(this[eDir.Down]);
            DetermineTarget(this[eDir.Left]);
            DetermineTarget(this[eDir.Right]);
        }
        #endregion Virtuals

        /// <summary>
        /// Get the end point of a path
        /// </summary>
        /// <param name="pathOut"></param>
        /// <returns></returns>
        public PointF FindEndPoint(SMPathOut pathOut)
        {
            return pathOut.FindEndPoint(GridLoc);
        }
        public void ChangeExit(SMPathOut pathOut, int x, int y)
        {
            int directionVal = 0;
            eDir dirThis = this[pathOut];
            switch (dirThis)
            {
                case eDir.Num:
                    return;
                case eDir.Up:
                    directionVal = Math.Sign(x);
                    break;
                case eDir.Down:
                    directionVal = -Math.Sign(x);
                    break;
                case eDir.Right:
                    directionVal = Math.Sign(y);
                    break;
                case eDir.Left:
                    directionVal = -Math.Sign(y);
                    break;
            }
            if (directionVal == 0)
                return;
           // Loop to find next or previous empty spot
            eDir dirSwap = dirThis;
            do
            {
                if (directionVal > 0)
                {
                    dirSwap++;
                    if (dirSwap == eDir.Num)
                    {
                        dirSwap = eDir.Up;
                    }
                }
                else
                {
                    if (dirSwap == eDir.Up)
                    {
                        dirSwap = eDir.Left;
                    }
                    else
                    {
                        dirSwap--;
                    }
                }
                if (this[dirSwap] is SMPathOutPlug)
                {
                    // Got it
                    this[dirThis] = this[dirSwap];
                    this[dirSwap] = pathOut;
                    switch (dirSwap)
                    {
                        case eDir.Up:
                            pathOut.GridDistance = -Math.Abs(pathOut.GridDistance);
                            break;
                        case eDir.Down:
                            pathOut.GridDistance = Math.Abs(pathOut.GridDistance);
                            break;
                        case eDir.Right:
                            pathOut.GridDistance = Math.Abs(pathOut.GridDistance);
                            break;
                        case eDir.Left:
                            pathOut.GridDistance = -Math.Abs(pathOut.GridDistance);
                            break;
                    }
                    pathOut.Initialize(this, IsVertical(dirSwap));
                    return;
                }
            } while (dirSwap != dirThis);
        }
    }
}
