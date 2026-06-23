public static class RoundCreator
{
    public static Random r = EntityManager.r;
    public static int CurrentRound {get; set;} = 1;

    public static int Turn {get; set;} = 1;

    public static int EnemysCount = 0;


    public static int EnemysAmount {get; set;}

    public static List<RoundedEnemy> RoundedEnemys = new();
    private static RoundedEnemy goblin = new((a, b) => new Goblin{Level=r.Next(a, b)});
    private static RoundedEnemy archer = new((a, b) => new Archer{Level=r.Next(a, b)});
    private static RoundedEnemy summoner = new((a, b) => new Summoner{Level=r.Next(a, b)});
    private static RoundedEnemy magnectMage = new((a, b) => new MagnectMage{Level=r.Next(a, b)});
    private static RoundedEnemy jumpingSlime = new((a, b) => new JumpingSlime{Level=r.Next(a, b)});
    public static bool roundClear = false;

    public static void VerifyLevel()
    {   // Go to next round if clear the current round

        if (EntityManager.EnemyList.Count==0)
        {
            SetLevel();
            CurrentRound++;
            Turn=1;   
            roundClear = true;  
        }
        
        if (roundClear) 
        {
            SpawnEnemies();
            if (EntityManager.player!=null) SaveManager.SaveGame(EntityManager.player); // AutoSave
            roundClear = false;
        }
        
    }

    public static void SpawnEnemies()
    {
        foreach (Enemy enemy in EntityManager.NotSpawnedEnemys)
        {
            enemy.Spawn();
        } 
    }

    public static void SetLevel()
    {   // Create enemys
        ChooseEnemyAmount();
        ChooseEnemys();
        
        {
            if (EntityManager.player?.Life+(EntityManager.player?.TotalLife*10/100)>EntityManager.player?.TotalLife)
            {
                EntityManager.player.Life=EntityManager.player.TotalLife;
            }
            else
            {
                EntityManager.player?.Life+=EntityManager.player.TotalLife*10/100;
            }
        }

        for (int i=0; i<EnemysAmount;)
        {   
            if (EntityManager.EnemyList.Count>=EntityManager.MaxEnemys)
            {
                break;
            }
            int index = r.Next(0, RoundedEnemys.Count);
            RoundedEnemy enemy = RoundedEnemys[index];
            if (enemy.CanSpawn)
            {
                enemy.Spawn(enemy.MinLevel, enemy.MaxLevel+1);
                i++;
            }
        }
  
        foreach (Enemy enemy in EntityManager.EnemyList)
        {
            enemy.SetAttributes();
        }
    }

    public static void ChooseEnemyAmount()
    {   // Decide the total enemys in round
        if (CurrentRound<3)
        {
            EnemysAmount=r.Next(2,4);
        }
        else if (CurrentRound<7)
        {
            EnemysAmount=r.Next(4,8);
        }
        else if (CurrentRound<13)
        {
            EnemysAmount=r.Next(8,12);
        }
        else
        {
            EnemysAmount=r.Next(12,16);
        }
    }
    public static void ChooseEnemys()
    {   // Choose if enemys can spawn and the max and minimum level
        goblin.CanSpawn=true;
        if (CurrentRound>3)
        {
            archer.CanSpawn=true;
            goblin.MaxLevel=2;
        }
        if (CurrentRound>5)
        {   
            jumpingSlime.CanSpawn=true;

            goblin.MinLevel=2;
            archer.MaxLevel=2;
        }
        if (CurrentRound>7)
        {
            summoner.CanSpawn=true;
            magnectMage.CanSpawn=true;

            jumpingSlime.MaxLevel=2;
            archer.MaxLevel=4;
            goblin.MaxLevel=5;
        }
        if (CurrentRound>9)
        {   
            magnectMage.MaxLevel=2;

            summoner.MaxLevel=2;

            jumpingSlime.MaxLevel=4;

            archer.MinLevel=2;
            archer.MaxLevel=6;

            goblin.MinLevel=5;
            goblin.MaxLevel=7;

        }
        if (CurrentRound>11)
        {   
            jumpingSlime.MinLevel=2;
            jumpingSlime.MaxLevel=4;

            magnectMage.MaxLevel=4;

            summoner.MinLevel=2;
            summoner.MaxLevel=4;
        }
        if (CurrentRound>13)
        {   
            magnectMage.MaxLevel=6;
            magnectMage.MinLevel=3;

            jumpingSlime.MinLevel=4;
            jumpingSlime.MaxLevel=6;

            goblin.MinLevel=6;
            goblin.MaxLevel=10;

            archer.MinLevel=6;
            archer.MaxLevel=10;

            summoner.MinLevel=3;
            summoner.MaxLevel=7;
        }
        if (CurrentRound>17)
        {    
            magnectMage.MaxLevel=9;
            magnectMage.MinLevel=6;

            jumpingSlime.MinLevel=4;
            jumpingSlime.MaxLevel=8;

            goblin.MaxLevel=12;
            goblin.MinLevel=8;

            archer.MaxLevel=12;
            archer.MinLevel=7;

            summoner.MaxLevel=9;
            summoner.MinLevel=6;
        }

    }
}
