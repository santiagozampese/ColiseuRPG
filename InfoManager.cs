public static class InfoManager
{   
    public static string? playerInfo;
    public static void ShowInfo()
    {   
        if (EntityManager.player == null) return;

        int? plusHits = 0;
        Func<List<(Enemy enemy, double damage)>, string> HitsString = x =>{
            if (x==null) return "";
            string? text="";
            for (int j=0; j<x.Count; j++)
            {   
                if (x.Count==0) break;
                text+=$"{x.ToList()[j].enemy.GetType()} - {x.ToList()[j].damage}\n";
                if (j>=4) plusHits=x.Count-4;
            }
            if (text!="") text="Enemys Damage - " + text;
            return text;
        };

        Func<int?, string> plusHitsString = x =>
        {
            if (x>0) return $"+{x}";
            else return "";

        };

        
        Console.SetCursorPosition(0, Map.height+4);

        string playerInfo = $"""
        Life - {(int?)EntityManager.player.Life}       Mode - {EntityManager.player.mode}   Round - {RoundCreator.CurrentRound}   Turn - {RoundCreator.Turn}
        Damage - {(int?)EntityManager.player.Damage}   Xp - {(int?)EntityManager.player.xp}/{(int?)EntityManager.player.neededXp}    Level - {EntityManager.player.Level}

        Receive Damage - {(int?)EntityManager.player.DamageReceiveInCurrentTurn}

        {HitsString(EntityManager.player.EnemysHits)}
        {plusHitsString(plusHits)}
        """;

        Console.WriteLine(playerInfo);
    
        if (EntityManager.player==null)
        {
            return;
        }
        int i=1;
        foreach (Enemy enemy in EntityManager.EnemyList)
        {   

            if (i>=4) break;
            int distance;
            if (enemy.Range>enemy.SpecialRange)
            {
                distance=enemy.Range;
            }
            else
            {
                distance=enemy.SpecialRange;
            }

            if (EntityManager.VerifySides(EntityManager.player, enemy, distance+1))
            {
                List<string> enemyInfo = new();

                enemyInfo = [$"Type - {enemy.GetType()}", $"Life - {(int?)enemy.Life}", $"Damage - {(int?)enemy.Damage}", $"Level- {enemy.Level}", $"Range - {enemy.Range}", $"Attack in - {enemy.RoundsToAttack-enemy.RoundAttackCount}", $"Special in - {enemy.RoundsToSpecial-enemy.RoundSpecialCount}"];

                int j=0;
                foreach (var info in enemyInfo)
                {   
                    if (Map.width+(14*i)+2 < Console.WindowWidth)
                    {
                        Console.SetCursorPosition(Map.width+(14*i)+2, j);
                        Console.Write(info);  
                        j++;           
                    }
                    else
                    {
                        break;
                    }
                }
                i++;
            }           
        }      
    }
}