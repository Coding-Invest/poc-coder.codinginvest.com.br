using Server.DataTransferObject.Request;

namespace Server.Interfaces
{
    public interface IFetchUrlService
    {
        Task<DataTransferObject.Response.ProtocolResponse> Handle(FetchUrl request);
    }
}