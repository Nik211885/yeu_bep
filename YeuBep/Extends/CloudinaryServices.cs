using CloudinaryDotNet;
using YeuBep.Extends.DataModel;

namespace YeuBep.Extends;

public class CloudinaryServices
{
    private readonly CloudinaryDataModels _cloudinaryConfig;
    private readonly Cloudinary _cloudinary;

    public CloudinaryServices(CloudinaryDataModels cloudinaryConfig, Cloudinary cloudinary)
    {
        _cloudinaryConfig = cloudinaryConfig;
        _cloudinary = cloudinary;
    }
    public string GetUrlUploadFileBySignature()
    {
        var timeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        var parameters = new SortedDictionary<string, object>
        {
            {"folder", _cloudinaryConfig.UploadFolder },
            {"timestamp", timeStamp},
        };
        var stringSign = string.Join("&", parameters.Select(kvp => $"{kvp.Key}={kvp.Value}"));
        var signature = _cloudinary.Api.SignParameters(parameters);
        var urlWithSignature =
            string.Concat(_cloudinaryConfig.UrlUpload, 
                string.Format("/{0}/image/upload?api_key={1}&{2}&signature={3}",
                    _cloudinaryConfig.CloudName, 
                    _cloudinaryConfig.ApiKey, 
                    stringSign, 
                    signature));
        return urlWithSignature;
    }
}