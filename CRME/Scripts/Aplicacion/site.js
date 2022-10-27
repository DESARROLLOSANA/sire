/*Variables*/
var countries = {
    'au': {
        center: { lat: -25.3, lng: 133.8 },
        zoom: 4
    },
    'br': {
        center: { lat: -14.2, lng: -51.9 },
        zoom: 3
    },
    'ca': {
        center: { lat: 62, lng: -110.0 },
        zoom: 3
    },
    'fr': {
        center: { lat: 46.2, lng: 2.2 },
        zoom: 5
    },
    'de': {
        center: { lat: 51.2, lng: 10.4 },
        zoom: 5
    },
    'mx': {
        center: { lat: 23.6, lng: -102.5 },
        zoom: 5
    },
    'nz': {
        center: { lat: -40.9, lng: 174.9 },
        zoom: 5
    },
    'it': {
        center: { lat: 41.9, lng: 12.6 },
        zoom: 5
    },
    'za': {
        center: { lat: -30.6, lng: 22.9 },
        zoom: 5
    },
    'es': {
        center: { lat: 40.5, lng: -3.7 },
        zoom: 5
    },
    'pt': {
        center: { lat: 39.4, lng: -8.2 },
        zoom: 6
    },
    'us': {
        center: { lat: 37.1, lng: -95.7 },
        zoom: 3
    },
    'uk': {
        center: { lat: 54.8, lng: -4.6 },
        zoom: 5
    }
};

/*Funciones*/
function ActualizarBadge(url, idCliente) {
    $.ajax({
        url: url,
        type: "POST",
        data: { idCliente },
        success: function (result) {
            $(".notificacion").text(result);
        }
    });
}

function showLoading(id) {
    id.removeClass('hide');
}

function hideLoading(id) {
    id.addClass('hide');
}

ConfigTab();
function ConfigTab() {
    $('.tab-panel').off('click');
    $('.tab-panel').on('click', function () {

        var head = $('.tab-panel');
        var body = $('.tab-body');
        var index = $(this).index();

        $.each(head, function (i, value) {
            $(value).removeClass('active');
        });

        $.each(body, function (i, value) {
            $(value).removeClass('active');
        });

        $(this).addClass('active');
        $('.tab-body:eq(' + index + ')').addClass("active");
    });
}

(function ($) {
    /*loading button*/
    $.fn.StartRotate = function (icon) {
        var element = this;
        element.find('.load-icon').removeClass(icon).addClass('mdi-autorenew');
        element.find('.load-icon').addClass('loading-icon')
    };

    $.fn.StopRotate = function (icon) {
        var element = this;
        element.find('.load-icon').one('animationiteration webkitAnimationIteration', function () {
            element.find('.load-icon').removeClass('mdi-autorenew').addClass(icon);
            element.find('.load-icon').removeClass("loading-icon");
        });
    };

    /*loading gif*/
    $.fn.StartLoading = function () {
        this.addClass("loading-show").removeClass("loading");
    };

    $.fn.StopLoading = function () {
        this.addClass("loading").removeClass("loading-show");
    };
})(jQuery);


//Luis lopez 

$('#mail-box h6').click(function (event) {
    event.preventDefault();
    $(this).addClass('active');
    $(this).siblings().removeClass('active');

    var ph = $(this).parent().height();
    var ch = $(this).next().height();

    if (ch > ph) {
        $(this).parent().css({
            'min-height': ch + 'px'
        });
    } else {
        $(this).parent().css({
            'height': 'auto'
        });
    }
});

function tabParentHeight() {
    var ph = $('#mail-box').height();
    var ch = $('#mail-box ul').height();
    if (ch > ph) {
        $('#mail-box').css({
            'height': ch + 'px'
        });
    } else {
        $(this).parent().css({
            'height': 'auto'
        });
    }
}

$(window).resize(function () {
    tabParentHeight();
});

$(document).resize(function () {
    tabParentHeight();
});
tabParentHeight();

function AgregarFormatoMoneda(cantidad) {
    var valor = cantidad.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
    if (valor.substring(0, 1) == ',') {
        valor = valor.substring(1);
    }

    return valor;
}

function EliminarComaMontosFormulario(form) {
    form.find(".monto").each(function () {
        $(this).val(EliminarComaMonto($(this).val()));
    });

    return form;
}

function EliminarComaMonto(monto) {
    return monto.replace(/,/g, "");
}

$.validator.addMethod(
    "regex",
    function (value, element, regexp) {
        var re = new RegExp(regexp);
        return this.optional(element) || re.test(value);
    },
    "El correo electrónico es incorrecto."
);

