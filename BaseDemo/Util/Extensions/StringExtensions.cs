using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BaseDemo.Util.Extensions
{
    public static class StringExtensions
    {
        //public static int NthIndexOf(this string target, string value, int n)
        //{
        //    target.IndexOfAny
        //}

        private static int NumOfVal(string target, string value)
        {
            Regex rege = new Regex(value, RegexOptions.Compiled);
            return rege.Matches(target).Count;
        }

        //public static int NthLastIndexOf(this string target, string value, int n)
        //{
        //    var num = NumOfVal(target, value);
        //    return NthIndexOf(target, value, num - n +1);
        //}
    }
}
