using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piksel.LogViewer.Helpers
{
    public static class ListExtensions
    {
        public static bool Set<T>(this List<T> list, T value)
        {
            if(list.Contains(value))
            {
                return true;
            }
            else
            {
                list.Add(value);
                return false;
            }
        }
    }
}
