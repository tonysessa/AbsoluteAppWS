using System;
using System.Data;
using System.Data.Linq;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
// Support
using Support.db;
using Support.CmsFunction;
using Support.Library;
using Support.Web;
//
using dataLibs;
using DbModel;

namespace backOffice
{
    public partial class _default : System.Web.UI.Page
    {

        #region objUtil
        protected Support.Library.DateUtil objLibDate = new Support.Library.DateUtil();
        protected Support.Library.DbUtil objLibDB = new Support.Library.DbUtil();
        protected Support.Library.StringUtil objLibString = new Support.Library.StringUtil();
        protected Support.Library.StringSqlUtil objLibSqlString = new Support.Library.StringSqlUtil();
        protected Support.Library.MathUtil objLibMath = new Support.Library.MathUtil();
        protected Support.Library.CriptUtil objLibCript = new Support.Library.CriptUtil();
        protected Support.Library.ImageUtil objLibImage = new Support.Library.ImageUtil();
        protected Support.Library.FileUtil objFileUtil = new Support.Library.FileUtil();
        protected Support.Library.LogUtil objLogUtil = new Support.Library.LogUtil();
        //
        protected WebContext wctx = null;
        protected MyHeader header = new MyHeader();
        protected CmsFunction objCmsFunction = new CmsFunction();
        protected CmsBoDataLibs objCmsBoDataLibs = new CmsBoDataLibs();
        #endregion        

        #region Parametri
        public String sStartingpage = WebContext.getConfig("%.startingpage").ToString();
        public String sCmsStartingpage = WebContext.getConfig("%.cmsStartingpage").ToString();
        public String sCryptKey = WebContext.getConfig("%.cryptKey").ToString();
        public String sLogDir = WebContext.getConfig("%.LogDir").ToString();
        public String sLogFile = WebContext.getConfig("%.LogFile").ToString();
        public String sCmsNlsContextUid = String.Empty;
        public String sStoragePublicBaseUrl = WebContext.getConfig("%.storagePublicBaseUrl").ToString();
        #endregion        

        #region Property
        public CmsUserSession currentCmsUserSession
        {
            get
            {
                if (ViewState["_CmsUserSession"] == null)
                    return null;
                else
                    return (CmsUserSession)ViewState["_CmsUserSession"];
            }
            set { ViewState["_CmsUserSession"] = value; }
        }
        public String sRedirecturl
        {
            get
            {
                if (ViewState["_Redirecturl"] == null)
                    return "";
                else
                    return (String)ViewState["_Redirecturl"];
            }
            set { ViewState["_Redirecturl"] = value; }
        }
        #endregion

        #region Page Method
        protected void Page_Load(object sender, EventArgs e)
        {
            # region Check Login
            if (Session["CmsUserSession"] == null)
            {
                //
                Response.Redirect(sCmsStartingpage + "login.aspx");
            }
            else
            {
                //
                Response.Redirect(sCmsStartingpage + "main.aspx");
            }
            #endregion            
        }
        #endregion
    }
}

