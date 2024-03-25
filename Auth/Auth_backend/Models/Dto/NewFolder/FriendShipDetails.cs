namespace FLiu__Auth.Models.Dto.NewFolder
{
    public class FriendShipDetails
    {
        public int FriendshipID { get; set; }
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Status { get; set; }
        public string Email { get; set; }
        public DateTime Since { get; set; } // Date since the friendship was established
    }

}
