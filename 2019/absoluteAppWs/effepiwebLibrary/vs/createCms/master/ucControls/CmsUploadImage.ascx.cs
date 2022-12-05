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
    public partial class CmsUploadImage : System.Web.UI.UserControl
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
        /* Required Option */
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
        public Boolean ReadOnly
        {
            get
            {
                if (ViewState["_READONLY"] == null)
                    return false;
                else
                    return (Boolean)ViewState["_READONLY"];
            }
            set { ViewState["_READONLY"] = value; }
        }
        /* Image */
        public String ImageUrl
        {
            get
            {
                if (ViewState["_ImageUrl"] == null)
                    return "";
                else
                    return (String)ViewState["_ImageUrl"];
            }
            set { ViewState["_ImageUrl"] = value; }
        }
        public double ResizeWidth
        {
            get
            {
                if (ViewState["_ResizeWidth"] == null)
                    return 0;
                else
                    return (double)ViewState["_ResizeWidth"];
            }
            set { ViewState["_ResizeWidth"] = value; }
        }
        public double ResizeHeight
        {
            get
            {
                if (ViewState["_ResizeHeight"] == null)
                    return 0;
                else
                    return (double)ViewState["_ResizeHeight"];
            }
            set { ViewState["_ResizeHeight"] = value; }
        }
        /* Preview */
        public String ImageUrl_Preview
        {
            get
            {
                if (ViewState["_ImageUrl_Preview"] == null)
                    return "";
                else
                    return (String)ViewState["_ImageUrl_Preview"];
            }
            set { ViewState["_ImageUrl_Preview"] = value; }
        }
        public double ResizeWidthPrev
        {
            get
            {
                if (ViewState["_ResizeWidthPrev"] == null)
                    return 0;
                else
                    return (double)ViewState["_ResizeWidthPrev"];
            }
            set { ViewState["_ResizeWidthPrev"] = value; }
        }
        public double ResizeHeightPrev
        {
            get
            {
                if (ViewState["_ResizeHeightPrev"] == null)
                    return 0;
                else
                    return (double)ViewState["_ResizeHeightPrev"];
            }
            set { ViewState["_ResizeHeightPrev"] = value; }
        }
        /* Resize Option */
        public Boolean ResizeForce
        {
            get
            {
                if (ViewState["_ResizeForce"] == null)
                    return false;
                else
                    return (Boolean)ViewState["_ResizeForce"];
            }
            set { ViewState["_ResizeForce"] = value; }
        }
        //
        public Boolean showSpacer
        {
            get
            {
                if (ViewState["_showSpacer"] == null)
                    return true;
                else
                    return (Boolean)ViewState["_showSpacer"];
            }
            set { ViewState["_showSpacer"] = value; }
        }
        public String ImageUrl_Ori
        {
            get
            {
                if (ViewState["_ImageUrl_Ori"] == null)
                    return "";
                else
                    return (String)ViewState["_ImageUrl_Ori"];
            }
            set { ViewState["_ImageUrl_Ori"] = value; }
        }
        #endregion

        #region uc Method
        protected void Page_Load(object sender, EventArgs e)
        {

            // PostBack
            InitUC();

            if (TextBox_HttpUrlFile.Text.Length > 0)
                HttpFileUrl = TextBox_HttpUrlFile.Text;

            spacerDiv.Visible = showSpacer;

        }
        protected void InitUC()
        {
            //
            if (Title.Length > 0)
                Literal_Label.Text = Title;

            HyperLink_ShowRepository.NavigateUrl = "javascript:ShowRepository('" + TextBox_HttpUrlFile.ClientID + "');";
            HyperLink_Change.NavigateUrl = "javascript:ShowRepository('" + TextBox_HttpUrlFile.ClientID + "');";
            if (!String.IsNullOrEmpty(ImageUrl_Preview))
            {
                //
                HyperLink_ShowRepository.Visible = false;
                Panel_FilePreview.Attributes.Add("style", "display:block");          
            }
            else
            {
                HyperLink_ShowRepository.Visible = true;
                Panel_FilePreview.Attributes.Add("style", "display:none");
            }

            if (ReadOnly)
            {
                HyperLink_ShowRepository.Visible = false;
                LinkButton_Remove.Visible = false;
                HyperLink_Change.Visible = false;
                Literal_Or.Visible = false;
                iLinkButton_Remove.Visible = false;
            }
            
        }     
        protected void LinkButton_Remove_Click(object sender, EventArgs e)
        {
            HttpFileUrl = "";
            ImageUrl = "";
            ImageUrl_Preview = "";

            Image_Preview.ImageUrl = "~/img/logo.png";
            
            Panel_FilePreview.Attributes.Remove("style");
            Panel_FilePreview.Attributes.Add("style", "display:none;");

            Panel_Filename.Attributes.Remove("style");
            Panel_Filename.Attributes.Add("style", "display:none;");

            HyperLink_ShowRepository.Visible = true;
        }
        #endregion

        #region uc Public Method
        public void SetValue(String _ImageUrl_Preview, String _ImageUrl)
        {
            if (!String.IsNullOrEmpty(_ImageUrl_Preview))
            {
                CmsUserSession currentCmsUserSession = (CmsUserSession)Session["CmsUserSession"];

                //
                ImageUrl_Preview = _ImageUrl_Preview;
                Image_Preview.ImageUrl = sStoragePublicBaseUrl + currentCmsUserSession.currentCmsNlsContext.StorageFolder + "/" + _ImageUrl_Preview;
                Image_Preview.CssClass = "";

                //
                String sFileName = _ImageUrl_Preview;
                if (sFileName.LastIndexOf('/') > 0)
                    sFileName = sFileName.Substring(sFileName.LastIndexOf('/') + 1);
                Literal_FileName.Text = sFileName;
                Panel_Filename.Visible = true;
                Panel_Filename.Attributes.Add("style", "display:block;");
                Panel_FilePreview.Attributes.Add("style", "display:block;");

                //
                HyperLink_ShowRepository.Visible = false;
                               
            }

            if (!String.IsNullOrEmpty(_ImageUrl))
            {
                ImageUrl = _ImageUrl;                
            }
        }
        public void SetTitle(String _Value)
        {
            if (!String.IsNullOrEmpty(_Value))
            {
                Literal_Label.Text = _Value;
            }
        }
        public void SetValue(String _ImageUrl_Preview, String _ImageUrl, String _ImageOri)
        {
            if (!String.IsNullOrEmpty(_ImageUrl_Preview))
            {
                //
                CmsUserSession currentCmsUserSession = (CmsUserSession)Session["CmsUserSession"];

                //
                ImageUrl_Preview = _ImageUrl_Preview;
                Image_Preview.ImageUrl = sStoragePublicBaseUrl + currentCmsUserSession.currentCmsNlsContext.StorageFolder + "/" + ImageUrl_Preview;
                Image_Preview.CssClass = "";

                //
                String sFileName = _ImageUrl_Preview;
                if (sFileName.LastIndexOf('/') > 0)
                    sFileName = sFileName.Substring(sFileName.LastIndexOf('/') + 1);
                Literal_FileName.Text = sFileName;

                //Panel_Filename.Visible = true;
                //Panel_Filename.Attributes.Add("style", "display:block;");
                //Panel_FilePreview.Attributes.Add("style", "display:block;");
                Panel_Filename.Visible = true;
                Panel_Filename.Attributes.Add("style", "display:block");
                filesingleupload.Attributes.Add("style", "display:none");

                //
                HyperLink_ShowRepository.Visible = false;

            }

            if (!String.IsNullOrEmpty(_ImageUrl))
            {
                ImageUrl = _ImageUrl;
            }

            if (!String.IsNullOrEmpty(_ImageOri))
            {
                ImageUrl_Ori = _ImageOri;
            }
        }
        #endregion       
    }
}