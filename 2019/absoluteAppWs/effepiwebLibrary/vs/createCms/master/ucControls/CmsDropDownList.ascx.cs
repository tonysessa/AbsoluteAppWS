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
    public partial class CmsDropDownList : System.Web.UI.UserControl
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
        public String ValueText
        {
            get
            {
                if (ViewState["_ValueText"] == null)
                    return "";
                else
                    return (String)ViewState["_ValueText"];
            }
            set { ViewState["_ValueText"] = value; }
        }
        public String DefaultValue
        {
            get
            {
                if (ViewState["_DefaultValue"] == null)
                    return "";
                else
                    return (String)ViewState["_DefaultValue"];
            }
            set { ViewState["_DefaultValue"] = value; }
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

        public String ValidationGroup
        {
            get
            {
                if (ViewState["_ValidationGroup"] == null)
                    return "";
                else
                    return (String)ViewState["_ValidationGroup"];
            }
            set { ViewState["_ValidationGroup"] = value; }
        }
        #endregion

        #region uc Method
        protected void Page_Load(object sender, EventArgs e)
        {
            //
            Value = TextBox_Value.Text;
            ValueText = TextBox_TextValue.Text;

            InitUC();
            //

            if (!String.IsNullOrEmpty(SelectedIndexChangedMethod))
            {
                TextBox_Value.AutoPostBack = true;
                TextBox_Value.TextChanged += TextBox_Value_TextChanged;
            }

            RequiredFieldValidator_TextBox_Value.Enabled = Required;
            if (!String.IsNullOrEmpty(ValidationGroup))
                RequiredFieldValidator_TextBox_Value.ValidationGroup = ValidationGroup;

            Literal_FirstEmpty.Visible = FirstEmpty;

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
                if (DataSourceString.Contains(";"))
                {
                    //Ho un datasource chiave-Valore
                    String[] sList = DataSourceString.Split('|');

                    Dictionary<String, String> dictionarySource = new Dictionary<String, String>();
                    for (int i = 0; i < sList.Length; i++)
                    {
                        String[] sListItem = sList[i].Split(';');
                        if (sListItem.Length == 2)
                            dictionarySource.Add(sListItem[0], sListItem[1]);
                    }

                    Repeater_Option.DataSource = dictionarySource;
                    Repeater_Option.ItemDataBound += Repeater_JSonDataSourceValue_ItemDataBound;
                }
                else
                {
                    String[] sList = DataSourceString.Split('|');

                    Repeater_Option.DataSource = sList;
                    Repeater_Option.ItemDataBound += Repeater_DataSourceStringSimple_ItemDataBound;
                }

                if ((String.IsNullOrEmpty(Value)) && (!String.IsNullOrEmpty(DefaultValue)))
                {
                    Value = DefaultValue;
                    TextBox_Value.Text = Value;
                }

                Repeater_Option.DataBind();
            }
            else
            {
                if (Page.IsPostBack)
                    Bind();
            }

        }
        public void Bind()
        {

            if (DataSource is Object)
            {
                //
                Repeater_Option.DataSource = DataSource;
                Repeater_Option.ItemDataBound += Repeater_JSon_ItemDataBound;
                Repeater_Option.DataBind();
            }
            else if ((DataSourceText != null) && (DataSourceText.Length.Equals(DataSourceValue.Length)))
            {
                try
                {
                    Dictionary<String, String> dictionarySource = new Dictionary<String, String>();
                    for (int i = 0; i < DataSourceValue.Length; i++)
                        dictionarySource.Add(DataSourceText[i], DataSourceValue[i]);

                    Repeater_Option.DataSource = dictionarySource;
                    Repeater_Option.ItemDataBound += Repeater_JSonDataSourceValue_ItemDataBound;
                    Repeater_Option.DataBind();
                }
                catch
                {

                }
            }
        }

        private void Repeater_JSon_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
            {
                //
                String sDataTextField = DataBinder.Eval(e.Item.DataItem, DataTextField).ToString();
                String sDataValueField = DataBinder.Eval(e.Item.DataItem, DataValueField).ToString();

                Literal myLiteral_Option = (Literal)e.Item.FindControl("Literal_Option");
                if (sDataValueField.Equals(Value))
                    myLiteral_Option.Text = "<option value=\"" + sDataValueField + "\" selected>" + sDataTextField + "</option>";
                else
                    myLiteral_Option.Text = "<option value=\"" + sDataValueField + "\">" + sDataTextField + "</option>";

            }
        }
        private void Repeater_JSonDataSourceValue_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
            {
                //
                String sDataTextField = DataBinder.Eval(e.Item.DataItem, "Key").ToString();
                String sDataValueField = DataBinder.Eval(e.Item.DataItem, "Value").ToString();

                Literal myLiteral_Option = (Literal)e.Item.FindControl("Literal_Option");
                if (sDataValueField.Equals(Value))
                    myLiteral_Option.Text = "<option value=\"" + sDataValueField + "\" selected>" + sDataTextField + "</option>";
                else
                    myLiteral_Option.Text = "<option value=\"" + sDataValueField + "\">" + sDataTextField + "</option>";
            }
        }
        private void Repeater_DataSourceStringSimple_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
            {
                //
                String sDataValueField = e.Item.DataItem.ToString();

                Literal myLiteral_Option = (Literal)e.Item.FindControl("Literal_Option");
                if (sDataValueField.Equals(Value))
                    myLiteral_Option.Text = "<option value=\"" + sDataValueField + "\" selected>" + sDataValueField + "</option>";
                else
                    myLiteral_Option.Text = "<option value=\"" + sDataValueField + "\">" + sDataValueField + "</option>";
            }
        }

        public void SetValue(String _Value)
        {
            try
            {
                if (!String.IsNullOrEmpty(_Value))
                {
                    TextBox_Value.Text = _Value;
                    Value = _Value;

                    //
                    Repeater_Option.DataBind();
                }
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

        protected void TextBox_Value_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(SelectedIndexChangedMethod))
                Page.GetType().InvokeMember(SelectedIndexChangedMethod, System.Reflection.BindingFlags.InvokeMethod, null, this.Page, new object[] { TextBox_Value.Text });

        }

    }
}