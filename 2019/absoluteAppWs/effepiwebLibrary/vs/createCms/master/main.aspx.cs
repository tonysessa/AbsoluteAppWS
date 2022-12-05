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
using System.Web.UI.WebControls;

namespace backOffice
{
    public partial class main : System.Web.UI.Page
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
        public String responseScript
        {
            get
            {
                if (ViewState["_responseScript"] == null)
                    return "";
                else
                    return (String)ViewState["_responseScript"];
            }
            set { ViewState["_responseScript"] = value; }
        }
        #endregion

        #region Page Method
        protected void Page_Load(object sender, EventArgs e)
        {
            # region Check Login
            if (Session["CmsUserSession"] == null)
                Response.Redirect(sCmsStartingpage + "login.aspx?currentPage=" + Server.UrlEncode(Request.Url.ToString()));
            else
                currentCmsUserSession = (CmsUserSession)Session["CmsUserSession"];
            #endregion

            #region Header
            header.cmsUserUid = currentCmsUserSession.currentUid;
            header.cmsCmsNlsContext = currentCmsUserSession.currentCmsNlsContext;
            #endregion


            if (!Page.IsPostBack)
            {
                String sCmsNlsContextUid = "" + Request["currentCmsNlsContext"];
                if (objLibMath.isNumber(sCmsNlsContextUid))
                {
                    Int32 nCmsNlsContextUid = System.Convert.ToInt32(sCmsNlsContextUid);
                    currentCmsUserSession.currentCmsNlsContext = currentCmsUserSession.currentEnableCmsNlsContext.Where(t => t.Uid == nCmsNlsContextUid).FirstOrDefault();

                    header.cmsUserUid = currentCmsUserSession.currentUid;
                    header.cmsCmsNlsContext = currentCmsUserSession.currentCmsNlsContext;

                    //
                    currentCmsUserSession.Init(header, currentCmsUserSession.currentCmsNlsContext);

                    //
                    Session["CmsUserSession"] = currentCmsUserSession;
                }

                InitPage();
            }

        }
        #endregion

        #region Page Method
        protected void InitPage()
        {
            //
            if (!String.IsNullOrEmpty(currentCmsUserSession.currentName))
                Literal_Login.Text = currentCmsUserSession.currentName;
            else
                Literal_Login.Text = currentCmsUserSession.currentName;

            // Uc
            sidebarMenu1.currentCmsUserSession = currentCmsUserSession;
            toolbar1.currentCmsUserSession = currentCmsUserSession;

            Literal_Welcome.Text = currentCmsUserSession.GetGlobalLabel("Cms.Literal_Welcome", "Welcome") + " ";
        }
        #endregion

        #region Add Method

        #endregion
    }
}
