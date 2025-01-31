﻿
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using gruppArbete.Models; // använd din namespace
    using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace gruppArbete.Services
{

        public class JsonFileTaskService
        {

            public JsonFileTaskService(IWebHostEnvironment webHostEnvironment)
            {
                WebHostEnvironment = webHostEnvironment;
            }

            public IWebHostEnvironment WebHostEnvironment { get; } // until here and above is just getting the website information

            private string JsonFileName => Path.Combine(WebHostEnvironment.WebRootPath, "Data", "test.json"); //Combining the website and the json file

             public List<Books>  GetTasks()
                {
                using var jsonFileReader = File.OpenText(JsonFileName);
                return JsonSerializer.Deserialize<List<Books>>(jsonFileReader.ReadToEnd(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }

            public static void SaveBook(List<Books> UpdateList)
            {
            
                var strResultJson = JsonConvert.SerializeObject(UpdateList);
                System.Console.WriteLine(strResultJson);
                File.WriteAllText(@"wwwroot/Data/test.json", strResultJson);

            }
        public void AddReviews(string id, string review)
        {
            var books = GetTasks();
            //Finding the null value and add a new text in the review array.
            if (books.First(x => x.ISBN == id).Review == null)      
            {
                books.First(x => x.ISBN == id).Review = new string[] { review };
            }
            //Finding the review that has already been added, and add a new review.
            else
            {
                var newReview = books.First(x => x.ISBN == id).Review.ToList();
                newReview.Add(review);
                books.First(x => x.ISBN == id).Review = newReview.ToArray();
            }

            string output = JsonConvert.SerializeObject(books, Formatting.Indented);
            //File.Delete("DataSourceInJson.json");
            File.WriteAllText(@"wwwroot/Data/test.json", output);
        }

        public void Edit(Books updatedBook) // method resign oldbook info with updated.
        {
            var books = GetTasks();
            var oldBook = books.First(x => x.ISBN == updatedBook.ISBN);

            oldBook.Title = updatedBook.Title;
            oldBook.Author = updatedBook.Author;
            oldBook.Genre = updatedBook.Genre;
            oldBook.Language = updatedBook.Language;
            oldBook.ReleaseDate = updatedBook.ReleaseDate;
            oldBook.Read= updatedBook.Read;
            oldBook.Price = updatedBook.Price;
            oldBook.NobelPriceWinner = updatedBook.NobelPriceWinner;
            oldBook.Ownership = updatedBook.Ownership;
            oldBook.Publisher = updatedBook.Publisher;
            oldBook.Image = updatedBook.Image;

            //When use delete, the title will be null & therefor not be written to updated list.
            books = books.Where(b => b.Title != null).ToList();
            string output = JsonConvert.SerializeObject(books, Formatting.Indented);
            File.WriteAllText(@"wwwroot/Data/test.json", output);
        }
    }
  }




