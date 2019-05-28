using System;
using System.Data;
using System.Data.SqlClient;
using System.Management;

namespace Gtcs
{
    class Gt
    {
        public static DateTime GetStime()
        {

            SqlConnection objSqlConnection = new SqlConnection(Global.serverName);
            DataSet objDataSet = new DataSet();
            string UserSqlstr = "SELECT  GETDATE()";
            SqlDataAdapter objDataAdpter = new SqlDataAdapter(UserSqlstr, objSqlConnection);
            objDataAdpter.Fill(objDataSet);
            DateTime dt_temp = Convert.ToDateTime(objDataSet.Tables[0].Rows[0][0]);

            return dt_temp;
        }
        public static int GetBc(string s_dh,string s_jbdate)  //班次
        {
            int i_bc=0;

            String str = "select top 1 bc from t_pos_bill where dh_who_receive='" + s_dh + "'" + " and jbdate ='" + s_jbdate + "'";
            Gtsql r1 = new Gtsql();
            DataTable d1 = new DataTable();
            d1 = r1.ExecuteQuery(str);            
            if (d1 != null && d1.Rows.Count > 0)    //查询有结果
            {
                i_bc = int.Parse(d1.Rows[0]["bc"].ToString());
            }


                return i_bc;
        }
        public static string Getjetxt(string str)
        {

            String s_sum = "合计：";

            Gtsql r2 = new Gtsql();
            DataTable d2 = new DataTable();
            d2 = r2.ExecuteQuery(str);          //使用ExecuteQuery（）执行sql语句；

            if (d2 != null && d2.Rows.Count > 0 && !d2.Rows[0].IsNull("cash"))
            {               
                
                if (decimal.Parse(d2.Rows[0]["cash"].ToString()) > 0 )
                {
                    s_sum = s_sum + " [现金]" + Decimal.Parse(d2.Rows[0]["cash"].ToString()).ToString("F1");
                }
                if (decimal.Parse(d2.Rows[0]["hy_card"].ToString()) > 0)
                {
                    s_sum = s_sum + " [会员卡]" + Decimal.Parse(d2.Rows[0]["hy_card"].ToString()).ToString("F1");
                }
                if (decimal.Parse(d2.Rows[0]["hlq"].ToString()) > 0)
                {
                    s_sum = s_sum + " [代金券]" + Decimal.Parse(d2.Rows[0]["hlq"].ToString()).ToString("F1");
                }
                if (decimal.Parse(d2.Rows[0]["wx"].ToString()) > 0)
                {
                    s_sum = s_sum + " [微信]" + Decimal.Parse(d2.Rows[0]["wx"].ToString()).ToString("F1");
                }
                if (decimal.Parse(d2.Rows[0]["zfb"].ToString()) > 0)
                {
                    s_sum = s_sum + " [支付]" + Decimal.Parse(d2.Rows[0]["zfb"].ToString()).ToString("F1");
                }

            }        
            return s_sum;
        }
        public static string GetViptxt(string s_rec_key)
        {
            
            string str = "select  carddh,cardvalue,dz_ratio,dz_cost from t_pos_bill where rec_key = '" + s_rec_key + "'";
            string s_sum = "";
            Gtsql r2 = new Gtsql();
            DataTable d2 = new DataTable();
            d2 = r2.ExecuteQuery(str);          //使用ExecuteQuery（）执行sql语句；
            if (d2 != null && d2.Rows.Count > 0)
            {                
                s_sum = "会员卡：" + d2.Rows[0]["carddh"].ToString() + "  扣款前：" + decimal.Parse(d2.Rows[0]["cardvalue"].ToString()).ToString("F1") + "\n扣款后：" + (decimal.Parse(d2.Rows[0]["cardvalue"].ToString()) - decimal.Parse(d2.Rows[0]["dz_cost"].ToString())).ToString("F1");                
            }
            return s_sum;
        }
        public static string GetSystemSet(int i_col)
        {
            SqlConnection objSqlConnection = new SqlConnection(Global.serverName);
            DataSet objDataSet = new DataSet();
            string UserSqlstr = "SELECT * FROM t_set";

            SqlDataAdapter objDataAdpter = new SqlDataAdapter(UserSqlstr, objSqlConnection);
            objDataAdpter.Fill(objDataSet);
            return objDataSet.Tables[0].Rows[0][i_col].ToString();

        }
        public static DateTime GetJbdate()
        {
            DateTime dt_temp;

            if (int.Parse(GetStime().ToString("HH")) > 7)
            {
                dt_temp = GetStime();

            }
            else
                dt_temp = GetStime().AddDays(-1);
            return dt_temp;
        }
        public static string GetVipinfo(string s_card, int i)
        {
            string str = "select cardvalue,dzratio from t_card where carddh='" + s_card + "'";
            Gtsqlvip  r1 = new Gtsqlvip();
            DataTable d1 = new DataTable();
            d1 = r1.ExecuteQuery(str);
            string s_vipinfo="0.0";
            if (d1 != null && d1.Rows.Count > 0)    //查询有结果
            {
                
                switch (i)
                {
                    case 1:
                        s_vipinfo = d1.Rows[0]["cardvalue"].ToString();
                        break;
                    case 2:
                        s_vipinfo = d1.Rows[0]["dzratio"].ToString();
                        break;
                    default:
                        s_vipinfo = "";
                        break;
                }
            }
        
            return s_vipinfo;
        }

