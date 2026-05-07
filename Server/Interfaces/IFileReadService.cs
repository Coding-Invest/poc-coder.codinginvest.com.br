using Server.DataTransferObject.Request;

namespace Server.Interfaces
{
    public interface IFileReadService
    {
        DataTransferObject.Response.ProtocolResponse Handle(FileRead request);
    }
}