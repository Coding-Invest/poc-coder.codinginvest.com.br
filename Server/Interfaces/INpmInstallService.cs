namespace Server.Interfaces
{
    public interface INpmInstallService
    {
        DataTransferObject.Response.ProtocolResponse Handle(DataTransferObject.Request.NpmInstall request);
    }
}