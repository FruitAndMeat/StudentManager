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
        #region 查询学员
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
        /// <summary>
        /// 根据学号查询学员对象
        /// </summary>
        /// <param name="studentId">学员学号</param>
        /// <returns>返回的学生对象</returns>
        public Student GetStudentById(string studentId)
        {
            string sql = "select StudentId, StudentName, Gender, Birthday, ";
            sql += "StudentIdNo,PhoneNumber,ClassName,StudentAddress,CardNo,StuImage from Students ";
            sql += "inner join StudentClass on Students.ClassId=StudentClass.ClassId ";
            sql += "where StudentId={0}";
            sql = string.Format(sql, studentId);
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            Student objStudent = null;
            if (objReader.Read())
            {
                objStudent = new Student()
                {
                    StudentId = Convert.ToInt32(objReader["StudentId"]),
                    StudentName = objReader["StudentName"].ToString(),
                    Gender = objReader["Gender"].ToString(),
                    PhoneNumber = objReader["PhoneNumber"].ToString(),
                    Birthday = Convert.ToDateTime(objReader["Birthday"].ToString()),
                    StudentIdNo = objReader["StudentIdNo"].ToString(),
                    ClassName = objReader["ClassName"].ToString(),
                    StudentAddress = objReader["StudentAddress"].ToString(),
                    CardNo = objReader["CardNo"].ToString(),
                    StuImage = objReader["StuImage"] == null ? "" : objReader["StuImage"].ToString(),
                };
            }
            objReader.Close();
            return objStudent;
        }
        #endregion
        /// <summary>
        /// 修改学员时判断身份证号和其他学员是否重复
        /// </summary>
        /// <param name="studentIdNo">新的身份证号</param>
        /// <param name="studentId">当前学员的学号</param>
        /// <returns></returns>
        public bool IsIdNoExisted(string studentIdNo,string studentId)
        {
            string sql = string.Format("select count(*) from Students where StudentIdNo={0} and StudentId<>{1}", 
                studentIdNo, studentId);
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
        //修改学员时卡号的判断和判断身份证时一样的
        
        #region 修改学员
        /// <summary>
        /// 修改学员对象
        /// </summary>
        /// <param name="objStudent"></param>
        /// <returns></returns>
        public int ModifyStudent(Student objStudent)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("update Students set StudentName='{0}',Gender='{1}',Birthday='{2}',");
            sqlBuilder.Append("StudentIdNo={3},Age={4},PhoneNumber='{5}',StudentAddress='{6}',");
            sqlBuilder.Append("CardNo='{7}',ClassId={8},StuImage='{9}' ");
            sqlBuilder.Append("where StudentId={10}");
            string sql = string.Format(sqlBuilder.ToString(), objStudent.StudentName, objStudent.Gender,
                objStudent.Birthday.ToString("yyyy-MM-dd"),objStudent.StudentIdNo, objStudent.Age, objStudent.PhoneNumber, 
                objStudent.StudentAddress,objStudent.CardNo, objStudent.ClassId, objStudent.StuImage, objStudent.StudentId);
            try
            {
                return SQLHelper.Update(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("修改学员信息时数据访问发生异常："+ex.Message);
            }
        }


        #endregion

        #region 删除学员
        /// <summary>
        /// 根据学号删除学员对象
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public int DeleteStudent(string studentId)
        {
            //在正规框架中 ，是按照对象编程，所以需要将参数替换为Student objStudent ;sql语句参数换为 objStudent.studentId。
            string sql = "delete from Students where StudentId=" + studentId;
            try
            {
                return SQLHelper.Update(sql);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    throw new Exception("该学号被其他实体引用，不能直接删除该学员对象！");
                }
                else
                {
                    throw new Exception("删除学员对象发生数据操作异常：\r\n" + ex.Message);
                }
            }
            catch (Exception  ex)
            {
                throw ex;
            }   
        }
        #endregion
    }
}
