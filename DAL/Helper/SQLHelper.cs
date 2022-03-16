using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;

using System.Configuration;
namespace DAL
{
    /// <summary>
    /// 访问SQLServer数据的通用类
    /// </summary>
    class SQLHelper
    {
        //定义连接字符串
        public static readonly string connString =ConfigurationManager.ConnectionStrings["connString"].ToString();
        /// <summary>
        /// 执行增删改操作
        /// </summary>
        /// <param name="sql">查询的SQL语句</param>
        /// <returns>返回受影响的行数</returns>
        public static int Update(string sql)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                //将异常信息写入日志...
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        /// <summary>
        /// 执行单一结果查询--单行单列
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static object GetSingleResult (string sql)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                //将异常信息写入日志...
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        /// <summary>
        /// 执行一个结果集的查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static SqlDataReader GetReader(string sql)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                //将异常信息写入日志...
                if (conn.State==ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Close();
                throw ex;
            }
            
        }
        /// <summary>
        /// 执行返回数据集的查询
        /// </summary>
        /// <param name="sql">SQL查询语句</param>
        /// <returns></returns>
        public static DataSet GetDataSet(string sql)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            //创建数据适配器对象
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            try
            {
                conn.Open();
                da.Fill(ds);//使用数据适配器填充数据
                return ds;
            }
            catch (Exception ex)
            {
                //将异常信息写入日志...
                throw ex;
            }
            finally
            {
                conn.Close();
            }

        }
        /// <summary>
        /// 获取数据库服务器的时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetServerTime()
        {
            return Convert.ToDateTime( GetSingleResult("select GetDate()"));
        }
    }
}
