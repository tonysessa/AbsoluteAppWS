<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="details.aspx.cs" Inherits="backOffice.cmssections.details" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" ValidateRequest="false" %>

<%@ Register Src="~/cms/uc/sidebarMenu.ascx" TagName="sidebarMenu" TagPrefix="ucSidebarMenu" %>
<%@ Register src="~/cms/uc/navbar.ascx" tagname="navbar" tagprefix="ucNavBar" %>

<%@ Register Src="~/cms/ucControls/CmsDropDownList.ascx" TagName="CmsDropDownList" TagPrefix="uc2" %>
<%@ Register Src="~/cms/ucControls/CmsHtmlEditor.ascx" TagName="CmsHtmlEditor" TagPrefix="uc3" %>
<%@ Register Src="~/cms/ucControls/CmsTextBox.ascx" TagName="CmsTextBox" TagPrefix="uc4" %>
<%@ Register Src="~/cms/ucControls/CmsTextBoxData.ascx" TagName="CmsTextBoxData" TagPrefix="uc5" %>
<%@ Register Src="~/cms/ucControls/CmsTextBoxLink.ascx" TagName="CmsTextBoxLink" TagPrefix="uc6" %>
<%@ Register Src="~/cms/ucControls/CmsTextBoxNumber.ascx" TagName="CmsTextBoxNumber" TagPrefix="uc7" %>
<%@ Register Src="~/cms/ucControls/CmsUploadFile.ascx" TagName="CmsUploadFile" TagPrefix="uc8" %>
<%@ Register Src="~/cms/ucControls/CmsUploadImageCrop.ascx" TagName="CmsUploadImageCrop" TagPrefix="uc11" %>
<%@ Register Src="~/cms/ucControls/CmsTextBoxDateTime.ascx" TagName="CmsTextBoxDateTime" TagPrefix="uc12" %>
<%@ Register Src="~/cms/ucControls/CmsTranslation.ascx" TagName="CmsTranslation" TagPrefix="uc13" %>
<%@ Register Src="~/cms/ucControls/CmsRadioButtonList.ascx" TagName="CmsRadioButtonList" TagPrefix="uc14" %>
<%@ Register Src="~/cms/ucControls/CmsRepository.ascx" TagName="CmsRepository" TagPrefix="uc99" %>
<%@ Register Src="~/cms/ucControls/CmsRepositoryModal.ascx" TagName="CmsRepositoryModal" TagPrefix="uc1" %>
<!DOCTYPE html>

<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="shortcut icon" href="/cms/assets/img/favicon.png" type="image/x-icon" />
    <title>Dettagli CmsSections</title>

    <!--Basic Styles-->
    <link href="/cms/assets/css/bootstrap.min.css" rel="stylesheet" />
    <link id="bootstrap_rtl_link" rel="stylesheet" />

    <!--Beyond styles-->
    <link href="/cms/assets/css/beyond.min.css" rel="stylesheet" />
    <link href="/cms/assets/css/font-awesome.min.css" rel="stylesheet" />
    <link href="/cms/assets/css/animate.min.css" rel="stylesheet" />
	<link href="/cms/assets/css/application.css" rel="stylesheet" />
	<link href="/cms/assets/css/add.css" rel="stylesheet" />
    <link href="/cms/assets/css/skins/default.min.css" rel="stylesheet" />

    <!--Skin Script: Place this script in head to load scripts for skins and rtl support-->
    <script src="/cms/assets/js/skins.min.js"></script>    

    <!--Basic Scripts-->
    <script src="/cms/assets/js/jquery.min.js"></script>

