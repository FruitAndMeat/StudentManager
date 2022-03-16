using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Models;


namespace StudentManager
{
    public partial class FrmStudentInfo : Form
    {
        public FrmStudentInfo()
        {
            InitializeComponent();
        }
        public FrmStudentInfo(Student objStudent):this()  
        {
            //��ʾѧԱ��ϸ��Ϣ
            this.lblStudentName.Text = objStudent.StudentName;
            this.lblStudentIdNo.Text = objStudent.StudentIdNo;
            this.lblPhoneNumber.Text = objStudent.PhoneNumber;
            this.lblBirthday.Text = objStudent.Birthday.ToShortDateString();
            this.lblAddress.Text = objStudent.StudentAddress.ToString();
            this.lblClass.Text = objStudent.ClassName;
            this.lblGender.Text = objStudent.Gender;
            this.lblCardNo.Text = objStudent.CardNo;
            //��ʾ��Ƭ
            this.pbStu.Image = objStudent.StuImage.Length != 0 ?
                (Image)new Common.SerializeObjectToString().DeserializeObject(objStudent.StuImage) : Image.FromFile("default.png");
        }
        //�ر�
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}