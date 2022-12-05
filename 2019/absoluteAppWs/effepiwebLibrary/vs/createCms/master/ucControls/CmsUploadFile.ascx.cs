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
    public partial class CmsUploadFile : System.Web.UI.UserControl
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
        public String HttpFileUrl
        {
            get
            {
                if (ViewState["_HttpFileUrl"] == null)
                    return "";
                else
                    return (String)ViewState["_HttpFileUrl"];
            }
            set { ViewState["_HttpFileUrl"] = value; }
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
        /* select Crop*/
        public Int32 SelectWidth
        {
            get
            {
                if (ViewState["_SelectWidth"] == null)
                    return 0;
                else
                    return (Int32)ViewState["_SelectWidth"];
            }
            set { ViewState["_SelectWidth"] = value; }
        }
        public Int32 SelectHeight
        {
            get
            {
                if (ViewState["_SelectHeight"] == null)
                    return 0;
                else
                    return (Int32)ViewState["_SelectHeight"];
            }
            set { ViewState["_SelectHeight"] = value; }
        }
        public String FileUrl
        {
            get
            {
                if (ViewState["_FileUrl"] == null)
                    return "";
                else
                    return (String)ViewState["_FileUrl"];
            }
            set { ViewState["_FileUrl"] = value; }
        }
        //
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

            if (HiddenField_Parameter.Text.Length > 0)
                Parameter = HiddenField_Parameter.Text;

            if (HiddenField_HttpFileUrl.Text.Length > 0)
                HttpFileUrl = HiddenField_HttpFileUrl.Text;

            if (HiddenField_FileUrl.Text.Length > 0)
                FileUrl = HiddenField_FileUrl.Text;
            //
            spacerDiv.Visible = showSpacer;

            //
            if (!isVisible)
                Panel_Input.CssClass += " hidden";

            if (Session["CmsUserSession"] != null)
            {
                CmsUserSession currentCmsUserSession = (CmsUserSession)Session["CmsUserSession"];
                if (currentCmsUserSession != null)
                {
                    if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel("Cms.buttonUploadFast.Text")))
                        buttonUploadFast.InnerText = currentCmsUserSession.GetGlobalLabel("Cms.buttonUploadFast.Text");

                    if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel("Cms.Literal_AddFile.Text")))
                        Literal_AddFile.Text = currentCmsUserSession.GetGlobalLabel("Cms.Literal_AddFile.Text");
                }
            }
        }
        protected void InitUC()
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
                    Title += " (<a href=\"/cms/Adm/CmsLabels/details.aspx?key=" + cmsLabelKey + "\" target=\"_blank\"\">edit labels</a>)";

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
            buttonUpload.Attributes.Add("data-crop", HiddenField_Parameter.ClientID);
            buttonUploadFast.Attributes.Add("data-crop", HiddenField_Parameter.ClientID);

            // data-crop-imageUrl = l'url originale del file croppato
            buttonUpload.Attributes.Add("data-cropUrl", HiddenField_HttpFileUrl.ClientID);
            buttonUploadFast.Attributes.Add("data-cropUrl", HiddenField_HttpFileUrl.ClientID);

            // data-configuration = { hasCrop:false, selectFolder:true/false }
            buttonUpload.Attributes.Add("data-configuration", "false|false");
            buttonUploadFast.Attributes.Add("data-configuration", "false|false");

        }
        protected void LinkButton_Remove_Click(object sender, EventArgs e)
        {
            HttpFileUrl = "";
            FileUrl = "";
            HiddenField_HttpFileUrl.Text = "";
            HiddenField_Parameter.Text = "";
        }
        #endregion

        #region uc Public Method
        public void SetValue(String _UrlFile)
        {
            if (!String.IsNullOrEmpty(_UrlFile))
            {
                //
                FileUrl = _UrlFile;
                HiddenField_FileUrl.Text = FileUrl;

                //
                String sFileName = _UrlFile;
                if (sFileName.LastIndexOf('/') > 0)
                    sFileName = sFileName.Substring(sFileName.LastIndexOf('/') + 1);
                Label_Filename.Text = sFileName;
            }
        }
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