namespace GameOfLifeSignalR.Shared
{
    public class RoomInfoDTO
    {
        public required Guid Guid { get; set; }
        public required int MaxPlayers { get; set; }
        public required int CurrentPlayers { get; set; }
        public required bool[][] Field { get; set; }
    }
}
