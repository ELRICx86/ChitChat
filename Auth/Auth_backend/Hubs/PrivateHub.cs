﻿using FLiu__Auth.Models.DTO_Message;
using FLiu__Auth.Services;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace FLiu__Auth.Hubs
{
    
    public class PrivateHub : Hub
    {
        private readonly IHubContext<PrivateHub> _hubContext;
        private readonly IPrivateService _pService;

        public PrivateHub(IHubContext<PrivateHub> hubContext, IPrivateService pservice)
        {
            _hubContext = hubContext;
            _pService = pservice;
        }

/*
        public async Task SendPrivateMessage(Message msg)
        {
            try
            {
                // Assuming msg.To and msg.From are connection IDs
                Task task = _hubContext.Clients.Clients(msg.To, msg.From).SendAsync("SendPrivateMessage", msg);

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
        }*/

        public async Task Connected(Connections s)
        {
            // Assuming 's' is of type that contains connectionId and UserId properties


            try
            {
                Connections conn = new Connections
                {
                    ConnectionId = s.ConnectionId,
                    UserId = s.UserId,
                };

                var list = await _pService.GetConnection(conn).ConfigureAwait(false);

                foreach (var x in list)
                {
                    await Clients.Client(x.ConnectionId).SendAsync("UserConnected", s);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions gracefully
                Console.WriteLine("An error occurred in Connected method: " + ex.Message);
            }
        }



        public void HelloWorld()
        {
            Debug.WriteLine("Hello From Myself"+" "+ Context.ConnectionId);
        }
        
        public override async Task OnConnectedAsync()
        {
            string connectionId = Context.ConnectionId;
            //await Clients.Caller.SendAsync("OnConnect",connectionId);

           
            
            await Clients.Caller.SendAsync("OnConnect", connectionId);
            //base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception e)
        {
            base.OnDisconnectedAsync(e);
        }
    }
}
