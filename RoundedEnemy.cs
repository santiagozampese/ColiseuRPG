public class RoundedEnemy
{   
    // class who represent if enemy can Spawn, min and max level in the round
    public bool CanSpawn {get; set;} = false;
    public int MinLevel {get; set;} = 1;
    public int MaxLevel {get; set;} = 1;
    public Func<int, int, Enemy> Spawn {get; set;}
    public RoundedEnemy(Func<int, int, Enemy> method)
    {   
        Spawn=method;
        RoundCreator.RoundedEnemys.Add(this);
    }
}