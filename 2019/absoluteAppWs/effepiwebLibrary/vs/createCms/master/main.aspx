<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main.aspx.cs" Inherits="backOffice.main" %>

<%@ Register Src="uc/sidebarMenu.ascx" TagName="sidebarMenu" TagPrefix="ucSidebarMenu" %>
<%@ Register Src="uc/toolbar.ascx" TagName="toolbar" TagPrefix="ucToolbar" %>
<%@ Register Src="uc/navbar.ascx" TagName="navbar" TagPrefix="ucNavBar" %>
<!DOCTYPE html>

<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="shortcut icon" href="assets/img/favicon.ico" type="image/x-icon" />
    <title>Notice Board</title>

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
    <form id="Form1" runat="server">
        <ucNavBar:navbar ID="navBar1" runat="server" />
        <div class="main-container container-fluid">
            <!-- Page Container -->
            <div class="page-container">
                <!-- Page Sidebar -->
                <div class="page-sidebar" id="sidebar">
                    <!-- Page Sidebar Header-->
                    <div class="sidebar-header-wrapper">
                    </div>
                    <!-- /Page Sidebar Header -->
                    <!-- Sidebar Menu -->
                    <ucSidebarMenu:sidebarMenu ID="sidebarMenu1" runat="server" />
                    <!-- /Sidebar Menu -->
                </div>
                <!-- Page Content -->
                <div class="page-content">
                    <div class="section-loading-container">
                        <div class="loader"></div>
                    </div>
                    <!-- Page Breadcrumb -->
                    <div class="page-breadcrumbs">
                    </div>
                    <!-- /Page Breadcrumb -->
                    <!-- Page Header -->
                    <div class="page-header position-relative">
                        <div class="header-title">
                            <h1>
                                <asp:Literal ID="Literal_Welcome" runat="server">Welcome</asp:Literal>
                                <asp:Literal ID="Literal_Login" runat="server"></asp:Literal>!
                            </h1>
                        </div>
                        <!--Header Buttons-->
                        <div class="header-buttons">
                            <a class="sidebar-toggler" href="#">
                                <i class="fa fa-arrows-h"></i>
                            </a>
                            <a class="refresh" id="refresh-toggler" href="">
                                <i class="glyphicon glyphicon-refresh"></i>
                            </a>
                            <a class="fullscreen" id="fullscreen-toggler" href="#">
                                <i class="glyphicon glyphicon-fullscreen"></i>
                            </a>
                        </div>
                        <!--Header Buttons End-->
                    </div>
                    <!-- /Page Header -->
                    <!-- Page Body -->
                    <div class="page-body">
                        <div class="row">
                            <div class="col-lg-12 col-sm-12 col-xs-12">
                                <div class="row">

                                    <div class="col-lg-4 col-sm-4 col-xs-12">
                                    </div>

                                    <div class="col-lg-4 col-sm-4 col-xs-12">
                                    </div>

                                    <asp:Panel ID="Panel_NewsletterInvii" runat="server" CssClass="col-lg-4 col-sm-4 col-xs-12" Visible="true">
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- /Page Body -->
                </div>
                <!-- /Page Content -->
            </div>
            <!-- /Page Container -->
            <!-- Main Container -->

        </div>
        <!-- END CONTAINER -->
        <ucToolbar:toolbar ID="toolbar1" runat="server" />

        <!--jquery Plug-In-->
        <script src="assets/js/jquery-ui-1.10.4.custom.js"></script>
        <script src="assets/js/slimscroll/jquery.slimscroll.min.js"></script>

        <!--Bootstrap Scripts-->
        <script src="assets/js/bootstrap.min.js"></script>

        <!--Beyond Scripts-->
        <script src="assets/js/beyond.min.js"></script>
        <script src="assets/js/bootbox/bootbox.js"></script>
        <script src="assets/js/datatable/jquery.dataTables.min.js"></script>
        <script src="assets/js/datatable/ZeroClipboard.js"></script>
        <script src="assets/js/datatable/dataTables.tableTools.min.js"></script>
        <script src="assets/js/datatable/dataTables.bootstrap.min.js"></script>
        <script src="assets/js/datatable/datatables-init.js"></script>

        <!--Page Related Scripts-->
        <script src="assets/js/toastr/toastr.js"></script>

        <!--Main functions-->
        <script src="assets/js/main.js"></script>

        <script>
            $(document).ready(function () {
                <%=responseScript %>
            });
        </script>
    </form>
</body>
</html>
