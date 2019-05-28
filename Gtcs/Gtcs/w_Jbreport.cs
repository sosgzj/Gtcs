using FastReport;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Gtcs
{    
    public partial class w_Jbreport : Form
    {
        public static EnvironmentSettings eSet = new EnvironmentSettings();
        Report report = new Report();

        public w_Jbreport()
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

        private void button1_Click(object sender, EventArgs e)
        {
            string s_time = "07:00:00";
            string s_bc, s_Height = "0";
            string s_sum;
            

            DateTime dt_1 = DateTime.Parse(dp1.Text + s_time);
            DateTime dt_2 = dt_1.AddDays(1);

            s_bc = Gt.GetBc(textBox1.Text, dp1.Value.ToString("yyyy-MM-dd")).ToString();
            String str = "SELECT  SUM(cash)AS cash, SUM(hy_card) AS hy_card,SUM(hlq)AS hlq, SUM(wx) AS wx,SUM(zfb) AS zfb  FROM t_pos_bill WHERE bc = '" + s_bc + "' and dh_who_receive='" + textBox1.Text + "' and pos_date >= '" + dt_1 + "' and pos_date < '" + dt_2 + "'";

            s_sum = Gt.Getjetxt(str);
            

            String str1 = "SELECT cpmc,  SUM(qy)AS qy, SUM(je) AS je  FROM t_cp_yd  WHERE (rec_key IN(SELECT rec_key  FROM t_pos_bill WHERE bc = '" + s_bc + "' and dh_who_receive='" + textBox1.Text + "' and pos_date >= '" + dt_1 + "' and pos_date < '" + dt_2 + "')) GROUP BY cpdh,cpmc ORDER BY cpdh";
            Gtsql r1 = new Gtsql();
            DataTable d1 = new DataTable();
            d1 = r1.ExecuteQuery(str1);          //使用ExecuteQuery（）执行sql语句；
            if (d1 != null && d1.Rows.Count > 0)
            {   //65为表头高度，7.5 为单行高度
                s_Height = (Convert.ToUInt32(d1.Rows.Count * 7.5) + 85).ToString();
            }
            // 报表路径
            string path = Application.StartupPath + "\\Report\\Report_jb.frx";

            //Report report = new Report();
            report.Load(path);



            report.SetParameterValue("dt_1", dt_1);
            report.SetParameterValue("dt_2", dt_2);

            report.SetParameterValue("s_xm", Global.s_xm);
            report.SetParameterValue("s_jbdate", dp1.Text);
            report.SetParameterValue("s_bc", s_bc);
            report.SetParameterValue("s_Height", s_Height);

            report.SetParameterValue("s_sum", s_sum); //合计

            //report.PrintSettings.Printer = "Foxit Reader PDF Printer";
            //设置打印页码
            //report.PrintSettings.PageNumbers = "1-3";
            //report.PrintSettings.PageNumbers = "1";

            //打印对话框
            // report.PrintSettings.ShowDialog = false;

            report.Preview = previewControl1;

            report.RegisterData(d1, "t_cp_yd");
            
            report.Show();
            

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void w_Jbreport_Load(object sender, EventArgs e)
        {

            textBox1.Text = Global.s_dh;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (report.Preview == null) //判断是否为空
                return;
            previewControl1.Report.PrintSettings.ShowDialog = false;
            eSet.ReportSettings.ShowProgress = false;
            previewControl1.Print();
         }

    }
}
