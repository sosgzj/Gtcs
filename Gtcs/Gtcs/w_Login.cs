using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;


namespace Gtcs
{
    public partial class w_login : Form
    {
        public w_login()
        {
            InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Escape:
                    Application.Exit();
                    return true;
                case Keys.Enter:
                    button1_Click(null, null);
                    return true;
                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }
        }
                

        private void w_login_Load(object sender, EventArgs e)
        {

            //配置服务器
            Global.serverName = ConfigurationManager.AppSettings["serverName"];
            Global.serverNamevip = ConfigurationManager.AppSettings["serverNamevip"];



            String str1 = "select dh FROM t_pwd";
            Gtsql r1 = new Gtsql();   //我写的那个用来连接数据库的类是Gtsql，所以用其创建对象；
            DataTable d1 = new DataTable();
            d1 = r1.ExecuteQuery(str1);
            if (d1 != null && d1.Rows.Count > 0)
            {

                cbUserid.ValueMember = "dh";
                cbUserid.DisplayMember = "dh";
                cbUserid.DataSource = d1;

            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //登录
            if (tbPwd.Text == "" || cbUserid.Text == "")
            {
                MessageBox.Show("请输入登录口令!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbPwd.SelectAll();
                return;
                
            }
            String str1 = "select dh,xm,pw,bm FROM t_pwd WHERE dh='" + cbUserid.Text + "'AND pw='" + tbPwd.Text + "'";   //编写sql语句；
            Gtsql r1 = new Gtsql();
            DataTable d1 = new DataTable();
            d1 = r1.ExecuteQuery(str1);          //使用ExecuteQuery（）执行sql语句；
            if (d1 != null && d1.Rows.Count > 0)    //查询有结果
            {

                Global.s_dh = cbUserid.Text;
                Global.s_xm = d1.Rows[0]["xm"].ToString();
                Global.s_bm = d1.Rows[0]["bm"].ToString();
                Global.s_mac = Gt.GetNetworkAdpaterID();

                w_Main frm = new w_Main(); //这三行代码是实现界面跳转；
                frm.Show();
                this.Visible = false;
            }
            else
            {
                MessageBox.Show("您输入的口令不正确!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbPwd.SelectAll();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
