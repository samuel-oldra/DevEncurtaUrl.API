namespace DevEncurtaUrl.API.Models
{
    public class AddOrUpdateShortenedLinkModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string DestinationLink { get; set; }
    }
}