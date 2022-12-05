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

namespace backOffice.ucControls
{
    public partial class CmsTextBoxLink : System.Web.UI.UserControl
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
        public String sCmsStartingpage = WebContext.getConfig("%.cmsStartingpage").ToString();
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
        public String CssClass
        {
            get
            {
                if (ViewState["_CSSCLASS"] == null)
                    return "";
                else
                    return (String)ViewState["_CSSCLASS"];
            }
            set { ViewState["_CSSCLASS"] = value; }
        }
        public String Value_Title
        {
            get
            {
                if (ViewState["_Value_Title"] == null)
                    return null;
                else
                    return (String)ViewState["_Value_Title"];
            }
            set { ViewState["_Value_Title"] = value; }
        }
        public String Value_Url
        {
            get
            {
                if (ViewState["_Value_Url"] == null)
                    return null;
                else
                    return (String)ViewState["_Value_Url"];
            }
            set { ViewState["_Value_Url"] = value; }
        }
        public String Value_Target
        {
            get
            {
                if (ViewState["_Value_Target"] == null)
                    return null;
                else
                    return (String)ViewState["_Value_Target"];
            }
            set { ViewState["_Value_Target"] = value; }
        }
        public String DefaultValue
        {
            get
            {
                if (ViewState["_DEFAULTVALUE"] == null)
                    return null;
                else
                    return (String)ViewState["_DEFAULTVALUE"];
            }
            set { ViewState["_DEFAULTVALUE"] = value; }
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
        public Int32 MaxLength
        {
            get
            {
                if (ViewState["_MaxLength"] == null)
                    return 0;
                else
                    return (Int32)ViewState["_MaxLength"];
            }
            set { ViewState["_MaxLength"] = value; }
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
        //
        public Boolean ViewTitle
        {
            get
            {
                if (ViewState["_VIEWTITLE"] == null)
                    return true;
                else
                    return (Boolean)ViewState["_VIEWTITLE"];
            }
            set { ViewState["_VIEWTITLE"] = value; }
        }
        public Boolean ViewTarget
        {
            get
            {
                if (ViewState["_VIEWTARGET"] == null)
                    return true;
                else
                    return (Boolean)ViewState["_VIEWTARGET"];
            }
            set { ViewState["_VIEWTARGET"] = value; }
        }
        public Boolean ClearUrl
        {
            get
            {
                if (ViewState["_CLEARURL"] == null)
                    return true;
                else
                    return (Boolean)ViewState["_CLEARURL"];
            }
            set { ViewState["_CLEARURL"] = value; }
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
            #region Check Login
            CmsUserSession currentCmsUserSession = null;
            if (Session["CmsUserSession"] != null)
                currentCmsUserSession = (CmsUserSession)Session["CmsUserSession"];
            #endregion  

            //
            InitUC();

            // Set Value
            Value_Title = TextBox_Title.Text;
            if ((ClearUrl) && (currentCmsUserSession != null))
            {
                Value_Url = objCmsFunction.ClearLinkUrl(TextBox_Value.Text, currentCmsUserSession.currentCmsNlsContext.StartingPage);
            }
            else
            {
                Value_Url = TextBox_Value.Text;
            }

            Value_Target = DropDownList_Target.SelectedValue;

            spacerDiv.Visible = showSpacer;

            if (!isVisible)
                Panel_Input.CssClass += " hidden";

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

            if (!String.IsNullOrEmpty(Title))
                Literal_Title.Text = Title;

            if (MaxLength > 0)
                TextBox_Value.MaxLength = MaxLength;

            if (Required)
                RequiredFieldValidator_TextBox_Value.Enabled = true;

            if (!ViewTitle)
            {
                Panel_Title.Visible = false;
                //
                if (!ViewTarget)
                    Panel_Value_Content.CssClass = "col-md-12";
                else
                    Panel_Value_Content.CssClass = "col-md-10";

            }

            if (!ViewTarget)
                Panel_Target.Visible = false;

            if (!String.IsNullOrEmpty(CssClass))
                TextBox_Value.CssClass += " " + CssClass;

            TextBox_Title.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
            TextBox_Value.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
        }
        public void SetValue(String _Value_Title, String _Value_Url, String _Value_Target)
        {
            if (!String.IsNullOrEmpty(_Value_Title))
                TextBox_Title.Text = _Value_Title;

            if (!String.IsNullOrEmpty(_Value_Url))
                TextBox_Value.Text = _Value_Url;

            if (!String.IsNullOrEmpty(_Value_Target))
                DropDownList_Target.SelectedValue = _Value_Target;
        }
        #endregion
    }
}