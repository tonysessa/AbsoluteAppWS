using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Net.Mime;
//
using Support.Web;
using Support.Library;

namespace Support.Mail
{
    public class MailSenderNet
    {
        private WebContext ctx = null;
        // smtp
        private String smtpServerHostName = "";
        private String smtpServerPort = "25";
        private String smtpNeedAutentication = "false";
        private String smtpUseSSL = "false";
        private String smtpUser = "";
        private String smtpPassword = "";
        // maggiordomo
        private String smtpMaggiordomo = "";
        // 
        public MailSenderNet(WebContext ctx)
        {
            this.ctx = ctx;
            //
            smtpServerHostName = WebContext.getConfig("%.smtpServerHostName").ToString();
            smtpServerPort = WebContext.getConfig("%.smtpServerPort").ToString();
            if (String.IsNullOrEmpty(smtpServerPort))
                smtpServerPort = "25";
            smtpNeedAutentication = WebContext.getConfig("%.smtpNeedAutentication").ToString();
            smtpUseSSL = WebContext.getConfig("%.smtpUseSSL").ToString();
            smtpUser = WebContext.getConfig("%.smtpUser").ToString();
            smtpPassword = WebContext.getConfig("%.smtpPassword").ToString();
            smtpMaggiordomo = WebContext.getConfig("%.smtpMaggiordomo").ToString();
        }

        public bool sendHtmlMailFromUrl(String from,
                                   String to,
                                   String subject,
                                   String url)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            StreamReader responseStream = new StreamReader(res.GetResponseStream());
            String body = responseStream.ReadToEnd();
            responseStream.Close();
            res.Close();
            //
            return sendHtmlMail(from, to, subject, body);
        }

        public bool sendHtmlMail(   String from,
                                    String to,
                                    String subject,
                                    String body)
        {
            return sendHtmlMail(from, new String[] { to }, null,null,null,null,subject,body);
        }

