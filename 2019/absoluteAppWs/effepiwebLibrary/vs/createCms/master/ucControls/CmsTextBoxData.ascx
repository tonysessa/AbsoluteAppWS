<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CmsTextBoxData.ascx.cs" Inherits="backOffice.ucControls.CmsTextBoxData" %>

<asp:Panel ID="Panel_Input" runat="server" CssClass="form-group">
    <!-- -->
    <div class="tf_button-group">
        <asp:HyperLink runat="server" ID="HyperLink_MasterContent" Visible="false" CssClass="tf_master btn btn-default btn-sm shiny icon-only"><i class="fa fa-comment-o"></i></asp:HyperLink>
        <asp:HyperLink runat="server" ID="HyperLink_InfoContent" Visible="false" CssClass="tf_help btn btn-default btn-sm shiny icon-only"><i class="fa fa-info-circle"></i></asp:HyperLink>
    </div>
    <!-- -->
    <label class="control-label">
        <asp:Literal ID="Literal_Label" runat="server"></asp:Literal></label>
    <div class="row">
        <div class="col-md-6">
            <div class="input-group">
                <asp:TextBox class="form-control datetime-picker" ID="TextBoxDate_Value" runat="server" type="text" data-date-format="dd/mm/yyyy"></asp:TextBox>
                <span class="input-group-addon"><i class="fa fa-calendar lnkCalendar"></i></span>
            </div>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator_TextBoxDate_Value" runat="server" Display="Dynamic"
                SetFocusOnError="true" ControlToValidate="TextBoxDate_Value" ErrorMessage="required"
                Enabled="false"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator_TextBoxDate_Value" Display="Dynamic"
                SetFocusOnError="true" ValidationExpression="(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)dd"
                ErrorMessage="Data non valida (gg/mm/aaaa)" runat="server" ControlToValidate="TextBoxDate_Value"
                Enabled="false"></asp:RegularExpressionValidator>
        </div>
        <div class="col-md-6">           
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

<script>
    $(document).ready(function () {

        //
        $("#<%=TextBoxDate_Value.ClientID %>").datepicker();            

        //
        $(".lnkCalendar").click(function () {
            //$(this).parent().trigger("click");
        });
    });
</script>
