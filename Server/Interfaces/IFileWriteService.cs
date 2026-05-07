using Server.DataTransferObject.Request;

namespace Server.Interfaces
{
    public interface IFileWriteService
    {
        DataTransferObject.Response.ProtocolResponse Handle(FileWrite request);
    }
}