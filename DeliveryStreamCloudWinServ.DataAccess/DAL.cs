//2014.01.28  Ramesh M Added For Support Multi BOL ites added ExtSysTrxLine For CR#62038
using System;

namespace DeliveryStreamCloudWinServ.DataAccess
{

    /// <summary>
    /// DAL
    /// Partial class for data access layer
    /// </summary>
    public partial class DAL
    {

        /// <summary>
        /// CustomerRow
        /// Partial Customer row class to get and set datamembers
        /// </summary>
        partial class CustomerRow
        {
            /// <summary>
            /// CustomerID_Ext
            /// Properties for CustomerID datamember
            /// </summary>
            public String CustomerID_Ext
            {
                get
                {
                    return this.CustomerID;
                }
                set
                {
                    this.CustomerID = value;
                }
            }

            /// <summary>
            /// Password_Ext
            /// Properties for Password datamember
            /// </summary>
            public String Password_Ext
            {
                get
                {
                    return this.Password;
                }
                set
                {
                    this.Password = value;
                }
            }

            /// <summary>
            /// WCFURL_Ext
            /// Properties for WCFURL datamember
            /// </summary>
            public String WCFURL_Ext
            {
                get
                {
                    return this.IsWCFURLNull() ? null : WCFURL;
                }
                set
                {
                    this.WCFURL = value;
                }
            }
        }

        /// <summary>
        /// LoadRow
        /// Partial Load row class to get and set datamembers
        /// </summary>
        partial class LoadRow
        {
            /// <summary>
            /// Id_Ext
            /// Properties for Id datamember
            /// </summary>
            public Guid Id_Ext
            {
                get
                {
                    return this.ID;
                }
                set
                {
                    this.ID = value;
                }
            }

            /// <summary>
            /// LoadNo_Ext
            /// Properties for LoadNo datamember
            /// </summary>
            public string LoadNo_Ext
            {
                get
                {
                    return this.LoadNo;
                }
                set
                {
                    this.LoadNo = value;
                }
            }

            /// <summary>
            /// CustomerID_Ext
            /// Properties for CustomerID datamember
            /// </summary>
            public string CustomerID_Ext
            {
                get
                {
                    return this.CustomerID;
                }
                set
                {
                    this.CustomerID = value;
                }
            }

            /// <summary>
            /// VehicleID_Ext
            /// Properties for VehicleID datamember
            /// </summary>
            public int VehicleID_Ext
            {
                get
                {
                    if (this.IsVehicleIDNull())
                    {
                        return 0;
                    }
                    return this.VehicleID;
                }
                set
                {
                    this.VehicleID = value;
                }
            }

            /// <summary>
            /// DriverID_Ext
            /// Properties for DriverID datamember
            /// </summary>
            public int DriverID_Ext
            {
                get
                {
                    if (this.IsDriverIDNull())
                    {
                        return 0;
                    }
                    return this.DriverID;
                }
                set
                {
                    this.DriverID = value;
                }
            }

            /// <summary>
            /// NeedUpdate_Ext
            /// Properties for NeedUpdate datamember
            /// </summary>
            public Boolean NeedUpdate_Ext
            {
                get
                {
                    if (this.IsNeedUpdateNull())
                    {
                        return false;
                    }
                    return this.NeedUpdate;
                }
                set
                {
                    this.NeedUpdate = value;
                }
            }
        }

        /// <summary>
        /// LoadStatusHistoryRow
        /// Partial Load status history row class to get and set datamembers
        /// </summary>
        partial class LoadStatusHistoryRow
        {
            /// <summary>
            /// LoadID_Ext
            /// Properties for LoadID datamember
            /// </summary>
            public Guid LoadID_Ext
            {
                get
                {
                    return this.LoadID;
                }
                set
                {
                    this.LoadID = value;
                }
            }

            /// <summary>
            /// LoadStatusID_Ext
            /// Properties for LoadStatusID datamember
            /// </summary>
            public string LoadStatusID_Ext
            {
                get
                {
                    return this.LoadStatusID;
                }
                set
                {
                    this.LoadStatusID = value;
                }
            }

            /// <summary>
            /// Latitude_Ext
            /// Properties for Latitude datamember
            /// </summary>
            public string Latitude_Ext
            {
                get
                {
                    return this.Latitude;
                }
                set
                {
                    this.Latitude = value;
                }
            }

            /// <summary>
            /// Longitude_Ext
            /// Properties for Longitude datamember
            /// </summary>
            public string Longitude_Ext
            {
                get
                {
                    return this.Longitude;
                }
                set
                {
                    this.Longitude = value;
                }
            }

            /// <summary>
            /// NeedUpdate_Ext
            /// Properties for NeedUpdate datamember
            /// </summary>
            public Boolean NeedUpdate_Ext
            {
                get
                {
                    return this.NeedUpdate;
                }
                set
                {
                    this.NeedUpdate = value;
                }
            }

