using System.ComponentModel.DataAnnotations;

namespace BroadcastSocialMedia.ViewModels
{
    public class HomeBroadcastViewModel
    {
        [Required]
        public string Message { get; set; }

        public IFormFile ImageFile { get; set; }
        public string ErrorMessage { get; set; }
    }
}
