using Server.DataTransferObject.Request;
using Server.DataTransferObject.Response;

namespace Server.Interfaces
{
    public interface IDotnetBuildService
    {
        ProtocolResponse Handle(DotnetBuild request);
    }
}