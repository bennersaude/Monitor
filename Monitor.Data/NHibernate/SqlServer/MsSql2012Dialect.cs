using NHibernate;
using NHibernate.Dialect.Function;

namespace Monitor.Data.NHibernate.SqlServer
{
    public class MsSql2012Dialect : global::NHibernate.Dialect.MsSql2012Dialect
    {
        public MsSql2012Dialect()
        {
            /*RegisterFunction("fn_PegarApenasNumeros", new StandardSQLFunction("dbo.fn_PegarApenasNumeros", NHibernateUtil.String));
            RegisterFunction(Names.PADRONIZAR_TEXTO_PARA_PESQUISA, new StandardSQLFunction($"dbo.{Names.PADRONIZAR_TEXTO_PARA_PESQUISA}", NHibernateUtil.String));                        
            RegisterFunction("concat", new StandardSQLFunction("concat", NHibernateUtil.String));*/
        }
    }
}
