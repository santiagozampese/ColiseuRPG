public class Goblin : Enemy
{
    public Goblin()
    {
        Apparence = 'G';

        BonusXp = 5;

        RoundsToAttack = 2;

        BaseDamage = 5;

        BaseLife = 40;

        Range = 1;

        TotalLife = BaseLife;

        Damage = BaseDamage;

    }
}
