namespace GameOfLifeSignalR.Shared
{
    public class GameRoom
    {
        public Guid Guid { get; }

        public GameField GameField { get; }

        public int MaxPlayers { get; }

        public List<string> Clients { get; }
        public int CurrentPlayers => Clients.Count;


        public GameRoom(int rows, int cols, int maxPlayers)
        {
            Guid = Guid.NewGuid();
            GameField = new GameField(rows, cols);
            MaxPlayers = maxPlayers;
            Clients = new List<string>();
        }

        public void AddPlayer(string connectionId)
        {
            Clients.Add(connectionId);
        }

        public bool RemovePlayer(string connectionId)
        {
            return Clients.Remove(connectionId);
        }

        public bool HasPlayer(string connectionId)
        {
            return Clients.Contains(connectionId);
        }

        public RoomInfoDTO GetRoomInfoDTO()
        {
            bool[][] fieldValues = new bool[GameField.Rows][];
            for (int i = 0; i < GameField.Rows; i++)
            {
                fieldValues[i] = new bool[GameField.Columns];
                for (int j = 0; j < GameField.Columns; j++)
                {
                    fieldValues[i][j] = GameField[i, j];
                }
            }
            return new RoomInfoDTO
            {
                CurrentPlayers = CurrentPlayers,
                Guid = Guid,
                MaxPlayers = MaxPlayers,
                Field = fieldValues
            };
        }

    }
}
