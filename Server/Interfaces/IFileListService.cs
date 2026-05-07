using Server.DataTransferObject.Request;

namespace Server.Interfaces
{
    public interface IFileListService
    {
        DataTransferObject.Response.ProtocolResponse Handle(FileList request);
    }
}