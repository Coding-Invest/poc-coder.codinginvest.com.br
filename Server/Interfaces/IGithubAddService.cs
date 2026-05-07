using Server.DataTransferObject.Request;

namespace Server.Interfaces
{
    public interface IGithubAddService
    {
        DataTransferObject.Response.ProtocolResponse Handle(GitAdd request);
    }
}