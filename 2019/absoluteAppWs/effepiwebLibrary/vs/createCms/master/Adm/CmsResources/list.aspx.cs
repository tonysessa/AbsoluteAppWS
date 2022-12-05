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

namespace backOffice.adm.cmsresources
{
    public partial class list : System.Web.UI.Page
    {
        #region objUtil
        protected Support.Library.DateUtil objLibDate = new Support.Library.DateUtil();
        protected Support.Library.DbUtil objLibDB = new Support.Library.DbUtil();
        protected Support.Library.StringUtil objLibString = new Support.Library.StringUtil();
        protected Support.Library.StringSqlUtil objLibSqlString = new Support.Library.StringSqlUtil();
        protected Support.Library.MathUtil objLibMath = new Support.Library.MathUtil();
        protected Support.Library.CriptUtil objLibCript = new Support.Library.CriptUtil();
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
        public Int32 listPageSize
        {
            get
            {
                if (ViewState["_LISTPAGESIZE"] == null)
                    return 10;
                else
                    return (Int32)ViewState["_LISTPAGESIZE"];
            }
            set { ViewState["_LISTPAGESIZE"] = value; }
        }
        public Int32 listPageNumber
        {
            get
            {
                if (ViewState["_LISTPAGENUMBER"] == null)
                    return 0;
                else
                    return (Int32)ViewState["_LISTPAGENUMBER"];
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
                if (ViewState["_listOrderAscendig"] == null)
                    return "asc";
                else
                    return (String)ViewState["_listOrderAscendig"];
            }
            set { ViewState["_listOrderAscendig"] = value; }
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
        //
        public Hashtable hashtableSelected
        {
            get
            {
                if (ViewState["_hashtableSelected"] == null)
                    return null;
                else
                    return (Hashtable)ViewState["_hashtableSelected"];
            }
            set { ViewState["_hashtableSelected"] = value; }
        }
        public Hashtable hashtableAll
        {
            get
            {
                if (ViewState["_hashtableAll"] == null)
                    return null;
                else
                    return (Hashtable)ViewState["_hashtableAll"];
            }
            set { ViewState["_hashtableAll"] = value; }
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

            // PostBack
            if (!Page.IsPostBack)
            {
                // Parametri in QueryString     
                if (!String.IsNullOrEmpty("" + Request["section"]))
                    section = Request["section"];

                if (objLibMath.isNumber("" + Request["pageSize"]))
                    listPageSize = System.Convert.ToInt32(Request["pageSize"]);

                if (objLibMath.isNumber("" + Request["pageNumber"]))
                    listPageNumber = System.Convert.ToInt32(Request["pageNumber"]);

                if (!String.IsNullOrEmpty("" + Request["orderFied"]))
                    listOrderFied = Request["orderFied"];

                if (!String.IsNullOrEmpty("" + Request["orderAscendig"]))
                    listOrderAscendig = Request["orderAscendig"].ToLower();

                if (!String.IsNullOrEmpty("" + Request["searchText"]))
                    searchText = Request["searchText"];

                if (!String.IsNullOrEmpty("" + Request["searchUid"]))
                    searchUid = Request["searchUid"];

                if (!String.IsNullOrEmpty("" + Request["sectionUri"]))
                    sectionUri = Request["sectionUri"];					

                if (!String.IsNullOrEmpty("" + Request["showDeleted"]))
                    showDeleted = "" + Request["showDeleted"];                

				// Enable Order
				Boolean bOrder = false;

                // Set Default Order                        
                if (String.IsNullOrEmpty(listOrderFied))
                {
                    listOrderFied = "Uid";
                    listOrderAscendig = "Asc";

					if (bOrder) 
					{
						//
						listOrderFied = "Ord";
						listOrderAscendig = "Asc";
					}
                }
			
				#region Notify
                if (!String.IsNullOrEmpty("" + Request["confirm"]))
                    confirm = "" + Request["confirm"];

                // Notify(message, position, timeout, theme, icon, closable)
                if (confirm.Equals("draft"))
                    responseScript += "Notify('Item Saved as a Draft', 'bottom-right', '5000', 'palegreen', 'fa-power-off', true)";
                else if (confirm.Equals("stage"))
                    responseScript += "Notify('Item Saved on Stage', 'bottom-right', '5000', 'success', 'fa-power-off', true)";
                else if (confirm.Equals("delete"))
                    responseScript += "Notify('Item Deleted', 'bottom-right', '5000', 'danger', 'fa-trash-o', true)";
                else if (confirm.Equals("copy"))
                    responseScript += "Notify('Item Copied', 'bottom-right', '5000', 'palegreen', 'fa-files-o', true)";
                else if (confirm.Equals("copyAll"))
                    responseScript += "Notify('Items Copied', 'bottom-right', '5000', 'palegreen', 'fa-files-o', true)";
                else if (confirm.Equals("publish"))
                    responseScript += "Notify('Item Saved on Stage And Published', 'bottom-right', '5000', 'success', 'fa-globe', true)";
                else if (confirm.Equals("unpublish"))
                    responseScript += "Notify('Item UnPublished', 'bottom-right', '5000', 'danger', 'fa-globe', true)";
                else if (confirm.Equals("publishorder"))
                    responseScript += "Notify('Order Published', 'bottom-right', '5000', 'success', 'fa-globe', true)";
                else if (confirm.Equals("errorSave"))
                    responseScript += "Notify('Unexpected error, can not save data', 'bottom-right', '5000', 'danger', 'fa-warning', true)";
                else if (confirm.Equals("errorPublish"))
                    responseScript += "Notify('Unexpected error, can not publish item', 'bottom-right', '5000', 'warning', 'fa-warning', true)";
				#endregion
				
                //
                InitPage();
            }
			else
            {
                responseScript = "";
            }

            // Uc
            sidebarMenu1.currentCmsSubSections = "AdminCmsResources";
            sidebarMenu1.currentCmsSubSections_SectionUri = sectionUri;
            sidebarMenu1.currentCmsUserSession = currentCmsUserSession;
        }
        protected void InitPage()
        {
			#region Cms Language
			/*
			if (!String.IsNullOrEmpty(currentCmsUserSession.GetLabel("Cms.TextBox_Search.Text"))) 
				TextBox_Search.Attributes.Add("placeholder", currentCmsUserSession.GetLabel("Cms.TextBox_Search.Text"));

			if (!String.IsNullOrEmpty(currentCmsUserSession.GetLabel("Cms.HyperLink_Action_Add.Text")))
				HyperLink_Action_Add.Text = currentCmsUserSession.GetLabel("Cms.HyperLink_Action_Add.Text"); 			

			if (!String.IsNullOrEmpty(currentCmsUserSession.GetLabel("Cms.Literal_Cms_RecordListFor.Text")))
				Literal_Cms_RecordListFor.Text = currentCmsUserSession.GetLabel("Cms.Literal_Cms_RecordListFor.Text"); 

			if (!String.IsNullOrEmpty(currentCmsUserSession.GetLabel("Cms.Literal_Cms_RecordForPage.Text")))
				Literal_Cms_RecordForPage.Text = currentCmsUserSession.GetLabel("Cms.Literal_Cms_RecordForPage.Text");

			if (!String.IsNullOrEmpty(currentCmsUserSession.GetLabel("Cms.HyperLink_Section_Preview.Text")))
				HyperLink_Section_Preview.Text = currentCmsUserSession.GetLabel("Cms.HyperLink_Section_Preview.Text");

			if (!String.IsNullOrEmpty(currentCmsUserSession.GetLabel("Cms.HyperLink_Download_Excel.Text")))
				HyperLink_Download_Excel.Text = currentCmsUserSession.GetLabel("Cms.HyperLink_Download_Excel.Text");				

			if (!String.IsNullOrEmpty(currentCmsUserSession.GetLabel("Cms.Button_ReorderRecord.Text")))            
                Button_ReorderRecord.Text = currentCmsUserSession.GetLabel("Cms.Button_ReorderRecord.Text");            

            if (!String.IsNullOrEmpty(currentCmsUserSession.GetLabel("Cms.Button_ApplyReorder.Text")))            
                Button_ApplyReorder.Text = currentCmsUserSession.GetLabel("Cms.Button_ApplyReorder.Text");            

			if (!String.IsNullOrEmpty(currentCmsUserSession.GetLabel("Cms.CheckBox_Show_Deleted.Text")))
                CheckBox_Show_Deleted.Text = currentCmsUserSession.GetLabel("Cms.CheckBox_Show_Deleted.Text");				
			*/
			#endregion
            
            #region Preview Link
            // HyperLink_Section_Preview.NavigateUrl = sStartingpage;
			// HyperLink_Section_Preview.Visible = true;
			// i_HyperLink_Section_Preview.Visible = true;
            #endregion

			HyperLink_Action_Add.NavigateUrl = getCurrentDetailUrl();

            //
            HyperLink_Action_Enable.Attributes.Add("data-url", "action.aspx");
            HyperLink_Action_Enable.Attributes.Add("data-content", "CmsResources");
            HyperLink_Action_Enable.Attributes.Add("data-returnUrl", getCurrentListUrl());
            HyperLink_Action_Enable.Attributes.Add("data-action", "enable");

            //
            HyperLink_Action_Disable.Attributes.Add("data-url", "action.aspx");
            HyperLink_Action_Disable.Attributes.Add("data-content", "CmsResources");
            HyperLink_Action_Disable.Attributes.Add("data-returnUrl", getCurrentListUrl());
            HyperLink_Action_Disable.Attributes.Add("data-action", "disable");

            //
            HyperLink_Action_Delete.Attributes.Add("data-url", "action.aspx");
            HyperLink_Action_Delete.Attributes.Add("data-content", "CmsResources");
            HyperLink_Action_Delete.Attributes.Add("data-returnUrl", getCurrentListUrl());
            HyperLink_Action_Delete.Attributes.Add("data-action", "delete");

			//
			String cmsLabelKey =  String.Empty;
			String headerTitle =  String.Empty;

            //Cms.CmsResources.Key;
            cmsLabelKey = "Cms.CmsResources.Key";
            headerTitle = currentCmsUserSession.GetGlobalLabel(cmsLabelKey);
            if (String.IsNullOrEmpty(headerTitle))
                headerTitle = cmsLabelKey.Substring(cmsLabelKey.LastIndexOf(".") + 1);
            Literal_Column_Header_Key.Text = headerTitle;
            //Cms.CmsResources.Description;
            cmsLabelKey = "Cms.CmsResources.Description";
            headerTitle = currentCmsUserSession.GetGlobalLabel(cmsLabelKey);
            if (String.IsNullOrEmpty(headerTitle))
                headerTitle = cmsLabelKey.Substring(cmsLabelKey.LastIndexOf(".") + 1);
            Literal_Column_Header_Description.Text = headerTitle;
            //Cms.CmsResources.Note;
            cmsLabelKey = "Cms.CmsResources.Note";
            headerTitle = currentCmsUserSession.GetGlobalLabel(cmsLabelKey);
            if (String.IsNullOrEmpty(headerTitle))
                headerTitle = cmsLabelKey.Substring(cmsLabelKey.LastIndexOf(".") + 1);
            Literal_Column_Header_Note.Text = headerTitle;


            if (hashtableSelected == null)
                hashtableSelected = new Hashtable();

            if (searchText.Length > 0)
                TextBox_Search.Text = searchText;

			// List Response
			/*
            SubSearchListResponse responseSubSearch = objCmsBoDataLibs.SubSearchList(header, rq);
            if (responseSubSearch != null)
            {
				String sDefaulTxt = String.Empty;
				if (DropDownList_Search.Items.Count > 0)
				    sDefaulTxt = DropDownList_Search.Items[0].Text;
				DropDownList_Search.DataSource = currentCmsUserSession.currentEnableCmsNlsContext;
				DropDownList_Search.DataTextField = "Uid";
				DropDownList_Search.DataValueField = "Description";
				DropDownList_Search.DataBind();
				if (!String.IsNullOrEmpty(sDefaulTxt))
				    DropDownList_Search.Items.Insert(0, new ListItem(sDefaulTxt, ""));

				if (searchUid.Length > 0)
				    DropDownList_Search.SelectedValue = searchUid;
				else
				    searchUid = DropDownList_Search.SelectedValue;
            }
			*/

            if (showDeleted.ToLower().Equals("true"))
                CheckBox_Show_Deleted.Checked = true;

            // Uc
            sidebarMenu1.currentCmsSubSections = "AdminCmsResources";
			sidebarMenu1.currentCmsSubSections_SectionUri = sectionUri;
            sidebarMenu1.currentCmsUserSession = currentCmsUserSession;

            //
            Repeater_List_Bind();
        }

        /* Header Click */
        protected void LinkButton_Column_Header_Click(object sender, EventArgs e)
        {
            LinkButton myLinkButton = (LinkButton)sender;
            if (myLinkButton.CommandArgument.Equals(listOrderFied))
            {
                //Stesso Campo, cambio solo la direction
                if (listOrderAscendig.ToLower().Equals("asc"))
                    listOrderAscendig = "desc";
                else
                    listOrderAscendig = "asc";
            }
            else
            {
                //Altro Campo
                listOrderFied = myLinkButton.CommandArgument;
                if (listOrderAscendig.ToLower().Equals("asc"))
                    listOrderAscendig = "desc";
                else
                    listOrderAscendig = "asc";
            }
            listPageNumber = 0;

            //
            Repeater_List_Bind();
        }
        protected void LinkButton_Column_Header_AddNew_Click(object sender, EventArgs e)
        {
            Response.Redirect(getCurrentDetailUrl());
        }

        /*List Action Click */
        protected void LinkButton_Edit_Click(object sender, EventArgs e)
        {
            LinkButton myImageButton = (LinkButton)sender;
            String sUid = myImageButton.CommandArgument;

            //
            Response.Redirect(getCurrentDetailUrl(sUid));
        }
        protected void LinkButton_Title_Click(object sender, EventArgs e)
        {
            LinkButton myImageButton = (LinkButton)sender;
            String sUid = myImageButton.CommandArgument;

            //
            Response.Redirect(getCurrentDetailUrl(sUid));
        }        
        protected void LinkButton_Copy_Click(object sender, EventArgs e)
        {
            LinkButton myImageButton = (LinkButton)sender;
            String sUid = myImageButton.CommandArgument;

            //
            Response.Redirect(getCurrentDetailUrlCopy(sUid));
        }

        /* Paging */
        protected void Button_Paging_Click(object sender, EventArgs e)
        {
            LinkButton myButton = (LinkButton)sender;
            listPageSize = System.Convert.ToInt32(myButton.CommandArgument);
            listPageNumber = 0;

            //
            Repeater_List_Bind();
        }
        protected void LinkButton_List_Pager_Click(object sender, EventArgs e)
        {
            LinkButton myLinkbutton = (LinkButton)sender;
            if (myLinkbutton != null)
            {
                listPageNumber = Convert.ToInt32(myLinkbutton.CommandArgument.ToString());
                Repeater_List_Bind();
            }
        }
		protected void Repeater_List_Pager_Bind(Paging _objPds)
        {
		    //
            liPagerSize10.Attributes.Remove("class");
            liPagerSize25.Attributes.Remove("class");
            liPagerSize50.Attributes.Remove("class");
            liPagerSize5000.Attributes.Remove("class");
			//
            HtmlControl myliPagerSize = (HtmlControl)FindControl("liPagerSize" + _objPds.PageSize);
            if ((myliPagerSize != null) && (listPageSize.Equals(_objPds.PageSize)))
                myliPagerSize.Attributes.Add("class", "active"); 

			//
            if (_objPds.TotalPages > 1)
            {
                ul_List_Pager.Visible = true;
				Panel_Pager_Find.Visible = true;
                LinkButton_List_Pager_End.CommandArgument = (_objPds.TotalPages - 1).ToString();
                LinkButton_List_Pager_End.ToolTip = "go to page " + (_objPds.TotalPages - 1).ToString();

                LinkButton_List_Pager_Start.CommandArgument = "0";
                LinkButton_List_Pager_Start.ToolTip = "go to page 1";

                if (_objPds.PageNumber > 0)
                {
                    LinkButton_List_Pager_Prev.CommandArgument = (_objPds.PageNumber - 1).ToString();
                    LinkButton_List_Pager_Prev.ToolTip = "go to page " + (_objPds.PageNumber - 1).ToString();
                    LinkButton_List_Pager_Prev.Enabled = true;
                }
                else
                {
                    LinkButton_List_Pager_Prev.Enabled = false;
                }

                if (_objPds.PageNumber < _objPds.TotalPages - 1)
                {
                    LinkButton_List_Pager_Next.CommandArgument = (_objPds.PageNumber + 1).ToString();
                    LinkButton_List_Pager_Next.ToolTip = "go to page " + (_objPds.PageNumber + 1).ToString();
                    LinkButton_List_Pager_Next.Enabled = true;
                }
                else
                {
                    LinkButton_List_Pager_Next.Enabled = false;
                }

				//
                RangeValidator_TextBox_Pager_Find.MinimumValue = "1";
                RangeValidator_TextBox_Pager_Find.MaximumValue = (_objPds.TotalPages ).ToString();           

                ArrayList pages = new ArrayList();
                Int32 nStart = _objPds.PageNumber;
                Int32 nEnd = _objPds.TotalPages;

                //
                if (_objPds.TotalPages > 10)
                {
                    //
                    if (_objPds.PageNumber <= 5)
                        nStart = 1;

                    if (_objPds.PageNumber > 5)
                        nStart = _objPds.PageNumber - 5;

                    if (_objPds.PageNumber < _objPds.TotalPages - 9)
                        nEnd = nStart + 9;
                    else
                        nEnd = _objPds.TotalPages;

                }
                else
                {
                    nStart = 1;
                    nEnd = _objPds.TotalPages;
                }


                for (int i = nStart; i <= nEnd; i++)
                    pages.Add((i).ToString());

                Repeater_List_Pager.Visible = true;
                Repeater_List_Pager.DataSource = pages;
                Repeater_List_Pager.ItemDataBound += new RepeaterItemEventHandler(Repeater_List_Pager_ItemDataBound);
                Repeater_List_Pager.DataBind();
            }
            else
            {
                ul_List_Pager.Visible = false;
				Panel_Pager_Find.Visible = false;
            }
        }
        protected void Repeater_List_Pager_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;

            if (e.Item.ItemType == ListItemType.Item | e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlControl myliPager = (HtmlControl)e.Item.FindControl("liPager");
                if ((listPageNumber + 1).ToString().Equals(e.Item.DataItem))
                    myliPager.Attributes.Add("class", " active");

                LinkButton myLinkButton_List_Pager = (LinkButton)e.Item.FindControl("LinkButton_List_Pager");
                myLinkButton_List_Pager.Text = (e.Item.DataItem).ToString();
                myLinkButton_List_Pager.ToolTip = "go to page " + (e.Item.DataItem);
                myLinkButton_List_Pager.CommandArgument = (System.Convert.ToInt32(e.Item.DataItem) - 1).ToString();
            }
        }
		protected void LinkButton_GoToPage_Click(object sender, EventArgs e)
        {
            LinkButton myLinkbutton = (LinkButton)sender;
            if (myLinkbutton != null)
            {
                if (objLibMath.isNumber(TextBox_Pager_Find.Text))
                {
                    listPageNumber = System.Convert.ToInt32(TextBox_Pager_Find.Text);
                    if (listPageNumber > 1)
                        listPageNumber = listPageNumber - 1;
                    Repeater_List_Bind();
                }
            }
        }
        /* Action List */
        protected void CheckBox_Row_OnCheckedChanged(object sender, EventArgs e)
        {
            CheckBox myCheckBox = (CheckBox)sender;
            if (myCheckBox != null)
            {
                String sIdRow = myCheckBox.Attributes["value"].ToString();
                if (!String.IsNullOrEmpty(sIdRow))
                {
                    if (myCheckBox.Checked)
                    {
                        // Selezionato, lo inserisco se non c'è
                        if (!hashtableSelected.ContainsKey(sIdRow))
                            hashtableSelected.Add(sIdRow, "1");
                    }
                    else
                    {
                        // Non selezionato, lo elimino se c'è
                        if (hashtableSelected.ContainsKey(sIdRow))
                            hashtableSelected.Remove(sIdRow);
                    }
                }
            }
        }        

