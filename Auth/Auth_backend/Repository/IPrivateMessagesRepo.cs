﻿using FLiu__Auth.Models.DTO_Message;

namespace FLiu__Auth.Repository
{
    public interface IPrivateMessagesRepo
    {
        public Task<List<Connections>> GetConnection(Connections conn);
    }
}
