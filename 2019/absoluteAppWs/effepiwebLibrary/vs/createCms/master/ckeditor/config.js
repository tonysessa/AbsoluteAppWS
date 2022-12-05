/*
Copyright (c) 2003-2012, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/
CKEDITOR.stylesSet.add('KWstyle', [
 { name: 'Green Kawasaki', element: 'span', styles: { 'color': '#69be28 !important'} }
 //{ name: 'Bold Kawasaki', element: 'span', styles: { 'font-family': 'delta_promedium','font-weight':'bold'} }
]);

 CKEDITOR.config.resize_enabled = false;

 CKEDITOR.editorConfig = function (config) {

     // Define changes to default configuration here. For example:
     config.toolbar = 'MyToolbar';
     config.skin = 'kama';
     config.stylesSet = 'KWstyle';
     config.language = 'en';
	 
     //
     config.fontSize_sizes = '10px/10px;11px/11px;12px/12px;13px/13px;14px/14px;15px/15px;16px/16px;17px/17px;18px/18px;';

     //
     config.enterMode = CKEDITOR.ENTER_BR;
     config.shiftEnterMode = CKEDITOR.ENTER_P;
     config.toolbar_MyToolbar =
	[
		{ name: 'basicstyles', items: ['Bold', 'Italic', '-', 'RemoveFormat'] },
        { name: 'styles', items: ['Styles'] },
        { name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'] },
		{ name: 'links', items: ['Link', 'Unlink'] }, '/',
        { name: 'clipboard', items: ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'] },
        { name: 'insert', items : [ 'Table' ] },
		{ name: 'document', items: ['Source'] },

     /*
     { name: 'document', items : [ 'Source','-','Save','NewPage','DocProps','Preview','Print','-','Templates' ] },
     { name: 'clipboard', items : [ 'Cut','Copy','Paste','PasteText','PasteFromWord','-','Undo','Redo' ] },
     { name: 'editing', items : [ 'Find','Replace','-','SelectAll','-','SpellChecker', 'Scayt' ] },
     { name: 'forms', items : [ 'Form', 'Checkbox', 'Radio', 'TextField', 'Textarea', 'Select', 'Button', 'ImageButton', 
     'HiddenField' ] },
     '/',
     { name: 'basicstyles', items : [ 'Bold','Italic','Underline','Strike','Subscript','Superscript','-','RemoveFormat' ] },
     { name: 'paragraph', items : [ 'NumberedList','BulletedList','-','Outdent','Indent','-','Blockquote','CreateDiv',
     '-','JustifyLeft','JustifyCenter','JustifyRight','JustifyBlock','-','BidiLtr','BidiRtl' ] },
     { name: 'links', items : [ 'Link','Unlink','Anchor' ] },
     { name: 'insert', items : [ 'Image','Flash','Table','HorizontalRule','Smiley','SpecialChar','PageBreak','Iframe' ] },
     '/',
     { name: 'styles', items : [ 'Styles','Format','Font','FontSize' ] },
     { name: 'colors', items : [ 'TextColor','BGColor' ] },
     { name: 'tools', items : [ 'Maximize', 'ShowBlocks','-','About' ] }ù
     */
	];
 };