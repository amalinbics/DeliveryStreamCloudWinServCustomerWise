using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeliveryStreamCloudWinServ
{
    /// <summary>
    /// ApplicationConstants class
    /// </summary>
    public static class ApplicationConstants
    {
        public struct Connection
        {
            public const String ConnectionString = "DeliveryStreamCloud";
        }

        public struct Errors
        {
            public const String FunctionError = "Error occured. Function : in {0}, Class : {1}, Error Message : {2} Stack Trace : {3}";
            public const String ConnectionString = "Could not find {0} connection string in config file";

        }

        public struct Logging
        {
            public const String Log = "DeliveryStream";
            public const String Source = "CloudWinServ";
        }

         public struct Status
         {
             public const String NotReassigned = "N";
             public const String Redispatched = "M";
         }
    }
}
