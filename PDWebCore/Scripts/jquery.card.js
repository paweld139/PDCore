(function ($) {
    $.fn.card = function () {
        var header = $(this).find(".card-header"), body, icon;

        $(this).addClass("card mb-4");

        header.on("mouseover", function () { this.style.opacity = '0.8'; }).on("mouseout", function () { this.style.opacity = '1'; });

        header.click(function () {
            body = $(this).next();
            icon = $(this).find("span.fa:eq(0)");

            if (body.is(":visible")) {
                body.slideUp('slow');

                icon.attr('class', 'fa fa-chevron-down button-icon');
            }
            else {
                body.slideDown('slow');

                icon.attr('class', 'fa fa-chevron-up button-icon');
            }
        });
    }
})
(jQuery);