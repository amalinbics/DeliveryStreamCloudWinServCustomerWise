using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DeliveryStreamCloudWinServ.DataAccess
{
    /// <summary>
    /// ISession interface
    /// </summary>
    public interface ISession : IDbConnection
    {
        // Methods
        void CommitTransaction();
        bool OpenTransactionIfNot();
        void RollbackTransaction();

        // Properties
        IDbConnection Connection { get; set; }
        bool HasTransaction { get; }
        IDbTransaction Transaction { get; set; }
    }
}
