public class Archer : Enemy
{
    public Archer()
    {
        Apparence = 'A';

        BonusXp = 10;

        RoundsToAttack = 4;

        BaseDamage = 5;

        BaseLife = 30;

        Range = 5;

        TotalLife = BaseLife;

        this.SetAttributes();
        EntityManager.AddNotSpawnedEnemy(this);
    }

     public override void WalkToPlayer()
    {    
        Player? player = EntityManager.player;
        if (player==null)
        {
            return;
        }
        
        if (EntityManager.VerifySides(player, this, this.Range))
        {
            CanWalk=false;
        }
        else
        {
            CanWalk=true;
        }
        base.WalkToPlayer();
    }

}