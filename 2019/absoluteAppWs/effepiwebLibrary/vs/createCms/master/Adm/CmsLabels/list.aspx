<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="list.aspx.cs" EnableEventValidation="false" Inherits="backOffice.adm.cmslabels.list" %>

<%@ Register Src="~/cms/uc/sidebarMenu.ascx" TagName="sidebarMenu" TagPrefix="ucSidebarMenu" %>
<%@ Register Src="~/cms/uc/navbar.ascx" TagName="navbar" TagPrefix="ucNavBar" %>
<!DOCTYPE html>

<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="shortcut icon" href="/cms/assets/img/favicon.png" type="image/x-icon" />
    <title>Elenco CmsLabels</title>

    <!--Basic Styles-->
    <link href="/cms/assets/css/bootstrap.min.css" rel="stylesheet" />
    <link id="bootstrap_rtl_link" rel="stylesheet" />

    <!--Beyond styles-->
    <link href="/cms/assets/css/beyond.min.css" rel="stylesheet" />
    <link href="/cms/assets/css/font-awesome.min.css" rel="stylesheet" />
    <link href="/cms/assets/css/animate.min.css" rel="stylesheet" />
    <link href="/cms/assets/css/application.css" rel="stylesheet" />
    <link href="/cms/assets/css/add.css" rel="stylesheet" />
    <link href="/cms/assets/css/skins/default.min.css" rel="stylesheet" />

    <!--Skin Script: Place this script in head to load scripts for skins and rtl support-->
    <script src="/cms/assets/js/skins.min.js"></script>

    <!--Basic Scripts-->
    <script src="/cms/assets/js/jquery.min.js"></script>

