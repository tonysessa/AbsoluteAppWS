<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CmsRepositoryModal.ascx.cs" Inherits="backOffice.ucControls.CmsRepositoryModal" %>

<div class='modal fade' id='modal-delete' role='dialog' tabindex='-1'>
    <div class='modal-dialog'>
        <div class='modal-content'>
            <div class='modal-header'>
                <button aria-label='Close' class='close' data-dismiss='modal' type='button'>
                    <span aria-hidden='true'>&times</span>
                </button>
                <h4 class='modal-title'>Delete</h4>
            </div>
            <div class='modal-body'>
                <p>Do you confirm that you would like to DELETE the following files<span></span></p>
            </div>
            <div class='modal-footer'>
                <button class='btn btn-default' data-dismiss='modal' type='button'>Close</button>
                <button class='btn btn-primary' data-action='js-confirm-delete' type='button'>Save changes</button>
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
                <h4 class='modal-title'>Move</h4>
            </div>
            <div class='modal-body'>
                <p>Do you confirm that you would like to MOVE the following files?<span></span></p>
            </div>
            <div class='modal-footer'>
                <button class='btn btn-default' data-dismiss='modal' type='button'>Close</button>
                <button class='btn btn-primary' data-action='js-confirm-move' type='button'>Save changes</button>
            </div>
        </div>
    </div>
</div>
<div class='modal fade' id='modal-crop' role='dialog' tabindex='-1' data-backdrop='static'>
    <div class='modal-dialog'>
        <div class='modal-content'>
            <div class='modal-header'>
                <button aria-label='Close' class='close' data-dismiss='modal' type='button'>
                    <span aria-hidden='true'>&times</span>
                </button>
                <h4 class='modal-title'>Crop files</h4>
            </div>
            <div class='modal-body'>
                <img id='box_image' />
            </div>
            <div class='modal-footer'>
                <button class='btn btn-primary' data-action='js-confirm-generic' type='button'>Save changes</button>
            </div>
        </div>
    </div>
</div>
