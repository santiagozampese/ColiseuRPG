public class MagnectMage : Enemy
{
    public MagnectMage()
    {
        Apparence = 'M';

        BonusXp = 30;

        RoundsToAttack = 2;

        RoundsToSpecial=3;

        BaseDamage = 6;

        BaseLife = 50;

        Range = 1;
        SpecialRange = 4;

        TotalLife = BaseLife;

        Damage = BaseDamage;

        this.SetAttributes();
        EntityManager.AddNotSpawnedEnemy(this);
    }

    public override void Special()
    {
        if (RoundSpecialCount>=RoundsToSpecial)
        {   
            RoundSpecialCount=0;

            Player player;
            if (EntityManager.player!=null)
            {
                player = EntityManager.player;
            }
            else
            {
                return;
            }

            if (EntityManager.VerifySides(this, player, SpecialRange))
            {
                if (EntityManager.VerifyUp(this, player, SpecialRange))
                {
                    player.PosY++;
                }

                if (EntityManager.VerifyDown(this, player, SpecialRange))
                {
                    player.PosY--;
                }

                if (EntityManager.VerifyRight(this, player, SpecialRange))
                {
                    player.PosX--;
                }

                if (EntityManager.VerifyLeft(this, player, SpecialRange))
                {
                    player.PosX++;
                }
            }
        }

        else
        {
            RoundSpecialCount++;
        }
    }
}