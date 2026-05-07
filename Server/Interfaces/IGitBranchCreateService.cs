using Server.DataTransferObject.Request;

namespace Server.Interfaces
{
    public interface IGitBranchCreateService
    {
        DataTransferObject.Response.ProtocolResponse Handle(GitBranchCreate request);
    }
}
