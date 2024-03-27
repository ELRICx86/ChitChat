namespace FLiu__Auth.Models.Dto.NewFolder
{
    public class MiniStatement
    {
        public int FriendshipID { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Status { get; set; }
        public string LastMessage { get; set; }
        public DateTime LastActivityAt { get; set; }
        public bool IsActive { get; set; }
        public string UserName { get; set; }
    }
}
