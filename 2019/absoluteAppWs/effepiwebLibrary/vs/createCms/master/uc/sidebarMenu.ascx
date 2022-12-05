<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="sidebarMenu.ascx.cs" Inherits="backOffice.uc.sidebarMenu" EnableViewState="false" %>


<ul class="nav sidebar-menu">
    <li runat="server" id="liHyperLink_Main">
        <asp:HyperLink ID="HyperLink_Main" runat="server" NavigateUrl="~/cms/main.aspx">
            <i class="menu-icon glyphicon glyphicon-th-large"></i><span class="menu-text">
                <asp:Literal ID="Literal_NoticeBoard" runat="server">Notice Board</asp:Literal></span>
        </asp:HyperLink></li>
    <li runat="server" id="liHyperLink_Calendar">
        <asp:HyperLink ID="HyperLink_Calendar" runat="server" NavigateUrl="~/cms/calendar.aspx">
            <i class="menu-icon glyphicon glyphicon-calendar"></i><span class="menu-text">Calendario</span></asp:HyperLink></li>
    <asp:Repeater ID="Repeater_Section" runat="server">
        <ItemTemplate>
            <li id="liHyperLink_Section_Title" runat="server">
                <asp:HyperLink ID="HyperLink_Section_Title" runat="server" CssClass="menu-dropdown" NavigateUrl="#"></asp:HyperLink>
                <ul class="submenu">
                    <asp:Repeater ID="Repeater_SubSection" runat="server">
                        <ItemTemplate>
                            <li id="liHyperLink_SubSection_Title" runat="server">
                                <asp:HyperLink ID="HyperLink_SubSection_Title" runat="server"></asp:HyperLink></li>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Repeater ID="Repeater_Filter" runat="server">
                        <ItemTemplate>
                            <li id="liHyperLink_SubSection_Title" runat="server">
                                <asp:HyperLink ID="HyperLink_SubSection_Title" runat="server"></asp:HyperLink></li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </li>
        </ItemTemplate>
    </asp:Repeater>
    <li>
        <a href="/cms/adm/CmsRepository/main.aspx" title=""><i class="menu-icon glyphicon glyphicon-folder-open"></i><span class="menu-text">
            <asp:Literal ID="Literal_ManageFiles" runat="server">Manage Files</asp:Literal></span></a></li>
    <li style="display: none">
        <asp:HyperLink ID="HyperLink_Impostazioni" runat="server"><i class="menu-icon glyphicon glyphicon-cog"></i><span class="menu-text">Settings</span></asp:HyperLink>
    </li>
</ul>