            /// <summary>
            /// UpdatedBy_Ext
            /// Properties for UpdatedBy datamember
            /// </summary>
            public Int32 UpdatedBy_Ext
            {
                get
                {
                    return this.IsUpdatedByNull() ? 0 : UpdatedBy;
                }
                set
                {
                    this.UpdatedBy = value;
                }
            }

            /// <summary>
            /// DateTime_Ext
            /// Properties for DateTime datamember
            /// </summary>
            public DateTime DateTime_Ext
            {
                get
                {
                    return this.DateTime;
                }
                set
                {
                    this.DateTime = value;
                }
            }

            /// <summary>
            /// CustomerID_Ext
            /// Properties for CustomerID datamember
            /// </summary>
            public string CustomerID_Ext
            {
                get
                {
                    return this.CustomerID;
                }
                set
                {
                    this.CustomerID = value;
                }
            }

            /// <summary>
            /// LoadNo_Ext
            /// Properties for LoadNo datamember
            /// </summary>
            public string LoadNo_Ext
            {
                get
                {
                    return this.LoadNo;
                }
                set
                {
                    this.LoadNo = value;
                }
            }
        }

        /// <summary>
        /// OrderStatusHistoryRow
        /// Partial Order status history row class to get and set datamembers
        /// </summary>
        partial class OrderStatusHistoryRow
        {
            /// <summary>
            /// OrderID_Ext
            /// Properties for OrderID datamember
            /// </summary>
            public Guid OrderID_Ext
            {
                get
                {
                    return this.OrderID;
                }
                set
                {
                    this.OrderID = value;
                }
            }

            /// <summary>
            /// OrderStatusID_Ext
            /// Properties for OrderStatusID datamember
            /// </summary>
            public string OrderStatusID_Ext
            {
                get
                {
                    return this.OrderStatusID;
                }
                set
                {
                    this.OrderStatusID = value;
                }
            }

            /// <summary>
            /// Latitude_Ext
            /// Properties for Latitude datamember
            /// </summary>
            public string Latitude_Ext
            {
                get
                {
                    return this.Latitude;
                }
                set
                {
                    this.Latitude = value;
                }
            }

            /// <summary>
            /// Longitude_Ext
            /// Properties for Longitude datamember
            /// </summary>
            public string Longitude_Ext
            {
                get
                {
                    return this.Longitude;
                }
                set
                {
                    this.Longitude = value;
                }
            }

            /// <summary>
            /// NeedUpdate_Ext
            /// Properties for NeedUpdate datamember
            /// </summary>
            public Boolean NeedUpdate_Ext
            {
                get
                {
                    return this.NeedUpdate;
                }
                set
                {
                    this.NeedUpdate = value;
                }
            }

            /// <summary>
            /// UpdatedBy_Ext
            /// Properties for UpdatedBy datamember
            /// </summary>
            public Int32 UpdatedBy_Ext
            {
                get
                {
                    return this.IsUpdatedByNull() ? 0 : UpdatedBy;
                }
                set
                {
                    this.UpdatedBy = value;
                }
            }

            /// <summary>
            /// DateTime_Ext
            /// Properties for DateTime datamember
            /// </summary>
            public DateTime DateTime_Ext
            {
                get
                {
                    return this.DateTime;
                }
                set
                {
                    this.DateTime = value;
                }
            }

            /// <summary>
            /// CustomerID_Ext
            /// Properties for CustomerID datamember
            /// </summary>
            public string CustomerID_Ext
            {
                get
                {
                    return this.CustomerID;
                }
                set
                {
                    this.CustomerID = value;
                }
            }

            /// <summary>
            /// SysTrxNo_Ext
            /// Properties for SysTrxNo datamember
            /// </summary>
            public Decimal SysTrxNo_Ext
            {
                get
                {
                    return this.SysTrxNo;
                }
                set
                {
                    this.SysTrxNo = value;
                }
            }
        }

        /// <summary>
        /// BOLItemRow
        /// Partial BOL item row class to get and set datamembers
        /// </summary>
        partial class BOLItemRow
        {
            /// <summary>
            /// Id_Ext
            /// Properties for Id datamember
            /// </summary>
            public Guid Id_Ext
            {
                get
                {
                    return this.ID;
                }
                set
                {
                    this.ID = value;
                }
            }

            /// <summary>
            /// BOLHdrID_Ext
            /// Properties for BOLHdrID datamember
            /// </summary>
            public Guid BOLHdrID_Ext
            {
                get
                {
                    return this.BOLHdrID;
                }
                set
                {
                    this.BOLHdrID = value;
                }
            }

