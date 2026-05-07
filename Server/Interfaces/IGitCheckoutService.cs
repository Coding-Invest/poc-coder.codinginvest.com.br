using Server.DataTransferObject.Request;

namespace Server.Interfaces
{
    public interface IGitCheckoutService
    {
        DataTransferObject.Response.ProtocolResponse Handle(GitCheckout request);
    }
}