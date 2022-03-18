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
            //�󶨰༶������
            this.cboClass.DataSource = new StudentClassService().GetAllClass();
            this.cboClass.DisplayMember = "ClassName";
            this.cboClass.ValueMember = "ClassId";
            this.cboClass.SelectedIndex = -1;

            //����������¼�����
            this.cboClass.SelectedIndexChanged += new System.EventHandler(this.cboClass_SelectedIndexChanged);
            //��ʼ��DataGridView
            this.dgvScoreList.AutoGenerateColumns = false;
            new Common.DataGridViewStyle().DgvStyle1(this.dgvScoreList);
        }     
        //���ݰ༶��ѯ      
        private void cboClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cboClass.SelectedIndex==-1)
            {
                MessageBox.Show("������ѡ��Ҫ��ѯ�İ༶��", "��ѯ��ʾ");
                return;
            }
            //��1������ʾ�ɼ���ѯ����б�
            this.dgvScoreList.DataSource = objScoreService.QueryScoreList(this.cboClass.Text.ToString().Trim());
            QueryScore(this.cboClass.SelectedValue.ToString());
        }
        //�ر�
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void QueryScore(string classId)
        {
            //��2������ʾ��ѯͳ�ƽ��
            Dictionary<string, string> infoList = objScoreService.QueryScoreInfo(classId);
            this.lblAttendCount.Text = infoList["stuCount"];
            this.lblAbsentCount.Text = infoList["absentCount"];
            this.lblCSharpAvg.Text = infoList["avgCSharp"];
            this.lblDBAvg.Text = infoList["avgDB"];
            //��3������ʾȱ����Ա�б�
            List<string> absentList = objScoreService.QueryAbsentList(classId);
            this.lblList.Items.Clear();
            if (absentList.Count == 0)
            {
                this.lblList.Items.Add("û��ȱ��");
            }
            else
            {
                this.lblList.Items.AddRange(absentList.ToArray());
            }
        }
        //ͳ��ȫУ���Գɼ�
        private void btnStat_Click(object sender, EventArgs e)
        {
            //��ʾ��ѯ���
            this.dgvScoreList.DataSource = objScoreService.QueryScoreList("");
            QueryScore("");
        }

        private void dgvScoreList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.DataGridViewStyle.DgvRowPostPaint(this.dgvScoreList, e);
        }

    
     
        //ѡ���ѡ��ı䴦��
        private void dgvScoreList_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
         
        }
        //�����������
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