            /// <summary>
            /// SysTrxNo_Ext
            /// Properties for SysTrxNo datamember
            /// </summary>
            public Decimal SysTrxNo_Ext
            {
                get
                {
                    return this.SysTrxNo;
                }
                set
                {
                    this.SysTrxNo = value;
                }
            }

            /// <summary>
            /// SysTrxLine_Ext
            /// Properties for SysTrxLine datamember
            /// </summary>
            public Int32 SysTrxLine_Ext
            {
                get
                {
                    return this.SysTrxLine;
                }
                set
                {
                    this.SysTrxLine = value;
                }
            }

            /// <summary>
            /// ComponentNo_Ext
            /// Properties for ComponentNo datamember
            /// </summary>
            public Int32 ComponentNo_Ext
            {
                get
                {
                    return this.ComponentNo == 0 ? 1 : this.ComponentNo;
                }
                set
                {
                    this.ComponentNo = value;
                }
            }

            /// <summary>
            /// GrossQty_Ext
            /// Properties for GrossQty datamember
            /// </summary>
            public Decimal GrossQty_Ext
            {
                get
                {
                    return this.GrossQty;
                }
                set
                {
                    this.GrossQty = value;
                }
            }

            /// <summary>
            /// NetQty_Ext
            /// Properties for NetQty datamember
            /// </summary>
            public Decimal NetQty_Ext
            {
                get
                {
                    return this.NetQty;
                }
                set
                {
                    this.NetQty = value;
                }
            }

            /// <summary>
            /// ShipDocSysTrxNo_Ext
            /// Properties for ShipDocSysTrxNo datamember
            /// </summary>
            public Decimal ShipDocSysTrxNo_Ext
            {
                get
                {
                    return IsShipDocSysTrxNoNull() ? 0m : this.ShipDocSysTrxNo;
                }
                set
                {
                    this.ShipDocSysTrxNo = value;
                }
            }

            /// <summary>
            /// ShipDocSysTrxLine_Ext
            /// Properties for ShipDocSysTrxLine datamember
            /// </summary>
            public Int32 ShipDocSysTrxLine_Ext
            {
                get
                {
                    return IsShipDocSysTrxLineNull() ? 0 : this.ShipDocSysTrxLine;
                }
                set
                {
                    this.ShipDocSysTrxLine = value;
                }
            }

            /// <summary>
            /// NeedUpdate_Ext
            /// Properties for NeedUpdate datamember
            /// </summary>
            public Boolean NeedUpdate_Ext
            {
                get
                {
                    return this.NeedUpdate;
                }
                set
                {
                    this.NeedUpdate = value;
                }
            }

            /// <summary>
            /// LoadID_Ext
            /// Properties for LoadID datamember
            /// </summary>
            public Guid LoadID_Ext
            {
                get
                {
                    return this.LoadID;
                }
                set
                {
                    this.LoadID = value;
                }
            }

            /// <summary>
            /// BolNo_Ext
            /// Properties for BolNo datamember
            /// </summary>
            public String BolNo_Ext
            {
                get
                {
                    return this.BOLNo;
                }
                set
                {
                    this.BOLNo = value;
                }
            }


            /// <summary>
            /// BOLQtyVarianceReason_Ext
            /// Properties for BOLQtyVarianceReason datamember
            /// </summary>
            public String BOLQtyVarianceReason_Ext
            {
                get
                {
                    return IsBOLQtyVarianceReasonNull() ? null : this.BOLQtyVarianceReason;
                }
                set
                {
                    this.BOLQtyVarianceReason = value;
                }
            }
            ///// <summary>
            ///// Image_Ext
            ///// Properties for Image datamember
            ///// </summary>
            //public Byte[] Image_Ext
            //{
            //    get
            //    {
            //        return IsImageNull() ? null : this.Image;
            //    }
            //    set
            //    {
            //        this.Image = value;
            //    }
            //}

            /// <summary>
            /// BOLDateTime_Ext
            /// Properties for BOLDateTime datamember
            /// </summary>
            public DateTime BOLDateTime_Ext
            {
                get
                {
                    return this.BOLDateTime;
                }
                set
                {
                    this.BOLDateTime = value;
                }
            }

            /// <summary>
            /// CustomerID_Ext
            /// Properties for CustomerID datamember
            /// </summary>
            public string CustomerID_Ext
            {
                get
                {
                    return this.CustomerID;
                }
                set
                {
                    this.CustomerID = value;
                }
            }

            /// <summary>
            /// UserName_Ext
            /// Properties for UserName datamember
            /// </summary>
            public String UserName_Ext
            {
                get
                {
                    return IsUserNameNull() ? null : this.UserName;
                }
                set
                {
                    this.UserName = value;
                }
            }
            /// <summary>
            /// Image_Ext
            /// Properties for Image datamember
            /// </summary>
            public Byte[] Image_Ext
            {
                get
                {
                    if (this.IsImageNull())
                    {
                        return null;
                    }
                    return this.Image;
                }
                set
                {
                    if (value == null)
                    {
                        this.SetImageNull();
                    }
                    else
                    {
                        this.Image = value;
                    }
                }

            }

