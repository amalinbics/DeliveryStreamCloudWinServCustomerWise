using System;
using System.Configuration;

namespace DeliveryStreamCloudWinServ
{
    public static class GetCofig
    {
        public static double AutoLogOut_Interval()
        {
            double rtn;
            if (double.TryParse(ConfigurationManager.AppSettings["AutoLogOutInterval"], out rtn))
            {
                return rtn;
            }
            else
            {
                throw new ApplicationException("Unable to determine timerInterval");
            }
        }

        public static double UpdateState_Interval()
        {
            double rtn;
            if (double.TryParse(ConfigurationManager.AppSettings["SetStateInterval"], out rtn))
            {
                return rtn;
            }
            else
            {
                throw new ApplicationException("Unable to determine SetStateInterval");
            }
        }

        public static double CalcualateStatus_Interval()
        {
            double rtn;
            if (double.TryParse(ConfigurationManager.AppSettings["CalcualateStatusInterval"], out rtn))
            {
                return rtn;
            }
            else
            {
                throw new ApplicationException("Unable to determine CalcualateStatusInterval");
            }
        }
    }
}
