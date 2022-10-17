$(function () {
    $("form").on("submit", function (e) {
        var $form = $(this);
        // クライアントバリデーションに成功したら submit を無効にする
        if ($form.valid()) {
            $(this).find("[data-disable-with]").each(function () {
                var $button = $(this);
                var text = $button.attr("data-disable-with");
                $button.val(text);
                $button.prop("disabled", true);
            });
        }
    });
});