            //2014.01.28  Ramesh M Added For Support Multi BOL ites added ExtSysTrxLine For CR#62038
            public String SupplierCode_Ext
            {
                get
                {
                    return this.SupplierCode;
                }
                set
                {
                    this.SupplierCode = value;
                }
            }
            public String SupplyPointCode_Ext
            {
                get
                {
                    return this.SupplyPointCode;
                }
                set
                {
                    this.SupplyPointCode = value;
                }
            }

            /// <summary>
            /// ExtSysTrxLine_Ext
            /// Properties for ExtSysTrxLine datamember
            /// </summary>
            public Int32 ExtSysTrxLine_Ext
            {
                get
                {
                    return this.ExtSysTrxLine;
                }
                set
                {
                    this.ExtSysTrxLine = value;
                }
            }
        }

        /// <summary>
        /// DeliveryDetailsRow
        /// Partial Delivery details row class to get and set datamembers
        /// </summary>
        partial class DeliveryDetailsRow
        {
            /// <summary>
            /// OrderItemID_Ext
            /// Properties for OrderItemID datamember
            /// </summary>
            public Guid OrderItemID_Ext
            {
                get
                {
                    return this.OrderItemID;
                }
                set
                {
                    this.OrderItemID = value;
                }
            }

            /// <summary>
            /// GrossQty_Ext
            /// Properties for GrossQty datamember
            /// </summary>
            public Decimal GrossQty_Ext
            {
                get
                {
                    return this.GrossQty;
                }
                set
                {
                    this.GrossQty = value;
                }
            }

            /// <summary>
            /// NetQty_Ext
            /// Properties for NetQty datamember
            /// </summary>
            public Decimal NetQty_Ext
            {
                get
                {
                    return this.NetQty;
                }
                set
                {
                    this.NetQty = value;
                }
            }

            /// <summary>
            /// DelivQty_Ext
            /// Properties for DelivQty datamember
            /// </summary>
            public Decimal DelivQty_Ext
            {
                get
                {
                    return this.DelivQty;
                }
                set
                {
                    this.DelivQty = value;
                }
            }

            /// <summary>
            /// DeliveryDateTime_Ext
            /// Properties for DeliveryDateTime datamember
            /// </summary>
            public DateTime DeliveryDateTime_Ext
            {
                get
                {
                    return this.DeliveryDateTime;
                }
                set
                {
                    this.DeliveryDateTime = value;
                }
            }



            /// <summary>
            /// DeliveryQtyVarianceReason_Ext
            /// Properties for DeliveryQtyVarianceReason datamember
            /// </summary>
            public String DeliveryQtyVarianceReason_Ext
            {
                get
                {
                    return IsDeliveryQtyVarianceReasonNull() ? null : this.DeliveryQtyVarianceReason;
                }
                set
                {
                    this.DeliveryQtyVarianceReason = value;
                }
            }

            /// <summary>
            /// NeedUpdate_Ext
            /// Properties for NeedUpdate datamember
            /// </summary>
            public Boolean NeedUpdate_Ext
            {
                get
                {
                    return this.NeedUpdate;
                }
                set
                {
                    this.NeedUpdate = value;
                }
            }

            /// <summary>
            /// LoadID_Ext
            /// Properties for LoadID datamember
            /// </summary>
            public Guid LoadID_Ext
            {
                get
                {
                    return this.LoadID;
                }
                set
                {
                    this.LoadID = value;
                }
            }

            /// <summary>
            /// SysTrxNo_Ext
            /// Properties for SysTrxNo datamember
            /// </summary>
            public Decimal SysTrxNo_Ext
            {
                get
                {
                    return this.SysTrxNo;
                }
                set
                {
                    this.SysTrxNo = value;
                }
            }

            /// <summary>
            /// SysTrxLine_Ext
            /// Properties for SysTrxLine datamember
            /// </summary>
            public Int32 SysTrxLine_Ext
            {
                get
                {
                    return this.SysTrxLine;
                }
                set
                {
                    this.SysTrxLine = value;
                }
            }

            /// <summary>
            /// CustomerID_Ext
            /// Properties for CustomerID datamember
            /// </summary>
            public string CustomerID_Ext
            {
                get
                {
                    return this.CustomerID;
                }
                set
                {
                    this.CustomerID = value;
                }
            }

