using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Linq;
using System.Runtime.Serialization;
// Support
using Support.db;
using Support.CmsFunction;
using Support.Library;
using Support.Web;
//
using dataLibs;
using System.Collections.Generic;

namespace backOffice.uc
{
    public partial class sidebarMenu : System.Web.UI.UserControl
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
        protected CmsBoDataLibs objCmsBoDataLibs = new CmsBoDataLibs();
        #endregion

        #region Parametri
        public String sStartingpage = WebContext.getConfig("%.startingpage").ToString();
        public String sCmsStartingpage = WebContext.getConfig("%.cmsStartingpage").ToString();
        public String sCryptKey = WebContext.getConfig("%.cryptKey").ToString();
        public String sStoragePublicBaseUrl = WebContext.getConfig("%.storagePublicBaseUrl").ToString();
        #endregion        

        #region Property
        public CmsUserSession currentCmsUserSession
        {
            get
            {
                if (ViewState["_CmsUserSession"] == null)
                    return null;
                else
                    return (CmsUserSession)ViewState["_CmsUserSession"];
            }
            set { ViewState["_CmsUserSession"] = value; }
        }
        public String currentCmsSubSections
        {
            get
            {
                if (ViewState["_currentCmsSubSections"] == null)
                    return "";
                else
                    return (String)ViewState["_currentCmsSubSections"];
            }
            set { ViewState["_currentCmsSubSections"] = value; }
        }
        public String currentCmsSubSections_SectionUri
        {
            get
            {
                if (ViewState["_currentCmsSubSections_SectionUri"] == null)
                    return "";
                else
                    return (String)ViewState["_currentCmsSubSections_SectionUri"];
            }
            set { ViewState["_currentCmsSubSections_SectionUri"] = value; }
        }
        public String sJsCurrentContentTable
        {
            get
            {
                if (ViewState["_sJsCurrentContentTable"] == null)
                    return "";
                else
                    return (String)ViewState["_sJsCurrentContentTable"];
            }
            set { ViewState["_sJsCurrentContentTable"] = value; }
        }
        #endregion

