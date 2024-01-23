namespace BrainWare.Api
{
    using System.Data.Common;

    public interface IDatabase : IDisposable
    {
        public DbDataReader ExecuteReader(string query);

        public int ExecuteNonQuery(string query);
    }
}