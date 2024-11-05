# GettyImagesScraper

## Overview

GettyImagesScraper is a technical assessment project to scrape image URLs from [Getty Images](https://www.gettyimages.com/), specifically based on user-specified search terms. The scraper operates using parallel processing for efficiency and stores results in an SQLite database.

## Task Requirements

1. Write a simple scraper which will get all image urls from https://www.gettyimages.com/ for a given search. Scraper should be run in parallel and take as an argument the number of workers (total number of pages should be divided between workers). Results should be written to the simple database. Please note that we do not require you to download images, we just want to save urls.

2. Run the scraper for ["hotel", "restaurant", "swimming pool"] and save urls together with their category.

## Project Structure

- **`Scraper.cs`**: Contains the scraping logic, manages parallel requests, retrieves image URLs, and divides tasks among multiple workers.
- **`DatabaseHelper.cs`**: Handles SQLite operations, including creating the database, saving URLs, and retrieving saved records.
- **`Program.cs`**: Configures and starts the scraper with specified search terms and initiates database operations.
