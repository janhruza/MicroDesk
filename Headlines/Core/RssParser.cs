using System;
using System.Net.Http;
using System.Xml;
using Headlines.Core;
using LibMicroDesk;

public static class RssReader
{
    public static RssFeed Parse(string rssUrl)
    {
        var feed = new RssFeed();

        try
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; RefSafeRssReader/1.0)");

                using (var response = client.GetAsync(rssUrl).Result)
                using (var stream = response.Content.ReadAsStreamAsync().Result)
                using (var reader = XmlReader.Create(stream, new XmlReaderSettings
                {
                    IgnoreComments = true,
                    IgnoreWhitespace = true
                }))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element &&
                            reader.LocalName.Equals("channel", StringComparison.OrdinalIgnoreCase))
                        {
                            using (var channelReader = reader.ReadSubtree())
                            {
                                channelReader.Read(); // move to <channel>
                                ParseChannel(ref feed, channelReader);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error parsing feed: {ex}");
        }

        return feed;
    }

    private static void ParseChannel(ref RssFeed feed, XmlReader reader)
    {
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.EndElement &&
                reader.LocalName.Equals("channel", StringComparison.OrdinalIgnoreCase))
                break;

            if (!reader.IsStartElement())
                continue;

            switch (reader.LocalName.ToLowerInvariant())
            {
                case "title":
                    feed.Title = reader.ReadElementContentAsString();
                    break;
                case "link":
                    feed.Link = reader.ReadElementContentAsString();
                    break;
                case "description":
                    feed.Description = reader.ReadElementContentAsString();
                    break;
                case "item":
                    var item = new RssFeedItem();
                    using (var itemReader = reader.ReadSubtree())
                    {
                        itemReader.Read(); // move to <item>
                        ParseItem(ref item, itemReader);
                    }
                    feed.Items.Add(item);
                    break;
            }
        }
    }

    private static void ParseItem(ref RssFeedItem item, XmlReader reader)
    {
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.EndElement &&
                reader.LocalName.Equals("item", StringComparison.OrdinalIgnoreCase))
                break;

            if (!reader.IsStartElement())
                continue;

            switch (reader.LocalName.ToLowerInvariant())
            {
                case "title":
                    item.Title = reader.ReadElementContentAsString();
                    break;
                case "link":
                    item.Link = reader.ReadElementContentAsString();
                    break;
                case "description":
                    item.Description = reader.ReadElementContentAsString();
                    break;
                case "pubdate":
                    item.Date = reader.ReadElementContentAsString();
                    break;
                case "guid":
                    item.Guid = reader.ReadElementContentAsString();
                    break;
            }
        }
    }
}
