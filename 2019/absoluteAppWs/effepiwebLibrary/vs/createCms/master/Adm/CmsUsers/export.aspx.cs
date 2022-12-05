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
//
using System.Linq;
using System.Linq.Dynamic;
using System.Runtime.Serialization;
using System.Collections.Generic;
// Excel Library
using NPOI;

namespace backOffice.cmsusers
{
    public partial class export : System.Web.UI.Page
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
        public String sStoragePublicBaseUrl = WebContext.getConfig("%.storagePublicBaseUrl").ToString();
        #endregion

        #region Property
        public CmsUserSession currentCmsUserSession = null;
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
				// Request List
				CmsUsersListOptions rq = new CmsUsersListOptions();

				//
				//rq.uid_CmsNlsContext = currentCmsUserSession.currentCmsNlsContext.Uid;                     

				// List Response
				CmsUsersListResponse responseList = objCmsBoDataLibs.CmsUsersListCms(header, rq);
				if (responseList != null)
				{
					String[] listColumns = new String[15];
                listColumns[0] = "Uid";
                listColumns[1] = "CreationDate";
                listColumns[2] = "Uid_CreationUser";
                listColumns[3] = "UpdateDate";
                listColumns[4] = "Uid_UpdateUser";
                listColumns[5] = "StatusFlag";
                listColumns[6] = "Name";
                listColumns[7] = "Surname";
                listColumns[8] = "Email";
                listColumns[9] = "Username";
                listColumns[10] = "Password";
                listColumns[11] = "DateLastLogin";
                listColumns[12] = "NumLogin";
                listColumns[13] = "Uid_CmsRoles";
                listColumns[14] = "Note";
               

				    NPOIExtended objXLS = new NPOIExtended();
				    List<object> myListGeneric = (responseList.items as IEnumerable<object>).Cast<object>().ToList();
				    objXLS.InitAndAddRowsObject(myListGeneric, listColumns,"CmsUsers", false);

					/*
					String[] listColumns = new String[15];
                listColumns[0] = "Uid";
                listColumns[1] = "CreationDate";
                listColumns[2] = "Uid_CreationUser";
                listColumns[3] = "UpdateDate";
                listColumns[4] = "Uid_UpdateUser";
                listColumns[5] = "StatusFlag";
                listColumns[6] = "Name";
                listColumns[7] = "Surname";
                listColumns[8] = "Email";
                listColumns[9] = "Username";
                listColumns[10] = "Password";
                listColumns[11] = "DateLastLogin";
                listColumns[12] = "NumLogin";
                listColumns[13] = "Uid_CmsRoles";
                listColumns[14] = "Note";

					objXLS.UpdateRow(listColumns, 0, "CmsUsers");
					*/

					//
				    objXLS.SaveXlsToWeb("CmsUsersExport.xls");
				}
			}
        }       
        #endregion
    }
}

