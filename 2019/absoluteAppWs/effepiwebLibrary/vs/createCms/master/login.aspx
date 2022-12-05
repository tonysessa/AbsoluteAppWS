<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="backOffice.login" %>

<!DOCTYPE html>

<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="shortcut icon" href="assets/img/favicon.ico" type="image/x-icon" />
    <title>Back-Office - Login</title>

    <!--Basic Styles-->
    <link href="assets/css/bootstrap.min.css" rel="stylesheet" />
    <link id="bootstrap_rtl_link" rel="stylesheet" />

    <!--Beyond styles-->
    <link href="assets/css/beyond.min.css" rel="stylesheet" />
    <link href="assets/css/font-awesome.min.css" rel="stylesheet" />
    <link href="assets/css/animate.min.css" rel="stylesheet" />
    <link href="assets/css/application.css" rel="stylesheet" />
    <link href="assets/css/add.css" rel="stylesheet" />
    <link href="assets/css/skins/default.min.css" rel="stylesheet" />

    <!--Skin Script: Place this script in head to load scripts for skins and rtl support-->
    <script src="assets/js/skins.min.js"></script>

    <!--Basic Scripts-->
    <script src="assets/js/jquery.min.js"></script>

</head>
<body>
    <form id="frmLogin" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

        <asp:Panel ID="Panel_login" runat="server" DefaultButton="Button_Invia" CssCclass="main-container container-fluid">
            <!-- Page Container -->
            <div class="page-container">

                <div class="login-container animated fadeInDown">
                    <div class="loginbox bg-white">
                        <div class="loginbox-title">LOGIN</div>

                        <div class="loginbox-or">
                            <div class="or-line"></div>
                        </div>
                        <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                            <p class="text-danger">
                                <asp:Literal runat="server" ID="FailureText" />
                            </p>
                        </asp:PlaceHolder>
                        <div class="loginbox-textbox">
                            <asp:TextBox ID="TextBox_Login" runat="server" CssClass="form-control" placeholder="Login"></asp:TextBox><br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator_Login" runat="server" ControlToValidate="TextBox_Login" Display="Dynamic" CssClass="text-danger" ErrorMessage="insert username" SetFocusOnError="True"></asp:RequiredFieldValidator>
                        </div>
                        <div class="loginbox-textbox">
                            <asp:TextBox ID="TextBox_Password" TextMode="Password" CssClass="form-control" runat="server" placeholder="Password"></asp:TextBox><br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator_Password" runat="server" ControlToValidate="TextBox_Password" Display="Dynamic" CssClass="text-danger" ErrorMessage="insert password" SetFocusOnError="True"></asp:RequiredFieldValidator>
                        </div>
                        <div class="loginbox-submit">
                            <asp:Button ID="Button_Invia" CssClass="btn btn-primary btn-block" runat="server" OnClick="Button_Invia_Click" Text="LOG IN" />
                        </div>
                        <div class="loginbox-signup">
                            <asp:Label ID="Label_response" ForeColor="Red" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="logobox">
                        <asp:Image ID="Image_Logo" runat="server" ImageUrl="~/cms/assets/img/logo.png" />
                    </div>
                </div>

            </div>
            <!-- /Page Container -->
            <!-- Main Container -->

        </asp:Panel>

        <asp:Panel ID="Panel_SelectContext" runat="server" CssClass="main-container container-fluid" Visible="false">
            <!-- Page Container -->
            <div class="page-container">

                <div class="login-container animated fadeInDown">
                    <div class="loginbox bg-white">
                        <div class="loginbox-title">LOGIN</div>

                        <div class="loginbox-or">
                            <div class="or-line"></div>
                        </div>

                        <div class="loginbox-textbox">
                            <div class="btn-group">
                                <a class="btn btn-default dropdown-toggle" data-toggle="dropdown" href="javascript:void(0);"><asp:Literal ID="Literal_SelectCountry" runat="server">SELECT COUNTRY</asp:Literal></a>
                                <a class="btn btn-default dropdown-toggle" data-toggle="dropdown" href="javascript:void(0);"><i class="fa fa-angle-down"></i></a>
                                <ul class="dropdown-menu" style="max-height:300px;overflow:auto;">
                                    <asp:Repeater ID="Repeater_CmsNlsContext" runat="server">
                                        <ItemTemplate>
                                            <li>
                                                <asp:HyperLink runat="server" ID="HyperLink_CmsNlsContext_Title" style="padding-top:1px;padding-bottom:1px;"></asp:HyperLink>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                            </div>
                        </div>
                    </div>
                    <div class="logobox">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/cms/assets/img/logo.png" />
                    </div>
                </div>

            </div>
            <!-- /Page Container -->
            <!-- Main Container -->

        </asp:Panel>

        <!--Basic Scripts-->
        <script src="assets/js/jquery.min.js"></script>

        <script src="assets/js/bootstrap.min.js"></script>
        <script src="assets/js/slimscroll/jquery.slimscroll.min.js"></script>

        <!--Beyond Scripts-->
        <script src="assets/js/beyond.min.js"></script>
    </form>
</body>
</html>


