namespace FLiu__Auth.Models.Dto.NewFolder
{
    public class FriendShip
    {
        public bool Type { get; set; }
        public string Message { get; set; }
        public int FriendShipID { get; set; }
        public int UserID1 { get; set; }
        public int UserID2 { get; set; }
        public string Status { get; set; } //accepted, pending, rejected request
        public int? ActionUserID { get; set; } // who initiated the request
        public DateTime? LastActivityAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


    }
}
