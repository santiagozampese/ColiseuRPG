public static class Program
{   
    public static void Main()
    {  
        Random r = EntityManager.r;

        Player player = new();

        #if DEBUG
        /*player.Level=10000;
        player.VerifyLevel();
        RoundCreator.CurrentRound=18;*/
        #endif

        RoundCreator.SetLevel();


        if (player == null)
        {
            return;
        }

        while (!player.isDead)
        {
            // Game loop 

            foreach (Enemy enemy in EntityManager.NotSpawnedEnemys)
            {
                enemy.Spawn();
            }    

            player.VerifyDead();

            if (player.isDead)
            {
                player.Die();
            }

            player.mode = "Walk";

            Map.DrawMap();
            InfoManager.ShowInfo();

            Console.SetCursorPosition(0, Map.height+(Map.height/2));

            player.Walk();

            player.mode = "Attack";

            Map.DrawMap();
            InfoManager.ShowInfo();

            Console.SetCursorPosition(0, Map.height+(Map.height/2));

            player.Attack();


            foreach (Entity entity in EntityManager.EntityList)
            {
                entity.VerifyDead();

                if (entity.isDead && entity is Enemy)
                {   
                    // Give Xp for enemys Dead
                    Enemy enemy = (Enemy)entity;
                    if (enemy.GiveXp)
                    {                      
                        player.xp+=(player.xpValue*entity.Level)+enemy.BonusXp;
                    }
                }

                if (entity.isDead)
                {
                    entity.Die();
                }
            }

            player.EnemysHits.Clear();
            player.DamageReceiveInCurrentTurn=0;

            RoundCreator.Turn++;
            
            player.VerifyLevel();

            RoundCreator.VerifyLevel();
           
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
    }
    
}