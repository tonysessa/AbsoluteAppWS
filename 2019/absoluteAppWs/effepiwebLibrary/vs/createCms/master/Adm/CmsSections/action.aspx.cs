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

namespace backOffice.cmssections
{
    public partial class action : System.Web.UI.Page
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
        public String sUploadDir = WebContext.getConfig("%.uploaddir").ToString();
        public String sStoragePublicBasePath = WebContext.getConfig("%.storagePublicBasePath").ToString();
		public String sCryptKey = WebContext.getConfig("%.cryptKey").ToString();
        #endregion

		#region Property
        public CmsUserSession currentCmsUserSession;
        public String Uid;
        public String UidRelated = String.Empty;
        //
        public String section = String.Empty;
        public String listPageNumber = String.Empty;
        public String listPageSize = String.Empty;
        public String listOrderFied = String.Empty;
        public String listOrderAscendig = String.Empty;
        public String searchText = String.Empty;
        public String searchUid = String.Empty;
        public String showDeleted = String.Empty;        
        public String contentTableRelated = String.Empty;
        public String sectionUri = String.Empty;
        #endregion

        #region Page Method
        protected void Page_Load(object sender, EventArgs e)
        {
			# region Check Login
            if (Session["CmsUserSession"] != null)
                currentCmsUserSession = (CmsUserSession)Session["CmsUserSession"];
            #endregion         

			if (currentCmsUserSession != null)
            {
				#region Header
				header.cmsUserUid = currentCmsUserSession.currentUid;
				header.cmsCmsNlsContext = currentCmsUserSession.currentCmsNlsContext;
				#endregion
			
				//
				String sDataContent = "" + Request["dataContent"];
				String sDataUid = "" + Request["dataUid"];
				String sDataUidRelated = "" + Request["dataUidRelated"];
				String sDataAction = Request["dataAction"];

				// Parametri in QueryString
				section = "" + Request["section"];
				searchText = "" + Request["searchText"];
				searchUid = "" + Request["searchUid"];
				sectionUri = Request["sectionUri"];
				showDeleted = "" + Request["showDeleted"];
				listPageSize = "" + Request["pageSize"];
				listPageNumber = "" + Request["pageNumber"];
				listOrderFied = "" + Request["orderFied"];
				listOrderAscendig = "" + Request["orderAscendig"];

				if (!String.IsNullOrEmpty(sDataUid))
				{

				    //
				    if (!String.IsNullOrEmpty(sDataUid))
				        Uid = sDataUid;				
					if (!String.IsNullOrEmpty(sDataUidRelated))
				        UidRelated = sDataUidRelated;

					//
					if (sDataAction.ToLower().Equals("delete"))
					{
					    // Delete Status
						CmsSections updateObject = null;
						
						CmsSectionsItemResponse responseUserGet = objCmsBoDataLibs.CmsSectionsGetCms(header, sDataUid);
						updateObject = responseUserGet.item;
					
						if (updateObject != null)
						{
						    //
						    updateObject.StatusFlag = (int)EnumCmsContent.Deleted;

						    CmsSectionsItemResponse responseUpsert = objCmsBoDataLibs.CmsSectionsUpsert(header, updateObject);
						    if (responseUpsert.Success)							    
				                Response.Write("SUCCESS");
						}										
					}
					else if (sDataAction.ToLower().Equals("enable"))
					{
					    // Enable Status
						CmsSections updateObject = null;
						
						CmsSectionsItemResponse responseUserGet = objCmsBoDataLibs.CmsSectionsGetCms(header, sDataUid);
						updateObject = responseUserGet.item;
						
						if (updateObject != null)
						{
						    //
						    updateObject.StatusFlag = (int)EnumCmsContent.Enabled;

						    CmsSectionsItemResponse responseUpsert = objCmsBoDataLibs.CmsSectionsUpsert(header, updateObject);
						    if (responseUpsert.Success)							    
				                Response.Write("SUCCESS");
						}								
					}
					else if (sDataAction.ToLower().Equals("disable"))
					{
					    // Disable Status
						CmsSections updateObject = null;
						
						CmsSectionsItemResponse responseUserGet = objCmsBoDataLibs.CmsSectionsGetCms(header, sDataUid);
						updateObject = responseUserGet.item;						

						if (updateObject != null)
						{
						    //
						    updateObject.StatusFlag = (int)EnumCmsContent.Disabled;

						    CmsSectionsItemResponse responseUpsert = objCmsBoDataLibs.CmsSectionsUpsert(header, updateObject);
						    if (responseUpsert.Success)							    
				                Response.Write("SUCCESS");
						}												  
					}
					else if (sDataAction.ToLower().Equals("list"))
					{						
	                    if (sDataContent.Equals("CmsSubSections")) 
                    {
                       Repeater_CmsSubSections_Bind();
                    }

					}  	
					else if (sDataAction.ToLower().Equals("deleterelated"))
					{
	                         if (sDataContent.Equals("CmsSubSections"))
                         {
                              // Delete Status
                              CmsSubSections updateObject = null;
                              if (!String.IsNullOrEmpty(sDataUidRelated))
                              {
                                  CmsSubSectionsItemResponse responseUserGet = objCmsBoDataLibs.CmsSubSectionsGetCms(header, sDataUidRelated);
                                  updateObject = responseUserGet.item;
                              
                                  if (updateObject != null)
                                  {
                                      //
                                      updateObject.StatusFlag = (int)EnumCmsContent.Deleted;
                              
                                      CmsSubSectionsItemResponse responseUpsert = objCmsBoDataLibs.CmsSubSectionsUpsert(header, updateObject);
                                      if (responseUpsert.Success)
                                          Repeater_CmsSubSections_Bind();
                                  }
                              }
                         }

					}
				}
			}			
            
        }
		#endregion

