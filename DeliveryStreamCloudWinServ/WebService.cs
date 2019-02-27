// 2014.01.06 FSWW, Ramesh M Added VersionNo as input parameter in all methods For Versioning handling
// 2014.03.18  Ramesh M Added For CR#62719 added  TrailerCode to go back to ascend

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;
using System.Configuration;
using System.Diagnostics;
using DeliveryStreamCloudWinServ.DataAccess;
using System.ServiceModel;
using DeliveryStreamCloudWinServ.Customer;
using System.Net;
using System.Reflection;
using System.Data;
using System.Net.Mail;

namespace DeliveryStreamCloudWinServ
{
    /// <summary>
    /// WebServices class
    /// </summary>
    public class WebServices : ServiceBases
    {
        public static String VersionNo = "";

        private static Int32 ShipmentRetryCount
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["ShipmentRetryCount"]);
                }
                catch
                {
                    return 0;
                }
            }
        }

        private static Int32 BOLHdrRetryCount
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["BOLHdrRetryCount"]);
                }
                catch
                {
                    return 0;
                }
            }
        }

        private static Int32 DeliveryRetryCount
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["DeliveryRetryCount"]);
                }
                catch
                {
                    return 0;
                }
            }
        }

        //private static String CustomerID
        //{
        //    get
        //    {
        //        try
        //        {
        //            return MergeCustomerID(ConfigurationManager.AppSettings["CustomerID"]);
        //        }
        //        catch
        //        {
        //            return ConfigurationManager.AppSettings["CustomerID"];
        //        }
        //    }
        //}

        private static string CustomerID = ConfigurationManager.AppSettings["CustomerID"].ToString();
        private static string EMAIL_SERVER = ConfigurationManager.AppSettings["EMAILSERVER"].ToString();
        private static string EMAIL_NAME = ConfigurationManager.AppSettings["EMAILNAME"].ToString();
        private static string EMAIL_PASSWORD = ConfigurationManager.AppSettings["EMAILPASSWORD"].ToString();
        private static string WCFUrl;
        private static string Password;
        /// <summary>
        /// CallWebService
        /// Function to Call Web Service
        /// This function reads Customer name,password and vehicle id from aap setting 
        /// </summary>
        /// <param name="endpointConfigName">End point configuration name</param>
        public static void CallWebService()
        {
            try
            {
                VersionNo = Assembly.GetEntryAssembly().GetName().Version.ToString().Replace(".0", "");

                string customerID = "";

                string[] sptcutomerID = CustomerID.Split(',');

                for (int i = 0; i < sptcutomerID.Length; i++)
                {
                    customerID = sptcutomerID[i].Trim();

                    if (IsWCFAvailableForCustomer(customerID))
                    {
                        //Logging.WriteLog("WCFUrl working for Customer" + " " + customerID, EventLogEntryType.Error);

                        try
                        {
                            UpdateLoad(customerID);
                        }
                        catch (Exception ex)
                        {
                            Logging.LogError(ex);
                        }
                        try
                        {
                            UpdateLoadStatus(customerID);
                        }
                        catch (Exception ex)
                        {
                            Logging.LogError(ex);
                        }
                        try
                        {
                            UpdateOrderStatus(customerID);
                        }
                        catch (Exception ex)
                        {
                            Logging.LogError(ex);
                        }
                        try
                        {
                            UpdateShipmentDetails(customerID);
                        }
                        catch (Exception ex)
                        {
                            Logging.LogError(ex);
                        }
                        try
                        {
                            UpdateDeliveryDetails(customerID);
                        }
                        catch (Exception ex)
                        {
                            Logging.LogError(ex);
                        }
                        //try
                        //{
                        //    UpdateEODInventoryDetails();
                        //}
                        //catch (Exception ex)
                        //{
                        //    Logging.LogError(ex);
                        //}
                        try
                        {
                            UpdateBOLImages(customerID);
                        }
                        catch (Exception ex)
                        {
                            Logging.LogError(ex);
                        }
                        try
                        {
                            SendEmailforRejectedLoads(customerID);
                        }
                        catch (Exception ex)
                        {
                            Logging.LogError(ex);
                        }
                        try
                        {
                            UpdatePONo(customerID);
                        }
                        catch (Exception ex)
                        {
                            Logging.LogError(ex);
                        }
                        try
                        {
                            UpdateRejectedLoads(customerID);
                        }
                        catch (Exception ex)
                        {
                            Logging.LogError(ex);
                        }

                        try
                        {
                            UpdateDeliveryNotes(customerID);
                        }
                        catch (Exception ex)
                        {
                            Logging.LogError(ex);
                        }

                        //UpdateLoad();
                        //UpdateLoadStatus();
                        //UpdateOrderStatus();
                        //UpdateShipmentDetails();
                        //UpdateDeliveryDetails();
                        //UpdateEODInventoryDetails();
                        //UpdateBOLImages();
                        //SendEmailforRejectedLoads();
                        //UpdatePONo();
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        /// <summary>
        /// UpdateLoad
        /// Function to update load
        /// </summary>
        public static void UpdateLoad(string customerID)
        {
            ISession session = null;
            try
            {
                session = GetNewSession();

                DataTable dtLoads = DALMethods.GetUpdatedLoadForCustomer(customerID, session, VersionNo);
                if (dtLoads != null && dtLoads.Rows.Count > 0)
                {
                    for (int i = 0; i < dtLoads.Rows.Count; i++)
                    {
                        try
                        {
                            DAL.CustomerRow cust = DALMethods.GetCustomer(dtLoads.Rows[i]["CustomerID"].ToString().Trim(), session, VersionNo);

                            //if (String.IsNullOrEmpty(cust.WCFURL_Ext))
                            //{
                            //    throw new ApplicationException(String.Format("No WCF URL set for customer {0}.", dtLoads.Rows[i]["CustomerID"].ToString().Trim()));
                            //}
                            //else
                            //{
                            CustomerServiceClient client = GetServiceClient(WCFUrl, VersionNo);
                            client.UpdateLoad(cust.CustomerID_Ext, cust.Password_Ext, dtLoads.Rows[i]["LoadNo"].ToString().Trim(), Convert.ToInt32(dtLoads.Rows[i]["VehicleID"].ToString().Trim()), Convert.ToInt32(dtLoads.Rows[i]["DriverID"].ToString().Trim()), VersionNo);
                            DALMethods.UpdateLoad(Guid.Parse(dtLoads.Rows[i]["ID"].ToString().Trim()), session, VersionNo);
                            //}
                        }
                        catch (Exception ex)
                        {
                            Logging.WriteLog(String.Format("Error for Customer customer {0} and LoadID {1}.", dtLoads.Rows[i]["CustomerID"].ToString().Trim(), dtLoads.Rows[i]["ID"].ToString().Trim()), EventLogEntryType.Error);
                            Logging.LogError(ex);
                        }
                    }
                }
                CloseSession(session);
            }
            catch (Exception ex)
            {
                CloseSession(session);
                Logging.LogError(ex);
            }
            finally
            {
                if (session != null)
                    CloseSession(session);
            }
        }

        /// <summary>
        /// UpdateLoadStatus
        /// Function to update load status
        /// </summary>
        public static void UpdateLoadStatus(string customerID)
        {
            ISession session = null;
            try
            {
                session = GetNewSession();
                //TimeSpan.FromMinutes((double)Convert.ToInt32(ConfigurationManager.AppSettings["Interval"]));
                List<DAL.LoadStatusHistoryRow> rows = DALMethods.GetUpdatedLoadStatusHistory(customerID, session, VersionNo);
                List<DAL.LoadRow> lstLoad = DALMethods.GetUndispatchedLoad(customerID, session, VersionNo);

                if (lstLoad != null && lstLoad.Count > 0)
                {
                    Logging.WriteToFile1("GetUndispatchedLoad");
                    lstLoad.Sort((x, y) => string.Compare(x.CustomerID_Ext, y.CustomerID_Ext));
                    foreach (DAL.LoadRow load in lstLoad)
                    {
                        try
                        {
                            //DAL.CustomerRow cust = DALMethods.GetCustomer(load.CustomerID_Ext, session, VersionNo);

                            //if (String.IsNullOrEmpty(cust.WCFURL_Ext))
                            //{
                            //    throw new ApplicationException(String.Format("No WCF URL set for customer {0}.", load.CustomerID_Ext));
                            //}
                            //else
                            //{
                            Logging.WriteToFile1("GetUndispatchedLoad WCFURL:" + WCFUrl + " CustomerID" + customerID);
                            CustomerServiceClient client = GetServiceClient(WCFUrl, VersionNo);
                            string status = "";
                            if (load.Undispatched == 0)
                            {
                                status = ApplicationConstants.Status.NotReassigned;
                            }
                            else
                            {
                                status = ApplicationConstants.Status.Redispatched;

                                if (rows != null && rows.Count > 0)
                                {
                                    DAL.LoadStatusHistoryRow StatusHistoryRow = rows.Find(item => item.LoadNo_Ext == load.LoadNo_Ext && item.CustomerID_Ext == load.CustomerID_Ext);
                                    rows.Remove(StatusHistoryRow);
                                }
                            }

                            Logging.WriteToFile1("GetUndispatchedLoad beforeupdateloadstatus LoadNo: " + load.LoadNo_Ext);
                            client.UpdateLoadStatus(customerID, Password, load.LoadNo_Ext, status, VersionNo);
                            Logging.WriteToFile1("GetUndispatchedLoad afterupdateloadstatus LoadNo: " + load.LoadNo_Ext);

                            if (load.Undispatched == 1)
                            {
                                //Delete load from cloud DB if Load is Undispatched
                                DALMethods.DeleteLoad(load.Id_Ext, session, VersionNo);
                            }
                            else
                            {
                                DALMethods.UpdateLoadNeedUpdate(load.Id_Ext, session, VersionNo);
                            }
                            //}
                        }
                        catch (Exception ex)
                        {
                            Logging.WriteLog(String.Format("Error for Customer customer {0} , LoadNumber {1}.", load.CustomerID_Ext, load.LoadNo_Ext), EventLogEntryType.Error);
                            Logging.LogError(ex);
                        }
                    }
                }

                if (rows != null && rows.Count > 0)
                {
                    //rows.Sort((x, y) => string.Compare(x.CustomerID_Ext, y.CustomerID_Ext));
                    Logging.WriteToFile1("GetUpdatedLoadStatusHistory");
                    foreach (DAL.LoadStatusHistoryRow row in rows)
                    {
                        try
                        {
                            //DAL.CustomerRow cust = DALMethods.GetCustomer(row.CustomerID_Ext, session, VersionNo);

                            //if (String.IsNullOrEmpty(cust.WCFURL_Ext))
                            //{
                            //    throw new ApplicationException(String.Format("No WCF URL set for customer {0}.", row.CustomerID_Ext));
                            //}
                            //else
                            //{
                            Logging.WriteToFile1("GetUpdatedLoadStatusHistory WCFURL:" + WCFUrl + " CustomerID" + customerID);
                            CustomerServiceClient client = GetServiceClient(WCFUrl, VersionNo);
                            Logging.WriteToFile1("GetUpdatedLoadStatusHistory beforeupdateloadstatus LoadNo: " + row.LoadNo_Ext);
                            client.UpdateLoadStatus(customerID, Password, row.LoadNo_Ext, row.LoadStatusID_Ext, VersionNo);
                            Logging.WriteToFile1("GetUpdatedLoadStatusHistory afterupdateloadstatus LoadNo: " + row.LoadNo_Ext);

                            //On Sucess
                            Logging.WriteToFile1("GetUpdatedLoadStatusHistory beforeupdateloadstatushistory");
                            DALMethods.UpdateLoadStatusHistory(row.LoadID_Ext, row.LoadStatusID_Ext, session, VersionNo);
                            Logging.WriteToFile1("GetUpdatedLoadStatusHistory afterupdateloadstatushistory");

                            string IsEnabledFrtBrk = DALMethods.IsEnabledFrkBrkdown(customerID, session, VersionNo);
                            if (IsEnabledFrtBrk.ToUpper().Trim() == "TRUE")
                            {
                                if (row.LoadStatusID_Ext.ToUpper().Trim() == "L" || row.LoadStatusID_Ext.ToUpper().Trim() == "U")
                                {
                                    List<DAL.Cloud_GetMilesByLoadRow> lstGetMilesByLoad = DALMethods.GetFreightBreakdownDetails(customerID, row.LoadNo_Ext, session, VersionNo);
                                    foreach (DAL.Cloud_GetMilesByLoadRow LRow in lstGetMilesByLoad)
                                    {
                                        client.UpdateFreightBreakdown(customerID, Password, LRow.SysTrxNo, 0, Convert.ToChar(LRow.Status), LRow.OriginCity, LRow.DestCity, LRow.OriginState, LRow.DestState, 0, LRow.TotalMiles, Convert.ToDecimal(LRow.OriginLat), Convert.ToDecimal(LRow.OriginLong), Convert.ToDecimal(LRow.DestLat), Convert.ToDecimal(LRow.DestLong), VersionNo);
                                    }
                                }
                            }

                            //}


                        }
                        catch (Exception ex)
                        {
                            Logging.WriteLog(String.Format("Error for Customer customer {0} , LoadID {1} and Status {2}.", row.CustomerID_Ext, row.LoadID_Ext, row.LoadStatusID_Ext), EventLogEntryType.Error);
                            Logging.LogError(ex);
                        }
                    }
                }
                CloseSession(session);
            }
            catch (Exception ex)
            {
                CloseSession(session);
                Logging.LogError(ex);
            }
            finally
            {
                if (session != null)
                    CloseSession(session);
            }
        }

        /// <summary>
        /// UpdateOrderStatus
        /// Function to update order status
        /// </summary>
        public static void UpdateOrderStatus(string customerID)
        {
            ISession session = null;
            try
            {
                session = GetNewSession();
                List<DAL.OrderStatusHistoryRow> rows = DALMethods.GetUpdatedOrderStatusHistory(customerID, session, VersionNo);
                if (rows != null && rows.Count > 0)
                {
                    //rows.Sort((x, y) => string.Compare(x.CustomerID_Ext, y.CustomerID_Ext));
                    foreach (DAL.OrderStatusHistoryRow row in rows)
                    {
                        try
                        {
                            //DAL.CustomerRow cust = DALMethods.GetCustomer(row.CustomerID_Ext, session, VersionNo);

                            //if (String.IsNullOrEmpty(cust.WCFURL_Ext))
                            //{
                            //    throw new ApplicationException(String.Format("No WCF URL set for customer {0}.", row.CustomerID_Ext));
                            //}
                            //else
                            //{
                                CustomerServiceClient client = GetServiceClient(WCFUrl, VersionNo);
                                client.UpdateOrderStatus(customerID, Password, row.SysTrxNo_Ext, row.OrderStatusID_Ext, VersionNo);

                                //On Sucess
                                DALMethods.UpdateOrderStatusHistory(row.OrderID_Ext, row.OrderStatusID_Ext, session, VersionNo);
                            //}


                        }
                        catch (Exception ex)
                        {
                            Logging.WriteLog(String.Format("Error for Customer customer {0} , OrderID {1} and Status {2}.", row.CustomerID_Ext, row.OrderID_Ext, row.OrderStatusID_Ext), EventLogEntryType.Error);
                            Logging.LogError(ex);
                        }
                    }
                }
                CloseSession(session);
            }
            catch (Exception ex)
            {
                CloseSession(session);
                Logging.LogError(ex);
            }
            finally
            {
                if (session != null)
                    CloseSession(session);
            }
        }

        //<summary>
        //UpdateShipmentDetails
        //Function to update shipment details
        //</summary>
        //public static void UpdateShipmentDetails()
        //{
        //    ISession session = null;
        //    try
        //    {
        //        session = GetNewSession();
        //        List<DAL.ShipmentDataRow> rows = DALMethods.GetShipmentData(ShipmentRetryCount, session, VersionNo);
        //        Logging.WriteToFile1("AAA GetShipmentData.," + rows.Count.ToString());

        //        if (rows != null && rows.Count > 0)
        //        {
        //            foreach (DAL.ShipmentDataRow row in rows)
        //            {
        //                try
        //                {
        //                    Logging.WriteToFile1("BBB GetShipmentData.," + row.CustomerID.ToString());
        //                    DAL.CustomerRow cust = DALMethods.GetCustomer(row.CustomerID, session, VersionNo);

        //                    if (String.IsNullOrEmpty(cust.WCFURL_Ext))
        //                    {
        //                        throw new ApplicationException(String.Format("No WCF URL set for customer {0}.", row.CustomerID));
        //                    }
        //                    else
        //                    {
        //                        CustomerServiceClient client = GetServiceClient(cust.WCFURL_Ext, VersionNo);
        //                        List<DAL.BOLItemRow> bolItems = DALMethods.GetUpdatedBolItem(row.CustomerID, row.SysTrxNo, row.SysTrxLine, session, VersionNo);
        //                        if (bolItems.Count > 0)
        //                        {
        //                            ShipmentRequest req = new ShipmentRequest();
        //                            req.SysTrxNo = row.SysTrxNo;
        //                            req.SysTrxLine = row.SysTrxLine;
        //                            List<ShipmentRequestComponents> lstComponents = new List<ShipmentRequestComponents>();
        //                            foreach (DAL.BOLItemRow bolItem in bolItems)
        //                            {
        //                                ShipmentRequestComponents component = new ShipmentRequestComponents();
        //                                component.ComponentNo = bolItem.ComponentNo_Ext;
        //                                component.NetQty = bolItem.NetQty_Ext;
        //                                component.GrossQty = bolItem.GrossQty_Ext;
        //                                component.BOLNo = bolItem.BolNo_Ext;
        //                                //component.Image = bolItem.Image_Ext;
        //                                component.BOLDateTime = bolItem.BOLDateTime_Ext;
        //                                component.ComponentNo = bolItem.ComponentNo_Ext;
        //                                component.BOLQtyVarianceReason = bolItem.BOLQtyVarianceReason_Ext;
        //                                //2013.09.23 FSWW, Ramesh M Added For CR#60090 to push BolImage data to Ascend
        //                                component.BOLImage = bolItem.Image_Ext;
        //                                component.SupplierCode = bolItem.SupplierCode_Ext;
        //                                component.SupplyPointCode = bolItem.SupplyPointCode_Ext;
        //                                component.ExtSysTrxLine = bolItem.ExtSysTrxLine_Ext;
        //                                // 2014.03.18  Ramesh M Added For CR#62719 added  TrailerCode to go back to ascend
        //                                component.TrailerCode = row.TrailerCode;
        //                                lstComponents.Add(component);
        //                            }
        //                            req.Components = lstComponents.ToArray();
        //                            req.UserID = row.UserName;
        //                            req.OrderLoadReviewEnabled = row.OrderLoadReviewEnabled;


        //                            ShipmentResponse res = client.UpdateShipment(cust.CustomerID_Ext, cust.Password_Ext, req, VersionNo);
        //                            foreach (DAL.BOLItemRow bolItem in bolItems)
        //                            {
        //                                DALMethods.UpdateBolItem(res.ShipDocSysTrxNo, res.ShipDocSysTrxLine, bolItem.Id_Ext, session, VersionNo);
        //                            }
        //                        }
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    DALMethods.BOLItemIncreateRetryCount(row.CustomerID, row.SysTrxNo, row.SysTrxLine, session, VersionNo);
        //                    Logging.WriteLog(String.Format("Error for CustomerID : {0}, SysTrxNo : {1}, SysTrxLine : {2}.", row.CustomerID, row.SysTrxNo, row.SysTrxLine), EventLogEntryType.Error);
        //                    Logging.LogError(ex);
        //                }
        //            }
        //        }
        //        CloseSession(session);
        //    }
        //    catch (Exception ex)
        //    {
        //        CloseSession(session);
        //        Logging.LogError(ex);
        //    }
        //}

        //<summary>
        //UpdateShipmentDetails
        //Function to update shipment details
        //</summary>

        public static void UpdateShipmentDetails(string customerID)
        {
            ISession session = null;
            try
            {
                session = GetNewSession();
                DataTable dtSD = DALMethods.GetShipmentData1(customerID, ShipmentRetryCount, session, VersionNo);

                if (dtSD != null && dtSD.Rows.Count > 0)
                {
                    for (var i = 0; i < dtSD.Rows.Count; i++)
                    {
                        try
                        {
                            //DAL.CustomerRow cust = DALMethods.GetCustomer(dtSD.Rows[i]["CustomerID"].ToString(), session, VersionNo);

                            //if (String.IsNullOrEmpty(cust.WCFURL_Ext))
                            //{
                            //    throw new ApplicationException(String.Format("No WCF URL set for customer {0}.", dtSD.Rows[i]["CustomerID"].ToString()));
                            //}
                            //else
                            //{
                            CustomerServiceClient client = GetServiceClient(WCFUrl, VersionNo);
                            DataTable bolItems = DALMethods.GetUpdatedBolItem1(dtSD.Rows[i]["CustomerID"].ToString(), Convert.ToDecimal(dtSD.Rows[i]["SysTrxNo"].ToString()), Convert.ToInt32(dtSD.Rows[i]["SysTrxLine"].ToString()), dtSD.Rows[i]["BOLNo"].ToString(), session, VersionNo);
                            if (bolItems.Rows.Count > 0)
                            {

                                ShipmentRequest req = new ShipmentRequest();
                                req.SysTrxNo = Convert.ToDecimal(dtSD.Rows[i]["SysTrxNo"].ToString());
                                req.SysTrxLine = Convert.ToInt32(dtSD.Rows[i]["SysTrxLine"].ToString());
                                List<ShipmentRequestComponents> lstComponents = new List<ShipmentRequestComponents>();
                                for (var j = 0; j < bolItems.Rows.Count; j++)
                                {
                                    if (DALMethods.IsSplitLoad(new Guid(bolItems.Rows[j]["LoadID"].ToString()), new Guid(), 1, session))
                                    {
                                        if (Convert.ToInt32(bolItems.Rows[j]["ShipDocSysTrxNo"].ToString()) != 0 && bolItems.Rows[j]["IsShipDocAtDelivery"].ToString() == "Y")
                                        {
                                            client.UndoShipDoc(customerID, Password, Convert.ToInt32(bolItems.Rows[j]["ShipDocSysTrxNo"].ToString()), 'A', VersionNo);
                                        }
                                    }
                                    ShipmentRequestComponents component = new ShipmentRequestComponents();
                                    component.ComponentNo = Convert.ToInt32(bolItems.Rows[j]["ComponentNo"].ToString());
                                    component.NetQty = Convert.ToDecimal(bolItems.Rows[j]["NetQty"].ToString());
                                    component.GrossQty = Convert.ToDecimal(bolItems.Rows[j]["GrossQty"].ToString());
                                    component.BOLNo = bolItems.Rows[j]["BOLNo"].ToString();
                                    //component.Image = bolItem.Image_Ext;
                                    component.BOLDateTime = Convert.ToDateTime(bolItems.Rows[j]["BOLDateTime"].ToString());
                                    component.BOLEndDateTime = Convert.ToDateTime(bolItems.Rows[j]["BOLEndDateTime"].ToString());
                                    //Convert.ToDateTime(bolItems.Rows[j]["BOLEndDateTime"].ToString());
                                    //component.ComponentNo = bolItem.ComponentNo_Ext;
                                    component.BOLQtyVarianceReason = bolItems.Rows[j]["BOLQtyVarianceReason"].ToString();
                                    //2013.09.23 FSWW, Ramesh M Added For CR#60090 to push BolImage data to Ascend
                                    //byte[] array = Encoding.ASCII.GetBytes(input);
                                    byte[] image = null;
                                    try
                                    {

                                        if (!String.IsNullOrWhiteSpace(bolItems.Rows[j]["Image"].ToString()))
                                        {
                                            //image = Encoding.ASCII.GetBytes(bolItems.Rows[j]["Image"].ToString());
                                            //image = System.Convert.FromBase64String(bolItems.Rows[j]["Image"].ToString());
                                            image = (byte[])bolItems.Rows[j]["Image"];
                                        }
                                    }
                                    catch
                                    {
                                        image = null;
                                    }
                                    component.BOLImage = image;
                                    //component.BOLImage = bolItems.Rows[j]["Image"];
                                    component.SupplierCode = bolItems.Rows[j]["SupplierCode"].ToString();
                                    component.SupplyPointCode = bolItems.Rows[j]["SupplyPointCode"].ToString();
                                    component.ExtSysTrxLine = Convert.ToInt32(bolItems.Rows[j]["ExtSysTrxLine"].ToString());
                                    // 2014.03.18  Ramesh M Added For CR#62719 added  TrailerCode to go back to ascend
                                    component.TrailerCode = dtSD.Rows[i]["TrailerCode"].ToString();
                                    lstComponents.Add(component);
                                }
                                req.Components = lstComponents.ToArray();
                                req.UserID = dtSD.Rows[i]["UserName"].ToString();
                                req.OrderLoadReviewEnabled = dtSD.Rows[i]["OrderLoadReviewEnabled"].ToString();


                                string EnableEventLog = DALMethods.IsEnableEventLog(dtSD.Rows[i]["CustomerID"].ToString(), session, VersionNo);

                                ShipmentResponse res = client.UpdateShipment(customerID, Password, req, VersionNo);
                                //Logging.WriteToFile1(" Dinesh Test ");

                                if (customerID == ConfigurationManager.AppSettings["BOLWaitTimeDetails"])
                                {
                                    DataTable dtBD = DALMethods.GetBOLWaitTimeDetails(req.SysTrxNo, session, VersionNo);

                                    if (dtBD != null && dtBD.Rows.Count > 0)
                                    {
                                        for (var k = 0; k < dtBD.Rows.Count; k++)
                                        {
                                            client.UpdateBOLWaitTimeDetails(customerID, Password, customerID, Convert.ToDecimal(dtBD.Rows[k]["SysTrxNo"]), dtBD.Rows[k]["BOLNo"].ToString(), Convert.ToDateTime(dtBD.Rows[k]["BOLWaitTimeStart"].ToString()), Convert.ToDateTime(dtBD.Rows[k]["BOLWaitTimeEnd"].ToString()), dtBD.Rows[k]["BOLWaitTimeComment"].ToString(), "");
                                        }
                                    }
                                }

                                for (var j = 0; j < bolItems.Rows.Count; j++)
                                {
                                    if (EnableEventLog.ToUpper().Trim() == "Y")
                                    {
                                        if (res.ErrorMessage != "")
                                        {
                                            DALMethods.AddShipmentEventLog(dtSD.Rows[i]["CustomerID"].ToString(), (Guid)bolItems.Rows[j]["LoadID"], (Guid)bolItems.Rows[j]["BOLHdrID"], (Guid)bolItems.Rows[j]["ID"], Convert.ToDateTime(bolItems.Rows[j]["BOLDateTime"].ToString()), Convert.ToDecimal(bolItems.Rows[j]["SysTrxNo"]), Convert.ToInt32(bolItems.Rows[j]["SysTrxLine"]), Convert.ToInt32(bolItems.Rows[j]["ComponentNo"]), Convert.ToDecimal(bolItems.Rows[j]["GrossQty"]), Convert.ToDecimal(bolItems.Rows[j]["NetQty"]), Convert.ToInt32(bolItems.Rows[j]["ExtSysTrxLine"]), res.ErrorMessage, GetMeaningfullMsg(res.ErrorMessage), session, "");
                                            DALMethods.BOLItemIncreateRetryCount(dtSD.Rows[i]["CustomerID"].ToString(), Convert.ToDecimal(dtSD.Rows[i]["SysTrxNo"].ToString()), Convert.ToInt32(dtSD.Rows[i]["SysTrxLine"].ToString()), session, VersionNo);
                                        }
                                        else
                                        {
                                            DALMethods.UpdateBolItem(res.ShipDocSysTrxNo, res.ShipDocSysTrxLine, new Guid(bolItems.Rows[j]["ID"].ToString()), session, VersionNo);
                                        }
                                    }
                                    else
                                    {
                                        DALMethods.UpdateBolItem(res.ShipDocSysTrxNo, res.ShipDocSysTrxLine, new Guid(bolItems.Rows[j]["ID"].ToString()), session, VersionNo);
                                    }
                                }
                            }
                            //}
                        }
                        catch (Exception ex)
                        {
                            DALMethods.BOLItemIncreateRetryCount(dtSD.Rows[i]["CustomerID"].ToString(), Convert.ToDecimal(dtSD.Rows[i]["SysTrxNo"].ToString()), Convert.ToInt32(dtSD.Rows[i]["SysTrxLine"].ToString()), session, VersionNo);
                            Logging.WriteLog(String.Format("Error for CustomerID : {0}, SysTrxNo : {1}, SysTrxLine : {2}.", dtSD.Rows[i]["CustomerID"].ToString(), dtSD.Rows[i]["SysTrxNo"].ToString(), dtSD.Rows[i]["SysTrxLine"].ToString()), EventLogEntryType.Error);
                            Logging.LogError(ex);
                        }
                    }
                }
                CloseSession(session);
            }
            catch (Exception ex)
            {
                CloseSession(session);
                Logging.LogError(ex);
            }
            finally
            {
                if (session != null)
                    CloseSession(session);
            }
        }

        /// <summary>
        /// UpdateShipmentDetails
        /// Function to update shipment details
        /// </summary>
        public static void UpdateBOLTankWagonDetails()
        {
            ISession session = null;
            try
            {
                session = GetNewSession();
                List<DAL.BOLHdr_WagonRow> rows = DALMethods.GetBOLHdrData(BOLHdrRetryCount, session, VersionNo);
                if (rows != null && rows.Count > 0)
                {
                    foreach (DAL.BOLHdr_WagonRow row in rows)
                    {
                        try
                        {
                            DAL.CustomerRow cust = DALMethods.GetCustomer(row.ClientID, session, VersionNo);

                            if (String.IsNullOrEmpty(cust.WCFURL_Ext))
                            {
                                throw new ApplicationException(String.Format("No WCF URL set for customer {0}.", row.ClientID));
                            }
                            else
                            {
                                CustomerServiceClient client = GetServiceClient(cust.WCFURL_Ext, VersionNo);
                                List<DAL.BOLItem_WagonRow> bolItems = DALMethods.GetBolItemWagon(row.ID, session, VersionNo);
                                if (bolItems.Count > 0)
                                {
                                    TWBOLDetails req = new TWBOLDetails();
                                    req.BOLHdrID = row.ID;
                                    req.BOLNo = row.BOLNo;
                                    req.SupplierCode = row.SupplierCode;
                                    req.SupplyPointCode = row.SupplyPointCode;
                                    req.UpdatedBy = row.UpdatedBy;
                                    req.BOLDateTime = row.BOLDatetime;

                                    List<TWBOLItemDetails> lstComponents = new List<TWBOLItemDetails>();
                                    foreach (DAL.BOLItem_WagonRow bolItem in bolItems)
                                    {
                                        TWBOLItemDetails TWBOLItem = new Customer.TWBOLItemDetails();
                                        TWBOLItem.CompartmentID = bolItem.CompartmentID_Ext;
                                        TWBOLItem.SystrxNo = bolItem.SysTrxNo_Ext;
                                        TWBOLItem.SystrxLine = bolItem.SysTrxLine_Ext;
                                        TWBOLItem.ProdCode = bolItem.ProdCode_Ext;
                                        //component.Image = bolItem.Image_Ext;
                                        TWBOLItem.GrossQty = bolItem.GrossQty_Ext;
                                        TWBOLItem.OrderedQty = bolItem.OrderedQty_Ext;
                                        TWBOLItem.NetQty = bolItem.NetQty_Ext;
                                        //2013.09.23 FSWW, Ramesh M Added For CR#60090 to push BolImage data to Ascend
                                        TWBOLItem.Notes = bolItem.Notes_Ext;
                                        lstComponents.Add(TWBOLItem);
                                    }
                                    req.BOLItemDetails = lstComponents.ToArray();
                                    String Result;
                                    Result = client.UpdateWagonShipment(cust.CustomerID_Ext, cust.Password_Ext, req, VersionNo);
                                    // ShipmentResponse res = client.UpdateShipment(cust.CustomerID_Ext, cust.Password_Ext, req, VersionNo);
                                    //foreach (DAL.BOLItemRow bolItem in bolItems)
                                    //{
                                    //    DALMethods.UpdateBolItem(res.ShipDocSysTrxNo, res.ShipDocSysTrxLine, bolItem.Id_Ext, session, VersionNo);
                                    //}

                                    //string ComponentsXML = string.Empty;
                                    //using (StringWriter sw = new StringWriter())
                                    //{
                                    //    XmlSerializer xs = new XmlSerializer(typeof(List<TWBOLItemDetails>));
                                    //    xs.Serialize(sw, req.BOLItemDetails);
                                    //    ComponentsXML = sw.ToString().Replace("utf-16", "utf-8");
                                    //}


                                    Logging.WriteLog("Wagon ShipmentDetails: " + req.BOLItemDetails.Count().ToString() + req.ToString() + " :" + Result, EventLogEntryType.SuccessAudit);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //DALMethods.BOLItemIncreateRetryCount(row.CustomerID, row.SysTrxNo, row.SysTrxLine, session, VersionNo);
                            //Logging.WriteLog(String.Format("Error for CustomerID : {0}, SysTrxNo : {1}, SysTrxLine : {2}.", row.CustomerID, row.SysTrxNo, row.SysTrxLine), EventLogEntryType.Error);
                            Logging.LogError(ex);
                        }
                    }
                }
                CloseSession(session);
            }
            catch (Exception ex)
            {
                CloseSession(session);
                Logging.LogError(ex);
            }
        }

        /// <summary>
        /// UpdateDeliveryDetails
        /// Function to update delivery details
        /// </summary>
        //public static void UpdateDeliveryDetails()
        //{
        //    ISession session = null;
        //    try
        //    {
        //        session = GetNewSession();
        //        List<DAL.DeliveryOrdersRow> rows = DALMethods.GetUpdatedDeliveryOrders(DeliveryRetryCount, session, VersionNo);
        //        if (rows != null && rows.Count > 0)
        //        {
        //            rows.Sort((x, y) => string.Compare(x.CustomerID_Ext, y.CustomerID_Ext));
        //            foreach (DAL.DeliveryOrdersRow row in rows)
        //            {
        //                try
        //                {
        //                    DAL.CustomerRow cust = DALMethods.GetCustomer(row.CustomerID_Ext, session, VersionNo);
        //                    Logging.WriteLog(String.Format("UpdateDeliveryDetails AAA CustomerID .", row.CustomerID_Ext.ToString()), EventLogEntryType.Error);
        //                    if (String.IsNullOrEmpty(cust.WCFURL_Ext))
        //                    {
        //                        throw new ApplicationException(String.Format("No WCF URL set for customer {0}.", row.CustomerID_Ext));
        //                    }
        //                    else
        //                    {
        //                        Logging.WriteLog(String.Format("UpdateDeliveryDetails BBB CustomerID .", row.CustomerID_Ext.ToString()), EventLogEntryType.Error);
        //                        CustomerServiceClient client = GetServiceClient(cust.WCFURL_Ext, VersionNo);

        //                        DAL.OrderFrtRow orderFrt = DALMethods.GetFrightDetails(row.ID_Ext, session, VersionNo);
        //                        DeliveryRequest req = new DeliveryRequest();
        //                        req.SysTrxNo = row.SysTrxNo_Ext;
        //                        if (orderFrt != null)
        //                        {
        //                            req.BOLWaitTime = orderFrt.BOLWaitTime_Ext;
        //                            req.BOLWaitTimeTotal = orderFrt.BOLWaitTimeTotal_Ext;
        //                            req.SiteWaitTime = orderFrt.SiteWaitTime_Ext;
        //                            req.SiteWaitTime_Comment = orderFrt.SiteWaitTime_Comment_Ext;
        //                            req.SiteWaitTime_Start = orderFrt.SiteWaitTime_Start_Ext;
        //                            req.SiteWaitTime_End = orderFrt.SiteWaitTime_End_Ext;
        //                            req.SplitLoad = orderFrt.SplitLoad_Ext;
        //                            req.SplitLoad_Comment = orderFrt.SplitLoad_Comment_Ext;
        //                            req.SplitDrop = orderFrt.SplitDrop_Ext;
        //                            req.SplitDrop_Comment = orderFrt.SplitDrop_Comment_Ext;
        //                            req.PumpOut = orderFrt.PumpOut_Ext;
        //                            req.PumpOut_Comment = orderFrt.PumpOut_Comment_Ext;
        //                            req.Diversion = orderFrt.Diversion_Ext;
        //                            req.Diversion_Comment = orderFrt.Diversion_Comment_Ext;
        //                            req.MinimumLoad = orderFrt.MinimumLoad_Ext;
        //                            req.MinimumLoad_Comment = orderFrt.MinimumLoad_Comment_Ext;
        //                            req.Other = orderFrt.Other_Ext;
        //                            req.Other_Comment = orderFrt.Other_Comment_Ext;
        //                            req.SignatureStatus = orderFrt.SignatureStatus_Ext;
        //                            //2013.09.13 FSWW, Ramesh M Added ForCR#60123 Adding Two parameters in Class list
        //                            req.SignatureImage = orderFrt.SignatureImage_Ext;
        //                            req.SignatureDateTime = orderFrt.SignatureDateTime_Ext;
        //                        }


        //                        List<DAL.DeliveryDetailsRow> items = DALMethods.GetUpdatedDeliveryDetails(row.CustomerID_Ext, row.LoadID_Ext, row.SysTrxNo_Ext, session, VersionNo);

        //                        List<DeliveryRequestItems> lstItems = new List<DeliveryRequestItems>();
        //                        List<Guid> OrderItemIDs = new List<Guid>();

        //                        foreach (DAL.DeliveryDetailsRow item in items)
        //                        {
        //                            DeliveryRequestItems deliveryRequestitem = new DeliveryRequestItems();
        //                            deliveryRequestitem.SysLineNo = item.SysTrxLine_Ext;
        //                            deliveryRequestitem.ShipDocSysTrxNo = item.ShipDocSysTrxNo_Ext;
        //                            deliveryRequestitem.ShipDocSysTrxLine = item.ShipDocSysTrxLine_Ext;
        //                            deliveryRequestitem.GrossQty = item.GrossQty_Ext;
        //                            deliveryRequestitem.NetQty = item.NetQty_Ext;
        //                            deliveryRequestitem.DelivDtTm = item.DeliveryDateTime_Ext;
        //                            deliveryRequestitem.DeliveryQtyVarianceReason = item.DeliveryQtyVarianceReason_Ext;
        //                            lstItems.Add(deliveryRequestitem);
        //                            OrderItemIDs.Add(item.OrderItemID_Ext);
        //                        }
        //                        req.Items = lstItems.ToArray();
        //                        req.UserID = row.UserName_Ext;
        //                        req.OrderLoadReviewEnabled = row.OrderLoadReviewEnabled;

        //                        client.UpdateDeliveryDetails(cust.CustomerID_Ext, cust.Password_Ext, req, VersionNo);
        //                        //On Sucess
        //                        foreach (Guid OrderItemID in OrderItemIDs)
        //                        {
        //                            DALMethods.UpdateDeliveryDetails(OrderItemID, session, VersionNo);
        //                        }
        //                    }

        //                }
        //                catch (Exception ex)
        //                {
        //                    DALMethods.DeliveryDetailIncreaseRetryCount(row.CustomerID_Ext, row.LoadID_Ext, row.SysTrxNo_Ext, session, VersionNo);
        //                    Logging.WriteLog(String.Format("Error for Customer customer {0} and SysTrxNo {1}.", row.CustomerID_Ext, row.SysTrxNo_Ext), EventLogEntryType.Error);
        //                    Logging.LogError(ex);
        //                }
        //            }
        //        }
        //        CloseSession(session);
        //    }
        //    catch (Exception ex)
        //    {
        //        CloseSession(session);
        //        Logging.LogError(ex);
        //    }
        //}

        /// <summary>
        /// UpdateDeliveryDetails
        /// Function to update delivery details
        /// </summary>
        /// 
        public static void UpdateDeliveryDetails(string customerID)
        {

            ISession session = null;
            try
            {
               
                session = GetNewSession();
                DataTable rows = DALMethods.GetUpdatedDeliveryOrders1(customerID, DeliveryRetryCount, session, VersionNo);
                if (rows != null && rows.Rows.Count > 0)
                {
                    Logging.WriteToFile1("GetUpdatedDeliveryOrders1");
                    //rows.Sort((x, y) => string.Compare(x.CustomerID_Ext, y.CustomerID_Ext));
                    for (var i = 0; i < rows.Rows.Count; i++)
                    {
                        List<Guid> OrderItemIDs = null;
                        try
                        {
                            //DAL.CustomerRow cust = DALMethods.GetCustomer(rows.Rows[i]["CustomerID"].ToString(), session, VersionNo);
                            ////Logging.WriteLog(String.Format("UpdateDeliveryDetails AAA CustomerID .", rows.Rows[i]["CustomerID"].ToString()), EventLogEntryType.Error);
                            //if (String.IsNullOrEmpty(cust.WCFURL_Ext))
                            //{
                            //    throw new ApplicationException(String.Format("No WCF URL set for customer {0}.", rows.Rows[i]["CustomerID"].ToString()));
                            //}
                            //else
                            //{
                            //Logging.WriteLog(String.Format("UpdateDeliveryDetails BBB CustomerID .", rows.Rows[i]["CustomerID"].ToString()), EventLogEntryType.Error);
                            //Logging.WriteToFile1("BeforeWCFConnecting");
                            CustomerServiceClient client = GetServiceClient(WCFUrl, VersionNo);
                            //Logging.WriteToFile1("AfterWCFConnecting");

                            DAL.OrderFrtRow orderFrt = DALMethods.GetFrightDetails(new Guid(rows.Rows[i]["ID"].ToString()), session, VersionNo);

                            DeliveryRequest req = new DeliveryRequest();
                            req.SysTrxNo = Convert.ToDecimal(rows.Rows[i]["SysTrxNo"].ToString());
                            if (orderFrt != null)
                            {
                                req.BOLWaitTime = orderFrt.BOLWaitTime_Ext;
                                req.BOLWaitTimeTotal = orderFrt.BOLWaitTimeTotal_Ext;
                                req.SiteWaitTime = orderFrt.SiteWaitTime_Ext;
                                req.SiteWaitTime_Comment = orderFrt.SiteWaitTime_Comment_Ext;
                                req.SiteWaitTime_Start = orderFrt.SiteWaitTime_Start_Ext;
                                req.SiteWaitTime_End = orderFrt.SiteWaitTime_End_Ext;
                                req.SplitLoad = orderFrt.SplitLoad_Ext;
                                req.SplitLoad_Comment = orderFrt.SplitLoad_Comment_Ext;
                                req.SplitDrop = orderFrt.SplitDrop_Ext;
                                req.SplitDrop_Comment = orderFrt.SplitDrop_Comment_Ext;
                                req.PumpOut = orderFrt.PumpOut_Ext;
                                req.PumpOut_Comment = orderFrt.PumpOut_Comment_Ext;
                                req.Diversion = orderFrt.Diversion_Ext;
                                req.Diversion_Comment = orderFrt.Diversion_Comment_Ext;
                                req.MinimumLoad = orderFrt.MinimumLoad_Ext;
                                req.MinimumLoad_Comment = orderFrt.MinimumLoad_Comment_Ext;
                                req.Other = orderFrt.Other_Ext;
                                req.Other_Comment = orderFrt.Other_Comment_Ext;
                                req.SignatureStatus = orderFrt.SignatureStatus_Ext;
                                //2013.09.13 FSWW, Ramesh M Added ForCR#60123 Adding Two parameters in Class list
                                req.SignatureImage = orderFrt.SignatureImage_Ext;

                                req.SignatureDateTime = orderFrt.SignatureDateTime_Ext;
                            }

                            Logging.WriteToFile1("BeforeDeliveryItemDetails");
                            DataTable items = DALMethods.GetUpdatedDeliveryDetails1(rows.Rows[i]["CustomerID"].ToString(), new Guid(rows.Rows[i]["LoadID"].ToString()), Convert.ToDecimal(rows.Rows[i]["SysTrxNo"].ToString()), session, VersionNo);
                            Logging.WriteToFile1("AfterDeliveryItemDetails");

                            List<DeliveryRequestItems> lstItems = new List<DeliveryRequestItems>();
                            OrderItemIDs = new List<Guid>();

                            for (var j = 0; j < items.Rows.Count; j++)
                            {
                                DeliveryRequestItems deliveryRequestitem = new DeliveryRequestItems();
                                deliveryRequestitem.SysLineNo = Convert.ToInt32(items.Rows[j]["SysTrxLine"].ToString());
                                deliveryRequestitem.ShipDocSysTrxNo = Convert.ToDecimal(items.Rows[j]["ShipDocSysTrxNo"].ToString());
                                deliveryRequestitem.ShipDocSysTrxLine = Convert.ToInt32(items.Rows[j]["ShipDocSysTrxLine"].ToString());
                                deliveryRequestitem.GrossQty = Convert.ToDecimal(items.Rows[j]["GrossQty"].ToString());
                                deliveryRequestitem.NetQty = Convert.ToDecimal(items.Rows[j]["NetQty"].ToString());
                                deliveryRequestitem.DelivDtTm = Convert.ToDateTime(items.Rows[j]["DeliveryDateTime"].ToString());
                                deliveryRequestitem.DeliveryQtyVarianceReason = items.Rows[j]["DeliveryQtyVarianceReason"].ToString();
                                deliveryRequestitem.BeforeVolume = Convert.ToDecimal(items.Rows[j]["BeforeVolume"].ToString());
                                deliveryRequestitem.AfterVolume = Convert.ToDecimal(items.Rows[j]["AfterVolume"].ToString());
                                // Add by amal for Multi BOL issue                                    
                                deliveryRequestitem.BOLNo = !string.IsNullOrEmpty(items.Rows[j]["BOLNo"].ToString()) ? items.Rows[j]["BOLNo"].ToString() : string.Empty;
                                string DeliveryImage = DALMethods.IsEnabledDeliveryImagePost(rows.Rows[i]["CustomerID"].ToString(), session, VersionNo);

                                Logging.WriteToFile1("BOLNo - " + items.Rows[j]["BOLNo"].ToString());

                                if (DeliveryImage.ToUpper().Trim() == "Y")
                                {
                                    byte[] image = null;
                                    byte[] imagePdf = null;
                                    try
                                    {

                                        if (!String.IsNullOrWhiteSpace(items.Rows[j]["Image"].ToString()))
                                        {
                                            //image = Encoding.ASCII.GetBytes(bolItems.Rows[j]["Image"].ToString());
                                            //image = System.Convert.FromBase64String(bolItems.Rows[j]["Image"].ToString());
                                            image = (byte[])items.Rows[j]["Image"];
                                            // imagePdf = (byte[])items.Rows[j]["ImagePdf"];
                                        }
                                    }
                                    catch
                                    {
                                        image = null;
                                        imagePdf = null;
                                    }
                                    deliveryRequestitem.DeliveryImage = image;
                                    deliveryRequestitem.DeliveryImagePdf = imagePdf;

                                }
                                lstItems.Add(deliveryRequestitem);
                                OrderItemIDs.Add(new Guid(items.Rows[j]["OrderItemID"].ToString()));
                            }
                            req.Items = lstItems.ToArray();
                            req.UserID = rows.Rows[i]["UserName"].ToString();
                            req.OrderLoadReviewEnabled = rows.Rows[i]["OrderLoadReviewEnabled"].ToString();

                            Logging.WriteToFile1(customerID + " - " + Password + " - " + req + " - " + VersionNo);

                            client.UpdateDeliveryDetails(customerID, Password, req, VersionNo);
                            //On Sucess
                            foreach (Guid OrderItemID in OrderItemIDs)
                            {
                                Logging.WriteToFile1("UpdateDeliveryDetails OrderItemID: " + OrderItemID);
                                DALMethods.UpdateDeliveryDetails(OrderItemID, session, VersionNo);
                            }
                            //}
                            client.Close();
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message.Contains("Item is already invoiced!"))
                            {
                                Logging.WriteToFile1("UpdateDeliveryDetails OrderItemID Count: " + OrderItemIDs.Count);
                                Logging.WriteToFile1("DeliveryDetails Item is already invoiced.");
                                foreach (Guid OrderItemID in OrderItemIDs)
                                {
                                    Logging.WriteToFile1("UpdateDeliveryDetails OrderItemID ex: " + OrderItemID);
                                    try
                                    {
                                        DALMethods.UpdateDeliveryDetails(OrderItemID, session, VersionNo);
                                        Logging.WriteToFile1("DeliveryDetails needupdate updated");
                                    }
                                    catch (Exception ex1)
                                    {
                                        Logging.WriteToFile1("DeliveryDetails Item invoiced Error msg: " + ex1.Message);
                                    }
                                }
                            }
                            DALMethods.DeliveryDetailIncreaseRetryCount(rows.Rows[i]["CustomerID"].ToString(), new Guid(rows.Rows[i]["LoadID"].ToString()), Convert.ToDecimal(rows.Rows[i]["SysTrxNo"].ToString()), session, VersionNo);

                            Logging.WriteLog(String.Format("Error for Customer customer {0} and SysTrxNo {1}.", rows.Rows[i]["CustomerID"].ToString(), rows.Rows[i]["SysTrxNo"].ToString()), EventLogEntryType.Error);
                            Logging.LogError(ex);
                        }
                    }
                }

                CloseSession(session);
            }
            catch (Exception ex)
            {
                CloseSession(session);
                Logging.LogError(ex);
            }
            finally
            {
                if (session != null)
                    CloseSession(session);
            }
        }

        //public static void UpdateDeliveryDetailsNew(string customerID)
        //{

        //    ISession session = null;
        //    try
        //    {
        //        List<Guid> OrderItemIDs = new List<Guid>();
        //        session = GetNewSession();
        //        DataTable rows = DALMethods.GetUpdatedDeliveryOrders1(customerID, DeliveryRetryCount, session, VersionNo);                
        //        if (rows != null && rows.Rows.Count > 0)
        //        {
        //            //rows.Sort((x, y) => string.Compare(x.CustomerID_Ext, y.CustomerID_Ext));
        //            for (var i = 0; i < rows.Rows.Count; i++)
        //            {
        //                try
        //                {
        //                    //DAL.CustomerRow cust = DALMethods.GetCustomer(rows.Rows[i]["CustomerID"].ToString(), session, VersionNo);
        //                    ////Logging.WriteLog(String.Format("UpdateDeliveryDetails AAA CustomerID .", rows.Rows[i]["CustomerID"].ToString()), EventLogEntryType.Error);
        //                    //if (String.IsNullOrEmpty(cust.WCFURL_Ext))
        //                    //{
        //                    //    throw new ApplicationException(String.Format("No WCF URL set for customer {0}.", rows.Rows[i]["CustomerID"].ToString()));
        //                    //}
        //                    //else
        //                    //{
        //                    //Logging.WriteLog(String.Format("UpdateDeliveryDetails BBB CustomerID .", rows.Rows[i]["CustomerID"].ToString()), EventLogEntryType.Error);
        //                    CustomerServiceClient client = GetServiceClient(WCFUrl, VersionNo);

        //                    DAL.OrderFrtRow orderFrt = DALMethods.GetFrightDetails(new Guid(rows.Rows[i]["ID"].ToString()), session, VersionNo);

        //                    DeliveryRequest req = new DeliveryRequest();
        //                    req.SysTrxNo = Convert.ToDecimal(rows.Rows[i]["SysTrxNo"].ToString());
        //                    if (orderFrt != null)
        //                    {
        //                        req.BOLWaitTime = orderFrt.BOLWaitTime_Ext;
        //                        req.BOLWaitTimeTotal = orderFrt.BOLWaitTimeTotal_Ext;
        //                        req.SiteWaitTime = orderFrt.SiteWaitTime_Ext;
        //                        req.SiteWaitTime_Comment = orderFrt.SiteWaitTime_Comment_Ext;
        //                        req.SiteWaitTime_Start = orderFrt.SiteWaitTime_Start_Ext;
        //                        req.SiteWaitTime_End = orderFrt.SiteWaitTime_End_Ext;
        //                        req.SplitLoad = orderFrt.SplitLoad_Ext;
        //                        req.SplitLoad_Comment = orderFrt.SplitLoad_Comment_Ext;
        //                        req.SplitDrop = orderFrt.SplitDrop_Ext;
        //                        req.SplitDrop_Comment = orderFrt.SplitDrop_Comment_Ext;
        //                        req.PumpOut = orderFrt.PumpOut_Ext;
        //                        req.PumpOut_Comment = orderFrt.PumpOut_Comment_Ext;
        //                        req.Diversion = orderFrt.Diversion_Ext;
        //                        req.Diversion_Comment = orderFrt.Diversion_Comment_Ext;
        //                        req.MinimumLoad = orderFrt.MinimumLoad_Ext;
        //                        req.MinimumLoad_Comment = orderFrt.MinimumLoad_Comment_Ext;
        //                        req.Other = orderFrt.Other_Ext;
        //                        req.Other_Comment = orderFrt.Other_Comment_Ext;
        //                        req.SignatureStatus = orderFrt.SignatureStatus_Ext;
        //                        //2013.09.13 FSWW, Ramesh M Added ForCR#60123 Adding Two parameters in Class list
        //                        req.SignatureImage = orderFrt.SignatureImage_Ext;

        //                        req.SignatureDateTime = orderFrt.SignatureDateTime_Ext;
        //                    }


        //                    DataTable items = DALMethods.GetUpdatedDeliveryDetails1(rows.Rows[i]["CustomerID"].ToString(), new Guid(rows.Rows[i]["LoadID"].ToString()), Convert.ToDecimal(rows.Rows[i]["SysTrxNo"].ToString()), session, VersionNo);

        //                    List<DeliveryRequestItems> lstItems = new List<DeliveryRequestItems>();
        //                    OrderItemIDs = new List<Guid>();

        //                    for (var j = 0; j < items.Rows.Count; j++)
        //                    {
        //                        DeliveryRequestItems deliveryRequestitem = new DeliveryRequestItems();
        //                        deliveryRequestitem.SysLineNo = Convert.ToInt32(items.Rows[j]["SysTrxLine"].ToString());
        //                        deliveryRequestitem.ShipDocSysTrxNo = Convert.ToDecimal(items.Rows[j]["ShipDocSysTrxNo"].ToString());
        //                        deliveryRequestitem.ShipDocSysTrxLine = Convert.ToInt32(items.Rows[j]["ShipDocSysTrxLine"].ToString());
        //                        deliveryRequestitem.GrossQty = Convert.ToDecimal(items.Rows[j]["GrossQty"].ToString());
        //                        deliveryRequestitem.NetQty = Convert.ToDecimal(items.Rows[j]["NetQty"].ToString());
        //                        deliveryRequestitem.DelivDtTm = Convert.ToDateTime(items.Rows[j]["DeliveryDateTime"].ToString());
        //                        deliveryRequestitem.DeliveryQtyVarianceReason = items.Rows[j]["DeliveryQtyVarianceReason"].ToString();
        //                        deliveryRequestitem.BeforeVolume = Convert.ToDecimal(items.Rows[j]["BeforeVolume"].ToString());
        //                        deliveryRequestitem.AfterVolume = Convert.ToDecimal(items.Rows[j]["AfterVolume"].ToString());
        //                        // Add by amal for Multi BOL issue                                    
        //                        deliveryRequestitem.BOLNo = !string.IsNullOrEmpty(items.Rows[j]["BOLNo"].ToString()) ? items.Rows[j]["BOLNo"].ToString() : string.Empty;
        //                        string DeliveryImage = DALMethods.IsEnabledDeliveryImagePost(rows.Rows[i]["CustomerID"].ToString(), session, VersionNo);

        //                        Logging.WriteToFile1("BOLNo - " + items.Rows[j]["BOLNo"].ToString());

        //                        if (DeliveryImage.ToUpper().Trim() == "Y")
        //                        {
        //                            byte[] image = null;
        //                            byte[] imagePdf = null;
        //                            try
        //                            {

        //                                if (!String.IsNullOrWhiteSpace(items.Rows[j]["Image"].ToString()))
        //                                {
        //                                    //image = Encoding.ASCII.GetBytes(bolItems.Rows[j]["Image"].ToString());
        //                                    //image = System.Convert.FromBase64String(bolItems.Rows[j]["Image"].ToString());
        //                                    image = (byte[])items.Rows[j]["Image"];
        //                                    // imagePdf = (byte[])items.Rows[j]["ImagePdf"];
        //                                }
        //                            }
        //                            catch
        //                            {
        //                                image = null;
        //                                imagePdf = null;
        //                            }
        //                            deliveryRequestitem.DeliveryImage = image;
        //                            deliveryRequestitem.DeliveryImagePdf = imagePdf;

        //                        }
        //                        lstItems.Add(deliveryRequestitem);
        //                        OrderItemIDs.Add(new Guid(items.Rows[j]["OrderItemID"].ToString()));
        //                    }
        //                    req.Items = lstItems.ToArray();
        //                    req.UserID = rows.Rows[i]["UserName"].ToString();
        //                    req.OrderLoadReviewEnabled = rows.Rows[i]["OrderLoadReviewEnabled"].ToString();

        //                    Logging.WriteToFile1(customerID + " - " + Password + " - " + req + " - " + VersionNo);

        //                    client.UpdateDeliveryDetails(customerID, Password, req, VersionNo);
        //                    //On Sucess
        //                    foreach (Guid OrderItemID in OrderItemIDs)
        //                    {
        //                        Logging.WriteToFile1("UpdateDeliveryDetails OrderItemID: " + OrderItemID);
        //                        DALMethods.UpdateDeliveryDetails(OrderItemID, session, VersionNo);
        //                    }
        //                    //}

        //                }
        //                catch (Exception ex)
        //                {
        //                    if (ex.Message.Contains("Item is already invoiced!"))
        //                    {
        //                        Logging.WriteToFile1("DeliveryDetails Item is already invoiced.");
        //                        foreach (Guid OrderItemID in OrderItemIDs)
        //                        {
        //                            DALMethods.UpdateDeliveryDetails(OrderItemID, session, VersionNo);
        //                        }
        //                    }

        //                    DALMethods.DeliveryDetailIncreaseRetryCount(rows.Rows[i]["CustomerID"].ToString(), new Guid(rows.Rows[i]["LoadID"].ToString()), Convert.ToDecimal(rows.Rows[i]["SysTrxNo"].ToString()), session, VersionNo);
        //                    Logging.WriteLog(String.Format("Error for Customer customer {0} and SysTrxNo {1}.", rows.Rows[i]["CustomerID"].ToString(), rows.Rows[i]["SysTrxNo"].ToString()), EventLogEntryType.Error);
        //                    Logging.LogError(ex);
        //                }
        //            }
        //        }

        //        CloseSession(session);
        //    }
        //    catch (Exception ex)
        //    {
        //        CloseSession(session);
        //        Logging.LogError(ex);
        //    }
        //    finally
        //    {
        //        if (session != null)
        //            CloseSession(session);
        //    }
        //}

        /// <summary>
        /// UpdateEODInventoryDetails
        /// Function to update EODInventoryDetails
        /// </summary>
        public static void UpdateEODInventoryDetails()
        {
            ISession session = null;
            try
            {
                session = GetNewSession();
                List<DAL.EODInventoryProcessRow> rows = DALMethods.GetEODClientID(session, VersionNo);
                if (rows != null && rows.Count > 0)
                {
                    foreach (DAL.EODInventoryProcessRow row in rows)
                    {
                        try
                        {
                            string IsEnabledTW = DALMethods.IsEnabledTankwagon(row.ClientID, session, VersionNo);
                            if (IsEnabledTW.ToUpper().Trim() == "Y")
                            {
                                DAL.CustomerRow cust = DALMethods.GetCustomer(row.ClientID, session, VersionNo);

                                if (String.IsNullOrEmpty(cust.WCFURL_Ext))
                                {
                                    throw new ApplicationException(String.Format("No WCF URL set for customer {0}.", row.ClientID));
                                }
                                else
                                {
                                    CustomerServiceClient client = GetServiceClient(cust.WCFURL_Ext, VersionNo);
                                    List<DAL.EODInventoryProcessRow> EODInventorys = DALMethods.GetDataEODInventoryProcess(row.ClientID, session, VersionNo);
                                    if (EODInventorys.Count > 0)
                                    {
                                        Logging.WriteToFile1("Test UpdateEODInventoryDetails--," + EODInventorys.Count.ToString());
                                        foreach (DAL.EODInventoryProcessRow EODInventory in EODInventorys)
                                        {
                                            try
                                            {
                                                Logging.WriteToFile1("Test UpdateEODInventoryDetails-- BOLNO --," + EODInventory.BOLNo_Ext);
                                                Boolean Result;
                                                Result = client.EODInventoryProcess(Convert.ToInt32(cust.CustomerID_Ext), cust.Password_Ext, EODInventory.ToSiteID_Ext.ToString(), EODInventory.SupplierCode_Ext, EODInventory.SupplyPointCode_Ext, EODInventory.ProdCode_Ext, EODInventory.OrderedQty_Ext, EODInventory.AvailableQty_Ext, EODInventory.NetQty_Ext, EODInventory.OrderSysTrxNo_Ext, EODInventory.OrderSysTrxLineNo_Ext, EODInventory.VehicleID_Ext, EODInventory.DriverID_Ext, EODInventory.UserID_Ext.ToString(), EODInventory.BOLNo_Ext, EODInventory.BOLDatetime_Ext, EODInventory.BOLSessionID_Ext, EODInventory.IsOverShort_Ext, VersionNo);
                                                //Logging.WriteLog(String.Format("Error for  {0} and SysTrxNo {1}.", cust.CustomerID_Ext + Result.ToString(), row.ToSiteID_Ext), EventLogEntryType.Error);
                                                //Logging.WriteLog(String.Format("Error for Customer customer {0} and SysTrxNo {1}.", cust.CustomerID_Ext + Result.ToString(), row.ToSiteID_Ext), EventLogEntryType.Error);
                                                if (Result)
                                                    DALMethods.UpdateBOLNeedUpdate(EODInventory.ToSiteID_Ext, EODInventory.SupplierCode_Ext, EODInventory.SupplyPointCode_Ext, EODInventory.ProdCode_Ext, EODInventory.OrderedQty_Ext, EODInventory.AvailableQty_Ext, EODInventory.VehicleID_Ext, EODInventory.DriverID_Ext, EODInventory.BOLNo_Ext, EODInventory.UserID_Ext, session, VersionNo);
                                            }
                                            catch (Exception ex)
                                            {
                                                Logging.WriteLog(String.Format("Error in UpdateEODInventoryDetails for CustomerID : {0}.", row.ClientID), EventLogEntryType.Error);
                                                Logging.LogError(ex);
                                            }
                                        }

                                        //Logging.WriteLog("Wagon ShipmentDetails: " + req.BOLItemDetails.Count().ToString() + req.ToString() + " :" + Result, EventLogEntryType.SuccessAudit);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //DALMethods.BOLItemIncreateRetryCount(row.CustomerID, row.SysTrxNo, row.SysTrxLine, session, VersionNo);
                            Logging.WriteLog(String.Format("Error in UpdateEODInventoryDetails for CustomerID : {0}.", row.ClientID), EventLogEntryType.Error);
                            Logging.LogError(ex);
                        }
                    }
                }
                CloseSession(session);
            }
            catch (Exception ex)
            {
                CloseSession(session);
                Logging.LogError(ex);
            }
            finally
            {
                if (session != null)
                    CloseSession(session);
            }
        }

        /// <summary>
        /// UpdateBOLImages
        /// Function to update BOLImages
        /// </summary>
        public static void UpdateBOLImages(string customerID)
        {
            ISession session = null;
            try
            {
                session = GetNewSession();
                DataTable dtSD = DALMethods.GetDataToUpdateBOLImage(customerID, session, VersionNo);
                if (dtSD != null && dtSD.Rows.Count > 0)
                {
                    for (var i = 0; i < dtSD.Rows.Count; i++)
                    {
                        try
                        {
                            int Systrxno = 0;
                            //DAL.CustomerRow cust = DALMethods.GetCustomer(dtSD.Rows[i]["CustomerID"].ToString(), session, VersionNo);

                            //if (String.IsNullOrEmpty(cust.WCFURL_Ext))
                            //{
                            //    throw new ApplicationException(String.Format("No WCF URL set for customer {0}.", dtSD.Rows[i]["CustomerID"].ToString()));
                            //}
                            //else
                            //{
                            CustomerServiceClient client = GetServiceClient(WCFUrl, VersionNo);
                            if (dtSD.Rows[i]["SysTrxNo"] != null && dtSD.Rows[i]["SysTrxNo"].ToString() != string.Empty)
                            {
                                Systrxno = Convert.ToInt32(dtSD.Rows[i]["SysTrxNo"].ToString());
                            }
                            byte[] image = null;
                            byte[] imagepdf = null;
                            try
                            {

                                if (!String.IsNullOrWhiteSpace(dtSD.Rows[i]["Image"].ToString()))
                                {
                                    //image = Encoding.ASCII.GetBytes(bolItems.Rows[j]["Image"].ToString());
                                    //image = System.Convert.FromBase64String(bolItems.Rows[j]["Image"].ToString());
                                    image = (byte[])dtSD.Rows[i]["Image"];

                                }

                                //if (!String.IsNullOrWhiteSpace(dtSD.Rows[i]["Imagepdf"].ToString()))
                                //{
                                //    imagepdf = (byte[])dtSD.Rows[i]["Imagepdf"];
                                //}

                            }
                            catch
                            {
                                image = null;
                                //imagepdf = null;
                            }
                            client.UpdateBOLImage(customerID, Password, image, imagepdf, Systrxno, dtSD.Rows[i]["BOLNo"].ToString(), VersionNo);
                            DALMethods.UpdateNeedBOLImageUpdate(new Guid(dtSD.Rows[i]["ID"].ToString()), session, VersionNo);
                            //}
                        }
                        catch (Exception ex)
                        {
                            Logging.WriteLog(String.Format("Error for CustomerID : {0}, SysTrxNo : {1} ", dtSD.Rows[i]["CustomerID"].ToString(), dtSD.Rows[i]["SysTrxNo"].ToString()), EventLogEntryType.Error);
                            Logging.LogError(ex);
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                CloseSession(session);
                Logging.LogError(ex);
            }
            finally
            {
                if (session != null)
                    CloseSession(session);
            }
        }

        public static void SendEmailforRejectedLoads(string customerID)
        {
            ISession session = null;
            try
            {
                session = GetNewSession();
                DataTable dtRejectedLoads = DALMethods.GetRejectedLoads(customerID, session, VersionNo);
                if (dtRejectedLoads != null && dtRejectedLoads.Rows.Count > 0)
                {
                    for (var i = 0; i < dtRejectedLoads.Rows.Count; i++)
                    {
                        try
                        {
                            string IsEnabledNotificationRequired = DALMethods.GetIsEnabledNotifyRequired(dtRejectedLoads.Rows[i]["CustomerID"].ToString(), session, VersionNo);
                            if (IsEnabledNotificationRequired.ToUpper().Trim() == "Y")
                            {
                                if (dtRejectedLoads.Rows[i]["DispatcherEmailID"].ToString() != "" && dtRejectedLoads.Rows[i]["DispatcherEmailID"] != null)
                                {
                                    MailMessage Msg = new MailMessage();
                                    // Sender e-mail address.
                                    Msg.From = new MailAddress("karthiksweb@firestreamww.com");

                                    // Recipient e-mail address.
                                    Msg.To.Add(new MailAddress(dtRejectedLoads.Rows[i]["DispatcherEmailID"].ToString()));
                                    Msg.Subject = "Rejected Load";
                                    Msg.Body = "Load No : " + dtRejectedLoads.Rows[i]["LoadNo"].ToString() + " <br /> Reason : " + dtRejectedLoads.Rows[i]["RejectedNotes"].ToString();
                                    Msg.IsBodyHtml = true;

                                    // your remote SMTP server IP.
                                    SmtpClient smtp = new SmtpClient(EMAIL_SERVER);
                                    smtp.Credentials = new NetworkCredential(EMAIL_NAME, EMAIL_PASSWORD);
                                    try
                                    {
                                        smtp.Send(Msg);
                                    }
                                    catch (SmtpFailedRecipientsException ex)
                                    {
                                        for (int j = 0; j <= ex.InnerExceptions.Length; j++)
                                        {
                                            SmtpStatusCode status = ex.InnerExceptions[j].StatusCode;
                                            if ((status == SmtpStatusCode.MailboxBusy) || (status == SmtpStatusCode.MailboxUnavailable))
                                            {
                                                System.Threading.Thread.Sleep(5000);
                                                smtp.Send(Msg);
                                            }
                                        }
                                    }
                                    DALMethods.UpdateSentEmailFlag(new Guid(dtRejectedLoads.Rows[i]["ID"].ToString()), dtRejectedLoads.Rows[i]["CustomerID"].ToString(), session, VersionNo);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Logging.WriteLog(String.Format("Error for CustomerID : {0}, Dispatcher EmailID : {1} , LoadID : {2} ", dtRejectedLoads.Rows[i]["CustomerID"].ToString(), dtRejectedLoads.Rows[i]["DispatcherEmailID"].ToString(), dtRejectedLoads.Rows[i]["ID"].ToString()), EventLogEntryType.Error);
                            Logging.LogError(ex);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                CloseSession(session);
                Logging.LogError(ex);
            }
            finally
            {
                if (session != null)
                    CloseSession(session);
            }
        }

        public static string GetMeaningfullMsg(string ErrorMessage)
        {
            string MeaningfullMsg = string.Empty;

            if (ErrorMessage.Contains("Cannot insert the value NULL into column 'MasterProdID'"))
            {
                MeaningfullMsg = "Order in Ascend has been modified/deleted.";
            }
            if (ErrorMessage.Contains("Cannot insert duplicate key row in object 'dbo.BOLHdr' with unique index 'idxBOLNO_DtTm'"))
            {
                MeaningfullMsg = "BOL has already been posted.";
            }
            if (ErrorMessage.Contains("Shipments: Item is already invoiced!"))
            {
                MeaningfullMsg = "Shipment Item is already invoiced!";
            }
            if (ErrorMessage.Contains("has a different Vehicle!"))
            {
                MeaningfullMsg = "Existing BOL has a different Vehicle.";
            }
            if (ErrorMessage.Contains("has a different CarrierID!"))
            {
                MeaningfullMsg = "Existing BOL has a different Carrier.";
            }
            if (ErrorMessage.Contains("Missing Supp/Supt/Prod"))
            {
                MeaningfullMsg = "PurchRack Price for the Supplier/SupplyPt/Product Combination is Missing!";
            }
            if (ErrorMessage.Contains("is not set up at pass through site"))
            {
                MeaningfullMsg = "SiteProdContainer Settings is Missing!";
            }


            return MeaningfullMsg;
        }

        /// <summary>
        /// UpdatePONo
        /// Function to update UpdatePONo
        /// </summary>
        public static void UpdatePONo(string customerID)
        {
            ISession session = null;
            try
            {
                session = GetNewSession();
                DataTable dtSD = DALMethods.GetDataToUpdatePONo(customerID, session, VersionNo);
                if (dtSD != null && dtSD.Rows.Count > 0)
                {
                    for (var i = 0; i < dtSD.Rows.Count; i++)
                    {
                        try
                        {
                            int Systrxno = 0;
                            string PONo = string.Empty;
                            string OrderNo = string.Empty;
                            //DAL.CustomerRow cust = DALMethods.GetCustomer(dtSD.Rows[i]["CustomerID"].ToString(), session, VersionNo);

                            //if (String.IsNullOrEmpty(cust.WCFURL_Ext))
                            //{
                            //    throw new ApplicationException(String.Format("No WCF URL set for customer {0}.", dtSD.Rows[i]["CustomerID"].ToString()));
                            //}
                            //else
                            //{
                            CustomerServiceClient client = GetServiceClient(WCFUrl, VersionNo);
                            if (dtSD.Rows[i]["SysTrxNo"] != null && dtSD.Rows[i]["SysTrxNo"].ToString() != string.Empty)
                            {
                                Systrxno = Convert.ToInt32(dtSD.Rows[i]["SysTrxNo"].ToString());
                            }
                            if (dtSD.Rows[i]["OrderNo"] != null && dtSD.Rows[i]["OrderNo"].ToString() != string.Empty)
                            {
                                OrderNo = dtSD.Rows[i]["OrderNo"].ToString();
                            }
                            if (dtSD.Rows[i]["PONo"] != null && dtSD.Rows[i]["PONo"].ToString() != string.Empty)
                            {
                                PONo = dtSD.Rows[i]["PONo"].ToString();
                            }
                            client.UpdateOrderPONo(customerID, Password, OrderNo, Systrxno, PONo, VersionNo);
                            DALMethods.UpdateNeedPONoUpdate(new Guid(dtSD.Rows[i]["ID"].ToString()), session, VersionNo);
                            //}
                        }
                        catch (Exception ex)
                        {
                            Logging.WriteLog(String.Format("Error for CustomerID : {0}, SysTrxNo : {1} ", dtSD.Rows[i]["CustomerID"].ToString(), dtSD.Rows[i]["SysTrxNo"].ToString()), EventLogEntryType.Error);
                            Logging.LogError(ex);
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                CloseSession(session);
                Logging.LogError(ex);
            }
            finally
            {
                if (session != null)
                    CloseSession(session);
            }
        }



        public static void UpdateRejectedLoads(string customerID)
        {

            ISession session = GetNewSession();
            string loadNo = string.Empty;
            //string customerID = string.Empty;
            try
            {

                DataTable dtRejectedLoads = DALMethods.FetchRejectedLoads(customerID, session);
                //Logging.WriteToFile1(string.Format("FetchRejectedLoads is called at {0} and rowcount = {1}", DateTime.Now, dtRejectedLoads.Rows.Count));
                foreach (DataRow drRejectedLoad in dtRejectedLoads.Rows)
                {
                    loadNo = drRejectedLoad["LoadNo"].ToString();
                    //customerID = drRejectedLoad["CustomerID"].ToString();
                    //DAL.CustomerRow cust = DALMethods.GetCustomer(drRejectedLoad["CustomerID"].ToString(), session, VersionNo);

                    //if (String.IsNullOrEmpty(cust.WCFURL_Ext))
                    //{
                    //    throw new ApplicationException(String.Format("No WCF URL set for customer {0}.", drRejectedLoad["CustomerID"].ToString()));
                    //}
                    //else
                    //{
                    CustomerServiceClient client = GetServiceClient(WCFUrl, VersionNo);
                    client.UpdateRejectedLoads(loadNo, drRejectedLoad["RejectedNotes"].ToString(), customerID);
                    DALMethods.UpdateNeedUpdate(session, loadNo);
                    //}

                }
            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex.Message, EventLogEntryType.Error);
            }
            finally
            {
                if (session != null)
                    CloseSession(session);
            }
        }

        public static void UpdateDeliveryNotes(string customerID)
        {
            ISession session = GetNewSession();
            string sysTrsNo = string.Empty;
            //string customerID = string.Empty;
            string orderId = string.Empty;
            try
            {

                DataTable dtDeliveryNotes = DALMethods.FetchDeliveryNotes(customerID, session);
                //Logging.WriteToFile1(string.Format("FetchDeliveryNotes is called at {0} and rowcount = {1}", DateTime.Now, dtDeliveryNotes.Rows.Count));
                foreach (DataRow drDeliveryNotes in dtDeliveryNotes.Rows)
                {
                    sysTrsNo = drDeliveryNotes["SysTrxNo"].ToString();
                    orderId = drDeliveryNotes["OrderID"].ToString();
                    // customerID = drDeliveryNotes["CustomerID"].ToString();                   
                    //DAL.CustomerRow cust = DALMethods.GetCustomer(drDeliveryNotes["CustomerID"].ToString(), session, VersionNo);

                    //if (String.IsNullOrEmpty(cust.WCFURL_Ext))
                    //{
                    //    throw new ApplicationException(String.Format("No WCF URL set for customer {0}.", drDeliveryNotes["CustomerID"].ToString()));
                    //}
                    //else
                    //{
                    //Logging.WriteToFile1(string.Format("Delivery notes for SysTrsNo no {0} - {1}", sysTrsNo, drDeliveryNotes["NOTES"].ToString()));
                    CustomerServiceClient client = GetServiceClient(WCFUrl, VersionNo);
                    client.UpdateDeliveryNotes(sysTrsNo, drDeliveryNotes["NOTES"].ToString());
                    DALMethods.UpdateSyncFlagDeliveryNotes(session, orderId);
                    //Logging.WriteToFile1(string.Format("Delivery notes is updated for SysTrsNo no {0}", sysTrsNo));
                    //}
                }
            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex.Message, EventLogEntryType.Error);
            }
        }

        public static string MergeCustomerID(string CustomerID)
        {
            string cutomerid = "";

            if (CustomerID.Contains(","))
            {
                string[] sptcutomerID = CustomerID.Split(',');

                for (int i = 0; i < sptcutomerID.Length; i++)
                {
                    if (i == 0)
                        cutomerid = "'" + sptcutomerID[i].Trim() + "'";/* + ",";*/
                    else
                        cutomerid = cutomerid + "," + "'" + sptcutomerID[i].Trim() + "'" + ",";
                }
            }
            else
            {
                cutomerid = CustomerID;
            }

            return cutomerid.TrimEnd(',');
        }

        public static bool IsWCFAvailableForCustomer(string CustomerID)
        {
            ISession session = null;
            string Message = string.Empty;
            try
            {
                session = GetNewSession();

                DAL.CustomerRow cust = DALMethods.GetCustomer(CustomerID, session, VersionNo);

                if (cust != null)
                {
                    WCFUrl = cust.WCFURL_Ext;
                    Password = cust.Password_Ext;
                }

                if (String.IsNullOrEmpty(WCFUrl))
                {
                    throw new ApplicationException(String.Format("No WCF URL set for customer {0}.", CustomerID.ToString()));
                }

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(WCFUrl);

                request.Credentials = System.Net.CredentialCache.DefaultCredentials;
                request.Method = "GET";

                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {

                    }
                }
                catch (WebException ex)
                {
                    Logging.WriteLog(ex.Message + " " + CustomerID, EventLogEntryType.Error);
                    Message += ((Message.Length > 0) ? "\n" : "") + ex.Message;
                }

                CloseSession(session);
            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex.Message, EventLogEntryType.Error);
                Message += ((Message.Length > 0) ? "\n" : "") + ex.Message;
            }
            finally
            {
                if (session != null)
                    CloseSession(session);
            }

            return (Message.Length == 0);
        }
    }
}
