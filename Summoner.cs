public class Summoner : Enemy
{   
    Random r = EntityManager.r;
    public Summoner()
    {   

        Apparence = 'S';

        CanWalk = false;

        BonusXp = 25;

        RoundsToSpecial = 6;

        BaseDamage = 0;

        BaseLife = 35;

        Range = 1;

        TotalLife = BaseLife;

        this.SetAttributes();
        EntityManager.AddNotSpawnedEnemy(this);
    }

    public override void Special()
    {   
        if (RoundSpecialCount>=RoundsToSpecial)
        {   
            RoundSpecialCount=0;

            if (EntityManager.EnemyList.Count>EntityManager.MaxEnemys)
            {
                return;
            }

            int x;
            if (r.Next(1, 3)==1)
            {   
                x=1;
            }
            else
            {
                x=-1;
            }

            while (true)
            {   
                if (this.PosX+x+1==Map.width) x=-1;
                else if (this.PosX+x-1==0) x=1;   
                if (x>0 && this.PosX>Map.width || EntityManager.IsPositionOccupied(this.PosX+x, this.PosY))
                {
                    x++;
                }
                else if (x<0 && this.PosX<0 || EntityManager.IsPositionOccupied(this.PosX+x, this.PosY))
                {
                    x--;
                }
                else break;
            }
            new Goblin{Level=r.Next(1, this.Level+1), PosX=this.PosX+x, PosY=this.PosY, GiveXp=false};
        }
        else
        {
            RoundSpecialCount++;
        }
    }
}