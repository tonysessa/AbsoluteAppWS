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
    public partial class CmsTextBoxNumberFloat : System.Web.UI.UserControl
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
        public Double? Value
        {
            get
            {
                if (ViewState["_VALUE"] == null)
                    return 0;
                else
                    return (Double)ViewState["_VALUE"];
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
        public String DefaultValue
        {
            get
            {
                if (ViewState["_DEFAULTVALUE"] == null)
                    return "";
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
        public String ValidationGroup
        {
            get
            {
                if (ViewState["_VALIDATIONGROUP"] == null)
                    return "";
                else
                    return (String)ViewState["_VALIDATIONGROUP"];
            }
            set { ViewState["_VALIDATIONGROUP"] = value; }
        }
        public String ValidationExpression
        {
            get
            {
                if (ViewState["_VALIDATIONEXPRESSION"] == null)
                    return "";
                else
                    return (String)ViewState["_VALIDATIONEXPRESSION"];
            }
            set { ViewState["_VALIDATIONEXPRESSION"] = value; }
        }
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
        #endregion

        #region uc Method
        protected void Page_Load(object sender, EventArgs e)
        {
            //
            InitUC();

            //
            if (!String.IsNullOrEmpty(TextBoxNumber_Value.Text))
            {
                String sValue = TextBoxNumber_Value.Text;
                sValue = sValue.Replace(".", objLibMath.GetDecimaSimbol());
                Double nValue = System.Convert.ToDouble(sValue);
                Value = nValue;

            }
            else
            {
                Value = 0;
            }

            spacerDiv.Visible = showSpacer;
        }
        protected void InitUC()
        {

            if (!String.IsNullOrEmpty(Title))
                Literal_Label.Text = Title;

            if (MaxLength > 0)
                TextBoxNumber_Value.MaxLength = MaxLength;

            if (Required)
                RequiredFieldValidator_TextBoxNumber_Value.Enabled = true;

            if (!String.IsNullOrEmpty(CssClass))
                TextBoxNumber_Value.CssClass += " " + CssClass;

            if (!String.IsNullOrEmpty(ValidationExpression))
                RegularExpressionValidator_TextBoxNumber_Value.ValidationExpression = ValidationExpression;

            TextBoxNumber_Value.Attributes.Add("onkeydown", "return (event.keyCode!=13);");

            if (!String.IsNullOrEmpty(ValidationGroup))
                RegularExpressionValidator_TextBoxNumber_Value.ValidationGroup = ValidationGroup;
        }
        public void SetValue(Double? _Value)
        {
            if (_Value > 0)
            {
                TextBoxNumber_Value.Text = _Value.ToString().Replace(objLibMath.GetDecimaSimbol(), ".");
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