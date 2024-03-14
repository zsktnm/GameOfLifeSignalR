using GameOfLifeSignalR.Services;
using GameOfLifeSignalR.Shared;
using Microsoft.AspNetCore.SignalR;

namespace GameOfLifeSignalR.Hubs
{
    public class GameHub : Hub
    {
        private readonly GameRoomsService roomsService;

        public GameHub(GameRoomsService roomsService)
        {
            this.roomsService = roomsService;
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var room = roomsService.Rooms.FirstOrDefault(r => r.HasPlayer(Context.ConnectionId));
            await Console.Out.WriteLineAsync("Disconnect " + Context.ConnectionId);
            if (room is not null)
            {
                room.RemovePlayer(Context.ConnectionId);
                await Console.Out.WriteLineAsync("Removed " + Context.ConnectionId);

                await Clients.All.SendAsync("DecPlayerCount", room.Guid.ToString());
            }

            await Task.CompletedTask;
        }

        public override async Task OnConnectedAsync()
        {
            await Console.Out.WriteLineAsync("Connected " + Context.ConnectionId);
            await Clients.Caller.SendAsync("GetGamesList",
                roomsService.Rooms.Select(r => r.GetRoomInfoDTO()).ToList());
        }

        public async Task Enter(string roomId)
        {
            await Console.Out.WriteLineAsync($"Client {Context.ConnectionId} enter room {roomId} ");
            var room = roomsService.Rooms.FirstOrDefault(r => r.Guid.ToString() == roomId);
            if (room is not null)
            {
                if (room.CurrentPlayers >= room.MaxPlayers)
                {
                    await Clients.Caller.SendAsync("Leave");
                }
                room.AddPlayer(Context.ConnectionId);
                await Clients.Caller.SendAsync("LoadRoom", room.GetRoomInfoDTO());
                await Clients.All.SendAsync("IncPlayerCount", roomId);
            }
            else
            {
                await Clients.Caller.SendAsync("Leave");
            }
        }

        public async Task SendFilledCells(string roomId, Cell[] values)
        {
            var room = roomsService.Rooms.FirstOrDefault(r => r.Guid.ToString() == roomId);
            await Console.Out.WriteLineAsync(values.Length.ToString());
            if (room is not null)
            {
                room.GameField.DrawPoints(values);
                await Clients.Clients(room.Clients).SendAsync("LoadRoom", room.GetRoomInfoDTO());
                await Clients.Clients(room.Clients).SendAsync("HighlightCells", values);
            }
        }

        public async Task Leave(string roomId)
        {
            string connectionId = Context.ConnectionId;
            var room = roomsService.Rooms.FirstOrDefault(r => r.Guid.ToString() == roomId);
            if (room is not null)
            {
                room.RemovePlayer(roomId);
                await Clients.All.SendAsync("DecPlayerCount", roomId);
            }
        }
    }
}
