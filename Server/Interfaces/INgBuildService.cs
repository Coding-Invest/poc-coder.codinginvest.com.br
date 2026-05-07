namespace Server.Interfaces
{
    public interface INgBuildService
    {
        DataTransferObject.Response.ProtocolResponse Handle(DataTransferObject.Request.NgBuild request);
    }
}