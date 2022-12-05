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
    public partial class CmsLabel : System.Web.UI.UserControl
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
        public String Value
        {
            get
            {
                if (ViewState["_VALUE"] == null)
                    return "";
                else
                    return (String)ViewState["_VALUE"];
            }
            set { ViewState["_VALUE"] = value; }
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
        #endregion

        #region uc Method
        protected void Page_Load(object sender, EventArgs e)
        {
            //
            InitUC();

            //
            Value = Literal_Value.Text;


            spacerDiv.Visible = showSpacer;
        }
        protected void InitUC()
        {

            if (!String.IsNullOrEmpty(Title))
                Literal_Label.Text = Title;            
        }
        public void SetValue(String _Value)
        {
            if (!String.IsNullOrEmpty(_Value))
                Literal_Value.Text = _Value;
            else
                Literal_Value.Text = String.Empty;
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