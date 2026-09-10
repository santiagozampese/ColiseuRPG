public class JumpingSlime : Enemy
{   
    public bool IsChild {get; set;} = false;
    public JumpingSlime(bool isChild=false)
    {   
        IsChild=isChild;

        if (isChild)
        {
            Apparence = 'j';

            BonusXp = 5;
            
            BaseDamage = 4;

            BaseLife = 20;
        }
        else
        {
            Apparence = 'J';

            BonusXp = 10;

            BaseDamage = 6;

            BaseLife = 40;
        }
        
        RoundsToAttack = 1;

        Range = 1;

        TotalLife = BaseLife;

        Damage = BaseDamage;
        HasAttack = true;
        HasSpecial = false;
    }

    public override void Die()
    {
        base.Die();

        if (!IsChild)
        {   
            for (int offset=1; offset<=this.PosX; offset++)
            {
                int newX = this.PosX - offset;
                if (newX>=0 && !EntityManager.IsPositionOccupied(newX, this.PosY))
                {
                    JumpingSlime slime = new JumpingSlime(true) {PosX=newX, PosY=this.PosY, Level=this.Level};
                    EntityManager.NotSpawnedEnemys.Add(slime);
                    slime.VerifyDead();
                    break;
                }
            }

            for (int offset=1; offset<=Map.width - 1 - this.PosX; offset++)
            {
                int newX = this.PosX + offset;
                if (newX>=0 && !EntityManager.IsPositionOccupied(newX, this.PosY))
                {
                    JumpingSlime slime = new JumpingSlime(true) {PosX=newX, PosY=this.PosY, Level=this.Level};
                    EntityManager.NotSpawnedEnemys.Add(slime);
                    slime.VerifyDead();
                    break;
                }
            }
            EntityManager.EnemyList.Remove(this);
            EntityManager.DeadEnemiesQueue.Remove(this);
        }
    }

}