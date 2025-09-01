using System;
using System.Collections.Generic;
using System.Xml;
using Headlines.Core;
using LibMicroDesk;

/// <summary>
/// Representing a simple RSS parser.
/// </summary>
public static class RssParser
{
    /// <summary>
    /// Parses the RSS feed from the given URL.
    /// </summary>
    /// <param name="rssUrl">Source address.</param>
    /// <returns>Parsed RSS feed item.</returns>
    public static RssFeed Parse(string rssUrl)
    {
        try
        {
            Uri uri = new Uri(rssUrl, UriKind.RelativeOrAbsolute);
            var feed = new RssFeed
            {
                Items = new List<RssFeedItem>()
            };

            using (var reader = XmlReader.Create(uri.AbsoluteUri))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement() && reader.Name == "channel")
                    {
                        ParseChannel(reader, ref feed);
                    }
                }
            }

            return feed;
        }

        catch (Exception ex)
        {
            Log.Error(ex, nameof(Parse));
            return new RssFeed();
        }
    }

    private static void ParseChannel(XmlReader reader, ref RssFeed feed)
    {
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.EndElement && reader.Name.ToLower() == "channel")
                break;

            if (reader.IsStartElement())
            {
                switch (reader.Name.ToLower())
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
                    case "image":
                        feed.Image = ParseImage(reader);
                        break;
                    case "item":
                        var itemReader = reader.ReadSubtree(); // Create a sub-reader for the item
                        var item = ParseItem(itemReader);
                        feed.Items.Add(item);
                        itemReader.Close(); // Important: close the sub-reader
                        break;
                }
            }
        }
    }

    private static string ParseImage(XmlReader reader)
    {
        string imageUrl = "";

        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "image")
                break;

            if (reader.IsStartElement() && reader.Name == "url")
            {
                imageUrl = reader.ReadElementContentAsString();
            }
        }

        return imageUrl;
    }

    private static RssFeedItem ParseItem(XmlReader reader)
    {
        var item = new RssFeedItem();

        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "item")
                break;

            if (reader.IsStartElement())
            {
                switch (reader.Name)
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
                    case "pubDate":
                        item.Date = reader.ReadElementContentAsString();
                        break;
                    case "guid":
                        item.Guid = reader.ReadElementContentAsString();
                        break;
                }
            }
        }

        return item;
    }
}