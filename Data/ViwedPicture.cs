using System.ComponentModel.DataAnnotations;

namespace GameX1.Data
{
    public class ViewedPicture
    {
        [Key]
        public int ViewedPictureId { get; set; }

        public int PictureId { get; set; }
    }
}
