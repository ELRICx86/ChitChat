using FLiu__Auth.Models;
using FLiu__Auth.Models.Dto.NewFolder;

namespace FLiu__Auth.Repository
{
    public interface IFriendShipRepo
    {
        public Task<IEnumerable<FriendShipDetails>> GetAllFriends(int temp);
        public Task<FriendShip> GetFriendById(int temp);
        public Task<FriendShip> Add(int id1, int id2);
        public Task<FriendShip> Delete(int id1, int id2);
        public Task<IEnumerable<FriendShipDetails>> GetRequest(int id);
        public Task<FriendShip> Response(int user, int friend, string action);
        public Task<IEnumerable<FriendShipDetails>> GetAllPendings(int userId);
        public Task<IEnumerable<MiniStatement>> GetMiniStatement(int userId);
        public Task<int> getFriendsCount(int userId);
        public Task<IEnumerable<PotentialFriends>> getPotentialFriends(int UserId, int page_number, int page_size);
    }
}
