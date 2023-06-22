namespace Songlyrics.Models;

public class Item
{
    public string Added_At { get; set; }
    public AddedBy Added_By { get; set; }
    public bool Is_Local { get; set; }
    public Track Track { get; set; }
}