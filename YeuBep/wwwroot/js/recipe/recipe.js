$(document).ready(function() {
    let isFavorited = false;
    const btnRating = $("#btn-rating")
    const recipeId = $('#container').data('id');
    let countCountRating = btnRating.data("count-total-rating");
    let countTotalRatingPoint =btnRating.data("total-rating-point");
    let selectedRating = 0;
    let myRatingPoint = 0;
    $.apiHelper.get('/api/favorites/my-favorite?recipeId='+ recipeId,{
        success: function(data){
            isFavorited = true;
            console.log(isFavorited);
            handleColorBtnFavorite();
        }
    })
    $.apiHelper.get('/api/rating/my-rating?recipeId='+ recipeId,{
        success: function(data){
            myRatingPoint = data.ratingPoint;
            selectedRating = data.ratingPoint;
            highlightStars(selectedRating);
        }
    })
    function handleColorBtnFavorite() {
        const btnFavorite = $("#btnFavorite");
        const favoriteText = $("#favoriteText");
        if (isFavorited) {
            btnFavorite.removeClass('btn-outline-danger').addClass('btn-danger');
            btnFavorite.find('i').removeClass('heart-white').addClass('heart-red');
            favoriteText.text('Đã yêu thích');
        }
        else {
            btnFavorite.removeClass('btn-danger').addClass('btn-outline-danger');
            btnFavorite.find('i').removeClass('heart-red').addClass('heart-white');
            favoriteText.text('Yêu thích');
        }
    }
    // Xử lý nút yêu thích
    $('#btnFavorite').click(function() {
        isFavorited = !isFavorited;
        const url = '/api/favorites/toggle?recipeId='+ recipeId;
        $.apiHelper.post(url, null, {
            success: function(response) {
                if (isFavorited) {
                    handleColorBtnFavorite();
                    let currentCount = parseInt($('#favoriteCount').text());
                    $('#favoriteCount').text(currentCount + 1);

                } else {
                    handleColorBtnFavorite();
                    let currentCount = parseInt($('#favoriteCount').text());
                    $('#favoriteCount').text(currentCount - 1);
                }
            }
        })
    });

    // Xử lý hover và click sao
    $('.star').hover(
        function() {
            const rating = $(this).data('rating');
            highlightStars(rating);
        },
        function() {
            highlightStars(selectedRating);
        }
    );

    $('.star').click(function() {
        selectedRating = $(this).data('rating');
        $('#selectedRating').text(selectedRating);
        console.log('Đã chọn ' + selectedRating + ' sao');
    });

    function highlightStars(rating) {
        $('.star').each(function() {
            if ($(this).data('rating') <= rating) {
                $(this).addClass('active');
            } else {
                $(this).removeClass('active');
            }
        });
    }

    // Xử lý gửi đánh giá
    $('#btnSubmitRating').click(function() {
        if (selectedRating === 0) {
            alert('Vui lòng chọn số sao để đánh giá!');
            return;
        }
        const url = '/api/rating/create';
        const data = {
            recipeId: recipeId,
            ratingPoint: selectedRating,
        }
        $.apiHelper.post(url, data, {
            success: function(response) {
                // Cập nhật số lượng đánh giá
                if(myRatingPoint === 0) {
                    countCountRating+=1;
                }
                countTotalRatingPoint -= myRatingPoint;
                countTotalRatingPoint += selectedRating;
                const rating =  (countTotalRatingPoint / countCountRating).toFixed(1);
                myRatingPoint = selectedRating;
                $('#ratingCount').text(rating);
                $('#ratingModal').modal('hide');
            }
        })
    });

    // Xử lý thêm bình luận
    $('#btnAddComment').click(function() {
        const commentText = $('#newComment').val().trim();

        if (commentText === '') {
            alert('Vui lòng nhập nội dung bình luận!');
            return;
        }
        const url = '/api/comment/create';
        const data = {
            recipeId: recipeId,
            commentText: commentText,
        }
        $.apiHelper.post(url, data, {
            success: function(response) {
                const userName = $('#user-name').val().trim();
                const avatar = $('#avatar-main').attr('src');
                // Tạo HTML cho bình luận mới
                const newComment = `
                    <div class="card border-0 shadow-sm mb-3 comment-card">
                        <div class="card-body p-3 p-md-4">
                            <div class="d-flex gap-3">
                                <!-- Avatar -->
                                <div class="flex-shrink-0">
                                    <img src="${avatar}" 
                                         class="rounded-circle user-avatar" 
                                         alt="${{userName}}"
                                         onerror="this.onerror=null; this.src='https://ui-avatars.com/api/?name=${userName})&background=667eea&color=fff&size=48';">
                                </div>
                                <div class="flex-grow-1">
                                    <!-- Header -->
                                    <div class="d-flex align-items-center gap-2 mb-1">
                                        <h6 class="mb-0 fw-semibold text-dark">${userName}</h6>
                                    </div>
                                    <div class="d-flex align-items-center gap-1 text-muted mb-2" style="font-size: 0.813rem;">
                                        <svg xmlns="http://www.w3.org/2000/svg" width="13" height="13" fill="currentColor" viewBox="0 0 16 16">
                                            <path d="M8 3.5a.5.5 0 0 0-1 0V9a.5.5 0 0 0 .252.434l3.5 2a.5.5 0 0 0 .496-.868L8 8.71z"/>
                                            <path d="M8 16A8 8 0 1 0 8 0a8 8 0 0 0 0 16m7-8A7 7 0 1 1 1 8a7 7 0 0 1 14 0"/>
                                        </svg>
                                        <span>Now</span>
                                    </div>
                                    <p class="mb-0 text-secondary" style="line-height: 1.6; font-size: 0.938rem;">
                                        ${commentText}
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>
                `;
                // Thêm vào đầu danh sách
                $('#commentList').prepend(newComment);

                // Xóa nội dung textarea
                $('#newComment').val('');
                let countComment = $("#count-comment");
                countComment.text(parseInt(countComment.text()) + 1);
            }
        })
    });
});