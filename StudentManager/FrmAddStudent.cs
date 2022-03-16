using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Models;
using DAL;


namespace StudentManager
{
    public partial class FrmAddStudent : Form
    {
        //�������ݷ��ʶ���
        private StudentClassService objClassService=new StudentClassService();
        private StudentService objStudentService=new StudentService();
        List<Student> stuList=new List<Student>();//������ʱ����ѧԱ����

        public FrmAddStudent()
        {
            InitializeComponent();
            //��ʼ���༶������
            this.cboClassName.DataSource = objClassService.GetAllClass();
            this.cboClassName.DisplayMember = "ClassName";//������������ʾ�ı�
            this.cboClassName.ValueMember = "ClassId";//������������ʾ�ı���Ӧ������
            this.cboClassName.SelectedIndex = -1;//Ĭ�ϲ�ѡ���κΰ༶
            this.dgvStudentList.AutoGenerateColumns = false;//��ֹ�Զ������
        }
        //�����ѧԱ
        private void btnAdd_Click(object sender, EventArgs e)
        {
            #region ������֤
            if (this.txtStudentName.Text.Trim().Length==0)
            {
                MessageBox.Show("ѧ����������Ϊ�գ�", "��ʾ��Ϣ");
                this.txtStudentName.Focus();
                return;
            }
            if (this.txtCardNo.Text.Trim().Length==0)
            {
                MessageBox.Show("���ڿ��Ų���Ϊ�գ�", "��ʾ��Ϣ");
                this.txtCardNo.Focus();
                return;
            }
            if (!this.rdoFemale.Checked&&!this.rdoMale.Checked)
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
            int age=DateTime.Now.Year-Convert.ToDateTime(this.dtpBirthday.Text).Year;
            if (age>35||age<18)
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

            //��װ����
            Student objStudent = new Student()
            {
                StudentName = this.txtStudentName.Text.Trim(),
                Gender = this.rdoMale.Checked ? "��" : "Ů",
                Birthday = Convert.ToDateTime(this.dtpBirthday.Text),
                StudentIdNo = this.txtStudentIdNo.Text.Trim(),
                PhoneNumber = this.txtPhoneNumber.Text.Trim(),
                StudentAddress = this.txtAddress.Text.Trim(),
                ClassId = Convert.ToInt32(this.cboClassName.SelectedValue),
                ClassName=this.cboClassName.Text.Trim(),//Ϊ���б�չʾ��Ҫ
                Age=DateTime.Now.Year- Convert.ToDateTime(this.dtpBirthday.Text).Year,
                CardNo = this.txtCardNo.Text.Trim(),
                StuImage=this.pbStu.Image!=null?new Common.SerializeObjectToString().SerializeObject(this.pbStu.Image):""
            };
            //���ú�̨�������ݿ�
            try
            {
                int StudentId=objStudentService.AddStudent(objStudent);
                if (StudentId>1)
                {
                    //ͬ����ʾ��ӵ�ѧԱ
                    objStudent.StudentId= StudentId;
                    this.stuList.Add(objStudent);
                    this.dgvStudentList.DataSource = null;
                    this.dgvStudentList.DataSource=this.stuList;
                    //ѯ���Ƿ�������
                    DialogResult result = MessageBox.Show("��ѧԱ��ӳɹ����Ƿ������ӣ�", "��ʾ��Ϣ", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        //����û��������Ϣ
                        foreach (Control item in this.gbstuinfo.Controls)
                        {
                            if (item is TextBox)
                            {
                                item.Text = "";
                            }
                            else if (item is RadioButton)
                            {
                                ((RadioButton)item).Checked = false;
                            }
                        }
                        this.cboClassName.SelectedIndex = -1;
                        this.pbStu.Image = null;
                        this.txtStudentName.Focus();
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("���ѧԱ�������ݷ��ʲ����쳣:"+ex.Message);
            }

        }
        //�رմ���
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void FrmAddStudent_KeyDown(object sender, KeyEventArgs e)
        {
       

        }
        //ѡ������Ƭ
        private void btnChoseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog objFileDialog = new OpenFileDialog();
            DialogResult result=objFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.pbStu.Image=Image.FromFile(objFileDialog.FileName);
            }
        }
        //��������ͷ
        private void btnStartVideo_Click(object sender, EventArgs e)
        {
         
        }
        //����
        private void btnTake_Click(object sender, EventArgs e)
        {
        
        }
        //�����Ƭ
        private void btnClear_Click(object sender, EventArgs e)
        {
            this.pbStu.Image = null;
        }

        private void dgvStudentList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.DataGridViewStyle.DgvRowPostPaint(this.dgvStudentList, e);
        }
    }
}