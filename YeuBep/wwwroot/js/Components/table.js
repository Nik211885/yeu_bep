$(document).ready(function() {
    let sortColumn = null;
    let sortDirection = 'asc';
    let selectedRows = new Set();
    let filterTimeout = null;

    // Update sort icons
    function updateSortIcons() {
        $('.sort-icon').text('↕');
        if (sortColumn) {
            const $th = $(`th[data-column="${sortColumn}"]`);
            const $icon = $th.find('.sort-icon');
            $icon.text(sortDirection === 'asc' ? '↑' : '↓');
        }
    }

    // Enable/Disable buttons based on selection
    function ableAndDisableButton() {
        const count = selectedRows.size;
        const hasSelection = count > 0;
        const singleSelection = count === 1;

        // Enable/disable buttons
        $('#btnEdit').prop('disabled', !singleSelection);
        $('#btnDelete').prop('disabled', !hasSelection);
        $('#btnView').prop('disabled', !singleSelection);
        $('#btnApprove').prop('disabled', !hasSelection);
        $('#btnReject').prop('disabled', !hasSelection);
        $('#btnLock').prop('disabled', !hasSelection);
        $('#btnUnlock').prop('disabled', !hasSelection);
    }

    // Sort data
    function sortData(column) {
        if (sortColumn === column) {
            sortDirection = sortDirection === 'asc' ? 'desc' : 'asc';
        } else {
            sortColumn = column;
            sortDirection = 'asc';
        }
        updateSortIcons();
        loadTableData();
    }
    //
    
    // Load table data with filters and sorting
    function loadTableData() {
        const filters = {};
        $('.filter-input').each(function() {
            const column = $(this).data('filter');
            const value = $(this).val().trim();
            if (value) {
                filters[column] = value;
            }
        });

        const rowsPerPage = $('#rowsPerPage').val();

        // TODO: Ajax call to server
        // $.ajax({
        //     url: '/YourController/GetData',
        //     method: 'GET',
        //     data: {
        //         sortColumn: sortColumn,
        //         sortDirection: sortDirection,
        //         filters: filters,
        //         pageSize: rowsPerPage,
        //         page: currentPage
        //     },
        //     success: function(data) {
        //         updateTable(data);
        //     }
        // });
    }
    // get url for in table
    function getQueryStringSelector(){
        let queryString = '';
        selectedRows.forEach(index => {
            const $row = $(`#tableBody tr:eq(${index})`);
            const $checkbox = $row.find('.check-item');
            const dataPath = $checkbox.data("path");
            queryString += dataPath;
        })
        return queryString;
    }
    // Handle Enter key on table
    $("#dataTable").on("keydown", function(e) {
        if (e.key === "Enter") {
            e.preventDefault();
            loadTableData();
        }
    });

    // Select all checkbox
    $("#selectAll").on("click", function() {
        const isChecked = $(this).prop("checked");
        selectedRows.clear();

        $(".check-item").each(function(index) {
            $(this).prop("checked", isChecked);
            if (isChecked) {
                selectedRows.add(index);
            }
        });

        ableAndDisableButton();
    });

    // Individual row checkbox
    $(document).on('change', '.check-item', function() {
        const index = $(this).closest('tr').index();

        if ($(this).prop('checked')) {
            selectedRows.add(index);
        } else {
            selectedRows.delete(index);
            $('#selectAll').prop('checked', false);
        }

        // Update select all checkbox
        const totalCheckboxes = $('.check-item').length;
        const checkedCheckboxes = $('.check-item:checked').length;
        $('#selectAll').prop('checked', totalCheckboxes === checkedCheckboxes && totalCheckboxes > 0);

        ableAndDisableButton();
    });

    // Sortable columns
    $('.sortable').on('click', function(e) {
        if (!$(e.target).hasClass('filter-input')) {
            const column = $(this).data('column');
            sortData(column);
        }
    });

    // Filter inputs with debounce
    $('.filter-input').on('input', function() {
        clearTimeout(filterTimeout);
        filterTimeout = setTimeout(function() {
            loadTableData();
        }, 500);
    });

    // Advanced filter toggle
    $('#advancedFilterToggle').on('click', function() {
        $('#advancedFilterPanel').slideToggle(300);
    });

    // Add filter button
    $('#addFilterBtn').on('click', function() {
        const filterHtml = `
            <div class="filter-row">
                <div class="filter-field">
                    <label>Cột</label>
                    <select class="filter-column">
                        <option value="">Chọn cột</option>
                        ${$('.sortable').map(function() {
            return `<option value="${$(this).data('column')}">${$(this).data('column')}</option>`;
        }).get().join('')}
                    </select>
                </div>
                <div class="filter-field">
                    <label>Điều kiện</label>
                    <select class="filter-operator">
                        <option value="contains">Chứa</option>
                        <option value="equals">Bằng</option>
                        <option value="notEquals">Không bằng</option>
                        <option value="startsWith">Bắt đầu với</option>
                        <option value="endsWith">Kết thúc với</option>
                        <option value="greaterThan">Lớn hơn</option>
                        <option value="lessThan">Nhỏ hơn</option>
                    </select>
                </div>
                <div class="filter-field filter-value-field">
                    <label>Giá trị</label>
                    <input type="text" class="filter-value" placeholder="Nhập giá trị...">
                </div>
                <button class="remove-filter-btn" title="Xóa điều kiện">
                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" viewBox="0 0 16 16">
                        <path d="M2.146 2.854a.5.5 0 1 1 .708-.708L8 7.293l5.146-5.147a.5.5 0 0 1 .708.708L8.707 8l5.147 5.146a.5.5 0 0 1-.708.708L8 8.707l-5.146 5.147a.5.5 0 0 1-.708-.708L7.293 8 2.146 2.854Z"/>
                    </svg>
                </button>
            </div>
        `;
        $('#advancedFilters').append(filterHtml);
    });
    
    // Remove filter
    $(document).on('click', '.remove-filter-btn', function() {
        $(this).closest('.filter-row').remove();
    });
    
    // Rows per page change
    $('#rowsPerPage').on('change', function() {
        loadTableData();
    });

    // Button actions
    $('#btnAdd').on('click', function() {
        const url = $(this).data("url");
        if (url) {
            window.location.href = url;
        }
    });

    $('#btnEdit').on('click', function() {
        if (selectedRows.size === 1) {
            const url = $(this).data("url");
            const queryString = getQueryStringSelector();
            if (url) {
                window.location.href = url + "?" +  queryString;
            }
            // TODO: Get ID and redirect to edit page
            // const id = $row.data('id');
            // window.location.href = `/YourController/Edit/${id}`;
        }
    });

    $('#btnDelete').on('click', function() {
        if (selectedRows.size > 0) {
            if (confirm(`Bạn có chắc chắn muốn xóa ${selectedRows.size} mục đã chọn?`)) {
                // TODO: Delete selected items
                const ids = [];
                selectedRows.forEach(index => {
                    const $row = $(`#tableBody tr:eq(${index})`);
                    console.log($row);
                    // ids.push($row.data('id'));
                });
                // Ajax delete request
                const url = $(this).data("url");
            }
        }
    });

    $('#btnView').on('click', function() {
        if (selectedRows.size === 1) {
            const url = $(this).data("url");
            const queryString = getQueryStringSelector();
            if (url) {
                window.location.href = url + "?" +  queryString;
            }
        }
    });

    $('#btnApprove').on('click', function() {
        if (selectedRows.size > 0) {
            if (confirm(`Bạn có chắc chắn muốn duyệt ${selectedRows.size} mục đã chọn?`)) {
                console.log(selectedRows);
                // TODO: Approve selected items
                const url = $(this).data("url");
            }
        }
    });

    $('#btnReject').on('click', function() {
        if (selectedRows.size > 0) {
            if (confirm(`Bạn có chắc chắn muốn từ chối ${selectedRows.size} mục đã chọn?`)) {
                console.log(selectedRows);
                // TODO: Reject selected items
                const url = $(this).data("url");
            }
        }
    });

    $('#btnLock').on('click', function() {
        if (selectedRows.size > 0) {
            if (confirm(`Bạn có chắc chắn muốn khóa ${selectedRows.size} mục đã chọn?`)) {
                console.log(selectedRows);
                // TODO: Lock selected items
                const url = $(this).data("url");
            }
        }
    });

    $('#btnUnlock').on('click', function() {
        if (selectedRows.size > 0) {
            console.log(selectedRows);
            if (confirm(`Bạn có chắc chắn muốn mở khóa ${selectedRows.size} mục đã chọn?`)) {
                // TODO: Unlock selected items
            }
        }
    });

    $('#btnRefresh').on('click', function() {
        // Clear filters
        $('.filter-input').val('');
        $('#advancedFilters').empty();
        selectedRows.clear();
        $('#selectAll').prop('checked', false);
        $('.check-item').prop('checked', false);
        sortColumn = null;
        sortDirection = 'asc';
        updateSortIcons();
        ableAndDisableButton();
        loadTableData();
        window.location.reload();
    });

    // Double click on row to view details
    $(document).on('dblclick', '#tableBody tr', function() {
        const $checkbox = $(this).find('.check-item');
        $checkbox.prop('checked', true).trigger('change');
        $('#btnView').trigger('click');
    });
    $(".filter-input").on("keydown", function (e) {
        if (e.key === "Enter" || e.keyCode === 13) {
            const queryString = $(".filter-input").map(function () {
                const key = $(this).data("path");
                const value = $(this).val();
                return encodeURIComponent(key) + "=" + encodeURIComponent(value);
            }).get().join("&");
            console.log(queryString);
            window.location.href = `${window.location.pathname}?${queryString}`;
        }
    });
    
    ableAndDisableButton();
});