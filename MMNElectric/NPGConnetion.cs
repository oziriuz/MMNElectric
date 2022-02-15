using Helpers;
using Npgsql;
using System;
using System.Data;


namespace SQLData
{
    class NPGConnection : IDisposable
    {
        static readonly string connectionString = Helper.CnnVal("MMNDatabase");

        public NpgsqlConnection cnn;

        public NPGConnection()
        {
            if((cnn = Cnn()) == null)
            {
                this.Dispose();
            }
        }

        private NpgsqlConnection Cnn()
        {
            NpgsqlConnection conn = null;

            //database connection info
            try
            {
                conn = new NpgsqlConnection(connectionString);
                conn.Open();
                return conn;
            }

            catch(Exception ex)
            {
                if(conn.State == ConnectionState.Open)
                    conn.Close();
                throw ex;
            }
        }

        public void Dispose()
        {
            if(cnn != null)
            {
                cnn.Close();
            }
        }

    }
}
