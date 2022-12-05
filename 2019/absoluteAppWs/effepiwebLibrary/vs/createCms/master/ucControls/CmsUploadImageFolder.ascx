<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CmsUploadImageFolder.ascx.cs" Inherits="backOffice.ucControls.CmsUploadImageFolder" %>


<asp:Panel ID="Panel_Input" runat="server" CssClass="form-group">
    <!-- -->
    <div class="tf_button-group">
        <asp:HyperLink runat="server" ID="HyperLink_MasterContent" Visible="false" CssClass="tf_master btn btn-default btn-sm shiny icon-only"><i class="fa fa-comment-o"></i></asp:HyperLink>
        <asp:HyperLink runat="server" ID="HyperLink_InfoContent" Visible="false" CssClass="tf_help btn btn-default btn-sm shiny icon-only"><i class="fa fa-info-circle"></i></asp:HyperLink>
    </div>
    <!-- -->
    <label class="control-label">
        <asp:Literal ID="Literal_Label" runat="server">Image From Repository Folder</asp:Literal></label>
    <div class="inform-upload inform-folder-upload bordered">
        <span class='inform-details'>
            <asp:Literal ID="Literal_Label_Dimension" runat="server"></asp:Literal>
            <small class="help-block" data-bv-validator="notEmpty" data-bv-for="password" data-bv-result="INVALID" style=""></small>
        </span>
        <div class='col-sm-12'>
            <button class='btn btn-primary' id="buttonUploadFolder" clientid="Static" runat="server" data-action='js-uploadfolder' type='button'>Retrieve Folder from Repository</button>
        </div>
        <div class='col-sm-12'>
            <div class='inform-folder-status'>
                <span class='inform-folder-name'>
                    <asp:Label ID="Label_Foldername" runat="server"></asp:Label></span>
                <asp:TextBox ID="HiddenField_UrlFolder" runat="server" CssClass="hidden" />
                <asp:TextBox ID="HiddenField_Parameter" runat="server" CssClass="hidden" />
            </div>
        </div>
    </div>
    <!-- -->
    <asp:Panel ID="Panel_MasterContent" runat="server" CssClass="tf-popup tf-master_content hide">
        <div class="tf_content">
            <asp:Literal ID="Literal_MasterContent" runat="server"></asp:Literal>
        </div>
    </asp:Panel>
    <asp:Panel ID="Panel_InfoContent" runat="server" CssClass="tf-popup tf-info_content hide">
        <div class="tf_header">
            <asp:Literal ID="Literal_InfoTitle" runat="server"></asp:Literal>
        </div>
        <div class="tf_content">
            <asp:Literal ID="Literal_InfoContent" runat="server"></asp:Literal>
        </div>
    </asp:Panel>
    <!-- -->
</asp:Panel>

<!-- INPUTS SPACER -->
<div class="inputs-spacer" id="spacerDiv" runat="server">
</div>

