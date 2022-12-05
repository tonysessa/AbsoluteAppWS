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

namespace backOffice.cmsroles
{
    public partial class details : System.Web.UI.Page
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
        public String sUploadDir = "cmsroles/";
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
                Uid = "" + Request["Uid"];
                
                // Parametri per il copia
                UidCopy = "" + Request["UidCopy"];

				// Parametri Correlati
				contentTableRelated = "" + Request["ContentTableRelated"];
                UidRelated = "" + Request["UidRelated"];
                
				#region Notify
                if (!String.IsNullOrEmpty("" + Request["confirm"]))
                    confirm = "" + Request["confirm"];

                // Notify(message, position, timeout, theme, icon, closable)
                if (confirm.Equals("draft")){
                    if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel("Cms.Notify_Draft.Text"))) 
						responseScript += "Notify('" + currentCmsUserSession.GetGlobalLabel("Cms.Notify_Draft.Text") +"', 'bottom-right', '5000', 'palegreen', 'fa-power-off', true)";
					else
						responseScript += "Notify('Item Saved as a Draft', 'bottom-right', '5000', 'palegreen', 'fa-power-off', true)";
				}
                else if (confirm.Equals("stage")){
					 if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel("Cms.Notify_Publish.Text"))) 
						responseScript += "Notify('" + currentCmsUserSession.GetGlobalLabel("Cms.Notify_Publish.Text") + "', 'bottom-right', '5000', 'success', 'fa-power-off', true)";
					 else
						responseScript += "Notify('Item Saved on Stage', 'bottom-right', '5000', 'success', 'fa-power-off', true)";
				}
                else if (confirm.Equals("delete")){
                    if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel("Cms.Notify_Deleted.Text"))) 
						responseScript += "Notify('"+ currentCmsUserSession.GetGlobalLabel("Cms.Notify_Deleted.Text") +"', 'bottom-right', '5000', 'danger', 'fa-trash-o', true)";
					else
						responseScript += "Notify('Item Deleted', 'bottom-right', '5000', 'danger', 'fa-trash-o', true)";
				}
                else if (confirm.Equals("errorSave")){
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

                // Copy                
                if (!String.IsNullOrEmpty(UidCopy))
                    InitUpdate(UidCopy);

                // Related
                if (!String.IsNullOrEmpty(contentTableRelated))              
                    InitUpdateRelated();                               
            }

            // Uc
            sidebarMenu1.currentCmsSubSections = "AdminCmsRoles";
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
			//
			Boolean bUpdate = false;
            if (!String.IsNullOrEmpty(Uid))
                bUpdate = true;

            if (Save(EnumCmsContent.Enabled))
            {
				if (bUpdate)
                    Response.Redirect(getCurrentListUrl() + "&confirm=stage");
                else
                    Response.Redirect(getCurrentDetailUrl(Uid) + "&confirm=stage");
            }
            else
            {
                // Error

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
			if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel("Cms.Literal_Cms_RecordDetail.Text")))
				Literal_Cms_RecordDetail.Text = currentCmsUserSession.GetGlobalLabel("Cms.Literal_Cms_RecordDetail.Text");

			if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel("Cms.Literal_Cms_LastCorrection.Text")))
				Literal_Cms_LastCorrection.Text =  currentCmsUserSession.GetGlobalLabel("Cms.Literal_Cms_LastCorrection.Text");

			if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel("Cms.Button_SaveAsDraft.Text")))
				Button_SaveAsDraft.Text = currentCmsUserSession.GetGlobalLabel("Cms.Button_SaveAsDraft.Text");

			if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel("Cms.Button_SaveOnStage.Text")))
				Button_SaveOnStage.Text = currentCmsUserSession.GetGlobalLabel("Cms.Button_SaveOnStage.Text");

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
            CmsRolesItemResponse responseGet = objCmsBoDataLibs.CmsRolesGetCms(header, _Uid);
            if (responseGet.Success)
            {
				//               
                CmsRoles updateObject = responseGet.item;

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

                
                // Title
                if (updateObject.Title != null)
                      CmsTextBox_Title.SetValue(updateObject.Title.ToString());
                
                // Uriname
                if (updateObject.Uriname != null)
                      CmsTextBox_Uriname.SetValue(updateObject.Uriname.ToString());

				#endregion 
            }

            // Load Related
            CmsDropDownList_CmsRoles_Acl_CmsSubSections_Uid_Bind();


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
            CmsRoles updateObject = null;
            if (!String.IsNullOrEmpty(Uid))
            {
                CmsRolesItemResponse responseUserGet = objCmsBoDataLibs.CmsRolesGetCms(header, Uid);
                updateObject = responseUserGet.item;
            }
            else
            {
                updateObject = new CmsRoles();
            }

            if (updateObject != null)
            {

                // Uid
				if (!String.IsNullOrEmpty(CmsTextBox_Uid.Value))
					updateObject.Uid = System.Convert.ToInt32(CmsTextBox_Uid.Value);

                
                // Title
                updateObject.Title = CmsTextBox_Title.Value;
                
                // Uriname
                updateObject.Uriname = CmsTextBox_Uriname.Value;

                updateObject.StatusFlag = (int)_statusFlag;

                CmsRolesItemResponse responseUpsert = objCmsBoDataLibs.CmsRolesUpsert(header, updateObject);
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
                sReturn = "details.aspx?Uid=" + _uid + "&section=" + section + "&pageNumber=" + listPageNumber + "&pageSize=" + listPageSize + "&orderFied=" + Server.UrlEncode(listOrderFied) + "&orderAscendig=" + listOrderAscendig.ToString() + "&searchText=" + Server.UrlEncode(searchText) + "&searchUid=" + searchUid + "&sectionUri=" + sectionUri + "&showDeleted=" + showDeleted;
            else
                sReturn = "details.aspx?section=" + section + "&pageNumber=" + listPageNumber + "&pageSize=" + listPageSize + "&orderFied=" + Server.UrlEncode(listOrderFied) + "&orderAscendig=" + listOrderAscendig.ToString() + "&searchText=" + Server.UrlEncode(searchText) + "&searchUid=" + searchUid + "&sectionUri=" + sectionUri + "&showDeleted=" + showDeleted;

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

		#region Related Method Parameter
		protected void InitUpdateRelated()
        {
            //
            HideAllRelated();

			// Panel Size
            if (!String.IsNullOrEmpty(contentTableRelated))
            {
                //Panel_Left.CssClass = Panel_Left.CssClass.Replace("col-lg-6", "col-lg-3");
                Panel_Left.CssClass = Panel_Left.CssClass.Replace("isnotinedit", "isinedit");
                
				//Panel_Related.CssClass = Panel_Related.CssClass.Replace("col-lg-6", "col-lg-3");
				//Panel_Related.CssClass = Panel_Related.CssClass.Replace("isnotinedit", "isinedit");
            }

			//
            //
            if (contentTableRelated.Equals("CmsUsers"))
            {
                 //
            if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel("Cms.Button_Related_Salva.Text")))
                Button_Related_CmsUsers_Save.Text = currentCmsUserSession.GetGlobalLabel("Cms.Button_Related_Salva.Text");
            if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel("Cms.Button_Related_SalvaContinua.Text")))
                Button_Related_CmsUsers_SaveAndNext.Text = currentCmsUserSession.GetGlobalLabel("Cms.Button_Related_SalvaContinua.Text");
            if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel("Cms.Button_Related_Annulla.Text")))
                Button_Related_CmsUsers_Cancel.Text = currentCmsUserSession.GetGlobalLabel("Cms.Button_Related_Annulla.Text");
                 Panel_Related_CmsUsers_Form.Visible = true;
                 Panel_Related_CmsUsers_List.Visible = false;

                 if (!String.IsNullOrEmpty(UidRelated))
                     InitUpdate_CmsUsers(UidRelated);
            }
            //
            if (contentTableRelated.Equals("CmsRoles_Acl"))
            {
                 //
            if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel("Cms.Button_Related_Salva.Text")))
                Button_Related_CmsRoles_Acl_Save.Text = currentCmsUserSession.GetGlobalLabel("Cms.Button_Related_Salva.Text");
            if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel("Cms.Button_Related_SalvaContinua.Text")))
                Button_Related_CmsRoles_Acl_SaveAndNext.Text = currentCmsUserSession.GetGlobalLabel("Cms.Button_Related_SalvaContinua.Text");
            if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel("Cms.Button_Related_Annulla.Text")))
                Button_Related_CmsRoles_Acl_Cancel.Text = currentCmsUserSession.GetGlobalLabel("Cms.Button_Related_Annulla.Text");
                 Panel_Related_CmsRoles_Acl_Form.Visible = true;
                 Panel_Related_CmsRoles_Acl_List.Visible = false;

                 if (!String.IsNullOrEmpty(UidRelated))
                     InitUpdate_CmsRoles_Acl(UidRelated);
            }

        }
		public void HideAllRelated()
        {
            //Hide
            Panel_Related_CmsUsers_List.Visible = false;
            Panel_Related_CmsUsers_Form.Visible = false;
            Panel_Related_CmsRoles_Acl_List.Visible = false;
            Panel_Related_CmsRoles_Acl_Form.Visible = false;

        }
        protected void CmsDropDownList_CmsRoles_Acl_CmsSubSections_Uid_Bind()
        {
            
            // Binding CmsDropDownList_CmsRoles_Acl_CmsSubSections_Uid
            CmsSubSectionsListOptions rqCmsSubSectionsListOptions = new CmsSubSectionsListOptions();
            rqCmsSubSectionsListOptions.sortBy = "Uid_CmsSections asc, Title";
            rqCmsSubSectionsListOptions.sortAscending = true;
            //rqCmsSubSectionsListOptions.uid_CmsNlsContext = header.cmsCmsNlsContext.Uid;
            
            CmsSubSectionsListResponse rsCmsSubSectionsListResponse = objCmsBoDataLibs.CmsSubSectionsListCms(header, rqCmsSubSectionsListOptions);
            if (rsCmsSubSectionsListResponse != null)
            {
                CmsDropDownList_CmsRoles_Acl_CmsSubSections_Uid.DataSource = rsCmsSubSectionsListResponse.items;
                CmsDropDownList_CmsRoles_Acl_CmsSubSections_Uid.DataTextField = "FullTitle";
                CmsDropDownList_CmsRoles_Acl_CmsSubSections_Uid.DataValueField = "Uid";
                CmsDropDownList_CmsRoles_Acl_CmsSubSections_Uid.Bind();
            }
        }

        protected void InitUpdate_CmsUsers(String nUid)
        {

            if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel("Cms.Literal_Cms_LastCorrection.Text")))
                Literal_Cms_CmsUsers_LastCorrection.Text = currentCmsUserSession.GetGlobalLabel("Cms.Literal_Cms_LastCorrection.Text");
            // Show Button Save
            Button_Related_CmsUsers_Save.Visible = true;

            CmsUsersItemResponse responseGet = objCmsBoDataLibs.CmsUsersGetCms(header, nUid);
            if ((responseGet.Success) && (responseGet.item != null))
            {
                // Get Object CmsUsers 
                CmsUsers updateObject = responseGet.item;
                #region Icon Status Flag
                if (updateObject.StatusFlag.Equals((int)EnumCmsContent.Enabled))
                    iconStatusFlag_CmsUsers.Attributes.Add("class", "fa fa-power-off success");
                else
                    iconStatusFlag_CmsUsers.Attributes.Add("class", "fa fa-power-off danger");

                Literal_CmsUsers_LastModify.Text = objLibDate.DateTimeToString(updateObject.CreationDate, "GMA", "/", ":");
                if (updateObject.UpdateDate != null)
                {
                    DateTime dUpdate = (DateTime)updateObject.UpdateDate;
                    Literal_CmsUsers_LastModify.Text = objLibDate.DateTimeToString(dUpdate, "GMA", "/", ":");
                }
                Literal_CmsUsers_LastModify.Visible = true;
                Literal_Cms_CmsUsers_LastCorrection.Visible = true;
                #endregion

                
                // Name
                if (updateObject.Name != null)
                    CmsTextBox_CmsUsers_Name.SetValue(updateObject.Name.ToString());
                
                // Surname
                if (updateObject.Surname != null)
                    CmsTextBox_CmsUsers_Surname.SetValue(updateObject.Surname.ToString());
                
                // Email
                if (updateObject.Email != null)
                    CmsTextBox_CmsUsers_Email.SetValue(updateObject.Email.ToString());
                
                // Username
                if (updateObject.Username != null)
                    CmsTextBox_CmsUsers_Username.SetValue(updateObject.Username.ToString());
                
                // Password
                if (updateObject.Password != null)
                    CmsTextBox_CmsUsers_Password.SetValue(updateObject.Password.ToString());
                
                // DateLastLogin
                if (updateObject.DateLastLogin != null)
                  CmsTextBoxData_CmsUsers_DateLastLogin.SetValue((DateTime)updateObject.DateLastLogin);
                
                // NumLogin
                if (updateObject.NumLogin != null)
                    CmsTextBox_CmsUsers_NumLogin.SetValue(updateObject.NumLogin.ToString());
                
                // Note
                if (updateObject.Note != null)
                    CmsHtmlEditor_CmsUsers_Note.SetValue(updateObject.Note);
            }
        }
        protected void InitUpdate_CmsRoles_Acl(String nUid)
        {

            if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel("Cms.Literal_Cms_LastCorrection.Text")))
                Literal_Cms_CmsRoles_Acl_LastCorrection.Text = currentCmsUserSession.GetGlobalLabel("Cms.Literal_Cms_LastCorrection.Text");
            // Show Button Save
            Button_Related_CmsRoles_Acl_Save.Visible = true;

            CmsRoles_AclItemResponse responseGet = objCmsBoDataLibs.CmsRoles_AclGetCms(header, nUid);
            if ((responseGet.Success) && (responseGet.item != null))
            {
                // Get Object CmsRoles_Acl 
                CmsRoles_Acl updateObject = responseGet.item;
                #region Icon Status Flag
                if (updateObject.StatusFlag.Equals((int)EnumCmsContent.Enabled))
                    iconStatusFlag_CmsRoles_Acl.Attributes.Add("class", "fa fa-power-off success");
                else
                    iconStatusFlag_CmsRoles_Acl.Attributes.Add("class", "fa fa-power-off danger");

                Literal_CmsRoles_Acl_LastModify.Text = objLibDate.DateTimeToString(updateObject.CreationDate, "GMA", "/", ":");
                if (updateObject.UpdateDate != null)
                {
                    DateTime dUpdate = (DateTime)updateObject.UpdateDate;
                    Literal_CmsRoles_Acl_LastModify.Text = objLibDate.DateTimeToString(dUpdate, "GMA", "/", ":");
                }
                Literal_CmsRoles_Acl_LastModify.Visible = true;
                Literal_Cms_CmsRoles_Acl_LastCorrection.Visible = true;
                #endregion

                
                // Uid_CmsSubSections
                if (updateObject.Uid_CmsSubSections != null)
                CmsDropDownList_CmsRoles_Acl_CmsSubSections_Uid.SetValue(updateObject.Uid_CmsSubSections.ToString());
                
                // CanRead
                if (updateObject.CanRead != null)
                    CmsTextBox_CmsRoles_Acl_CanRead.SetValue(updateObject.CanRead.ToString());
                
                // CanWrite
                if (updateObject.CanWrite != null)
                    CmsTextBox_CmsRoles_Acl_CanWrite.SetValue(updateObject.CanWrite.ToString());
            }
        }

        protected Boolean Save_CmsUsers(EnumCmsContent _statusFlag)
        {

            // Object CmsUsers 
            CmsUsers updateObject = new CmsUsers();

            if (!String.IsNullOrEmpty(UidRelated))
                updateObject = objCmsBoDataLibs.CmsUsersGetCms(header, UidRelated).item;

            // Uid CmsRoles 
            if (objLibMath.isNumber(Uid)) 
                updateObject.Uid_CmsRoles = System.Convert.ToInt32(Uid);
            
            // Name
            updateObject.Name = CmsTextBox_CmsUsers_Name.Value;
            
            // Surname
            updateObject.Surname = CmsTextBox_CmsUsers_Surname.Value;
            
            // Email
            updateObject.Email = CmsTextBox_CmsUsers_Email.Value;
            
            // Username
            updateObject.Username = CmsTextBox_CmsUsers_Username.Value;
            
            // Password
            updateObject.Password = CmsTextBox_CmsUsers_Password.Value;
                
                // DateLastLogin
                if (!CmsTextBoxData_CmsUsers_DateLastLogin.Value.Equals(DateTime.MinValue))
                    updateObject.DateLastLogin = CmsTextBoxData_CmsUsers_DateLastLogin.Value;
                else
                    updateObject.DateLastLogin = null;
            
            // NumLogin
            if (objLibMath.isNumber(CmsTextBox_CmsUsers_NumLogin.Value))
              updateObject.NumLogin = System.Convert.ToInt32(CmsTextBox_CmsUsers_NumLogin.Value);
            else
              updateObject.NumLogin = null;
            
            // Note
            updateObject.Note = CmsHtmlEditor_CmsUsers_Note.Value;

            // StatusFlag 
            updateObject.StatusFlag = (int)EnumCmsContent.Enabled;

            CmsUsersItemResponse responseUpsert = objCmsBoDataLibs.CmsUsersUpsert(header, updateObject);
            if (responseUpsert.Success)
            {
                updateObject.Uid = responseUpsert.item.Uid;
                UidRelated = updateObject.Uid.ToString();
            }
            return responseUpsert.Success;        }
        protected void Button_Related_CmsUsers_Save_Click(object sender, EventArgs e)
        {

            //nn avevo questa Associazione, quindi procedo con inserimento
            if (Save_CmsUsers(EnumCmsContent.Enabled))
            {
                //
                Response.Redirect(getCurrentDetailUrl(Uid.ToString()));
            }
            else
            {
                // Error
            }
        }
        protected void Button_Related_CmsUsers_SaveAndNext_Click(object sender, EventArgs e)
        {

            //nn avevo questa Associazione, quindi procedo con inserimento
            if (Save_CmsUsers(EnumCmsContent.Enabled))
            {
                //
                Response.Redirect(getCurrentDetailUrl(Uid.ToString() + "&ContentTableRelated=CmsUsers&UidRelated=" + HiddenField_CmsUsers_NextUid.Value));
            }
            else
            {
                // Error
            }
        }
        protected void Button_Related_CmsUsers_Cancel_Click(object sender, EventArgs e)
        {
            //
            Response.Redirect(getCurrentDetailUrl(Uid.ToString()));
        }

        protected Boolean Save_CmsRoles_Acl(EnumCmsContent _statusFlag)
        {

            // Object CmsRoles_Acl 
            CmsRoles_Acl updateObject = new CmsRoles_Acl();

            if (!String.IsNullOrEmpty(UidRelated))
                updateObject = objCmsBoDataLibs.CmsRoles_AclGetCms(header, UidRelated).item;

            // Uid CmsRoles 
            if (objLibMath.isNumber(Uid)) 
                updateObject.Uid_CmsRoles = System.Convert.ToInt32(Uid);
                
                if (objLibMath.isNumber(CmsDropDownList_CmsRoles_Acl_CmsSubSections_Uid.Value))
                    updateObject.Uid_CmsSubSections = System.Convert.ToInt32(CmsDropDownList_CmsRoles_Acl_CmsSubSections_Uid.Value);
            
            // CanRead
            if (objLibMath.isNumber(CmsTextBox_CmsRoles_Acl_CanRead.Value))
              updateObject.CanRead = System.Convert.ToInt32(CmsTextBox_CmsRoles_Acl_CanRead.Value);
            else
              updateObject.CanRead = null;
            
            // CanWrite
            if (objLibMath.isNumber(CmsTextBox_CmsRoles_Acl_CanWrite.Value))
              updateObject.CanWrite = System.Convert.ToInt32(CmsTextBox_CmsRoles_Acl_CanWrite.Value);
            else
              updateObject.CanWrite = null;

            // StatusFlag 
            updateObject.StatusFlag = (int)EnumCmsContent.Enabled;

            CmsRoles_AclItemResponse responseUpsert = objCmsBoDataLibs.CmsRoles_AclUpsert(header, updateObject);
            if (responseUpsert.Success)
            {
                updateObject.Uid = responseUpsert.item.Uid;
                UidRelated = updateObject.Uid.ToString();
            }
            return responseUpsert.Success;        }
        protected void Button_Related_CmsRoles_Acl_Save_Click(object sender, EventArgs e)
        {

            //nn avevo questa Associazione, quindi procedo con inserimento
            if (Save_CmsRoles_Acl(EnumCmsContent.Enabled))
            {
                //
                Response.Redirect(getCurrentDetailUrl(Uid.ToString()));
            }
            else
            {
                // Error
            }
        }
        protected void Button_Related_CmsRoles_Acl_SaveAndNext_Click(object sender, EventArgs e)
        {

            //nn avevo questa Associazione, quindi procedo con inserimento
            if (Save_CmsRoles_Acl(EnumCmsContent.Enabled))
            {
                //
                Response.Redirect(getCurrentDetailUrl(Uid.ToString() + "&ContentTableRelated=CmsRoles_Acl&UidRelated=" + HiddenField_CmsRoles_Acl_NextUid.Value));
            }
            else
            {
                // Error
            }
        }
        protected void Button_Related_CmsRoles_Acl_Cancel_Click(object sender, EventArgs e)
        {
            //
            Response.Redirect(getCurrentDetailUrl(Uid.ToString()));
        }


		#endregion
    }
}
