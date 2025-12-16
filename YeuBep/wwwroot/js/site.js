// Cấu hình chung cho toastr
const notifications =[];
$.apiHelper.get("/api/notification", {
    success: function(response) {
        response.items.forEach((element) => {
            notifications.push({
                title: element.title,
                text: element.text,
                message: element.body,
                type:'Kiểm duyệt công thức',
                time:  element.time,
                link: element.link,
            });
        })
        renderNotifications();
    }
})
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
document.addEventListener('click', function(event) {
    const dropdown = document.getElementById('notificationDropdown');
    const headerIcon = document.querySelector('.header-icon');

    if (!dropdown.contains(event.target) && !headerIcon.contains(event.target)) {
        dropdown.classList.remove('active');
    }
});

function toggleNotifications() {
    const dropdown = document.getElementById('notificationDropdown');
    dropdown.classList.toggle('active');
}

function renderNotifications() {
    updateNotificationCount();
    const notificationList = document.getElementById('notificationList');

    if (notifications.length === 0) {
        notificationList.innerHTML = `
                    <div class="empty-notifications">
                        <i class="fas fa-bell-slash"></i>
                        <p>Không có thông báo nào</p>
                    </div>
                `;
        return;
    }

    notificationList.innerHTML = notifications.map(notif => `
                <a href="/${notif.link}" style="text-decoration: beige !important;">
                    <div class="notification-item">
                        <div class="notification-icon ${notif.type}">
                            <i class="fas ${notif.icon}"></i>
                        </div>
                        <div class="notification-content">
                            <div class="notification-title">${notif.title}</div>
                            <div class="notification-message">${notif.message}</div>
                            <div class="notification-time">${notif.time}</div>
                        </div>
                    </div>
                </a>
            `).join('');
    
}

function updateNotificationCount() {
    const unreadCount = notifications.length;
    const badge = document.getElementById('notificationCount');
    if (unreadCount > 0) {
        badge.textContent = unreadCount > 9 ? '9+' : unreadCount;
        badge.style.display = 'flex';
    } else {
        badge.style.display = 'none';
    }
}

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notification")
    .build();

connection.on("ReceiveMessage", function (message) {
    const element = JSON.parse(message);
    console.log(element);
    notifications.unshift({
        title: element.Title,
        text: element.Text,
        message: element.Body,
        type:'Kiểm duyệt công thức',    
        time:  element.Time,
        link: element.Link
    });
    renderNotifications();
});

connection.start()
    .then(() => {
        console.log("Kết nối SignalR đến Server thành công!");
        $("#sendButton").prop("disabled", false);
    })
    .catch(err => {
        console.error("Lỗi kết nối SignalR: " + err.toString());
    });


const chatButton = $('#chatButton');
const chatWindow = $('#chatWindow');
const closeChat = $('#closeChat');
const messageInput = $('#messageInput');
const sendButton = $('#sendButton');
const chatBody = $('#chatBody');
const typingIndicator = $('#typingIndicator');

chatButton.on('click', function() {
    chatWindow.css('display', 'flex');
    messageInput.focus();
});

closeChat.on('click', function() {
    chatWindow.hide();
});

function sendMessage() {
    const message = messageInput.val().trim();

    if (message === '') return;
    const userMessage = `
                    <div class="message user">
                        <div class="message-content">${escapeHtml(message)}</div>
                    </div>
                `;
    typingIndicator.before(userMessage);
    messageInput.val('');
    scrollToBottom();
    messageInput.prop('disabled', true);
    sendButton.prop('disabled', true);
    typingIndicator.show();
    scrollToBottom();
    simulateAIResponse(message);
}

function simulateAIResponse(userMessage) {
    $.ajax({
        url: "/api/recipe/suggestion/recipe",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(userMessage),
        success: function(response) {
            typingIndicator.hide();
            let botReply = '';

            if (response.userAnalysis) {
                botReply += 'Phân tích: ' + JSON.stringify(response.userAnalysis) + '<br>';
            }

            if (response.suggestedRecipes && response.suggestedRecipes.length > 0) {
                botReply += '<div style="margin-top: 10px;">';
                response.suggestedRecipes.forEach(function(recipe) {
                    const recipeName = recipe.name || recipe.title || 'Công thức';
                    const recipeSlug = recipe.slug || recipe.id || '';
                    const recipeUrl = '/Recipe/Slug?slug=' + encodeURIComponent(recipeSlug);
                    const recipeImage = recipe.avatar || recipe.image || 'https://via.placeholder.com/80x80?text=Recipe';
                    const recipeTime = recipe.timeToCook || recipe.avatar || '';
                    const recipePortion = recipe.portionCount || recipe.servings || '';

                    botReply += `
                                    <a href="${recipeUrl}" target="_blank" style="text-decoration: none; color: inherit; display: block; margin-bottom: 12px;">
                                        <div style="display: flex; align-items: center; padding: 12px; background: #f8f9fa; border-radius: 12px; transition: all 0.3s; border: 1px solid #e9ecef;">
                                            <img src="${recipeImage}" 
                                                 onerror="this.src='https://via.placeholder.com/80x80?text=Recipe'"
                                                 style="width: 70px; height: 70px; border-radius: 10px; object-fit: cover; margin-right: 12px; box-shadow: 0 2px 8px rgba(0,0,0,0.1);" alt="">
                                            <div style="flex: 1; min-width: 0;">
                                                <div style="font-weight: 600; color: #333; margin-bottom: 4px; font-size: 14px; overflow: hidden; text-overflow: ellipsis; white-space: nowrap;">
                                                    ${recipeName}
                                                </div>
                                                <div style="font-size: 12px; color: #666; display: flex; align-items: center; gap: 12px;">
                                                    ${recipeTime ? `<span><i class="fas fa-clock" style="color: #667eea; margin-right: 4px;"></i>${recipeTime}</span>` : ''}
                                                    ${recipePortion ? `<span><i class="fas fa-users" style="color: #667eea; margin-right: 4px;"></i>${recipePortion}</span>` : ''}
                                                </div>
                                            </div>
                                            <i class="fas fa-chevron-right" style="color: #667eea; font-size: 14px; margin-left: 8px;"></i>
                                        </div>
                                    </a>
                                `;
                });
                botReply += '</div>';
            }

            if (!botReply) {
                botReply = response.message || 'Tôi đã nhận được yêu cầu của bạn!';
            }

            const botMessage = `
                            <div class="message bot">
                                <div class="message-content">${botReply}</div>
                            </div>
                        `;

            typingIndicator.before(botMessage);
            scrollToBottom();
            messageInput.prop('disabled', false);
            sendButton.prop('disabled', false);
            messageInput.focus();
        },
        error: function(xhr, status, error) {
            typingIndicator.hide();

            const errorMessage = `
                            <div class="message bot">
                                <div class="message-content">Xin lỗi, đã có lỗi xảy ra. Vui lòng thử lại! 😔</div>
                            </div>
                        `;

            typingIndicator.before(errorMessage);
            scrollToBottom();

            messageInput.prop('disabled', false);
            sendButton.prop('disabled', false);
            messageInput.focus();
        }
    });
}

sendButton.on('click', sendMessage);

messageInput.on('keypress', function(e) {
    if (e.which === 13) { // Enter key
        sendMessage();
    }
});

function scrollToBottom() {
    chatBody.scrollTop(chatBody[0].scrollHeight);
}

function escapeHtml(text) {
    const map = {
        '&': '&amp;',
        '<': '&lt;',
        '>': '&gt;',
        '"': '&quot;',
        "'": '&#039;'
    };
    return text.replace(/[&<>"']/g, m => map[m]);
}