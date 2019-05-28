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
    public partial class w_Getdh : Form
    {
        public w_Getdh()
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

                    if (tbCounts.Text != "")
                    {

                        Global.s_fh = "K" + tbCounts.Text;
                        this.Close();
                    }

                    return true;

                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }
        }
    }
}
