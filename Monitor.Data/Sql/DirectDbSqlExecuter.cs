using System;
using log4net;
using NHibernate;

namespace Monitor.Data.Sql
{
    public class DirectDbSqlExecuter : IDirectDbSqlExecuter
    {
        private readonly ILog logger = LogManager.GetLogger(typeof(DirectDbSqlExecuter));

        public int Execute(string sqlCommand, ISession session)
        {
            using (var command = session.Connection.CreateCommand())
            {
                command.Connection = session.Connection;
                if (session.GetCurrentTransaction() != null && session.GetCurrentTransaction().IsActive)
                    session.GetCurrentTransaction().Enlist(command);
                command.CommandText = sqlCommand;
                var rows = command.ExecuteNonQuery();
                var logMessage = string.Concat("DirectDbSqlExecuter: ", sqlCommand);
                logger.Info(logMessage);
                Console.WriteLine(logMessage);
                return rows;
            }
        }
    }
}