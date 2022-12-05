<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CmsUploadImageCrop.ascx.cs" Inherits="backOffice.ucControls.CmsUploadImageCrop" %>

<asp:Panel ID="Panel_Input" runat="server" CssClass="form-group">
    <!-- -->
    <div class="tf_button-group">
        <asp:HyperLink runat="server" ID="HyperLink_MasterContent" Visible="false" CssClass="tf_master btn btn-default btn-sm shiny icon-only"><i class="fa fa-comment-o"></i></asp:HyperLink>
        <asp:HyperLink runat="server" ID="HyperLink_InfoContent" Visible="false" CssClass="tf_help btn btn-default btn-sm shiny icon-only"><i class="fa fa-info-circle"></i></asp:HyperLink>
    </div>
    <!-- -->
    <label class="control-label">
        <asp:Literal ID="Literal_Label" runat="server">Image</asp:Literal></label>
    <div class="inform-upload bordered">
        <span class='inform-details'>
            <asp:Literal ID="Literal_Label_Dimension" runat="server"></asp:Literal>
            <small class="help-block" data-bv-validator="notEmpty" data-bv-for="password" data-bv-result="INVALID" style="">
                <!-- qui va il literal -->
                The image don't respect the minimum size, may be blurred</small>
        </span>
        <div class='col-sm-12'>
            <button class='btn btn-primary' id="buttonUploadFast" clientid="Static" runat="server" data-action='js-open-repository' type='button'>Upload to / Retrieve from Repository</button>
            <div class='btn btn-primary' id="buttonUpload" clientid="Static" runat="server" data-action='js-upload-inform'><input class='file-upload-inform' name='fileupload2' type='file'><i class='fa fa-upload'></i><asp:Literal ID="Literal_AddFile" runat="server">Add File</asp:Literal></div>
        </div>
        <div class='col-sm-12'>
            <div class='inform-status'>
                <span class='inform-name'>
                    <asp:Label ID="Label_Filename" runat="server"></asp:Label></span>
                <asp:TextBox ID="HiddenField_UrlFile" runat="server" CssClass="hidden" />
                <asp:TextBox ID="HiddenField_Parameter" runat="server" CssClass="hidden" />
                <asp:TextBox ID="HiddenField_ImageUrl" runat="server" CssClass="hidden" />
                <asp:TextBox ID="HiddenField_ImageUrl_Preview" runat="server" CssClass="hidden" />
                <asp:TextBox ID="HiddenField_ImageUrl_Ori" runat="server" CssClass="hidden" />
                <asp:TextBox ID="HiddenField_IsCrop" runat="server" CssClass="hidden" />
            </div>
            <div class='uploadstatus'>
                <div class='uploadstatus__inner'></div>
            </div>
            <div class='inform-thumb'>
                <asp:Image ID="Image_Preview" runat="server" class='inform-image' Visible="false" />
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

