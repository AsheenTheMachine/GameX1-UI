using System.ComponentModel.DataAnnotations;

namespace GameX1.Data
{
    public class Picture
    {
        [Key]
        public int PictureId { get; set; }

        public string? Url { get; set; } = "";

        public string? Name { get; set; } = "";

        public string? Power { get; set; } = "";

        public int Age { get; set; } = 0;
    }
}
