namespace BroadcastSocialMedia.Models
{
    public class Broadcast
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime Published { get; set; } = DateTime.Now;
        public string ImageFilenameGUID { get; set; }
        public ICollection<UserThatLikeBroadcast> UserThatLike { get; set; } = new List<UserThatLikeBroadcast>();
    }
}
