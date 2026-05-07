using Server.DataTransferObject.Request;

namespace Server.Interfaces
{
    public interface IGitPushService
    {
        DataTransferObject.Response.ProtocolResponse Handle(GitPush request);
    }
}