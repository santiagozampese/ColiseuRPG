public class Player : Entity
{   
    public double xpValue {get; set;} = 10;
    public double DamageReceiveInCurrentTurn {get; set;} = 0;

    public List<(Enemy enemy, double damage)> EnemysHits = new();
    public double neededXp {get; set;} = 150;
    public double xp {get; set;} = 0;
    public string? mode {get; set;}

    public Player()
    {     
        Apparence = '@';

        Level=1;

        xp=0;

        BaseDamage = 10;

        BaseLife=100;

        TotalLife = BaseLife;
        Life = TotalLife;

        Damage=BaseDamage;

        neededXp=Level*150;

        EntityManager.AddEntity(this);

    }

    public override void Die()
    {   
        EntityManager.RemoveEntity(this);
    }

    public void VerifyLevel()
    {   // Up level when claim xp
        if (this.xp>=this.neededXp)
        {
            this.Level++;

            this.xp=0;

            neededXp = Level*150;
        }

        this.Damage = this.BaseDamage*this.Level;

        if (this.BaseLife*this.Level>this.TotalLife)
        {           
            this.Life+=this.BaseLife*this.Level-this.TotalLife;
            this.TotalLife = this.BaseLife*this.Level;
        }

    }
    public void Walk(ConsoleKey key)
    {
        // Walk in direction of the key pressed

        if (!this.CanWalk) return;


        switch (key)
        {
            case ConsoleKey.W:
                this.GoUp();
                break;

            case ConsoleKey.S:
                this.GoDown();
                break;

            case ConsoleKey.D:
                this.GoRight();
                break;

            case ConsoleKey.A:
                this.GoLeft();
                break;

            default:
                break;
        }
    }

    public void Attack(ConsoleKey key)
    {   
        // Attack in the pressed direction
        foreach (Enemy enemy in EntityManager.EnemyList)
        {
            if (enemy.PosX==this.PosX && enemy.PosY==this.PosY)
            {
                enemy.Life-=this.Damage;
            }
        }
        
        switch (key)
        {
            case ConsoleKey.W:
                foreach (Enemy enemy in EntityManager.EnemyList)
                {
                    if (EntityManager.VerifyUp(this, enemy, 1) && !SamePos(enemy))
                    {
                        enemy.Life-=this.Damage;
                    }
                }
                break;

            case ConsoleKey.S:
                foreach (Enemy enemy in EntityManager.EnemyList)
                {
                    if (EntityManager.VerifyDown(this, enemy, 1) && !SamePos(enemy))
                    {
                        enemy.Life-=this.Damage;
                    }        
                }
                break;
            
            case ConsoleKey.D:
                foreach (Enemy enemy in EntityManager.EnemyList)
                {
                    if (EntityManager.VerifyRight(this, enemy, 1) && !SamePos(enemy))
                    {
                        enemy.Life-=this.Damage;
                    }
                }
                break;

            case ConsoleKey.A:
                foreach (Enemy enemy in EntityManager.EnemyList)
                {
                    if (EntityManager.VerifyLeft(this, enemy, 1) && !SamePos(enemy))
                    {
                        enemy.Life-=this.Damage;
                    }
                }
                break;
            
            default:
                break;
        }

        bool SamePos(Enemy enemy)
        {
            if (enemy.PosX==this.PosX && enemy.PosY==this.PosY) return true;
            else return false;

        }
    }
}