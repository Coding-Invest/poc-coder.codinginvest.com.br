using Server.DataTransferObject.Request;

namespace Server.Interfaces
{
    public interface IFileDeleteService
    {
        DataTransferObject.Response.ProtocolResponse Handle(FileDelete request);
    }
}