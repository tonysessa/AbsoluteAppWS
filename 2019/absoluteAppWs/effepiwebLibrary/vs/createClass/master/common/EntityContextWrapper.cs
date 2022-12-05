using System.Data;
using System.Data.Entity;

namespace Support
{
    public class EntityContextWrapper<T> : System.IDisposable where T : DbContext, new()
    {
   
        protected T ctx = null; 
        protected bool autoDispose = false;
        //
        public EntityContextWrapper(T ctx)
        {
            this.ctx = ctx;
            //
            if (this.ctx == null)
            {
                this.autoDispose = true;
                this.ctx = newInstance(); // new T();
                
            }
            if (this.ctx.Database.Connection.State != ConnectionState.Open)
                ctx.Database.Connection.Open();
        }
        //
        public EntityContextWrapper(EntityContextWrapper<T> wrp)
        {
            this.ctx = wrp!=null ? wrp.Context : null;
            //
            if (this.ctx == null)
            {
                this.autoDispose = true;
                this.ctx = newInstance(); // new T();
            }
            if (this.ctx.Database.Connection.State != ConnectionState.Open)
                ctx.Database.Connection.Open();
        }
        //
        public EntityContextWrapper()
        {
            this.ctx = newInstance(); // new T();
            this.autoDispose = true;
            if (this.ctx.Database.Connection.State != ConnectionState.Open)
                ctx.Database.Connection.Open();
        }
        //
        public T Context
        {
            get
            {
                return ctx;
            }
        }
        //
        public int SaveChanges()
        {
            return ctx.SaveChanges();
        }
        //
        public virtual void Dispose()
        {
            if (autoDispose && ctx != null)
            {
                if (ctx.Database.Connection.State == ConnectionState.Open)
                    ctx.Database.Connection.Close();
                ctx.Dispose();
            }
            ctx = null;
        }
        //
        public void SqlExecuteNonQuery(string sql)
        {
            System.Data.EntityClient.EntityConnection con = (System.Data.EntityClient.EntityConnection)ctx.Database.Connection;
            System.Data.SqlClient.SqlConnection sqlCon = (System.Data.SqlClient.SqlConnection)(con.StoreConnection);
            System.Data.SqlClient.SqlCommand cmd = sqlCon.CreateCommand();
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }
        //
        protected T newInstance()
        {
            return new T();
        }
    }
}
