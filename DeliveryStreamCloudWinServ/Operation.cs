// 2014.01.03 FSWW, Ramesh M Added For CR#61563
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using DeliveryStreamCloudWinServ.DataAccess;
using System.IO;
using System.Reflection;

namespace DeliveryStreamCloudWinServ
{
    public class Operation : ServiceBases
    {
        /// <summary>
        /// Auto log-off on web.
        /// </summary>
        public static String VersionNo = "";
        public static void AutoLogOut()
        {
            VersionNo = Assembly.GetEntryAssembly().GetName().Version.ToString().Replace(".0", "");
            //AutoLog Off
            ISession session = null;
            try
            {
                //session = GetNewSession();
                //double logOutTime = 0;
                //double.TryParse(ConfigurationManager.AppSettings["LogOutAfterMinutes"], out logOutTime);
                //// Calculate Driver Dots for this session  == Need to write calculation procedure
                //DALMethods.AutoLogOut(logOutTime, session, VersionNo);
                //CloseSession(session);
            }
            catch (Exception ex)
            {
                CloseSession(session);
                Logging.LogError(ex);
            }
            finally
            {
                if (session != null)
                {
                    CloseSession(session);
                }
            }
        }

        /// <summary>
        /// Modify the GPS history table to add STATE column. 
        /// Do reverse geo-coding to get State and  update the column in GPS history.
        /// </summary>
        public static void UpdateGPSHistoryState()
        {
            VersionNo = Assembly.GetEntryAssembly().GetName().Version.ToString().Replace(".0", "");
            ISession session = null;
            try
            {
                session = GetNewSession();

                // 2014.01.03 FSWW, Ramesh M Added For CR#61563
                DALMethods.UpdateStateCodeInSQL(session, VersionNo);

                // 2014.01.03 FSWW, Ramesh M Commented  For CR#61563
                #region StateCodeUpdation Using Google API
                //XmlDocument doc = new XmlDocument();
                //string latitude = string.Empty;
                //string longitude = string.Empty;
                //// 2013.12.09 FSWW, Ramesh M Added For CR#60647
                //string sVehicleID = string.Empty;
                //string sCompanyID = string.Empty;
                //string sSessionID = string.Empty;

                //List<DAL.GPSHistoryRow> lstGPSHistory = DALMethods.GPSHistoryData(session);
                //foreach (DAL.GPSHistoryRow GPSHistory in lstGPSHistory)
                //{
                //    // 2013.12.09 FSWW, Ramesh M Added For CR#60647
                //    List<DAL.LoginHistoryRow> lstLoginHistory = DALMethods.GetLoginHistoryDetails(GPSHistory.SessionID, session);
                //    foreach (DAL.LoginHistoryRow LoginHistory in lstLoginHistory)
                //    {
                //        sVehicleID = LoginHistory.VehicleID.ToString();
                //        sCompanyID = LoginHistory.CustomerID.ToString();
                //        sSessionID = LoginHistory.SessionID.ToString();
                //    }

                //    string state = "";
                //    if (GPSHistory.Latitude == latitude && GPSHistory.Longitude == longitude)
                //    {
                //        continue;
                //    }
                //    else
                //    {
                //        latitude = GPSHistory.Latitude;
                //        longitude = GPSHistory.Longitude;
                       
                //        try
                //        {
                //            doc.Load("http://maps.googleapis.com/maps/api/geocode/xml?latlng=" + latitude + "," + longitude + "&sensor=false");
                //            XmlNode element = doc.SelectSingleNode("//GeocodeResponse/status");
                //            if (element == null)
                //            {
                //                continue;
                //            }
                //            if (element.InnerText == "ZERO_RESULTS")
                //            {
                //                // 2013.12.09 FSWW, Ramesh M Added For CR#60647 modified error log message added,VehicleId,CustomerId,SessionID
                //                throw new Exception("No data available for the specified location; latitude,longitude (" + latitude + "," + longitude + "), CompanyID-" + sCompanyID + ", VehicleID-" + sVehicleID + ", SessionID" + sSessionID);
                //            }
                //            else
                //            {
                //                element = doc.SelectSingleNode("//GeocodeResponse/result/formatted_address");
                //                XmlNode xNode = doc.SelectNodes("//GeocodeResponse/result/address_component[type='administrative_area_level_1']")[0];
                //                if (xNode == null)
                //                {
                //                    xNode = doc.SelectNodes("//GeocodeResponse/result/address_component[type='country']")[0];
                //                    if (xNode == null)
                //                    {
                //                        continue;
                //                    }
                //                }

                //                if (xNode["short_name"] != null)
                //                {
                //                    state = xNode["short_name"].InnerText;
                //                }

                //                if (state.Length > 2)
                //                {
                //                    try
                //                    {
                //                        string path = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
                //                        doc.Load(path + "//States.xml");
                //                        xNode = doc.SelectSingleNode("//States/State[key='" + state + "']");
                //                        if (xNode != null)
                //                        {
                //                            if (!String.IsNullOrEmpty(xNode["code"].InnerText))
                //                            {
                //                                state = xNode["code"].InnerText;
                //                            }
                //                        }
                //                    }
                //                    catch (Exception ex)
                //                    {
                //                        //
                //                    }
                //                }
                //            }

                //            if (!string.IsNullOrEmpty(state))
                //            {
                //                //update Table
                //                DALMethods.UpdateState(state, longitude, latitude, session);
                //            }
                //        }
                //        catch (Exception ex)
                //        {
                //            Logging.LogError(ex);
                //        }
                       
                //    }
                //}
                #endregion
            }
            catch (Exception ex)
            {

                Logging.LogError(ex);
            }
            finally
            {
                if (session != null)
                {
                    CloseSession(session);
                }
            }
        }

        public static void CalculateDriverStatus()
        {
            //TODO : Calculate Driver Status & Store into the table
        }
    }
}
