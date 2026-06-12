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

        EntityManager.player = this;

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
    public void Walk()
    {
        // Walk in direction of the key pressed
        if (this.CanWalk)
        {          
            switch (char.ToLower(Console.ReadKey().KeyChar))
            {
                case 'w':
                    this.GoUp();
                    break;

                case 's':
                    this.GoDown();
                    break;
                
                case 'd':
                    this.GoRight();
                    break;

                case 'a':
                    this.GoLeft();
                    break;
            }
        }
    }

    public void Attack()
    {   // Attack in the pressed direction
        switch (char.ToLower(Console.ReadKey().KeyChar))
        {
            case 'w':
                foreach (Enemy enemy in EntityManager.EnemyList)
                {
                    if (EntityManager.VerifyUp(this, enemy, 1))
                    {
                        enemy.Life-=this.Damage;
                        AttackSamePos(enemy);
                    }
                }
                break;

            case 's':
                foreach (Enemy enemy in EntityManager.EnemyList)
                {
                    if (EntityManager.VerifyDown(this, enemy, 1))
                    {
                        enemy.Life-=this.Damage;
                        AttackSamePos(enemy);
                    }        
                }
                break;
            
            case 'd':
                foreach (Enemy enemy in EntityManager.EnemyList)
                {
                    if (EntityManager.VerifyRight(this, enemy, 1))
                    {
                        enemy.Life-=this.Damage;
                        AttackSamePos(enemy);
                    }
                }
                break;

            case 'a':
                foreach (Enemy enemy in EntityManager.EnemyList)
                {
                    if (EntityManager.VerifyLeft(this, enemy, 1))
                    {
                        enemy.Life-=this.Damage;
                        AttackSamePos(enemy);
                    }
                }
                break;
        }

        void AttackSamePos(Enemy enemy)
        {
            if (enemy.PosX==this.PosX && enemy.PosY==this.PosY)
            {
                enemy.Life-=this.Damage;
            }
        }
    }
}