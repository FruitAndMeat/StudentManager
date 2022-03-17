using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Models;

namespace DAL
{
    /// <summary>
    /// 成绩表数据访问类
    /// </summary>
    public class ScoreListService
    {
        #region 使用DataSet保存数据
        /// <summary>
        /// 获取全部考试成绩
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllScoreList()
        {
            string sql = "select Students.StudentId,StudentName,ClassName,Gender,PhoneNumber,CSharp,SQLServerDB ";
            sql += "from Students ";
            sql += "inner join StudentClass on StudentClass.ClassId=Students.ClassId ";
            sql += "inner join ScoreList on ScoreList.StudentId=Students.StudentId";
            return SQLHelper.GetDataSet(sql);
        }

        #endregion

        /// <summary>
        /// 根据班级查询考试成绩（或全校成绩列表）
        /// </summary>
        /// <param name="className">班级名称</param>
        /// <returns>返回的查询结果</returns>
        public List<Student> QueryScoreList(string className)
        {
            string sql = "select Students.StudentId,StudentName,ClassName,Gender,CSharp,SQLserverDB ";
            sql += "from Students ";
            sql += "inner join StudentClass on StudentClass.ClassId=Students.ClassId ";
            sql += "inner join ScoreList on ScoreList.StudentId=Students.StudentId";
            if (className!=null&&className.Length!=0)
            {
                sql += string.Format(" where ClassName='{0}'", className);
            }
            SqlDataReader objReader=SQLHelper.GetReader(sql);
            List<Student> stuList = new List<Student>();
            while (objReader.Read())
            {
                stuList.Add(new Student()
                {
                    StudentId = Convert.ToInt32(objReader["StudentId"]),
                    StudentName = objReader["StudentName"].ToString(),
                    ClassName = objReader["ClassName"].ToString(),
                    Gender = objReader["Gender"].ToString(),
                    CSharp = Convert.ToInt32(objReader["CSharp"].ToString()),
                    SQLServerDb=Convert.ToInt32(objReader["SQLserverDB"].ToString())
                }) ;
            }
            objReader.Close();
            return stuList;
        }

        /// <summary>
        /// 根据班级统计考试成绩相关信息（或全校考试成绩统计）
        /// </summary>
        /// <param name="classId">班级编号</param>
        /// <returns>返回包含查询结果的集合</returns>
        public Dictionary<string,string> QueryScoreInfo(string classId)
        {
            //查询考试总人数、C#和数据库平均分。
            string sql = "select stuCount=Count(*),avgCSharp=avg(CSharp),avgDB=avg(SQLserverDB) from ScoreList";
            sql += " inner join Students on Students.StudentId=ScoreList.StudentId";
            if (classId!=null&&classId.Length!=0)
            {
                sql+=string.Format(" where ClassId={0}",classId);
            }
            //查询缺考总人数
            sql += ";select absentCount(*) from Students where StudentId not in ";
            sql += "(select StudentId from ScoreList)";
            if (classId != null && classId.Length != 0)
            {
                sql += string.Format(" and ClassId={0}", classId);
            }
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            Dictionary<string, string> scoreInfo = null;
            if (objReader.Read())//读取考试统计结果
            {
                scoreInfo = new Dictionary<string, string>();
                scoreInfo.Add("stuCount", objReader["stuCount"].ToString());
                scoreInfo.Add("avgCSharp", objReader["avgCSharp"].ToString());
                scoreInfo.Add("avgDB", objReader["avgDB"].ToString());
            }
            //读取缺考人员列表
            if (objReader.NextResult())
            {
                if (objReader.Read())
                {
                    scoreInfo.Add("absentCount", objReader["absentCount"].ToString());
                }
            }
            objReader.Close();
            return scoreInfo;
        }
        
        //根据班级查询缺考人员列表（或全校缺考人员列表）
    }
}
