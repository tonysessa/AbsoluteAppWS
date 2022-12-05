<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CmsDropDownList.ascx.cs" Inherits="backOffice.ucControls.CmsDropDownList" %>

<asp:Panel ID="Panel_Input" runat="server" CssClass="form-group">
    <div class="tf_button-group">
        <asp:HyperLink runat="server" ID="HyperLink_MasterContent" Visible="false" CssClass="tf_master btn btn-default btn-sm shiny icon-only"><i class="fa fa-comment-o"></i></asp:HyperLink>
        <asp:HyperLink runat="server" ID="HyperLink_InfoContent" Visible="false" CssClass="tf_help btn btn-default btn-sm shiny icon-only"><i class="fa fa-info-circle"></i></asp:HyperLink>
    </div>
    <label class="control-label">
        <asp:Literal ID="Literal_Label" runat="server"></asp:Literal></label>
    <asp:TextBox ID="TextBox_Value" runat="server" CssClass="hidden"></asp:TextBox>
    <asp:TextBox ID="TextBox_TextValue" CssClass="hidden" runat="server"></asp:TextBox>
    <select id="<%=TextBox_Value.ClientID %>_option" style="width: 100%;">
        <asp:Literal ID="Literal_FirstEmpty" runat="server" Visible="true"><option value="">-</option></asp:Literal>
        <asp:Repeater ID="Repeater_Option" runat="server">
            <ItemTemplate>
                <asp:Literal ID="Literal_Option" runat="server"></asp:Literal>
            </ItemTemplate>
        </asp:Repeater>
    </select>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator_TextBox_Value" runat="server" SetFocusOnError="true" Display="Dynamic" ControlToValidate="TextBox_Value" ErrorMessage="Required Field" Enabled="false"></asp:RequiredFieldValidator>
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
</asp:Panel>
<div class="inputs-spacer" id="spacerDiv" runat="server">
</div>
<script>
    $(document).ready(function () {
        $('#<%=TextBox_Value.ClientID %>_option').select2();
        $('#<%=TextBox_Value.ClientID %>_option').on('change', function (data) {
            $('#<%=TextBox_Value.ClientID %>').val($('#<%=TextBox_Value.ClientID %>_option').val()).change();
            //$('#<%=TextBox_TextValue.ClientID %>').val($('#<%=TextBox_Value.ClientID %>_option').select2('data')[0]['text']).change();            
        });
    });
</script>

