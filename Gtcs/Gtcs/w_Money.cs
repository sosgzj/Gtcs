using FastReport;
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
    public partial class w_Money : Form
    {
        public static  EnvironmentSettings eSet = new  EnvironmentSettings();
        public w_Money()
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
                    if (tbSSJE.Text.Trim() != "")
                    {
                        Cash();
                    }                        
                    return true;
                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }
        }

        private void w_Money_Load(object sender, EventArgs e)
        {
            this.Show();
            this.TopMost = true;
            tbYS.Text = string.Format("{0:F2}", double.Parse(this.Tag.ToString()));
            tbSSJE.Focus();


            String str2 = "select jzfs,je FROM t_jzfs";
            Gtsql r1 = new Gtsql();
            DataTable d1 = new DataTable();
            d1 = r1.ExecuteQuery(str2);
            if (d1 != null && d1.Rows.Count > 0)
            {

                dataGridView2.DataSource = d1;   //将查询结果放入到dataGridView；

                this.dataGridView2.Columns[0].HeaderText = "付款方式";
                this.dataGridView2.Columns[1].HeaderText = "金额";

                dataGridView2.Columns[0].ReadOnly = true;

                dataGridView2.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView2.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;

                // 调整字体
                dataGridView2.Font = new Font("宋体", 15);
                dataGridView2.Columns[0].Width = 150;
                dataGridView2.Columns[1].Width = 130;

                if(Global.d_svipje >= decimal.Parse(tbYS.Text))
                {
                    dataGridView2.Rows[1].Cells[1].Value = double.Parse(this.Tag.ToString()).ToString("F2");
                }
                else
                {
                    dataGridView2.Rows[0].Cells[1].Value = double.Parse(this.Tag.ToString()).ToString("F2");                    
                }
                


            }
        }

        private void tbSSJE_TextChanged(object sender, EventArgs e)
        {
            tbZL.Text = "";
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView2.CurrentCell = this.dataGridView2.Rows[e.RowIndex].Cells[1];
            dataGridView2.BeginEdit(true);
        }

        private void sumje()
        {
            double sum = 0;
            foreach (DataGridViewRow dgr in dataGridView2.Rows)
            {
                sum += double.Parse(dgr.Cells[1].Value.ToString());
                
            }
            //textBox1.Text = (int.Parse(Je.Text) - sum).ToString();

        }
        protected void Cash()
        {
            String s_dh="", s_mc="",s_cash="0.0",s_vip = "0.0", s_hlq = "0.0",s_wx="0.0",s_zfb="0.0", s_bf_consume_dh,s_rec_key;
            string s_cardtxt="";
            int i_flag = 0;

            if (tbZL.Text.Trim() == "")
            {

                tbZL.Text = string.Format("{0:F2}", (double.Parse(tbSSJE.Text.Trim()) - double.Parse(tbYS.Text.Trim())));
            }
            else
            {

                w_Main frmP = (w_Main)this.Owner;
                DataGridView dgv = (DataGridView)frmP.Controls.Find("dGV", true)[0];
                //消费总记录
                //insert into t_pos_bill(dh, mc, bf_consume_dh, total_cost, cash, djq, hy_card, hlq, dh_who_receive, mc_who_receive, rec_key, bc, carddh, dz_ratio, dz_cost, cardvalue)
                //values(:s_dh,:s_mc,:s_bf_consume_dh,:c_last,:c_xian,:c_djq,:c_card,:c_hlq,:s_user.dh,:s_user.xm,:s_rec_key, 0,:s_card.dh,:s_card.zl,:c_djq,:s_card.je) using sqlca;                

                s_dh = Global.s_fh;
                if ((s_dh == "") || (s_dh == null))
                {
                    s_dh = "K000";
                }

                                
                s_mc = Gt.GetListno(1);
                s_bf_consume_dh = s_dh + Gt.GetStime().ToString("yyyyMMddHHmmssfff");
                                
                s_cash = dataGridView2.Rows[0].Cells[1].Value.ToString();
                s_vip = dataGridView2.Rows[1].Cells[1].Value.ToString();
                s_hlq = dataGridView2.Rows[2].Cells[1].Value.ToString();
                s_wx = dataGridView2.Rows[3].Cells[1].Value.ToString();
                s_zfb = dataGridView2.Rows[4].Cells[1].Value.ToString();
                
                s_rec_key = "CS" + Gt.GetStime().ToString("yyMMddHHmmssfff");//超市与包房识别号  

                String str1 = "insert into t_pos_bill(dh,mc,bf_consume_dh,total_cost,cash,hy_card,hlq,wx,zfb,dh_who_receive, mc_who_receive,rec_key,bc,carddh,dz_ratio,dz_cost,cardvalue) values('" + s_dh + "','" + s_mc + "','" + s_bf_consume_dh + "','" + tbYS.Text + "','" + s_cash
                    + "','" + s_vip + "','" + s_hlq + "','" + s_wx + "','" + s_zfb + "','" + Global.s_dh + "','" + Global.s_xm + "','" + s_rec_key + "','" + "0"
                    + "','" + Global.s_vip + "','" + Gt.GetVipinfo(Global.s_vip,1) + "','" + s_vip + "','" + Gt.GetVipinfo(Global.s_vip, 2)
                    +  "')";                

                Gtsql r1 = new Gtsql();   //我写的那个用来连接数据库的类是ResM，所以用其创建对象；                                
                i_flag = r1.ExecuteUpdate(str1);
                if(i_flag == 0)
                {
                    //执行成功
                }
                //添加详单	insert into t_cp_yd(dh,mc,xldh,xlmc,cpdh,cpmc,fastdh,unit,unitprice,nowprice,isnow,is_dz,dzbl,kedian,qy,je,bfdh,is_free,printdh,printname,sequen_kf,rec_key,share_ratio,is_stock,dhid,in_price,printed,is_tc) 
                //select dh, mc, xldh, xlmc, cpdh, cpmc, fastdh, unit, unitprice, nowprice,:i_isnow,is_dz,dzbl,kedian,:c_qy,:c_je,:sle_bf.text,:i_isfree,printdh,printname,:s_bf_consume_dh,:s_rec_key,share_ratio,is_stock,dhid,in_price,1,:i_istc from t_cp
                //         where cpdh = :s_cpdh using sqlca;

                for (int i=0;i<dgv.RowCount; i++)
                {
                    //添加销售记录             
                    string s_cpdh = dgv.Rows[i].Cells[0].Value.ToString();
                    string s_qy = dgv.Rows[i].Cells[3].Value.ToString();                    
                    string s_je = dgv.Rows[i].Cells[4].Value.ToString();
                    string i_istc = dgv.Rows[i].Cells[5].Value.ToString();

                    String str2 = "insert into t_cp_yd(dh,mc,xldh,xlmc,cpdh,cpmc,fastdh,unit,unitprice,nowprice,qy,je,bfdh,sequen_kf,rec_key,is_tc)"
                        + " select dh,mc,xldh,xlmc,cpdh,cpmc,fastdh,unit,unitprice,nowprice,'"+ s_qy  + "','" + s_je + "','" + s_dh +  "','" + s_bf_consume_dh + "','" + s_rec_key + "','" + i_istc + "'" + " from t_cp"  + " where cpdh = '" + s_cpdh + "'";
                    Gtsql r2 = new Gtsql();                                
                    i_flag = r2.ExecuteUpdate(str2);                    


                }
                // 扣会员卡款
                if (Global.s_vip != null)
                {
                    int i_Vipflag;
                    i_Vipflag = Gt.UpdateVip(Global.s_vip, decimal.Parse(s_vip), s_rec_key, 3, "消费");

                    if (i_Vipflag == 1)
                    {                        
                        s_cardtxt = "会员卡:"+ Global.s_vip + "  扣款前:" + Global.d_svipje.ToString("F1")  + "\n扣款后:" + (Global.d_svipje - decimal.Parse(s_vip)).ToString("F1");
                        MessageBox.Show(s_cardtxt, "会员卡扣款", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("会员卡扣款失败!");
                    }
                }


                String str3 = "SELECT cpmc, qy,unitprice,je FROM t_cp_yd WHERE  rec_key ='" + s_rec_key + "'";
                String s_sumjetxt = "select cash,hy_card,hlq,wx,zfb from t_pos_bill where rec_key='" + s_rec_key + "'";

                Gtsql r3 = new Gtsql();
                DataTable d3 = new DataTable();
                string s_Height = "65";
                d3 = r3.ExecuteQuery(str3);          //使用ExecuteQuery（）执行sql语句；
                if (d3 != null && d3.Rows.Count > 0)
                {
                    s_Height = (Convert.ToUInt32(d3.Rows.Count * 7.5) + 85).ToString();

                }
                // 报表路径
                string path = Application.StartupPath + "\\Report\\Report_mx.frx";
                Report report = new Report();
                
                report.Load(path);
                report.SetParameterValue("s_xm", Global.s_xm);
                report.SetParameterValue("s_posdate", Gt.GetStime().ToString("yy/MM/dd hh:mm"));
                report.SetParameterValue("s_mc", s_mc);
                report.SetParameterValue("s_dh", s_dh);
                report.SetParameterValue("s_Height", s_Height);

                report.SetParameterValue("s_sumje", Gt.Getjetxt(s_sumjetxt));
                report.SetParameterValue("s_card", s_cardtxt);
                
                
                report.RegisterData(d3, "t_cp_yd");

                eSet.ReportSettings.ShowProgress = false;

                report.PrintSettings.ShowDialog = false;
                

                report.Print();

                //    //减少货物质量
                //    bGoods.UpdateCount(int.Parse(dgv.Rows[i].Cells[3].Value.ToString()), dgv.Rows[i].Cells[0].Value.ToString());
                //}
 
                frmP.ClearForm();
                frmP.showPre();
                this.Close();


            }
        }
    }

}