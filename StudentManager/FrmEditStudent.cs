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
            //��ʼ���༶������
            this.cboClassName.DataSource = new StudentClassService().GetAllClass();//������Դ
            this.cboClassName.DisplayMember = "ClassName";//������������ʾ�ı�
            this.cboClassName.ValueMember = "ClassId";//������������ʾ�ı���Ӧ��Value
            //��ʾѧԱ��ϸ��Ϣ
            this.txtStudentName.Text = objStudent.StudentName;
            this.txtStudentIdNo.Text = objStudent.StudentIdNo;
            this.txtPhoneNumber.Text = objStudent.PhoneNumber;
            this.dtpBirthday.Text = objStudent.Birthday.ToShortDateString();
            this.txtAddress.Text = objStudent.StudentAddress.ToString();
            this.txtStudentId.Text = objStudent.StudentId.ToString();
            this.cboClassName.Text = objStudent.ClassName;
            if (objStudent.Gender=="��") this.rdoMale.Checked=true;
            else this.rdoFemale.Checked = true;
            this.txtCardNo.Text = objStudent.CardNo;
            //��ʾ��Ƭ
            this.pbStu.Image = objStudent.StuImage.Length != 0 ?
                (Image)new Common.SerializeObjectToString().DeserializeObject(objStudent.StuImage) : Image.FromFile("default.png");
        }

        //�ύ�޸�
        private void btnModify_Click(object sender, EventArgs e)
        {
            //������֤��ѧԱ�����һ�������֤�źͿ��ڿ�����Ҫ�����ж�
            #region ������֤
            if (this.txtStudentName.Text.Trim().Length == 0)
            {
                MessageBox.Show("ѧ����������Ϊ�գ�", "��ʾ��Ϣ");
                this.txtStudentName.Focus();
                return;
            }
            if (this.txtCardNo.Text.Trim().Length == 0)
            {
                MessageBox.Show("���ڿ��Ų���Ϊ�գ�", "��ʾ��Ϣ");
                this.txtCardNo.Focus();
                return;
            }
            if (!this.rdoFemale.Checked && !this.rdoMale.Checked)
            {
                MessageBox.Show("��ѡ��ѧ���Ա�", "��ʾ��Ϣ");
                return;
            }
            if (this.cboClassName.Text.Trim().Length == 0)
            {
                MessageBox.Show("��ѡ��༶��", "��ʾ��Ϣ");
                this.cboClassName.Focus();
                return;
            }
            int age = DateTime.Now.Year - Convert.ToDateTime(this.dtpBirthday.Text).Year;
            if (age > 35 || age < 18)
            {
                MessageBox.Show("���������18~35��֮�䣡", "��ʾ��Ϣ");
                return;
            }
            //��֤���ڿ��ű���������

            //���֤��ʽ��֤
            if (!Common.DataValidate.IsIdentityCard(this.txtStudentIdNo.Text.Trim()))
            {
                MessageBox.Show("���֤������Ҫ��", "��ʾ��Ϣ");
                this.txtStudentIdNo.Focus();
                this.txtStudentIdNo.SelectAll();
                return;
            }
            //��֤���֤�еĳ������ں��û�ѡ��ĳ��������Ƿ�һ��
            string birthday = Convert.ToDateTime(this.dtpBirthday.Text).ToString("yyyyMMdd");
            if (!this.txtStudentIdNo.Text.Trim().Contains(birthday))
            {
                MessageBox.Show("���֤�źͳ������ڲ�ƥ��", "��֤��ʾ");
                this.txtStudentIdNo.SelectAll();
                this.txtStudentIdNo.Focus();
                return;
            }
            //�����ݿ���֤���֤�źͿ��ڿ����Ƿ����
            if (objStudentService.IsIdNoExisted(this.txtStudentIdNo.Text.Trim()))
            {
                MessageBox.Show("��ǰ���֤���Ѿ�������ѧԱʹ�ã�", "��֤��ʾ");
                this.txtStudentIdNo.Focus();
                this.txtStudentIdNo.SelectAll();
                return;
            }
            if (objStudentService.IsCardNoExisted(this.txtCardNo.Text.Trim()))
            {
                MessageBox.Show("��ǰ���ڿ����Ѿ�������ѧԱʹ�ã�", "��֤��ʾ");
                this.txtCardNo.Focus();
                this.txtCardNo.SelectAll();
                return;
            }
            #endregion
            //��֤���֤��
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //ѡ����Ƭ
        private void btnChoseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog objFileDialog = new OpenFileDialog();
            DialogResult result = objFileDialog.ShowDialog();
            if (result == DialogResult.OK)
                this.pbStu.Image = Image.FromFile(objFileDialog.FileName);
        }
    }
}