
using System.ComponentModel.DataAnnotations;

namespace Stocks.Server.Models;

public class Article
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Url { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }
    public DateTime PublishedUtc { get; set; }

    public virtual Company Company { get; set; }
}