using FastReport;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Gtcs
{
    public partial class w_Xf : Form
    {
        public static EnvironmentSettings eSet = new EnvironmentSettings();
        public w_Xf()
        {
            InitializeComponent();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Escape:

                    this.Close();
                    return true;
                case Keys.Enter:
                    button1.PerformClick();
                    return true;

                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }
        }

        private void w_Xf_Load(object sender, EventArgs e)
        {
            
            String str = "select dh,mc,total_cost,pos_date,rec_key,carddh from t_pos_bill where bc = 0 and dh_who_receive ='" + Global.s_dh + "' order by pos_date" ;
            Gtsql r1 = new Gtsql();
            DataTable d1 = new DataTable();
            d1 = r1.ExecuteQuery(str);            
            if (d1 != null && d1.Rows.Count > 0)
            {
                dataGridView1.DataSource = d1;   //将查询结果放入到dataGridView；

                dataGridView1.Columns[0].HeaderText = "房号";
                dataGridView1.Columns[1].HeaderText = "单号";
                dataGridView1.Columns[2].HeaderText = "包厢消费";
                dataGridView1.Columns[3].HeaderText = "日期";

                dataGridView1.Columns[0].Width = 60;
                dataGridView1.Columns[1].Width = 80;
                dataGridView1.Columns[2].Width = 60;
                dataGridView1.Columns[3].Width = 160;
                

                dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;       //列表居中        

                dataGridView1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


                dataGridView1.ReadOnly = true;
                dataGridView1.Columns[4].Visible = false;
                dataGridView1.Columns[5].Visible = false;

                Setdgvmx(dataGridView1.Rows[0].Cells[4].Value.ToString());
            }

        }
        public void Setdgvmx(string rec_key)
        {
            String str = "select cpmc,unitprice,qy,je from t_cp_yd where rec_key='" + rec_key + "'";
            Gtsql r1 = new Gtsql();
            DataTable d1 = new DataTable();
            d1 = r1.ExecuteQuery(str);
            if (d1 != null && d1.Rows.Count > 0)
            {
                dataGridView2.DataSource = d1;   //将查询结果放入到dataGridView；

                dataGridView2.Columns[0].HeaderText = "商品名称";
                dataGridView2.Columns[1].HeaderText = "单价";
                dataGridView2.Columns[2].HeaderText = "数量";
                dataGridView2.Columns[3].HeaderText = "金额";

                dataGridView2.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;       //列表居中        

                dataGridView2.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridView2.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView2.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView2.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


                dataGridView2.Columns[0].Width = 180;
                dataGridView2.Columns[1].Width = 60;
                dataGridView2.Columns[2].Width = 60;
                dataGridView2.Columns[3].Width = 60;

                dataGridView2.ReadOnly = true;

            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {

                Setdgvmx(dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString());                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)        {

            if (dataGridView1.RowCount == 0)
            {
                MessageBox.Show("无记录可打印!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
                
            int i_row = int.Parse(dataGridView1.CurrentRow.Index.ToString());
            string s_rec_key = dataGridView1.Rows[i_row].Cells["rec_key"].Value.ToString();            
            string str3 = "SELECT cpmc, qy,unitprice,je FROM t_cp_yd WHERE  rec_key ='" + s_rec_key + "'";
            string s_sumjetxt = "select cash,hy_card,hlq,wx,zfb from t_pos_bill where rec_key='" + s_rec_key + "'";            
            string s_carddh = dataGridView1.Rows[i_row].Cells["carddh"].Value.ToString();
            string s_cardtxt;
            

            if (s_carddh == "")
                s_cardtxt = "";
            else
                s_cardtxt = Gt.GetViptxt(s_rec_key);


            Gtsql r3 = new Gtsql();
            DataTable d3 = new DataTable();
            string s_Height = "65";
            d3 = r3.ExecuteQuery(str3);          //使用ExecuteQuery（）执行sql语句；
            if (d3 != null && d3.Rows.Count > 0)
            {
                s_Height = (Convert.ToUInt32(d3.Rows.Count * 7.5) + 85).ToString();

            }
            
            
            // 报表路径
            string path = Application.StartupPath + "/Report/Report_mx.frx";
            Report report = new Report();

            report.Load(path);
            report.SetParameterValue("s_xm", Global.s_xm);           
            report.SetParameterValue("s_posdate", DateTime.Parse(dataGridView1.Rows[i_row].Cells[3].Value.ToString()).ToString("yy/MM/dd hh:mm"));
            report.SetParameterValue("s_mc", dataGridView1.Rows[i_row].Cells[1].Value.ToString());  //流水号
            report.SetParameterValue("s_dh", dataGridView1.Rows[i_row].Cells[0].Value.ToString());
            report.SetParameterValue("s_Height", s_Height);

             report.SetParameterValue("s_sumje", Gt.Getjetxt(s_sumjetxt));
             report.SetParameterValue("s_card", s_cardtxt);
            

            report.RegisterData(d3, "t_cp_yd");

             eSet.ReportSettings.ShowProgress = false;

             report.PrintSettings.ShowDialog = false;


            report.Print();

        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex % 2 == 1)
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.PowderBlue;
        }
    }
}
