using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace DeliveryStreamCloudWinServ.DataAccess
{
    public abstract class BaseTableAdapter : Component
    {
        // Fields
        private SqlTransaction m_transaction;

        // Methods
        [DebuggerNonUserCode]
        protected BaseTableAdapter()
        {
        }

        public void BeginTransaction()
        {
            if (this.CurrentConnection.State != ConnectionState.Open)
            {
                this.CurrentConnection.Open();
            }
            this.Transaction = (SqlTransaction)this.CurrentConnection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            this.Transaction.Commit();
            this.CurrentConnection.Close();
        }

        public void RollbackTransaction()
        {
            this.Transaction.Rollback();
            this.CurrentConnection.Close();
        }

        // Properties
        private SqlDataAdapter Adapter
        {
            get
            {
                return (SqlDataAdapter)this.GetType().GetProperty("Adapter", ~BindingFlags.Default).GetValue(this, null);
            }
        }

        private SqlCommand[] CommandCollection
        {
            get
            {
                return (SqlCommand[])this.GetType().GetProperty("CommandCollection", ~BindingFlags.Default).GetValue(this, null);
            }
        }

        public ISession CurrentConnection
        {
            get
            {
                IDbConnection conn = (IDbConnection)this.GetType().GetProperty("Connection", ~BindingFlags.Default).GetValue(this, null);
                if (conn == null)
                {
                    return null;
                }
                return new Session(conn, this.Transaction);
            }
            set
            {
                this.Transaction = (SqlTransaction)value.Transaction;
                this.GetType().GetProperty("Connection", ~BindingFlags.Default).SetValue(this, value.Connection, null);
            }
        }

        public SqlTransaction Transaction
        {
            get
            {
                return this.m_transaction;
            }
            set
            {
                if (this.CommandCollection != null)
                {
                    foreach (SqlCommand command in this.CommandCollection)
                    {
                        command.Transaction = value;
                    }
                }
                if (this.Adapter.InsertCommand != null)
                {
                    this.Adapter.InsertCommand.Transaction = value;
                }
                if (this.Adapter.UpdateCommand != null)
                {
                    this.Adapter.UpdateCommand.Transaction = value;
                }
                if (this.Adapter.DeleteCommand != null)
                {
                    this.Adapter.DeleteCommand.Transaction = value;
                }
                if (value != null)
                {
                    if (this.CurrentConnection == null)
                    {
                        this.CurrentConnection = new Session(value.Connection, value);
                    }
                }
                else if (this.m_transaction != null)
                {
                    this.CurrentConnection = null;
                }
                this.m_transaction = value;
            }
        }
    }
}