		/* Order */
        protected void Button_ReorderRecord_Click(object sender, EventArgs e)
        {
            //
            listOrderFied = "Ord";
            listOrderAscendig = "asc";

            //
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SetDraggableRows", "$(document).ready(function () {SetDraggableRows();});", true);

            //
            Repeater_List_Bind(false);
        }
        protected void Button_ApplyReorder_Click(object sender, EventArgs e)
        {
            //
            ScriptManager.RegisterStartupScript(this, this.GetType(), "DisableDraggableRows", "$(document).ready(function () {DisableDraggableRows();});", true);

            //
            Repeater_List_Bind();
        }
        protected void Button_RequiredPublishOrder_Click(object sender, EventArgs e)
        {
            //           
        }				

        /* Search Button */
        protected void LinkButton_Search_Click(object sender, EventArgs e)
        {
            // Filter
            if (!String.IsNullOrEmpty(DropDownList_Search.SelectedValue))
                searchUid = DropDownList_Search.SelectedValue;
            else
                searchUid = null;
            if (!String.IsNullOrEmpty(TextBox_Search.Text))
                searchText = TextBox_Search.Text;
            else
                searchText = null;

            //
            listPageNumber = 0;

            //
            Repeater_List_Bind();
        }
        protected void DropDownList_Search_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(DropDownList_Search.SelectedValue))
                searchUid = DropDownList_Search.SelectedValue;
            else
                searchUid = null;

