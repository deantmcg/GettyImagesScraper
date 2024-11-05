namespace GettyImagesScraper
{
    public class Image
    {
        public Image(int id, string url, string category)
        {
            Id = id;
            Url = url;
            Category = category;
        }

        public int Id { get; init; }
        public string Url { get; init; }
        public string Category { get; init; }

        public override string ToString()
        {
            return $"{Category} | {Url}";
        }
    }
}