namespace WhaleLand.Extensions.EventBus.MySqlLogging
{
    public interface IDbConnectionFactory
    {
        System.Data.Common.DbConnection GetDbConnection();
    }
}
