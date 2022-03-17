using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DAL;
using Models;


namespace StudentManager
{
    public partial class FrmScoreManage : Form
    {     
        private ScoreListService objScoreService=new ScoreListService();
        public FrmScoreManage()
        {
            InitializeComponent();
            //绑定班级下拉框
            this.cboClass.DataSource = new StudentClassService().GetAllClass();
            this.cboClass.DisplayMember = "ClassName";
            this.cboClass.ValueMember = "ClassId";
            this.cboClass.SelectedIndex = -1;

            //将下拉框的事件关联
            this.cboClass.SelectedIndexChanged += new System.EventHandler(this.cboClass_SelectedIndexChanged);
            //初始化DataGridView
            this.dgvScoreList.AutoGenerateColumns = false;
            new Common.DataGridViewStyle().DgvStyle1(this.dgvScoreList);
        }     
        //根据班级查询      
        private void cboClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cboClass.SelectedIndex==-1)
            {
                MessageBox.Show("请首先选择要查询的班级！", "查询提示");
                return;
            }
            //显示查询结果
            this.dgvScoreList.DataSource = objScoreService.QueryScoreList(this.cboClass.Text.ToString().Trim());
            
            
        }
        //关闭
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //统计全校考试成绩
        private void btnStat_Click(object sender, EventArgs e)
        {
            //显示查询结果
            this.dgvScoreList.DataSource = objScoreService.QueryScoreList("");
            
        }

        private void dgvScoreList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.DataGridViewStyle.DgvRowPostPaint(this.dgvScoreList, e);
        }

    
     
        //选择框选择改变处理
        private void dgvScoreList_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
         
        }

       
    }
}