$(document).ready(function(){
    const toastSuccess = '@TempData["ToastSuccess"]';
    const toastError = '@TempData["ToastError"]';
    if (toastSuccess) {
        toastr.success(toastSuccess);
    }

    if (toastError) {
        toastr.error(toastError);
    }
})