            /// <summary>
            /// ShipDocSysTrxNo_Ext
            /// Properties for ShipDocSysTrxNo datamember
            /// </summary>
            public Decimal ShipDocSysTrxNo_Ext
            {
                //get
                //{
                //    return IsShipDocSysTrxNoNull() ? 0m : this.ShipDocSysTrxNo;
                //}
                get
                {
                    return this.ShipDocSysTrxNo;
                }
                set
                {
                    this.ShipDocSysTrxNo = value;
                }
            }

            /// <summary>
            /// ShipDocSysTrxLine_Ext
            /// Properties for ShipDocSysTrxLine datamember
            /// </summary>
            public Int32 ShipDocSysTrxLine_Ext
            {
                get
                {
                    return this.ShipDocSysTrxLine;
                }
                //get
                //{
                //    return IsShipDocSysTrxLineNull() ? 0 : this.ShipDocSysTrxLine;
                //}
                set
                {
                    this.ShipDocSysTrxLine = value;
                }
            }

            /// <summary>
            /// UserName_Ext
            /// Properties for UserName datamember
            /// </summary>
            public String UserName_Ext
            {
                get
                {
                    return IsUserNameNull() ? null : this.UserName;
                }
                set
                {
                    this.UserName = value;
                }
            }
        }

        /// <summary>
        /// OrderFrtRow
        /// Partial Fright row class to get and set datamembers
        /// </summary>
        partial class OrderFrtRow
        {
            /// <summary>
            /// OrderID
            /// Properties for OrderID datamember
            /// </summary>
            public Guid OrderID_Ext
            {
                get
                {
                    return this.OrderID;
                }
                set
                {
                    this.OrderID = value;
                }
            }


            public bool BOLWaitTime_Ext
            {
                get
                {
                    if (this.IsBOLWaitTimeNull())
                    {
                        return false;
                    }
                    return this.BOLWaitTime;
                }
                set
                {
                    this.BOLWaitTime = value;
                }

            }


            public decimal BOLWaitTimeTotal_Ext
            {
                get
                {
                    if (this.IsBOLWaitTimeTotalNull())
                    {
                        return 0.0m;
                    }
                    return this.BOLWaitTimeTotal;
                }
                set
                {
                    this.BOLWaitTimeTotal = value;
                }
            }


            public bool SiteWaitTime_Ext
            {
                get
                {
                    if (this.IsSiteWaitTimeNull())
                    {
                        return false;
                    }
                    return this.SiteWaitTime;
                }
                set
                {
                    this.SiteWaitTime = value;
                }
            }


            public string SiteWaitTime_Comment_Ext
            {
                get
                {
                    if (this.IsSiteWaitTime_CommentNull())
                    {
                        return null;
                    }
                    return this.SiteWaitTime_Comment;
                }
                set
                {
                    this.SiteWaitTime_Comment = value;
                }
            }


            public System.DateTime? SiteWaitTime_Start_Ext
            {
                get
                {
                    if (this.IsSiteWaitTime_StartNull())
                    {
                        return null;
                    }
                    return this.SiteWaitTime_Start;
                }
                set
                {
                    if (value == null)
                    {
                        this.SetSiteWaitTime_StartNull();
                    }
                    else
                    {
                        this.SiteWaitTime_Start = value.Value;
                    }
                }
            }


            public System.DateTime? SiteWaitTime_End_Ext
            {
                get
                {
                    if (this.IsSiteWaitTime_EndNull())
                    {
                        return null;
                    }
                    return this.SiteWaitTime_End;
                }
                set
                {
                    if (value == null)
                    {
                        this.SetSiteWaitTime_EndNull();
                    }
                    else
                    {
                        this.SiteWaitTime_End = value.Value;
                    }
                }
            }


            public bool SplitLoad_Ext
            {
                get
                {
                    if (this.IsSplitLoadNull())
                    {
                        return false;
                    }
                    return this.SplitLoad;
                }
                set
                {
                    this.SplitLoad = value;
                }
            }


            public string SplitLoad_Comment_Ext
            {
                get
                {
                    if (this.IsSplitLoad_CommentNull())
                    {
                        return null;
                    }
                    return this.SplitLoad_Comment;
                }
                set
                {
                    this.SplitLoad_Comment = value;
                }
            }


            public bool SplitDrop_Ext
            {
                get
                {
                    if (this.IsSplitDropNull())
                    {
                        return false;
                    }
                    return this.SplitDrop;
                }
                set
                {
                    this.SplitDrop = value;
                }
            }


            public string SplitDrop_Comment_Ext
            {
                get
                {
                    if (this.IsSplitDrop_CommentNull())
                    {
                        return null;
                    }
                    return this.SplitDrop_Comment;
                }
                set
                {
                    this.SplitDrop_Comment = value;
                }
            }


            public bool PumpOut_Ext
            {
                get
                {
                    if (this.IsPumpOutNull())
                    {
                        return false;
                    }
                    return this.PumpOut;
                }
                set
                {
                    this.PumpOut = value;
                }
            }


