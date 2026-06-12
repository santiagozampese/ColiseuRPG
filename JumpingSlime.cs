public class JumpingSlime : Enemy
{   
    public bool IsChild {get; set;} = false;
    public JumpingSlime()
    {
        Apparence = 'J';

        BonusXp = 5;

        RoundsToAttack = 2;

        BaseDamage = 6;

        BaseLife = 40;

        Range = 1;

        TotalLife = BaseLife;

        Damage = BaseDamage;

        this.SetAttributes();
        EntityManager.AddNotSpawnedEnemy(this);
    }

    public JumpingSlime(bool isChild)
    {   
        IsChild=isChild;

        if (isChild)
        {
            Apparence = 'j';

            BonusXp = 2;
            
            BaseDamage = 3;

            BaseLife = 15;
        }
        else
        {
            Apparence = 'J';

            BonusXp = 5;

            BaseDamage = 6;

            BaseLife = 30;
        }
        
        RoundsToAttack = 1;

        Range = 1;

        TotalLife = BaseLife;

        Damage = BaseDamage;

        this.SetAttributes();
        EntityManager.AddNotSpawnedEnemy(this);
    }

    public override void Die()
    {
        base.Die();

        if (!IsChild)
        {   
            while (true)
            {   
                int x = 1;
                if (this.PosX-x>=0 || !EntityManager.IsPositionOccupied(this.PosX-x, this.PosY))
                {                  
                    new JumpingSlime(true) {PosX=this.PosX-x, PosY=this.PosY, Level=this.Level};
                    break;
                }
                else x++;

                if (this.PosX<0)
                {
                    break;
                }
            }
            while (true)
            {   
                int x = 1;
                if (this.PosX+x<Map.width || !EntityManager.IsPositionOccupied(this.PosX+x, this.PosY))
                {                  
                    new JumpingSlime(true) {PosX=this.PosX+x, PosY=this.PosY, Level=this.Level};
                    break;
                }
                else x++;

                if (this.PosX>Map.width)
                {
                    break;
                }
            }
        }
    }

}