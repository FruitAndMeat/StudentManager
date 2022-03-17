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
            //��ʾ��ѯ���
            this.dgvScoreList.DataSource = objScoreService.QueryScoreList(this.cboClass.Text.ToString().Trim());
            
            
        }
        //�ر�
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //ͳ��ȫУ���Գɼ�
        private void btnStat_Click(object sender, EventArgs e)
        {
            //��ʾ��ѯ���
            this.dgvScoreList.DataSource = objScoreService.QueryScoreList("");
            
        }

        private void dgvScoreList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.DataGridViewStyle.DgvRowPostPaint(this.dgvScoreList, e);
        }

    
     
        //ѡ���ѡ��ı䴦��
        private void dgvScoreList_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
         
        }

       
    }
}