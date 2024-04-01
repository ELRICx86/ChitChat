using FLiu__Auth.Models.DTO_Message;

namespace FLiu__Auth.Services
{
    public interface IPrivateService
    {
        public Task<List<Connections>> GetConnection(Connections conn);
    }
}
