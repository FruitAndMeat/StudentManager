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
            //初始化班级下拉框
            this.cboClass.DataSource=objClassService.GetAllClass();
            this.cboClass.DisplayMember = "ClassName";
            this.cboClass.ValueMember = "ClassId";
            this.cboClass.SelectedIndex = -1;

            this.dgvStudentList.AutoGenerateColumns = false;
        
        }
        //按照班级查询
        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (this.cboClass.SelectedIndex==-1)
            {
                MessageBox.Show("请选择班级！", "查询提示");
                return;
            }
            //执行查询并绑定数据
            this.stuList = objStuService.GetStudentByClass(this.cboClass.Text.ToString());
            this.dgvStudentList.DataSource=this.stuList;
            new Common.DataGridViewStyle().DgvStyle1(this.dgvStudentList);
            
        }
        //根据学号查询
        private void btnQueryById_Click(object sender, EventArgs e)
        {
            //数据验证
            if (this.txtStudentId.Text.Trim().Length==0)
            {
                MessageBox.Show("请输入查询的学号！", "提示信息");
                this.txtStudentId.Focus();
                return;
            }
            if (!Common.DataValidate.IsInteger(this.txtStudentId.Text.Trim()))
            {
                MessageBox.Show("学号必须是整数！", "提示信息");
                this.txtStudentId.SelectAll();
                this.txtStudentId.Focus();
                return;
            }
            Student objStudent = objStuService.GetStudentById(this.txtStudentId.Text.Trim());
            if (objStudent==null)
            {
                MessageBox.Show("学院信息不存在", "查询提示");
                this.txtStudentId.Focus();
            }
            else
            {
                //显示学员信息
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
        //双击选中的学员对象并显示详细信息
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
        //修改学员对象
        private void btnEidt_Click(object sender, EventArgs e)
        {
            if (this.dgvStudentList.RowCount==0)
            {
                MessageBox.Show("没有任何要修改的学员信息", "提示信息");
                return;
            }
            if (this.dgvStudentList.CurrentRow==null)
            {
                MessageBox.Show("请选中要修改的学员信息", "提示信息");
                return;
            }
            //获取学号
            string studentId = this.dgvStudentList.CurrentRow.Cells["StudentId"].Value.ToString();
            Student objStudent = objStuService.GetStudentById(studentId);
            //显示要修改的学员信息窗口
            FrmEditStudent objFrm = new FrmEditStudent(objStudent);
            if (objFrm.ShowDialog()==DialogResult.OK)
            {
                //同步刷新
                btnQuery_Click(null,null);
            }
        }
        //删除学员对象
        private void btnDel_Click(object sender, EventArgs e)
        {
           
        }
        //姓名降序
        private void btnNameDESC_Click(object sender, EventArgs e)
        {
            if (this.dgvStudentList.RowCount==0)
            {
                return;
            }
            this.stuList.Sort(new NameDesc());
            this.dgvStudentList.Refresh();
        }
        //学号降序
        private void btnStuIdDESC_Click(object sender, EventArgs e)
        {
            if (this.dgvStudentList.RowCount == 0)
            {
                return;
            }
            this.stuList.Sort(new StuIdDesc());
            this.dgvStudentList.Refresh();
        }
        //添加行号
        private void dgvStudentList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.DataGridViewStyle.DgvRowPostPaint(this.dgvStudentList, e);
        }
        //打印当前学员信息
        private void btnPrint_Click(object sender, EventArgs e)
        {
          
        }

        //关闭
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //导出到Excel
        private void btnExport_Click(object sender, EventArgs e)
        {

        }
        //右键菜单修改学员
        private void tsmiModifyStu_Click(object sender, EventArgs e)
        {
            btnEidt_Click(null, null);
        }
    }
    #region 实现排序
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