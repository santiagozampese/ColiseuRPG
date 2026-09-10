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

        if (!SaveManager.LoadGame()) // Load the save
        {         
            EntityManager.AddEntity(EntityManager.player);
            RoundCreator.SetLevel(); // Prepare the Level
        }

        EntityManager.player.VerifyLevel();

        RoundCreator.SpawnEnemies();

        SaveManager.SaveGame(EntityManager.player);

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

        while (!EntityManager.player.isDead || isRunning)
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

            RoundCreator.SpawnEnemies(); // Place enemies

            Walk();
            
            RoundCreator.SpawnEnemies(); // Check for new enemies

            if (EntityManager.player.isDead || !isRunning) break;

            Attack();

            RoundCreator.SpawnEnemies(); // Check for new enemies

            if (EntityManager.player.isDead || !isRunning) break;

            if (CheckEnemiesDie())
            {
                Map.DrawMap();

                Console.SetCursorPosition(0, Map.height+(Map.height/2));

                Thread.Sleep(45);
            }

            RemoveDeadEnemies();

            EntityManager.player.EnemysHits.Clear();
            EntityManager.player.DamageReceiveInCurrentTurn=0;

            RoundCreator.Turn++;
            
            EntityManager.player.VerifyLevel();

            RoundCreator.VerifyLevel();
           
            EnemiesMoves(); // Enemies turn
        }
    } 

    private static void Attack()
    {
        EntityManager.player.mode = "Attack";

        Map.DrawMap();

        Console.SetCursorPosition(0, Map.height+(Map.height/2));

        var key = Console.ReadKey(true).Key;

        if (CheckSpecialKeys(key)) Attack();

        EntityManager.player.Attack(key); // Attack
    }

    private static void Walk()
    {
        EntityManager.player.mode = "Walk";

        Map.DrawMap();

        Console.SetCursorPosition(0, Map.height+(Map.height/2));

        var key = Console.ReadKey(true).Key;

        if (CheckSpecialKeys(key)) Walk();

        EntityManager.player.Walk(key); // Walk
            
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

    public static bool CheckEnemiesDie()
    {
        if (SaveManager.IsLoading || SaveManager.IsSaving || SaveManager.IsReseting) return false;
        if (EntityManager.player == null) return false;

        bool enemieDie = false;

        foreach (Enemy enemy in EntityManager.EnemyList)
        {
            enemy.VerifyDead();

            if (enemy.isDead)
            {   
                enemieDie=true;
                // Give Xp for enemys Dead
                if (enemy.GiveXp)
                {                      
                    EntityManager.player.xp+=(EntityManager.player.xpValue*enemy.Level)+enemy.BonusXp;
                }
                EntityManager.DeadEnemiesQueue.Add(enemy);
            }
        }
        return enemieDie;
    }

    public static void RemoveDeadEnemies()
    {
        foreach (Enemy enemy in EntityManager.DeadEnemiesQueue)
        {
            enemy.Die();
        }
    }

    public static bool CheckSpecialKeys(ConsoleKey key)
    {
        if (EntityManager.player==null) return false;

        if (key==SaveManager.saveKey)
        {
            SaveManager.SaveGame(EntityManager.player);
            Console.Clear();
            Map.DrawMap();
            InfoManager.ShowInfo();
            return true;
        }
        else if (key==SaveManager.loadKey)
        {
            SaveManager.LoadGame();
            Console.Clear();
            Map.DrawMap();
            InfoManager.ShowInfo();
            return true;
        }
        else if (key==SaveManager.resetKey)
        {
            SaveManager.DeleteSave();
            Console.Clear();
            EntityManager.player.Die();
            return true;
        }
        return false;
    }
}
