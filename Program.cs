public static class Program
{ 
    private static Random r = EntityManager.r;    

    public static bool isRunning {get; set;} = false;

    public static void InitConfig()
    {
        SaveManager.SetKeys();

        #if DEBUG // Debug settings
        /*EntityManager.player.Level=10000;
        RoundCreator.CurrentRound=18;*/
        #endif

        EntityManager.EnemyList.Clear();
        EntityManager.EntityList.Clear();
        EntityManager.NotSpawnedEnemys.Clear();

        if (EntityManager.player == null)
        {
            EntityManager.player = new Player();
        }

        if (!ThreadingManager.IsActive) ThreadingManager.StartThreads();

        EntityManager.player.PosX=r.Next(0, Map.width);
        EntityManager.player.PosY=r.Next(0, Map.height);

        EntityManager.player.VerifyLevel();
        

        if (!SaveManager.LoadGame()) // Load the save
        {         
            RoundCreator.SetLevel(); // Prepare the Level
        }

        SaveManager.SaveGame(EntityManager.player);

        RoundCreator.SpawnEnemies();

        Console.Clear();
        Map.DrawMap();
    }
    public static void Main()
    { 
        while (true)
        {   
            if (!isRunning)
            {
                isRunning=true;
                InitConfig();
                GameLoop();
            }
        }
    } 
    public static void GameLoop()
    {   
        if (EntityManager.player == null) return;

        while (!EntityManager.player.isDead || !isRunning)
        {
            // Game loop 

            // Reset Save Mode
            SaveManager.IsSaving=false;
            SaveManager.IsLoading=false;
            SaveManager.IsReseting=false;

            EntityManager.player.VerifyDead();

            if (EntityManager.player.isDead)
            {
                EntityManager.player.Die();
            }

            if (EntityManager.player.isDead || !isRunning) break;

            RoundCreator.SpawnEnemies();

            EntityManager.player.mode = "Walk";

            Map.DrawMap();

            Console.SetCursorPosition(0, Map.height+(Map.height/2));

            var key = Console.ReadKey(true).Key;

            CheckSpecialKeys(key);

            EntityManager.player.Walk(key);
            RoundCreator.SpawnEnemies();

            if (EntityManager.player.isDead || !isRunning) break;

            EntityManager.player.mode = "Attack";

            Map.DrawMap();

            Console.SetCursorPosition(0, Map.height+(Map.height/2));

            key = Console.ReadKey(true).Key;

            CheckSpecialKeys(key);

            EntityManager.player.Attack(key);
            RoundCreator.SpawnEnemies();

            if (EntityManager.player.isDead || !isRunning) break;

            CheckEnemiesDie();

            EntityManager.player.EnemysHits.Clear();
            EntityManager.player.DamageReceiveInCurrentTurn=0;

            RoundCreator.Turn++;
            
            EntityManager.player.VerifyLevel();

            RoundCreator.VerifyLevel();
           
            EnemiesMoves();
        }
    } 

    public static void EnemiesMoves()
    {
        if (SaveManager.IsLoading || SaveManager.IsSaving || SaveManager.IsReseting) return;
        
        foreach (Enemy enemy in EntityManager.EnemyList)
        {   
            if (!enemy.isDead)
            {
                enemy.Attack();  
                enemy.Special();
                enemy.WalkToPlayer();
            }
        }  
    }

    public static void CheckEnemiesDie()
    {
        if (SaveManager.IsLoading || SaveManager.IsSaving || SaveManager.IsReseting) return;
        if (EntityManager.player == null) return;

        foreach (Entity entity in EntityManager.EntityList)
        {
            entity.VerifyDead();

            if (entity.isDead && entity is Enemy)
            {   
                // Give Xp for enemys Dead
                Enemy enemy = (Enemy)entity;
                if (enemy.GiveXp)
                {                      
                    EntityManager.player.xp+=(EntityManager.player.xpValue*entity.Level)+enemy.BonusXp;
                }
            }

            if (entity.isDead)
            {
                entity.Die();
            }
        }
    }

    public static void CheckSpecialKeys(ConsoleKey key)
    {
        if (EntityManager.player==null) return;

        if (key==SaveManager.saveKey)
        {
            SaveManager.SaveGame(EntityManager.player);
            Console.Clear();
            Map.DrawMap();
            InfoManager.ShowInfo();
        }
        else if (key==SaveManager.loadKey)
        {
            SaveManager.LoadGame();
            Console.Clear();
            Map.DrawMap();
            InfoManager.ShowInfo();
        }
        else if (key==SaveManager.resetKey)
        {
            SaveManager.DeleteSave();
            Console.Clear();
            EntityManager.player.Die();
        }
    }
}
