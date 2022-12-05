<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CmsLabel.ascx.cs"
    Inherits="backOffice.ucControls.CmsLabel" %>
<!-- SHORT TEXT -->
<div class="form-group">
    <label class="control-label">
        <asp:Literal ID="Literal_Label" runat="server"></asp:Literal></label>:
    <asp:Literal ID="Literal_Value" runat="server"></asp:Literal>
</div>
<!-- INPUTS SPACER -->
<div class="inputs-spacer" id="spacerDiv" runat="server">
</div>
