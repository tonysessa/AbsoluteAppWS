// by wlf

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;
using System.Web.SessionState;

namespace Support.Web
{
    public class WebContext
    {

        public static string[] supportedLaguages = null;
        public static System.Collections.Hashtable supportedResources = null;

        HttpContext httpContext = null;

        protected WebContext(HttpContext httpContext)
        {
            this.httpContext = httpContext;
            // inizializza le risorse in modo statico se la prima volta che viene invocato il costruttore

            if (supportedResources == null)
            {
                supportedResources = new System.Collections.Hashtable();
                if (supportedLaguages != null)
                {
                    for (int i = 0; i < supportedLaguages.Length; i++)
                        supportedResources.Add(supportedLaguages[i], new NlsResorceManager(this, supportedLaguages[i]));
                }
            }
        }

        public HttpRequest Request
        {
            get
            {
                return httpContext.Request;
            }
        }

        public HttpResponse Response
        {
            get
            {
                return httpContext.Response;
            }
        }

        public HttpSessionState Session
        {
            get
            {
                return httpContext.Session;
            }
        }

        public HttpApplicationState Application
        {
            get
            {
                return httpContext.Application;
            }
        }

        public HttpContext getHttpContext()
        {
            return httpContext;
        }

        public Object getSessionObject(string varName)
        {
            return httpContext.Session[varName];
        }

        public void setSessionObject(string varName, Object obj)
        {
            httpContext.Session[varName] = obj;
        }

        public Object getApplicationObject(string varName)
        {
            return httpContext.Application[varName];
        }

        public void setApplicationObject(string varName, Object obj)
        {
            httpContext.Application[varName] = obj;
        }

        public string getLanguage()
        {
            String lng = (String)httpContext.Session["__language"];
            if (lng == null)
                lng = setLanguage(null);
            return lng;
        }

        public string setLanguage(string lng)
        {
            if (String.IsNullOrEmpty(lng))
                lng = supportedLaguages[0];
            if (Array.IndexOf(supportedLaguages, lng.ToUpper()) == -1)
                lng = supportedLaguages[0];
            httpContext.Session["__language"] = lng;
            return lng;
        }

        public void logoutCurrentUser()
        {
            httpContext.Session.Remove("__USR__");
        }

        public string getPublicFisicalPath()
        {
            return this.httpContext.Server.MapPath("~/public/");
        }

        public string getRepositoryFisicalPath()
        {
            return this.httpContext.Server.MapPath("~/xmlr");
        }

        public string getRepositoryMailFisicalPath()
        {
            return this.httpContext.Server.MapPath("~/xmlr/templateEmails/");
        }

        /*
         * restiyuisce una risorsa XML nazionalizzata in base alla lingua corrente
         * 
         */
        public System.IO.Stream getXMLResourceFromRepository(string resourcePath)
        {
            return getXMLResourceFromRepository(resourcePath, getLanguage());
        }

        /*
         * restiyuisce una risorsa XML nazionalizzata in base alla lingua passata
         * 
         */
        public System.IO.Stream getXMLResourceFromRepository(string resourcePath, string lng)
        {
            string path = Path.Combine(getRepositoryFisicalPath(), resourcePath);
            //string lng = getLanguage();
            if (File.Exists(path + "_" + lng + ".xml"))
                return System.IO.File.OpenRead(path + "_" + lng + ".xml");
            if (File.Exists(path + ".xml"))
                return System.IO.File.OpenRead(path + ".xml");
            //
            return null;
        }

        /*
         * restiyuisce una risorsa XML nazionalizzata in base alla lingua corrente
         * 
         */
        public System.IO.Stream getSELMXResourceFromRepository(string resourcePath)
        {
            return getSELMXResourceFromRepository(resourcePath, getLanguage());
        }

        /*
         * restiyuisce una risorsa XML nazionalizzata in base alla lingua passata
         * 
         */
        public System.IO.Stream getSELMXResourceFromRepository(string resourcePath, string lng)
        {
            string path = Path.Combine(getRepositoryMailFisicalPath(), resourcePath);
            //
            if (File.Exists(path + "_" + lng + ".selmx"))
                return System.IO.File.OpenRead(path + "_" + lng + ".selmx");
            if (File.Exists(path + ".selmx"))
                return System.IO.File.OpenRead(path + ".selmx");
            //
            return null;
        }