        #region uc Method
        protected void Page_Load(object sender, EventArgs e)
        {
            # region Check Login
            if (Session["CmsUserSession"] == null)
                Response.Redirect(sCmsStartingpage + "login.aspx?currentPage=" + Server.UrlEncode(Request.Url.ToString()));
            else
                currentCmsUserSession = (CmsUserSession)Session["CmsUserSession"];
            #endregion

            // PostBack
            //
            sStoragePublicBaseUrl = sStoragePublicBaseUrl + currentCmsUserSession.currentCmsNlsContext.StorageFolder + "/";

            //
            InitUC();

            HyperLink_Impostazioni.NavigateUrl = "/cms/adm/GlobalParameter/details.aspx?Uid=" + currentCmsUserSession.currentCmsNlsContext.Uid.ToString();

            Literal_NoticeBoard.Text = currentCmsUserSession.GetGlobalLabel("Cms.Literal_NoticeBoard.Text", "Notice Board");
            Literal_ManageFiles.Text = currentCmsUserSession.GetGlobalLabel("Cms.Literal_ManageFiles.Text", "Manage Files");
        }
        protected void InitUC()
        {
            //
            Repeater_Section_Bind();
        }
        protected void Repeater_Section_Bind()
        {

            Repeater_Section.DataSource = currentCmsUserSession.currentCmsRoles.EnableCmsSections.OrderBy(t => t.Ord).ToList();
            Repeater_Section.ItemDataBound += new RepeaterItemEventHandler(Repeater_Section_ItemDataBound);
            Repeater_Section.DataBind();
        }
        protected void Repeater_Section_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
            {
                CmsSections myCmsSections = (CmsSections)e.Item.DataItem;

                if (myCmsSections != null)
                {
                    String sIconClass = "" + myCmsSections.IconClass;
                    // Custom Link
                    if (!String.IsNullOrEmpty(myCmsSections.Link))
                    {
                        HyperLink myHyperLink_Section_Title = (HyperLink)e.Item.FindControl("HyperLink_Section_Title");
                        myHyperLink_Section_Title.Text = "<i class=\"menu-icon " + sIconClass + "\"></i><span class=\"menu-text\">" + myCmsSections.Title + "<i class=\"menu-expand\"></i></span>";
                        myHyperLink_Section_Title.Target = myCmsSections.Link_Target;
                        myHyperLink_Section_Title.NavigateUrl = myCmsSections.Link;
                    }
                    else
                    {
                        HyperLink myHyperLink_Section_Title = (HyperLink)e.Item.FindControl("HyperLink_Section_Title");
                        myHyperLink_Section_Title.Text = "<i class=\"menu-icon " + sIconClass + "\"></i><span class=\"menu-text\">" + myCmsSections.Title + "<i class=\"menu-expand\"></i></span>";
                    }

                    // Preview link
                    if (!String.IsNullOrEmpty(myCmsSections.Link_Preview))
                    {
                        HyperLink myHyperLink_Section_Preview = (HyperLink)e.Item.FindControl("HyperLink_Section_Preview");
                        myHyperLink_Section_Preview.Text = "<i class=\"menu-icon " + sIconClass + "\"></i><span class=\"menu-text\">" + myCmsSections.Title + "<i class=\"menu-expand\"></i></span>";
                        myHyperLink_Section_Preview.Target = "_blank";
                        myHyperLink_Section_Preview.NavigateUrl = myCmsSections.Link_Preview;
                    }

                    //                
                    List<CmsSubSections> myCmsSubSections = currentCmsUserSession.currentCmsRoles.EnableCmsSubSections.Where(t => t.Uid_CmsSections == myCmsSections.Uid).OrderBy(t => t.Ord).ToList();
                    if (myCmsSubSections.Count > 0)
                    {
                        Repeater Repeater_SubSection = (Repeater)e.Item.FindControl("Repeater_SubSection");

                        Repeater_SubSection.DataSource = myCmsSubSections;
                        Repeater_SubSection.ItemDataBound += new RepeaterItemEventHandler(Repeater_SubSection_ItemDataBound);
                        Repeater_SubSection.DataBind();
                    }
                }
            }
        }
        protected void Repeater_SubSection_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
            {
                CmsSubSections myCmsSubSections = (CmsSubSections)e.Item.DataItem;
                if (myCmsSubSections != null)
                {
                    String sIconClass = String.Empty;
                    // Custom Link
                    if (!String.IsNullOrEmpty(myCmsSubSections.Link))
                    {
                        HyperLink myHyperLink_SubSection_Title = (HyperLink)e.Item.FindControl("HyperLink_SubSection_Title");
                        myHyperLink_SubSection_Title.Text = "<i class=\"menu-icon " + sIconClass + "\"></i><span class=\"menu-text\">" + myCmsSubSections.Title + "</span>";
                        myHyperLink_SubSection_Title.NavigateUrl = myCmsSubSections.Link;
                        myHyperLink_SubSection_Title.Target = myCmsSubSections.Link_Target;
                        myHyperLink_SubSection_Title.CssClass = "lnk" + objLibString.ClearUrlName(myCmsSubSections.ContentTable + myCmsSubSections.SectionUri);
                    }
                    else
                    {
                        HyperLink myHyperLink_SubSection_Title = (HyperLink)e.Item.FindControl("HyperLink_SubSection_Title");
                        myHyperLink_SubSection_Title.Text = "<i class=\"menu-icon " + sIconClass + "\"></i><span class=\"menu-text\">" + myCmsSubSections.Title + "</span>";

                        if ((!String.IsNullOrEmpty(myCmsSubSections.SectionUri)) && (myCmsSubSections.SectionUri.ToUpper().Equals("ADMIN")))
                            myHyperLink_SubSection_Title.NavigateUrl = sCmsStartingpage + "adm/" + myCmsSubSections.ContentTable + "/list.aspx?section=" + myCmsSubSections.ContentTable + "&sectionUri=" + myCmsSubSections.SectionUri;
                        else
                            myHyperLink_SubSection_Title.NavigateUrl = sCmsStartingpage + myCmsSubSections.ContentTable + "/list.aspx?section=" + myCmsSubSections.ContentTable + "&sectionUri=" + myCmsSubSections.SectionUri;

                        myHyperLink_SubSection_Title.CssClass = "lnk" + objLibString.ClearUrlName(myCmsSubSections.ContentTable + myCmsSubSections.SectionUri);

                        HtmlControl liHyperLink_SubSection_Title = (HtmlControl)e.Item.FindControl("liHyperLink_SubSection_Title");
                        /*
                        if ((!String.IsNullOrEmpty(myCmsSubSections.ContentTable)) && (("Admin" + myCmsSubSections.ContentTable).Equals(currentCmsSubSections)) && (myCmsSubSections.SectionUri.ToUpper().Equals("ADMIN")))
                        {
                            liHyperLink_SubSection_Title.Attributes.Add("class", "active");
                            //
                            HtmlControl liHyperLink_Section_Title = (HtmlControl)liHyperLink_SubSection_Title.Parent.Parent.Parent.FindControl("liHyperLink_Section_Title");
                            liHyperLink_Section_Title.Attributes.Add("class", "active open");
                        }
                        else if ((!String.IsNullOrEmpty(myCmsSubSections.ContentTable)) && (myCmsSubSections.ContentTable.Equals(currentCmsSubSections)) && ((myCmsSubSections.SectionUri != null) && (!myCmsSubSections.SectionUri.ToUpper().Equals("ADMIN"))))
                        {
                            liHyperLink_SubSection_Title.Attributes.Add("class", "active");
                            //
                            HtmlControl liHyperLink_Section_Title = (HtmlControl)liHyperLink_SubSection_Title.Parent.Parent.Parent.FindControl("liHyperLink_Section_Title");
                            liHyperLink_Section_Title.Attributes.Add("class", "active open");
                        }
                        else */
                        if ((myCmsSubSections.SectionUri!=null) && (myCmsSubSections.SectionUri.ToUpper().Equals("ADMIN")))
                        {
                            if ((!String.IsNullOrEmpty(myCmsSubSections.ContentTable)) && (("Admin" + myCmsSubSections.ContentTable).Equals(currentCmsSubSections)))
                            {
                                liHyperLink_SubSection_Title.Attributes.Add("class", "active");
                                //
                                HtmlControl liHyperLink_Section_Title = (HtmlControl)liHyperLink_SubSection_Title.Parent.Parent.Parent.FindControl("liHyperLink_Section_Title");
                                liHyperLink_Section_Title.Attributes.Add("class", "active open");
                            }
                        }
                        else
                        {
                            if ((!String.IsNullOrEmpty(myCmsSubSections.ContentTable)) && (myCmsSubSections.ContentTable.Equals(currentCmsSubSections)))
                            {
                                liHyperLink_SubSection_Title.Attributes.Add("class", "active");
                                //
                                HtmlControl liHyperLink_Section_Title = (HtmlControl)liHyperLink_SubSection_Title.Parent.Parent.Parent.FindControl("liHyperLink_Section_Title");
                                liHyperLink_Section_Title.Attributes.Add("class", "active open");
                            }
                        }
                    }

                    // Preview link
                    if (!String.IsNullOrEmpty(myCmsSubSections.Link_Preview))
                    {
                        HyperLink myHyperLink_SubSection_Preview = (HyperLink)e.Item.FindControl("HyperLink_SubSection_Preview");
                        myHyperLink_SubSection_Preview.Text = "<i class=\"menu-icon " + sIconClass + "\"></i><span class=\"menu-text\">" + myCmsSubSections.Title + "</span>";
                        myHyperLink_SubSection_Preview.Target = "_blank";
                        myHyperLink_SubSection_Preview.NavigateUrl = myCmsSubSections.Link_Preview;
                    }
                }
            }
        }

        protected void LinkButton_Logout_Click(object sender, EventArgs e)
        {

            //
            Session["CmsUserSession"] = null;

            //
            Response.Redirect(sCmsStartingpage + "login.aspx?currentCmsNlsContext=" + objLibCript.uEncode(currentCmsUserSession.currentCmsNlsContext.Uid.ToString(), objLibCript.getCryptKey(sCryptKey)));
        }
        #endregion
    }
}