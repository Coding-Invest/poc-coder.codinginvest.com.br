namespace Domain
{
    public class ClientToLLM
    {
        public static IList<Message> Memory = new List<Message> { new Message { Role="system", Content= Tools.PRIMARY_INSTRUCTION} };
    }
}