        public void initPageCulture(System.Web.UI.Page page)
        {
            if (page != null)
                page.UICulture = getLanguage().ToLower();
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(getLanguage().ToLower());
        }

        public NlsResorceManager nls
        {
            get
            {
                return (NlsResorceManager)supportedResources[getLanguage()];
            }
        }


        public string getRequestParam(string requestVar)
        {
            string ret = httpContext.Request[requestVar];
            return ret;
        }

        public string getRequestParam(string requestVar, string devValue)
        {
            string ret = getRequestParam(requestVar);
            if (string.IsNullOrEmpty(ret))
                ret = devValue;
            return ret;
        }

        public void Redirect(string url)
        {
            httpContext.Response.Redirect(url);
        }

        /**
         Costruisce una url relativa: 
         * per esempio
         * xmlr/a.xml -> /testApp/xmlr/a.xml
         */
        public string ResolveUrl(string path)
        {
            string s = this.httpContext.Request.ApplicationPath;
            if (path.StartsWith("/") && s.EndsWith("/"))
                s += path.Substring(1);
            else
                if (path.StartsWith("/") || s.EndsWith("/"))
                    s += path;
                else
                    s += "/" + path;
            //
            s = s.Replace("/~/", "/");
            //
            return s;
        }

        /**
         Restituisce un path assoluta:
         * per esempio
         * xmlr/a.xml -> http://www.xxx.it/testApp/xmlr/a.xml
         */
        public string ResolveUrlAsAbsolute(string path)
        {
            string url = ResolveUrl(path);
            string Port = this.httpContext.Request.ServerVariables["SERVER_PORT"];
            string serverName = this.httpContext.Request.ServerVariables["SERVER_NAME"];
            //
            string ret = "";
            //
            if (string.IsNullOrEmpty(Port))
                Port = "80";
            ret = "http://" + serverName;
            if (!Port.Equals("80"))
                ret += ":" + Port;
            ret += url;
            //
            return ret;
        }

        public string ResolveHttpsUrlAsAbsolute(string path)
        {
            string HTTPSENABLED = WebContext.getConfig("HTTPSENABLED");
            //
            string url = ResolveUrl(path);
            string Port = this.httpContext.Request.ServerVariables["SERVER_PORT"];
            string serverName = this.httpContext.Request.ServerVariables["SERVER_NAME"];
            //
            string ret = "";
            if (HTTPSENABLED.Equals("true"))
            {
                ret = "https://" + serverName + url;
            }
            else
            {
                if (string.IsNullOrEmpty(Port))
                    Port = "80";
                ret = "http://" + serverName;
                if (!Port.Equals("80"))
                    ret += ":" + Port;
                ret += url;
            }
            //
            return ret;
        }

        public String getHomeUrlAsAbsolute()
        {
            return ResolveUrlAsAbsolute("");
        }

        /// <summary>
        ///  Factory
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>

        public static WebContext getContext(HttpContext httpContext)
        {
            WebContext wctx = new WebContext(httpContext);
            return wctx;
        }

        public static WebContext getContext(System.Web.UI.Page page, HttpContext httpContext)
        {
            WebContext wctx = new WebContext(httpContext);
            //
            // ********************************************************
            // ********************************************************
            // setto la cultura di questa pagina e becco nls

            // ********************************************************
            string clng = wctx.getRequestParam("language", null);
            if (clng != null)
                wctx.setLanguage(clng);
            //
            wctx.initPageCulture(page);
            //
            return wctx;
        }




        public static String getConfig(String config)
        {
            if (config.StartsWith("%."))
            {
                String deploy = System.Configuration.ConfigurationManager.AppSettings["deploy"];
                if (String.IsNullOrEmpty(deploy))
                    deploy = "default";
                //
                config = deploy + "." + config.Substring(2);
            }
            if (System.Configuration.ConfigurationManager.AppSettings[config] != null)
                return System.Configuration.ConfigurationManager.AppSettings[config];
            else
                return "";
        }

        public static String getConfig(String config, String def)
        {
            String ret = getConfig(config);
            if (String.IsNullOrEmpty(ret))
                ret = def;
            return def;
        }

    }
}
