﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="action.aspx.cs" Inherits="backOffice.cmssections.action" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" ValidateRequest="false" %>

                                                    <asp:Panel ID="Panel_Related_CmsSubSections_List" CssClass="widget" runat="server" Visible="false" EnableViewState="false">
                                                      <div class="widget-header bordered-bottom bordered-themesecondary">
                                                          <span class="widget-caption">
                                                              CmsSubSections</span>
                                                      </div>
                                                      <div class="widget-body">
                                                          <div class="with-footer">
                                                              <div class="btn-group">
                                                                  <div class="dropdown">
                                                                      <asp:HyperLink ID="HyperLink_CmsSubSections_Action" NavigateUrl="javascript:;" runat="server" CssClass="btn btn-default dropdown-toggle" data-toggle="dropdown">Action Tools<i class="fa fa-sort-desc"></i></asp:HyperLink>
                                                                      <ul class="dropdown-menu dropdown-default">
                                                                          <li>
                                                                              <asp:HyperLink ID="HyperLink_CmsSubSections_Action_Add" CssClass="lnkActionList" runat="server"><i class="dropdown-icon fa fa-plus"></i>Add CmsSubSections</asp:HyperLink></li>
                                                                          <li class="divider">
                                                                              </li>
                                                                          <li>
                                                                              <asp:HyperLink ID="HyperLink_CmsSubSections_Action_Delete" CssClass="lnkActionList" runat="server"><i class="dropdown-icon fa fa-trash-o"></i>Delete</asp:HyperLink></li>
                                                                      </ul>
                                                                  </div>
                                                              </div>
                                                              <div class="btn-group pull-right">
                                                                <asp:HyperLink ID="HyperLink_CmsSubSections_ReorderRecord" runat="server" CssClass="btn btn-default lnkReorder" CausesValidation="false" Visible="true" NavigateUrl="javascript:enableSortable('Panel_Related_CmsSubSections_List');" ><i class='fa fa-sort'></i> Reorder record</asp:HyperLink>
                                                                <asp:HyperLink ID="HyperLink_CmsSubSections_ApplyReorder" runat="server" CssClass="btn btn-default lnkApply" CausesValidation="false" Visible="true" NavigateUrl="javascript:disableSortable('Panel_Related_CmsSubSections_List');" ><i class='fa fa-check'></i> Apply order</asp:HyperLink>
                                                              </div>
                                                              <div class="card-list card-stripe listsortable" contenttable="CmsSubSections">
                                                                  <asp:Repeater ID="Repeater_CmsSubSections" runat="server" EnableViewState="false">
                                                                      <ItemTemplate>
                                                                          <asp:Panel ID="Panel_Card" runat="server" CssClass="card card-inverse">
                                                                              <div class="card-block">
                                                                                  <div class="checkbox">
                                                                                      <label>
                                                                                          <input type="checkbox" ID="CmsSubSections_checkboxSel" Visible="true" runat="server" class="lnkCheckbox" />
                                                                                              <span class="text"></span></label>
                                                                                  </div>
                                                                                  <h3 class="card-title"><asp:Literal ID="Literal_Title" runat="server"></asp:Literal></h3>
                                                                                  <div class="btn-group">
                                                                                      <asp:HyperLink ID="HyperLink_Update" CssClass="lnkAction btn btn-default btn-sm shiny icon-only" runat="server" ToolTip="Update" NavigateUrl="javascript:;"><i class="fa fa-edit"></i></asp:HyperLink>
                                                                                      <asp:HyperLink ID="HyperLink_Delete" CssClass="lnkAction btn btn-default btn-sm shiny icon-only" runat="server" ToolTip="Delete" NavigateUrl="javascript:;"><i class="fa fa-trash-o"></i></asp:HyperLink>
                                                                                  </div>
                                                                              </div>
                                                                          </asp:Panel>
                                                                      </ItemTemplate>
                                                                  </asp:Repeater>
                                                              </div>
                                                          </div>
                                                          <div class="footer">
                                                          </div>
                                                      </div>
                                                  </asp:Panel>

