namespace NexsolCrmBackendVersion2.Dtos.Clients
{
    public class ClientDto
    {
        public string Name { get; set; }
        public string WorkStatus { get; set; }
        public string PhisicalStatus { get; set; }
        public string Comment { get; set; }
        public List<Dictionary<string, string>> AdditionalData { get; set; }
    }
}
