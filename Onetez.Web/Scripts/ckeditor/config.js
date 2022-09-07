/**
 * @license Copyright (c) 2003-2018, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function (config) {

    config.language = 'en';
    config.uiColor = '#f9f9f9';
    config.height = 400;
    config.allowedContent = false;
    config.entities_latin = false;
    config.syntaxhighlight_lang = 'csharp';
    config.syntaxhighlight_hideControls = true;

    config.extraPlugins = 'image2';

    config.toolbarGroups = [
        { name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
        { name: 'paragraph', groups: ['list', 'align', 'indent', 'blocks', 'bidi', 'paragraph'] },
        { name: 'colors', groups: ['colors'] },
        { name: 'links', groups: ['links', 'filetools'] },
        { name: 'document', groups: ['document', 'doctools', 'mode'] },
        { name: 'tools', groups: ['tools'] },
        { name: 'forms', groups: ['forms'] },
        { name: 'others', groups: ['others'] },
        { name: 'about', groups: ['about'] },
        { name: 'insert', groups: ['insert'] },
        { name: 'styles', groups: ['styles'] }
    ];

    config.removeButtons = 'Save,Print,Cut,Copy,Paste,PasteText,PasteFromWord,Scayt,SelectAll,Form,Radio,TextField,Textarea,Select,Button,HiddenField,CopyFormatting,BidiLtr,BidiRtl,Language,Anchor,Flash,SpecialChar,Smiley,PageBreak,About,ShowBlocks,Subscript,Superscript,HorizontalRule,ImageButton,NewPage,Preview,Templates,Styles,Font,FontSize,RemoveFormat,BGColor,Outdent,Indent,Blockquote,CreateDiv,Maximize,Source,Table,Iframe,JustifyBlock,Unlink,Undo,Redo,Checkbox,Underline,Link,JustifyLeft,JustifyCenter,JustifyRight';
    
};
