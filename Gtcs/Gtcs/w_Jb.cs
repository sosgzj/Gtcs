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
    public partial class w_Jb : Form
    {
        public w_Jb()
        {
            InitializeComponent();
        }

        private void w_Jb_Load(object sender, EventArgs e)
        {
            label2.Text = Gt.GetJbdate().ToString("yyyy-MM-dd");
            comboBox1.SelectedIndex = 0;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i_count=0;
            Gtsql r1 = new Gtsql();
            DataTable d1 = new DataTable();
            
            i_count = Gt.GetBc(Global.s_dh,label2.Text);
            if (i_count > 0)  //有交班
            {
                    if(i_count != int.Parse(comboBox1.Text))
                    {                        
                        MessageBox.Show("请选择原来的班次： " + i_count.ToString() + " 交班");
                        return;
                    }               

            }

            String str = "update t_pos_bill set jbdate ='" + label2.Text + "'" + ",bc='" + comboBox1.Text + "' where bc = 0";
            i_count = r1.ExecuteUpdate(str);
            if (i_count >= 1)
            {
                 MessageBox.Show("交班成功!");
                 this.Close();
            }
            else
            {
                MessageBox.Show("交班不成功!");
            }            

        }
        
    }
}
