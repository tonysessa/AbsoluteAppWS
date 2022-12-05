using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
//
using Support.db;
using Support.Web;
using System.Security.Cryptography;
using System.Text;

namespace Support.Library
{
    public class CriptUtil
	{
        public CriptUtil() { }

        #region Cripting Function
        public String uEncode(String str, UInt16 k)
        {
            try
            {
                String ret = "";
                for (int i = 0; i < str.Length; i++)
                {
                    UInt16 c = (UInt16)(((UInt16)(str[i])) ^ k);
                    ret += c.ToString("X").PadLeft(4, '0');
                }
                return ret;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public String uDecode(String str, UInt16 k)
        {
            try
            {
                String ret = "";
                for (int i = 0; i < str.Length; i += 4)
                {
                    String s = str.Substring(i, 4);
                    UInt16 c = (UInt16)(UInt16.Parse(s, NumberStyles.HexNumber) ^ k);
                    ret += (char)c;
                }
                return ret;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public UInt16 getCryptKey(String sCryptKey)
        {
            UInt16 cryptKey = UInt16.Parse(sCryptKey);
            return cryptKey;
        }
        public String getNewUID()
        {
            String ret = "";
            int m_lastId = 1;
            int iRandomNumber = 0;
            long msec = 0;

            Random rand = new Random(System.DateTime.Now.Millisecond);
            iRandomNumber = rand.Next(1000000000);

            DateTime dt_startTime = new DateTime();
            DateTime dt_endTime = System.DateTime.Now;

            msec = dt_endTime.Subtract(dt_startTime).Milliseconds;

            m_lastId = (m_lastId + 1) % 9999999;

            ret += msec.ToString().PadLeft(10, '0');
            ret += m_lastId.ToString().PadLeft(10, '0');
            ret += iRandomNumber.ToString().PadLeft(10, '0');
            //
            return ret;
        }
        public String getNewUID(String prefix)
        {
            return prefix + getNewUID();
        }
        public String getRandomPasswordUsingGUID(int length)
        {
            // Get the GUID
            String guidResult = System.Guid.NewGuid().ToString();

            // Remove the hyphens
            guidResult = guidResult.Replace("-", String.Empty);

            // Make sure length is valid
            if (length <= 0 || length > guidResult.Length)
                throw new ArgumentException("Length must be between 1 and " + guidResult.Length);

            // Return the first length bytes
            return guidResult.Substring(0, length);
        }
        public string MD5(string str)
        {
            try
            {
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                byte[] h = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                //String s = BitConverter.ToString(h);
                return BitConverter.ToString(h).Replace("-", "").ToUpper();
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion
    }
}
