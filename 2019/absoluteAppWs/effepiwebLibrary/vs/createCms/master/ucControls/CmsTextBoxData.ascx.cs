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
    public partial class CmsTextBoxData : System.Web.UI.UserControl
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
        public DateTime Value
        {
            get
            {
                if (ViewState["_VALUE"] == null)
                    return DateTime.MinValue;
                else
                    return (DateTime)ViewState["_VALUE"];
            }
            set { ViewState["_VALUE"] = value; }
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
        public DateTime DefaultValue
        {
            get
            {
                if (ViewState["_DEFAULTVALUE"] == null)
                    return DateTime.MinValue;
                else
                    return (DateTime)ViewState["_DEFAULTVALUE"];
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
            InitUC();

            //
            if (TextBoxDate_Value.Text.Length > 0)
            {
                String sValueDate = TextBoxDate_Value.Text;
                Value = objLibDate.StringToDate(sValueDate, "GMA", "/");
            }
            else
                Value = System.DateTime.MinValue;

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

            if (Required)
            {
                RequiredFieldValidator_TextBoxDate_Value.Enabled = true;
            }

            if (ReadOnly)
            {
                TextBoxDate_Value.ReadOnly = true;
            }

            TextBoxDate_Value.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
        }
        public void SetValue(DateTime _Value)
        {
            if (!DateTime.MinValue.Equals(_Value))
            {
                TextBoxDate_Value.Text = objLibDate.DateToString(_Value, "GMA", "/");
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