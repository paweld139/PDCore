(function ($) {
    $.fn.panel = function (options) {
        var settings = $.extend({}, $.fn.panel.defaults, options);

        var ph = $(this).find(".panel-heading"), pb, ps;

        $(this).attr("class", "panel-" + settings.type);


        if (ph.find("span.fa-chevron-up").length) {
            $(this).addClass("panel");
        }

        ph.on("mouseover", function () { this.style.opacity = '0.5'; }).on("mouseout", function () { this.style.opacity = '1'; });

        ph.click(function () {
            pb = $(this).next();
            ps = $(this).find("span.fa:eq(0)");

            if (pb.is(":visible")) {
                pb.slideUp('slow');

                $(this).parent().attr('class', 'panel-' + settings.type);
                ps.attr('class', 'fa fa-chevron-down');
            }
            else {
                pb.slideDown('slow');

                $(this).parent().attr('class', 'panel panel-' + settings.type);
                ps.attr('class', 'fa fa-chevron-up');
            }
        });
    }

    $.fn.panel.defaults = {
        type: "info"
    };
})
(jQuery);