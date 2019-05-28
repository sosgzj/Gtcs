using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gtcs
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new w_login());
        }
    }
   
    public class Global
    {
         //全局用户结构信息
        public static string s_xm,s_dh,s_bm,s_pw,s_mac;
        
        //房号，会员卡号
        public static string s_fh,s_vip;
        public static decimal d_svipje;

        public static int l_Listflag; //流水号成功标志


        //服务器配置
        public static string conString;
        public static string serverName;
        public static string serverNamevip;


        //conString = "Server=.;user=sa;pwd=sa;database=oskktv" ;



    }

}