</head>
<body>
    <form id="frmList" runat="server">
        <ucNavBar:navbar ID="navBar1" runat="server" CurrentContentTable="CmsLabels" />
        <div class="main-container container-fluid">
            <div class="page-container">
                <div class="page-sidebar" id="sidebar">
                    <div class="sidebar-header-wrapper">
                    </div>
                    <ucSidebarMenu:sidebarMenu ID="sidebarMenu1" runat="server" />
                </div>
                <div class="page-content">
                    <div class="section-loading-container">
                        <div class="loader"></div>
                    </div>
                    <div class="page-breadcrumbs">
                    </div>
                    <div class="page-header position-relative">
                        <div class="header-title">
                            <h1>CmsLabels
                            </h1>
                        </div>
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
                    </div>
                    <div class="page-body">
                        <div class="row">
                            <div class="col-xs-12 col-md-12">
                                <div class="widget">
                                    <div class="widget-header bordered-bottom bordered-primary">
                                        <span class="widget-caption">
                                            <asp:Literal ID="Literal_Cms_RecordListFor" runat="server">Elenco</asp:Literal>
                                            CmsLabels</span>
                                        <span class="widget-caption pull-right padding-right-20">
                                            <label>
                                                <asp:CheckBox ID="CheckBox_Show_Deleted" OnCheckedChanged="CheckBox_Show_Deleted_Click" AutoPostBack="true" Visible="true" runat="server" />
                                                <span class="text">
                                                    <asp:Literal ID="Literal_ShowDeleted" runat="server">Show deleted records</asp:Literal></span>
                                            </label>
                                        </span>
                                    </div>
                                    <div class="well">
                                        <div class="table-toolbar">
                                            <div class="btn-group margin-bottom-20">
                                                <div class="dropdown">
                                                    <asp:HyperLink ID="HyperLink_Action" NavigateUrl="javascript:;" runat="server" CssClass="btn btn-default dropdown-toggle" data-toggle="dropdown">Action Tools<i class="fa fa-sort-desc"></i></asp:HyperLink>
                                                    <ul class="dropdown-menu dropdown-palegreen">
                                                        <li class="bordered-bottom">
                                                            <asp:HyperLink ID="HyperLink_Action_Add" runat="server"><i class="dropdown-icon fa fa-plus"></i>Add record</asp:HyperLink>
                                                        </li>
                                                        <li class="divider"></li>
                                                        <li>
                                                            <asp:HyperLink ID="HyperLink_Action_Enable" CssClass="lnkActionList" runat="server"><i class="dropdown-icon fa fa-power-off success"></i>Enable</asp:HyperLink>
                                                        </li>
                                                        <li>
                                                            <asp:HyperLink ID="HyperLink_Action_Disable" CssClass="lnkActionList" runat="server"><i class="dropdown-icon fa fa-power-off danger"></i>Disable</asp:HyperLink>
                                                        </li>
                                                        <li>
                                                            <asp:HyperLink ID="HyperLink_Action_Delete" CssClass="lnkActionList" runat="server"><i class="dropdown-icon fa fa-trash-o"></i>Delete</asp:HyperLink>
                                                        </li>
                                                        <li>
                                                            <asp:HyperLink ID="HyperLink_Download_Excel" runat="server" Target="_blank" NavigateUrl="export.aspx"><i class="dropdown-icon fa fa-file-excel-o"></i>Export Excel</asp:HyperLink>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>
                                            <div class="btn-group margin-bottom-20">
                                                <asp:LinkButton ID="Button_ReorderRecord" runat="server" CssClass="btn btn-default" OnClick="Button_ReorderRecord_Click" CausesValidation="false" Visible="false"><i class='fa fa-sort'></i> Reorder record</asp:LinkButton>
                                                <asp:LinkButton ID="Button_ApplyReorder" runat="server" CssClass="btn btn-default disabled" OnClick="Button_ApplyReorder_Click" CausesValidation="false" Visible="false"><i class='fa fa-check'></i> Apply order</asp:LinkButton>
                                            </div>
                                            <asp:Panel ID="Panel_Search" runat="server" CssClass="pull-right form-inline padding-bottom-20" Visible="true" DefaultButton="LinkButton_Search">
                                                <asp:DropDownList ID="DropDownList_Search" CssClass="form-control" runat="server" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="LinkButton_Search_Click">
                                                    <asp:ListItem Value="">-</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:TextBox ID="TextBox_Search" runat="server" CssClass="form-control" placeholder="full-text search"></asp:TextBox>
                                                <asp:LinkButton ID="LinkButton_Search" CssClass="btn btn-default" runat="server" OnClick="LinkButton_Search_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                            </asp:Panel>
                                        </div>
                                        <div class="table-scrollable">
                                            <table class="table table-striped table-bordered table-hover">
                                                <thead>
                                                    <tr>
                                                        <th style="width: 35px;">
                                                            <div class="checkbox">
                                                                <label>
                                                                    <input type="checkbox" id="checkboxAll" visible="true" runat="server" class="lnkCheckboxHeader" />
                                                                    <span class="text"></span>
                                                                </label>
                                                            </div>
                                                        </th>
                                                        <th style="width: 140px;"></th>
                                                        <th class="record-data-header txt-middle">
                                                            <asp:Literal ID="Literal_Column_Header_Key" runat="server">Key</asp:Literal><asp:LinkButton ID="LinkButton_Column_Header_Key" OnClick="LinkButton_Column_Header_Click" CausesValidation="false" CommandArgument="Key" runat="server"><i id="iLinkButton_Column_Header_Key" runat="server" class="fa fa-sort - desc"></i></asp:LinkButton></th>
                                                            <th class="record-data-header txt-long"><asp:Literal ID="Literal_Column_Header_Description" runat="server">Description</asp:Literal></th>
                                                        <!--<th class="record-data-header txt-long"><asp:Literal ID="Literal_Column_Header_Note" runat="server">Note</asp:Literal></th>-->


                                                    </tr>
                                                </thead>
                                                <tbody class="listsortable" runat="server" id="tbody" contenttable="CmsLabels">
                                                    <asp:Repeater ID="Repeater_List_Column" runat="server">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <div class="checkbox">
                                                                        <label>
                                                                            <input type="checkbox" id="checkboxSel" visible="true" runat="server" class="lnkCheckbox" />
                                                                            <span class="text"></span>
                                                                        </label>
                                                                    </div>
                                                                </td>
                                                                <td style="white-space: nowrap;">
                                                                    <asp:HyperLink ID="HyperLink_Edit" CssClass="lnkAction btn btn-default btn-sm shiny icon-only" runat="server" ToolTip="Update"><i class="fa fa-edit"></i></asp:HyperLink>
                                                                    <asp:HyperLink ID="HyperLink_Delete" CssClass="lnkAction btn btn-default btn-sm shiny icon-only" runat="server" NavigateUrl="javascript:;" ToolTip="Delete"><i class="fa fa-trash-o"></i></asp:HyperLink>
                                                                    <asp:HyperLink ID="HyperLink_EnableDisable" CssClass="lnkAction btn btn-default btn-sm shiny icon-only" runat="server" NavigateUrl="javascript:;" ToolTip="Change status"><i class="fa fa-power-off" id="li_LinkButton_EnableDisable" runat="server"></i></asp:HyperLink>
                                                                </td>
                                                                <td class="record-data txt-middle">
                                                                    <asp:Literal ID="Literal_Column_Key" runat="server"></asp:Literal></td>
                                                                    <td class="record-data txt-long"><asp:Literal ID="Literal_Column_Description" runat="server"></asp:Literal></td>
                                                                <!--<td class="record-data txt-long"><asp:Literal ID="Literal_Column_Note" runat="server"></asp:Literal></td>-->

                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>
                                        </div>
                                        <asp:Panel ID="Panel_List_Pager_Content" runat="server" CssClass="table-toolbar" Visible="true">
                                            <ul id="ul_List_Pager" runat="server" class="pagination" visible="false">
                                                <li>
                                                    <asp:LinkButton ID="LinkButton_List_Pager_Start" OnClick="LinkButton_List_Pager_Click" CommandArgument="1" CausesValidation="false" runat="server"><i class="fa fa-angle-double-left"></i></asp:LinkButton></li>
                                                <li>
                                                    <asp:LinkButton ID="LinkButton_List_Pager_Prev" OnClick="LinkButton_List_Pager_Click" CausesValidation="false" runat="server"><i class="fa fa-angle-left"></i></asp:LinkButton></li>
                                                <asp:Repeater ID="Repeater_List_Pager" runat="server">
                                                    <ItemTemplate>
                                                        <li id="liPager" runat="server">
                                                            <asp:LinkButton ID="LinkButton_List_Pager" OnClick="LinkButton_List_Pager_Click" CausesValidation="false" runat="server"></asp:LinkButton></li>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                                <li>
                                                    <asp:LinkButton ID="LinkButton_List_Pager_Next" OnClick="LinkButton_List_Pager_Click" CausesValidation="false" runat="server"><i class="fa fa-angle-right"></i></asp:LinkButton></li>
                                                <li>
                                                    <asp:LinkButton ID="LinkButton_List_Pager_End" OnClick="LinkButton_List_Pager_Click" CausesValidation="false" runat="server"><i class="fa fa-angle-double-right"></i></asp:LinkButton></li>
                                            </ul>
                                            <asp:Panel ID="Panel_Pager_Find" runat="server" CssClass="pagination-search form-inline" Visible="true" DefaultButton="LinkButton_GoToPage" ValidationGroup="Pager_Find">
                                                <div class="input-group">
                                                    <asp:TextBox ID="TextBox_Pager_Find" runat="server" Text="" CssClass="form-control" ValidationGroup="Pager_Find"></asp:TextBox>
                                                    <span class="input-group-btn">
                                                        <asp:LinkButton ID="LinkButton_GoToPage" CssClass="btn btn-default" runat="server" OnClick="LinkButton_GoToPage_Click" ValidationGroup="Pager_Find">Go to page</asp:LinkButton>
                                                    </span>
                                                    <asp:RangeValidator ID="RangeValidator_TextBox_Pager_Find" runat="server" ErrorMessage="Invalid page" class="message-validator" ControlToValidate="TextBox_Pager_Find" ValidationGroup="Pager_Find"></asp:RangeValidator>
                                                </div>
                                            </asp:Panel>
                                            <asp:Panel ID="Panel_List_Pager_Size" runat="server" CssClass="pull-right">
                                                <ul class="pagination">
                                                    <li>
                                                        <a href="javascript:;">
                                                            <asp:Literal ID="Literal_Cms_RecordForPage" runat="server">Records for page:</asp:Literal></a>
                                                    </li>
                                                    <li id="liPagerSize10" runat="server">
                                                        <asp:LinkButton ID="Button_Paging_10" OnClick="Button_Paging_Click" Text="10" CommandArgument="10" runat="server" CausesValidation="false"></asp:LinkButton></li>
                                                    <li id="liPagerSize25" runat="server">
                                                        <asp:LinkButton ID="Button_Paging_25" OnClick="Button_Paging_Click" Text="25" CommandArgument="25" runat="server" CausesValidation="false"></asp:LinkButton></li>
                                                    <li id="liPagerSize50" runat="server">
                                                        <asp:LinkButton ID="Button_Paging_50" OnClick="Button_Paging_Click" Text="50" CommandArgument="50" runat="server" CausesValidation="false"></asp:LinkButton></li>
                                                    <li id="liPagerSize5000" runat="server">
                                                        <asp:LinkButton ID="Button_Paging_All" OnClick="Button_Paging_Click" Text="All" CommandArgument="5000" runat="server" CausesValidation="false"></asp:LinkButton></li>
                                                </ul>
                                            </asp:Panel>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!--jquery Plug-In-->
        <script src="/cms/assets/js/jquery-ui-1.10.4.custom.js"></script>
        <script src="/cms/assets/js/slimscroll/jquery.slimscroll.min.js"></script>

        <!--Bootstrap Scripts-->
        <script src="/cms/assets/js/bootstrap.min.js"></script>

        <!--Beyond Scripts-->
        <script src="/cms/assets/js/beyond.min.js"></script>
        <script src="/cms/assets/js/bootbox/bootbox.js"></script>
        <script src="/cms/assets/js/datatable/jquery.dataTables.min.js"></script>
        <script src="/cms/assets/js/datatable/ZeroClipboard.js"></script>
        <script src="/cms/assets/js/datatable/dataTables.tableTools.min.js"></script>
        <script src="/cms/assets/js/datatable/dataTables.bootstrap.min.js"></script>
        <script src="/cms/assets/js/datatable/datatables-init.js"></script>

        <!--Custom Scripts-->
        <script src="/cms/assets/js/functionList.js"></script>

        <!--Page Related Scripts-->
        <script src="/cms/assets/js/toastr/toastr.js"></script>

        <script>
            $(document).ready(function () {
            <%=responseScript %>
        });
        </script>
    </form>
</body>
</html>
