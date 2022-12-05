using System;
using System.Data;
using System.Web.UI;
using System.Linq;
// Support
using Support.CmsFunction;
using Support.Web;
// 
using dataLibs;
using DbModel;

namespace backOffice.uc
{
    public partial class toolbar : System.Web.UI.UserControl
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
        #endregion        

        #region Property
        public CmsUserSession currentCmsUserSession
        {
            get
            {
                if (ViewState["_CURRENTCMSSESSIONOBJECT"] == null)
                    return null;
                else
                    return (CmsUserSession)ViewState["_CURRENTCMSSESSIONOBJECT"];
            }
            set { ViewState["_CURRENTCMSSESSIONOBJECT"] = value; }
        }
        #endregion

        #region uc Method
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }
        #endregion        
    }
}