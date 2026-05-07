using Server.DataTransferObject.Request;
using Server.DataTransferObject.Response;

namespace Server.Interfaces
{
    public interface IMemorySummarizeService
    {
        Task<ProtocolResponse> Handle(MemorySummarize request);
    }
}