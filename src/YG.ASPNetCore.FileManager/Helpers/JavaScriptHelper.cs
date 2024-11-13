using YG.ASPNetCore.FileManager.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YG.ASPNetCore.FileManager.Helpers
{
    public static class JavaScriptHelper
    {
        public static string ContainsToStringToLower(this HashSet<Command> hashSet, Command value)
        {
            return hashSet.Contains(value).ToString().ToLower();
        }
    }
}
