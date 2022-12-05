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

namespace backOffice.adm.cmslabels
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
						CmsLabels updateObject = null;
						
						CmsLabelsItemResponse responseUserGet = objCmsBoDataLibs.CmsLabelsGetCms(header, sDataUid);
						updateObject = responseUserGet.item;
					
						if (updateObject != null)
						{
						    //
						    updateObject.StatusFlag = (int)EnumCmsContent.Deleted;

						    CmsLabelsItemResponse responseUpsert = objCmsBoDataLibs.CmsLabelsUpsert(header, updateObject);
						    if (responseUpsert.Success)							    
				                Response.Write("SUCCESS");
						}										
					}
					else if (sDataAction.ToLower().Equals("enable"))
					{
					    // Enable Status
						CmsLabels updateObject = null;
						
						CmsLabelsItemResponse responseUserGet = objCmsBoDataLibs.CmsLabelsGetCms(header, sDataUid);
						updateObject = responseUserGet.item;
						
						if (updateObject != null)
						{
						    //
						    updateObject.StatusFlag = (int)EnumCmsContent.Enabled;

						    CmsLabelsItemResponse responseUpsert = objCmsBoDataLibs.CmsLabelsUpsert(header, updateObject);
						    if (responseUpsert.Success)							    
				                Response.Write("SUCCESS");
						}								
					}
					else if (sDataAction.ToLower().Equals("disable"))
					{
					    // Disable Status
						CmsLabels updateObject = null;
						
						CmsLabelsItemResponse responseUserGet = objCmsBoDataLibs.CmsLabelsGetCms(header, sDataUid);
						updateObject = responseUserGet.item;						

						if (updateObject != null)
						{
						    //
						    updateObject.StatusFlag = (int)EnumCmsContent.Disabled;

						    CmsLabelsItemResponse responseUpsert = objCmsBoDataLibs.CmsLabelsUpsert(header, updateObject);
						    if (responseUpsert.Success)							    
				                Response.Write("SUCCESS");
						}												  
					}
					else if (sDataAction.ToLower().Equals("list"))
					{						
	
					}  	
					else if (sDataAction.ToLower().Equals("deleterelated"))
					{
	
					}
				}
			}			
            
        }
		#endregion

		#region Add Method

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
