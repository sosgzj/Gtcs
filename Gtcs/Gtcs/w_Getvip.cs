using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gtcs
{
    public partial class w_Getvip : Form
    {
        public w_Getvip()
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

                    if (tbVipCode.Text.Trim() != "" && tbVipName.Text.Trim() == "")
                    {
                        String str1 = "select carddh,xm,cardvalue,cardflag FROM t_card WHERE carddh='" + tbVipCode.Text.Trim() + "'";   //编写sql语句；
                        Gtsqlvip r1 = new Gtsqlvip();
                        DataTable d1 = new DataTable();
                        d1 = r1.ExecuteQuery(str1);          //使用ExecuteQuery（）执行sql语句；
                        if (d1 != null && d1.Rows.Count > 0) //找到
                         {
                            if (d1.Rows[0]["cardflag"].ToString() == "1")
                            {
                                tbVipCode.Text = d1.Rows[0]["carddh"].ToString();
                                tbVipName.Text = d1.Rows[0]["xm"].ToString();
                                Global.s_vip = tbVipCode.Text;
                                Global.d_svipje = decimal.Parse(d1.Rows[0]["cardvalue"].ToString());                                
                            }
                            else
                            {
                                MessageBox.Show("此卡不是有效卡!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                tbVipCode.Focus();
                                tbVipCode.Select(0, tbVipCode.TextLength);                                
                            }
                        }
                        else //找不到
                        {
                            MessageBox.Show("无此卡信息!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            tbVipCode.Text = "";
                        }

                    }
                    else
                    {
                        this.Close();
                        return true;
                    }
                        return true;

                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }
        }
    }
}
