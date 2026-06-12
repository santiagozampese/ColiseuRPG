public static class InfoManager
{   
    static Player? player = EntityManager.player;
    public static string? playerInfo;
    public static void ShowInfo()
    { 
        int? plusHits = 0;
        Func<List<(Enemy enemy, double damage)>, string> HitsString = x =>{
            if (x==null) return "";
            string? text="Enemys Damage - ";
            for (int j=0; j<x.Count; j++)
            {   
                if (x.Count==0) break;
                text+=$"{x.ToList()[j].enemy.GetType()} - {x.ToList()[j].damage}\n";
                if (j>=4) plusHits=x.Count-4;
            }
            return text;
        };

        Func<int?, string> plusHitsString = x =>
        {
            if (x>0) return $"+{x}";
            else return "";

        };

        Console.SetCursorPosition(0, Map.height+4);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        playerInfo = $"""
        Life - {(int?)player.Life}       Mode - {player.mode}   Round - {RoundCreator.CurrentRound}   Turn - {RoundCreator.Turn}
        Damage - {(int?)player.Damage}   Xp - {(int?)player.xp}/{(int?)player.neededXp}    Level - {player.Level}

        Receive Damage - {(int?)player.DamageReceiveInCurrentTurn}

        {HitsString(player.EnemysHits)}
        {plusHitsString(plusHits)}
        """;
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        Console.WriteLine(playerInfo);
    


        if (player==null)
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

            if (EntityManager.VerifySides(player, enemy, distance+1))
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