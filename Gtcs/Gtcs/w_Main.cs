using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;


namespace Gtcs
{
    public partial class w_Main : Form
    {
        public w_Main()
        {
            InitializeComponent();            
            tbCode.TextChanged += new EventHandler(tbCode_TextChanged);
            
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {

            switch (keyData)
            {
                case Keys.Escape:
                    if (MessageBox.Show("确认退出收银界面?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    {
                        this.Close();
                    }
                    return true;
                case Keys.Add:
                    addRow();
                    return true;
                case Keys.Subtract:
                    subRow();                    
                    return true;
                case Keys.F8:
                    if(lblCash.Text != "00.00")
                    {
                        Cash();
                    }                    
                    return true;
                case Keys.F1:
                    Getdh();
                    return true;
                case Keys.F2:
                    Getvip();
                    return true;
                case Keys.F5:
                    ClearForm();
                    return true;
                case Keys.F9:
                    Jb();
                    return true;
                case Keys.F10:
                    JbReport();
                    return true;
                case Keys.F11:
                    Xf();
                    return true;
                case Keys.Delete:
                    delGoods();
                    tbCode.Focus();
                    return true;
                case Keys.Enter:
                    if (!listBox1.Visible)
                        addGoods(tbCode.Text.Trim());
                    else
                        addmc(); //速记码输入
                    return true;

                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }
           
        }

        private void w_Main_Load(object sender, EventArgs e)
        {
            Rectangle ScreenArea = System.Windows.Forms.Screen.GetBounds(this);
            int width1 = ScreenArea.Width;
            int height1 = ScreenArea.Height;
            this.Width = width1;
            this.Height = height1;
            this.Top = 0;
            this.Left = 0;
            ////this.TopMost = true;

            dGV.Columns.Add("Code", "编号");
            dGV.Columns.Add("Name", "名称");
            dGV.Columns.Add("Price", "单价");
            dGV.Columns.Add("Counts", "数量");
            dGV.Columns.Add("Sum", "总价");
            dGV.Columns.Add("Taocan", "套餐");
            dGV.Columns[5].Visible = false;
            dGV.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dGV.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dGV.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            //当前编号
            lblBill.Text = Gt.GetListno(0); 
            lblUser.Text = Global.s_xm;             
            lblTime.Text = DateTime.Now.ToString();
            lblCash.Text = "00.00";
            lblSF.Text = "00.00";
            lblVipje.Text = "0.0";
            showPre();  //显示上一单 
            tbCode.Focus();


            listBox1.Location = new Point(tbCode.Location.X, dGV.Height -  listBox1.Height);



        }

        protected void Cash()
        {
            w_Money frmCash = new w_Money();
            frmCash.Owner = this;
            frmCash.Tag = lblSF.Text;
            frmCash.ShowDialog();
        }
        protected void Xf()
        {
            w_Xf frmXf = new w_Xf();
            frmXf.Owner = this;
            frmXf.Tag = lblSF.Text;
            frmXf.ShowDialog();
        }
        protected void Getdh()
        {
            w_Getdh frmDh = new w_Getdh();
            frmDh.Owner = this;
            frmDh.ShowDialog();
            lbldh.Text = Global.s_fh;

        }
        protected void Jb()
        {
            w_Jb frmDh = new w_Jb();
            frmDh.Owner = this;
            frmDh.ShowDialog();           

        }
        protected void JbReport()
        {
            w_Jbreport frmDh = new w_Jbreport();
            frmDh.Owner = this;
            frmDh.ShowDialog();
        }
        protected void Getvip()
        {
            w_Getvip frmDh = new w_Getvip();
            frmDh.Owner = this;
            frmDh.ShowDialog();
            if(Global.s_vip != "" || Global.s_vip != null)
            {
                lblVip.Text = Global.s_vip;
                lblVipje.Text = string.Format("{0:F2}", Global.d_svipje);
            }
            
        }
        public void addmc()
        {
            string str = "select dhid FROM t_cp WHERE cpmc ='" + listBox1.Text + "'";   //编写sql语句；
            Gtsql r1 = new Gtsql();
            DataTable d1 = new DataTable();
            d1 = r1.ExecuteQuery(str);
            if (d1 != null && d1.Rows.Count > 0)
                addGoods(d1.Rows[0][0].ToString());
        }
        protected void addGoods(string s_code)  //增加商品
        {
            s_code = s_code.Trim();
            string pattern = @"^\d+(\.\d)?$";
            if (s_code !="")
            {               

                if (!Regex.IsMatch(s_code, pattern))
                {
                    //dGV1.Visible = false;
                }
                else
                {
                    //检测是否为多码
                    String str = "select dhno,dhid FROM t_cp_dhid WHERE dhid='" + s_code + "'";   //编写sql语句；
                    Gtsql r = new Gtsql();
                    DataTable d = new DataTable();
                    d = r.ExecuteQuery(str);          //使用ExecuteQuery（）执行sql语句；
                    if (d != null && d.Rows.Count > 0)    //查询有结果
                        s_code = d.Rows[0]["dhno"].ToString();

                    //添加商品            
                    string str1 = "select cpdh,cpmc,unitprice,nowprice FROM t_cp WHERE dhid='" + s_code + "'";   //编写sql语句；   
                    Gtsql r1 = new Gtsql();
                    DataTable d1 = new DataTable();
                    d1 = r1.ExecuteQuery(str1);          //使用ExecuteQuery（）执行sql语句；
                    if (d1 != null && d1.Rows.Count > 0)    //查询有结果
                    {
                        DataGridViewRow row = new DataGridViewRow();
                        row.CreateCells(dGV);
                        row.Cells[0].Value = d1.Rows[0]["cpdh"].ToString();
                        row.Cells[1].Value = d1.Rows[0]["cpmc"].ToString();
                        if (Global.s_vip == "" || Global.s_vip == null)
                            row.Cells[2].Value = string.Format("{0:F2}", Convert.ToDecimal(d1.Rows[0]["unitprice"].ToString()));
                        else
                            row.Cells[2].Value = string.Format("{0:F2}", Convert.ToDecimal(d1.Rows[0]["nowprice"].ToString()));

                        row.Cells[3].Value = "1";
                        row.Cells[4].Value = string.Format("{0:F2}", decimal.Parse(row.Cells[2].Value.ToString()) * decimal.Parse(row.Cells[3].Value.ToString()));
                        row.Cells[5].Value = "0";
                        dGV.Rows.Add(row);
                        row.Selected = true;
                        tbCode.Text = "";


                        //计算总钱数
                        sumCash();
                        lblGoodsName.Text = d1.Rows[0]["cpmc"].ToString();

                        taocan(d1.Rows[0]["cpdh"].ToString()); //检察套餐
                        //lblStock.Text = mGoods.Counts.ToString();
                    }
                    else
                    {
                        MessageBox.Show("对不起,此商品缺货!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        tbCode.SelectAll();
                    }
                }
            }
           
            
        }
        public void taocan(string tcdh) //套餐
        {
            String str1 = "select v_cpdh,v_cpqy FROM t_cp_taocuan_pack WHERE v_tcdh='" + tcdh + "'";   //编写sql语句；
            Gtsql r1 = new Gtsql();
            DataTable d1 = new DataTable();
            d1 = r1.ExecuteQuery(str1);          //使用ExecuteQuery（）执行sql语句；
            if (d1 != null && d1.Rows.Count > 0)    //查询有结果
            {
                for (int i = 0; i < d1.Rows.Count; i++)
                {
                    String str2 = "select cpdh,cpmc,unitprice FROM t_cp WHERE cpdh='" + d1.Rows[i]["v_cpdh"].ToString() + "'";   //编写sql语句；
                    Gtsql r2 = new Gtsql();
                    DataTable d2 = new DataTable();
                    d2 = r2.ExecuteQuery(str2);          //使用ExecuteQuery（）执行sql语句；
                    if (d2 != null && d2.Rows.Count > 0)    //查询有结果
                    {
                        DataGridViewRow row = new DataGridViewRow();
                        row.CreateCells(dGV);
                        row.Cells[0].Value = d2.Rows[0]["cpdh"].ToString();
                        row.Cells[1].Value = d2.Rows[0]["cpmc"].ToString();
                        row.Cells[2].Value = 0;
                        row.Cells[3].Value = d1.Rows[i]["v_cpqy"].ToString();
                        row.Cells[4].Value = 0;
                        row.Cells[5].Value = 1;
                        dGV.Rows.Add(row);
                        row.Selected = true;

                    }

                }

            }
        }
        public void sumCash()
        {
            double cash = 0;
            for (int i = 0; i < dGV.Rows.Count; i++)
            {
                 double price = double.Parse(dGV.Rows[i].Cells[4].Value.ToString());
                 cash = cash + price;
            }
            lblCash.Text = string.Format("{0:F2}", cash);
            lblSF.Text = string.Format("{0:F2}", cash);
            
            //Dong.Model.GlobalsInfo.vipZK =0.99;
            //lblSF.Text = string.Format("{0:F2}", cash * Dong.Model.GlobalsInfo.vipZK);
            //lblVip.Text = Dong.Model.GlobalsInfo.vipCode;
        }

        public void ClearForm()
        {
            lblBill.Text = Gt.GetListno(0);            

            lblCash.Text = "00.00";
            lblSF.Text = "00.00";
            dGV.Rows.Clear();
            tbCode.Text = "";
            lblGoodsName.Text = "";
            lblStock.Text = "";
            lbldh.Text = "";
            lblVip.Text = "";
            lblVipje.Text = "0.0";

            Global.s_vip = "";
        }
        protected void delGoods()
        {
            if (dGV.SelectedRows.Count > 0)
            {
                dGV.Rows.Remove(dGV.SelectedRows[0]);
                //计算总钱数
                sumCash();
            }
        }
        public void showPre()
        {
            String str1 = "select top 1 mc,total_cost,pos_date  FROM t_pos_bill  WHERE dh_who_receive ='" + Global.s_dh + "'" + " order by pos_date DESC";  //编写sql语
            Gtsql r1 = new Gtsql();
            DataTable d1 = new DataTable();
            d1 = r1.ExecuteQuery(str1);         
            if (d1 != null && d1.Rows.Count > 0)    //查询有结果
            {
                
                lblPreBillNo.Text = d1.Rows[0]["mc"].ToString();                
                lblPreBillCash.Text = string.Format("{0:F2}", d1.Rows[0]["total_cost"].ToString());
                lblPreTime.Text = d1.Rows[0]["pos_date"].ToString();
            }
        }
        public void addRow()
        {
            if(dGV.SelectedRows.Count >0)
            {
                double price = double.Parse(dGV.SelectedRows[0].Cells[2].Value.ToString()); //单价
                int qy = int.Parse(dGV.SelectedRows[0].Cells[3].Value.ToString());
                if (qy < 99)
                {
                    dGV.SelectedRows[0].Cells[3].Value = qy + 1; //数量
                }                
                dGV.SelectedRows[0].Cells[4].Value = string.Format("{0:F2}", (price * int.Parse(dGV.SelectedRows[0].Cells[3].Value.ToString())));
                sumCash();
            }
        }
        public void subRow()
        {
            
            if (dGV.SelectedRows.Count > 0)
            {
                double price = double.Parse(dGV.SelectedRows[0].Cells[2].Value.ToString()); //单价
                int qy = int.Parse(dGV.SelectedRows[0].Cells[3].Value.ToString());
                if (qy > 1)
                {
                    dGV.SelectedRows[0].Cells[3].Value = qy - 1; //数量
                }
                dGV.SelectedRows[0].Cells[4].Value = string.Format("{0:F2}", (price * int.Parse(dGV.SelectedRows[0].Cells[3].Value.ToString())));
                sumCash();
            }
        }
        public void editCounts(int counts)
        {
            if (dGV.SelectedRows.Count > 0)
            {
                double price = double.Parse(dGV.SelectedRows[0].Cells[2].Value.ToString());
                dGV.SelectedRows[0].Cells[3].Value = counts.ToString();
                dGV.SelectedRows[0].Cells[4].Value = string.Format("{0:F2}", (price * int.Parse(dGV.SelectedRows[0].Cells[3].Value.ToString())));
                sumCash();
            }
        }
        private void w_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void tbCode_TextChanged(object sender, EventArgs e)
        {
            char chr;
            if (tbCode.Text.Trim().Length > 0 && (char.IsLetter(chr = tbCode.Text[0])))
            {
                listBox1.Visible = true;
                string str1 = "select cpmc FROM t_cp WHERE fastdh LIKE '%" + tbCode.Text + "%'";   //编写sql语句；
                Gtsql r1 = new Gtsql();
                DataTable d1 = new DataTable();
                d1 = r1.ExecuteQuery(str1);          //使用ExecuteQuery（）执行sql语句；
                if (d1 != null && d1.Rows.Count > 0)
                {                  

                    foreach (DataRow dr in d1.Rows)
                    {
                        listBox1.DisplayMember = "cpmc";
                        listBox1.ValueMember = "cpmc";
                        listBox1.DataSource = d1;
                    }
                }
            }
            else
            {
                listBox1.Visible = false;
            }
        }
        

        private void tbCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                e.Handled = true;
                if(e.KeyCode == Keys.Down && listBox1.SelectedIndex < listBox1.Items.Count -1 )
                    listBox1.SelectedIndex = listBox1.SelectedIndex + 1;
                if (e.KeyCode == Keys.Up && listBox1.SelectedIndex > 0)
                    listBox1.SelectedIndex = listBox1.SelectedIndex - 1;
            }       
            
        }

        private void tbCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 0x20)
                e.Handled = true;
        }
    }
}
