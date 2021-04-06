using System;
using System.Data;
using Monitor.ComponentModel.Helpers;
using Monitor.Data.Sql;

namespace Monitor.Data.Infra.Data.Types
{
    public class SqlServerTypeHelper : IDbTypeHelper
    {
        public SqlDbType FromDotNetType(Type type)
        {
            if ((type == typeof(Int32)) || (type == typeof(Int32?)))
                return SqlDbType.Int;
            if ((type == typeof(Int64)) || (type == typeof(Int64?)))
                return SqlDbType.Int;
            if ((type == typeof(long)) || (type == typeof(long?)))
                return SqlDbType.Int;
            if ((type == typeof(decimal)) || (type == typeof(decimal?)))
                return SqlDbType.Decimal;
            if ((type == typeof(double)) || (type == typeof(double?)) ||
                (type == typeof(float)) || (type == typeof(float?)))
                return SqlDbType.Float;
            if (type == typeof(string) || type == typeof(Version))
                return SqlDbType.VarChar;
            if ((type == typeof(DateTime)) || (type == typeof(DateTime?)))
                return SqlDbType.DateTime;
            if (type == typeof(byte[]))
                return SqlDbType.VarBinary;
            throw new InvalidOperationException(String.Format("Tipo {0} não suportado!", type));
        }

        public string FromDbTypeToDDL(SqlDbType dbType, int size)
        {
            if (dbType == SqlDbType.Int)
                return "INT";
            if (dbType == SqlDbType.BigInt)
                return "INT";
            if (dbType == SqlDbType.Decimal)
                return "FLOAT";
            if (dbType == SqlDbType.Float)
                return "FLOAT";
            if (dbType == SqlDbType.DateTime)
                return "DATETIME";
            if ((dbType == SqlDbType.VarChar) && (size > 0) && (size <= ColumnDefaults.MAX_VARCHAR_SIZE))
                return String.Format("VARCHAR({0})", size);
            if (((dbType == SqlDbType.Text) || ((dbType == SqlDbType.VarChar) && (size == StringHelper.UnlimitedLength))))
                return "TEXT";
            if (dbType == SqlDbType.VarBinary)
                return "VARBINARY(MAX)";
            throw new InvalidOperationException(String.Format("Tipo {0} com tamanho {1} não suportado!", dbType, size));
        }

        public string FromDotNetTypeToDLL(Type type, int size)
        {
            return FromDbTypeToDDL(FromDotNetType(type), size);
        }
    }
}
