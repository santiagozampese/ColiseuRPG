using System.Text;

public static class InfoManager
{   
    public static string? playerInfo;
    public static void ShowInfo()
    {   
        if (EntityManager.player == null) return;

        int? plusHits = 0;
        Func<List<(Enemy enemy, double damage)>, string> HitsString = x =>
        {
            if (x==null) return "";
            var hits = x.ToList();
            StringBuilder text = new();

            int counter = 0;
            foreach (var hit in hits)
            {   
                if (x.Count==0) break;
                text.Append($"{hit.enemy.GetType()} - {hit.damage}");
                text.AppendLine();
                if (counter >= 4) plusHits=x.Count-4;
            }
            return text.ToString();
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

                enemyInfo = [$"Type - {enemy.GetType()}", $"Life - {(int?)enemy.Life}",
                $"Damage - {(int?)enemy.Damage}", $"Level- {enemy.Level}",
                $"Range - {enemy.Range}",
                (enemy.HasAttack ? $"Attack in - {enemy.RoundsToAttack-enemy.RoundAttackCount}" : ""),
                (enemy.HasSpecial ? $"Special in - {enemy.RoundsToSpecial-enemy.RoundSpecialCount}" : "")];

                int j=0;
                foreach (var info in enemyInfo)
                {   
                    if (Map.width+(14*i)+2 < Console.WindowWidth)
                    {
                        Console.SetCursorPosition(Map.width+(14*i)+2, j);
                        if (!string.IsNullOrWhiteSpace(info))
                        {                        
                            Console.Write(info);  
                            j++;           
                        }
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