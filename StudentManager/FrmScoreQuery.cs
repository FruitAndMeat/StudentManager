using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DAL;


namespace StudentManager
{
    public partial class FrmScoreQuery : Form
    {
        private DataSet ds = null;//保存全部查询结果的数据集
        private StudentClassService objClassService = new StudentClassService();
        private ScoreListService objScoreSerice = new ScoreListService();

        public FrmScoreQuery()
        {
            InitializeComponent();
            //基于DataTable绑定下拉框
            DataTable dt = objClassService.GetAllClasses().Tables[0];
            this.cboClass.DataSource = dt;
            this.cboClass.ValueMember = "ClassId";
            this.cboClass.DisplayMember = "ClassName";
            //显示全部考试成绩
            this.ds = objScoreSerice.GetAllScoreList();//将所有成绩保存到成员变量中
            this.dgvScoreList.DataSource = ds.Tables[0];
            new Common.DataGridViewStyle().DgvStyle1(this.dgvScoreList);
        }     

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //根据班级名称动态筛选
        private void cboClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ds == null )return;//目前不会出现为空的状况
            this.ds.Tables[0].DefaultView.RowFilter = $"ClassName='{this.cboClass.Text.Trim()}'";

        }
        //显示全部成绩
        private void btnShowAll_Click(object sender, EventArgs e)
        {
            this.ds.Tables[0].DefaultView.RowFilter = "ClassName like '%%'";
        }
        //根据C#成绩动态筛选
        private void txtScore_TextChanged(object sender, EventArgs e)
        {
            if (this.txtScore.Text.Trim().Length==0)
            {
                return;
            }
            if (Common.DataValidate.IsInteger(this.txtScore.Text.Trim()))
            {
                this.ds.Tables[0].DefaultView.RowFilter = "CSharp>" + this.txtScore.Text.Trim();
            }
            else return;
        }

        private void dgvScoreList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
           Common.DataGridViewStyle.DgvRowPostPaint(this.dgvScoreList, e);
        }

        //打印当前的成绩信息
        private void btnPrint_Click(object sender, EventArgs e)
        {
          
        }
    }
}
