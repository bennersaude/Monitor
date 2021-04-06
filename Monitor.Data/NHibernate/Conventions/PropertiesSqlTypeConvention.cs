using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using Monitor.Data.Infra.Data.Types;
using NHibernate.Util;

namespace Monitor.Data.NHibernate.Conventions
{
    public class PropertiesSqlTypeConvention: IPropertyConvention, IIdConvention
    {
        public void Apply(IPropertyInstance instance)
        {
            ConfigureNullableField(instance);

            if (instance.Property.PropertyType.IsEnum ||
                (instance.Property.PropertyType.IsNullable() &&
                 Nullable.GetUnderlyingType(instance.Property.PropertyType).IsEnum))
            {
                instance.CustomType(instance.Property.PropertyType);
                return;
            }

            if (instance.Property.PropertyType == typeof(bool) ||
                (instance.Property.PropertyType.IsNullable() && (Nullable.GetUnderlyingType(instance.Property.PropertyType) == typeof(bool))))
                return;

            var length = ConfigureLength(instance);
            /*var typeHelper = (new DataBaseHelper().DiscoverDataBaseType() == DataBaseType.Oracle)
                    ? new OracleTypeHelper() as IDbTypeHelper
                    : new SqlServerTypeHelper() as IDbTypeHelper;*/
            var typeHelper = new SqlServerTypeHelper() as IDbTypeHelper;
            instance.CustomSqlType(typeHelper.FromDotNetTypeToDLL(instance.Property.PropertyType, length));
        }

        private static int ConfigureLength(IPropertyInstance instance)
        {
            var length = 0;
            if (instance.Property.PropertyType == typeof(byte[]))
                length = int.MaxValue;
            if (instance.Property.PropertyType == typeof(string) || instance.Property.PropertyType == typeof(Version))
            {
                var myMemberInfos = ((PropertyInstance)(instance)).EntityType.GetMember(instance.Name);
                if (myMemberInfos.Any())
                {
	                if (myMemberInfos[0].GetCustomAttributes(false).FirstOrDefault(x => x is StringLengthAttribute) is StringLengthAttribute lengthAttribute)
                        length = lengthAttribute.MaximumLength;
                }
                if (length == 0)
                    throw new InvalidOperationException(
		                    $"Falta definir o atributo StringLength para a propriedade '{instance.Name}' do tipo '{((PropertyInstance) (instance)).EntityType}'!");
            }
            if (length > 0)
                instance.Length(length);
            return length;
        }

        private static void ConfigureNullableField(IPropertyInstance instance)
        {
            if (instance.Property.PropertyType.IsValueType && !instance.Property.PropertyType.IsNullable())
                instance.Not.Nullable();
            else
                instance.Nullable();
        }

        public void Apply(IIdentityInstance instance)
        {
            /*var typeHelper = (new DataBaseHelper().DiscoverDataBaseType() == DataBaseType.Oracle)
                    ? new OracleTypeHelper() as IDbTypeHelper
                    : new SqlServerTypeHelper() as IDbTypeHelper;*/
            var typeHelper = new SqlServerTypeHelper() as IDbTypeHelper;
            instance.CustomSqlType(typeHelper.FromDotNetTypeToDLL(instance.Type.GetUnderlyingSystemType(), 0));
        }
    }
}