</head>
<body>
    <form id="frmDetails" runat="server">
        <ucNavBar:navBar ID="navBar1" runat="server" CurrentContentTable="CmsSections" />
        <div class="main-container container-fluid">
            <div class="page-container">
                <div class="page-sidebar" id="sidebar">
                    <div class="sidebar-header-wrapper">
                    </div>
					<ucSidebarMenu:sidebarMenu ID="sidebarMenu1" runat="server" />					
                </div>
                <div class="page-content">
					<div class="section-loading-container">
						<div class="loader"></div>
					</div>
                    <div class="page-breadcrumbs">
                    </div>
                    <div class="page-header position-relative">
                        <div class="header-title">
                            <h1>
								CmsSections</h1>
                        </div>
                        <div class="header-buttons">
                            <a class="sidebar-toggler" href="#">
                                <i class="fa fa-arrows-h"></i>
                            </a>
                            <a class="refresh" id="refresh-toggler" href="">
                                <i class="glyphicon glyphicon-refresh"></i>
                            </a>
                            <a class="fullscreen" id="fullscreen-toggler" href="#">
                                <i class="glyphicon glyphicon-fullscreen"></i>
                            </a>
                        </div>
                    </div>
                    <div class="page-body">
                        <div class="row">
                            <div class="col-lg-12 col-sm-12 col-xs-12">
                                <div class="row">
                                    <asp:Panel ID="Panel_Left" runat="server" Visible="true" CssClass="col-lg-6 col-sm-6 col-xs-12 isnotinedit">
                                        <div class="widget">
                                            <div class="widget-header bordered-bottom bordered-primary">
                                                <span class="widget-caption">
                                                    <asp:Literal ID="Literal_Cms_RecordDetail" runat="server">Dettaglio</asp:Literal> CmsSections</span>
                                                <span class="widget-caption-right">
                                                    <asp:Literal ID="Literal_Cms_LastCorrection" runat="server" Visible="false">Last Update</asp:Literal>:&nbsp;<asp:Literal ID="Literal_LastModify" runat="server"></asp:Literal> | <i runat="server" id="iconStatusFlag" Visible="false"></i>&nbsp;<i runat="server" id="iconPublished" Visible="false"></i></span>
                                            </div>										
                                            <asp:Panel ID="Panel_Content_Form" runat="server" Visible="true" CssClass="widget-body" EnableViewState="false">
												<uc4:CmsTextBox ID="CmsTextBox_Uid" Title="Uid" MaxLength="50" Required="false" runat="server" showSpacer="true" />
                                                <uc4:CmsTextBox ID="CmsTextBox_Title" Title="Title" MaxLength="150" Required="true" runat="server" showSpacer="true" cmsLabelKey="Cms.CmsSections.Title" />
                                                <uc4:CmsTextBox ID="CmsTextBox_ContentTable" Title="Contenttable" MaxLength="150" runat="server" showSpacer="true" cmsLabelKey="Cms.CmsSections.ContentTable" />
                                                <uc4:CmsTextBox ID="CmsTextBox_SectionUri" Title="Sectionuri" MaxLength="150" runat="server" showSpacer="true" cmsLabelKey="Cms.CmsSections.SectionUri" />
                                                <uc4:CmsTextBox ID="CmsTextBox_Link" Title="Link" MaxLength="200" runat="server" showSpacer="true" cmsLabelKey="Cms.CmsSections.Link" />
                                                <uc4:CmsTextBox ID="CmsTextBox_Link_Target" Title="Link target" MaxLength="20" runat="server" showSpacer="true" cmsLabelKey="Cms.CmsSections.Link_Target" />
                                                <uc4:CmsTextBox ID="CmsTextBox_Link_Preview" Title="Link preview" MaxLength="200" runat="server" showSpacer="true" cmsLabelKey="Cms.CmsSections.Link_Preview" />
                                                <uc4:CmsTextBox ID="CmsTextBox_IconClass" Title="Iconclass" MaxLength="200" runat="server" showSpacer="true" cmsLabelKey="Cms.CmsSections.IconClass" />

                                                <asp:Button CssClass="btn btn-palegreen" ID="Button_SaveAsDraft" Text="Save as draft" runat="server" OnClick="Button_SaveAsDraft_Click" />
                                                <asp:Button CssClass="btn btn-success" ID="Button_SaveOnStage" Text="Save on Stage" runat="server" OnClick="Button_SaveOnStage_Click" />												
                                                <br />
												<br />
                                                <asp:Button CssClass="btn btn-danger" ID="Button_Cancel" Text="Exit" runat="server" CausesValidation="false" OnClick="Button_Cancel_Click" />
                                                <asp:Button CssClass="btn btn-default" ID="Button_Reset" Text="Reset" runat="server" CausesValidation="false" OnClick="Button_Reset_Click" />
                                            </asp:Panel>
                                        </div>
                                    </asp:Panel>
									<asp:Panel ID="Panel_Related" runat="server" Visible="true" CssClass="col-lg-6 col-sm-6 col-xs-12 isnotinedit" EnableViewState="false">
                                                   <asp:Panel ID="Panel_Related_CmsSubSections_List" CssClass="widget" runat="server" Visible="true" EnableViewState="false">
                                                     <div class="widget-header bordered-bottom bordered-themesecondary">
                                                         <span class="widget-caption">CmsSubSections</span>
                                                     </div>
                                                     <div class="widget-body">
                                                             <div class="related-loading-container">
                                                                 <div class="loader"></div> 
                                                             </div>
                                                     </div>
                                                   </asp:Panel>

									</asp:Panel>
                                    <asp:Panel ID="Panel_Related_Form" runat="server" Visible="true" CssClass="col-lg-6 col-sm-6 col-xs-12 col--editmode" EnableViewState="false">

                                        		    <!-- CmsSubSections List -->

                                                    <asp:Panel ID="Panel_Related_CmsSubSections_Form" CssClass="widget" runat="server" DefaultButton="Button_Related_CmsSubSections_Save" Visible="false">
                                                        <div class="widget-header bordered-bottom bordered-themesecondary">
                                                          <span class="widget-caption">
                                                            CmsSubSections</span>
                                                          <span class="widget-caption-right">                                                              <asp:Literal ID="Literal_Cms_CmsSubSections_LastCorrection" runat="server" Visible="false">Last Update</asp:Literal>:&nbsp;<asp:Literal ID="Literal_CmsSubSections_LastModify" runat="server"></asp:Literal> | <i runat="server" id="iconStatusFlag_CmsSubSections"></i></span>                                                        </div>
                                                        <asp:Panel ID="Panel_Field_CmsSubSections" runat="server" CssClass="widget-body" EnableViewState="false">
                    <uc4:CmsTextBox ID="CmsTextBox_CmsSubSections_Title" Title="Title" MaxLength="150" Required="true" runat="server" showSpacer="true" cmsLabelKey="Cms.CmsSubSections.Title" />
                    <uc4:CmsTextBox ID="CmsTextBox_CmsSubSections_ContentTable" Title="Contenttable" MaxLength="150" runat="server" showSpacer="true" cmsLabelKey="Cms.CmsSubSections.ContentTable" />
                    <uc4:CmsTextBox ID="CmsTextBox_CmsSubSections_SectionUri" Title="Sectionuri" MaxLength="150" runat="server" showSpacer="true" cmsLabelKey="Cms.CmsSubSections.SectionUri" />
                    <uc4:CmsTextBox ID="CmsTextBox_CmsSubSections_Link" Title="Link" MaxLength="200" runat="server" showSpacer="true" cmsLabelKey="Cms.CmsSubSections.Link" />
                    <uc4:CmsTextBox ID="CmsTextBox_CmsSubSections_Link_Target" Title="Link target" MaxLength="20" runat="server" showSpacer="true" cmsLabelKey="Cms.CmsSubSections.Link_Target" />
                    <uc4:CmsTextBox ID="CmsTextBox_CmsSubSections_Link_Preview" Title="Link preview" MaxLength="200" runat="server" showSpacer="true" cmsLabelKey="Cms.CmsSubSections.Link_Preview" />
                    <uc4:CmsTextBox ID="CmsTextBox_CmsSubSections_IconClass" Title="Iconclass" MaxLength="200" runat="server" showSpacer="true" cmsLabelKey="Cms.CmsSubSections.IconClass" />
                                                        <asp:HiddenField ID="HiddenField_CmsSubSections_NextUid" ClientIDMode="Static" runat="server" />
                                                        <br />
                                                        <br />
                                                        <asp:Button CssClass="btn btn-success" ID="Button_Related_CmsSubSections_Save" Text="Save" ValidationGroup="CmsSubSections" runat="server" OnClick="Button_Related_CmsSubSections_Save_Click" />
                                                        <asp:Button CssClass="btn btn-success disabled" ID="Button_Related_CmsSubSections_SaveAndNext" Text="Save and continue" ValidationGroup="CmsSubSections" runat="server" OnClick="Button_Related_CmsSubSections_SaveAndNext_Click" Visible="false" />
                                                        <asp:Button CssClass="btn btn-danger" ID="Button_Related_CmsSubSections_Cancel" Text="Cancel" CausesValidation="false" runat="server" OnClick="Button_Related_CmsSubSections_Cancel_Click" />
                                                    </asp:Panel>
                                                </asp:Panel>


                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

		<!-- Repository -->
        <asp:Panel ID="Panel_Repository" data-state="" ClientIDMode="Static" runat="server" EnableViewState="false">
            <uc99:CmsRepository ID="CmsRepositoryMain" runat="server"></uc99:CmsRepository>
        </asp:Panel>
        <uc1:CmsRepositoryModal ID="CmsRepositoryModal1" runat="server" />     
		<!-- End Repository -->

		<!--jquery Plug-In-->
		<script src="/cms/assets/js/jquery-ui-1.10.4.custom.js"></script>
		<script src="/cms/assets/js/slimscroll/jquery.slimscroll.min.js"></script>
		
		<!--Bootstrap Scripts-->
		<script src="/cms/assets/js/bootstrap.min.js"></script>		

        <!--Beyond Scripts-->
        <script src="/cms/assets/js/beyond.min.js"></script>
		<script src="/cms/assets/js/bootbox/bootbox.js"></script>
        <script src="/cms/assets/js/datatable/jquery.dataTables.min.js"></script>
        <script src="/cms/assets/js/datatable/ZeroClipboard.js"></script>
        <script src="/cms/assets/js/datatable/dataTables.tableTools.min.js"></script>
        <script src="/cms/assets/js/datatable/dataTables.bootstrap.min.js"></script>
        <script src="/cms/assets/js/datatable/datatables-init.js"></script>

        <!--Jquery Select2-->
        <script src="/cms/assets/js/select2/select2.js"></script>   
		   		
		<!--Bootstrap Date Picker-->
        <script src="/cms/assets/js/datetime/bootstrap-datepicker.js"></script>
		<script src="/cms/assets/js/datetime/bootstrap-timepicker.js"></script>
               
		<!--Page Related Scripts-->
		<script src="/cms/assets/js/toastr/toastr.js"></script>	

        <!--Custom Scripts-->
		<script src="/cms/assets/js/functions.js"></script>
		<script src="/cms/assets/js/functionDetails.js"></script>
        <script src="/cms/assets/js/crop.js"></script>

		<script>
        function Panel_Related_CmsSubSections_List_Bind() {
            var sData = '<%=getDataActionUrl(Uid, "CmsSubSections") %>';
            $.ajax({
                url: "action.aspx",
                type: 'POST',
                data: sData,
                success: function (data) {
                    $('#Panel_Related_CmsSubSections_List').html(data);
                    InitAction('Panel_Related_CmsSubSections_List');
                }
            });
        }


		$(document).ready(function () {
			//
			<%=responseScript %>

			//
                setTimeout("Panel_Related_CmsSubSections_List_Bind()", 100);

		});
		</script>
    </form>
</body>
</html>
