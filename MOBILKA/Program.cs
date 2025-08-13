using System;
using System.Collections.Generic;
//using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Data.OleDb;

namespace MOBILKA
{
    static class Program
    {
        public static int uniq=1;
        public static string _com_out;
        public static string _clcc_str;
        public static string _alarm_tel_str;
        public static string _main_db_path;
        public static string _modem_manufuckture="";
        public static string _temp_com_str = "";
        public static string _sms_str = "";
        public static string _sms_mem = "";
        public static Thread modem_reader;
        public static Thread alarm_player;
        public static Thread lic_check;
        public static Thread send_info;
        public static OleDbConnection conn;
        public static int user_level = 3;
        public static string cur_user = "";
        public static string log_file = "";
        public static int cur_alarm;
        public static bool client_mode_only = false;
        public static bool no_lic = true;
        public static bool send_msg = false;
        public static DateTime date_now;
        public static int crazy_i = 0;
        public static int _com_error_count = 0;
        public static int _signal_error_count = 0;
        public static Int64 total_fucking_shit;
        public static bool spacebar = false;
        public static bool app_close_admin_only = false;
        public static bool alarm_in_list = false;
        public static bool _com_error_check = false;
        public static bool screen720x400 = false;
        public static bool total_log = false;
        public static bool _com_read = false;
        public static bool _com_write = false;
        public static bool _clear_all_sms = false;

        public static bool reinit_modem;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
