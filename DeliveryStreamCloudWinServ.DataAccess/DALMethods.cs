// 2014.01.03 FSWW, Ramesh M Added For CR#61563
// 2014.01.06 FSWW, Ramesh M Added VersionNo as input parameter in all methods For Versioning handling
// 2014.03.31 Ramesh M Added modified SQL statement to support multi line item of ship doc CR#62905
// 2014.07.11 MadhuVenkat K Modified SQL statement for CR#64226-ShipDocs not created when an order item is deleted in ASCEND and dispatched to the DeliveryStream. 
// 2014.10.17 MadhuVenkat K Modified SQL statement for CR#65002-Changing From Site in Shipment Processing.
// 2014.10.18 MadhuVenkat K Modified SQL statement for CR#64905-App crashes when we process multi BOL
// 2014.11.25 MadhuVenkat K Modified SQL statement for CR#65604 - Application is updating null for Supplier Code/ Supply Point in the BOLHdr for shipments created from iPAD app.
// 2015.02.02 MadhuVenkat K Modified SQL statement for CR#66165-App Order Processing fails when an order is processed by previous App version.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeliveryStreamCloudWinServ.DataAccess.DALTableAdapters;
using System.Data.SqlClient;
using System.Data;
using DeliveryStreamCloudWinServ;

namespace DeliveryStreamCloudWinServ.DataAccess
{
    /// <summary>
    /// DALMethods class
    /// </summary>
    public class DALMethods
    {
        #region GetDetails

