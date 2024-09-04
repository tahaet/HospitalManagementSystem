using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Utility
{
    public static class JwtValues
    {
        public static string Issuer { get; set; } = string.Empty;

        public static string Audience { get; set; } = string.Empty;

        public static string Key { get; set; } = string.Empty;
    }
}
