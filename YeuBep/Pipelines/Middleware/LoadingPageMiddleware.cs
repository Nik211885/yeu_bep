using System.Text;

namespace YeuBep.Pipelines.Middleware;

public class LoadingPageMiddleware
{
    private readonly RequestDelegate _next;

    public LoadingPageMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;

        using var memoryStream = new MemoryStream();
        context.Response.Body = memoryStream;

        await _next(context);
        
        if (context.Response.StatusCode == 200 && 
            context.Response.ContentType?.Contains("text/html") == true)
        {
            memoryStream.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();
            
            var loadingHtml = GetLoadingHtml();
            var modifiedBody = responseBody.Replace("</body>", $"{loadingHtml}</body>");
            
            var bytes = Encoding.UTF8.GetBytes(modifiedBody);
            context.Response.ContentLength = bytes.Length;
            context.Response.Body = originalBodyStream;
            await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
        }
        else
        {
            // Copy response as-is
            memoryStream.Seek(0, SeekOrigin.Begin);
            context.Response.Body = originalBodyStream;
            await memoryStream.CopyToAsync(originalBodyStream);
        }
    }
    private string GetLoadingHtml()
    {
        return @"
            <div id='page-loader' class='page-loader'>
                <div class='loader-content'>
                    <div class='spinner'></div>
                    <p class='loader-text'>Đang tải...</p>
                </div>
            </div>
            <style>
                .page-loader {
                    position: fixed;
                    top: 0;
                    left: 0;
                    width: 100%;
                    height: 100%;
                    background: rgba(255, 255, 255, 0.95);
                    display: none;
                    justify-content: center;
                    align-items: center;
                    z-index: 9999999999999999999;
                    backdrop-filter: blur(5px);
                }
                .page-loader.show {
                    display: flex;
                }
                .loader-content {
                    text-align: center;
                }
                .spinner {
                    width: 50px;
                    height: 50px;
                    border: 4px solid #2b9fc6;
                    border-top: 4px solid #dc3545;
                    border-radius: 50%;
                    animation: spin 1s linear infinite;
                    margin: 0 auto 20px;
                }
                @keyframes spin {
                    0% { transform: rotate(0deg); }
                    100% { transform: rotate(360deg); }
                }
                .loader-text {
                    color: #333;
                    font-size: 16px;
                    font-weight: 500;
                    margin: 0;
                }
            </style>
            <script>
                (function() {
                    const loader = document.getElementById('page-loader');
                    
                    document.addEventListener('click', function(e) {
                        const target = e.target.closest('a');
                        if (target && target.href && !target.hasAttribute('data-no-loader')) {
                            const url = new URL(target.href);
                            // Chỉ show loader cho internal links
                            if (url.origin === window.location.origin && 
                                !target.hasAttribute('download') &&
                                target.target !== '_blank') {
                                loader.classList.add('show');
                            }
                        }
                    });

                    document.addEventListener('submit', function(e) {
                        const form = e.target;
                        if (!form.hasAttribute('data-no-loader')) {
                            loader.classList.add('show');
                        }
                    });

                    window.addEventListener('load', function() {
                        loader.classList.remove('show');
                    });

                    window.addEventListener('pageshow', function(event) {
                        if (event.persisted) {
                            loader.classList.remove('show');
                        }
                    });
                })();
            </script>
        ";
    }
}

public static class LoadingMiddlewareExtensions
{
    extension(IApplicationBuilder builder)
    {
        public IApplicationBuilder UsePageLoading()
        {
            return builder.UseMiddleware<LoadingPageMiddleware>();
        }
    }
}