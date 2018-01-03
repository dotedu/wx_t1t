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

namespace wx_t1t
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string base_site = "https://mp.weixin.qq.com/wxagame/";

        string referer = "https://servicewechat.com/wx7c8d593b2c3a7703/{0}/page-frame.html";

        string USER_AGENT = "MicroMessenger/6.6.1.1200(0x26060130) NetType/4G Language/zh_CN";
        long times;
        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text)&& !string.IsNullOrEmpty(textBox3.Text))
            {
                var session_id = textBox1.Text;
                var version = numericUpDown1.Value.ToString();
                var score = textBox3.Text;
                wxagame_init(session_id, version);
                wxagame_getuserinfo(session_id, version);
                wxagame_init(session_id, version);
                wxagame_init(session_id, version);

            }
            else
            {
                MessageBox.Show("“session_id”与 “得分”不能为空");
            }
        }


        private void wxagame_init(string session_id, string version)
        {
            var client = new RestClient(base_site+"wxagame_init");
            client.UserAgent = USER_AGENT;
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("referer", string.Format(referer, version));
            request.AddParameter("application/json", string.Format("{{\"base_req\":{{\"session_id\":\"{0}\",\"fast\":1}},\"version\":{1}}}", session_id, version), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
        }

        private void wxagame_getuserinfo(string session_id, string version)
        {
            var client = new RestClient(base_site + "wxagame_getuserinfo");
            client.UserAgent = USER_AGENT;
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("referer", string.Format(referer, version));
            request.AddParameter("application/json", string.Format("{{\"base_req\":{{\"session_id\":\"{0}\",\"fast\":1}},\"version\":{1}}}", session_id, version), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            //var resultJS = JsonConvert.DeserializeObject<Result>(response.Content);
            var resultJS = ReadToObject(response.Content);
            times = resultJS.base_resp.ts + 1000;
        }

        private void wxagame_getfriendsscore(string session_id, string version)
        {
            var client = new RestClient(base_site + "wxagame_getfriendsscore");
            client.UserAgent = USER_AGENT;
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("referer", string.Format(referer, version));
            request.AddParameter("application/json", string.Format("{{\"base_req\":{{\"session_id\":\"{0}\",\"fast\":1}},\"version\":{1}}}", session_id, version), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
        }





    private void wxagame_settlement(string session_id, string version,string score)
        {
            var client = new RestClient(base_site + "wxagame_getfriendsscore");
            client.UserAgent = USER_AGENT;
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("referer", string.Format(referer, version));
            request.AddParameter("application/json", string.Format("{{\"base_req\":{{\"session_id\":\"{0}\",\"fast\":1}},\"action_data\":{1}}}", session_id, version), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
        }

        //private string action_data()
        //{

        //}

        private string encrypt(string text, string originKey)
        {

            
            originKey = originKey.Substring(0, 16);

            var ciphertext ="";
            return ciphertext;
        }

        public string AESEncrypt(string text, string originKey)
           
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
 

        public string WriteFromObject<T>(Object ad)
        {
            MemoryStream ms = new MemoryStream();

            // Serializer the User object to the stream.  
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            ser.WriteObject(ms, ad);
            byte[] json = ms.ToArray();
            ms.Close();
            return Encoding.UTF8.GetString(json, 0, json.Length);

        }
        public Result ReadToObject(string json)
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

        private void button2_Click(object sender, EventArgs e)
        {
            var s = "{\"base_resp\":{\"errcode\":0,\"ts\":\"1514948042711\"},\"nickname\":\"鱼跃\",\"headimg\":\"http\"}";
            var O = ReadToObject(s);

            ActionDate ad = new ActionDate();

            GameData gd = new GameData();
            ad.score = 99;
            ad.times = 666;

            gd.seed = 1514965815335;
            gd.action = new List<object>();
            gd.musicList = new List<bool>();
            gd.touchList = new List<object>();

            for (var i = Math.Round(12000 + randomd() * 2000); i > 0; i--)
            {
                gd.action.Add(new object[3] { Math.Round(randomd(), 3), Math.Round(randomd() * 2, 2), i / 5000 == 0 ? true : false });
            gd.musicList.Add(false);
            gd.touchList.Add(new object[2] { Math.Round(250 - randomd() * 10, 4), Math.Round(650 - randomd() * 10 * 2, 4) });
        }

            gd.version = 1;
            var s2= WriteFromObject<GameData>(gd);
            ad.game_data = s2;

            //textBox4.Text = WriteFromObject<ActionDate>((Object)ad);

            var ss = AESEncrypt(WriteFromObject<ActionDate>((Object)ad), textBox1.Text);
        }

        private long GetTimeStamp(DateTime dateTime)
        {
            DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return (dateTime.ToUniversalTime().Ticks - dt1970.Ticks) / 10000;
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
        public string nickname { get; set; }
        public string headimg { get; set; }
        public string version { get; set; }
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
