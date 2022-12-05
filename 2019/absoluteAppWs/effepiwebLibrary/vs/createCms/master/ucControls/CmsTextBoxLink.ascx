<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CmsTextBoxLink.ascx.cs"
    Inherits="backOffice.ucControls.CmsTextBoxLink" %>
<!-- SHORT TEXT -->
<asp:Panel ID="Panel_Input" runat="server" CssClass="form-group">
    <!-- -->
    <div class="tf_button-group">
        <asp:HyperLink runat="server" ID="HyperLink_MasterContent" Visible="false" CssClass="tf_master btn btn-default btn-sm shiny icon-only"><i class="fa fa-comment-o"></i></asp:HyperLink>
        <asp:HyperLink runat="server" ID="HyperLink_InfoContent" Visible="false" CssClass="tf_help btn btn-default btn-sm shiny icon-only"><i class="fa fa-info-circle"></i></asp:HyperLink>
    </div>
    <!-- -->
    <label class="control-label">
        <asp:Literal ID="Literal_Title" runat="server"></asp:Literal></label><br>
    <div class="row">
        <asp:Panel ID="Panel_Title" runat="server">
            <asp:Panel ID="Panel_Title_Content" runat="server" CssClass="col-md-5">
                <asp:Literal ID="Literal_Label_Title" runat="server">Title</asp:Literal><br />
                <asp:TextBox ID="TextBox_Title" runat="server" CssClass="form-control"></asp:TextBox>
            </asp:Panel>
        </asp:Panel>
        <asp:Panel ID="Panel_Value" runat="server">
            <asp:Panel ID="Panel_Value_Content" runat="server" CssClass="col-md-5">
                <asp:Literal ID="Literal_Label_Value" runat="server">Url</asp:Literal><br />
                <asp:TextBox ID="TextBox_Value" runat="server" CssClass="form-control"></asp:TextBox>
            </asp:Panel>
        </asp:Panel>
        <asp:Panel ID="Panel_Target" runat="server">
            <div class="col-md-2">
                <asp:Literal ID="Literal_Label_Target" runat="server"></asp:Literal>Target<br />
                <asp:DropDownList ID="DropDownList_Target" runat="server" CssClass="form-control">
                    <asp:ListItem Value="">-</asp:ListItem>
                    <asp:ListItem Value="_self">Self</asp:ListItem>
                    <asp:ListItem Value="_blank">Blank</asp:ListItem>
                </asp:DropDownList>
            </div>
        </asp:Panel>
    </div>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator_TextBox_Value" runat="server" Display="Dynamic" ControlToValidate="TextBox_Value" ErrorMessage="Required Field" Enabled="false"></asp:RequiredFieldValidator>
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
