namespace YeuBep.Const;

public static class Template
{
    public const string RegisterSuccessNotification =
        """
          <p>🎉 Chúc mừng bạn đã đăng ký tài khoản thành công! Chỉ còn một bước nữa để hoàn tất quá trình.</p>
          <p>📧 Chúng tôi đã gửi cho bạn một email để xác nhận tài khoản của bạn.</p>
          <p>🔑 Hãy nhấn vào link trong email trước khi đăng nhập vào ứng dụng nhé. Chúc bạn có những trải nghiệm nấu ăn thật vui cùng <strong>Yeu Bep</strong>! 🍰💖</p>
          """;
    public const string ConfirmEmailSuccess = 
        """
          <p>🎉 Hurray! Bạn đã xác nhận email thành công!</p>
          <p>Bây giờ bạn có thể đăng nhập vào <strong>Yeu Bep</strong> để cùng nhau chia sẻ những công thức nấu ăn siêu ngon 🍰🍜🥗 với mọi người.</p>
          <p>Chúc bạn một ngày nấu ăn thật vui và ngọt ngào nhé 💖</p>
          """;
    public const string SendEmailForgetPasswordSuccess =
        """
         <p>🎉 Yay! Email đặt lại mật khẩu đã được gửi thành công!</p>
         <p>Vui lòng kiểm tra hộp thư của bạn và nhấn vào link để tạo mật khẩu mới cho tài khoản <strong>Yeu Bep</strong> 🍰💖</p>
         <p>Chúc bạn có những trải nghiệm nấu ăn thật vui vẻ và ngọt ngào nhé! 🥗🍜💝</p>
         """;

    public const string RegisterSuccessEmailSenderSubject = "Xác nhận email và bắt đầu phiêu lưu ẩm thực cùng Yeu Bep!";
    
    public static string RegisterSuccessEmailSenderBody(string fullName, string? linkConfirmEmail) => $"""
         <!DOCTYPE html>
         <html lang="vi">
         <head>
             <meta charset="UTF-8">
             <title>Xác nhận tài khoản Yeu Bep</title>
         </head>
         <body style="font-family: 'Comic Sans MS', cursive, sans-serif; background-color: #fff8f0; color: #333;">
             <div style="max-width: 600px; margin: auto; padding: 20px; border: 2px dashed #ff7f50; border-radius: 15px; text-align: center;">
                 <!-- Logo -->
                 <img src="https://static.thenounproject.com/png/1092638-200.png" alt="Yeu Bep Logo" style="width: 120px; margin-bottom: 20px;"/>

                 <!-- Greeting -->
                 <h2 style="color: #ff6347;">Chào {fullName}!</h2>
                 <p style="font-size: 16px;">Cảm ơn bạn đã đăng ký tài khoản <strong>Yeu Bep</strong> 🍰💖</p>

                 <!-- Confirmation link -->
                 <p style="font-size: 16px;">Để hoàn tất đăng ký, vui lòng nhấn nút bên dưới để xác nhận email của bạn:</p>
                 <a href="{linkConfirmEmail}" 
                    style="display: inline-block; padding: 12px 25px; background-color: #ff7f50; color: white; text-decoration: none; border-radius: 10px; font-weight: bold; margin-top: 10px;">
                    Xác nhận email
                 </a>

                 <p style="margin-top: 20px; font-size: 14px; color: #888;">
                     Nếu bạn không đăng ký tài khoản này, vui lòng bỏ qua email này.  
                     Chúng mình hứa sẽ không spam đâu nha 😘
                 </p>

                 <hr style="margin-top: 30px; border: none; border-top: 1px dashed #ff7f50;" />
                 <p style="font-size: 12px; color: #aaa;">© 2025 Yeu Bep. All rights reserved.</p>
             </div>
         </body>
         </html>
         """;

    public const string ForgotPasswordEmailSenderSubject = "Đặt lại mật khẩu Yeu Bep 🍰💖";
    public static string ForgotPasswordEmailSenderBody(string fullName, string? linkForgotPassword) => $"""
         <!DOCTYPE html>
         <html lang="vi">
         <head>
             <meta charset="UTF-8">
             <title>Đặt lại mật khẩu Yeu Bep</title>
         </head>
         <body style="font-family: 'Comic Sans MS', cursive, sans-serif; background-color: #fff8f0; color: #333;">
             <div style="max-width: 600px; margin: auto; padding: 20px; border: 2px dashed #ff7f50; border-radius: 15px; text-align: center;">
                 <!-- Logo -->
                 <img src="https://static.thenounproject.com/png/1092638-200.png" alt="Yeu Bep Logo" style="width: 120px; margin-bottom: 20px;"/>

                 <!-- Greeting -->
                 <h2 style="color: #ff6347;">Chào {fullName}!</h2>
                 <p style="font-size: 16px;">Chúng mình nhận được yêu cầu đặt lại mật khẩu cho tài khoản <strong>Yeu Bep</strong> của bạn 🍰💖</p>

                 <!-- Reset password link -->
                 <p style="font-size: 16px;">Để tiếp tục, vui lòng nhấn nút bên dưới để tạo mật khẩu mới:</p>
                 <a href="{linkForgotPassword}" 
                    style="display: inline-block; padding: 12px 25px; background-color: #ff7f50; color: white; text-decoration: none; border-radius: 10px; font-weight: bold; margin-top: 10px;">
                    Đặt lại mật khẩu
                 </a>

                 <p style="margin-top: 20px; font-size: 14px; color: #888;">
                     Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng bỏ qua email này.  
                     Chúng mình hứa sẽ không spam đâu nha 😘
                 </p>

                 <hr style="margin-top: 30px; border: none; border-top: 1px dashed #ff7f50;" />
                 <p style="font-size: 12px; color: #aaa;">© 2025 Yeu Bep. All rights reserved.</p>
             </div>
         </body>
         </html>
         """;
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    // Notification template
    public static string SendApproveRecipeTemplate(string recipeTitle)
    {
        return $"Yêu cầu kiểm duyệt công thức: {recipeTitle}. Vui lòng xem xét.";
    }

    public static string SendApproveRecipeLinkOpenDetail(string recipeId) => "RecipeManager/Recipe?RecipeId=" + recipeId;

}