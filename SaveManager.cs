using Newtonsoft.Json;

public static class SaveManager
{   
    // Class to Manage Saves

    public static ConsoleKey resetKey = ConsoleKey.Delete;
    public static ConsoleKey saveKey = ConsoleKey.F5;
    public static ConsoleKey loadKey = ConsoleKey.F9;

    public static List<ConsoleKey> specialKeys = new();

    public static void SetKeys()
    {
        specialKeys.Clear();
        specialKeys.Add(saveKey);
        specialKeys.Add(resetKey);
        specialKeys.Add(loadKey);
    }

    public static bool IsSaved { get; set; } = false;
    public static bool IsReseted { get; set; } = false;
    public static bool IsLoaded { get; set; } = false;

    public static bool IsSaving { get; set; } = false;
    public static bool IsReseting { get; set; } = false;
    public static bool IsLoading { get; set; } = false;
    private static string SavePath = ".savegame.json";
    private static JsonSerializerSettings options = new()
    {
        Formatting = Formatting.Indented,
        NullValueHandling = NullValueHandling.Ignore,
        TypeNameHandling = TypeNameHandling.None

    };

    public static void SaveGame(Player player)
    {   
        // Save the Game

        IsSaving=true;  

        var save = new GameSave()
        {
            Player = new PlayerData()
            {
                Level = player.Level,
                Life = player.Life,
                TotalLife = player.TotalLife,
                Xp = player.xp,
                NeededXp = player.neededXp,
                PosX = player.PosX,
                PosY = player.PosY
            },
            Round = new RoundData()
            {
                CurrentRound = RoundCreator.CurrentRound,
                Turn = RoundCreator.Turn
            },
            Enemies = new List<EnemyData>(),
            SavedAt = DateTime.Now
        };

        foreach (Enemy enemy in EntityManager.EnemyList)
        {
            if (!enemy.isDead)
            {
                var enemyData = new EnemyData()
                {
                    Type = enemy.GetType().Name,
                    Level = enemy.Level,
                    Life = enemy.Life,
                    PosX = enemy.PosX,
                    PosY = enemy.PosY,
                    RoundAttackCount = enemy.RoundAttackCount,
                    RoundSpecialCount = enemy.RoundSpecialCount
                };

                if (enemy is JumpingSlime slime)
                {
                    enemyData.IsChild = slime.IsChild;
                }

                save.Enemies.Add(enemyData);
            }
        }

        string json = JsonConvert.SerializeObject(save, options);
        File.WriteAllText(SavePath, json);

        IsSaving=false; 
        IsSaved=true; 
    }

    public static bool LoadGame()
    {   
        // Load the Game
        if (!File.Exists(SavePath)) return false;

        IsLoading=true;

        string json = File.ReadAllText(SavePath);
        GameSave? save = JsonConvert.DeserializeObject<GameSave>(json, options);

        if (save == null) return false;

        EntityManager.EntityList.Clear();
        EntityManager.EnemyList.Clear();
        EntityManager.NotSpawnedEnemys.Clear();


        RoundCreator.CurrentRound = save.Round.CurrentRound;
        RoundCreator.Turn = save.Round.Turn;

        if (save.Player==null) return false;

        if (EntityManager.player==null) return false;

        Player player = EntityManager.player;
        player.Level = save.Player.Level;
        player.Life = save.Player.Life;
        player.TotalLife = save.Player.TotalLife;
        player.xp = save.Player.Xp;
        player.neededXp = save.Player.NeededXp;
        player.PosX = save.Player.PosX;
        player.PosY = save.Player.PosY;

        player.isDead=false;

        player.VerifyLevel();

        foreach (var enemyData in save.Enemies)
        {
            Enemy enemy = CreateEnemyByType(enemyData);
            enemy.Level = enemyData.Level;
            enemy.Life = enemyData.Life;
            enemy.PosX = enemyData.PosX;
            enemy.PosY = enemyData.PosY;
            enemy.RoundAttackCount = enemyData.RoundAttackCount;
            enemy.RoundSpecialCount = enemyData.RoundSpecialCount;

            enemy.SetAttributes();

            EntityManager.AddEntity(enemy);
            EntityManager.AddEnemy(enemy);
        }

        Console.Clear();
        Map.DrawMap();

        IsLoading=false; 
        IsLoaded=true; 

        return true;
    }

    public static void DeleteSave()
    {
        IsReseting=true;
        File.Delete(SavePath);

        RoundCreator.CurrentRound=1;
        RoundCreator.Turn=1;

        Program.isRunning=false;
        IsReseting=false;
        IsReseted=true;  
    }
    private static Enemy CreateEnemyByType(EnemyData data)
    {   
        // Create an Enemy based on the type
        return data.Type switch
        {
            "Goblin" => new Goblin(),
            "Archer" => new Archer(),
            "Summoner" => new Summoner(),
            "MagnectMage" => new MagnectMage(),
            "JumpingSlime" => new JumpingSlime(data.IsChild),
            _ => new Goblin()
        };
    }
}