using Server.DataTransferObject.Request;

namespace Server.Interfaces
{
    public interface IGitPullService
    {
        DataTransferObject.Response.ProtocolResponse Handle(GitPull request);
    }
}