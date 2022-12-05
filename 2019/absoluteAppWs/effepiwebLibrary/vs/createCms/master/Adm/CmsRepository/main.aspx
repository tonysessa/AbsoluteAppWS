<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main.aspx.cs" Inherits="backOffice.cmsRepository.main" %>

<%@ Register Src="~/cms/uc/sidebarMenu.ascx" TagName="sidebarMenu" TagPrefix="ucSidebarMenu" %>
<%@ Register Src="~/cms/uc/toolbar.ascx" TagName="toolbar" TagPrefix="ucToolbar" %>
<%@ Register Src="~/cms/uc/navbar.ascx" TagName="navbar" TagPrefix="ucNavBar" %>
<%@ Register Src="~/cms/ucControls/CmsRepository.ascx" TagName="CmsRepository" TagPrefix="uc99" %>
<!DOCTYPE html>

<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="shortcut icon" href="../assets/img/favicon.ico" type="image/x-icon" />
    <title>Gestione Files</title>

    <!--Basic Styles-->
    <link href="../../assets/css/bootstrap.min.css" rel="stylesheet" />
    <link id="bootstrap_rtl_link" rel="stylesheet" />

    <!--Beyond styles-->
    <link href="../../assets/css/beyond.min.css" rel="stylesheet" />
    <link href="../../assets/css/font-awesome.min.css" rel="stylesheet" />
    <link href="../../assets/css/animate.min.css" rel="stylesheet" />
    <link href="../../assets/css/application.css" rel="stylesheet" />
    <link href="../../assets/css/add.css" rel="stylesheet" />
    <link href="../../assets/css/skins/default.min.css" rel="stylesheet" />

    <!--Skin Script: Place this script in head to load scripts for skins and rtl support-->
    <script src="../../assets/js/skins.min.js"></script>

    <!--Basic Scripts-->
    <script src="../../assets/js/jquery.min.js"></script>

</head>
<body class="p-repository">
    <form id="frmMain" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

        <!-- NavBar Menu -->
        <ucNavBar:navbar ID="navBar1" runat="server" CurrentContentTable="CmsRepository" />
        <!-- /NavBar Menu -->

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
                <!-- /Page Sidebar -->
                <!-- Page Content -->
                <div class="page-content">
                    <!-- Page Breadcrumb -->
                    <div class="page-breadcrumbs">
                    </div>
                    <!-- /Page Breadcrumb -->
                    <div class="page-header position-relative">
                        <div class="header-title">
                            <h1>
                                <asp:Literal ID="Literal_Cms_RecordListFor" runat="server"></asp:Literal>Gestione Files
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
                    <div class="page-body">
                        <div class="row">
                            <div class="col-xs-12 col-md-12">
                                <div class="widget">
                                    <uc99:CmsRepository ID="CmsRepositoryMain" runat="server"></uc99:CmsRepository>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- /Page Content -->
            </div>
            <!-- /Page Container -->
            <!-- Main Container -->

        </div>

        <!-- Repository -->
        <div class='modal fade' id='modal-delete' role='dialog' tabindex='-1'>
            <div class='modal-dialog'>
                <div class='modal-content'>
                    <div class='modal-header'>
                        <button aria-label='Close' class='close' data-dismiss='modal' type='button'>
                            <span aria-hidden='true'>&times</span>
                        </button>
                        <h4 class='modal-title'>Elimina FIle</h4>
                    </div>
                    <div class='modal-body'>
                        <p>Confermi di voler cancellare il file selezionati?<span></span></p>
                    </div>
                    <div class='modal-footer'>
                        <button class='btn btn-default' data-dismiss='modal' type='button'>Close</button>
                        <button class='btn btn-primary' data-action='js-confirm-delete' type='button'>Delete</button>
                    </div>
                </div>
            </div>
        </div>
        <div class='modal fade' id='modal-move' role='dialog' tabindex='-1'>
            <div class='modal-dialog'>
                <div class='modal-content'>
                    <div class='modal-header'>
                        <button aria-label='Close' class='close' data-dismiss='modal' type='button'>
                            <span aria-hidden='true'>&times</span>
                        </button>
                        <h4 class='modal-title'>Sposta Files</h4>
                    </div>
                    <div class='modal-body'>
                        <p>Confermi di voler spostare i file selezionati?<span></span></p>
                    </div>
                    <div class='modal-footer'>
                        <button class='btn btn-default' data-dismiss='modal' type='button'>Close</button>
                        <button class='btn btn-primary' data-action='js-confirm-move' type='button'>Move</button>
                    </div>
                </div>
            </div>
        </div>
        <div class='modal fade' id='modal-crop' role='dialog' tabindex='-1' data-backdrop='static' data-keyboard='false' data-backdrop='static'>
            <div class='modal-dialog'>
                <div class='modal-content'>
                    <div class='modal-header'>
                        <button aria-label='Close' class='close' data-dismiss='modal' type='button'>
                            <span aria-hidden='true'>&times</span>
                        </button>
                        <h4 class='modal-title'>Taglio Immagine</h4>
                    </div>
                    <div class='modal-body'>
                        <img id='box_image'>
                    </div>
                    <div class='modal-footer'>
                        <button class='btn btn-primary' data-action='js-confirm-generic' type='button'>Crop</button>
                    </div>
                </div>
            </div>
        </div>
        <!-- End Repository -->

        <!-- END NAV-COLUMN -->
        <!-- END RELATED -->

        <!-- END CONTENT EDITOR -->
        <ucToolbar:toolbar ID="toolbar1" runat="server" />

        <!--jquery Plug-In-->
        <script src="../../assets/js/jquery-ui-1.10.4.custom.js"></script>
        <script src="../../assets/js/slimscroll/jquery.slimscroll.min.js"></script>

        <!--Bootstrap Scripts-->
        <script src="../../assets/js/bootstrap.min.js"></script>

        <!--Beyond Scripts-->
        <script src="../../assets/js/beyond.min.js"></script>
        <script src="../../assets/js/bootbox/bootbox.js"></script>

        <!--Jquery Select2-->
        <script src="../../assets/js/select2/select2.js"></script>

        <!--Bootstrap Date Picker-->
        <script src="../../assets/js/datetime/bootstrap-datepicker.js"></script>

        <!--Page Related Scripts-->
        <script src="../../assets/js/toastr/toastr.js"></script>

        <!--Custom Scripts-->
        <script src="../../assets/js/functions.js"></script>
        <script src="../../assets/js/crop.js"></script>

    </form>
</body>
</html>
