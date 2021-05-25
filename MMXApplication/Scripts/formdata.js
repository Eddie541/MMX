$(function () {
    $('form').submit(function () {        
        if ($(this).valid()) {
            var val = $(this).find(".e-clicked").attr("id");
            var idval = val.substr(2);
            $.ajax({
                url: this.action,
                type: this.method,
                data: $(this).serialize(),
                success: function (result) {
                    $('#form' + idval).html(result);
                }
            });
        }
        return false;
    });   

});
