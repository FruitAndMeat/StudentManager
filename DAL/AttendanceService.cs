using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    /// <summary>
    /// 考勤数据访问类
    /// </summary>
   public class AttendanceService
    {
        /// <summary>
        /// 添加考勤记录
        /// </summary>
        /// <param name="cardNo"></param>
        /// <returns></returns>
        public string AddRecord(string cardNo)
        {
            string sql=string.Format("insert into Attendance (CardNo) values('{0}')",cardNo);
            try
            {
                SQLHelper.Update(sql);
                return "success";
            }
            catch (Exception ex)
            {
                return "打开失败！系统出现问题，请练习管理员！" + ex.Message;
            }
        }
    }
}
