using FLiu__Auth.Models.DTO_Message;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace FLiu__Auth.Hubs
{
    
    public class PrivateHub : Hub
    {
        private readonly IHubContext<PrivateHub> _hubContext;

        public PrivateHub(IHubContext<PrivateHub> hubContext)
        {
            _hubContext = hubContext;
        }


        public async Task SendPrivateMessage(Message msg)
        {
            try
            {
                // Assuming msg.To and msg.From are connection IDs
                Task task = _hubContext.Clients.Clients(msg.To, msg.From).SendAsync("MethodName", msg);

                // Wait for the task to complete
                await task;

                // Check if the task completed successfully
                if (task.IsCompletedSuccessfully)
                {
                    Console.WriteLine("Method invocation successful.");
                }
                else if (task.IsFaulted)
                {
                    // Task encountered an error
                    Console.WriteLine("Method invocation failed. Error: " + task.Exception.Message);
                }
            }
            catch (Exception ex)
            {
                // Exception occurred during invocation
                Console.WriteLine("Exception occurred: " + ex.Message);
            }
        }

        public void HelloWorld()
        {
            Debug.WriteLine("Hello From Myself"+" "+ Context.ConnectionId);
        }
        public override async Task OnConnectedAsync()
        {
            string connectionId = Context.ConnectionId;
            await Clients.Caller.SendAsync("ConnectionId",connectionId);
            base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception e)
        {
            base.OnDisconnectedAsync(e);
        }
    }
}
