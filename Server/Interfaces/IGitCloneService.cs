using Server.DataTransferObject.Request;

namespace Server.Interfaces
{
    public interface IGitCloneService
    {
        DataTransferObject.Response.ProtocolResponse Handle(GitClone request);
    }
}