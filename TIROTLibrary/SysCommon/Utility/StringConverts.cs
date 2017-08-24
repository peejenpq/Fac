using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TIROTLibrary.SysCommon.Utility
{
    public static class StringConverts
    {

        public static string MAC17ToMAC12Digit(string inMAC)
        {
            string outMac = inMAC.Replace("-", string.Empty);
            outMac = outMac.Replace(":", string.Empty);
            return outMac;
        }
    }
}
