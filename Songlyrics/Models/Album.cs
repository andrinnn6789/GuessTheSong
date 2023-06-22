namespace Songlyrics.Models;

public class Album
{
    public string Album_Type { get; set; }
    public int Total_Tracks { get; set; }
    public string[] Available_Markets { get; set; }
    public ExternalUrls External_Urls { get; set; }
    public string Href { get; set; }
    public string Id { get; set; }
    public Image[] Images { get; set; }
    public string Name { get; set; }
    public string Release_Date { get; set; }
    public string Release_Date_Precision { get; set; }
    public Restrictions Restrictions { get; set; }
    public string Type { get; set; }
    public string Uri { get; set; }
    public Copyright[] Copyrights { get; set; }
    public ExternalIds External_Ids { get; set; }
    public string[] Genres { get; set; }
    public string Label { get; set; }
    public int Popularity { get; set; }
    public string Album_Group { get; set; }
    public Artist[] Artists { get; set; }
}