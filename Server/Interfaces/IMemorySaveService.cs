using Server.DataTransferObject.Request;

namespace Server.Interfaces
{
    public interface IMemorySaveService
    {
        DataTransferObject.Response.ProtocolResponse Handle(MemorySave request);
    }
}
