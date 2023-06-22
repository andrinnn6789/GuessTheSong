namespace Songlyrics.Models;

public class Owner
{
    public ExternalUrls External_Urls { get; set; }
    public Followers Followers { get; set; }
    public string Href { get; set; }
    public string Id { get; set; }
    public string Type { get; set; }
    public string Uri { get; set; }
    public string Display_Name { get; set; }
}