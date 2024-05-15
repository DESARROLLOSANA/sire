
function ConfigModal(modal, title, message, tipo) {
    var icono = modal.find(".md-icono");
    var titulo = modal.find(".md-title");
    var mensaje = modal.find(".md-message");
    var cancelar = modal.find(".md-cancel");
    var aceptar = modal.find(".md-accept");

    icono.removeClass("mdi-check-circle").removeClass("md-succes")
        .removeClass("mdi-information").removeClass("md-warning")
        .removeClass("mdi-close-circle").removeClass("md-danger")
        .removeClass("mdi-help-circle").removeClass("md-question")
    cancelar.removeClass("hide");
    aceptar.removeClass("modal-close");
    aceptar.attr("href", "javascript:void(0)");

    switch (tipo) {
        case 1:
            icono.addClass("mdi-check-circle").addClass("md-succes");
            cancelar.addClass("hide");
            aceptar.addClass("modal-close");
            break;
        case 2:
            icono.addClass("mdi-information").addClass("md-warning");
            cancelar.addClass("hide");
            aceptar.addClass("modal-close");
            break;
        case 3:
            icono.addClass("mdi-close-circle").addClass("md-danger");
            cancelar.addClass("hide");
            aceptar.addClass("modal-close");
            break;
        case 4:
            icono.addClass("mdi-help-circle").addClass("md-question");
            break;
    }
    titulo.text(title);
    mensaje.text(message);
}

function ConfigFooter(modal, url) {
    var aceptar = modal.find(".md-accept");
    if (url != undefined && url != "") {
        aceptar.attr("href", url);
    }
}
