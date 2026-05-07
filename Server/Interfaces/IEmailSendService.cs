using Server.DataTransferObject.Request;

namespace Server.Interfaces
{
    public interface IEmailSendService
    {
        DataTransferObject.Response.ProtocolResponse Handle(EmailSend request);
    }
}