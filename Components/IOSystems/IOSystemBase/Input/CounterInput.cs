using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

using MCore.Helpers;

namespace MCore.Comp.IOSystem.Input
{
    public class CounterInput : IOChannel
    {
        private TriggerQueue<D.delBool_Int> _triggerQue = null;

        #region Public Properties

        /// <summary>
        /// The trigger queue
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public TriggerQueue<D.delBool_Int> TriggerQue
        {
            get { return _triggerQue; }
            set { _triggerQue = value; }
        }

        /// <summary>
        /// Value of the counter
        /// </summary>
        [Browsable(true)]
        [Category("Counter Input")]
        [Description("Value of the counter")]
        public int Value
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => Value, 0); }
            set { SetPropValue(() => Value, value); }
        }
        #endregion Public Properties

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public CounterInput()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public CounterInput(string name)
            : base(name)
        {
        }
        #endregion Constructors

        public override void Initialize()
        {
            base.Initialize();
            RegisterOnChanged( () => Value, NotifyAll);
        }

        private int _priorTrigger = 0;
        /// <summary>
        /// Get the next callback
        /// </summary>
        /// <returns></returns>
        protected void NotifyAll(int iNewVal)
        {
            if (TriggerQue != null)
            {
                //System.Diagnostics.Debug.WriteLine(string.Format("Counter Change ={0} Prior={1} Queue={2}",
                //    iNewVal, _priorTrigger, TriggerQue.CallbackCount));
                if (iNewVal == 0)
                {
                    _priorTrigger = 0;
                }
                else
                {
                    if (iNewVal <= _priorTrigger)
                    {
                        _priorTrigger = iNewVal - 1;
                    }
                    D.delBool_Int del = null;
                    do
                    {
                        del = TriggerQue.GetNextCallback();
                        if (del != null)
                        {
                            _priorTrigger++;
                            del(_priorTrigger);
                            //System.Diagnostics.Debug.WriteLine(string.Format("Counter Fired {0}",
                            //    _priorTrigger));
                        }
                    } while (_priorTrigger < iNewVal && del != null);
                }
            }
        }
        
        /// <summary>
        /// Increment the counter
        /// </summary>
        [StateMachineEnabled]
        public void Increment()
        {
            Value = Value + 1;
        }
    }
}
