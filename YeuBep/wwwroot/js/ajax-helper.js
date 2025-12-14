(function($) {
    const loadingModalHTML = `<div id="loadingModal" style="display: none; position: fixed; z-index: 99999999999999; left: 0; top: 0; width: 100%; height: 100%; background-color: rgba(0,0,0,0.5);">
        <div style="position: absolute; top: 50%; left: 50%; transform: translate(-50%, -50%); background: white; padding: 30px; border-radius: 10px; text-align: center;">
            <div class="spinner" style="border: 4px solid #f3f3f3; border-top: 4px solid #3498db; border-radius: 50%; width: 40px; height: 40px; animation: spin 1s linear infinite; margin: 0 auto 15px;"></div>
            <p style="margin: 0; color: #333; font-size: 16px;">Đang tải...</p>
        </div>
    </div>
    <style>
        @keyframes spin {
            0% { transform: rotate(0deg); }
            100% { transform: rotate(360deg); }
        }
    </style>`;

    const confirmModalHTML = `<div id="confirmModal" style="display: none; position: fixed; z-index: 99999999999999; left: 0; top: 0; width: 100%; height: 100%; background-color: rgba(0,0,0,0.5);">
        <div style="position: absolute; top: 50%; left: 50%; transform: translate(-50%, -50%); background: white; padding: 0; border-radius: 10px; min-width: 400px; box-shadow: 0 4px 6px rgba(0,0,0,0.1);">
            <div style="padding: 20px 24px; border-bottom: 1px solid #e5e7eb;">
                <h3 id="confirmModalTitle" style="margin: 0; color: #111827; font-size: 18px; font-weight: 600;">Xác nhận</h3>
            </div>
            <div style="padding: 24px;">
                <p id="confirmModalMessage" style="margin: 0; color: #6b7280; font-size: 14px; line-height: 1.5;">Bạn có chắc chắn muốn xóa không?</p>
            </div>
            <div style="padding: 16px 24px; background: #f9fafb; border-top: 1px solid #e5e7eb; display: flex; justify-content: flex-end; gap: 12px; border-radius: 0 0 10px 10px;">
                <button id="confirmModalCancel" style="padding: 8px 16px; border: 1px solid #d1d5db; background: white; color: #374151; border-radius: 6px; cursor: pointer; font-size: 14px; font-weight: 500; transition: all 0.2s;">
                    Hủy
                </button>
                <button id="confirmModalConfirm" style="padding: 8px 16px; border: none; background: #dc2626; color: white; border-radius: 6px; cursor: pointer; font-size: 14px; font-weight: 500; transition: all 0.2s;">
                    Xóa
                </button>
            </div>
        </div>
    </div>
    <style>
        #confirmModalCancel:hover {
            background: #f3f4f6;
        }
        #confirmModalConfirm:hover {
            background: #b91c1c;
        }
    </style>`;

    if ($('#loadingModal').length === 0) {
        $('body').append(loadingModalHTML);
    }

    if ($('#confirmModal').length === 0) {
        $('body').append(confirmModalHTML);
    }

    $.apiHelper = {
        showLoading: function() {
            $('#loadingModal').fadeIn(200);
        },

        hideLoading: function() {
            $('#loadingModal').fadeOut(200);
        },

        showConfirm: function(options) {
            const defaults = {
                title: 'Xác nhận',
                message: 'Bạn có chắc chắn muốn xóa không?',
                confirmText: 'Xóa',
                cancelText: 'Hủy',
                onConfirm: function() {},
                onCancel: function() {}
            };

            const settings = $.extend({}, defaults, options);

            $('#confirmModalTitle').text(settings.title);
            $('#confirmModalMessage').text(settings.message);
            $('#confirmModalConfirm').text(settings.confirmText);
            $('#confirmModalCancel').text(settings.cancelText);

            $('#confirmModal').fadeIn(200);

            $('#confirmModalConfirm').off('click').on('click', function() {
                $('#confirmModal').fadeOut(200);
                settings.onConfirm();
            });

            $('#confirmModalCancel').off('click').on('click', function() {
                $('#confirmModal').fadeOut(200);
                settings.onCancel();
            });

            $('#confirmModal').off('click').on('click', function(e) {
                if (e.target.id === 'confirmModal') {
                    $('#confirmModal').fadeOut(200);
                    settings.onCancel();
                }
            });
        },

        call: function(options) {
            const defaults = {
                url: '',
                method: 'GET',
                data: null,
                contentType: 'application/json',
                dataType: 'json',
                showLoading: true,
                success: function(response) {},
                error: function(xhr) {}
            };

            const settings = $.extend({}, defaults, options);
            const method = settings.method;

            if (settings.showLoading) this.showLoading();

            let requestData = settings.data;
            if (settings.contentType === 'application/json' && settings.data && typeof settings.data === 'object') {
                requestData = JSON.stringify(settings.data);
            }

            $.ajax({
                url: settings.url,
                type: settings.method,
                data: requestData,
                contentType: settings.contentType,
                dataType: settings.dataType,
                success: function(response) {
                    console.log("SUCCESS:", response);
                    if (method !== 'GET') {
                        toast.success("Xử lý thành công");
                    }
                    if (typeof settings.success === 'function') {
                        settings.success(response);
                    }
                },
                error: function(xhr) {
                    console.log("ERROR:", xhr.status, xhr.responseJSON);

                    let messages = [];
                    if (Array.isArray(xhr.responseJSON)) {
                        messages = xhr.responseJSON.map(e => e.message);
                        toast.warning(messages.join("<br/>"));
                    } else if (xhr.responseJSON && xhr.responseJSON.errors) {
                        const errors = xhr.responseJSON.errors;
                        for (const key in errors) {
                            if (errors.hasOwnProperty(key)) {
                                messages.push(...errors[key]);
                            }
                        }
                        toast.warning(messages.join("<br/>"));
                    }
                    
                    if (typeof settings.error === 'function') {
                        settings.error(xhr);
                    }
                },
                complete: function() {
                    if (settings.showLoading) $.apiHelper.hideLoading();
                    if (typeof settings.complete === 'function') {
                        settings.complete();
                    }
                }
            });
        },

        get: function(url, options) {
            const settings = $.extend({}, options, {
                url: url,
                method: 'GET'
            });
            this.call(settings);
        },

        post: function(url, data, options) {
            const settings = $.extend({}, options, {
                url: url,
                method: 'POST',
                data: data
            });
            this.call(settings);
        },

        put: function(url, data, options) {
            const settings = $.extend({}, options, {
                url: url,
                method: 'PUT',
                data: data
            });
            this.call(settings);
        },

        delete: function(url, options) {
            const self = this;
            const defaults = {
                confirmTitle: 'Xác nhận xóa',
                confirmMessage: 'Bạn có chắc chắn muốn xóa không?',
                confirmText: 'Xóa',
                cancelText: 'Hủy',
                showConfirm: true
            };

            const settings = $.extend({}, defaults, options);

            const executeDelete = function() {
                const deleteSettings = $.extend({}, settings, {
                    url: url,
                    method: 'DELETE',
                });
                delete deleteSettings.confirmTitle;
                delete deleteSettings.confirmMessage;
                delete deleteSettings.confirmText;
                delete deleteSettings.cancelText;
                delete deleteSettings.showConfirm;

                self.call(deleteSettings);
            };

            if (settings.showConfirm) {
                this.showConfirm({
                    title: settings.confirmTitle,
                    message: settings.confirmMessage,
                    confirmText: settings.confirmText,
                    cancelText: settings.cancelText,
                    onConfirm: executeDelete
                });
            } else {
                executeDelete();
            }
        }
    };

    $.uploadFile = function(file, options) {
        const defaults = {
            getUrlEndpoint: '/api/extend/url-upload',
            success: function(response) {
                console.log('Upload success:', response);
            },
            error: function(error) {
                console.error('Upload error:', error);
                toastr.error('Upload thất bại: ' + error);
            }
        };

        const settings = $.extend({}, defaults, options);

        $('#loadingModal').fadeIn(200);

        $.ajax({
            url: settings.getUrlEndpoint,
            type: 'GET',
            dataType: 'json',
            success: function(urlResponse) {
                console.log('Upload success:', urlResponse);
                const uploadUrl = urlResponse.url;

                if (!uploadUrl) {
                    $('#loadingModal').fadeOut(200);
                    settings.error('Không lấy được upload URL');
                    return;
                }

                const formData = new FormData();
                formData.append('file', file);

                if (urlResponse.upload_preset) {
                    formData.append('upload_preset', urlResponse.upload_preset);
                }

                if (urlResponse.folder) {
                    formData.append('folder', urlResponse.folder);
                }

                $.ajax({
                    url: uploadUrl,
                    type: 'POST',
                    data: formData,
                    processData: false,
                    contentType: false,
                    success: function(uploadResponse) {
                        $('#loadingModal').fadeOut(200);
                        settings.success(uploadResponse);
                    },
                    error: function(xhr, status, error) {
                        $('#loadingModal').fadeOut(200);
                        settings.error('Upload failed: ' + error);
                    }
                });
            },
            error: function(xhr, status, error) {
                $('#loadingModal').fadeOut(200);
                settings.error('Không lấy được upload URL: ' + error);
            }
        });
    };

})(jQuery);