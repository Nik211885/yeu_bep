$(document).ready(function(){
    let debounceTimer;
    const $searchInput = $("#search");
    const $dropdown = $("#autocompleteDropdown");

    $searchInput.on("input", function(){
        const keyword = $(this).val().trim();

        clearTimeout(debounceTimer);

        if(keyword.length < 2) {
            $dropdown.removeClass("show").html("");
            return;
        }

        debounceTimer = setTimeout(() => {
            const url = "/api/recipe/auto-complete-search?keyword=" + encodeURIComponent(keyword);

            $.ajax({
                url: url,
                method: "GET",
                success: function(data){
                    displayAutocomplete(data, keyword);
                },
                error: function(err){
                    console.error("Error fetching autocomplete:", err);
                    $dropdown.removeClass("show").html("");
                }
            });
        }, 300);
    });

    function displayAutocomplete(items, keyword) {
        if(!items || items.length === 0) {
            $dropdown.html('<div class="autocomplete-empty">Không tìm thấy kết quả</div>').addClass("show");
            return;
        }

        let html = "";
        items.forEach(item => {
            const highlightedText = highlightKeyword(item.name || item, keyword);
            html += `
                        <div class="autocomplete-item" data-value="${item.name || item}">
                            <i class="bi bi-clock-history"></i>
                            <span class="text">${highlightedText}</span>
                        </div>
                    `;
        });

        $dropdown.html(html).addClass("show");
    }

    function highlightKeyword(text, keyword) {
        const regex = new RegExp(`(${keyword})`, 'gi');
        return text.replace(regex, '<span class="highlight">$1</span>');
    }
    
    $(document).on("click", ".autocomplete-item", function(){
        const value = $(this).data("value");
        $searchInput.val(value);
        $dropdown.removeClass("show").html("");
        $searchInput.closest("form").submit();
    });
    $(document).on("click", function(e){
        if(!$(e.target).closest(".search-form").length) {
            $dropdown.removeClass("show").html("");
        }
    });
    
    $searchInput.on("keydown", function(e){
        if(e.key === "Enter" && $dropdown.hasClass("show")) {
            e.preventDefault();
            $(".autocomplete-item:first").click();
        }
    });
});