public class MagnectMage : Enemy
{
    public MagnectMage()
    {
        Apparence = 'M';

        BonusXp = 30;

        RoundsToAttack = 3;

        RoundsToSpecial=4;

        BaseDamage = 8;

        BaseLife = 50;

        Range = 1;
        SpecialRange = 4;

        TotalLife = BaseLife;

        Damage = BaseDamage;

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
                if (EntityManager.VerifyDirection(this, player, SpecialRange, 0, -1))
                {
                    player.PosY++;
                }

                if (EntityManager.VerifyDirection(this, player, SpecialRange, 0, 1))
                {
                    player.PosY--;
                }

                if (EntityManager.VerifyDirection(this, player, SpecialRange, 1, 0))
                {
                    player.PosX--;
                }

                if (EntityManager.VerifyDirection(this, player, SpecialRange, -1, 0))
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