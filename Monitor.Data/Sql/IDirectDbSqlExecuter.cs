using NHibernate;

namespace Monitor.Data.Sql
{
    public interface IDirectDbSqlExecuter
    {
        int Execute(string sqlCommand, ISession session);
    }   
}