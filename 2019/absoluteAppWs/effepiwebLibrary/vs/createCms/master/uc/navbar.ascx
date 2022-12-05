<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="navbar.ascx.cs" Inherits="backOffice.uc.navbar" EnableViewState="false" %>

<div class="navbar">
    <div class="navbar-inner">
        <div class="navbar-container">
            <!-- Navbar Barnd -->
            <div class="navbar-header pull-left">
                <a href="/cms/main.aspx" class="navbar-brand">
                    <small>
                        <img src="/cms/assets/img/logo-navbar.png" alt="" />
                    </small>
                </a>
            </div>
            <!-- /Navbar Barnd -->
            <!-- Sidebar Collapse -->
            <div class="sidebar-collapse" id="sidebar-collapse">
                <i class="collapse-icon fa fa-bars"></i>
            </div>

            <!-- /Sidebar Collapse -->
            <!-- Account Area and Settings -->
            <div class="navbar-header pull-right">
                <div class="navbar-account">
                    <ul class="account-area">
                        <li>
                            <div class="dropdown">
                                <div class="dropdown-toggle" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                    <asp:Literal ID="Literal_CurrentContext_Title" runat="server"></asp:Literal>
                                    <i class="icon fa fa-caret-down"></i>
                                    <asp:Literal ID="Literal_NumContext" runat="server" Visible="false"></asp:Literal>
                                </div>
                                <ul class="dropdown-menu pull-right" aria-labelledby="dropdownMenu1">
                                    <asp:Repeater ID="Repeater_CmsNlsContext" runat="server">
                                        <ItemTemplate>
                                            <li>
                                                <asp:HyperLink ID="HyperLink_CmsNlsContext_Title" runat="server">
                                                    <div class="clearfix">
                                                        <span class="pull-left">
                                                            <asp:Literal ID="Literal_CmsNlsContext_Title" runat="server"></asp:Literal></span>
                                                    </div>
                                                </asp:HyperLink>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                            </div>
                        </li>
                        <li>
                            <div class="dropdown">
                                <div class="dropdown-toggle" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                    <i class="fa fa-user"></i>
                                    <asp:Literal ID="Literal_Title" runat="server"></asp:Literal>
                                    <i class="icon fa fa-caret-down"></i>
                                </div>
                                <ul class="dropdown-menu pull-right">
                                    <li class="username">
                                        <a>
                                            <asp:Literal ID="Literal_Username" runat="server"></asp:Literal></a>
                                    </li>
                                    <li class="email">
                                        <asp:Literal ID="Literal_Email" runat="server"></asp:Literal>
                                    </li>
                                    <li>
                                        <asp:HyperLink ID="HyperLink_UserSetting" NavigateUrl="~/cms/user_settings.aspx" runat="server" Text="I tuoi dati"></asp:HyperLink>
                                    </li>
                                    <li>
                                        <asp:LinkButton ID="LinkButton_Logout" runat="server" Text="Esci" CausesValidation="false" OnClick="LinkButton_Logout_Click"></asp:LinkButton>
                                    </li>
                                </ul>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</div>
