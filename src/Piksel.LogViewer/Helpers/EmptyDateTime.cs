using System;
using System.Linq;

namespace Piksel.LogViewer.Helpers
{
    public static class EmptyDateTime
    {
        static readonly char[] TimeChars = new[] { 'd', 'f', 'F', 'g', 'h', 'H', 'K', 'm', 'M', 's', 't', 'y', 'z' };
        static readonly char[] LitChars = new[] { ':', '-', ' ', '/' };

        internal static string ToString(string format, IFormatProvider provider)
            => new string(format.Select(c => TimeChars.Contains(c) ? '?' : (LitChars.Contains(c) ? c : c)).ToArray());
    }
}