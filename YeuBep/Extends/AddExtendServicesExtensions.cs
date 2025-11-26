using CloudinaryDotNet;
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
            var account = new Account()
            {
                Cloud = cloudinaryConfig.CloudName,
                ApiKey = cloudinaryConfig.ApiKey,
                ApiSecret = cloudinaryConfig.ApiSecret,
            };
            var cloudinary = new Cloudinary(account);
            services.AddSingleton(cloudinary);
            services.AddSingleton(mailSettings);
            services.AddSingleton(cloudinaryConfig);
            return services;
        }
    }
}