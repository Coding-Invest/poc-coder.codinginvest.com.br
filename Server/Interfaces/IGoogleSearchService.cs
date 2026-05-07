using Server.DataTransferObject.Request;

namespace Server.Interfaces
{
    public interface IGoogleSearchService
    {
        DataTransferObject.Response.ProtocolResponse Handle(GoogleSearch request);
    }
}