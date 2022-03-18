using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DAL;
using Models;

namespace StudentManager
{
    public partial class FrmAttendance : Form
    {
        private AttendanceService objAttendanceService=new AttendanceService();
        private StudentService objStudentService=new StudentService();
        private List<Student> signdStudent=new List<Student>();//保存已经打卡的学员对象

        public FrmAttendance()
        {
            InitializeComponent();
            //设置dgv禁止生成列
            this.dgvStudentList.AutoGenerateColumns=false;
            timer1_Tick(null, null);//在构造方法中首先调用一次，避免一秒的延迟
            //显示应到、实到、缺勤人数
            //显示应到人数
            this.lblCount.Text = objAttendanceService.GetStudentCount().ToString();
            ShowStat();
        }
        private void ShowStat()
        {
            //显示实际出勤人数
            this.lblSign.Text=objAttendanceService.GetSignStudents().ToString();
            //显示缺勤人数
            this.lblAbsenceCount.Text=(Convert.ToInt32(objAttendanceService.GetStudentCount().ToString())
                -Convert.ToInt32(objAttendanceService.GetSignStudents().ToString())).ToString();
        }
        //显示当前时间
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.lblYear.Text = DateTime.Now.Year.ToString();
            this.lblMonth.Text=DateTime.Now.Month.ToString();  
            this.lblDay.Text=DateTime.Now.Day.ToString();
            this.lblTime.Text=DateTime.Now.ToLongTimeString();
            //显示星期
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    this.lblWeek.Text = "日";
                    break;
                case DayOfWeek.Monday:
                    this.lblWeek.Text = "一";
                    break;
                case DayOfWeek.Tuesday:
                    this.lblWeek.Text = "二";
                    break;
                case DayOfWeek.Wednesday:
                    this.lblWeek.Text = "三";
                    break;
                case DayOfWeek.Thursday:
                    this.lblWeek.Text = "四";
                    break;
                case DayOfWeek.Friday:
                    this.lblWeek.Text = "五";
                    break;
                case DayOfWeek.Saturday:
                    this.lblWeek.Text = "六";
                    break;
            }
        }
        //学员打卡        
        private void txtStuCardNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.txtStuCardNo.Text.Trim().Length!=0&&e.KeyValue==13)
            {
                //显示学员信息
                Student objStudent = objStudentService.GetStudentByCardNo(this.txtStuCardNo.Text.Trim());
                if (objStudent == null)
                {
                    MessageBox.Show("卡号不正确", "提示信息");
                    this.lblInfo.Text = "打卡失败";
                    this.txtStuCardNo.SelectAll();
                    this.lblStuName.Text = "";
                    this.lblStuId.Text = "";
                    this.lblStuClass.Text = "";
                    this.pbStu.Image = null;
                }
                else//显示学员信息
                {
                    this.lblStuName.Text = objStudent.StudentName;
                    this.lblStuId.Text = objStudent.StudentId.ToString();
                    this.lblStuClass.Text = objStudent.ClassName;
                    if (objStudent.StuImage!=null&&objStudent.StuImage.Length!=0)
                    {
                        this.pbStu.Image = (Image)new Common.SerializeObjectToString().DeserializeObject(objStudent.StuImage);
                    }
                    else { this.pbStu.Image = Image.FromFile("default.png"); }
                    //添加打卡信息
                    string result = objAttendanceService.AddRecord(this.txtStuCardNo.Text.Trim());
                    if (result!="success")
                    {
                        this.lblInfo.Text = "打卡失败";
                        MessageBox.Show(result, "错误提示");
                    }
                    else
                    {
                        this.lblInfo.Text = "打卡成功";
                        //更新已经打开的学员总数和缺勤总数
                        ShowStat();
                        objStudent.SignTime = DateTime.Now;
                        //将学员信息同步显示在列表中
                        signdStudent.Add(objStudent);
                        this.dgvStudentList.DataSource = null;
                        this.dgvStudentList.DataSource = signdStudent;

                        //等待下一个打卡
                        this.txtStuCardNo.Text = "";
                        this.txtStuCardNo.Focus();

                    }
                }
            }
        }
        //结束打卡
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvStudentList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.DataGridViewStyle.DgvRowPostPaint(this.dgvStudentList, e);
        }

        private void FrmAttendance_Load(object sender, EventArgs e)
        {
            this.txtStuCardNo.Focus();
        }
    }
}
