using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace 暗夜辅助服务端
{

    public partial class MainForm : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseClipboard();

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EmptyClipboard();

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GlobalAlloc(uint uFlags, UIntPtr dwBytes);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GlobalUnlock(IntPtr hMem);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GlobalFree(IntPtr hMem);

        Socket listenerSocket;
        IPAddress ipAddress;
        int port;
        IPEndPoint localEndPoint;
        int backlog;
        Socket clientSocket;
        public static string filePath = "C:\\yonghushuju.json";
        public static string heimingdan = "C:\\heimingdan.json";
        public static string km = "C:\\kami.json";
        public MainForm()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ipAddress = IPAddress.Parse("172.16.122.122");
            //ipAddress = IPAddress.Parse("127.0.0.1");
            port = 25101;
            localEndPoint = new IPEndPoint(ipAddress, port);
            listenerSocket.Bind(localEndPoint);
            backlog = 5000; // 最大连接数
            listenerSocket.Listen(backlog);

            comboBoxfwqzt.SelectedIndex = 0;
            comboBoxkaika.SelectedIndex = 0;


            gengxinbiaoge();

            Thread chulith = new Thread(chuli);
            chulith.Start();
            //Debug.WriteLine(toTime("2024年5月8日 18:57:40").AddMinutes(3).ToString("yyyy年MM月dd日 HH:mm:ss"));
        }

        void gengxinbiaoge()
        {
            Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(MainForm.km));

            LoadDataIntoDataGridView(data);
        }

        void chuli()
        {

            while (true)
            {
                clientSocket = listenerSocket.Accept(); // 接受连接
                Debug.WriteLine("收到来自 {0} 的连接！", clientSocket.RemoteEndPoint);
                ThreadPool.QueueUserWorkItem(new WaitCallback(duoxianchengchuli), clientSocket);
            }
        }


        string Encrypt(string plainText)
        {
            byte[] encryptedBytes;
            using (Aes aesAlg = Aes.Create())
            {
                byte[] key = {  0x6E, 0x36, 0xF8, 0xA1, 0x2D, 0x8F, 0xE2, 0x5D,
                                0x87, 0x1F, 0x64, 0x23, 0x5F, 0xF7, 0x97, 0xD5,
                                0x4B, 0x8E, 0x16, 0x51, 0x4D, 0x3A, 0x1C, 0xC7,
                                0xB8, 0xA9, 0x8E, 0x53, 0x2D, 0x7B, 0x28, 0xFF };
                aesAlg.Key = key;
                aesAlg.IV = new byte[16];

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encryptedBytes = msEncrypt.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(encryptedBytes);
        }

        string Decrypt(string cipherText)
        {
            string plaintext = null;
            using (Aes aesAlg = Aes.Create())
            {
                byte[] key = {  0x6E, 0x36, 0xF8, 0xA1, 0x2D, 0x8F, 0xE2, 0x5D,
                                0x87, 0x1F, 0x64, 0x23, 0x5F, 0xF7, 0x97, 0xD5,
                                0x4B, 0x8E, 0x16, 0x51, 0x4D, 0x3A, 0x1C, 0xC7,
                                0xB8, 0xA9, 0x8E, 0x53, 0x2D, 0x7B, 0x28, 0xFF };
                aesAlg.Key = key;
                aesAlg.IV = new byte[16];

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }

        string Recv(Socket cl2)
        {
            byte[] buffer = new byte[1024];
            try
            {
                int bytesRead = cl2.Receive(buffer);
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                return message;
            }
            catch (Exception ex)
            {

            }
            return " ";
        }
        void Send(string response, Socket cl2)
        {
            byte[] responseData = Encoding.UTF8.GetBytes(response);
            try
            {
                cl2.Send(responseData);
            }
            catch (Exception ex)
            {

            }
        }
        string getip(string ip)
        {
            string ipdk = ip.ToString();
            string[] ipfg = ipdk.Split(":");
            if (ipfg.Length > 0)
            {
                return ipfg[0];
            }
            else
            {
                return "0.0.0.0";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridViewkami.Rows.Add("ANYE" + GenerateRandomSequence(30), comboBoxkaika.SelectedItem.ToString());
            for (int i = 0; i < dataGridViewkami.RowCount; i++)
            {
                Json.writejsonkm(dataGridViewkami.Rows[i].Cells[0].Value.ToString(), dataGridViewkami.Rows[i].Cells[1].Value.ToString());
            }

        }

        string GenerateRandomSequence(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789012345678901234567890123456789";
            StringBuilder sb = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(chars.Length);
                sb.Append(chars[index]);
            }

            return sb.ToString();
        }

        private void dataGridViewkami_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }
        void LoadDataIntoDataGridView(Dictionary<string, string> data)
        {
            dataGridViewkami.Rows.Clear();
            //dataGridViewkami.Columns.Add("Key", "Key");
            //dataGridViewkami.Columns.Add("Value", "Value");

            foreach (var pair in data)
            {
                dataGridViewkami.Rows.Add(pair.Key, pair.Value);
            }


        }

        string gettime()
        {
            TimeZoneInfo beijingTimeZone = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");
            DateTime beijingTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, beijingTimeZone);
            return beijingTime.ToString("yyyy年MM月dd日 HH:mm:ss");
        }

        DateTime toTime(string dateTimeStr)
        {
            DateTime dateTime = DateTime.ParseExact(dateTimeStr, "yyyy年MM月dd日 HH:mm:ss", CultureInfo.GetCultureInfo("zh-CN"));
            return dateTime;
        }



        bool comptime(string time)
        {
            TimeZoneInfo beijingTimeZone = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");
            DateTime beijingTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, beijingTimeZone);
            string formattedDateTime = beijingTime.ToString("yyyy年MM月dd日 HH:mm:ss");
            DateTime compareDateTime = DateTime.ParseExact(time, "yyyy年MM月dd日 HH:mm:ss", CultureInfo.GetCultureInfo("zh-CN"));
            if (compareDateTime > beijingTime)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        void duoxianchengchuli(object status)
        {
            try
            {
                label7.Text = (int.Parse(label7.Text) + 1).ToString();
            }
            catch
            {

            }
            Socket cl2 = (Socket)status;
            try
            {
                while (true)
                {
                    string mes = Decrypt(Recv(cl2));
                    if (mes == "")
                    {
                        break;
                    }
                    Debug.WriteLine(mes);
                    if (mes == "获取信息")
                    {
                        Debug.WriteLine(getip(cl2.RemoteEndPoint.ToString()) + mes);
                        if (Json.checkjsonhmd(getip(cl2.RemoteEndPoint.ToString())))
                        {
                            if (Json.readjsonhmd(getip(cl2.RemoteEndPoint.ToString())) == "lahei")
                            {
                                Send(Encrypt("拉黑"), cl2);
                            }
                            else
                            {
                                Send(Encrypt(comboBoxfwqzt.SelectedItem.ToString()), cl2);
                            }
                        }
                        else
                        {
                            Send(Encrypt(comboBoxfwqzt.SelectedItem.ToString()), cl2);
                        }
                    }
                    if (mes.Length > 2)
                    {
                        if (mes.Substring(0, "登录".Length) == "登录")
                        {
                            string[] fenge = mes.Split("|");
                            if (fenge.Length == 5)
                            {
                                string account = fenge[1];
                                string psword = fenge[2];
                                string jiqima = fenge[3];
                                string jiqima2 = fenge[4];
                                Debug.WriteLine(getip(cl2.RemoteEndPoint.ToString() + mes));
                                if (Json.checkjson(account))
                                {
                                    if (Json.readjson2(account, "psword") == psword)
                                    {
                                        string bangdingkami = Json.readjson2(account, "kami");
                                        if (Json.checkjsonkm(bangdingkami))
                                        {
                                            if (account + "|" + jiqima == Json.readjsonkm(bangdingkami) || account + "|" + jiqima2 == Json.readjsonkm(bangdingkami))
                                            {
                                                string times = Json.readjson2(account, "time");
                                                if (comptime(times)) //comptime(times)
                                                {
                                                    Send(Encrypt("登录成功|到期时间：" + times), cl2);
                                                }
                                                else
                                                {
                                                    Send(Encrypt("卡密到期"), cl2);
                                                    Json.deljsonkm(bangdingkami);
                                                    gengxinbiaoge();
                                                }
                                            }
                                            else
                                            {
                                                Send(Encrypt("信息不一致"), cl2);
                                            }
                                        }
                                        else
                                        {
                                            Send(Encrypt("卡密到期"), cl2);
                                            bangdingkami = Json.readjson2(account, "kami");
                                            Json.deljsonkm(bangdingkami);
                                            gengxinbiaoge();
                                        }

                                    }
                                    else
                                    {
                                        Debug.WriteLine(Json.readjson2(account, "psword"));
                                        Send(Encrypt("密码错误"), cl2);
                                    }
                                }
                                else
                                {
                                    Send(Encrypt("账号不存在"), cl2);
                                }
                            }

                        }
                    }
                    if (mes == "退出")
                    {
                        Send(Encrypt("收到"), cl2);
                        break;
                    }
                    if (mes.Length > 2)
                    {
                        Debug.WriteLine("注册");
                        if (mes.Substring(0, "注册".Length) == "注册")
                        {
                            Debug.WriteLine("注册消息");
                            string[] fenge = mes.Split("|");
                            if (fenge.Length == 5)
                            {
                                string account = fenge[1];
                                string psword = fenge[2];
                                string km = fenge[3];
                                string jiqima = fenge[4];
                                if (!Json.checkjson(account))
                                {
                                    Debug.WriteLine("账号不存在");
                                    if (Json.checkjsonkm(km))
                                    {
                                        Debug.WriteLine("卡密存在");
                                        string shijian = Json.readjsonkm(km);
                                        if (shijian == "试用")
                                        {
                                            Debug.WriteLine("试用");
                                            Json.writejson2(account, "psword", psword);
                                            if (Json.checkjson2(account, "time"))
                                            {
                                                if (comptime(Json.readjson2(account, "time")))
                                                {
                                                    Send(Encrypt("卡密未到期"), cl2);
                                                }
                                                else
                                                {
                                                    Json.writejson2(account, "time", toTime(gettime()).AddHours(2).ToString("yyyy年MM月dd日 HH:mm:ss"));
                                                    Json.writejson2(account, "kami", km);
                                                    Json.writejsonkm(km, account + "|" + jiqima);
                                                    Send(Encrypt("注册成功"), cl2);
                                                    gengxinbiaoge();
                                                }
                                            }
                                            else
                                            {
                                                Json.writejson2(account, "time", toTime(gettime()).AddHours(2).ToString("yyyy年MM月dd日 HH:mm:ss"));
                                                Json.writejson2(account, "kami", km);
                                                Json.writejsonkm(km, account + "|" + jiqima);
                                                Send(Encrypt("注册成功"), cl2);
                                                gengxinbiaoge();
                                            }
                                        }
                                        else if (shijian == "天卡")
                                        {
                                            Json.writejson2(account, "psword", psword);
                                            if (Json.checkjson2(account, "time"))
                                            {
                                                if (comptime(Json.readjson2(account, "time")))
                                                {
                                                    Send(Encrypt("卡密未到期"), cl2);
                                                    gengxinbiaoge();
                                                }
                                                else
                                                {
                                                    Json.writejson2(account, "time", toTime(gettime()).AddDays(1).ToString("yyyy年MM月dd日 HH:mm:ss"));
                                                    Json.writejson2(account, "kami", km);
                                                    Json.writejsonkm(km, account + "|" + jiqima);
                                                    Send(Encrypt("注册成功"), cl2);
                                                    gengxinbiaoge();
                                                }
                                            }
                                            else
                                            {
                                                Json.writejson2(account, "time", toTime(gettime()).AddDays(1).ToString("yyyy年MM月dd日 HH:mm:ss"));
                                                Json.writejson2(account, "kami", km);
                                                Json.writejsonkm(km, account + "|" + jiqima);
                                                Send(Encrypt("注册成功"), cl2);
                                                gengxinbiaoge();
                                            }
                                        }
                                        else if (shijian == "周卡")
                                        {
                                            Json.writejson2(account, "psword", psword);
                                            if (Json.checkjson2(account, "time"))
                                            {
                                                if (comptime(Json.readjson2(account, "time")))
                                                {
                                                    Send(Encrypt("卡密未到期"), cl2);
                                                }
                                                else
                                                {
                                                    Json.writejson2(account, "time", toTime(gettime()).AddDays(7).ToString("yyyy年MM月dd日 HH:mm:ss"));
                                                    Json.writejson2(account, "kami", km);
                                                    Json.writejsonkm(km, account + "|" + jiqima);
                                                    Send(Encrypt("注册成功"), cl2);
                                                    gengxinbiaoge();
                                                }
                                            }
                                            else
                                            {
                                                Json.writejson2(account, "time", toTime(gettime()).AddDays(7).ToString("yyyy年MM月dd日 HH:mm:ss"));
                                                Json.writejson2(account, "kami", km);
                                                Json.writejsonkm(km, account + "|" + jiqima);
                                                Send(Encrypt("注册成功"), cl2);
                                                gengxinbiaoge();
                                            }
                                        }
                                        else if (shijian == "月卡")
                                        {
                                            Json.writejson2(account, "psword", psword);
                                            if (Json.checkjson2(account, "time"))
                                            {
                                                if (comptime(Json.readjson2(account, "time")))
                                                {
                                                    Send(Encrypt("卡密未到期"), cl2);
                                                }
                                                else
                                                {
                                                    Json.writejson2(account, "time", toTime(gettime()).AddMonths(1).ToString("yyyy年MM月dd日 HH:mm:ss"));
                                                    Json.writejson2(account, "kami", km);
                                                    Json.writejsonkm(km, account + "|" + jiqima);
                                                    Send(Encrypt("注册成功"), cl2);
                                                    gengxinbiaoge();
                                                }
                                            }
                                            else
                                            {
                                                Json.writejson2(account, "time", toTime(gettime()).AddMonths(1).ToString("yyyy年MM月dd日 HH:mm:ss"));
                                                Json.writejson2(account, "kami", km);
                                                Json.writejsonkm(km, account + "|" + jiqima);
                                                Send(Encrypt("注册成功"), cl2);
                                                gengxinbiaoge();
                                            }
                                        }
                                        else if (shijian == "季卡")
                                        {
                                            Json.writejson2(account, "psword", psword);
                                            if (Json.checkjson2(account, "time"))
                                            {
                                                if (comptime(Json.readjson2(account, "time")))
                                                {
                                                    Send(Encrypt("卡密未到期"), cl2);
                                                }
                                                else
                                                {
                                                    Json.writejson2(account, "time", toTime(gettime()).AddMonths(3).ToString("yyyy年MM月dd日 HH:mm:ss"));
                                                    Json.writejson2(account, "kami", km);
                                                    Json.writejsonkm(km, account + "|" + jiqima);
                                                    Send(Encrypt("注册成功"), cl2);
                                                    gengxinbiaoge();
                                                }
                                            }
                                            else
                                            {
                                                Json.writejson2(account, "time", toTime(gettime()).AddMonths(3).ToString("yyyy年MM月dd日 HH:mm:ss"));
                                                Json.writejson2(account, "kami", km);
                                                Json.writejsonkm(km, account + "|" + jiqima);
                                                Send(Encrypt("注册成功"), cl2);
                                                gengxinbiaoge();
                                            }
                                        }
                                        else if (shijian == "年卡")
                                        {
                                            Json.writejson2(account, "psword", psword);
                                            if (Json.checkjson2(account, "time"))
                                            {
                                                if (comptime(Json.readjson2(account, "time")))
                                                {
                                                    Send(Encrypt("卡密未到期"), cl2);
                                                }
                                                else
                                                {
                                                    Json.writejson2(account, "time", toTime(gettime()).AddYears(1).ToString("yyyy年MM月dd日 HH:mm:ss"));
                                                    Json.writejson2(account, "kami", km);
                                                    Json.writejsonkm(km, account + "|" + jiqima);
                                                    Send(Encrypt("注册成功"), cl2);
                                                    gengxinbiaoge();
                                                }
                                            }
                                            else
                                            {
                                                Json.writejson2(account, "time", toTime(gettime()).AddYears(1).ToString("yyyy年MM月dd日 HH:mm:ss"));
                                                Json.writejson2(account, "kami", km);
                                                Json.writejsonkm(km, account + "|" + jiqima);
                                                Send(Encrypt("注册成功"), cl2);
                                                gengxinbiaoge();
                                            }
                                        }
                                        else if (shijian == "永久卡")
                                        {
                                            Json.writejson2(account, "psword", psword);
                                            if (Json.checkjson2(account, "time"))
                                            {
                                                if (comptime(Json.readjson2(account, "time")))
                                                {
                                                    Send(Encrypt("卡密未到期"), cl2);
                                                }
                                                else
                                                {
                                                    Json.writejson2(account, "time", toTime(gettime()).AddYears(10).ToString("yyyy年MM月dd日 HH:mm:ss"));
                                                    Json.writejson2(account, "kami", km);
                                                    Json.writejsonkm(km, account + "|" + jiqima);
                                                    Send(Encrypt("注册成功"), cl2);
                                                    gengxinbiaoge();
                                                }
                                            }
                                            else
                                            {
                                                Json.writejson2(account, "time", toTime(gettime()).AddYears(10).ToString("yyyy年MM月dd日 HH:mm:ss"));
                                                Json.writejson2(account, "kami", km);
                                                Json.writejsonkm(km, account + "|" + jiqima);
                                                Send(Encrypt("注册成功"), cl2);
                                                gengxinbiaoge();
                                            }
                                        }
                                        else
                                        {
                                            Send(Encrypt("卡密已到期"), cl2);

                                        }
                                    }
                                    else
                                    {
                                        Debug.WriteLine("卡密不存在");
                                        Send(Encrypt("卡密不存在"), cl2);
                                    }
                                }
                                else
                                {
                                    Send(Encrypt("账号已存在"), cl2);
                                }
                            }
                        }
                    }
                    if (mes.Length > 2)
                    {
                        if (mes.Substring(0, "换绑".Length) == "换绑")
                        {
                            string[] fenge = mes.Split("|");
                            if (fenge.Length == 3)
                            {
                                string account = fenge[1];
                                string jiqima = fenge[2];
                                if (Json.checkjson(account))
                                {
                                    if (account == textBox4.Text)
                                    {
                                        string km = Json.readjson2(account, "kami");
                                        string time = Json.readjson2(account, "time");
                                        Json.writejsonkm(km, account + "|" + jiqima);
                                        label9.Text = account + "换绑成功";
                                        Send(Encrypt("换绑成功|" + account + "|" + time), cl2);
                                    }
                                    else
                                    {
                                        Send(Encrypt("无法换绑"), cl2);
                                    }
                                }
                                else
                                {
                                    Send(Encrypt("换绑账号不存在"), cl2);
                                }
                            }
                        }
                    }
                    if (mes.Length > 2)
                    {
                        Debug.WriteLine(mes);
                        if (mes.Substring(0, "充值".Length) == "充值")
                        {
                            string[] fenge = mes.Split("|");
                            Debug.WriteLine(mes);
                            if (fenge.Length == 4)
                            {
                                string account = fenge[1];
                                string km = fenge[2];
                                string jiqima = fenge[3];
                                if (Json.checkjson(account))
                                {
                                    if (Json.checkjsonkm(km))
                                    {
                                        string shijian = Json.readjsonkm(km);
                                        if (shijian == "试用")
                                        {
                                            Debug.WriteLine("试用");
                                            if (Json.checkjson2(account, "time"))
                                            {
                                                Debug.WriteLine(Json.readjson2(account, "time"));
                                                if (comptime(Json.readjson2(account, "time")))
                                                {
                                                    Send(Encrypt("卡密未到期"), cl2);
                                                }
                                                else
                                                {
                                                    Json.writejson2(account, "time", toTime(gettime()).AddHours(2).ToString("yyyy年MM月dd日 HH:mm:ss"));
                                                    Json.writejson2(account, "kami", km);
                                                    Json.writejsonkm(km, account + "|" + jiqima);
                                                    Send(Encrypt("充值成功"), cl2);
                                                    gengxinbiaoge();
                                                }
                                            }
                                            else
                                            {
                                                Json.writejson2(account, "time", toTime(gettime()).AddHours(2).ToString("yyyy年MM月dd日 HH:mm:ss"));
                                                Json.writejson2(account, "kami", km);
                                                Json.writejsonkm(km, account + "|" + jiqima);
                                                Send(Encrypt("充值成功"), cl2);
                                                gengxinbiaoge();
                                            }
                                        }
                                        else if (shijian == "天卡")
                                        {
                                            if (Json.checkjson2(account, "time"))
                                            {
                                                if (comptime(Json.readjson2(account, "time")))
                                                {
                                                    Send(Encrypt("卡密未到期"), cl2);
                                                    gengxinbiaoge();
                                                }
                                                else
                                                {
                                                    Json.writejson2(account, "time", toTime(gettime()).AddDays(1).ToString("yyyy年MM月dd日 HH:mm:ss"));
                                                    Json.writejson2(account, "kami", km);
                                                    Json.writejsonkm(km, account + "|" + jiqima);
                                                    Send(Encrypt("充值成功"), cl2);
                                                    gengxinbiaoge();
                                                }
                                            }
                                            else
                                            {
                                                Json.writejson2(account, "time", toTime(gettime()).AddDays(1).ToString("yyyy年MM月dd日 HH:mm:ss"));
                                                Json.writejson2(account, "kami", km);
                                                Json.writejsonkm(km, account + "|" + jiqima);
                                                Send(Encrypt("充值成功"), cl2);
                                                gengxinbiaoge();
                                            }
                                        }
                                        else if (shijian == "周卡")
                                        {
                                            if (Json.checkjson2(account, "time"))
                                            {
                                                if (comptime(Json.readjson2(account, "time")))
                                                {
                                                    Send(Encrypt("卡密未到期"), cl2);
                                                }
                                                else
                                                {
                                                    Json.writejson2(account, "time", toTime(gettime()).AddDays(7).ToString("yyyy年MM月dd日 HH:mm:ss"));
                                                    Json.writejson2(account, "kami", km);
                                                    Json.writejsonkm(km, account + "|" + jiqima);
                                                    Send(Encrypt("充值成功"), cl2);
                                                    gengxinbiaoge();
                                                }
                                            }
                                            else
                                            {
                                                Json.writejson2(account, "time", toTime(gettime()).AddDays(7).ToString("yyyy年MM月dd日 HH:mm:ss"));
                                                Json.writejson2(account, "kami", km);
                                                Json.writejsonkm(km, account + "|" + jiqima);
                                                Send(Encrypt("充值成功"), cl2);
                                                gengxinbiaoge();
                                            }
                                        }
                                        else if (shijian == "月卡")
                                        {
                                            if (Json.checkjson2(account, "time"))
                                            {
                                                if (comptime(Json.readjson2(account, "time")))
                                                {
                                                    Send(Encrypt("卡密未到期"), cl2);
                                                }
                                                else
                                                {
                                                    Json.writejson2(account, "time", toTime(gettime()).AddMonths(1).ToString("yyyy年MM月dd日 HH:mm:ss"));
                                                    Json.writejson2(account, "kami", km);
                                                    Json.writejsonkm(km, account + "|" + jiqima);
                                                    Send(Encrypt("充值成功"), cl2);
                                                    gengxinbiaoge();
                                                }
                                            }
                                            else
                                            {
                                                Json.writejson2(account, "time", toTime(gettime()).AddMonths(1).ToString("yyyy年MM月dd日 HH:mm:ss"));
                                                Json.writejson2(account, "kami", km);
                                                Json.writejsonkm(km, account + "|" + jiqima);
                                                Send(Encrypt("充值成功"), cl2);
                                                gengxinbiaoge();
                                            }
                                        }
                                        else if (shijian == "季卡")
                                        {
                                            if (Json.checkjson2(account, "time"))
                                            {
                                                if (comptime(Json.readjson2(account, "time")))
                                                {
                                                    Send(Encrypt("卡密未到期"), cl2);
                                                }
                                                else
                                                {
                                                    Json.writejson2(account, "time", toTime(gettime()).AddMonths(3).ToString("yyyy年MM月dd日 HH:mm:ss"));
                                                    Json.writejson2(account, "kami", km);
                                                    Json.writejsonkm(km, account + "|" + jiqima);
                                                    Send(Encrypt("充值成功"), cl2);
                                                    gengxinbiaoge();
                                                }
                                            }
                                            else
                                            {
                                                Json.writejson2(account, "time", toTime(gettime()).AddMonths(3).ToString("yyyy年MM月dd日 HH:mm:ss"));
                                                Json.writejson2(account, "kami", km);
                                                Json.writejsonkm(km, account + "|" + jiqima);
                                                Send(Encrypt("充值成功"), cl2);
                                                gengxinbiaoge();
                                            }
                                        }
                                        else if (shijian == "年卡")
                                        {
                                            if (Json.checkjson2(account, "time"))
                                            {
                                                if (comptime(Json.readjson2(account, "time")))
                                                {
                                                    Send(Encrypt("卡密未到期"), cl2);
                                                }
                                                else
                                                {
                                                    Json.writejson2(account, "time", toTime(gettime()).AddYears(1).ToString("yyyy年MM月dd日 HH:mm:ss"));
                                                    Json.writejson2(account, "kami", km);
                                                    Json.writejsonkm(km, account + "|" + jiqima);
                                                    Send(Encrypt("充值成功"), cl2);
                                                    gengxinbiaoge();
                                                }
                                            }
                                            else
                                            {
                                                Json.writejson2(account, "time", toTime(gettime()).AddYears(1).ToString("yyyy年MM月dd日 HH:mm:ss"));
                                                Json.writejson2(account, "kami", km);
                                                Json.writejsonkm(km, account + "|" + jiqima);
                                                Send(Encrypt("充值成功"), cl2);
                                                gengxinbiaoge();
                                            }
                                        }
                                        else if (shijian == "永久卡")
                                        {
                                            if (Json.checkjson2(account, "time"))
                                            {
                                                if (comptime(Json.readjson2(account, "time")))
                                                {
                                                    Send(Encrypt("卡密未到期"), cl2);
                                                }
                                                else
                                                {
                                                    Json.writejson2(account, "time", toTime(gettime()).AddYears(10).ToString("yyyy年MM月dd日 HH:mm:ss"));
                                                    Json.writejson2(account, "kami", km);
                                                    Json.writejsonkm(km, account + "|" + jiqima);
                                                    Send(Encrypt("充值成功"), cl2);
                                                    gengxinbiaoge();
                                                }
                                            }
                                            else
                                            {
                                                Json.writejson2(account, "time", toTime(gettime()).AddYears(10).ToString("yyyy年MM月dd日 HH:mm:ss"));
                                                Json.writejson2(account, "kami", km);
                                                Json.writejsonkm(km, account + "|" + jiqima);
                                                Send(Encrypt("充值成功"), cl2);
                                                gengxinbiaoge();
                                            }
                                        }
                                        else
                                        {
                                            if (comptime(Json.readjson2(account, "time")))
                                            {
                                                Send(Encrypt("卡密未到期"), cl2);
                                            }
                                            else
                                            {
                                                Send(Encrypt("卡密已到期"), cl2);
                                                Json.deljsonkm(account);
                                                gengxinbiaoge();
                                            }

                                        }
                                    }
                                    else
                                    {
                                        Send(Encrypt("卡密不存在"), cl2);
                                    }
                                }
                                else
                                {
                                    Send(Encrypt("账号不存在"), cl2);
                                }
                            }
                        }
                    }
                    if (mes.Length > 2)
                    {
                        if (mes.Substring(0, "心跳".Length) == "心跳")
                        {
                            string[] fenge = mes.Split("|");
                            if (fenge.Length == 3)
                            {
                                string account = fenge[1];
                                string timejiance = fenge[2];
                                if (toTime(timejiance).AddSeconds(15) > toTime(gettime()))
                                {
                                    if (!comptime(Json.readjson2(account, "time")))
                                    {
                                        Send(Encrypt("卡密到期"), cl2);
                                        Json.deljsonkm(account);
                                        gengxinbiaoge();
                                    }
                                    else
                                    {
                                        Send(Encrypt("正常|" + gettime()), cl2);
                                    }
                                }
                                else
                                {
                                    Send(Encrypt("连接丢失"), cl2);
                                }
                            }
                        }

                    }
                    if (mes.Length > 2)
                    {
                        if (mes.Substring(0, "调试".Length) == "调试")
                        {
                            string[] fenge = mes.Split("|");
                            string ip = fenge[1];
                            string jiqima = fenge[2];
                            Json.writejsonhmd(getip(ip), "lahei");
                            Json.writejsonhmd(jiqima, "lahei");
                            Send(Encrypt("拉黑"), cl2);
                        }
                    }
                    if (mes.Length > 3)
                    {
                        if (mes.Substring(0, "机器码".Length) == "机器码")
                        {
                            string jiqima = mes.Split("|")[1];
                            if (Json.checkjsonhmd(jiqima))
                            {
                                if (Json.readjsonhmd(jiqima) == "lahei")
                                {
                                    Send(Encrypt("***"), cl2);
                                }
                                else
                                {
                                    Send(Encrypt(gettime()), cl2);
                                }
                            }
                            else
                            {
                                Send(Encrypt(gettime()), cl2);
                            }
                        }
                    }
                    if (mes == "获取公告")
                    {
                        Send(Encrypt("公告|" + textBox1.Text), cl2);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error handling client: " + ex.Message);
            }
            finally
            {
                try
                {
                    label7.Text = (int.Parse(label7.Text) - 1).ToString();
                }
                catch
                {

                }
                cl2.Close();
                cl2.Dispose();
            }

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void dataGridViewkami_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                dataGridViewkami.ClearSelection();
                dataGridViewkami.CurrentCell = dataGridViewkami.Rows[e.RowIndex].Cells[e.ColumnIndex];
                dataGridViewkami.Rows[e.RowIndex].Selected = true;
            }
        }

        private void toolStripMenuItemfuzhi_Click(object sender, EventArgs e)
        {
            if (dataGridViewkami.SelectedCells.Count > 0)
            {
                int rowIndex = dataGridViewkami.SelectedCells[0].RowIndex;
                int columnIndex = dataGridViewkami.SelectedCells[0].ColumnIndex;
                object cellValue = dataGridViewkami.Rows[rowIndex].Cells[columnIndex].Value;
                if (cellValue != null)
                {
                    try
                    {
                        if (OpenClipboard(IntPtr.Zero))
                        {
                            EmptyClipboard();
                            byte[] data = System.Text.Encoding.Default.GetBytes(cellValue.ToString() + "\0");
                            IntPtr hGlobal = GlobalAlloc(0x0042, (UIntPtr)data.Length);
                            IntPtr pGlobal = GlobalLock(hGlobal);
                            Marshal.Copy(data, 0, pGlobal, data.Length);
                            GlobalUnlock(hGlobal);
                            SetClipboardData(1, hGlobal);
                            CloseClipboard();
                            Debug.WriteLine("文本已成功复制到剪贴板。");
                        }
                        else
                        {
                            throw new Exception("无法打开剪贴板。");
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("复制文本到剪贴板时发生异常：" + ex.Message);
                    }
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }



        private void button2_Click_1(object sender, EventArgs e)
        {
            if (File.Exists(MainForm.filePath))
            {
                string jsonf = File.ReadAllText(MainForm.filePath);
                JObject root = JObject.Parse(jsonf);
                foreach (var property in root.Properties())
                {
                    try
                    {
                        int hours = int.Parse(textBox2.Text);
                        string name = property.Name.ToString();
                        string time = Json.readjson2(name, "time");
                        Debug.WriteLine(time);
                        Json.writejson2(name, "time", toTime(time).AddHours(hours).ToString("yyyy年MM月dd日 HH:mm:ss"));

                    }
                    catch
                    {

                    }

                }
                MessageBox.Show("加时成功");
            }
            else
            {

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                int a = int.Parse(textBox3.Text);
                for (int i = 0; i < a; i++)
                {
                    Json.writejsonkm("ANYE" + GenerateRandomSequence(30), comboBoxkaika.SelectedItem.ToString());
                }
                gengxinbiaoge();
            }
            catch
            {

            }
        }
    }
}
