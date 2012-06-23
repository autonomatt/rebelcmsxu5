﻿/// <reference path="/Areas/RebelCms/Scripts/RebelCms.System/NamespaceManager.js" />
/// <reference path="/Areas/RebelCms/Modules/RebelCmsTabs/RebelCmsTabs.js" />

RebelCms.System.registerNamespace("RebelCms.Editors");

(function ($, Base) {

    RebelCms.Editors.ImplementSectionEditor = Base.extend({

        _editorViewModel: null,

        constructor: function () {

            this._editorViewModel = $.extend({}, RebelCms.System.BaseViewModel, {
                parent: this, // Allways set
                availableSections: ko.observableArray([]),
                selectedSection: ko.observable(),
                save: function(e) {
                    e.preventDefault();
                    
                    // Build up code snippet
                    var snippet = "@section " + this.selectedSection() + "\n{\n    \n}\n";
                    
                    // Insert snippet into current editors code mirror instance
                    window.parent.editor.getCodeMirrorInstance().replaceSelection(snippet);
                    window.parent.editor.getCodeMirrorInstance().focus();
                    
                    // Close the dialog
                    this.cancel(e);
                },
                cancel: function(e) {
                    e.preventDefault();
                    
                    // Close the dialog
                    window.parent.$u.Sys.WindowManager.getInstance().hideModal();
                }
            });

        },

        init: function (o) {

            var _this = this;

            $.each(o.availableSections, function(idx, itm) {
                _this._editorViewModel.availableSections.push(itm);
            });
            
            //apply knockout js bindings
            ko.applyBindings(this._editorViewModel);
            
        }
    }, {

        _instance: null,

        // Singleton accessor
        getInstance: function () {
            if (this._instance == null)
                this._instance = new RebelCms.Editors.ImplementSectionEditor();
            return this._instance;
        }

    });

})(jQuery, base2.Base);