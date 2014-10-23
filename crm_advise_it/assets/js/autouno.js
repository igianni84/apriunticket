jQuery(function($){
	$.datepicker.regional['it'] = {
		closeText: 'Chiudi',
		prevText: '&#x3C;Prec',
		nextText: 'Succ&#x3E;',
		currentText: 'Oggi',
		monthNames: ['Gennaio','Febbraio','Marzo','Aprile','Maggio','Giugno',
			'Luglio','Agosto','Settembre','Ottobre','Novembre','Dicembre'],
		monthNamesShort: ['Gen','Feb','Mar','Apr','Mag','Giu',
			'Lug','Ago','Set','Ott','Nov','Dic'],
		dayNames: ['Domenica','Lunedì','Martedì','Mercoledì','Giovedì','Venerdì','Sabato'],
		dayNamesShort: ['Dom','Lun','Mar','Mer','Gio','Ven','Sab'],
		dayNamesMin: ['Do','Lu','Ma','Me','Gi','Ve','Sa'],
		weekHeader: 'Sm',
		dateFormat: 'dd/mm/yy',
		firstDay: 1,
		isRTL: false,
		showMonthAfterYear: false,
		yearSuffix: ''};
	$.datepicker.setDefaults($.datepicker.regional['it']);
});


jQuery.validator.setDefaults({
    errorPlacement: function(error, element) {
        error.appendTo(element.closest(".field"));
    },
	highlight: function(element, errorClass) {
		$(element).closest(".field").addClass(errorClass);
	},
	unhighlight: function(element, errorClass) {
		$(element).closest(".field").removeClass(errorClass);
	}
});

jQuery.extend(jQuery.validator.messages, {
       required: "Campo obbligatorio.",
       remote: "Controlla questo campo.",
       email: "Inserisci un indirizzo email valido.",
       url: "Inserisci un indirizzo web valido.",
       date: "Inserisci una data valida.",
       dateISO: "Inserisci una data valida (ISO).",
       number: "Inserisci un numero valido.",
       digits: "Inserisci solo numeri.",
       creditcard: "Inserisci un numero di carta di credito valido.",
       equalTo: "Il valore non corrisponde.",
       lettersonly: "Sono ammesse solo lettere.",
       accept: "Inserisci un valore con un&apos;estensione valida.",
       integer: "Inserisci un numero intero.",
       color: "Inserisci un colore in formato esadecimale (#RRGGBB).",
       maxlength: jQuery.validator.format("Non inserire pi&ugrave; di {0} caratteri."),
       minlength: jQuery.validator.format("Inserisci almeno {0} caratteri."),
       rangelength: jQuery.validator.format("Inserisci un valore compreso tra {0} e {1} caratteri."),
       range: jQuery.validator.format("Inserisci un valore compreso tra {0} e {1}."),
       max: jQuery.validator.format("Inserisci un valore minore o uguale a {0}."),
       min: jQuery.validator.format("Inserisci un valore maggiore o uguale a {0}.")
});
/*	
$.fn.cycle.transitions.customScrollHorz = function($cont, $slides, opts) {
	$cont.css('overflow','hidden').width();
	opts.before.push(function(curr, next, opts, fwd) {
		next.cycleW = $(next).width();
		curr.cycleW = $(curr).width();
		if (opts.rev)
			fwd = !fwd;
		$.fn.cycle.commonReset(curr,next,opts);
		opts.cssBefore.left = fwd ? (next.cycleW-1) : (1-next.cycleW);
		opts.animOut.left = fwd ? -curr.cycleW : curr.cycleW;
	});
	opts.cssFirst.left = 0;
	opts.cssBefore.top = 0;
	opts.animIn.left = 0;
	opts.animOut.top = 0;
};*/

$(window).resize(function() {
	resizeLogin();
});	

function resizeLogin() {
	d_w = $(window).width();
	$("#login-area .image").each(function() {
		block = $("#login-area");
		container_width = block.find(".container").width();
		text_width = block.find("form").outerWidth();
		console.log(container_width);
		if (d_w > container_width) {
			width = ((d_w - container_width) / 2) + (container_width-text_width);
		} else {
			width = container_width-text_width;
		}
		$(this).css("width",width);
	});
}

$(document).ready(function() {
	$("input[type=radio], input[type=checkbox], select").uniform();
	resizeLogin();
	$("#login-area input[type=radio]").on("click",function() {
		rif = $(this).data("rif");
		el = $("#"+rif);
		$(".login-opt-field").hide();
		el.show();
	});
	/*
	$(".content-gallery a").fancybox({
		overlayColor:'#171721',
		overlayOpacity:0.95,
		padding:0
	});
	$(".feedback-message .close").on('click', function() {
		$(".feedback-message").slideUp(200);
	});
	
	$("#header .menu .handle").on('click',function() {
		$("#header .menu").toggleClass("open");
	});
	$("#header .menu").on('click',function(event) {
		event.stopPropagation();
	});
	$("html, body").on('click',function(event) {
		$("#header .menu").removeClass("open");
	});
	fixHighlights();
	
	$(".slider").cycle({
		fx:(Modernizr.touch) ? 'customScrollHorz' : 'fade',
		slideResize:0,
		fit:1,
		slideExpr: '.slide',
		pause: true,
		timeout: 6000,
		prev: $(".slider .prev"),
		next: $(".slider .next"),
		speed: (Modernizr.touch) ? 600 : 1200,
		before: function(currSlideElement, nextSlideElement, options, forwardFlag) {
			box = $(nextSlideElement).show().find(".box");
			box.css({marginTop:box.outerHeight()/2*-1});
		}
	});
	
	$(".slider").touchwipe({
	     wipeLeft: function() {
	     	$(".slider").cycle("next");
	     },
	     wipeRight: function() {
	     	$(".slider").cycle("prev");
	     },
	     min_move_x: 20,
	     min_move_y: 20,
	     preventDefaultEvents: false
	});
	
	$(".options-handle").on('click',function() {
		$(this).parents(".page-title").find(".more-options").toggleClass("open");
	});
	
	$(".options .plus a").on('click',function() {
		$(this).parents(".page-title").find(".more-options").toggleClass("open");
	});	
	
	$(".more-options .switch-page select").on('change',function() {
		window.location.href = $(this).val();
	});
	$(".more-options form select").on('change',function() {
		if ($(this).attr("name") == 'marca') {
			$(".more-options form select[name=modello]").val("");
		}
		$(this).parents("form").submit();
	});*/
//	.more-options
	
});

$(window).load(function() {
	//fixHighlights();
});

$(window).resize(function() {
	//fixHighlights();
});
/*
function fixHighlights() {
	//height = $(window).height()-$("#home-bottom").outerHeight();
	height = $(window).height();
	//if (height < 450) height = 450;
	$(".highlights").css({"height":height});
	$(".highlights .box").each(function() {
		$(this).css({marginTop:$(this).outerHeight()/2*-1});
	});
}*/