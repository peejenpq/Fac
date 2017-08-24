using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TIROTLibrary.SysCommon.Utility
{
    public static class SysConvert
    {
        /// <summary>
        /// Convert MAC Address by remove '-' or ':' 
        /// </summary>
        /// <param name="inMAC">12-34-56-78-90-AB</param>
        /// <returns>1234567890AB</returns>
        public static string MAC17ToMAC12Digit(string inMAC)
        {
            string outMac = inMAC.Replace("-", string.Empty);
            outMac = outMac.Replace(":", string.Empty);
            return outMac;
        }

        /// <summary>
        /// Convert Time Less then 23:59 to int.(Range 0 - 1440)
        /// </summary>
        /// <param name="inTime"></param>
        /// <returns></returns>
        public static int HMTimeSpan2Int(TimeSpan inTime)
        {
            // ----- Accept only time less than 23:59
            if (inTime.Hours > 23) return -1;
            if (inTime.Minutes > 59) return -1;
            
            return (inTime.Hours * 60) + inTime.Minutes;
        }

    }
}
