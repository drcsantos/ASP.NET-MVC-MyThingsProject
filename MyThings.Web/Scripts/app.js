(function () {
    "use strict";

    init();

    function init() {
        initPasteButtons();
        initDialogs();
        initTooltip();
    }

    function initTooltip() {
        $('[data-toggle="tooltip"]').tooltip();
    }

    function initPasteButtons() {
        $("button[data-paste-to]").on("click", function (e) {
            var $this = $(this),
                $input = $($this.data("paste-to"));

            if ($input) {
                $input.val("").select();
                document.execCommand("Paste");
                var text = $input.val();
                if (text === "") {
                    if (window.clipboardData)
                        text = window.clipboardData.getData("Text");

                    $input.val(text);
                }
            }
        });
    }

    function initDialogs() {
        initDeleteDialog();
    }

    function initDeleteDialog() {
        var dialog = $('<div class="modal fade"><div class="modal-dialog modal-sm"><div class="modal-content"><div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button><h4 class="modal-title">Confirmação</h4></div><div class="modal-body"></div><div class="modal-footer"><button type="button" class="btn btn-default" data-dismiss="modal">Não</button><button type="button" class="btn btn-primary btn-ok">Sim</button></div></div></div></div>');
        $("body").append(dialog);

        $("a[data-confirmation]").on("click", function (e) {
            e.preventDefault();
            e.stopPropagation();
            e.stopImmediatePropagation();

            var $this = $(this),
                href = $this.attr("href"),
                method = $this.data("method"),
                msg = $this.data('confirmation'),
                unbind = function () {
                    dialog.find("button.btn-ok").unbind("click");
                };

            if (!msg) {
                msg = "Deseja realmente excluir este item?";
            }

            dialog.find("div.modal-body").text(msg);
            dialog.find("button.btn-ok").on("click", function () {
                unbind();
                if (method) {
                    $.ajax({
                        url: href,
                        type: method.toUpperCase()
                    }).success(function () {
                        window.location.reload(true);
                    }).error(function (request, status, error) {
                        //TODO: Improve showing up error method
                        alert("Error: " + error);
                    });
                } else if (href) {
                    window.location = href;
                }
            });
            dialog.on('hidden.bs.modal', function (e) {
                unbind();
            })
            dialog.modal({ show: true });
        });
    }
})();