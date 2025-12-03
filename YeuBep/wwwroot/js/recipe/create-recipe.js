$(document).ready(function() {
    $('#main-upload').on('click', function(e) {
        const $target = $(e.target);
        
        if ($target.hasClass('remove-image') || $target.closest('.remove-image').length) {
            return;
        }
        const fileInput = document.getElementById('main-image-input');
        fileInput.click();
    });

    $('#main-image-input').on('change', function(e) {
        const file = e.target.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function(event) {
                const imageUrl = event.target.result;
                $('#main-upload').css({
                    'background-image': 'url("' + imageUrl + '")',
                    'background-size': 'cover',
                    'background-position': 'center'
                });
                $('#main-upload').addClass('has-image');
                $.uploadFile(file, {
                    success: function(result) {
                        console.log(result);
                        $("#main-upload-image-url").val(result.url);
                    }
                })
            };
            reader.onerror = function(error) {
                console.error('Error reading file:', error);
            };
            reader.readAsDataURL(file);
        } else {
            console.log('No file selected');
        }
    });

    $('#remove-main-image').on('click', function(e) {
        console.log('Remove main image');
        e.stopPropagation();
        e.preventDefault();
        $('#main-upload').css('background-image', '');
        $('#main-upload').removeClass('has-image');
        $('#main-image-input').val('');
    });
    
    $(document).on('click', '.image-placeholder', function(e) {
        const $target = $(e.target);
        
        if ($target.hasClass('remove-step-image') || $target.closest('.remove-step-image').length > 0) {
            console.log('Clicked on remove button, ignoring');
            return;
        }
        const fileInput = $(this).find('.step-image-input')[0];
        if (fileInput) {
            fileInput.click();
        } else {
            console.error('File input not found!');
        }
    });

    $(document).on('change', '.step-image-input', function(e) {
        const file = e.target.files[0];
        if (file) {
            const $currentInput = $(this);
            const reader = new FileReader();
            const $placeholder = $(this).closest('.image-placeholder');
            reader.onload = function(event) {
                const imageUrl = event.target.result;
                $placeholder.css({
                    'background-image': 'url("' + imageUrl + '")',
                    'background-size': 'cover',
                    'background-position': 'center'
                });
                $placeholder.addClass('has-image');
                $.uploadFile(file, {
                    success: function(result) {
                        console.log(result);
                        $currentInput.siblings('.step-image-description').val(result.url);
                    }
                })
            };
            reader.onerror = function(error) {
                console.error('Error reading step image file:', error);
            };
            reader.readAsDataURL(file);
        } else {
            console.log('No step image file selected');
        }
    });

    $(document).on('click', '.remove-step-image', function(e) {
        console.log('Remove step image');
        e.stopPropagation();
        e.preventDefault();
        const $placeholder = $(this).closest('.image-placeholder');
        $placeholder.css('background-image', '');
        $placeholder.removeClass('has-image');
        $placeholder.find('.step-image-input').val('');
    });
    
    function initIngredientSortable() {
        $('.ingredients-list').each(function() {
            if (!$(this).hasClass('ui-sortable')) {
                $(this).sortable({
                    handle: '.drag-handle',
                    placeholder: 'sortable-placeholder',
                    cursor: 'move',
                    axis: 'y',
                    opacity: 0.8,
                    tolerance: 'pointer',
                    items: '.list-item'
                });
            }
        });
    }
    initIngredientSortable();
    
    $('#ingredients-sections').sortable({
        handle: '.ingredient-section-header',
        placeholder: 'sortable-placeholder',
        cursor: 'move',
        axis: 'y',
        opacity: 0.8,
        items: '.ingredient-section',
        tolerance: 'pointer'
    });
    
    $('#steps-list').sortable({
        handle: '.step-number',
        placeholder: 'sortable-placeholder-step',
        cursor: 'move',
        axis: 'y',
        opacity: 0.8,
        tolerance: 'pointer',
        update: function(event, ui) {
            $('#steps-list .step-item').each(function(index) {
                $(this).find('.step-number').text(index + 1);
            });
        }
    });
    
    $('.toggle-icon').on('click', function() {
        $(this).text(function(i, text) {
            return text === '−' ? '+' : '−';
        });
        $(this).closest('.section-header').next('.section-content').slideToggle(200);
    });
    
    $(document).on('click', '.add-ingredient-btn', function() {
        const $list = $(this).siblings('.ingredients-list');
        const newItem = `
                    <div class="list-item">
                        <span class="drag-handle">☰</span>
                        <input type="text" class="item-input ingredient" placeholder="Nhập nguyên liệu...">
                        <button class="more-btn">⋯</button>
                    </div>
                `;
        $list.append(newItem);
    });
    
    $('#add-section').on('click', function() {
        const sectionCount = $('.ingredient-section').length + 1;
        const newSection = `
                    <div class="ingredient-section" id="ingredient-section-${sectionCount}">
                        <div class="ingredient-section-header">
                            <input type="text" class="ingredient-section-title" placeholder="Phần ${sectionCount}" value="Phần ${sectionCount}">
                            <button class="delete-section-btn">🗑️</button>
                        </div>
                        <div class="ingredients-list">
                            <div class="list-item">
                                <span class="drag-handle">☰</span>
                                <input type="text" class="item-input ingredient" placeholder="Nhập nguyên liệu...">
                                <button class="more-btn">⋯</button>
                            </div>
                        </div>
                        <button class="add-btn add-ingredient-btn">+ Nguyên liệu</button>
                    </div>
                `;
        $(this).before(newSection);
        initIngredientSortable();
    });
    
    $(document).on('click', '.delete-section-btn', function() {
        if ($('.ingredient-section').length > 1) {
            if (confirm('Bạn có chắc muốn xóa phần này?')) {
                $(this).closest('.ingredient-section').remove();
            }
        } else {
            alert('Không thể xóa phần cuối cùng!');
        }
    });
    
    $('#add-step').on('click', function() {
        const stepNumber = $('#steps-list .step-item').length + 1;
        const newStep = `
                    <div class="step-item">
                        <div class="step-number">${stepNumber}</div>
                        <div class="step-content">
                            <textarea class="step-textarea" placeholder="Nhập hướng dẫn cho bước này..."></textarea>
                            <div class="image-placeholder" data-step="${stepNumber}">
                                <input type="hidden" class="step-image-description"/>
                                <input type="file" class="step-image-input" accept="image/*">
                                <button class="remove-step-image">✕</button>
                                <div class="image-icon">📷</div>
                            </div>
                            <button class="more-btn step-more">⋯</button>
                        </div>
                    </div>
                `;
        $('#steps-list').append(newStep);
    });
    
    $(document).on('click', '.list-item .more-btn', function(e) {
        e.stopPropagation();
        const $item = $(this).closest('.list-item');
        const $list = $item.closest('.ingredients-list');

        // Tạo menu contextual
        if ($('.context-menu').length) {
            $('.context-menu').remove();
        }

        const $menu = $(`
                    <div class="context-menu" style="position: absolute; background: white; border: 1px solid #ddd; border-radius: 6px; box-shadow: 0 2px 8px rgba(0,0,0,0.15); z-index: 1000; min-width: 150px;">
                        <div class="menu-item" data-action="duplicate" style="padding: 10px 16px; cursor: pointer; border-bottom: 1px solid #f0f0f0;">📋 Sao chép</div>
                        <div class="menu-item" data-action="delete" style="padding: 10px 16px; cursor: pointer; color: #ff6b35;">🗑️ Xóa</div>
                    </div>
                `);

        const offset = $(this).offset();
        $menu.css({
            top: offset.top + 30,
            left: offset.left - 100
        });

        $('body').append($menu);

        $menu.find('.menu-item').hover(
            function() { $(this).css('background', '#f5f5f5'); },
            function() { $(this).css('background', 'white'); }
        );

        $menu.find('[data-action="duplicate"]').on('click', function() {
            const $clone = $item.clone();
            $item.after($clone);
            $list.sortable('refresh');
            $menu.remove();
        });

        $menu.find('[data-action="delete"]').on('click', function() {
            $item.fadeOut(200, function() {
                $(this).remove();
            });
            $menu.remove();
        });
        
        $(document).one('click', function() {
            $menu.remove();
        });

        return false;
    });
    
    $(document).on('click', '.step-more', function(e) {
        e.stopPropagation();
        const $step = $(this).closest('.step-item');
        
        if ($('.context-menu').length) {
            $('.context-menu').remove();
        }

        const $menu = $(`
                    <div class="context-menu" style="position: absolute; background: white; border: 1px solid #ddd; border-radius: 6px; box-shadow: 0 2px 8px rgba(0,0,0,0.15); z-index: 1000; min-width: 150px;">
                        <div class="menu-item" data-action="duplicate" style="padding: 10px 16px; cursor: pointer; border-bottom: 1px solid #f0f0f0;">📋 Sao chép</div>
                        <div class="menu-item" data-action="delete" style="padding: 10px 16px; cursor: pointer; color: #ff6b35;">🗑️ Xóa</div>
                    </div>
                `);

        const offset = $(this).offset();
        $menu.css({
            top: offset.top + 30,
            left: offset.left - 100
        });

        $('body').append($menu);

        $menu.find('.menu-item').hover(
            function() { $(this).css('background', '#f5f5f5'); },
            function() { $(this).css('background', 'white'); }
        );

        $menu.find('[data-action="duplicate"]').on('click', function() {
            const $clone = $step.clone();
            $step.after($clone);
            $('#steps-list').sortable('refresh');
            $('#steps-list .step-item').each(function(index) {
                $(this).find('.step-number').text(index + 1);
            });
            $menu.remove();
        });

        $menu.find('[data-action="delete"]').on('click', function() {
            if ($('#steps-list .step-item').length > 1) {
                $step.fadeOut(200, function() {
                    $(this).remove();
                    $('#steps-list .step-item').each(function(index) {
                        $(this).find('.step-number').text(index + 1);
                    });
                });
            } else {
                const $warning = $('<div style="position: fixed; top: 20px; left: 50%; transform: translateX(-50%); background: #ff6b35; color: white; padding: 12px 24px; border-radius: 6px; z-index: 10000; box-shadow: 0 2px 8px rgba(0,0,0,0.2);">Không thể xóa bước cuối cùng!</div>');
                $('body').append($warning);
                setTimeout(() => $warning.fadeOut(300, () => $warning.remove()), 2000);
            }
            $menu.remove();
        });
        
        $(document).one('click', function() {
            $menu.remove();
        });

        return false;
    });
    function aggregateDataFrom(){
        const title = $("#title").val();
        const description = $("#description").val();
        const avatar = $("#main-upload-image-url").val();
        const portionCount = $("#portionCount").val();
        const timeToCook = $("#timeToCook").val();
        const ingredientPart = [];
        const detailInstructionSteps = [];
        $(".ingredient-section").each(function(){
            const $block = $(this);
            const title = $block.find(".ingredient-section-title").val();
            const ingredients = [];
            $block.find(".ingredient").each(function() {
                const ingredientName = $(this).val();
                ingredients.push(ingredientName);
            });
            ingredientPart.push({
                title: title,
                ingredients: ingredients
            });
        })
        $(".step-item").each(function(){
            const $block = $(this);
            const stepDescription = $block.find(".step-textarea").val();
            const stepImageDescription = $block.find(".step-image-description").val();
            detailInstructionSteps.push({
                instructions:stepDescription,
                imageDescription: stepImageDescription,
            });
        })
        return {
            title: title,
            description: description,
            avatar: avatar,
            portionCount: portionCount,
            timeToCook: timeToCook,
            ingredientPart: ingredientPart,
            detailInstructionSteps: detailInstructionSteps,
        };
    }
    function setButton(res) {
        const $container = $("#recipe-container");
        const $status = $("#recipe-status");
        const $btnSend = $("#btn-send");
        const $btnDelete = $("#btn-delete");
        const $btnSave = $("#btn-save");
        const $btnUnPublish = $("#unpublish");
        let recipeStatus;
        if(res){
            $container.data("id", res.id);
            recipeStatus = res.recipeStatus;
        }
        else{
            recipeStatus = $status.text().trim();
        }
        let statusName = "";
        let statusColor = "";
        switch (recipeStatus) {
            case "Send":
                statusColor = "bg-warning";
                $btnSend.hide();
                $btnSave.hide();
                $btnDelete.show();
                $btnUnPublish.hide();
                statusName = "Chờ phê duyệt";
                break;

            case "Accept":
                $btnSend.hide();
                $btnSave.hide();
                $btnUnPublish.show();
                $btnDelete.show();
                statusColor = "bg-success";
                statusName = "Đã phê duyệt";
                break;

            case "Reject":
                $btnSend.show();
                $btnSave.hide();
                $btnDelete.show();
                $btnUnPublish.hide();
                statusColor = "bg-error";
                statusName = "Từ chối";
                break;

            case "Draft":
                $btnSend.show();
                $btnSave.show();
                $btnDelete.show();
                $btnUnPublish.hide();
                statusColor = "bg-info";
                statusName = "Nháp";
                break;
        }
        console.log(statusName);
        console.log(statusColor);
        $status
            .removeClass("bg-info bg-success bg-error")
            .addClass(statusColor)
            .text(statusName);
    }

    $("#btn-send").on("click", function() {
        const recipeId = $("#recipe-container").data("id");
        let url = "/api/recipe/send";
        if(recipeId){
            url += "?recipeId=" + recipeId;
        }
        const data = aggregateDataFrom();
        $.apiHelper.post(url, data, {
            success: function(res) {
                console.log(res);
                setButton(res);
            }
        })
    })
    $("#btn-save").on("click", function() {
        const recipeId = $("#recipe-container").data("id");
        let url = "";
        if(recipeId){
            url = "/api/recipe/update?recipeId=" + recipeId;
        }
        else{
            url = "/api/recipe/create";
        }
        const data = aggregateDataFrom();
        $.apiHelper.post(url, data, {
            success: function(res) {
                setButton(res);
            }
        })
    })
    $("#btn-delete").on("click", function() {
        const recipeId = $("#recipe-container").data("id");
        if(recipeId){
            const url = "/api/recipe/delete?recipeId=" + recipeId;
            $.apiHelper.delete(url,{
                success: function(res) {
                    window.location = "/Recipe/MyRecipe";
                }
            });
        }
        else{
            toastr.error("Có lỗi xảy ra");
        }
    })
    $("#unpublish").on("click", function() {
        const recipeId = $("#recipe-container").data("id");
        if(recipeId){
            const url = "/api/recipe/unpublish?recipeId=" + recipeId;
            $.apiHelper.pos(url,null,{
                success: function(res) {
                    setButton(res);
                }
            });
        }
        else{
            toastr.error("Có lỗi xảy ra");
        }
    })
    
    // btn for admin
    $("#btn-approve").on("click", function() {
        const recipeId = $("#recipe-container").data("id");
        if(recipeId){
            const url = "/api/recipe/approve?recipeId=" + recipeId;
            $.apiHelper.post(url,{}, {
                success: function(res) {
                    
                }
            })
        }
    })
    $("btn-unapprove").on("click", function() {
        const recipeId = $("#recipe-container").data("id");
        if(recipeId){
            const url = "/api/recipe/reject?recipeId=" + recipeId;
            $.apiHelper.post(url,{}, {
                success: function(res) {
                    
                }
            })
        }
    })
    setButton();
});