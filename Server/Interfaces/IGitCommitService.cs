using Server.DataTransferObject.Request;

namespace Server.Interfaces
{
    public interface IGitCommitService
    {
        DataTransferObject.Response.ProtocolResponse Handle(GitCommit request);
    }
}