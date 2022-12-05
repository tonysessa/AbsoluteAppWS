<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CmsRepository.ascx.cs" Inherits="backOffice.ucControls.CmsRepository" %>

<div class='workspace well table-toolbar' data-details='' data-state='open'>
        <button class="close-repository" type="button" data-action="js-close-repository"><i class="fa fa-times"></i></button>
        <div class='wrapper'>
            <div class='rp_header'>
                <div class='rp_header__upper'>
                    <div class='input-group'>
                        <input class='form-control search-input' placeholder='Cerca in tutte le cartelle' type='text'>
                        <span class='input-group-btn'>
                            <button class='btn btn-default' data-action='js-search' type='button'>cerca</button>
                        </span>
                    </div>
                    <button class='close-repository' data-action='js-close-repository' type='button'>
                        <i class='fa fa-times'></i>
                    </button>
                </div>
                <div class='rp_header__center'>
                    <div class='rp_mainactions'>
                        <button class='btn btn-primary' data-action='js-create-folder' type='button'>
                            <i class='fa fa-plus-circle'></i>
                            Nuova Cartella
                        </button>
                        <button class='btn btn-primary' data-action='js-uploadfile' type='button'>
                            <input class='file-upload' multiple='' name='fileupload1' type='file'>
                            <i class='fa fa-upload'></i>
                            Aggiungi File
                        </button>
                        <div class="panel-right">

                            <span class="fake-label">Cambia Visualizzazione</span>
                            <label for="change_layout_01">
                                <input type="radio" name="change_layout" id="change_layout_01" value="1">
                                <span><i class="glyphicon glyphicon-th"></i></span>
                            </label>
                            <label for="change_layout_02">
                                <input type="radio" name="change_layout" id="change_layout_02" value="0">
                                <span><i class="glyphicon glyphicon-th-list"></i></span>
                            </label>
                            <button class='btn btn-primary' data-action='show-hide-detail' type='button'>
                                <i class='fa fa-info-circle'></i>
                                Mostra/Nascondi Dettagli
                            </button>
                        </div>
                    </div>
                </div>

            </div>

            <div class='rp_header__downer'>
                <div class='wrapper-uploadfiles'>
                    <button class='btn btn-default' data-action='js-delete-all' type='button'>
                        Elimina tutti i Files
                    </button>
                    <button class='btn btn-primary' data-action='js-send-files' type='button'>
                        <i class='fa fa-plus-circle'></i>
                        Aggiungi Files
                    </button>
                    <ol class='wrapper-uploadfiles--content'></ol>
                </div>
            </div>
            <!-- END c-uploadfiles -->
            <div class='wrapper-subheader'>
                <div class='rp_breadcumbs col-xs-6 page-breadcrumbs'></div>
                <div class='checkbox-box col-xs-2'>
                    <label>
                        <input class='checkbox-slider toggle colored-palegreen toggle_multiselection' type='checkbox' />
                        <span class='text'></span>
                        <span class='label-text'>Multi selezione</span>
                    </label>
                </div>
                <div class='search-box col-xs-4'>
                    <div class='input-group'>
                        <span class='input-icon'>
                            <input class='form-control' data-action='js-inpage-search' id='glyphicon-search' placeholder='Cerca nella cartella corrente' type='text'>
                            <i class='glyphicon glyphicon-search circular danger'></i>
                        </span>
                    </div>
                </div>
            </div>

            <div class='rp_content'>
                <!-- START repository -->
                <div class='rp_content__inner'>
                    <div class='rp_wrap'>
                    </div>
                </div>
                <!-- END repository -->
                <!-- START details -->
                <aside class='rp_details'>
                    <div class='rp_details__inner'>
                        <button class='btn-nostyle' data-action='show-hide-detail' type='button'>
                            <i class='fa fa-times'></i>
                        </button>
                        <div class='rp_folder__details'>
                            <h4 class="tl">Dettagli</h4>
                            <div class='rp_folder__content'>
                                <!-- -->
                            </div>
                        </div>
                        <div class='rp_file__details'>
                            <figure class='rp_preview'></figure>
                            <a class='btn btn-default btn-block' href='#' target='_blank' type='button'>Download</a>
                            <h4 class="tl">Dettagli</h4>
                            <div class='rp_file__content'>
                            </div>
                        </div>
                        <div class='rp_undefined__details'>
                            <figure class='rp_preview'>
                                <img src="/cms/assets/img/folder_empty.png">
                            </figure>
                            <h4 class="tl">Dettagli</h4>
                            <p>Non ci sono informazioni per questo file/cartella</p>
                        </div>
                    </div>
                    <div class='rp_actions'>
                        <h4 class="tl">OPZIONI</h4>
                        <button class='btn btn-default btn-block' data-action='js-renamefile' type='button'>Rinomina</button>
                        <div class='row btn-correlated' data-correlated='js-move'>
                            <div class='col-xs-6'>
                                <button class='btn btn-primary btn-block' data-action='js-confirmmove' type='button'>Sposta qui</button>
                            </div>
                            <div class='col-xs-6'>
                                <button class='btn btn-default btn-block' data-action='js-reset-actions' disabled='disabled' type='reset'>Reset</button>
                            </div>
                        </div>
                        <button class='btn btn-primary btn-block' data-action='js-move' type='button'>Sposta</button>
                        <button class='btn btn-danger btn-block' data-action='js-delete' type='button'>Elimina</button>
                        <button class='btn btn-default btn-block' data-action='js-selected' type='button'>Seleziona File</button>
                        <button class='btn btn-default btn-block' data-action='js-selected-folder' type='button'>Seleziona Cartella</button>
                    </div>
                </aside>
                <!-- END details -->
            </div>
        </div>

    </div>
<script>
    var globalVar = {
        wsUrl: "<%=sCmsStartingpage %>adm/CmsRepository/Ws/repository.asmx/",
        wsHandler: "<%=sCmsStartingpage %>adm/CmsRepository/handler/upload.ashx",
        CmsNlsContext: "<%=currentCmsUserSession.currentCmsNlsContext.Uid %>",
        CmsUsers: "<%=currentCmsUserSession.currentUid %>",
        Uid_Repository: "<%=currentCmsUserSession.LastUidCmsRepository %>",
        Uid_Parent: "<%=currentCmsUserSession.LastUidCmsRepositoryFolder %>",
        LabelSearch: "search by: ",
        default_Repository: "1",
        default_Folder: "1",
        CmsContents: "<%=sCmsStartingpage %>adm/CmsContents/"
    }
</script>
