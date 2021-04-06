using System;

namespace Monitor.Data.Infra.Data.Types
{
    public interface IDbTypeHelper
    {
         string FromDotNetTypeToDLL(Type type, int size);
    }
}