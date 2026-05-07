using Server.DataTransferObject.Request;

namespace Server.Interfaces
{
    public interface IGitPullRequestService
    {
        DataTransferObject.Response.ProtocolResponse Handle(GitPullRequest request);
    }
}