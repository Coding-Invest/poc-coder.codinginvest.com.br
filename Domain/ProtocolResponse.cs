namespace Server.DataTransferObject.Response
{
    public class ProtocolResponse
    {
        public string Jsonrpc { get; set; } = "2.0";
        public int Id { get; set; }
        public string? Result { get; set; }
    }
}
