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
    public partial class FrmStudentManage : Form
    {
        private StudentClassService objClassService=new StudentClassService();
        private StudentService objStuService=new StudentService();
        private List<Student> stuList = new List<Student>();

        public FrmStudentManage()
        {
            InitializeComponent();
            //��ʼ���༶������
            this.cboClass.DataSource=objClassService.GetAllClass();
            this.cboClass.DisplayMember = "ClassName";
            this.cboClass.ValueMember = "ClassId";
            this.cboClass.SelectedIndex = -1;

            this.dgvStudentList.AutoGenerateColumns = false;
        
        }
        //���հ༶��ѯ
        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (this.cboClass.SelectedIndex==-1)
            {
                MessageBox.Show("��ѡ��༶��", "��ѯ��ʾ");
                return;
            }
            //ִ�в�ѯ��������
            this.stuList = objStuService.GetStudentByClass(this.cboClass.Text.ToString());
            this.dgvStudentList.DataSource=this.stuList;
            new Common.DataGridViewStyle().DgvStyle1(this.dgvStudentList);
            
        }
        //����ѧ�Ų�ѯ
        private void btnQueryById_Click(object sender, EventArgs e)
        {
            //������֤
            if (this.txtStudentId.Text.Trim().Length==0)
            {
                MessageBox.Show("�������ѯ��ѧ�ţ�", "��ʾ��Ϣ");
                this.txtStudentId.Focus();
                return;
            }
            if (!Common.DataValidate.IsInteger(this.txtStudentId.Text.Trim()))
            {
                MessageBox.Show("ѧ�ű�����������", "��ʾ��Ϣ");
                this.txtStudentId.SelectAll();
                this.txtStudentId.Focus();
                return;
            }
            Student objStudent = objStuService.GetStudentById(this.txtStudentId.Text.Trim());
            if (objStudent==null)
            {
                MessageBox.Show("ѧԺ��Ϣ������", "��ѯ��ʾ");
                this.txtStudentId.Focus();
            }
            else
            {
                //��ʾѧԱ��Ϣ
                FrmStudentInfo objFrm = new FrmStudentInfo(objStudent);
                objFrm.Show();
            }
        }
        private void txtStudentId_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.txtStudentId.Text.Trim().Length!=0&&e.KeyValue==13)
            {
                btnQueryById_Click(null, null);
            }
        }
        //˫��ѡ�е�ѧԱ������ʾ��ϸ��Ϣ
        private void dgvStudentList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dgvStudentList.CurrentRow != null)
            {
                string studentId = this.dgvStudentList.CurrentRow.Cells["StudentId"].Value.ToString();
                Student objStudent = objStuService.GetStudentById(studentId);
                FrmStudentInfo objFrm = new FrmStudentInfo(objStudent);
                objFrm.Show();
            }
        }
        //�޸�ѧԱ����
        private void btnEidt_Click(object sender, EventArgs e)
        {
            if (this.dgvStudentList.RowCount==0)
            {
                MessageBox.Show("û���κ�Ҫ�޸ĵ�ѧԱ��Ϣ", "��ʾ��Ϣ");
                return;
            }
            if (this.dgvStudentList.CurrentRow==null)
            {
                MessageBox.Show("��ѡ��Ҫ�޸ĵ�ѧԱ��Ϣ", "��ʾ��Ϣ");
                return;
            }
            //��ȡѧ��
            string studentId = this.dgvStudentList.CurrentRow.Cells["StudentId"].Value.ToString();
            Student objStudent = objStuService.GetStudentById(studentId);
            //��ʾҪ�޸ĵ�ѧԱ��Ϣ����
            FrmEditStudent objFrm = new FrmEditStudent(objStudent);
            if (objFrm.ShowDialog()==DialogResult.OK)
            {
                //ͬ��ˢ��
                btnQuery_Click(null,null);
            }
        }
        //ɾ��ѧԱ����
        private void btnDel_Click(object sender, EventArgs e)
        {
           
        }
        //��������
        private void btnNameDESC_Click(object sender, EventArgs e)
        {
            if (this.dgvStudentList.RowCount==0)
            {
                return;
            }
            this.stuList.Sort(new NameDesc());
            this.dgvStudentList.Refresh();
        }
        //ѧ�Ž���
        private void btnStuIdDESC_Click(object sender, EventArgs e)
        {
            if (this.dgvStudentList.RowCount == 0)
            {
                return;
            }
            this.stuList.Sort(new StuIdDesc());
            this.dgvStudentList.Refresh();
        }
        //����к�
        private void dgvStudentList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.DataGridViewStyle.DgvRowPostPaint(this.dgvStudentList, e);
        }
        //��ӡ��ǰѧԱ��Ϣ
        private void btnPrint_Click(object sender, EventArgs e)
        {
          
        }

        //�ر�
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //������Excel
        private void btnExport_Click(object sender, EventArgs e)
        {

        }
        //�Ҽ��˵��޸�ѧԱ
        private void tsmiModifyStu_Click(object sender, EventArgs e)
        {
            btnEidt_Click(null, null);
        }
    }
    #region ʵ������
    class NameDesc : IComparer<Student>
    {
        public int Compare(Student x, Student y)
        {
            return y.StudentName.CompareTo(x.StudentName);
        }
    }
    class StuIdDesc : IComparer<Student>
    {
        public int Compare(Student x, Student y)
        {
            return y.StudentId.CompareTo(x.StudentId);
        }
    }
    #endregion

}