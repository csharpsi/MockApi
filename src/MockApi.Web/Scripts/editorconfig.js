(function($) {
    /*var editor = ace.edit("editor");
    console.log(editor);
    var textarea = $("#ResponseJson").hide();
    editor.getSession().setValue(textarea.val());
    editor.getSession().on("change", function () {
        textarea.val(editor.getSession().getValue());
    });*/

    $(function() {

        $("#Mock_Path").focus();

        var textarea = $(".editor");

        textarea.ace({ theme: "github", lang: "json" });

        var editor = textarea.data("ace").editor.ace;

        editor.getSession().setTabSize(4);

        $(window).bind("keydown", function(event) {
            if (event.ctrlKey || event.metaKey) {

                var key = String.fromCharCode(event.which).toLocaleLowerCase();

                if (key === "s") {
                    event.preventDefault();
                    $("form").submit();
                }
            }
        });
    });

}(jQuery));