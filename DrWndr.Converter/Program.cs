using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using DrWndr.Models;
using Newtonsoft.Json;

namespace DrWndr.Converter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start converter.");

            // List of found items.
            var foundItems = new List<SyndicationItem>();

            // Get all files from input folder.
            var filePaths = Directory.GetFiles(@"C:\Users\Tobias\Desktop\input", "*.rss");

            // Parse each file and add result to found items.
            foreach (var path in filePaths)
            {
                var reader = XmlReader.Create(path);
                var formatter = new Rss20FeedFormatter();
                formatter.ReadFrom(reader);
                reader.Close();
                foundItems.AddRange(formatter.Feed.Items);
            }

            // Make a distinct list of found items.
            // and transfer items to new data object structure.
            var items = foundItems
                .GroupBy(i => i.Id)
                .Select(g => g.First())
                .ToList()
                .Select(i => new Post(item: i))
                .ToList();

            // Serialize list of objects into a json structure.
            var jsonString = JsonConvert.SerializeObject(items);

            // Write it to disk.
            File.WriteAllText(@"C:\Users\Tobias\Desktop\input\result.json", jsonString);
        }
    }
}
