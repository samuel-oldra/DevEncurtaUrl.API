namespace DevEncurtaUrl.API.Entities
{
    public class ShortenedCustomLink
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string DestinationLink { get; set; } = string.Empty;

        public string ShortenedLink { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public string CreatedAt { get; set; } = string.Empty;

        private ShortenedCustomLink() { }

        public ShortenedCustomLink(string title, string destinationLink, string domain)
        {
            var code = title.Split(" ")[0].ToLower();

            Title = title;
            DestinationLink = destinationLink;
            ShortenedLink = $"{domain}/{code}";

            Code = code;
            CreatedAt = DateTime.Now.ToShortDateString();
        }

        public void Update(string title, string destinationLink)
        {
            Title = title;
            DestinationLink = destinationLink;
        }
    }
}