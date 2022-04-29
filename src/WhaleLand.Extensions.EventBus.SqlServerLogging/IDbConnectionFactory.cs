namespace WhaleLand.Extensions.EventBus.SqlServerLogging
{
    public interface IDbConnectionFactory
    {
        System.Data.Common.DbConnection GetDbConnection();
    }
}
