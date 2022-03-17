using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DAL
{
    /// <summary>
    /// 成绩表数据访问类
    /// </summary>
    public class ScoreListService
    {
        public DataSet GetAllScoreList()
        {
            string sql = "select Students.StudentId,StudentName,ClassName,Gender,PhoneNumber,CSharp,SQLServerDB ";
            sql += "from Students ";
            sql += "inner join StudentClass on StudentClass.ClassId=Students.ClassId ";
            sql += "inner join ScoreList on ScoreList.StudentId=Students.StudentId";
            return SQLHelper.GetDataSet(sql);
        }
    }
}
