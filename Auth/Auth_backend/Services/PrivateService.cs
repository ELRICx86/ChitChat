using FLiu__Auth.Models.DTO_Message;
using FLiu__Auth.Repository;

namespace FLiu__Auth.Services
{
    public class PrivateService : IPrivateService
    {
        private readonly IPrivateMessagesRepo _messagesRepo;
        public PrivateService(IPrivateMessagesRepo messagesRepo)
        {
            _messagesRepo = messagesRepo;
        }
        public async Task<List<Connections>> GetConnection(Connections conn)
        {
            var connections = new List<Connections>();
            connections = await _messagesRepo.GetConnection(conn);
            return connections;
        }
    }
}
