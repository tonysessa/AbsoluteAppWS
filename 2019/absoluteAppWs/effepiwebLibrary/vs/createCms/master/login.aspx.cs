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
    public partial class login : System.Web.UI.Page
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

            }
            else
            {
                //
                //Response.Redirect(sCmsStartingpage + "main.aspx");
            }
            #endregion

            if (!Page.IsPostBack)
            {
                sRedirecturl = "" + Request["currentPage"];
                if (String.IsNullOrEmpty(sRedirecturl))
                    sRedirecturl = sCmsStartingpage + "main.aspx";

                sCmsNlsContextUid = "" + Request["currentCmsNlsContext"];
                if (!String.IsNullOrEmpty(sCmsNlsContextUid))
                    sCmsNlsContextUid = objLibCript.uDecode(sCmsNlsContextUid, objLibCript.getCryptKey(sCryptKey));

                
            }
        }
        protected void Button_Invia_Click(object sender, EventArgs e)
        {
            //
            String sUserName = TextBox_Login.Text;
            String sPassword = TextBox_Password.Text;

            //
            CmsUsersItemResponse myCmsUsersItemResponse = objCmsBoDataLibs.CmsUsersCheckLogin(sUserName, sPassword);
            if (myCmsUsersItemResponse.Success)
            {

                if (myCmsUsersItemResponse.item != null)
                {
                    CmsUsers myCmsUser = myCmsUsersItemResponse.item;
                    CmsUsers_AclListOptions rqCmsUsers_AclListOptions = new CmsUsers_AclListOptions();
                    rqCmsUsers_AclListOptions.statusFlag = (Int32)EnumCmsContent.Enabled;

                    //
                    if (myCmsUser.EnableCmsNlsContext.Count > 1)
                    {
                        //
                        currentCmsUserSession = new CmsUserSession(header, myCmsUser.EnableCmsNlsContext[0]);
                        currentCmsUserSession.currentEnableCmsNlsContext = myCmsUser.EnableCmsNlsContext.OrderBy(t => t.Description).ToList();
                        currentCmsUserSession.currentCmsRoles = myCmsUser.CmsRoles;
                        currentCmsUserSession.currentUid = myCmsUser.Uid;
                        currentCmsUserSession.currentName = myCmsUser.Title;
                        currentCmsUserSession.currentUsername = myCmsUser.Username;
                        currentCmsUserSession.currentPassword = myCmsUser.Password;
                        if (myCmsUser.DateLastLogin != null)
                            currentCmsUserSession.currentLastLogin = (DateTime)myCmsUser.DateLastLogin;
                        else
                            currentCmsUserSession.currentLastLogin = System.DateTime.Now;

                        //
                        Session["CmsUserSession"] = currentCmsUserSession;

                        //
                        header.cmsUserUid = currentCmsUserSession.currentUid;
                        header.cmsCmsNlsContext = currentCmsUserSession.currentCmsNlsContext;

                        //
                        myCmsUser.DateLastLogin = System.DateTime.Now;
                        myCmsUser.NumLogin = myCmsUser.NumLogin + 1;
                        CmsUsersItemResponse myCmsUserUpsertItemResponse = objCmsBoDataLibs.CmsUsersUpsert(header, myCmsUser);

                        //
                        if (myCmsUserUpsertItemResponse.Success)
                        {
                            //
                            Literal_SelectCountry.Text = currentCmsUserSession.GetGlobalLabel("Cms.Literal_SelectCountry", "Select Your Context");
                            //
                            Panel_SelectContext.Visible = true;
                            Panel_login.Visible = false;

                            //
                            Repeater_CmsNlsContext.DataSource = myCmsUser.EnableCmsNlsContext.ToList();
                            Repeater_CmsNlsContext.ItemDataBound += Repeater_CmsNlsContext_ItemDataBound;
                            Repeater_CmsNlsContext.DataBind();
                        }
                        else
                        {
                            //
                            Session["CmsUserSession"] = null;
                            Label_response.Text = currentCmsUserSession.GetGlobalLabel("Cms.loginError","Login Error");
                        }
                    }
                    else if (myCmsUser.EnableCmsNlsContext.Count.Equals(1))
                    {
                        //
                        currentCmsUserSession = new CmsUserSession(header, myCmsUser.EnableCmsNlsContext[0]);
                        currentCmsUserSession.currentEnableCmsNlsContext = myCmsUser.EnableCmsNlsContext.OrderBy(t => t.Description).ToList();
                        currentCmsUserSession.currentCmsRoles = myCmsUser.CmsRoles;
                        currentCmsUserSession.currentUid = myCmsUser.Uid;
                        currentCmsUserSession.currentName = myCmsUser.Title;
                        currentCmsUserSession.currentUsername = myCmsUser.Username;
                        currentCmsUserSession.currentPassword = myCmsUser.Password;
                        if (myCmsUser.DateLastLogin != null)
                            currentCmsUserSession.currentLastLogin = (DateTime)myCmsUser.DateLastLogin;
                        else
                            currentCmsUserSession.currentLastLogin = System.DateTime.Now;

                        //
                        Session["CmsUserSession"] = currentCmsUserSession;

                        //
                        header.cmsUserUid = currentCmsUserSession.currentUid;
                        header.cmsCmsNlsContext = currentCmsUserSession.currentCmsNlsContext;

                        //
                        myCmsUser.DateLastLogin = System.DateTime.Now;
                        myCmsUser.NumLogin = myCmsUser.NumLogin + 1;
                        CmsUsersItemResponse myCmsUserUpsertItemResponse = objCmsBoDataLibs.CmsUsersUpsert(header, myCmsUser);

                        //
                        if (myCmsUserUpsertItemResponse.Success)
                        {
                            //
                            Response.Redirect(sRedirecturl);
                        }
                        else
                        {
                            //
                            Session["CmsUserSession"] = null;
                            Label_response.Text = currentCmsUserSession.GetGlobalLabel("Cms.loginError", "Login Error");
                        }
                    }
                    else
                    {
                        //
                        Session["CmsUserSession"] = null;
                        if (currentCmsUserSession != null)
                            Label_response.Text = currentCmsUserSession.GetGlobalLabel("Cms.loginNoContext", "No Context");
                    }
                }
                else
                {
                    //
                    Session["CmsUserSession"] = null;
                    if (currentCmsUserSession != null)
                        Label_response.Text = currentCmsUserSession.GetGlobalLabel("Cms.loginNoContext", "No Context");
                }
            }
            else
            {
                //
                Session["CmsUserSession"] = null;
                if (currentCmsUserSession != null)
                    Label_response.Text = currentCmsUserSession.GetGlobalLabel("Cms.loginNoContext", "No user found");
            }
        }



        private void Repeater_CmsNlsContext_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            //            
            RepeaterItem item = e.Item;

            if (e.Item.ItemType == ListItemType.Item | e.Item.ItemType == ListItemType.AlternatingItem)
            {
                String sUid = ((CmsNlsContext)e.Item.DataItem).Uid.ToString();
                String sDescription = ((CmsNlsContext)e.Item.DataItem).Description;

                HyperLink myHyperLink_CmsNlsContext_Title = (HyperLink)e.Item.FindControl("HyperLink_CmsNlsContext_Title");
                myHyperLink_CmsNlsContext_Title.NavigateUrl = sCmsStartingpage + "main.aspx?currentCmsNlsContext=" + sUid.ToString();
                myHyperLink_CmsNlsContext_Title.Text = sDescription;
            }
        }

        #endregion
    }
}