            public string PumpOut_Comment_Ext
            {
                get
                {
                    if (this.IsPumpOut_CommentNull())
                    {
                        return null;
                    }
                    return this.PumpOut_Comment;
                }
                set
                {
                    this.PumpOut_Comment = value;
                }
            }


            public bool Diversion_Ext
            {
                get
                {
                    if (this.IsDiversionNull())
                    {
                        return false;
                    }
                    return this.Diversion;
                }
                set
                {
                    this.Diversion = value;
                }
            }


            public string Diversion_Comment_Ext
            {
                get
                {
                    if (this.IsDiversion_CommentNull())
                    {
                        return null;
                    }
                    return this.Diversion_Comment;
                }
                set
                {
                    this.Diversion_Comment = value;
                }

            }


            public bool MinimumLoad_Ext
            {
                get
                {
                    if (this.IsMinimumLoadNull())
                    {
                        return false;
                    }
                    return this.MinimumLoad;
                }
                set
                {
                    this.MinimumLoad = value;
                }
            }


            public string MinimumLoad_Comment_Ext
            {
                get
                {
                    if (this.IsMinimumLoad_CommentNull())
                    {
                        return null;
                    }
                    return this.MinimumLoad_Comment;
                }
                set
                {
                    this.MinimumLoad_Comment = value;
                }
            }


            public bool Other_Ext
            {
                get
                {
                    if (this.IsOtherNull())
                    {
                        return false;
                    }
                    return this.Other;
                }
                set
                {
                    this.Other = value;
                }
            }


            public string Other_Comment_Ext
            {
                get
                {
                    if (this.IsOther_CommentNull())
                    {
                        return null;
                    }
                    return this.Other_Comment;
                }
                set
                {
                    this.Other_Comment = value;
                }
            }

            public string SignatureStatus_Ext
            {
                get
                {
                    if (this.IsSignatureStatusNull())
                    {
                        return null;
                    }
                    return this.SignatureStatus;
                }
                set
                {
                    this.SignatureStatus = value;
                }
            }
            /// <summary>
            ///  //2013.09.13 FSWW, Ramesh M Added ForCR#60123 Adding Two parameters in Class list
            /// </summary>
            public byte[] SignatureImage_Ext
            {
                get
                {
                    if (this.IsSignatureImageNull())
                    {
                        return null;
                    }
                    return this.SignatureImage;
                }
                set
                {
                    this.SignatureImage = value;
                }
            }
            /// <summary>
            ///  //2013.09.13 FSWW, Ramesh M Added ForCR#60123 Adding Two parameters in Class list
            /// </summary>
            public System.DateTime? SignatureDateTime_Ext
            {
                get
                {
                    if (this.IsSignatureDateTimeNull())
                    {
                        return null;
                    }
                    return this.SignatureDateTime;
                }
                set
                {
                    if (value == null)
                    {
                        this.SetSignatureDateTimeNull();
                    }
                    else
                    {
                        this.SignatureDateTime = value.Value;
                    }
                }
            }


        }

        /// <summary>
        /// DeliveryOrdersRow
        /// Partial Delivery details row class to get and set datamembers
        /// </summary>
        partial class DeliveryOrdersRow
        {

            /// <summary>
            /// LoadID_Ext
            /// Properties for LoadID datamember
            /// </summary>
            public Guid LoadID_Ext
            {
                get
                {
                    return this.LoadID;
                }
                set
                {
                    this.LoadID = value;
                }
            }

            /// <summary>
            /// ID_Ext
            /// Properties for ID datamember
            /// </summary>
            public Guid ID_Ext
            {
                get
                {
                    return this.ID;
                }
                set
                {
                    this.ID = value;
                }
            }

            /// <summary>
            /// SysTrxNo_Ext
            /// Properties for SysTrxNo datamember
            /// </summary>
            public Decimal SysTrxNo_Ext
            {
                get
                {
                    return this.SysTrxNo;
                }
                set
                {
                    this.SysTrxNo = value;
                }
            }

            /// <summary>
            /// CustomerID_Ext
            /// Properties for CustomerID datamember
            /// </summary>
            public string CustomerID_Ext
            {
                get
                {
                    return this.CustomerID;
                }
                set
                {
                    this.CustomerID = value;
                }
            }


            /// <summary>
            /// UserName_Ext
            /// Properties for UserName datamember
            /// </summary>
            public String UserName_Ext
            {
                get
                {
                    return IsUserNameNull() ? null : this.UserName;
                }
                set
                {
                    this.UserName = value;
                }
            }
        }

        #region TankWagon Details
        /// <summary>
        /// DeliveryOrdersRow
        /// Partial Delivery details row class to get and set datamembers
        /// </summary>
        partial class BOLItem_WagonRow
        {

