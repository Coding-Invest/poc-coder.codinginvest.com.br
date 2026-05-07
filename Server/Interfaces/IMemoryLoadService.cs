using Server.DataTransferObject.Request;

namespace Server.Interfaces
{
    public interface IMemoryLoadService
    {
        DataTransferObject.Response.ProtocolResponse Handle(MemoryLoad request);
    }
}
