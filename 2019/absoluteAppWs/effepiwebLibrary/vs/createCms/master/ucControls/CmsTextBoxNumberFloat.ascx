<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CmsTextBoxNumberFloat.ascx.cs"
    Inherits="backOffice.ucControls.CmsTextBoxNumberFloat" %>
<!-- SHORT TEXT -->
<div class="form-group">
    <label class="control-label">
        <asp:Literal ID="Literal_Label" runat="server"></asp:Literal></label><br>
    <asp:TextBox ID="TextBoxNumber_Value" runat="server" CssClass="form-control"></asp:TextBox>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator_TextBoxNumber_Value" runat="server" SetFocusOnError="true" Display="Dynamic"
        ControlToValidate="TextBoxNumber_Value" ErrorMessage="Campo obbligatorio" Enabled="false"></asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator ID="RegularExpressionValidator_TextBoxNumber_Value" SetFocusOnError="true" Display="Dynamic"
        ControlToValidate="TextBoxNumber_Value" ValidationExpression="^[\d]+([.][0-9]\d*)?$"
        ErrorMessage="Valore non valido" runat="server"></asp:RegularExpressionValidator>
</div>
<!-- INPUTS SPACER -->
<div class="inputs-spacer" id="spacerDiv" runat="server">
</div>
