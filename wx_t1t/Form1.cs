using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RestSharp;

using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Threading;

namespace wx_t1t
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private Action OnPostSuccess;
        private Action<string> OnPostFail;
        long times { get; set; }
        static string session_id { get; set; }
        static int version = 9;
        static string base_req { get; set; }
        int score { get; set; }
        int playTimeSeconds { get; set; }
        string base_site = "https://mp.weixin.qq.com/wxagame/";

        string referer = "https://servicewechat.com/wx7c8d593b2c3a7703/6/page-frame.html";

        string USER_AGENT = "Mozilla/5.0 (iPhone; CPU iPhone OS 11_2_1 like Mac OS X) AppleWebKit/604.4.7 (KHTML, like Gecko) Mobile/15C153 MicroMessenger/6.6.1 NetType/WIFI Language/zh_CN";


        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(SessionId.Text))
            {
                session_id = SessionId.Text;
                score = (int)ScoreNum.Value;
                base_req = string.Format("{{\"base_req\":{{\"session_id\":\"{0}\",\"fast\":1}},\"version\":{1}}}", session_id, version);
                playTimeSeconds = score*(int)TimeNum.Value;
                button1.Enabled = false;
                button1.Text = "提交中.";
                OnPostSuccess = () =>
                {
                    RunInMainthread(() =>
                    {
                        MessageBox.Show("修改成功");
                        button1.Text = "提交";
                        button1.Enabled = true;
                    });
                };
                OnPostFail = (err) =>
                {
                    RunInMainthread(() =>
                    {
                        MessageBox.Show("修改失败!\r\n错误："+err);
                        button1.Enabled = true;
                        button1.Text = "提交";
                    });
                };
                RunAsync(() =>
                {
                    wxagame_getuserinfo();
                });

            }
            else
            {
                MessageBox.Show("\"session_id\" 不能为空!");
            }
        }

        private void run()
        {

            Post("wxagame_getuserinfo", base_req);
            var s1 = Post("wxagame_getfriendsscore", base_req);
            if (!string.IsNullOrEmpty(s1))
            {
                var resultJS = ReadToObject(s1);
                if (resultJS.base_resp.errcode == 0)
                {
                    times = resultJS.my_user_info.times + 1;
                    var s2 = Post("wxagame_init", base_req);
                    if (!string.IsNullOrEmpty(s2))
                    {
                        resultJS = ReadToObject(s2);
                        if (resultJS.base_resp.errcode == 0)
                        {
                            Thread.Sleep(playTimeSeconds);
                            var action_data = Datestr();
                            var postdate = string.Format("{{\"base_req\":{{\"session_id\":\"{0}\",\"fast\":1}},\"action_data\":\"{1}\"}}", session_id, action_data);
                            wxagame_settlement();
                        }
                    }

                }
            }
        }

        private void wxagame_getuserinfo()
        {
            var client = new RestClient(base_site + "wxagame_getuserinfo");
            client.UserAgent = USER_AGENT;
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("referer", referer);
            request.AddParameter("application/json", string.Format("{{\"base_req\":{{\"session_id\":\"{0}\",\"fast\":1}},\"version\":{1}}}", session_id, version), ParameterType.RequestBody);
            try
            {
                IRestResponse response = client.Execute(request);
                Debug.WriteLine(response.Content);
                wxagame_getfriendsscore();
            }
            catch (Exception)
            {
                throw;
            }
        }

      
        private void wxagame_getfriendsscore()
        {
            var client = new RestClient(base_site + "wxagame_getfriendsscore");
            client.UserAgent = USER_AGENT;
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("referer", referer);
            request.AddParameter("application/json", string.Format("{{\"base_req\":{{\"session_id\":\"{0}\",\"fast\":1}},\"version\":{1}}}", session_id, version), ParameterType.RequestBody);
            try
            {
                IRestResponse response = client.Execute(request);
                Debug.WriteLine(response.Content);
                var resultJS = ReadToObject(response.Content);
                if (resultJS.base_resp.errcode == 0)
                {
                    times = resultJS.my_user_info.times + 1;
                    wxagame_init();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        private string Post(string url, string content)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(base_site + url);
            req.Method = "POST";
            req.ContentType = "application/json";
            req.Referer = referer;
            req.UserAgent = USER_AGENT;

            using (var streamWriter = new StreamWriter(req.GetRequestStream()))
            {
                streamWriter.Write(content);
            }
            try
            {
                var httpResponse = (HttpWebResponse)req.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                return result;

            }
            catch (Exception)
            {
                return null;
                //throw;
            }
        }

        private void wxagame_init()
        {
            var client = new RestClient(base_site + "wxagame_init");
            client.UserAgent = USER_AGENT;
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("referer", referer);
            request.AddParameter("application/json", string.Format("{{\"base_req\":{{\"session_id\":\"{0}\",\"fast\":1}},\"version\":{1}}}", session_id, version), ParameterType.RequestBody);
            try
            {
                IRestResponse response = client.Execute(request);
                Debug.WriteLine(response.Content);
                var resultJS = ReadToObject(response.Content);
                if (resultJS.base_resp.errcode == 0)
                {
                    Thread.Sleep(playTimeSeconds);
                    wxagame_settlement();
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void wxagame_settlement()
        {
           var action_data = Datestr();
            var client = new RestClient(base_site + "wxagame_settlement");
            client.UserAgent = USER_AGENT;
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("referer", referer);
            request.AddParameter("application/json", string.Format("{{\"base_req\":{{\"session_id\":\"{0}\",\"fast\":1}},\"action_data\":\"{1}\"}}", session_id, action_data), ParameterType.RequestBody);

            IRestResponse response1 = client.Execute(request);
            IRestResponse response = client.Execute(request);
            Debug.WriteLine(response.Content);

            var resultJS = ReadToObject(response.Content);
            if (resultJS.base_resp.errcode== 0)
            {
                OnPostSuccess?.Invoke();
            }
            else
            {
                OnPostFail?.Invoke(response.Content);
            }
        }


        private string Datestr()
        {


            //mouseDownTime

                //lastSucceedTime

                //startTime
            ActionDate ad = new ActionDate();

            GameData gd = new GameData();
            ad.score = score;
            ad.times = times;

            var startTime = GetTimeStamp(DateTime.Now) - 1500000;

            gd.seed = GetTimeStamp(DateTime.Now);
            gd.action = new List<object>();
            gd.musicList = new List<bool>();
            gd.touchList = new List<object>();
            gd.steps = new List<IList<double>>();
            gd.timestamp = new List<long>();

            for (var i = 0; i < score; i++)
            {
                Random rd = new Random();
                var r = rd.NextDouble();

                gd.action.Add(new object[3] { Math.Round(r, 3), Math.Round(r * 2, 2), i / 5000 == 0 ? true : false });
                gd.musicList.Add(false);


                var touch_x = 250 - Math.Round(r * 10, 4);
                var touch_y = 650 - Math.Round(r * 10, 4);

                gd.touchList.Add(new object[2] { touch_x, touch_y });
                IList<double> touchMoveList = new List<double>();
                for (int l = 0; l < 5; l++)
                {
                    touchMoveList.Add(touch_x);
                    touchMoveList.Add(touch_y);
                }
                gd.steps.Add(touchMoveList);
                long newTime = startTime + (int)Math.Round(r * 2700);
                gd.timestamp.Add(newTime);
                startTime = newTime;
            }

            gd.version = 2;
            var s2 = WriteFromObject<GameData>(gd);
            ad.game_data = s2;
            var text = WriteFromObject<ActionDate>((Object)ad);
            var ActionData = AESEncrypt(text, session_id);

            return ActionData;
        }

        private string AESEncrypt(string text, string originKey)
           
        {
            var password = originKey.Substring(0, 16);
            var iv = originKey.Substring(0, 16);
            RijndaelManaged rijndaelCipher = new RijndaelManaged();

            rijndaelCipher.Mode = CipherMode.CBC;

            rijndaelCipher.Padding = PaddingMode.PKCS7;

            rijndaelCipher.KeySize = 128;

            rijndaelCipher.BlockSize = 128;

            byte[] pwdBytes = Encoding.UTF8.GetBytes(password);

            byte[] keyBytes = new byte[16];

            int len = pwdBytes.Length;

            if (len > keyBytes.Length) len = keyBytes.Length;

            Array.Copy(pwdBytes, keyBytes, len);


            rijndaelCipher.Key = keyBytes;


            byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
            byte[] IvBytes = new byte[16];


            len = ivBytes.Length;

            if (len > ivBytes.Length) len = ivBytes.Length;

            Array.Copy(ivBytes, IvBytes, len);

            rijndaelCipher.IV = IvBytes;
           ICryptoTransform transform = rijndaelCipher.CreateEncryptor();

            byte[] plainText = Encoding.UTF8.GetBytes(text);
            byte[] cipherBytes = transform.TransformFinalBlock(plainText, 0, plainText.Length);

            return Convert.ToBase64String(cipherBytes);

        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="text"></param>
        /// <param name="password"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string AESDecrypt(string text, string originKey)
        {
            var password = originKey.Substring(0, 16);
            RijndaelManaged rijndaelCipher = new RijndaelManaged();

            rijndaelCipher.Mode = CipherMode.CBC;

            rijndaelCipher.Padding = PaddingMode.PKCS7;

            rijndaelCipher.KeySize = 128;

            rijndaelCipher.BlockSize = 128;

            byte[] encryptedData = Convert.FromBase64String(text);

            byte[] pwdBytes = Encoding.UTF8.GetBytes(password);

            byte[] keyBytes = new byte[16];

            int len = pwdBytes.Length;

            if (len > keyBytes.Length) len = keyBytes.Length;

            Array.Copy(pwdBytes, keyBytes, len);

            rijndaelCipher.Key = keyBytes;

            byte[] ivBytes = keyBytes;
            rijndaelCipher.IV = ivBytes;

            ICryptoTransform transform = rijndaelCipher.CreateDecryptor();

            byte[] plainText = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);

            return Encoding.UTF8.GetString(plainText);

        }
        private string WriteFromObject<T>(Object ad)
        {
            MemoryStream ms = new MemoryStream();

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            ser.WriteObject(ms, ad);
            byte[] json = ms.ToArray();
            ms.Close();
            return Encoding.UTF8.GetString(json, 0, json.Length);

        }
        private Result ReadToObject(string json)
        {
            Result deserialized = new Result();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(deserialized.GetType());
            deserialized = ser.ReadObject(ms) as Result;
            ms.Close();
            return deserialized;
        }
        private double randomd()
        {
            Random rd = new Random();
            return  rd.NextDouble();
        }

        private long GetTimeStamp(DateTime dateTime)
        {
            DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return (dateTime.ToUniversalTime().Ticks - dt1970.Ticks) / 10000;
        }

        void RunAsync(Action action)
        {
            ((Action)(delegate () {
                action?.Invoke();
            })).BeginInvoke(null, null);
        }

        void RunInMainthread(Action action)
        {
            this.BeginInvoke((Action)(delegate () {
                action?.Invoke();
            }));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text = AESDecrypt(textBox1.Text, SessionId.Text);
        }
    }

    public class BaseResp
    {
        public int errcode { get; set; }
        public long ts { get; set; }
    }
    public class Result
    {
        public BaseResp base_resp { get; set; }
        public string version { get; set; }
        public MyUserInfo my_user_info { get; set; }
    }
    public class MyUserInfo
    {
        public string nickname { get; set; }
        public string headimg { get; set; }
        public IList<object> score_info { get; set; }
        public int history_best_score { get; set; }
        public int week_best_score { get; set; }
        public int grade { get; set; }
        public int times { get; set; }
        public IList<object> hongbao_list { get; set; }
    }



    [DataContract]
    public class GameData
    {

        [DataMember(Order = 0, IsRequired = true)]
        public long seed { get; set; }
        [DataMember(Order = 1, IsRequired = true)]
        public IList<object> action { get; set; }

        [DataMember(Order = 2, IsRequired = true)]
        public IList<bool> musicList { get; set; }
        [DataMember(Order = 3, IsRequired = true)]
       public IList<object> touchList { get; set; }
        [DataMember(Order = 4, IsRequired = true)]
        public IList<IList<double>> steps { get; set; }
        [DataMember(Order = 5, IsRequired = true)]
        public IList<long> timestamp { get; set; }
            
        [DataMember(Order = 6, IsRequired = true)]
        public int version { get; set; }
    }
    [DataContract]
    public class ActionDate
    {
        [DataMember(Order = 0, IsRequired = true)]
        public int score { get; set; }
        [DataMember(Order = 1, IsRequired = true)]
        public long times { get; set; }
        [DataMember(Order = 2, IsRequired = true)]
        public string game_data { get; set; }

    }
}
