using HtmlAgilityPack;

namespace GettyImagesScraper
{
    public class Scraper(int totalPages, int workers, string[] categories)
    {
        private readonly int _totalPages = totalPages;
        private readonly int _workers = workers;
        private readonly string[] _categories = categories;
        private const string BaseUrl = "https://www.gettyimages.com/search/2/image";

        public async Task StartAsync()
        {
            List<Task> tasks = [];

            foreach (var category in _categories)
            {
                int pagesPerWorker = _totalPages / _workers;

                for (int i = 0; i < _workers; i++)
                {
                    int startPage = i * pagesPerWorker + 1;
                    int endPage = i == _workers - 1 ? _totalPages : startPage + pagesPerWorker - 1;

                    tasks.Add(Task.Run(() => ScrapeCategoryPagesAsync(category, startPage, endPage)));
                }
            }

            await Task.WhenAll(tasks);
        }

        private static async Task ScrapeCategoryPagesAsync(string category, int startPage, int endPage)
        {
            for (int page = startPage; page <= endPage; page++)
            {
                try
                {
                    var urls = GetImageUrls(category, page);
                    await DatabaseHelper.SaveUrlsAsync(urls, category);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error scraping page {page} for category '{category}': {ex.Message}");
                }
            }
        }

        private static List<string> GetImageUrls(string phrase, int page)
        {
            var url = $"{BaseUrl}?phrase={phrase}&page={page}";
            var web = new HtmlWeb();
            var document = web.Load(url);

            if (document.ParseErrors != null && document.ParseErrors.Any())
            {
                throw new Exception("Unable to load webpage");
            }

            var pictures = document.DocumentNode.SelectNodes("//picture/img");
            return pictures?.Select(x => x.Attributes["src"].Value).ToList() ?? [];
        }
    }
}