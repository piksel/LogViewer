using Nett.Coma;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Piksel.LogViewer.Helpers
{
    public static class ConfigExtensions
    {
        public static bool Toggle<T>(this Config<T> config, Expression<Func<T, bool>> func) where T: class
        {
            var newValue = !config.Get(func);
            config.Set(func, newValue);
            return newValue;
        }

        public static void Update<T, TValue>(this Config<T> config, Expression<Func<T, TValue>> select, Action<TValue> update) 
            where T : class 
            where TValue : class
        {
            var property = config.Get(select);
            update(property);
            config.Set(select, property);
        }
    }
}