            /// <summary>
            /// LoadID_Ext
            /// Properties for LoadID datamember
            /// </summary>
            public Guid BOLHdrID_Ext
            {
                get
                {
                    return this.BOLHdrID;
                }
                set
                {
                    this.BOLHdrID = value;
                }
            }

            /// <summary>
            /// ID_Ext
            /// Properties for ID datamember
            /// </summary>
            public Decimal SysTrxNo_Ext
            {
                get
                {
                    return this.SysTrxNo;
                }
                set
                {
                    this.SysTrxNo = value;
                }
            }

            /// <summary>
            /// SysTrxNo_Ext
            /// Properties for SysTrxNo datamember
            /// </summary>
            public Int32 SysTrxLine_Ext
            {
                get
                {
                    return this.SysTrxLine;
                }
                set
                {
                    this.SysTrxLine = value;
                }
            }

            /// <summary>
            /// CustomerID_Ext
            /// Properties for CustomerID datamember
            /// </summary>
            public Int32 CompartmentID_Ext
            {
                get
                {
                    return this.CompartmentID;
                }
                set
                {
                    this.CompartmentID = value;
                }
            }

            /// <summary>
            /// CustomerID_Ext
            /// Properties for CustomerID datamember
            /// </summary>
            public decimal GrossQty_Ext
            {
                get
                {
                    return this.GrossQty;
                }
                set
                {
                    this.GrossQty = value;
                }
            }


            /// <summary>
            /// UserName_Ext
            /// Properties for UserName datamember
            /// </summary>
            public String ProdCode_Ext
            {
                get
                {
                    return this.ProdCode;
                }
                set
                {
                    this.ProdCode = value;
                }
            }

            /// <summary>
            /// CustomerID_Ext
            /// Properties for CustomerID datamember
            /// </summary>
            public decimal NetQty_Ext
            {
                get
                {
                    return this.NetQty;
                }
                set
                {
                    this.NetQty = value;
                }
            }
            /// <summary>
            /// CustomerID_Ext
            /// Properties for CustomerID datamember
            /// </summary>
            public decimal OrderedQty_Ext
            {
                get
                {
                    return this.OrderedQty;
                }
                set
                {
                    this.OrderedQty = value;
                }
            }
            /// <summary>
            /// UserName_Ext
            /// Properties for UserName datamember
            /// </summary>
            public String Notes_Ext
            {
                get
                {
                    return this.Notes;
                }
                set
                {
                    this.Notes = value;
                }
            }




        }

        partial class BOLHdr_WagonRow
        {
            /// <summary>
            /// ClientID_Ext
            /// Properties for ClientID datamember
            /// </summary>
            public String BOLNo_Ext
            {
                get
                {
                    return this.BOLNo;
                }
                set
                {
                    this.BOLNo = value;
                }
            }

            /// <summary>
            /// ClientID_Ext
            /// Properties for ClientID datamember
            /// </summary>
            public Guid BOLHdrID_Ext
            {
                get
                {
                    return this.ID;
                }
                set
                {
                    this.ID = value;
                }
            }

            /// <summary>
            /// CompartmentID_Ext
            /// Properties for CompartmentID datamember
            /// </summary>
            public Int32 UpdatedBy_Ext
            {
                get
                {
                    return this.UpdatedBy;
                }
                set
                {
                    this.UpdatedBy = value;
                }
            }

            /// <summary>
            /// SystrxNo_Ext
            /// Properties for SystrxNo datamember
            /// </summary>
            public DateTime BOLDatetime_Ext
            {
                get
                {
                    return this.BOLDatetime;
                }
                set
                {
                    this.BOLDatetime = value;
                }
            }

            /// <summary>
            /// Capacity_Ext
            /// Properties for Capacity datamember
            /// </summary>
            public String SupplierCode_Ext
            {
                get
                {
                    return this.SupplierCode;
                }
                set
                {
                    this.SupplierCode = value;
                }
            }


            /// <summary>
            /// ClientID_Ext
            /// Properties for ClientID datamember
            /// </summary>
            public String SupplyPointCode_Ext
            {
                get
                {
                    return this.SupplyPointCode;
                }
                set
                {
                    this.SupplyPointCode = value;
                }
            }


            /// <summary>
            /// ClientID_Ext
            /// Properties for ClientID datamember
            /// </summary>
            public String ClientID_Ext
            {
                get
                {
                    return this.ClientID;
                }
                set
                {
                    this.ClientID = value;
                }
            }

        }

        partial class EODInventoryProcessRow
        {
            /// <summary>
            /// ClientID_Ext
            /// Properties for ClientID datamember
            /// </summary>
            public String BOLNo_Ext
            {
                get
                {
                    return this.BOLNo;
                }
                set
                {
                    this.BOLNo = value;
                }
            }




