<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CmsTranslation.ascx.cs"
    Inherits="backOffice.ucControls.CmsTranslation" %>
<!-- ORIGINAL TEXT ACCORDION -->
<div class="inputContent">
    <div class="translations">
        <div class="original-text-open-single">
            <i class="icon-caret-right"></i>&nbsp;&nbsp;<a href="javascript:;">Master content</a>
        </div>
        <div class="original-text">
            <p>
                <asp:Literal ID="Literal_Original_Text" runat="server"></asp:Literal></p>
        </div>
    </div>
</div>
<div class="inputs-spacer" id="spacerDiv" runat="server">
</div>
