<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CmsTextBoxEmail.ascx.cs"
    Inherits="backOffice.ucControls.CmsTextBoxEmail" %>
<!-- SHORT TEXT -->
<asp:Panel ID="Panel_Input" runat="server" CssClass="form-group">
    <!-- -->
    <div class="tf_button-group">
        <asp:HyperLink runat="server" ID="HyperLink_MasterContent" Visible="false" CssClass="tf_master btn btn-default btn-sm shiny icon-only"><i class="fa fa-comment-o"></i></asp:HyperLink>
        <asp:HyperLink runat="server" ID="HyperLink_InfoContent" Visible="false" CssClass="tf_help btn btn-default btn-sm shiny icon-only"><i class="fa fa-info-circle"></i></asp:HyperLink>
    </div>
    <!-- -->
    <label class="control-label">
        <asp:Literal ID="Literal_Label" runat="server"></asp:Literal></label><br>
    <asp:TextBox ID="TextBox_Email" runat="server" CssClass="form-control" TextMode="Email"></asp:TextBox>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator_TextBoxEmail_Value" runat="server" SetFocusOnError="true" Display="Dynamic"
        ControlToValidate="TextBox_Email" ErrorMessage="Required Field" Enabled="false"></asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator runat="server"
        ID="RegularExpressionValidator_TextBoxEmail_Value" SetFocusOnError="true" Display="Dynamic"
        ControlToValidate="TextBox_Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
        ErrorMessage="Email non valida"></asp:RegularExpressionValidator>
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
