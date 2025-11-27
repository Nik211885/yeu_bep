// Cấu hình chung cho toastr
toastr.options = {
    closeButton: true,
    progressBar: true,
    positionClass: "toast-bottom-right",
    timeOut: 3000,
    extendedTimeOut: 1000,
    newestOnTop: true,
    showDuration: 300,
    hideDuration: 500,
    showMethod: "fadeIn",
    hideMethod: "fadeOut"
};

window.toast = {
    success: function(message, title) {
        toastr.success(message, title || "Thành công");
    },
    error: function(message, title) {
        toastr.error(message, title || "Lỗi");
    },
    info: function(message, title) {
        toastr.info(message, title || "Thông tin");
    },
    warning: function(message, title) {
        toastr.warning(message, title || "Cảnh báo");
    }
};

