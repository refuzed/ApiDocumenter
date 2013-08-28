(function ($) {
    $(document).on('click', ".data-post-ajax", function () {
        var that = $(this);
        var mode = that.data("mode");

        $.ajax({
            url: that.data("action"),
            data: { payload: that.data("payload") },
            DataType: "HTML",
            success: function (data) {
                switch (mode) {
                    case "before":
                        that.before(data + ",<br/>");
                        break;
                    case "replace":
                    default:
                        that.parent().html(data);
                        break;
                }
            },
            error: function (err) {
                alert(err);
            }
        })
    });
})(jQuery);