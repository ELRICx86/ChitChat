using Microsoft.AspNetCore.Server.HttpSys;

namespace chatapi.Services
{
    public class ChatService
    {
        private readonly Dictionary<string, string> Users = new Dictionary<string, string>();
        
        public bool AddUserToList(string userId)
        {
            lock (Users)
            {
                foreach (var user in Users)
                {
                    if (user.Key.ToLower() == userId.ToLower())
                    {
                        return false;
                    }
                }
                Users.Add(userId, null);
                return true;
            }
        }

        public void AddUserConnectionId(string userId, string connectionId) 
        {
            lock (Users)
            {
                if(Users.ContainsKey(userId))
                {
                    Users[userId] = connectionId;
                }
            }
        }

        public string GetUserByConnectionId(string connectionId)
        {
            lock (Users)
            {
                foreach(var val in  Users)
                {
                    if (val.Value.ToLower() == connectionId.ToLower())
                    {
                        return val.Key;
                    }
                }
                return null;
            }
        }

        public string GetConnectionIdByUser(string userId)
        {
            lock (Users)
            {
                foreach (var val in Users)
                {
                    if (val.Key.ToLower() == userId)
                    {
                        return val.Value;
                    }
                }
                return null;
            }
        }

        public void RemoveUserFromList(string userId)
        {
            if (Users.ContainsKey(userId))
            {
                Users.Remove(userId);
            }
        }

        public string[] GetOnlineUsers()
        {
            lock(Users)
            {
                return Users.OrderBy(x => x.Key).Select(x => x.Key).ToArray();
            }
        }


    }
}
