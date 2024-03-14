using GameOfLifeSignalR.Shared;

namespace GameOfLifeSignalR.Services
{
    public class GameRoomsService
    {
        public List<GameRoom> Rooms { get; private set; }

        private readonly Timer timer;

        public GameRoomsService(List<GameRoom> rooms)
        {
            Rooms = rooms;
            timer = new Timer(_ => Update(), null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(0.25));
        }

        private void Update()
        {
            foreach (var room in Rooms)
            {
                room.GameField.EvolutionStep();
            }
        }
    }
}
