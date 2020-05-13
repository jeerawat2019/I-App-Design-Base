using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MCore.Lists
{
    public class SortableList<T> : List<T>
    {
        Dictionary<string, bool> sortOrder = new Dictionary<string, bool>();
        string _curProperty = string.Empty;
        public void SortByProperty(string propertyName)
        {
            _curProperty = propertyName;
            if (sortOrder.ContainsKey(propertyName))
            {
                sortOrder[propertyName] = !sortOrder[propertyName];
            }
            else
            {
                sortOrder.Add(propertyName, true);
            }
            Sort(CompareByProperty);
        }
        public int CompareByProperty(T row1, T row2)
        {
            PropertyInfo pi = typeof(T).GetProperty(_curProperty);
            object field1 = pi.GetValue(row1, null);
            object field2 = pi.GetValue(row2, null);
            IComparable comparable1 = field1 as IComparable;
            IComparable comparable2 = field2 as IComparable;
            if (comparable1 == null || comparable2 == null)
                return 1;
            if (sortOrder[_curProperty])
            {
                return comparable1.CompareTo(comparable2);
            }
            return comparable2.CompareTo(comparable1);
        }
    }
}