        public bool sendHtmlMail(String from,
                                       String[] to,
                                       String[] cc,
                                       String[] bcc,
                                       String[] attachF,
                                       String[] attachU,
                                       String subject,
                                       String body)
        {
            bool bRet = false;
            //
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                //
                smtpClient.Host = smtpServerHostName;
                smtpClient.Port = Convert.ToInt32(smtpServerPort);
                if (smtpNeedAutentication.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                {
                    smtpClient.Credentials = new System.Net.NetworkCredential(smtpUser, smtpPassword);
                    if (smtpUseSSL.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                        smtpClient.EnableSsl = true;
                    else
                        smtpClient.EnableSsl = false;

                }
                // 
                MailMessage message = new MailMessage();
                // from
                message.From = new MailAddress(from);
                // to
                if (to != null)
                    for (int i = 0; i < to.Length; i++)
                        if (!String.IsNullOrEmpty(to[i]))
                            message.To.Add(to[i]);
                // subject
                message.Subject = subject;
                // cc
                if (cc != null)
                    for (int i = 0; i < cc.Length; i++)
                        if (!String.IsNullOrEmpty(cc[i]))
                            message.CC.Add(cc[i]);
                // bcc
                if (bcc != null)
                    for (int i = 0; i < bcc.Length; i++)
                        if (!String.IsNullOrEmpty(bcc[i]))
                            message.Bcc.Add(bcc[i]);
                // attach
                if (attachF != null)
                {
                    for (int i = 0; i < attachF.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(attachF[i]))
                        {
                            Attachment data = new Attachment(attachF[i], MediaTypeNames.Application.Octet);
                            data.ContentDisposition.Inline = false;
                            data.ContentDisposition.DispositionType = DispositionTypeNames.Attachment;
                            message.Attachments.Add(data);
                        }
                    }
                }
                // attach
                if (attachU != null)
                {
                    for (int i = 0; i < attachU.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(attachU[i]))
                        {
                            Attachment data = new Attachment(attachU[i], MediaTypeNames.Application.Octet);
                            String cid = Path.GetFileName(attachU[i]);
                            //
                            data.ContentId = cid;
                            data.ContentDisposition.Inline = true;
                            data.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
                            // data.ContentType.MediaType = "image/jpg";
                            data.ContentType.Name = cid;
                            //
                            body = body.Replace("{{" + cid + "}}", "cid:" + cid);
                            //
                            message.Attachments.Add(data);
                        }
                    }
                }
                // body+encoding
                message.IsBodyHtml = true;
                message.SubjectEncoding = System.Text.Encoding.UTF8;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.Body = body;
                //
                smtpClient.Send(message);
                //
                bRet = true;
            }
            catch (Exception Ex)
            {
                LogUtil objLogUtil = new LogUtil();
                string sActiveLog = WebContext.getConfig("%.ActiveLog").ToString();
                if (sActiveLog.Equals("1"))
                {
                    string sLogDir = WebContext.getConfig("%.LogDir").ToString();
                    string sLogFile = WebContext.getConfig("%.LogFile").ToString();
                    string sErr = "Message " + Ex.Message;
                    objLogUtil.Log(sLogDir + sLogFile, sErr);
                    sErr = "InnerException " + Ex.InnerException.ToString();
                    objLogUtil.Log(sLogDir + sLogFile, sErr);
                    sErr = "Source " + Ex.Source.ToString();
                    objLogUtil.Log(sLogDir + sLogFile, sErr);
                }
            }
            //
            return bRet;
        }

        protected String replaceMacros(Hashtable fields, String str)
        {
            if (String.IsNullOrEmpty(str))
                return "";
            //
            String ret = str;
            //
            ICollection keys = fields.Keys;
            foreach (String k in keys)
            {
                String val = (String)(fields[k]);
                if (String.IsNullOrEmpty(val))
                    val = "";
                ret = ret.Replace("{{" + k + "}}", val);
            }
            //
            return ret;
        }

        /**
         * Da file
         * in fields mettere le macro senza {{..}}
         * alcune macro di default vengono settate:
         * HOME_URL
         * SMTP_HOST            (da web.config smtpServerHostName)
         * SMTP_MAGGIORDOMO     (da web.config smtpMaggiordomo)
         * */
        public bool sendHtmlMail(String selmx, Hashtable fields)
        {
            return sendHtmlMail(selmx, ctx.getLanguage(), fields);
        }

        public bool sendHtmlMail(String selmx, String lng, Hashtable fields)
        {
            if (fields == null)
                fields = new Hashtable();
            //
            // variabili di default
            if (fields["HOME_URL"] == null)
                fields["HOME_URL"] = ctx.getHomeUrlAsAbsolute();
            if (fields["SMTP_HOST"] == null)
                fields["SMTP_HOST"] = smtpServerHostName;
            if (fields["SMTP_MAGGIORDOMO"] == null)
                fields["SMTP_MAGGIORDOMO"] = smtpMaggiordomo;
            //
            ODATA.Mail.mail res = ODATA.Mail.mail.deserializeFromXML(ctx.getSELMXResourceFromRepository(selmx, lng));
            if (res == null)
                return false;
            //
            String from = replaceMacros(fields,res._FROM._txt.Trim());
            List<String> to         = new List<String>();
            List<String> cc         = new List<String>();
            List<String> bcc        = new List<String>();
            List<String> attachF    = new List<String>();
            List<String> attachU    = new List<String>(); 
            String subject  = replaceMacros(fields,res._SUBJECT._txt.Trim());
            String body     = replaceMacros(fields,res._BODY._txt.Trim());
            //
            if (res._TO!=null)
                for (int i = 0; i < res._TO.Length; i++)
                    to.Add(replaceMacros(fields,res._TO[i]._txt.Trim()));
            if (res._CC!=null)
                for (int i = 0; i < res._CC.Length; i++)
                    cc.Add(replaceMacros(fields,res._CC[i]._txt.Trim()));
            if (res._BCC!=null)
                for (int i = 0; i < res._BCC.Length; i++)
                    bcc.Add(replaceMacros(fields,res._BCC[i]._txt.Trim()));
            if (res._ATTACHF!=null)
                for (int i = 0; i < res._ATTACHF.Length; i++)
                    attachF.Add(replaceMacros(fields,res._ATTACHF[i]._txt.Trim()));
            if (res._ATTACHU!=null)
                for (int i = 0; i < res._ATTACHU.Length; i++)
                    attachU.Add(replaceMacros(fields,res._ATTACHU[i]._txt.Trim()));
            //
            return sendHtmlMail(   from, 
                                   to.ToArray(), 
                                   cc.ToArray(), 
                                   bcc.ToArray(), 
                                   attachF.ToArray(), 
                                   attachU.ToArray(), 
                                   subject, 
                                   body );
        }
    }
}
