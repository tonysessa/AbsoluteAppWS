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
    public partial class CmsRadioButtonList : System.Web.UI.UserControl
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
        public String DefaultText
        {
            get
            {
                if (ViewState["_DEFAULTTEXT"] == null)
                    return "";
                else
                    return (String)ViewState["_DEFAULTTEXT"];
            }
            set { ViewState["_DEFAULTTEXT"] = value; }
        }
        public String TextValue
        {
            get
            {
                if (ViewState["_TextValue"] == null)
                    return "";
                else
                    return (String)ViewState["_TextValue"];
            }
            set { ViewState["_TextValue"] = value; }
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
        public Object DataSource
        {
            get
            {
                if (ViewState["_DataSource"] == null)
                    return null;
                else
                    return (Object)ViewState["_DataSource"];
            }
            set { ViewState["_DataSource"] = value; }
        }
        public String DataSourceString
        {
            get
            {
                if (ViewState["_DataSourceString"] == null)
                    return "";
                else
                    return (String)ViewState["_DataSourceString"];
            }
            set { ViewState["_DataSourceString"] = value; }
        }
        public String DataTextField
        {
            get
            {
                if (ViewState["_DataTextField"] == null)
                    return "Key";
                else
                    return (String)ViewState["_DataTextField"];
            }
            set { ViewState["_DataTextField"] = value; }
        }
        public String DataValueField
        {
            get
            {
                if (ViewState["_DataValueField"] == null)
                    return "Value";
                else
                    return (String)ViewState["_DataValueField"];
            }
            set { ViewState["_DataValueField"] = value; }
        }
        public Boolean FirstEmpty
        {
            get
            {
                if (ViewState["_FirstEmpty"] == null)
                    return true;
                else
                    return (Boolean)ViewState["_FirstEmpty"];
            }
            set { ViewState["_FirstEmpty"] = value; }
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
        public String SelectedIndexChangedMethod
        {
            get
            {
                if (ViewState["_SelectedIndexChangedMethod"] == null)
                    return "";
                else
                    return (String)ViewState["_SelectedIndexChangedMethod"];
            }
            set { ViewState["_SelectedIndexChangedMethod"] = value; }
        }
        //
        public String currentJson
        {
            get
            {
                if (ViewState["_currentJson"] == null)
                    return "";
                else
                    return (String)ViewState["_currentJson"];
            }
            set { ViewState["_currentJson"] = value; }
        }
        //
        public String[] DataSourceValue
        {
            get
            {
                if (ViewState["_DataSourceValue"] == null)
                    return null;
                else
                    return (String[])ViewState["_DataSourceValue"];
            }
            set { ViewState["_DataSourceValue"] = value; }
        }
        //
        public String[] DataSourceText
        {
            get
            {
                if (ViewState["_DataSourceText"] == null)
                    return null;
                else
                    return (String[])ViewState["_DataSourceText"];
            }
            set { ViewState["_DataSourceText"] = value; }
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
            Value = RadioButton_List.SelectedValue;

            if (RadioButton_List.SelectedItem != null)
                TextValue = RadioButton_List.SelectedItem.Text;

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

            if (!String.IsNullOrEmpty(DataSourceString))
            {
                try
                {
                    if (DataSourceString.Contains(";"))
                    {
                        //Ho un datasource chiave-Valore
                        String[] sList = DataSourceString.Split('|');

                        currentJson += "[";
                        Dictionary<String, String> dictionarySource = new Dictionary<String, String>();
                        for (int i = 0; i < sList.Length; i++)
                        {
                            String[] sListItem = sList[i].Split(';');
                            if (sListItem.Length == 2)
                                currentJson += "{\"text\":\"" + sListItem[0] + "\",\"value:\"" + sListItem[1] + "\"}";
                        }
                        currentJson += "]";
                    }
                    else
                    {
                        String[] sList = DataSourceString.Split('|');

                        currentJson += "[";
                        Dictionary<String, String> dictionarySource = new Dictionary<String, String>();
                        for (int i = 0; i < sList.Length; i++)
                            currentJson += "{\"text\":\"" + sList[i] + "\",\"value:\"" + sList[i] + "\"}";
                        currentJson += "]";
                    }
                }
                catch
                {

                }
            }

        }
        public void Bind()
        {
            if (DataSource is Object)
            {
                try
                {
                    //
                    RadioButton_List.DataSource = DataSource;
                    RadioButton_List.DataTextField = DataTextField;
                    RadioButton_List.DataValueField = DataValueField;
                    RadioButton_List.DataBind();
                }
                catch
                {

                }
            }
            else if ((DataSourceText != null) && (DataSourceText.Length.Equals(DataSourceValue.Length)))
            {
                try
                {
                    Dictionary<String, String> dictionarySource = new Dictionary<String, String>();
                    for (int i = 0; i < DataSourceValue.Length; i++)
                        dictionarySource.Add(DataSourceText[i], DataSourceValue[i]);

                    RadioButton_List.DataSource = dictionarySource;
                    RadioButton_List.DataTextField = "Key";
                    RadioButton_List.DataValueField= "Value";
                    RadioButton_List.DataBind();
                }
                catch
                {

                }
            }
        }

        public void SetValue(String _Value)
        {
            try
            {
                if (!String.IsNullOrEmpty(_Value))
                {
                    RadioButton_List.SelectedValue = _Value;
                    Value = _Value;
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void SetDefaultText(String _Value)
        {
            try
            {
                DefaultText = _Value;
            }
            catch (Exception ex)
            {

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