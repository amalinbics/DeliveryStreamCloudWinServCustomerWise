using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DeliveryStreamCloudWinServ.DataAccess
{
    /// <summary>
    /// Session class
    /// </summary>
    public class Session : ISession
    {
        // Fields
        private IDbConnection conn;
        private bool hasTrans;
        private IDbTransaction trans;

        // Methods
        /// <summary>
        /// Default constructor
        /// </summary>
        public Session()
        {
            this.hasTrans = false;
            this.conn = new SqlConnection();
        }

        /// <summary>
        /// Parameterised constructor
        /// </summary>
        /// <param name="connStr">Connection string</param>
        public Session(string connStr)
        {
            this.hasTrans = false;
            this.conn = new SqlConnection(connStr);
        }

        /// <summary>
        ///  Parameterised constructor
        /// </summary>
        /// <param name="connecion">Open Connecion to data source</param>
        /// <param name="transaction">Transaction to perform at data source</param>
        public Session(IDbConnection connecion, IDbTransaction transaction)
        {
            this.hasTrans = false;
            this.conn = connecion;
            this.trans = transaction;
        }

        /// <summary>
        /// BeginTransaction
        /// Implementation of BeginTransaction method of interface IDbTransaction
        /// </summary>
        /// <returns>IDbTransaction object</returns>
        public IDbTransaction BeginTransaction()
        {
            if (!this.hasTrans)
            {
                this.trans = this.conn.BeginTransaction();
                this.hasTrans = true;
            }
            return this.trans;
        }

        /// <summary>
        /// BeginTransaction
        /// Implementation of BeginTransaction method of interface IDbTransaction
        /// </summary>
        /// <param name="il">transaction locking behaviour</param>
        /// <returns>IDbTransaction object</returns>
        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            if (!this.hasTrans)
            {
                this.trans = this.conn.BeginTransaction(il);
                this.hasTrans = true;
            }
            return this.trans;
        }

        /// <summary>
        /// ChangeDatabase
        /// Implementation of ChangeDatabase method of interface IDbConnection
        /// </summary>
        /// <param name="databaseName">Database name</param>
        public void ChangeDatabase(string databaseName)
        {
            this.conn.ChangeDatabase(databaseName);
        }

        /// <summary>
        /// Close
        /// Implementation of Close method of interface IDbConnection
        /// </summary>
        public void Close()
        {
            this.conn.Close();
        }

        /// <summary>
        /// CommitTransaction
        /// Implementation of CommitTransaction method of interface IDbTransaction
        /// </summary>
        public void CommitTransaction()
        {
            if (this.hasTrans)
            {
                this.trans.Commit();
                this.hasTrans = false;
            }
        }

        /// <summary>
        /// CreateCommand
        /// Implementation of CreateCommand method of interface IDbCommand
        /// </summary>
        /// <returns>IDbCommand object</returns>
        public IDbCommand CreateCommand()
        {
            IDbCommand cmd = this.conn.CreateCommand();
            if (this.trans != null)
            {
                cmd.Transaction = this.trans;
            }
            return cmd;
        }

        /// <summary>
        /// Dispose
        /// Implementation of Dispose method of interface IDbConnection
        /// </summary>
        public void Dispose()
        {
            this.trans = null;
            this.conn.Dispose();
        }

        /// <summary>
        /// Open
        /// Implementation of Open method of interface IDbConnection
        /// </summary>
        public void Open()
        {
            this.conn.Open();
        }

        /// <summary>
        /// OpenTransactionIfNot
        /// Implementation of OpenTransactionIfNot method of interface IDbTransaction
        /// </summary>
        /// <returns>True = Open transaction, False = Failed </returns>
        public bool OpenTransactionIfNot()
        {
            if (this.HasTransaction)
            {
                return false;
            }
            this.BeginTransaction();
            return true;
        }

        /// <summary>
        /// RollbackTransaction
        /// Implementation of RollbackTransaction method of interface IDbTransaction
        /// </summary>
        public void RollbackTransaction()
        {
            if (this.HasTransaction)
            {
                this.trans.Rollback();
                this.hasTrans = false;
            }
        }

        #region Accessors

        /// <summary>
        /// Connection
        /// Properties for Connection
        /// </summary>
        public IDbConnection Connection
        {
            get
            {
                return this.conn;
            }
            set
            {
                this.trans = (IDbTransaction)value;
            }
        }

        /// <summary>
        /// ConnectionString
        /// Properties for ConnectionString
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return this.conn.ConnectionString;
            }
            set
            {
                this.conn.ConnectionString = value;
            }
        }

        /// <summary>
        /// ConnectionTimeout
        /// Readonly Property for ConnectionTimeout
        /// </summary>
        public int ConnectionTimeout
        {
            get
            {
                return this.conn.ConnectionTimeout;
            }
        }

        /// <summary>
        /// Database
        /// Readonly Property for Database
        /// </summary>
        public string Database
        {
            get
            {
                return this.conn.Database;
            }
        }

        /// <summary>
        /// HasTransaction
        /// Readonly Property for HasTransaction
        /// </summary>
        public bool HasTransaction
        {
            get
            {
                return this.hasTrans;
            }
        }

        /// <summary>
        /// State
        /// Readonly Property for State
        /// </summary>
        public ConnectionState State
        {
            get
            {
                return this.conn.State;
            }
        }

        /// <summary>
        /// Transaction
        /// Properties for Transaction
        /// </summary>
        public IDbTransaction Transaction
        {
            get
            {
                return this.trans;
            }
            set
            {
                this.trans = value;
            }
        }

        #endregion Accessors
    }
}
