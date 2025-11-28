$(document).ready(function(){
    $('#avatarInput').on('change', function () {
        const file = this.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function (e) {
                $('#avatarPreview').attr('src', e.target.result);
            };
            reader.readAsDataURL(file);
            $.uploadFile(file, {
                success: function (data) {
                    $("#avatar").val(data.url);
                }
            })
        }
    });
})