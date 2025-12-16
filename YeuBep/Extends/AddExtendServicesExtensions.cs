using CloudinaryDotNet;
using DeepSeek.ApiClient.Extensions;
using Milvus.Client;
using YeuBep.Extends.DataModel;

namespace YeuBep.Extends;

public static class AddExtendServicesExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddExtendServicesDefault(IConfiguration configuration)
        {
            services.AddConfigurationForExtendServicesDefault(configuration);
            services.AddTransient<EmailSenderServices>();
            services.AddScoped<CloudinaryServices>();
            return services;
        }

        private IServiceCollection AddConfigurationForExtendServicesDefault(IConfiguration configuration)
        {
            var cloudinaryConfig = configuration.GetSection("Cloudinary").Get<CloudinaryDataModels>()
                ?? throw new ArgumentNullException(nameof(CloudinaryDataModels));
            var mailSettings = configuration.GetSection("MailSettings").Get<MailSettings>()
                ?? throw new ArgumentNullException(nameof(MailSettings));
            var deepSeekApiKey = configuration.GetValue<string>("DeepSeekApiKey")
                ?? throw new ArgumentNullException(nameof(configuration));
            var milvusConfig = configuration.GetSection("Milvus").Get<MilvusDataModel>()
                ?? throw new ArgumentNullException(nameof(configuration));
            var account = new Account()
            {
                Cloud = cloudinaryConfig.CloudName,
                ApiKey = cloudinaryConfig.ApiKey,
                ApiSecret = cloudinaryConfig.ApiSecret,
            };
            var cloudinary = new Cloudinary(account);
            var milvusClient = new MilvusClient(
                host:milvusConfig.Endpoint,
                username:milvusConfig.Username,
                password:milvusConfig.Password,
                port:milvusConfig.Port,
                ssl:milvusConfig.EnableSSL,
                database:milvusConfig.Database);
            services.AddSingleton(cloudinary);
            services.AddDeepSeekClient(deepSeekApiKey);
            services.AddSingleton(mailSettings);
            services.AddSingleton(cloudinaryConfig);
            services.AddSingleton(milvusClient);
            return services;
        }
    }
}