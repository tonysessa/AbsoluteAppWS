using System;
using System.Data;
using System.Data.Linq;
using System.Configuration;
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
using System.Xml;
using System.IO;
// Support
using Support.db;
using Support.Library;
using Support.CmsFunction;
using Support.Web;
//
using DbModel;
using dataLibs;
namespace backOffice
{
    public partial class user_settings : System.Web.UI.Page
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
        public String sCmsStartingpage = WebContext.getConfig("%.cmsStartingpage").ToString();
        public String sStartingpage = WebContext.getConfig("%.Startingpage").ToString();
        public String sStoragePublicBaseUrl = WebContext.getConfig("%.storagePublicBaseUrl").ToString();
        public String sUploadDir = "courses/";
        public String sStoragePublicBasePath = WebContext.getConfig("%.storagePublicBasePath").ToString();
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
        //
        public String Uid
        {
            get
            {
                if (ViewState["_Uid"] == null)
                    return "";
                else
                    return (String)ViewState["_Uid"];
            }
            set { ViewState["_Uid"] = value; }
        }
        public String UidCopy
        {
            get
            {
                if (ViewState["_UidCopy"] == null)
                    return "";
                else
                    return (String)ViewState["_UidCopy"];
            }
            set { ViewState["_UidCopy"] = value; }
        }
        //
        public String section
        {
            get
            {
                if (ViewState["_SECTION"] == null)
                    return "";
                else
                    return (String)ViewState["_SECTION"];
            }
            set { ViewState["_SECTION"] = value; }
        }
        public String listPageSize
        {
            get
            {
                if (ViewState["_LISTPAGESIZE"] == null)
                    return "";
                else
                    return (String)ViewState["_LISTPAGESIZE"];
            }
            set { ViewState["_LISTPAGESIZE"] = value; }
        }
        public String listPageNumber
        {
            get
            {
                if (ViewState["_LISTPAGENUMBER"] == null)
                    return "";
                else
                    return (String)ViewState["_LISTPAGENUMBER"];
            }
            set { ViewState["_LISTPAGENUMBER"] = value; }
        }
        public String listOrderFied
        {
            get
            {
                if (ViewState["_LISTORDERFIED"] == null)
                    return "";
                else
                    return (String)ViewState["_LISTORDERFIED"];
            }
            set { ViewState["_LISTORDERFIED"] = value; }
        }
        public String listOrderAscendig
        {
            get
            {
                if (ViewState["_LISTORDERASCENDIG"] == null)
                    return "";
                else
                    return (String)ViewState["_LISTORDERASCENDIG"];
            }
            set { ViewState["_LISTORDERASCENDIG"] = value; }
        }
        //
        public String searchText
        {
            get
            {
                if (ViewState["_SEARCHTEXT"] == null)
                    return "";
                else
                    return (String)ViewState["_SEARCHTEXT"];
            }
            set { ViewState["_SEARCHTEXT"] = value; }
        }
        public String searchUid
        {
            get
            {
                if (ViewState["_searchUid"] == null)
                    return "";
                else
                    return (String)ViewState["_searchUid"];
            }
            set { ViewState["_searchUid"] = value; }
        }
        public String showDeleted
        {
            get
            {
                if (ViewState["_showDeleted"] == null)
                    return "";
                else
                    return (String)ViewState["_showDeleted"];
            }
            set { ViewState["_showDeleted"] = value; }
        }
        public String UidRelated
        {
            get
            {
                if (ViewState["_UidRelated"] == null)
                    return "";
                else
                    return (String)ViewState["_UidRelated"];
            }
            set { ViewState["_UidRelated"] = value; }
        }
        public String contentTableRelated
        {
            get
            {
                if (ViewState["_ContentTableRelated"] == null)
                    return "";
                else
                    return (String)ViewState["_ContentTableRelated"];
            }
            set { ViewState["_ContentTableRelated"] = value; }
        }
        public String sectionUri
        {
            get
            {
                if (ViewState["_sectionUri"] == null)
                    return "";
                else
                    return (String)ViewState["_sectionUri"];
            }
            set { ViewState["_sectionUri"] = value; }
        }
        public String confirm
        {
            get
            {
                if (ViewState["_confirm"] == null)
                    return "";
                else
                    return (String)ViewState["_confirm"];
            }
            set { ViewState["_confirm"] = value; }
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
            #region Check Login
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
                //
                section = "" + Request["section"];
                searchText = "" + Request["searchText"];
                searchUid = "" + Request["searchUid"];
                sectionUri = Request["sectionUri"];
                showDeleted = "" + Request["showDeleted"];

                // Parametri in QueryString                                
                listPageSize = "" + Request["pageSize"];
                listPageNumber = "" + Request["pageNumber"];
                listOrderFied = "" + Request["orderFied"];
                listOrderAscendig = "" + Request["orderAscendig"];

                // Parametri Dettaglio
                Uid = currentCmsUserSession.currentUid.ToString();

                // Parametri per il copia
                UidCopy = "" + Request["UidCopy"];

                // Parametri Correlati
                contentTableRelated = "" + Request["ContentTableRelated"];
                UidRelated = "" + Request["UidRelated"];

                #region Notify
                if (!String.IsNullOrEmpty("" + Request["confirm"]))
                    confirm = "" + Request["confirm"];

                // Notify(message, position, timeout, theme, icon, closable)
                if (confirm.Equals("draft"))
                {
                    if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel("Cms.Notify_Draft.Text")))
                        responseScript += "Notify('" + currentCmsUserSession.GetGlobalLabel("Cms.Notify_Draft.Text") + "', 'bottom-right', '5000', 'palegreen', 'fa-power-off', true)";
                    else
                        responseScript += "Notify('Item Saved as a Draft', 'bottom-right', '5000', 'palegreen', 'fa-power-off', true)";
                }
                else if (confirm.Equals("stage"))
                {
                    if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel("Cms.Notify_Publish.Text")))
                        responseScript += "Notify('" + currentCmsUserSession.GetGlobalLabel("Cms.Notify_Publish.Text") + "', 'bottom-right', '5000', 'success', 'fa-power-off', true)";
                    else
                        responseScript += "Notify('Item Saved on Stage', 'bottom-right', '5000', 'success', 'fa-power-off', true)";
                }
                else if (confirm.Equals("delete"))
                {
                    if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel("Cms.Notify_Deleted.Text")))
                        responseScript += "Notify('" + currentCmsUserSession.GetGlobalLabel("Cms.Notify_Deleted.Text") + "', 'bottom-right', '5000', 'danger', 'fa-trash-o', true)";
                    else
                        responseScript += "Notify('Item Deleted', 'bottom-right', '5000', 'danger', 'fa-trash-o', true)";
                }
                else if (confirm.Equals("errorSave"))
                {
                    if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel("Cms.Notify_Error.Text")))
                        responseScript += "Notify('" + currentCmsUserSession.GetGlobalLabel("Cms.Notify_Error.Text") + "', 'bottom-right', '5000', 'danger', 'fa-warning', true)";
                    else
                        responseScript += "Notify('Unexpected error, can not save data', 'bottom-right', '5000', 'danger', 'fa-warning', true)";
                }
                #endregion

                //
                InitPage();

                //
                if (!String.IsNullOrEmpty(Uid))
                    InitUpdate(Uid);
            }

            // Uc
            sidebarMenu1.currentCmsSubSections = "";
            sidebarMenu1.currentCmsUserSession = currentCmsUserSession;
            sidebarMenu1.currentCmsSubSections_SectionUri = sectionUri;
            CmsRepositoryMain.currentCmsUserSession = currentCmsUserSession;
        }

        /* Save Button */
        protected void Button_SaveAsDraft_Click(object sender, EventArgs e)
        {
            //
            Boolean bUpdate = false;
            if (!String.IsNullOrEmpty(Uid))
                bUpdate = true;

            if (Save(EnumCmsContent.Disabled))
            {
                if (bUpdate)
                    Response.Redirect(getCurrentListUrl() + "&confirm=draft");
                else
                    Response.Redirect(getCurrentDetailUrl(Uid) + "&confirm=draft");
            }
            else
            {
                // Error

            }
        }
        protected void Button_SaveOnStage_Click(object sender, EventArgs e)
        {

            if (CmsTextBox_Password.Value.Equals(CmsTextBox_ConfirmPassword.Value))
            {
                //
                Boolean bUpdate = false;
                if (!String.IsNullOrEmpty(Uid))
                    bUpdate = true;

                if (Save(EnumCmsContent.Enabled))
                {
                    Response.Redirect(getCurrentDetailUrl(Uid) + "&confirm=stage");
                }
                else
                {
                    // Error

                }
            }
            else
            {
                Literal_PasswordError.Text = "Le password inserite non corrispondono!";
            }
        }
        /* Other Button */
        protected void Button_Cancel_Click(object sender, EventArgs e)
        {
            String sCurrentUrl = getCurrentListUrl();
            Response.Redirect(sCurrentUrl);
        }
        protected void Button_Reset_Click(object sender, EventArgs e)
        {
            String sCurrentUrl = getCurrentDetailUrl();
            Response.Redirect(sCurrentUrl);
        }
        #endregion

        #region Add Method
        protected void InitPage()
        {
            #region Cms Language
            if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel("Cms.Literal_Cms_LastCorrection.Text")))
                Literal_Cms_LastCorrection.Text = currentCmsUserSession.GetGlobalLabel("Cms.Literal_Cms_LastCorrection.Text");

            if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel("Cms.Button_SaveAsDraft.Text")))
                Button_SaveAsDraft.Text = currentCmsUserSession.GetGlobalLabel("Cms.Button_SaveAsDraft.Text");

            if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel("Cms.Button_Reset.Text")))
                Button_Reset.Text = currentCmsUserSession.GetGlobalLabel("Cms.Button_Reset.Text");

            if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel("Cms.Button_Cancel.Text")))
                Button_Cancel.Text = currentCmsUserSession.GetGlobalLabel("Cms.Button_Cancel.Text");
            #endregion

            // Set Crop Aree And CmsDropDown Bindings


            // Panel Size
            //Panel_Left.CssClass = Panel_Left.CssClass.Replace("col-lg-3", "col-lg-6");
            Panel_Left.CssClass = Panel_Left.CssClass.Replace("isinedit", "isnotinedit");

            //Panel_Related.CssClass = Panel_Related.CssClass.Replace("col-lg-3", "col-lg-6");
            //Panel_Related.CssClass = Panel_Related.CssClass.Replace("isinedit", "isnotinedit");
        }

        /* Init Data Form */
        protected void InitUpdate(String _Uid)
        {
            //
            CmsUsersItemResponse responseGet = objCmsBoDataLibs.CmsUsersGet(header, _Uid);
            if (responseGet.Success)
            {
                //               
                CmsUsers updateObject = responseGet.item;

                #region Icon Status Flag
                iconStatusFlag.Visible = true;
                if (updateObject.StatusFlag.Equals((int)EnumCmsContent.Enabled))
                    iconStatusFlag.Attributes.Add("class", "fa fa-power-off success");
                else
                    iconStatusFlag.Attributes.Add("class", "fa fa-power-off danger");

                // LastModify
                Literal_LastModify.Text = objLibDate.DateTimeToString(updateObject.CreationDate, "GMA", "/", ":");
                if (updateObject.UpdateDate != null)
                {
                    DateTime dUpdate = (DateTime)updateObject.UpdateDate;
                    Literal_LastModify.Text = objLibDate.DateTimeToString(dUpdate, "GMA", "/", ":");
                }
                Literal_LastModify.Visible = true;
                Literal_Cms_LastCorrection.Visible = true;
                #endregion

                #region Form

                // Uid
                if (updateObject.Uid != null)
                    CmsTextBox_Uid.SetValue(updateObject.Uid.ToString());

                // Name
                if (updateObject.Name != null)
                    CmsTextBox_Name.SetValue(updateObject.Name.ToString());

                // Surname
                if (updateObject.Surname != null)
                    CmsTextBox_Surname.SetValue(updateObject.Surname.ToString());

                // Email
                if (updateObject.Email != null)
                    CmsTextBox_Email.SetValue(updateObject.Email.ToString());

                // Email
                if (updateObject.Password != null)
                    CmsTextBox_Password.SetValue(updateObject.Password.ToString());

                // Email
                if (updateObject.Password != null)
                    CmsTextBox_ConfirmPassword.SetValue(updateObject.Password.ToString());

                // Username
                if (updateObject.Username != null)
                    CmsTextBox_Username.SetValue(updateObject.Username.ToString());
                #endregion
            }

            // Panel Size
            //Panel_Left.CssClass = Panel_Left.CssClass.Replace("col-lg-3", "col-lg-6");
            Panel_Left.CssClass = Panel_Left.CssClass.Replace("isinedit", "isnotinedit");

            //Panel_Related.CssClass = Panel_Related.CssClass.Replace("col-lg-3", "col-lg-6");
            //Panel_Related.CssClass = Panel_Related.CssClass.Replace("isinedit", "isnotinedit"); 
        }

        /* Save */
        protected Boolean Save(EnumCmsContent _statusFlag)
        {
            // Get For Update
            CmsUsers updateObject = null;
            if (!String.IsNullOrEmpty(Uid))
            {
                CmsUsersItemResponse responseUserGet = objCmsBoDataLibs.CmsUsersGetCms(header, Uid);
                updateObject = responseUserGet.item;
            }
            else
            {
                updateObject = new CmsUsers();
            }

            if (updateObject != null)
            {

                // Uid
                if (!String.IsNullOrEmpty(CmsTextBox_Uid.Value))
                    updateObject.Uid = System.Convert.ToInt32(CmsTextBox_Uid.Value);

                // Title
                updateObject.Name = CmsTextBox_Name.Value;

                // ShortText
                updateObject.Surname = CmsTextBox_Surname.Value;

                // Email
                updateObject.Email = CmsTextBox_Email.Value;

                // Password
                updateObject.Surname = CmsTextBox_Surname.Value;

                // ProgramText
                updateObject.Password = CmsTextBox_Password.Value;

                updateObject.StatusFlag = (int)_statusFlag;

                CmsUsersItemResponse responseUpsert = objCmsBoDataLibs.CmsUsersUpsert(header, updateObject);
                if (responseUpsert.Success)
                {
                    updateObject.Uid = responseUpsert.item.Uid;
                    Uid = updateObject.Uid.ToString();
                }

                return responseUpsert.Success;
            }
            else
            {
                return false;
            }

        }
        #endregion        

        #region Add Method Parameter
        protected String getCurrentDetailUrl()
        {
            return getCurrentDetailUrl("");
        }
        protected String getCurrentDetailUrl(String _uid)
        {
            String sReturn = String.Empty;

            if (_uid.Length > 0)
                sReturn = "user_settings.aspx?Uid=" + _uid + "&section=" + section + "&pageNumber=" + listPageNumber + "&pageSize=" + listPageSize + "&orderFied=" + Server.UrlEncode(listOrderFied) + "&orderAscendig=" + listOrderAscendig.ToString() + "&searchText=" + Server.UrlEncode(searchText) + "&searchUid=" + searchUid + "&sectionUri=" + sectionUri + "&showDeleted=" + showDeleted;
            else
                sReturn = "user_settings.aspx?section=" + section + "&pageNumber=" + listPageNumber + "&pageSize=" + listPageSize + "&orderFied=" + Server.UrlEncode(listOrderFied) + "&orderAscendig=" + listOrderAscendig.ToString() + "&searchText=" + Server.UrlEncode(searchText) + "&searchUid=" + searchUid + "&sectionUri=" + sectionUri + "&showDeleted=" + showDeleted;

            return sReturn;

        }
        protected String getCurrentListUrl()
        {
            return "list.aspx?section=" + section + "&pageNumber=" + listPageNumber + "&pageSize=" + listPageSize + "&orderFied=" + Server.UrlEncode(listOrderFied) + "&orderAscendig=" + listOrderAscendig.ToString() + "&searchText=" + Server.UrlEncode(searchText) + "&searchUid=" + searchUid + "&sectionUri=" + sectionUri + "&showDeleted=" + showDeleted;
        }
        public String getDataActionUrl(String _uid, String _contentTable)
        {
            return "dataAction=list&dataContent=" + _contentTable + "&dataUid=" + _uid + "&section=" + section + "&pageNumber=" + listPageNumber + "&pageSize=" + listPageSize + "&orderFied=" + Server.UrlEncode(listOrderFied) + "&orderAscendig=" + listOrderAscendig.ToString() + "&searchText=" + Server.UrlEncode(searchText) + "&searchUid=" + searchUid + "&sectionUri=" + sectionUri + "&showDeleted=" + showDeleted + "&dataUidRelated=" + UidRelated;
        }
        #endregion        
    }
}
