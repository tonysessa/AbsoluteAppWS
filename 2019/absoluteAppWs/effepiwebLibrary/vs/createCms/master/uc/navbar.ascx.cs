using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Linq;
using System.Runtime.Serialization;
// Support
using Support.db;
using Support.CmsFunction;
using Support.Library;
using Support.Web;
//
using DbModel;
using dataLibs;
using System.Collections.Generic;

namespace backOffice.uc
{
    public partial class navbar : System.Web.UI.UserControl
    {
        #region objUtil
        protected Support.Library.DateUtil objLibDate = new Support.Library.DateUtil();
        protected Support.Library.DbUtil objLibDB = new Support.Library.DbUtil();
        protected Support.Library.StringUtil objLibString = new Support.Library.StringUtil();
        protected Support.Library.StringSqlUtil objLibSqlString = new Support.Library.StringSqlUtil();
        protected Support.Library.MathUtil objLibMath = new Support.Library.MathUtil();
        protected Support.Library.CriptUtil objLibCript = new Support.Library.CriptUtil();
        //
        protected CmsFunction objCmsFunction = new CmsFunction();
        protected CmsBoDataLibs objCmsBoDataLibs = new CmsBoDataLibs();
        #endregion

        #region Parametri
        public String sStartingpage = WebContext.getConfig("%.startingpage").ToString();
        public String sCmsStartingpage = WebContext.getConfig("%.cmsStartingpage").ToString();
        public String sCryptKey = WebContext.getConfig("%.cryptKey").ToString();
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
        public String currentCmsSubSections
        {
            get
            {
                if (ViewState["_currentCmsSubSections"] == null)
                    return "";
                else
                    return (String)ViewState["_currentCmsSubSections"];
            }
            set { ViewState["_currentCmsSubSections"] = value; }
        }
        public String currentCmsSubSections_SectionUri
        {
            get
            {
                if (ViewState["_currentCmsSubSections_SectionUri"] == null)
                    return "";
                else
                    return (String)ViewState["_currentCmsSubSections_SectionUri"];
            }
            set { ViewState["_currentCmsSubSections_SectionUri"] = value; }
        }
        public String sJsCurrentContentTable
        {
            get
            {
                if (ViewState["_sJsCurrentContentTable"] == null)
                    return "";
                else
                    return (String)ViewState["_sJsCurrentContentTable"];
            }
            set { ViewState["_sJsCurrentContentTable"] = value; }
        }
        #endregion

        #region uc Method
        protected void Page_Load(object sender, EventArgs e)
        {
            # region Check Login
            if (Session["CmsUserSession"] == null)
                Response.Redirect(sCmsStartingpage + "login.aspx?currentPage=" + Server.UrlEncode(Request.Url.ToString()));
            else
                currentCmsUserSession = (CmsUserSession)Session["CmsUserSession"];
            #endregion

            //
            InitUC();

        }
        protected void InitUC()
        {
            Literal_Title.Text = currentCmsUserSession.currentName;
            Literal_Username.Text = currentCmsUserSession.currentUsername;

            //
            Literal_CurrentContext_Title.Text = currentCmsUserSession.currentCmsNlsContext.Title;

            //
            Repeater_CmsNlsContext_Bind();
        }

        protected void LinkButton_Logout_Click(object sender, EventArgs e)
        {

            //
            Session["CmsUserSession"] = null;

            //
            Response.Redirect(sCmsStartingpage + "login.aspx?currentCmsNlsContext=" + objLibCript.uEncode(currentCmsUserSession.currentCmsNlsContext.Uid.ToString(), objLibCript.getCryptKey(sCryptKey)));
        }

        protected void Repeater_CmsNlsContext_Bind()
        {
            Literal_NumContext.Text = currentCmsUserSession.currentEnableCmsNlsContext.Count().ToString();
            Repeater_CmsNlsContext.DataSource = currentCmsUserSession.currentEnableCmsNlsContext;
            Repeater_CmsNlsContext.ItemDataBound += new RepeaterItemEventHandler(Repeater_CmsNlsContext_ItemDataBound);
            Repeater_CmsNlsContext.DataBind();
        }
        protected void Repeater_CmsNlsContext_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
            {
                CmsNlsContext myCmsNlsContext = (CmsNlsContext)e.Item.DataItem;

                String Uid = myCmsNlsContext.Uid.ToString();
                String sTitle = myCmsNlsContext.Title;

                Literal myLiteral_CmsNlsContext_Title = (Literal)e.Item.FindControl("Literal_CmsNlsContext_Title");
                myLiteral_CmsNlsContext_Title.Text = sTitle;

                HyperLink myHyperLink_CmsNlsContext_Title = (HyperLink)e.Item.FindControl("HyperLink_CmsNlsContext_Title");
                myHyperLink_CmsNlsContext_Title.NavigateUrl = sCmsStartingpage + "main.aspx?currentCmsNlsContext=" + Uid.ToString();
                //myLinkButton_CmsNlsContext_Title.Text = "";
            }
        }

        #endregion       
    }
}