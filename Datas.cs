// Datas for the save
public class GameSave
{
    public PlayerData Player { get; set; } = new();
    public RoundData Round { get; set; } = new();
    public List<EnemyData> Enemies { get; set; } = new();
    public DateTime SavedAt { get; set; }
}

public class PlayerData
{
    public int Level { get; set; }
    public double Life { get; set; }
    public double TotalLife { get; set; }
    public double Xp { get; set; }
    public double NeededXp { get; set; }
    public int PosX { get; set; }
    public int PosY { get; set; }
}

public class RoundData
{
    public int CurrentRound { get; set; }
    public int Turn { get; set; }
}

public class EnemyData
{
    public string? Type { get; set; }
    public int Level { get; set; }
    public double Life { get; set; }
    public int PosX { get; set; }
    public int PosY { get; set; }
    public int RoundAttackCount { get; set; }
    public int RoundSpecialCount { get; set; }
    public bool IsChild { get; set; }
}