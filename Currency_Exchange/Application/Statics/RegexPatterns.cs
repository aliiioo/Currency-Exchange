using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Statics
{
    public static class RegexPatterns
    {
        public static readonly Regex EmailPatternRegex = new Regex(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}");
        public static readonly Regex PhonePatternRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
    }
}
