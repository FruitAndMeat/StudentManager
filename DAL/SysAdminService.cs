using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Models;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    /// <summary>
    /// 管理员数据访问类
    /// </summary>
    public class SysAdminService
    {
        /// <summary>
        /// 根据账号和密码登录
        /// </summary>
        /// <param name="objAdmin">包含账号和密码的管理员对象</param>
        /// <returns></returns>
        public SysAdmin AdminLogin(SysAdmin objAdmin)
        {
            string sql = $"select AdminName from Admins where LoginId={objAdmin.LoginId} and LoginPwd={objAdmin.LoginPwd}";
            try
            {
                SqlDataReader objReader = SQLHelper.GetReader(sql);
                if (objReader.Read())
                {
                    objAdmin.AdminName = objReader["AdminName"].ToString();
                }
                else
                {
                    objAdmin = null;
                }
                objReader.Close();
            }
            catch(SqlException ex)
            {
                throw new Exception("应用程序和数据库连接出现问题："+ex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objAdmin;
        }
    }
}
