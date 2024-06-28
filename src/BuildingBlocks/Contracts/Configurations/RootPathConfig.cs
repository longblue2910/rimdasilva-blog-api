namespace Contracts.Configurations;

public class RootPathConfig
{
    private static readonly string Dirpath = Directory.GetCurrentDirectory();

    public class UploadPath
    {
        public static readonly string PathImagePost = Dirpath + @"/Uploads/Image/Post";
    }
}
