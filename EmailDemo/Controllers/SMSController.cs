using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EmailDemo.Controllers
{
    /// <summary>
    /// 短信发送
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SMSController : ControllerBase
    {

        private string THE_UID = "xiaotangmu"; //用户名
        private string THE_KEY = "d41d8cd98f00b204e980"; //接口秘钥

        public SMSController() { }

        /// <summary>返回UTF-8编码发送接口地址</summary>
        /// <param name="receivePhoneNumber">目的手机号码（多个手机号请用半角逗号隔开）</param>
        /// <param name="receiveSms">短信内容，最多支持400个字，普通短信70个字/条，长短信64个字/条计费</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SendMessage(string smsMob, string smsText)
        {
            string postUrl = "http://utf8.api.smschinese.cn/?Uid=" + THE_UID + "&key=" + THE_KEY + "&smsMob=" + smsMob + "&smsText=" + smsText;
            //调用时只需要把拼成的URL传给该函数即可。判断返回值即可
            string strRet = null;

            if (postUrl == null || postUrl.Trim().ToString() == "")
            {
                return new JsonResult(new
                {
                    message = GetResult(strRet),
                    code = 1001,
                    success = false
                });
            }
            string targeturl = postUrl.Trim().ToString();
            try
            {
                HttpWebRequest hr = (HttpWebRequest)WebRequest.Create(targeturl);
                hr.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)";
                hr.Method = "GET";
                hr.Timeout = 30 * 60 * 1000;
                WebResponse hs = hr.GetResponse();
                Stream sr = hs.GetResponseStream();
                StreamReader ser = new StreamReader(sr, Encoding.Default);
                strRet = ser.ReadToEnd();
            }
            catch (Exception ex)
            {
                strRet = null;
            }
            return new JsonResult(new
            {
                message = GetResult(strRet),
                code = 200,
                success = true
            });

        }
        /// <summary>
        /// 确认返回信息 
        /// </summary>
        /// <param name="strRet"></param>
        /// <returns></returns>
        public string GetResult(string strRet)
        {
            int result = 0;
            try
            {
                result = int.Parse(strRet);
                switch (result)
                {
                    case -1:
                        strRet = "没有该用户账户";
                        break;
                    case -2:
                        strRet = "接口密钥不正确,不是账户登陆密码";
                        break;
                    case -21:
                        strRet = "MD5接口密钥加密不正确";
                        break;
                    case -3:
                        strRet = "短信数量不足";
                        break;
                    case -11:
                        strRet = "该用户被禁用";
                        break;
                    case -14:
                        strRet = "短信内容出现非法字符";
                        break;
                    case -4:
                        strRet = "手机号格式不正确";
                        break;
                    case -41:
                        strRet = "手机号码为空";
                        break;
                    case -42:
                        strRet = "短信内容为空";
                        break;
                    case -51:
                        strRet = "短信签名格式不正确,接口签名格式为：【签名内容】";
                        break;
                    case -6:
                        strRet = "IP限制";
                        break;
                    default:
                        strRet = "发送短信数量：" + result;
                        break;
                }
            }
            catch (Exception ex)
            {
                strRet = ex.Message;
            }
            return strRet;
        }
    }
}
