using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using DAL;
using Models;

namespace StudentManager
{
    public partial class FrmEditStudent : Form
    {
        private StudentService objStudentService = new StudentService();
        public FrmEditStudent()
        {
            InitializeComponent();
        } 
        public FrmEditStudent(Student objStudent):this()
        {
            //初始化班级下拉框
            this.cboClassName.DataSource = new StudentClassService().GetAllClass();//绑定数据源
            this.cboClassName.DisplayMember = "ClassName";//设置下拉框显示文本
            this.cboClassName.ValueMember = "ClassId";//设置下拉框显示文本对应的Value
            //显示学员详细信息
            this.txtStudentName.Text = objStudent.StudentName;
            this.txtStudentIdNo.Text = objStudent.StudentIdNo;
            this.txtPhoneNumber.Text = objStudent.PhoneNumber;
            this.dtpBirthday.Text = objStudent.Birthday.ToShortDateString();
            this.txtAddress.Text = objStudent.StudentAddress.ToString();
            this.txtStudentId.Text = objStudent.StudentId.ToString();
            this.cboClassName.Text = objStudent.ClassName;
            if (objStudent.Gender=="男") this.rdoMale.Checked=true;
            else this.rdoFemale.Checked = true;
            this.txtCardNo.Text = objStudent.CardNo;
            //显示照片
            this.pbStu.Image = objStudent.StuImage.Length != 0 ?
                (Image)new Common.SerializeObjectToString().DeserializeObject(objStudent.StuImage) : Image.FromFile("default.png");
        }

        //提交修改
        private void btnModify_Click(object sender, EventArgs e)
        {
            //数据验证与学员添加是一样，身份证号和考勤卡号需要特殊判断
            #region 数据验证
            if (this.txtStudentName.Text.Trim().Length == 0)
            {
                MessageBox.Show("学生姓名不能为空！", "提示信息");
                this.txtStudentName.Focus();
                return;
            }
            if (this.txtCardNo.Text.Trim().Length == 0)
            {
                MessageBox.Show("考勤卡号不能为空！", "提示信息");
                this.txtCardNo.Focus();
                return;
            }
            if (!this.rdoFemale.Checked && !this.rdoMale.Checked)
            {
                MessageBox.Show("请选择学生性别！", "提示信息");
                return;
            }
            if (this.cboClassName.Text.Trim().Length == 0)
            {
                MessageBox.Show("请选择班级！", "提示信息");
                this.cboClassName.Focus();
                return;
            }
            int age = DateTime.Now.Year - Convert.ToDateTime(this.dtpBirthday.Text).Year;
            if (age > 35 || age < 18)
            {
                MessageBox.Show("年龄必须在18~35岁之间！", "提示信息");
                return;
            }
            //验证考勤卡号必须是数字

            //身份证格式验证
            if (!Common.DataValidate.IsIdentityCard(this.txtStudentIdNo.Text.Trim()))
            {
                MessageBox.Show("身份证不符合要求！", "提示信息");
                this.txtStudentIdNo.Focus();
                this.txtStudentIdNo.SelectAll();
                return;
            }
            //验证身份证中的出生日期和用户选择的出生日期是否一致
            string birthday = Convert.ToDateTime(this.dtpBirthday.Text).ToString("yyyyMMdd");
            if (!this.txtStudentIdNo.Text.Trim().Contains(birthday))
            {
                MessageBox.Show("身份证号和出生日期不匹配", "验证提示");
                this.txtStudentIdNo.SelectAll();
                this.txtStudentIdNo.Focus();
                return;
            }
            #endregion
            //验证身份证号
            if (objStudentService.IsIdNoExisted(this.txtStudentIdNo.Text.Trim(), this.txtStudentId.Text.Trim()))
            {
                MessageBox.Show("身份证不能和现有其他学员身份证号相同！请修改！", "修改提示");
                this.txtStudentIdNo.SelectAll();
                this.txtStudentIdNo.Focus();
                return;
            }
            //卡号验证和上面相同

            //封装对象
            Student objStudent = new Student()
            {
                StudentName = this.txtStudentName.Text.Trim(),
                Gender = this.rdoMale.Checked ? "男" : "女",
                Birthday = Convert.ToDateTime(this.dtpBirthday.Text),
                StudentIdNo = this.txtStudentIdNo.Text.Trim(),
                PhoneNumber = this.txtPhoneNumber.Text.Trim(),
                StudentAddress = this.txtAddress.Text.Trim(),
                ClassId = Convert.ToInt32(this.cboClassName.SelectedValue),
                Age = DateTime.Now.Year - Convert.ToDateTime(this.dtpBirthday.Text).Year,
                CardNo = this.txtCardNo.Text.Trim(),
                StuImage = this.pbStu.Image != null ? new Common.SerializeObjectToString().SerializeObject(this.pbStu.Image) : "",
                StudentId = Convert.ToInt32(this.txtStudentId.Text.Trim())
            };
            try
            {
                if (objStudentService.ModifyStudent(objStudent)==1)
                {
                    MessageBox.Show("学员信息修改成功！", "提示信息");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示信息");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //选择照片
        private void btnChoseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog objFileDialog = new OpenFileDialog();
            DialogResult result = objFileDialog.ShowDialog();
            if (result == DialogResult.OK)
                this.pbStu.Image = Image.FromFile(objFileDialog.FileName);
        }
    }
}