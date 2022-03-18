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
            //【1】、显示成绩查询结果列表
            this.dgvScoreList.DataSource = objScoreService.QueryScoreList(this.cboClass.Text.ToString().Trim());
            QueryScore(this.cboClass.SelectedValue.ToString());
        }
        //关闭
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void QueryScore(string classId)
        {
            //【2】、显示查询统计结果
            Dictionary<string, string> infoList = objScoreService.QueryScoreInfo(classId);
            this.lblAttendCount.Text = infoList["stuCount"];
            this.lblAbsentCount.Text = infoList["absentCount"];
            this.lblCSharpAvg.Text = infoList["avgCSharp"];
            this.lblDBAvg.Text = infoList["avgDB"];
            //【3】、显示缺考人员列表
            List<string> absentList = objScoreService.QueryAbsentList(classId);
            this.lblList.Items.Clear();
            if (absentList.Count == 0)
            {
                this.lblList.Items.Add("没有缺考");
            }
            else
            {
                this.lblList.Items.AddRange(absentList.ToArray());
            }
        }
        //统计全校考试成绩
        private void btnStat_Click(object sender, EventArgs e)
        {
            //显示查询结果
            this.dgvScoreList.DataSource = objScoreService.QueryScoreList("");
            QueryScore("");
        }

        private void dgvScoreList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.DataGridViewStyle.DgvRowPostPaint(this.dgvScoreList, e);
        }

    
     
        //选择框选择改变处理
        private void dgvScoreList_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
         
        }
        //解析组合属性
        private void dgvScoreList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex==0&&e.Value is Student)
            {
                e.Value = (e.Value as Student).StudentId;
            }
            if (e.ColumnIndex == 1 && e.Value is Student)
            {
                e.Value = (e.Value as Student).StudentName;
            }
            if (e.ColumnIndex == 2 && e.Value is Student)
            {
                e.Value = (e.Value as Student).Gender;
            }
            if (e.ColumnIndex == 3 && e.Value is StudentClass)
            {
                e.Value = (e.Value as StudentClass).ClassName;
            }
            if (e.ColumnIndex == 4 && e.Value is ScoreList)
            {
                e.Value = (e.Value as ScoreList).CSharp;
            }
            if (e.ColumnIndex == 5 && e.Value is ScoreList)
            {
                e.Value = (e.Value as ScoreList).SQLServerDB;
            }
        }
    }
}