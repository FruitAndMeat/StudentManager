using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using DAL;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{

    /// <summary>
    /// 学生管理数据访问类
    /// </summary>
     public class StudentService
    {
        #region 添加学员

        /// <summary>
        /// 判断身份证号是否存在
        /// </summary>
        /// <param name="StudentIdNo"></param>
        /// <returns></returns>
        public bool IsIdNoExisted(string StudentIdNo)
        {
            string sql = string.Format("select Count(*) from Students where StudentIdNo={0}", StudentIdNo);
            int result = Convert.ToInt32(SQLHelper.GetSingleResult(sql));
            if (result==1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 验证考勤卡号是否存在
        /// </summary>
        /// <param name="CardNo"></param>
        /// <returns></returns>
        public bool IsCardNoExisted(string CardNo)
        {
            string sql = string.Format("select Count(*) from Students where CardNo='{0}'", CardNo);
            int result = Convert.ToInt32(SQLHelper.GetSingleResult(sql));
            if (result == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 将学院对象保存到数据库
        /// </summary>
        /// <param name="objStudent"></param>
        /// <returns>返回当前新学员的学号</returns>
        /// <exception cref="Exception"></exception>
        public int AddStudent(Student objStudent)
        {
            //编写Sql语句
            StringBuilder sqlBuilder = new StringBuilder("insert into Students");
            sqlBuilder.Append("(StudentName,Gender,Birthday,Age,StudentIdNo,CardNo,PhoneNumber,StudentAddress,ClassId,StuImage)");
            sqlBuilder.Append("values('{0}','{1}','{2}',{3},{4},'{5}','{6}','{7}',{8},'{9}');select @@identity");
            string sql = string.Format(sqlBuilder.ToString(), objStudent.StudentName, objStudent.Gender,
                objStudent.Birthday.ToString("yyyy-MM-dd"), objStudent.Age, objStudent.StudentIdNo, objStudent.CardNo,
                objStudent.PhoneNumber, objStudent.StudentAddress, objStudent.ClassId,objStudent.StuImage);
            //提交SQL语句
            try
            {
                return Convert.ToInt32(SQLHelper.GetSingleResult(sql));//执行SQL语句，返回学号
            }
            catch (Exception ex)
            {
                throw new Exception("添加学员是发生数据库访问异常："+ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// 根据班级查询学员信息
        /// </summary>
        /// <param name="className">班级名称</param>
        /// <returns>返回学员信息列表</returns>
        public List<Student> GetStudentByClass(string className)
        {
            string sql = "select StudentId, StudentName, Gender, Birthday, ";
            sql += "StudentIdNo,PhoneNumber,ClassName from Students ";
            sql += "inner join StudentClass on Students.ClassId=StudentClass.ClassId ";
            sql += "where ClassName='{0}'";
            sql = string.Format(sql, className);
            SqlDataReader objReader=SQLHelper.GetReader(sql);
            List<Student> stuList = new List<Student>();
            while (objReader.Read())
            {
                stuList.Add(new Student()
                {
                    StudentId = Convert.ToInt32(objReader["StudentId"]),
                    StudentName = objReader["StudentName"].ToString(),
                    Gender = objReader["Gender"].ToString(),
                    PhoneNumber=objReader["PhoneNumber"].ToString(),
                    Birthday=Convert.ToDateTime(objReader["Birthday"].ToString()),
                    StudentIdNo=objReader["StudentIdNo"].ToString(),
                    ClassName=objReader["ClassName"].ToString()
                }) ;
            }
            objReader.Close();
            return stuList;
        }
    }
}
