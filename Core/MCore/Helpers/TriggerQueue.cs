using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MCore.Helpers
{
    public class TriggerQueue<T>
    {
        private Queue<T> _callbackQueue = new Queue<T>();
        private object __queueCallbackLock = new object();

        #region Functions

        /// <summary>
        /// Clear all callbacks
        /// </summary>
        /// <param name="del"></param>
        public void Clear()
        {
            lock (__queueCallbackLock)
            {
                //System.Diagnostics.Debug.WriteLine(string.Format("Clear all CB for {0} [{1}]", typeof(T).Name, _callbackQueue.Count));                
                _callbackQueue.Clear();
            }
        }

        /// <summary>
        /// Add this callback to the queue
        /// </summary>
        /// <param name="del"></param>
        public void AddtoCallbackQueue(T del)
        {
            lock (__queueCallbackLock)
            {
                _callbackQueue.Enqueue(del);
                //System.Diagnostics.Debug.WriteLine(string.Format("Add CB to {0} [{1}]", typeof(T).Name, _callbackQueue.Count));
            }
        }
        public int CallbackCount
        {
            get
            { 
                lock (__queueCallbackLock)
                {
                    return _callbackQueue.Count;
                }
            }
        }

        public T GetNextCallback()
        {
            T del = default(T);
            lock (__queueCallbackLock)
            {
                if (_callbackQueue.Count > 0)
                {
                    del = _callbackQueue.Dequeue();
                    //System.Diagnostics.Debug.WriteLine(string.Format("Remove CB from {0} [{1}]", typeof(T).Name, _callbackQueue.Count));
                }
            }
            return del;
        }


        #endregion Functions
    }
}
