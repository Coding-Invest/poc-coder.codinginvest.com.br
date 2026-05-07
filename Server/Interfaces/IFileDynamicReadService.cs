using Server.DataTransferObject.Request;
using Server.DataTransferObject.Response;

namespace Server.Interfaces
{
    public interface IFileDynamicReadService
    {
        ProtocolResponse Handle(FileDynamicRead request);
    }
}