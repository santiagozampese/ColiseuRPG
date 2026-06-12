public class RoundedEnemy
{   
    // class who represent if enemy can Spawn, min and max level in the round
    public bool CanSpawn {get; set;} = false;
    public int MinLevel {get; set;} = 1;
    public int MaxLevel {get; set;} = 1;
    public Action<int, int> Spawn {get; set;}
    public RoundedEnemy(Action<int, int> method)
    {   
        Spawn=method;
        RoundCreator.RoundedEnemys.Add(this);
    }
}