        /// <summary>
        /// GetCustomer
        /// Function to get the customer record related to customer id
        /// </summary>
        /// <param name="customerID">Customer ID</param>
        /// <param name="session">Session object</param>
        /// <returns>customer row object</returns>
        public static DAL.CustomerRow GetCustomer(String customerID, ISession session, String VersionNo = "")
        {
            CustomerTableAdapter ta = new CustomerTableAdapter();
            ta.CurrentConnection = session;
            List<DAL.CustomerRow> lstCust = ta.GetCustomer(customerID).ToList();
            if (lstCust.Count > 0)
            {
                return lstCust[0];
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// GetUpdatedLoad
        /// Function to return list of load records
        /// </summary>
        /// <param name="session">Session object</param>
        /// <returns>List of load</returns>
        public static List<DAL.LoadRow> GetUpdatedLoad(ISession session, String VersionNo = "")
        {
            LoadTableAdapter ta = new LoadTableAdapter();
            ta.CurrentConnection = session;
            return ta.GetUpdatedData().ToList();
        }

        /// <summary>
        /// GetUndispatchedLoad
        /// Function to return list of Undispatched load records
        /// </summary>
        /// <param name="session">Session object</param>
        /// <returns>List of load</returns>
        public static List<DAL.LoadRow> GetUndispatchedLoad(string customerID, ISession session, String VersionNo = "")
        {
            LoadTableAdapter ta = new LoadTableAdapter();
            ta.CurrentConnection = session;
            return ta.GetDataByUndispatched(customerID).ToList();
        }


        /// <summary>
        /// UpdateLoad
        /// Function to update the load records
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="session">Session object</param>
        public static void UpdateLoad(Guid id, ISession session, String VersionNo = "")
        {
            LoadTableAdapter ta = new LoadTableAdapter();
            ta.CurrentConnection = session;
            ta.UpdateQuery(id);
        }

        /// <summary>
        /// UpdateLoadNeedUpdate
        /// Function to update the load records
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="session">Session object</param>
        public static void UpdateLoadNeedUpdate(Guid id, ISession session, String VersionNo = "")
        {
            LoadTableAdapter ta = new LoadTableAdapter();
            ta.CurrentConnection = session;
            ta.UpdateLoadNeedUpdate(id);
        }

        /// <summary>
        /// DeleteLoad
        /// Function to Delete the load records
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="session">Session object</param>
        public static void DeleteLoad(Guid id, ISession session, String VersionNo = "")
        {
            LoadTableAdapter ta = new LoadTableAdapter();
            ta.CurrentConnection = session;
            ta.DeleteQuery(id);
        }

        /// <summary>
        /// GetUpdatedLoadStatusHistory
        /// Function to return list of Load status history records
        /// </summary>
        /// <param name="session">Session object</param>
        /// <returns>List of Load status history</returns>
        public static List<DAL.LoadStatusHistoryRow> GetUpdatedLoadStatusHistory(string customerID, ISession session, String VersionNo = "")
        {
            LoadStatusHistoryTableAdapter ta = new LoadStatusHistoryTableAdapter();
            ta.CurrentConnection = session;
            return ta.GetUpdatedData(customerID).ToList();
        }

        /// <summary>
        /// UpdateLoadStatusHistory
        /// Function to update the Load status history records
        /// </summary>
        /// <param name="loadID">Load ID</param>
        /// <param name="statusID">Status ID</param>
        /// <param name="session">Session object</param>
        public static void UpdateLoadStatusHistory(Guid loadID, String statusID, ISession session, String VersionNo = "")
        {
            LoadStatusHistoryTableAdapter ta = new LoadStatusHistoryTableAdapter();
            ta.CurrentConnection = session;
            ta.UpdateQuery(loadID, statusID);
        }

        /// <summary>
        /// GetUpdatedOrderStatusHistory
        /// Function to return list of Order status history records
        /// </summary>
        /// <param name="session">Session object</param>
        /// <returns>List of Order status history</returns>
        public static List<DAL.OrderStatusHistoryRow> GetUpdatedOrderStatusHistory(string customerID, ISession session, String VersionNo = "")
        {
            OrderStatusHistoryTableAdapter ta = new OrderStatusHistoryTableAdapter();
            ta.CurrentConnection = session;
            return ta.GetUpdatedData(customerID).ToList();
        }

        /// <summary>
        /// UpdateOrderStatusHistory
        /// Function to update the order status history records
        /// </summary>
        /// <param name="orderID">Order ID</param>
        /// <param name="statusID">Status ID</param>
        /// <param name="session">Session object</param>
        public static void UpdateOrderStatusHistory(Guid orderID, String statusID, ISession session, String VersionNo = "")
        {
            OrderStatusHistoryTableAdapter ta = new OrderStatusHistoryTableAdapter();
            ta.CurrentConnection = session;
            ta.UpdateQuery(orderID, statusID);
        }

        /// <summary>
        /// GetShipment Data
        /// </summary>
        /// <param name="session">session Object</param>
        /// <returns>List of ShipmentData which need to ship at customer end</returns>
        public static List<DAL.ShipmentDataRow> GetShipmentData(Int32 maxRetryCount, ISession session, String VersionNo = "")
        {
            ShipmentDataTableAdapter ta = new ShipmentDataTableAdapter();
            ta.CurrentConnection = session;
            ta.Adapter.SelectCommand = new System.Data.SqlClient.SqlCommand();
            ta.Adapter.SelectCommand.CommandTimeout = 90;
            return ta.GetShipmentData(maxRetryCount).ToList();
        }



        public static DataTable GetShipmentData1(string cutomerID, Int32 maxRetryCount, ISession session, String VersionNo = "")
        {
            SqlCommand cmd = (SqlCommand)session.CreateCommand();
            cmd.CommandText = "Cloud_GetShipmentData";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MaxRetryCount", maxRetryCount);
            cmd.Parameters.AddWithValue("@CustomerID", cutomerID);
            cmd.CommandTimeout = 30;
            //SqlConnection con = new SqlConnection(session.ConnectionString);
            //con.Open();
            SqlDataAdapter sa = new SqlDataAdapter();
            sa.SelectCommand = cmd;
            DataTable dt = new DataTable();
            sa.Fill(dt);
            //con.Close();
            return dt;
        }


        /// <summary>
        /// Function to Increate Retry Count of BOLItem
        /// </summary>
        /// <param name="customerID">Customer ID</param>
        /// <param name="sysTrxNo">sysTrxNo</param>
        /// <param name="sysTrxLine">sysTrxLine</param>
        /// <param name="session">session object</param>
        public static void BOLItemIncreateRetryCount(String customerID, Decimal sysTrxNo, Int32 sysTrxLine, ISession session, String VersionNo = "")
        {
            ShipmentDataTableAdapter ta = new ShipmentDataTableAdapter();
            ta.CurrentConnection = session;
            ta.IncreateRetryCount(customerID, sysTrxNo, sysTrxLine);
        }
        // 2014.10.17 MadhuVenkat K Modified SQL statement for CR#65002-Changing From Site in Shipment Processing.
        // 2014.10.18 MadhuVenkat K Modified SQL statement for CR#64905-App crashes when we process multi BOL
        // 2014.11.25 MadhuVenkat K Modified SQL statement for CR#65604 - Application is updating null for Supplier Code/ Supply Point in the BOLHdr for shipments created from iPAD app.

        /// <summary>
        /// Function to return list of BOL item
        /// </summary>
        /// <param name="customerID">Customer ID</param>
        /// <param name="sysTrxNo">SysTrxNo</param>
        /// <param name="sysTrxLine">sysTrxLine</param>
        /// <param name="session">session object</param>
        /// <returns>list of BOL item</returns>
        public static List<DAL.BOLItemRow> GetUpdatedBolItem(String customerID, Decimal sysTrxNo, Int32 sysTrxLine, ISession session, String VersionNo = "")
        {
            BOLItemTableAdapter ta = new BOLItemTableAdapter();
            ta.CurrentConnection = session;
            return ta.GetUpdatedData(customerID, sysTrxNo, sysTrxLine).ToList();
        }

        public static DataTable GetUpdatedBolItem1(String customerID, Decimal sysTrxNo, Int32 sysTrxLine, String BOLNo, ISession session, String VersionNo = "")
        {
            //BOLItemTableAdapter ta = new BOLItemTableAdapter();
            //ta.CurrentConnection = session;
            //return ta.GetUpdatedData(customerID, sysTrxNo, sysTrxLine).ToList();

            SqlCommand cmd = (SqlCommand)session.CreateCommand();
            cmd.CommandText = "Cloud_GetUpdatedBolItem";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@SysTrxNo", sysTrxNo);
            cmd.Parameters.AddWithValue("@SysTrxLine", sysTrxLine);
            cmd.Parameters.AddWithValue("@BOLNo", BOLNo);
            //SqlConnection con = new SqlConnection(session.ConnectionString);
            //con.Open();
            SqlDataAdapter sa = new SqlDataAdapter();
            sa.SelectCommand = cmd;
            DataTable dt = new DataTable();
            sa.Fill(dt);
            //con.Close();
            return dt;
        }

        /// <summary>
        /// Function to Add ShipmentEventLog
        /// </summary>
        /// <param name="sysTrxNo">sysTrxNo</param>
        /// <param name="sysTrxLine">sysTrxLine</param>
        /// <param name="session">session object</param>
        public static void AddShipmentEventLog(String CustomerID, Guid LoadID, Guid BOLHdrID, Guid BOLItemID, DateTime BOLDateTime, decimal SysTrxNo, int SysTrxLine, int ComponentNo, decimal GrossQty, decimal NetQty, int ExtSysTrxLine, string ErrorMessage, string MeaningFullMsg, ISession session, String VersionNo = "")
        {
            try
            {
                SqlCommand cmd = (SqlCommand)session.CreateCommand();
                cmd.CommandText = "Cloud_AddShipmentEventLog";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmd.Parameters.AddWithValue("@LoadID", LoadID);
                cmd.Parameters.AddWithValue("@BOLHdrID", BOLHdrID);
                cmd.Parameters.AddWithValue("@BOLItemID", BOLItemID);
                cmd.Parameters.AddWithValue("@BOLDateTime", BOLDateTime);
                cmd.Parameters.AddWithValue("@SysTrxNo", SysTrxNo);
                cmd.Parameters.AddWithValue("@SysTrxLine", SysTrxLine);
                cmd.Parameters.AddWithValue("@ComponentNo", ComponentNo);
                cmd.Parameters.AddWithValue("@GrossQty", GrossQty);
                cmd.Parameters.AddWithValue("@NetQty", NetQty);
                cmd.Parameters.AddWithValue("@ExtSysTrxLine", ExtSysTrxLine);
                cmd.Parameters.AddWithValue("@ErrorMessage", ErrorMessage);
                cmd.Parameters.AddWithValue("@MeaningFullMsg", MeaningFullMsg);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// UpdateBolItem
        /// Function to update the BOL item records
        /// </summary>
        /// <param name="shipDocSysTrxNo">Shipment doc system transaction number</param>
        /// <param name="shipDocSysTrxLine">Shipment doc system transaction line</param>
        /// <param name="id">Id</param>
        /// <param name="session">Session object</param>
        public static void UpdateBolItem(Decimal? shipDocSysTrxNo, Int32? shipDocSysTrxLine, Guid id, ISession session, String VersionNo = "")
        {
            BOLItemTableAdapter ta = new BOLItemTableAdapter();
            ta.CurrentConnection = session;
            // 2014.03.31 Ramesh M Added modified SQL statement to support multi line item of ship doc
            ta.UpdateQuery(shipDocSysTrxNo, shipDocSysTrxLine, id);
        }

        /// <summary>
        /// GetUpdatedDeliveryOrders
        /// Function to return list of delivery orders records
        /// </summary>
        /// <param name="session">Session object</param>
        /// <returns>List of delivery orders</returns>
        public static List<DAL.DeliveryOrdersRow> GetUpdatedDeliveryOrders(Int32 maxRetryCount, ISession session, String VersionNo = "")
        {
            DeliveryOrdersTableAdapter ta = new DeliveryOrdersTableAdapter();
            ta.CurrentConnection = session;
            return ta.GetData(maxRetryCount).ToList();
        }

        public static DataTable GetUpdatedDeliveryOrders1(string customerID, Int32 maxRetryCount, ISession session, String VersionNo = "")
        {
            SqlCommand cmd = (SqlCommand)session.CreateCommand();
            cmd.CommandText = "Cloud_GetUpdatedDeliveryOrders";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MaxRetryCount", maxRetryCount);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            //SqlConnection con = new SqlConnection(session.ConnectionString);
            //con.Open();
            SqlDataAdapter sa = new SqlDataAdapter();
            sa.SelectCommand = cmd;
            DataTable dt = new DataTable();
            sa.Fill(dt);
            //con.Close();
            return dt;

        }

        /// <summary>
        /// Function to Increate Retry Count of Delivery Details 
        /// </summary>
        /// <param name="customerID">CustomerID</param>
        /// <param name="loadID">loadID</param>
        /// <param name="sysTrxNo">sysTrxNo</param>
        /// <param name="session">session object</param>
        public static void DeliveryDetailIncreaseRetryCount(String customerID, Guid loadID, decimal sysTrxNo, ISession session, String VersionNo = "")
        {
            DeliveryOrdersTableAdapter ta = new DeliveryOrdersTableAdapter();
            ta.CurrentConnection = session;
            ta.IncreaseRetryCount(loadID, sysTrxNo, customerID);
        }
        // 2015.02.02 MadhuVenkat K Modified SQL statement for CR#66165-App Order Processing fails when an order is processed by previous App version.
        /// <summary>
        /// GetUpdatedDeliveryDetails
        /// Function to return list of delivery details records
        /// </summary>
        /// <param name="customerID">customer id</param>
        /// <param name="loadID">load id</param>
        /// <param name="sysTrxNo">system trx no</param>
        /// <param name="session">Session object</param>
        /// <returns>List of delivery details</returns>
        public static List<DAL.DeliveryDetailsRow> GetUpdatedDeliveryDetails(String customerID, Guid loadID, decimal sysTrxNo, ISession session, String VersionNo = "")
        {
            DeliveryDetailsTableAdapter ta = new DeliveryDetailsTableAdapter();
            ta.CurrentConnection = session;
            return ta.GetUpdatedData(customerID, loadID, sysTrxNo).ToList();
        }

        public static DataTable GetUpdatedDeliveryDetails1(String customerID, Guid loadID, decimal sysTrxNo, ISession session, String VersionNo = "")
        {
            SqlCommand cmd = (SqlCommand)session.CreateCommand();
            //cmd.CommandText = "Cloud_GetUpdatedDeliveryDetails";
            //cmd.CommandText = "Cloud_GetDeliveryItemDetails";
            cmd.CommandText = "Cloud_GetDeliveryItemDetailsDavidson";//for Davidson
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@LoadID", loadID);
            cmd.Parameters.AddWithValue("@SysTrxNo", sysTrxNo);
            cmd.CommandTimeout = 40;
            //SqlConnection con = new SqlConnection(session.ConnectionString);
            //con.Open();
            SqlDataAdapter sa = new SqlDataAdapter();
            sa.SelectCommand = cmd;
            DataTable dt = new DataTable();
            sa.Fill(dt);
            //con.Close();
            return dt;
        }

        /// <summary>
        /// GetFrightDetails
        /// </summary>
        /// <param name="orderId">Order Id</param>
        /// <param name="session">Session Object</param>
        /// <returns>OrderFrtRow</returns>
        public static DAL.OrderFrtRow GetFrightDetails(Guid orderId, ISession session, String VersionNo = "")
        {
            OrderFrtTableAdapter ta = new OrderFrtTableAdapter();
            ta.CurrentConnection = session;
            List<DAL.OrderFrtRow> rows = ta.GetDataByOrderID(orderId).ToList();
            if (rows.Count > 0)
            {
                return rows[0];
            }

            return null;
        }
        // 2013.12.09 FSWW, Ramesh M Added For CR#60647
        /// <summary>
        /// GetLoginHistoryDetails
        /// </summary>
        /// <param name="SessionID"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public static List<DAL.LoginHistoryRow> GetLoginHistoryDetails(Guid SessionID, ISession session, String VersionNo = "")
        {
            LoginHistoryTableAdapter ta = new LoginHistoryTableAdapter();
            ta.CurrentConnection = session;
            return ta.GetDataBySessionID(SessionID).ToList();
        }

        /// <summary>
        /// UpdateDeliveryDetails
        /// Function to update the delivery details records
        /// </summary>
        /// <param name="orderItemID">Order item ID</param>
        /// <param name="session">Session object</param>
        //public static void UpdateDeliveryDetails(Guid orderItemID, ISession session, String VersionNo = "")
        //{
        //    DeliveryDetailsTableAdapter ta = new DeliveryDetailsTableAdapter();
        //    ta.CurrentConnection = session;
        //    ta.UpdateQuery(orderItemID);
        //}
        public static void UpdateDeliveryDetails(Guid orderItemID, ISession session, String VersionNo = "")
        {
            DeliveryDetailsTableAdapter ta = new DeliveryDetailsTableAdapter();
            ta.CurrentConnection = session;
            ta.UpdateQuery(orderItemID);

            //SqlCommand cmd = (SqlCommand)session.CreateCommand();
            //cmd.CommandText = "Cloud_UpdateNeedUpdateDeliveryDetails";
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@orderItemID", orderItemID);
            //cmd.ExecuteNonQuery();
        }

        #endregion

        #region Loutout

        public static void AutoLogOut(double logOutTime, ISession session, String VersionNo = "")
        {
            AutoLogoffUserTableAdapter ta = new AutoLogoffUserTableAdapter();
            ta.CurrentConnection = session;
            ta.AutoLogoffUser((decimal?)logOutTime);
        }

        #endregion

        #region GPSHistory

        public static List<DAL.GPSHistoryRow> GPSHistoryData(ISession session, String VersionNo = "")
        {
            GPSHistoryTableAdapter ta = new GPSHistoryTableAdapter();
            ta.CurrentConnection = session;
            return ta.GetDateByState().ToList();
        }

        public static void UpdateState(string state, string longitude, string latitude, ISession session, String VersionNo = "")
        {
            GPSHistoryTableAdapter ta = new GPSHistoryTableAdapter();
            ta.CurrentConnection = session;
            ta.UpdateState(state, longitude, latitude);
        }
        // 2014.01.03 FSWW, Ramesh M Added For CR#61563
        public static void UpdateStateCodeInSQL(ISession session, String VersionNo = "")
        {
            GPSHistoryTableAdapter ta = new GPSHistoryTableAdapter();
            ta.CurrentConnection = session;
            ta.UpdateLocationStateUsingSQL();
        }

        public static string IsEnabledFrkBrkdown(string CompanyID, ISession session, String VersionNo = "")
        {
            string IsEnabledFrkBrk = "";
            //Cloud_IsEnabledFrtLoadorUnloadBrkdownTableAdapter ta = new Cloud_IsEnabledFrtLoadorUnloadBrkdownTableAdapter();
            //ta.CurrentConnection = session;
            //DataTable dt = ta.GetDataIsEnabledFrtBrkdown(CompanyID);
            //if (dt.Rows.Count > 0)
            //{
            //    IsEnabledFrkBrk = dt.Rows[0]["Result"].ToString();
            //}
            SqlCommand cmd = (SqlCommand)session.CreateCommand();
            cmd.CommandText = "Cloud_IsEnabledFrtLoadorUnloadBrkdown";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ClientID", CompanyID);
            SqlConnection con = new SqlConnection(session.ConnectionString);
            con.Open();
            SqlDataAdapter sa = new SqlDataAdapter();
            sa.SelectCommand = cmd;
            DataTable dt = new DataTable();
            sa.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
            {
                IsEnabledFrkBrk = dt.Rows[0]["Result"].ToString();
            }
            return IsEnabledFrkBrk;
        }

        /// <summary>
        /// GetFreightBreakdownDetails
        /// Function to return list of Freight Breakdown Details
        /// </summary>
        /// <param name="session">Session object</param>
        /// <returns>List of Freight Breakdown Details</returns>
        public static List<DAL.Cloud_GetMilesByLoadRow> GetFreightBreakdownDetails(String CustomerID, String LoadNo, ISession session, String VersionNo = "")
        {
            Cloud_GetMilesByLoadTableAdapter ta = new Cloud_GetMilesByLoadTableAdapter();
            ta.CurrentConnection = session;
            return ta.GetMilesByLoad(CustomerID, LoadNo).ToList();
        }
        //2015-dec-28 , Vinoth added for send email for rejected loads
        public static DataTable GetRejectedLoads(string customerID, ISession session, String VersionNo = "")
        {
            SqlCommand cmd = (SqlCommand)session.CreateCommand();
            cmd.CommandText = "Cloud_GetRejectedLoads";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            SqlConnection con = new SqlConnection(session.ConnectionString);
            con.Open();
            SqlDataAdapter sa = new SqlDataAdapter();
            sa.SelectCommand = cmd;
            DataTable dt = new DataTable();
            sa.Fill(dt);
            con.Close();
            return dt;
        }
        //2015-dec-28 , Vinoth added for get NotificationRequired flag for send email of rejected loads
        public static string GetIsEnabledNotifyRequired(string CompanyID, ISession session, String VersionNo = "")
        {
            string IsEnabledNotifyRequired = "";
            SqlCommand cmd = (SqlCommand)session.CreateCommand();
            cmd.CommandText = "Cloud_IsEnabledNotificationRequired";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ClientID", CompanyID);
            SqlConnection con = new SqlConnection(session.ConnectionString);
            con.Open();
            SqlDataAdapter sa = new SqlDataAdapter();
            sa.SelectCommand = cmd;
            DataTable dt = new DataTable();
            sa.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
            {
                IsEnabledNotifyRequired = dt.Rows[0]["Result"].ToString();
            }
            return IsEnabledNotifyRequired;
        }

        public static DataTable UpdateSentEmailFlag(Guid LoadID, String CustomerID, ISession session, String VersionNo = "")
        {
            SqlCommand cmd = (SqlCommand)session.CreateCommand();
            cmd.CommandText = "Cloud_UpdateSentEmailFlag";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@LoadID", LoadID);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            SqlConnection con = new SqlConnection(session.ConnectionString);
            con.Open();
            SqlDataAdapter sa = new SqlDataAdapter();
            sa.SelectCommand = cmd;
            DataTable dt = new DataTable();
            sa.Fill(dt);
            con.Close();
            return dt;
        }

        #endregion

        #region TankWagon Details


        /// <summary>
        /// GetBOLHdr Data
        /// </summary>
        /// <param name="session">session Object</param>
        /// <returns>List of ShipmentData which need to ship at customer end</returns>
        public static List<DAL.EODInventoryProcessRow> GetEODClientID(ISession session, String VersionNo = "")
        {
            EODInventoryProcessTableAdapter ta = new EODInventoryProcessTableAdapter();
            ta.CurrentConnection = session;
            return ta.GetDataByClientID().ToList();
        }

        /// <summary>
        /// GetBOLHdr Data
        /// </summary>
        /// <param name="session">session Object</param>
        /// <returns>List of ShipmentData which need to ship at customer end</returns>
        public static List<DAL.EODInventoryProcessRow> GetDataEODInventoryProcess(String CompanyID, ISession session, String VersionNo = "")
        {
            EODInventoryProcessTableAdapter ta = new EODInventoryProcessTableAdapter();
            ta.CurrentConnection = session;
            return ta.GetDataEODInventoryProcess(CompanyID).ToList();
        }


        /// <summary>
        /// UpdateBOLNeedUpdate
        /// Function to update the BOL records
        /// </summary>

        public static void UpdateBOLNeedUpdate(Int32 ToSiteID, String SupplierCode, String SupplyPointCode, String ProdCode, Decimal OrderedQty, Decimal AvailableQty, Int32 RetainedVehicleID, Int32 DriverID, String BOLNO, Int32 UpdatedBy, ISession session, String VersionNo = "")
        {
            Cloud_TW_UpdateBOLNeedUpdateTableAdapter ta = new Cloud_TW_UpdateBOLNeedUpdateTableAdapter();
            ta.CurrentConnection = session;
            ta.UpdateBOLNeedUpdate(ToSiteID, SupplierCode, SupplyPointCode, ProdCode, OrderedQty, AvailableQty, RetainedVehicleID, DriverID, BOLNO, UpdatedBy);
        }

        /// <summary>
        /// GetBOLHdr Data
        /// </summary>
        /// <param name="session">session Object</param>
        /// <returns>List of ShipmentData which need to ship at customer end</returns>
        public static List<DAL.BOLHdr_WagonRow> GetBOLHdrData(Int32 maxRetryCount, ISession session, String VersionNo = "")
        {
            BOLHdr_WagonTableAdapter ta = new BOLHdr_WagonTableAdapter();
            ta.CurrentConnection = session;
            return ta.GetDataBOLHdr(maxRetryCount).ToList();
        }
        public static string IsEnabledTankwagon(string CompanyID, ISession session, String VersionNo = "")
        {
            string IsEnabledTW = "";
            //Cloud_IsEnabledFrtLoadorUnloadBrkdownTableAdapter ta = new Cloud_IsEnabledFrtLoadorUnloadBrkdownTableAdapter();
            //ta.CurrentConnection = session;
            //DataTable dt = ta.GetDataIsEnabledFrtBrkdown(CompanyID);
            //if (dt.Rows.Count > 0)
            //{
            //    IsEnabledFrkBrk = dt.Rows[0]["Result"].ToString();
            //}
            SqlCommand cmd = (SqlCommand)session.CreateCommand();
            cmd.CommandText = "Cloud_IsEnabledTankwagon";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ClientID", CompanyID);
            SqlConnection con = new SqlConnection(session.ConnectionString);
            con.Open();
            SqlDataAdapter sa = new SqlDataAdapter();
            sa.SelectCommand = cmd;
            DataTable dt = new DataTable();
            sa.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
            {
                IsEnabledTW = dt.Rows[0]["Result"].ToString();
            }
            return IsEnabledTW;
        }

        public static DataTable GetDataToUpdateBOLImage(string customerID, ISession session, String VersionNo = "")
        {
            SqlCommand cmd = (SqlCommand)session.CreateCommand();
            cmd.CommandText = "Cloud_GetDataToUpdateBOLImage";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            SqlConnection con = new SqlConnection(session.ConnectionString);
            con.Open();
            SqlDataAdapter sa = new SqlDataAdapter();
            sa.SelectCommand = cmd;
            DataTable dt = new DataTable();
            sa.Fill(dt);
            con.Close();
            return dt;
        }

        public static DataTable UpdateNeedBOLImageUpdate(Guid BOLHdrID, ISession session, String VersionNo = "")
        {
            SqlCommand cmd = (SqlCommand)session.CreateCommand();
            cmd.CommandText = "Cloud_NeedBOLImageUpdate";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BOLHdrID", BOLHdrID);
            SqlConnection con = new SqlConnection(session.ConnectionString);
            con.Open();
            SqlDataAdapter sa = new SqlDataAdapter();
            sa.SelectCommand = cmd;
            DataTable dt = new DataTable();
            sa.Fill(dt);
            con.Close();
            return dt;
        }
        /// <summary>
        /// Function to return list of BOL item Wagon
        /// </summary>
        /// <param name="customerID">BOL Hdr ID</param>

        /// <returns>list of BOL item Wagon</returns>
        public static List<DAL.BOLItem_WagonRow> GetBolItemWagon(Guid BolHdrID, ISession session, String VersionNo = "")
        {
            BOLItem_WagonTableAdapter ta = new BOLItem_WagonTableAdapter();
            ta.CurrentConnection = session;
            return ta.GetDataBOLItemWagon(BolHdrID).ToList();
        }


        public static DataTable GetDataToUpdatePONo(string customerID, ISession session, String VersionNo = "")
        {
            SqlCommand cmd = (SqlCommand)session.CreateCommand();
            cmd.CommandText = "GetDataToUpdatePONo";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            SqlConnection con = new SqlConnection(session.ConnectionString);
            con.Open();
            SqlDataAdapter sa = new SqlDataAdapter();
            sa.SelectCommand = cmd;
            DataTable dt = new DataTable();
            sa.Fill(dt);
            con.Close();
            return dt;
        }

        public static DataTable UpdateNeedPONoUpdate(Guid OrderID, ISession session, String VersionNo = "")
        {
            SqlCommand cmd = (SqlCommand)session.CreateCommand();
            cmd.CommandText = "Cloud_NeedPONoUpdate";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OrderID", OrderID);
            SqlConnection con = new SqlConnection(session.ConnectionString);
            con.Open();
            SqlDataAdapter sa = new SqlDataAdapter();
            sa.SelectCommand = cmd;
            DataTable dt = new DataTable();
            sa.Fill(dt);
            con.Close();
            return dt;
        }


        // 2016-Feb-25 Vinoth Added For Get Split Load
        public static Boolean IsSplitLoad(Guid LoadID, Guid OrderItemID, Int32 IsLoadID, ISession session)
        {
            string IsSlpit = "";
            SqlCommand cmd = (SqlCommand)session.CreateCommand();
            cmd.CommandText = "Cloud_IsSplitLoad";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@LoadID", LoadID);
            cmd.Parameters.AddWithValue("@OrderItemID", OrderItemID);
            cmd.Parameters.AddWithValue("@IsLoadID", IsLoadID);
            SqlConnection con = new SqlConnection(session.ConnectionString);
            con.Open();
            SqlDataAdapter sa = new SqlDataAdapter();
            sa.SelectCommand = cmd;
            DataTable dt = new DataTable();
            sa.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
            {
                IsSlpit = dt.Rows[0]["LoadNo"].ToString();
            }
            return IsSlpit.Contains('*');
        }

        public static string IsEnabledDeliveryImagePost(string CompanyID, ISession session, String VersionNo = "")
        {
            string IsEnabledDeliveryImagePost = "";
            SqlCommand cmd = (SqlCommand)session.CreateCommand();
            cmd.CommandText = "Cloud_IsEnabledDeliveryImagePost";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ClientID", CompanyID);
            SqlConnection con = new SqlConnection(session.ConnectionString);
            con.Open();
            SqlDataAdapter sa = new SqlDataAdapter();
            sa.SelectCommand = cmd;
            DataTable dt = new DataTable();
            sa.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
            {
                IsEnabledDeliveryImagePost = dt.Rows[0]["Result"].ToString();
            }
            return IsEnabledDeliveryImagePost;
        }

        public static string IsEnableEventLog(string CompanyID, ISession session, String VersionNo = "")
        {
            string IsEnableEventLog = "";
            SqlCommand cmd = (SqlCommand)session.CreateCommand();
            cmd.CommandText = "Cloud_IsEnableEventLog";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ClientID", CompanyID);
            //SqlConnection con = new SqlConnection(session.ConnectionString);
            //con.Open();
            SqlDataAdapter sa = new SqlDataAdapter();
            sa.SelectCommand = cmd;
            DataTable dt = new DataTable();
            sa.Fill(dt);
            //con.Close();
            if (dt.Rows.Count > 0)
            {
                IsEnableEventLog = dt.Rows[0]["Result"].ToString();
            }
            return IsEnableEventLog;
        }


        #endregion TankWagon Details

        #region RejectedLoads
        public static DataTable FetchRejectedLoads(string customerID, ISession session)
        {
            SqlConnection con = new SqlConnection(session.ConnectionString);
            DataTable dtRejectedLoads = new DataTable();
            try
            {
                SqlCommand cmd = (SqlCommand)session.CreateCommand();
                cmd.CommandText = "Cloud_GetRejectedNoteLoads";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerID", customerID);
                con.Open();
                SqlDataAdapter sa = new SqlDataAdapter();
                sa.SelectCommand = cmd;
                sa.Fill(dtRejectedLoads);
                con.Close();

            }
            catch (Exception ex)
            {
                string exception = ex.Message;
            }
            finally
            {
                con.Close();
            }
            return dtRejectedLoads;
        }

        // 2016-12-05 update need update field
        public static void UpdateNeedUpdate(ISession session, string loadNumber)
        {

            SqlConnection con = new SqlConnection(session.ConnectionString);
            try
            {
                SqlCommand cmd = (SqlCommand)session.CreateCommand();
                cmd.CommandText = "Clound_UpdateNeedUpdate";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LoadNo", loadNumber);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string exception = ex.Message;
            }
            finally
            {
                con.Close();
            }

        }

        #endregion

        #region DeliveryNotes

        public static DataTable FetchDeliveryNotes(string customerID, ISession session)
        {
            SqlConnection con = new SqlConnection(session.ConnectionString);
            DataTable dtDeliveryNote = new DataTable();
            try
            {
                SqlCommand cmd = (SqlCommand)session.CreateCommand();
                cmd.CommandText = "Cloud_GetDeliveryNotes";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerID", customerID);
                con.Open();
                SqlDataAdapter sa = new SqlDataAdapter();
                sa.SelectCommand = cmd;
                sa.Fill(dtDeliveryNote);
                con.Close();

            }
            catch (Exception ex)
            {
                string exception = ex.Message;
            }
            finally
            {
                con.Close();
            }
            return dtDeliveryNote;
        }

        public static string GetOrderItemId(ISession session, string orderItemId)
        {
            SqlConnection con = new SqlConnection(session.ConnectionString);
            DataTable dtOrderItemId = new DataTable();
            string orderItemIds = string.Empty;
            try
            {
                SqlCommand cmd = (SqlCommand)session.CreateCommand();
                cmd.CommandText = "Cloud_GetOrderItemId";
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataAdapter sa = new SqlDataAdapter();
                sa.SelectCommand = cmd;
                sa.Fill(dtOrderItemId);
                con.Close();

                foreach (DataRow item in dtOrderItemId.Rows)
                {
                    orderItemIds += item["ID"] + ",";
                }
                orderItemIds.TrimEnd(',');

            }
            catch (Exception ex)
            {
                string exception = ex.Message;
            }
            finally
            {
                con.Close();
            }
            return orderItemIds;
        }

        public static int UpdateSyncFlagDeliveryNotes(ISession session, string orderId)
        {

            SqlConnection con = new SqlConnection(session.ConnectionString);
            int rowAffected = 0;
            try
            {
                SqlCommand cmd = (SqlCommand)session.CreateCommand();
                cmd.CommandText = "Cloud_UpdateSyncFlag";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ORDERID", orderId);
                con.Open();
                rowAffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string exception = ex.Message;
            }
            finally
            {
                con.Close();
            }
            return rowAffected;
        }



        #endregion

        #region CustomerIDParam
        public static DataTable GetUpdatedLoadForCustomer(string CustomerID, ISession session, String VersionNo = "")
        {
            SqlCommand cmd = (SqlCommand)session.CreateCommand();
            cmd.CommandText = "Cloud_UpdateLoads";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            SqlConnection con = new SqlConnection(session.ConnectionString);
            con.Open();
            SqlDataAdapter sa = new SqlDataAdapter();
            sa.SelectCommand = cmd;
            DataTable dt = new DataTable();
            sa.Fill(dt);
            con.Close();
            return dt;
        }

        public static DataTable GetBOLWaitTimeDetails(decimal SysTrxNo, ISession session, String VersionNo = "")
        {
            SqlCommand cmd = (SqlCommand)session.CreateCommand();
            cmd.CommandText = "Cloud_GetBOLWaitTimeDetails";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SysTrxNo", SysTrxNo);
            //SqlConnection con = new SqlConnection(session.ConnectionString);
            //con.Open();
            SqlDataAdapter sa = new SqlDataAdapter();
            sa.SelectCommand = cmd;
            DataTable dt = new DataTable();
            sa.Fill(dt);
            //con.Close();
            return dt;
        }

        #endregion
    }
}
