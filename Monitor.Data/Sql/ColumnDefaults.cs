using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Monitor.Data.Sql
{
    public static class ColumnDefaults
    {
        public const int MAX_VARCHAR_SIZE = 4000;

        public static void FixFKColumnsType(IList<string> mappingsCommands, IEnumerable<string> metadataTables)
        {
            for (var i = 0; i < mappingsCommands.Count; i++)
            {
                if (metadataTables.Any(x => mappingsCommands[i].ToUpper().Contains(x)))
                    continue;
                if (mappingsCommands[i].Contains("BIGINT"))
                {
                    var r = new Regex(@"\bBIGINT\b");
                    mappingsCommands[i] = r.Replace(mappingsCommands[i], "INT");
                }
                mappingsCommands[i] = mappingsCommands[i].Replace("NUMBER(20,0)", "NUMBER(10,0)");
            }
        }
    }
}
