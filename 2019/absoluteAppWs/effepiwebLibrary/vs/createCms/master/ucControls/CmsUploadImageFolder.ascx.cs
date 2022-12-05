using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
// Support
using Support.db;
using Support.CmsFunction;
using Support.Library;
using Support.Web;
using backOffice.uc;

namespace backOffice.ucControls
{
    public partial class CmsUploadImageFolder : System.Web.UI.UserControl
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
        protected WebContext wctx = null;
        #endregion

        #region Parametri
        protected String sCmsStartingpage = WebContext.getConfig("%.cmsStartingpage").ToString();
        protected String sStoragePublicBaseUrl = WebContext.getConfig("%.storagePublicBaseUrl").ToString();
        #endregion

        #region Property
        public String Title
        {
            get
            {
                if (ViewState["_TITLE"] == null)
                    return "";
                else
                    return (String)ViewState["_TITLE"];
            }
            set { ViewState["_TITLE"] = value; }
        }
        public String Info
        {
            get
            {
                if (ViewState["_Info"] == null)
                    return "";
                else
                    return (String)ViewState["_Info"];
            }
            set { ViewState["_Info"] = value; }
        }
        public String MasterContent
        {
            get
            {
                if (ViewState["_MasterContent"] == null)
                    return "";
                else
                    return (String)ViewState["_MasterContent"];
            }
            set { ViewState["_MasterContent"] = value; }
        }
        public String UploadDir
        {
            get
            {
                if (ViewState["_UploadDir"] == null)
                    return "";
                else
                    return (String)ViewState["_UploadDir"];
            }
            set { ViewState["_UploadDir"] = value; }
        }
        public String PathUrl
        {
            get
            {
                if (ViewState["_PathUrl"] == null)
                    return "";
                else
                    return (String)ViewState["_PathUrl"];
            }
            set { ViewState["_PathUrl"] = value; }
        }

        public String Parameter
        {
            get
            {
                if (ViewState["_Parameter"] == null)
                    return "";
                else
                    return (String)ViewState["_Parameter"];
            }
            set { ViewState["_Parameter"] = value; }
        }
        public Boolean Required
        {
            get
            {
                if (ViewState["_REQUIRED"] == null)
                    return false;
                else
                    return (Boolean)ViewState["_REQUIRED"];
            }
            set { ViewState["_REQUIRED"] = value; }
        }       
        //
        public Boolean showSpacer
        {
            get
            {
                if (ViewState["_showSpacer"] == null)
                    return false;
                else
                    return (Boolean)ViewState["_showSpacer"];
            }
            set { ViewState["_showSpacer"] = value; }
        }
        public Boolean isVisible
        {
            get
            {
                if (ViewState["_hidden"] == null)
                    return true;
                else
                    return (Boolean)ViewState["_hidden"];
            }
            set { ViewState["_hidden"] = value; }
        }
        public String cmsLabelKey
        {
            get
            {
                if (ViewState["_cmsLabelKey"] == null)
                    return "";
                else
                    return (String)ViewState["_cmsLabelKey"];
            }
            set { ViewState["_cmsLabelKey"] = value; }
        }
        #endregion

        #region uc Method
        protected void Page_Load(object sender, EventArgs e)
        {

            //
            // PostBack
            InitUC();

            if (HiddenField_UrlFolder.Text.Length > 0)
                PathUrl = HiddenField_UrlFolder.Text;

            if (HiddenField_Parameter.Text.Length > 0)
                Parameter = HiddenField_Parameter.Text;
            //
            spacerDiv.Visible = showSpacer;
                        if (!isVisible)
                Panel_Input.CssClass += " hidden";

        }
        public void InitUC()
        {
            #region CmsLabes
            if (Session["CmsUserSession"] != null)
            {
                CmsUserSession currentCmsUserSession = (CmsUserSession)Session["CmsUserSession"];

                if (!String.IsNullOrEmpty(cmsLabelKey))
                    Title = currentCmsUserSession.GetGlobalLabel(cmsLabelKey);
                if (String.IsNullOrEmpty(Title))
                    Title = cmsLabelKey.Substring(cmsLabelKey.LastIndexOf(".") + 1);
                if ((currentCmsUserSession.currentCmsRoles.Uriname.Equals("ADMIN")) && (!String.IsNullOrEmpty(cmsLabelKey)))
                    Title += " (<a href=\"/cms/Adm/CmsLabels/details.aspx?key=" + cmsLabelKey + "\" target=\"_blank\"\">edit labels</a> or <a href=\"/cms/adm/CmsResources/details.aspx?key=" + cmsLabelKey + "\" target=\"_blank\"\">edit value</a>)";

                if (!String.IsNullOrEmpty(cmsLabelKey))
                    Info = currentCmsUserSession.GetGlobalLabelNote(cmsLabelKey);                
            }
            #endregion

            //
            if (Title.Length > 0)
                Literal_Label.Text = Title;

            if (!String.IsNullOrEmpty(Info))
            {
                HyperLink_InfoContent.Visible = true;
                Panel_InfoContent.Visible = true;
                Literal_InfoTitle.Text = Title;
                Literal_InfoContent.Text = Info;
            }

            if (!String.IsNullOrEmpty(MasterContent))
            {
                HyperLink_MasterContent.Visible = true;
                Panel_MasterContent.Visible = true;
                Literal_MasterContent.Text = MasterContent;
            }

            //
            // data-crop = id dell'elemento dove inserire i dati di crop separati da pipe w|h|width|height            
            buttonUploadFolder.Attributes.Add("data-folder", HiddenField_Parameter.ClientID);

            // data-crop-imageUrl = l'url originale del file croppato
            buttonUploadFolder.Attributes.Add("data-folderUrl", HiddenField_UrlFolder.ClientID);
        }        
        #endregion

        #region uc Public Method
        public void SetTitle(String _Value)
        {
            if (!String.IsNullOrEmpty(_Value))
            {
                Literal_Label.Text = _Value;
            }
        }
        #endregion       
    }
}