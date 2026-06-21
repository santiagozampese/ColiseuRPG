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

        BaseLife = 40;

        Range = 1;

        TotalLife = BaseLife;
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

            int direction = r.Next(0, 2);
            
            int newX=this.PosX;

            if (direction==0)
            {
                for (int offset=1; offset<=Map.width - 1 - this.PosX; offset++)
                {
                    newX = this.PosX + offset;
                    if (!EntityManager.IsPositionOccupied(newX, this.PosY))
                    {
                        break;
                    }
                }
            }
            else
            {
                for (int offset=1; offset<=this.PosX; offset++)
                {
                    newX = this.PosX - offset;
                    if (!EntityManager.IsPositionOccupied(newX, this.PosY))
                    {
                        break;
                    }
                }
            }

            if (newX>=0 && newX < Map.width)    
            {
                Goblin goblin = new Goblin{Level=r.Next(1, this.Level+1), PosX=newX, PosY=this.PosY, GiveXp=false};
            }
        }
        else
        {
            RoundSpecialCount++;
        }
    }
}