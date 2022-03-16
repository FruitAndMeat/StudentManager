using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;

namespace StudentManager.Common
{
    /// <summary>
    /// 基于正则表达式的数据验证
    /// </summary>
    public class DataValidate
    {
        public static bool IsInteger(string txt)
        {
            Regex objReg = new Regex(@"^[1-9]\d*$");//正则表达式
            return objReg.IsMatch(txt);
        }
    }
}
