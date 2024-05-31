namespace ComicApp.Models
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public List<Comic> Comics { get; set; }
    }
}
