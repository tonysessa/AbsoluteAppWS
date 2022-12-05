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
    public partial class CmsUploadImageCrop : System.Web.UI.UserControl
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

        public Int32 SelectWidth_Prev
        {
            get
            {
                if (ViewState["_SelectWidth_Prev"] == null)
                    return 0;
                else
                    return (Int32)ViewState["_SelectWidth_Prev"];
            }
            set { ViewState["_SelectWidth_Prev"] = value; }
        }
        public Int32 SelectHeight_Prev
        {
            get
            {
                if (ViewState["_SelectHeight_Prev"] == null)
                    return 0;
                else
                    return (Int32)ViewState["_SelectHeight_Prev"];
            }
            set { ViewState["_SelectHeight_Prev"] = value; }
        }
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
        //
        public Boolean isCrop
        {
            get
            {
                if (ViewState["_isCrop"] == null)
                    return false;
                else
                    return (Boolean)ViewState["_isCrop"];
            }
            set { ViewState["_isCrop"] = value; }
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

            if (HiddenField_UrlFile.Text.Length > 0)
                HttpFileUrl = HiddenField_UrlFile.Text;

            if (HiddenField_Parameter.Text.Length > 0)
                Parameter = HiddenField_Parameter.Text;

            if (HiddenField_ImageUrl.Text.Length > 0)
                ImageUrl = HiddenField_ImageUrl.Text;

            if (HiddenField_ImageUrl_Preview.Text.Length > 0)
                ImageUrl_Preview = HiddenField_ImageUrl_Preview.Text;

            if (HiddenField_ImageUrl_Ori.Text.Length > 0)
                ImageUrl_Ori = HiddenField_ImageUrl_Ori.Text;



            if (HiddenField_IsCrop.Text.Equals("true"))
                isCrop = true;

            //
            spacerDiv.Visible = showSpacer;
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

                // Resources
                int nCmsUploadImageCrop_ImageUrl_Width = System.Convert.ToInt32(currentCmsUserSession.GetGlobalResources(cmsLabelKey + ".Width", "300"));
                int nCmsUploadImageCrop_ImageUrl_Height = System.Convert.ToInt32(currentCmsUserSession.GetGlobalResources(cmsLabelKey + ".Height", "200"));
                int nCmsUploadImageCrop_ImageUrl_Width_Prev = System.Convert.ToInt32(currentCmsUserSession.GetGlobalResources(cmsLabelKey + "_Prev.Width", "200"));
                int nCmsUploadImageCrop_ImageUrl_Height_Prev = System.Convert.ToInt32(currentCmsUserSession.GetGlobalResources(cmsLabelKey + "_Prev.Height", "100"));
                Boolean bCmsUploadImageCrop_ImageUrl_HasCrop = System.Convert.ToBoolean(currentCmsUserSession.GetGlobalResources(cmsLabelKey + ".HasCrop", "true"));
                if (bCmsUploadImageCrop_ImageUrl_HasCrop)
                    SetCropArea(nCmsUploadImageCrop_ImageUrl_Width, nCmsUploadImageCrop_ImageUrl_Height, nCmsUploadImageCrop_ImageUrl_Width_Prev, nCmsUploadImageCrop_ImageUrl_Height_Prev);
                else
                    SetSize(nCmsUploadImageCrop_ImageUrl_Width, nCmsUploadImageCrop_ImageUrl_Height, nCmsUploadImageCrop_ImageUrl_Width_Prev, nCmsUploadImageCrop_ImageUrl_Height_Prev);
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
            buttonUpload.Attributes.Add("data-cropUrl", HiddenField_UrlFile.ClientID);
            buttonUploadFast.Attributes.Add("data-cropUrl", HiddenField_UrlFile.ClientID);
        }
        protected void LinkButton_Remove_Click(object sender, EventArgs e)
        {
            HttpFileUrl = "";
            ImageUrl = "";
            ImageUrl_Preview = "";
            ImageUrl_Ori = "";
            HiddenField_UrlFile.Text = "";
            HiddenField_Parameter.Text = "";
        }
        #endregion

        #region uc Public Method
        public void SetValue(String _ImageUrl_Preview, String _ImageUrl)
        {
            SetValue(_ImageUrl_Preview, _ImageUrl, "");
        }
        public void SetValue(String _ImageUrl_Preview, String _ImageUrl, String _ImageUrlOri)
        {
            if (!String.IsNullOrEmpty(_ImageUrl_Preview))
            {
                //
                HiddenField_ImageUrl_Preview.Text = _ImageUrl_Preview;

                //
                Image_Preview.ImageUrl = sStoragePublicBaseUrl + _ImageUrl_Preview;
                Image_Preview.CssClass = "";
                Image_Preview.Visible = true;

                //
                String sFileName = _ImageUrl_Preview;
                if (sFileName.LastIndexOf('/') > 0)
                    sFileName = sFileName.Substring(sFileName.LastIndexOf('/') + 1);
                Label_Filename.Text = sFileName;
            }

            if (!String.IsNullOrEmpty(_ImageUrl))
            {
                //
                HiddenField_ImageUrl.Text = _ImageUrl;
            }

            if (!String.IsNullOrEmpty(_ImageUrlOri))
            {
                //
                HiddenField_ImageUrl_Ori.Text = _ImageUrlOri;
            }

        }
        public void SetCropArea(Int32 _SelectWidth, Int32 _SelectHeight, Int32 _SelectWidth_Prev, Int32 _SelectHeight_Prev)
        {
            //
            isCrop = true;
            SelectWidth = _SelectWidth;
            SelectHeight = _SelectHeight;
            SelectWidth_Prev = _SelectWidth_Prev;
            SelectHeight_Prev = _SelectHeight_Prev;
            HiddenField_IsCrop.Text = "true";

            if ((SelectWidth.Equals("0")) || (SelectHeight.Equals("0")))
            {
                buttonUpload.Attributes["data-configuration"] = "true|true|" + SelectWidth_Prev.ToString() + "|" + _SelectHeight_Prev.ToString();
                buttonUploadFast.Attributes["data-configuration"] = "true|true|" + SelectWidth_Prev.ToString() + "|" + _SelectHeight_Prev.ToString();
                Literal_Label_Dimension.Text = "<br /><i>Min-Width: " + SelectWidth_Prev.ToString() + ", Min-Height:" + _SelectHeight_Prev.ToString() + "</i>";
            }
            else
            {
                buttonUpload.Attributes["data-configuration"] = "true|true|" + _SelectWidth.ToString() + "|" + SelectHeight.ToString();
                buttonUploadFast.Attributes["data-configuration"] = "true|true|" + _SelectWidth.ToString() + "|" + SelectHeight.ToString();
                Literal_Label_Dimension.Text = "<br /><i>Min-Width: " + SelectWidth.ToString() + ", Min-Height:" + SelectHeight.ToString() + "</i>";
            }
        }
        public void SetSize(Int32 _SelectWidth, Int32 _SelectHeight, Int32 _SelectWidth_Prev, Int32 _SelectHeight_Prev)
        {
            //
            isCrop = false;
            SelectWidth = _SelectWidth;
            SelectHeight = _SelectHeight;
            SelectWidth_Prev = _SelectWidth_Prev;
            SelectHeight_Prev = _SelectHeight_Prev;
            HiddenField_IsCrop.Text = "false";
            //
            buttonUpload.Attributes.Add("data-configuration", "false|false");
            buttonUploadFast.Attributes.Add("data-configuration", "false|false");

            if ((SelectWidth.Equals("0")) || (SelectHeight.Equals("0")))
                Literal_Label_Dimension.Text = "<br /><i>Min-Width: " + SelectWidth_Prev.ToString() + ", Min-Height:" + SelectHeight_Prev.ToString() + "</i>";
            else
                Literal_Label_Dimension.Text = "<br /><i>Min-Width: " + SelectWidth.ToString() + ", Min-Height:" + SelectHeight.ToString() + "</i>";
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