            //
            listPageNumber = 0;

            //
            Repeater_List_Bind();
        }		
        protected void CheckBox_Show_Deleted_Click(object sender, EventArgs e)
        {
            //
            listPageNumber = 0;

            //
            Repeater_List_Bind();
        }
		#endregion

        #region Add Method
        protected void Repeater_List_Bind()
        {
            //
            Repeater_List_Bind(true);
        }
        protected void Repeater_List_Bind(Boolean bPaging)
        {
            // Request List
            CmsResourcesListOptions rq = new CmsResourcesListOptions();

            //
			rq.uid_CmsNlsContext = currentCmsUserSession.currentCmsNlsContext.Uid;            

            if (bPaging)
            {
                rq.paging = new Paging();
                rq.paging.PageSize = listPageSize;
                rq.paging.PageNumber = listPageNumber;
            }

			// Include deleted records
			rq.includeDeleted = CheckBox_Show_Deleted.Checked;
			

            if (!String.IsNullOrEmpty(searchText))
                rq.searchText = searchText;
            else
                rq.searchText = null;

            if (listOrderFied.Length > 0)
            {
                rq.sortBy = listOrderFied;
                if (listOrderAscendig.ToLower().Equals("asc"))
                    rq.sortAscending = true;
                else
                    rq.sortAscending = false;
            }

            // Filtro per sezione
            if (!String.IsNullOrEmpty(DropDownList_Search.SelectedValue))
			{
                searchUid = DropDownList_Search.SelectedValue;
                // rq.filterFieldName = System.Convert.ToInt32(searchUid);
			}

            // List Response
            CmsResourcesListResponse responseList = objCmsBoDataLibs.CmsResourcesListForCms(header, rq);
            if (responseList != null)
            {
                //
                Repeater_List_Column.Visible = true;
                Repeater_List_Column.DataSource = responseList.items;
                Repeater_List_Column.ItemDataBound += new RepeaterItemEventHandler(Repeater_List_Column_ItemDataBound);
                Repeater_List_Column.DataBind();

                //
                if (bPaging)
                    Repeater_List_Pager_Bind(responseList.paging);
            }
            else
            {
                //
                Repeater_List_Column.Visible = false;
                Repeater_List_Pager.Visible = false;
            }
        }
        protected void Repeater_List_Column_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
            {
                //
                CmsResources currentObj = (CmsResources)e.Item.DataItem;

                //
                String sUid = currentObj.Uid.ToString();

				// CheckBox
                HtmlControl mycheckboxSel = (HtmlControl)e.Item.FindControl("checkboxSel");
                mycheckboxSel.Attributes.Add("data-uid", sUid);

                // Row
                HtmlControl mytrRow = (HtmlControl)e.Item.FindControl("trRow");

				//Edit
                HyperLink myHyperLink_Edit = (HyperLink)e.Item.FindControl("HyperLink_Edit");
                myHyperLink_Edit.NavigateUrl = getCurrentDetailUrl(sUid);

				// Delete
                HyperLink myHyperLink_Delete = (HyperLink)e.Item.FindControl("HyperLink_Delete");
                myHyperLink_Delete.Attributes.Add("data-url", "action.aspx");
                myHyperLink_Delete.Attributes.Add("data-content", "CmsResources");
                myHyperLink_Delete.Attributes.Add("data-uid", sUid);                
                myHyperLink_Delete.Attributes.Add("data-returnUrl", getCurrentListUrl());
                myHyperLink_Delete.Attributes.Add("data-action", "delete");						

                // Enable/Disable
                HyperLink myHyperLink_EnableDisable = (HyperLink)e.Item.FindControl("HyperLink_EnableDisable");
                myHyperLink_EnableDisable.Attributes.Add("data-url", "action.aspx");
                myHyperLink_EnableDisable.Attributes.Add("data-content", "CmsResources");
                myHyperLink_EnableDisable.Attributes.Add("data-uid", sUid);
                myHyperLink_EnableDisable.Attributes.Add("data-returnUrl", getCurrentListUrl());
                if (currentObj.StatusFlag.Equals((int)EnumCmsContent.Enabled))
                {
                    myHyperLink_EnableDisable.Attributes.Add("data-action", "disable");
                    myHyperLink_EnableDisable.CssClass += " success";
					//mytrRow.Attributes.Add("class", "active");
                }
                else if (currentObj.StatusFlag.Equals((int)EnumCmsContent.Disabled))
                {
                    myHyperLink_EnableDisable.Attributes.Add("data-action", "enable");
                    myHyperLink_EnableDisable.CssClass += " danger";
                }
                else if (currentObj.StatusFlag.Equals((int)EnumCmsContent.Deleted))
                {
                    myHyperLink_EnableDisable.Attributes.Add("data-action", "enable");
                    myHyperLink_EnableDisable.CssClass += " danger";
					//mytrRow.Attributes.Add("class", "danger");
                }

                
                // Key
                String sKey = String.Empty;
                if (currentObj.Key != null)
                    sKey = currentObj.Key.ToString();
                Literal myLiteral_Column_Key = (Literal)e.Item.FindControl("Literal_Column_Key");
                myLiteral_Column_Key.Text = sKey;
                
                // Description
                String sDescription = String.Empty;
                if (currentObj.Description != null)
                    sDescription = currentObj.Description.ToString();
                Literal myLiteral_Column_Description = (Literal)e.Item.FindControl("Literal_Column_Description");
                myLiteral_Column_Description.Text = sDescription;
                
                // Note
                String sNote = currentObj.Note;
                sNote = objLibString.sStripHTML(sNote);
                Literal myLiteral_Column_Note = (Literal)e.Item.FindControl("Literal_Column_Note");
                myLiteral_Column_Note.Text = objLibString.TroncaTesto(sNote, 100, true);

                
            }
        }
        #endregion

		#region Add Function
        protected String getCurrentDetailUrl()
        {
            return getCurrentDetailUrl("");
        }
        protected String getCurrentDetailUrl(String _uid)
        {
            String sReturn = String.Empty;

            if (_uid.Length > 0)
                sReturn = "details.aspx?Uid=" + _uid + "&section=" + section + "&pageNumber=" + listPageNumber + "&pageSize=" + listPageSize + "&orderFied=" + Server.UrlEncode(listOrderFied) + "&orderAscendig=" + listOrderAscendig.ToString() + "&searchText=" + Server.UrlEncode(searchText) + "&searchUid=" + searchUid + "&sectionUri=" + sectionUri + "&showDeleted=" + CheckBox_Show_Deleted.Checked.ToString();
            else
                sReturn = "details.aspx?section=" + section + "&pageNumber=" + listPageNumber + "&pageSize=" + listPageSize + "&orderFied=" + Server.UrlEncode(listOrderFied) + "&orderAscendig=" + listOrderAscendig.ToString() + "&searchText=" + Server.UrlEncode(searchText) + "&searchUid=" + searchUid + "&sectionUri=" + sectionUri + "&showDeleted=" + CheckBox_Show_Deleted.Checked.ToString();

            return sReturn;

        }
        protected String getCurrentDetailUrlCopy(String _uid)
        {
            String sReturn = String.Empty;

            //
            sReturn = "details.aspx?UidCopy=" + _uid + "&section=" + section + "&pageNumber=" + listPageNumber + "&pageSize=" + listPageSize + "&orderFied=" + Server.UrlEncode(listOrderFied) + "&orderAscendig=" + listOrderAscendig.ToString() + "&searchText=" + Server.UrlEncode(searchText) + "&searchUid=" + searchUid + "&sectionUri=" + sectionUri + "&showDeleted=" + CheckBox_Show_Deleted.Checked.ToString();

            return sReturn;

        }
        protected String getCurrentListUrl()
        {
            return "list.aspx?section=" + section + "&pageNumber=" + listPageNumber + "&pageSize=" + listPageSize + "&orderFied=" + Server.UrlEncode(listOrderFied) + "&orderAscendig=" + listOrderAscendig.ToString() + "&searchText=" + Server.UrlEncode(searchText) + "&searchUid=" + searchUid + "&sectionUri=" + sectionUri + "&showDeleted=" + CheckBox_Show_Deleted.Checked.ToString();
        }
        #endregion
    }
}

