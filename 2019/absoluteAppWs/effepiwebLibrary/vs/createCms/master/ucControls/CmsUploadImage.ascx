<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CmsUploadImage.ascx.cs"
    Inherits="backOffice.ucControls.CmsUploadImage" %>
<!-- IMAGE UPLOAD -->
<div class="form-group">
    <label class="control-label">
        <asp:Literal ID="Literal_Label" runat="server">Immagine</asp:Literal></label><br />
    <div class="file-upload-box">
        <asp:HyperLink ID="HyperLink_ShowRepository" CssClass="file-upload-repo large" runat="server" Visible="false"><i class="icon-picture"></i>&nbsp;Aggiungi Immagine dalla Raccolta</asp:HyperLink>
        
            <asp:HyperLink ID="HyperLink_ShowAddFile" CssClass="file-upload-repo large" runat="server" Visible="false"><i class="icon-picture"></i>&nbsp;Add Image From Your Computer</asp:HyperLink>
            <input type="file" runat="server" id="filesingleupload">
        
        <asp:TextBox ID="TextBox_HttpUrlFile" runat="server" CssClass="hidden"></asp:TextBox>
        <asp:Panel ID="Panel_FilePreview" runat="server" CssClass="file-preview">
            <a href="javascript:;">
                <asp:Image ID="Image_Preview" runat="server" /></a>
        </asp:Panel>
        <asp:Panel ID="Panel_Filename" runat="server" CssClass="file-name" style="display:none">
        <span id="spanNomeFile"><asp:Literal ID="Literal_FileName" runat="server"></asp:Literal></span>
            <br>
            <i class="icon-remove-sign icon-remove" id="iLinkButton_Remove" runat="server"></i>
            <asp:HyperLink CssClass="remove-img" ID="HyperLink_Change" runat="server">Sostituisci</asp:HyperLink><asp:Literal ID="Literal_Or" runat="server"> or </asp:Literal><asp:LinkButton CssClass="remove-img" ID="LinkButton_Remove" runat="server" OnClick="LinkButton_Remove_Click" ValidationGroup="UploadImage" CausesValidation="false">Rimuovi</asp:LinkButton>            
        </asp:Panel>
    </div>    
</div>
<!-- INPUTS SPACER -->
<div class="inputs-spacer" id="spacerDiv" runat="server">
</div>
