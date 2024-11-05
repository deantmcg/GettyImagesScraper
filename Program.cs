using SQLitePCL;

namespace GettyImagesScraper
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            int totalPages = 5;
            int workers = 3;
            string[] categories = ["hotel", "restaurant", "swimming pool"];

            // Initialise database
            Batteries.Init();
            DatabaseHelper.InitialiseDatabase();

            // Initialise and start the scraper
            var scraper = new Scraper(totalPages, workers, categories);
            await scraper.StartAsync();

            Console.WriteLine("Scraping complete.");
            Console.WriteLine("Images:");

            // Get all images stored in database
            var images = await DatabaseHelper.GetAllImagesAsync();

            foreach (var image in images)
            {
                Console.WriteLine(image.ToString());
            }
        }
    }
}