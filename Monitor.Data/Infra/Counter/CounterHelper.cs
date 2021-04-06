using System;
using Monitor.Data.Types;
using NHibernate;

namespace Monitor.Data.Infra.Counter
{
    public class CounterHelper: ICounterHelper
    {
        private readonly DataBaseType dataBaseType;

        private ISession Sessao;

        public CounterHelper(ISession Sessao)
        {
            if (Sessao == null)
                throw new ArgumentNullException(nameof(Sessao));
            this.Sessao = Sessao;
            dataBaseType = DataBaseType.SQLServer;
        }

        public long GetNextValue(string sequenceName)
        {
            if (string.IsNullOrEmpty(sequenceName))
                throw new ArgumentNullException(nameof(sequenceName));

            switch (dataBaseType)
            {
                case DataBaseType.Oracle:
                    return Convert.ToInt64(CreateQuery(string.Concat("SELECT ", sequenceName, ".NEXTVAL FROM DUAL")));
                case DataBaseType.SQLServer:
                    return Convert.ToInt64(CreateQuery(string.Concat("SELECT NEXT VALUE FOR ", sequenceName)));
                case DataBaseType.SQLite:
                    throw new NotSupportedException("Sequência ainda não suportada no SQLite!");
                default:
                    throw new ArgumentOutOfRangeException("Banco de dados desconhecido!");
            }
        }

        private object CreateQuery(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentNullException(nameof(query));
            var criteria = Sessao.CreateSQLQuery(query);

            Sessao.Flush();
            return criteria.UniqueResult();
        }
    }
}