            /// <summary>
            /// SystrxNo_Ext
            /// Properties for SystrxNo datamember
            /// </summary>
            public DateTime BOLDatetime_Ext
            {
                get
                {
                    return this.BOLDatetime;
                }
                set
                {
                    this.BOLDatetime = value;
                }
            }

            /// <summary>
            /// Capacity_Ext
            /// Properties for Capacity datamember
            /// </summary>
            public String SupplierCode_Ext
            {
                get
                {
                    return this.SupplierCode;
                }
                set
                {
                    this.SupplierCode = value;
                }
            }


            /// <summary>
            /// ClientID_Ext
            /// Properties for ClientID datamember
            /// </summary>
            public String SupplyPointCode_Ext
            {
                get
                {
                    return this.SupplyPointCode;
                }
                set
                {
                    this.SupplyPointCode = value;
                }
            }


            /// <summary>
            /// ClientID_Ext
            /// Properties for ClientID datamember
            /// </summary>
            public String ClientID_Ext
            {
                get
                {
                    return this.ClientID;
                }
                set
                {
                    this.ClientID = value;
                }
            }



            /// <summary>
            /// UserName_Ext
            /// Properties for UserName datamember
            /// </summary>
            public String ProdCode_Ext
            {
                get
                {
                    return this.ProdCode;
                }
                set
                {
                    this.ProdCode = value;
                }
            }
            /// <summary>
            /// ClientID_Ext
            /// Properties for ClientID datamember
            /// </summary>
            public Decimal OrderedQty_Ext
            {
                get
                {
                    return this.OrderedQty;
                }
                set
                {
                    this.OrderedQty = value;
                }
            }




            /// <summary>
            /// ClientID_Ext
            /// Properties for ClientID datamember
            /// </summary>
            public Decimal AvailableQty_Ext
            {
                get
                {
                    return this.AvailableQty;
                }
                set
                {
                    this.AvailableQty = value;
                }
            }

            /// <summary>
            /// ClientID_Ext
            /// Properties for ClientID datamember
            /// </summary>
            public Decimal NetQty_Ext
            {
                get
                {
                    return this.NetQty;
                }
                set
                {
                    this.NetQty = value;
                }
            }

            /// <summary>
            /// ClientID_Ext
            /// Properties for ClientID datamember
            /// </summary>
            public Int32 OrderSysTrxNo_Ext
            {
                get
                {
                    return this.OrderSysTrxNo;
                }
                set
                {
                    this.OrderSysTrxNo = value;
                }
            }

            /// <summary>
            /// ClientID_Ext
            /// Properties for ClientID datamember
            /// </summary>
            public Int32 OrderSysTrxLineNo_Ext
            {
                get
                {
                    return this.OrderSysTrxLineNo;
                }
                set
                {
                    this.OrderSysTrxLineNo = value;
                }
            }


            /// <summary>
            /// ClientID_Ext
            /// Properties for ClientID datamember
            /// </summary>
            public Int32 ToSiteID_Ext
            {
                get
                {
                    return this.ToSiteID;
                }
                set
                {
                    this.ToSiteID = value;
                }
            }

            /// <summary>
            /// ClientID_Ext
            /// Properties for ClientID datamember
            /// </summary>
            public Int32 UserID_Ext
            {
                get
                {
                    return this.UserID;
                }
                set
                {
                    this.UserID = value;
                }
            }


            /// <summary>
            /// ClientID_Ext
            /// Properties for ClientID datamember
            /// </summary>
            public int DriverID_Ext
            {
                get
                {
                    return this.DriverID;
                }
                set
                {
                    this.DriverID = value;
                }
            }

            /// <summary>
            /// ClientID_Ext
            /// Properties for ClientID datamember
            /// </summary>
            public int VehicleID_Ext
            {
                get
                {
                    return this.RetainedVehicleID;
                }
                set
                {
                    this.RetainedVehicleID = value;
                }
            }


            /// <summary>
            /// ClientID_Ext
            /// Properties for ClientID datamember
            /// </summary>
            public String BOLSessionID_Ext
            {
                get
                {
                    return this.BOLSessionID;
                }
                set
                {
                    this.BOLSessionID = value;
                }
            }


            /// <summary>
            /// ClientID_Ext
            /// Properties for ClientID datamember
            /// </summary>
            public String IsOverShort_Ext
            {
                get
                {
                    return this.IsOverShort;
                }
                set
                {
                    this.IsOverShort = value;
                }
            }




        }

        #endregion TankWagon Details
    }
}



namespace DeliveryStreamCloudWinServ.DataAccess.DALTableAdapters {
    
    
    public partial class DeliveryDetailsTableAdapter {
    }
}