        public static int UpdateVip(string s_card,decimal cardvalue,string rec_key,int i_valueflag,string s_valueflagtext)
        {
            //---------------------------------------------------------------------------------
            //	arg_carddh 卡号
            //	arg_cardvalue   dec 扣除钱数 
            //	arg_rec_key    标识码
            //	arg_valueflag   int 充值类型代号
            //	arg_valueflagtext 充值类型
            //----------------------------------
            //insert into t_card_fillvalue(carddh, cardvalue, who, who_name, rec_key, is_act_fill, v_mac, now_cardvalue, valueflag, valueflagtext, bm)
            //    values(:arg_carddh,:arg_cardvalue,:s_user.dh,:s_user.xm,:arg_rec_key, "0",:s_user.mac,:c_cardvalue,:arg_valueflag,:s_valueflagtext,:arg_bm)
            //    using sqlcb;
            //update t_card set cardvalue = :c_cardvalue,totalcost = :c_totalcost where carddh = :arg_carddh using sqlcb;
            int i_flag;
            decimal d_cardvalue,d_totalcost;
            d_cardvalue = Global.d_svipje - cardvalue;

            String str3 = "select carddh,xm,cardvalue,totalcost from t_card where carddh ='" + s_card + "'";
            Gtsqlvip r3 = new Gtsqlvip();
            DataTable d3 = new DataTable();
            d3 = r3.ExecuteQuery(str3);
            d_totalcost =  decimal.Parse(d3.Rows[0]["totalcost"].ToString()) + cardvalue;       


            String str = "insert into t_card_fillvalue(carddh,cardvalue,who,who_name,rec_key,valueflag,valueflagtext,v_mac,bm,now_cardvalue) values('"
                + s_card + "','" + (cardvalue * -1).ToString() + "','" + Global.s_dh + "','" + Global.s_xm + "','" + rec_key + "','" + i_valueflag + "','"
                + s_valueflagtext + "','" + Global.s_mac + "','" + Global.s_bm + "','" + Global.d_svipje + "')";            
            Gtsqlvip r1 = new Gtsqlvip();            
            i_flag = r1.ExecuteUpdate(str);



            String str2 = "update t_card set cardvalue = '" + d_cardvalue  + "',totalcost = '" + d_totalcost  + "' where carddh ='" + s_card + "'";                  
            Gtsqlvip r2 = new Gtsqlvip();
            i_flag = r2.ExecuteUpdate(str2);

            if (i_flag == 1 )
            {
                return 1;
            }
            else
            {
                return 0 ;
            }                
        }
        // 获得当前机器的活动中Mac地址，若无联网则返回空""
        /// 需在项目引用中添加 System.Management
        /// </summary>
        /// <returns>mac地址，例如：00-00-00-00-00-00</returns>
        public static string GetNetworkAdpaterID()
        {
            try
            {
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        mac = mo["MacAddress"].ToString();
                        break;
                    }
                moc = null;
                mc = null;
                return mac.Trim().Replace(':', '-');
            }
            catch (Exception e)
            {
                return "error:" + e.Message;
            }
        }
        public static string GetListno(int i_set)  //返回流水号 0不更新 1更新
        {
            DateTime dt = Gt.GetStime();
            String str;
            long l_listno = 0;
            string s_listno;
            int i_flag = 0;

            if (int.Parse(dt.Hour.ToString()) > 6)
            {

                str = "SELECT listno FROM t_pos_bill_number WITH ( TABLOCKX ) where listdate ='" + dt.Date + "'";
                Gtsql r1 = new Gtsql();
                DataTable d1 = new DataTable();                
                d1 = r1.ExecuteQuery(str);
                if (d1 != null && d1.Rows.Count > 0)
                {
                    l_listno = long.Parse(d1.Rows[0]["listno"].ToString());
                    i_flag = 1;
                }
            }
            else
            {
                str = "select listno from t_pos_bill_number with(tablockx)";
                Gtsql r1 = new Gtsql();
                DataTable d1 = new DataTable();
                d1 = r1.ExecuteQuery(str);
                if (d1 != null && d1.Rows.Count > 0)
                {
                    l_listno = long.Parse(d1.Rows[0]["listno"].ToString());
                    dt = dt.AddDays(-1);
                    i_flag = 1;
                }

            }
            if (i_set == 1)
            {
                if (i_flag == 1)
                {
                    l_listno++;
                    str = "update t_pos_bill_number set listno ='" + l_listno + "'";
                    Gtsql r2 = new Gtsql();
                    r2.ExecuteUpdate(str);
                }
                else
                {
                    l_listno = 1;
                    str = "update t_pos_bill_number set listno ='" + l_listno + "'" + "," + "listdate = '" + dt.Date + "'";
                    Gtsql r3 = new Gtsql();
                    r3.ExecuteUpdate(str);
                }

            }
            else
            {
                if (i_flag == 1)
                {
                    l_listno++;
                }
                else
                {
                    l_listno = 1;

                }
            }

            s_listno = dt.ToString("MMdd") + "-" + l_listno.ToString("d3");
            return s_listno;
        }
    }
}
