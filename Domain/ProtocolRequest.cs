namespace Domain
{
    public class ProtocolRequest
    {
        public required string Jsonrpc { get; set; }
        public required string Method { get; set; }
        public object[]? @Params { get; set; }
        public int Id { get; set; }
    }
}
