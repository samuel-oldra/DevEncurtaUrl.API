namespace DevEncurtaUrl.API.Models
{
    public class AddOrUpdateShortenedLinkModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string DestinationLink { get; set; } = string.Empty;
    }
}