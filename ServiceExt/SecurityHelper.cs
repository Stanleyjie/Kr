using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ServiceExt
{
    public class SecurityHelper
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToMD5(string str)
        {
            var encode = Encoding.UTF8;

            if (string.IsNullOrWhiteSpace(str)) return "";

            using (var md5 = MD5.Create())
            {
                var bytes = md5.ComputeHash(encode.GetBytes(str));
                var sb = new StringBuilder();
                foreach (var i in bytes)
                {
                    sb.Append(i.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// SHA1 加密，返回大写字符串
        /// </summary>
        /// <param name="content">需要加密字符串</param>
        /// <param name="encode">指定加密编码</param>
        /// <returns>返回40位大写字符串</returns>
        public static string SHA1(string content, Encoding encode)
        {
            try
            {
                SHA1 sha1 = new SHA1CryptoServiceProvider();
                byte[] bytes_in = encode.GetBytes(content);
                byte[] bytes_out = sha1.ComputeHash(bytes_in);
                sha1.Dispose();
                string result = BitConverter.ToString(bytes_out);
                result = result.Replace("-", "");
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("SHA1加密出错：" + ex.Message);
            }
        }

        /// <summary>
        /// AES加密 - CBC - PKCS7
        /// </summary>
        /// <param name="content"></param>
        /// <param name="aesKey"></param>
        /// <returns></returns>
        public static string AESEncrypt(string content, string aesKey)
        {
 

            if (string.IsNullOrWhiteSpace(content)) return "";

            string aesIV = "ABCDEF1234567890";

            byte[] byteKEY = Encoding.UTF8.GetBytes(aesKey);
            byte[] byteIV = Encoding.UTF8.GetBytes(aesIV);

            byte[] byteContnet = Encoding.UTF8.GetBytes(content);

            var _aes = new RijndaelManaged();
            _aes.Padding = PaddingMode.PKCS7;
            _aes.Mode = CipherMode.CBC;

            _aes.Key = byteKEY;
            _aes.IV = byteIV;

            var _crypto = _aes.CreateEncryptor(byteKEY, byteIV);
            byte[] decrypted = _crypto.TransformFinalBlock(
                byteContnet, 0, byteContnet.Length);

            _crypto.Dispose();

            return Convert.ToBase64String(decrypted);
        }

        /// <summary>
        /// AES解密- CBC - PKCS7
        /// </summary>
        /// <param name="toDecrypt"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string AESDecrypt(string decryptStr, string aesKey)
        {
            string aesIV = "ABCDEF1234567890";
            if (string.IsNullOrWhiteSpace(decryptStr)) return "";

            byte[] byteKEY = Encoding.UTF8.GetBytes(aesKey);
            byte[] byteIV = Encoding.UTF8.GetBytes(aesIV);

            byte[] byteDecrypt = System.Convert.FromBase64String(decryptStr);

            var _aes = new RijndaelManaged();
            _aes.Padding = PaddingMode.PKCS7;
            _aes.Mode = CipherMode.CBC;

            _aes.Key = byteKEY;
            _aes.IV = byteIV;

            var _crypto = _aes.CreateDecryptor(byteKEY, byteIV);
            byte[] decrypted = _crypto.TransformFinalBlock(
                byteDecrypt, 0, byteDecrypt.Length);

            _crypto.Dispose();

            return Encoding.UTF8.GetString(decrypted);

 
        }

        /// <summary>
        /// 随机生成16位AESkey
        /// </summary>
        /// <returns></returns>
        public static string GeyRandomAESKey()
        {
            string str = string.Empty;
            Random rnd1 = new Random();
            int r = rnd1.Next(10, 100);
            long num2 = DateTime.Now.Ticks + r;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> r)));
            for (int i = 0; i < 16; i++)
            {
                char ch;
                int num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;
        }

        public static string GetMD5HashFromFile(Stream stream)
        {
            try
            {
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(stream);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }
    }
}
