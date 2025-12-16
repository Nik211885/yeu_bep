namespace YeuBep.Extends.DataModel;

public class MilvusDataModel
{
    public string Endpoint { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Database { get; set; }
    public bool EnableSSL { get; set; }
}