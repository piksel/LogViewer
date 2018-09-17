using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piksel.LogViewer.Helpers
{
    public static class ArrayExtensions
    {
        public static T? FirstOrNull<T>(this T[] array) where T: struct
            => array.Length > 0 ? array[0] : (T?)null;
    }
}