		#region Add Method
        protected void Repeater_CmsSubSections_Bind()
        {

            Panel_Related_CmsSubSections_List.Visible = true;
            if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel("Cms.HyperLink_Action.Text")))
               HyperLink_CmsSubSections_Action.Text = currentCmsUserSession.GetGlobalLabel("Cms.HyperLink_Action.Text");

            HyperLink_CmsSubSections_Action_Add.NavigateUrl = getCurrentDetailUrl(Uid.ToString()) + "&ContentTableRelated=CmsSubSections";

            if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel("Cms.HyperLink_Related_Action_Add.Text")))
               HyperLink_CmsSubSections_Action_Add.Text = currentCmsUserSession.GetGlobalLabel("Cms.HyperLink_Related_Action_Add.Text");

            if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel("Cms.HyperLink_Related_Action_Delete.Text")))
               HyperLink_CmsSubSections_Action_Delete.Text = currentCmsUserSession.GetGlobalLabel("Cms.HyperLink_Related_Action_Delete.Text");

            if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel("Cms.Button_ReorderRecord.Text")))
               HyperLink_CmsSubSections_ReorderRecord.Text = currentCmsUserSession.GetGlobalLabel("Cms.Button_ReorderRecord.Text");

            if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel("Cms.Button_ApplyReorder.Text")))
               HyperLink_CmsSubSections_ApplyReorder.Text = currentCmsUserSession.GetGlobalLabel("Cms.Button_ApplyReorder.Text");

            // Filtri per elenco
            CmsSubSectionsListOptions rq = new CmsSubSectionsListOptions();
            rq.uid_CmsSections = System.Convert.ToInt32(Uid);
            Boolean bOrderRelated =  true;
            if (bOrderRelated)
            {
                rq.sortBy = "Ord";
                rq.sortAscending = true;
                // Enable Sort
                Panel_Related_CmsSubSections_List.Attributes["class"] = "listSortable";
                HyperLink_CmsSubSections_ReorderRecord.Visible = true;
                HyperLink_CmsSubSections_ApplyReorder.Visible = true;
            }
            rq.statusFlag = (int)EnumCmsContent.Enabled;

            //
            CmsSubSectionsListResponse responseList = objCmsBoDataLibs.CmsSubSectionsListCms(header, rq);
            if (responseList.Success)
            {
                if (responseList != null)
                {
                    Repeater_CmsSubSections.DataSource = responseList.items;
                    Repeater_CmsSubSections.ItemDataBound += new RepeaterItemEventHandler(Repeater_CmsSubSections_ItemDataBound);
                    Repeater_CmsSubSections.DataBind();
                }
            }
        }
        protected void Repeater_CmsSubSections_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
            {
                String nUid_Related = ((CmsSubSections)e.Item.DataItem).Uid.ToString();
                String sTitolo = ((CmsSubSections)e.Item.DataItem).Title.ToString();
                //
                Panel myPanel_Card = (Panel)e.Item.FindControl("Panel_Card");
                myPanel_Card.Attributes.Add("data-uid", nUid_Related.ToString());
                if (UidRelated.Equals(nUid_Related))
                   myPanel_Card.CssClass += " relatedActive active";

                // CheckBox
                HtmlControl mycheckboxSel = (HtmlControl)e.Item.FindControl("CmsSubSections_checkboxSel");
                mycheckboxSel.Attributes.Add("data-uid", nUid_Related.ToString());

                /*
                String sImageUrl_Prev = ((CmsSubSections)e.Item.DataItem).ImageUrl_Prev;
                if (!String.IsNullOrEmpty(sImageUrl_Prev))
                    myPanel_Card.Attributes.Add("style", "background-image: url('" + sStoragePublicBaseUrl + sImageUrl_Prev + "')");
                else
                    myPanel_Card.Attributes.Add("style", "background-image: url('" + sCmsStartingpage + "img/nofoto_related.png')");
                */

                Literal myLiteral_Title = (Literal)e.Item.FindControl("Literal_Title");
                myLiteral_Title.Text = sTitolo;

                HyperLink myHyperLink_Update = (HyperLink)e.Item.FindControl("HyperLink_Update");
                myHyperLink_Update.NavigateUrl = getCurrentDetailUrl(Uid.ToString()) + "&ContentTableRelated=CmsSubSections&UidRelated=" + nUid_Related.ToString();
                if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel("Cms.HyperLink_Edit.ToolTip"))) 
                   myHyperLink_Update.ToolTip = currentCmsUserSession.GetGlobalLabel("Cms.HyperLink_Edit.ToolTip"); 

                HyperLink myHyperLink_Delete = (HyperLink)e.Item.FindControl("HyperLink_Delete");
                myHyperLink_Delete.Attributes.Add("data-url", "action.aspx");
                myHyperLink_Delete.Attributes.Add("data-content", "CmsSubSections");
                myHyperLink_Delete.Attributes.Add("data-returnUrl", getCurrentDetailUrl(Uid.ToString()));
                myHyperLink_Delete.Attributes.Add("data-action", "deleteRelated"); 
                myHyperLink_Delete.Attributes.Add("data-uid", Uid.ToString()); 
                myHyperLink_Delete.Attributes.Add("data-uidRelated", nUid_Related.ToString()); 
                if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel("Cms.HyperLink_Delete.ToolTip"))) 
                   myHyperLink_Delete.ToolTip = currentCmsUserSession.GetGlobalLabel("Cms.HyperLink_Delete.ToolTip"); 

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
        #endregion
    }
}
