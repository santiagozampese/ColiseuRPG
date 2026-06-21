public class Enemy : Entity
{       
    private Random r = EntityManager.r;
    public int RoundsToAttack {get;set;}
    public int RoundsToSpecial {get;set;}

    public int Range {get; set;} = 1;
    public int SpecialRange {get; set;} = 1;

    public double BonusXp {get; set;} = 0;
    public int RoundAttackCount {get; set;}
    public int RoundSpecialCount {get; set;}
    public Enemy()
    {
        this.Damage = this.BaseDamage*this.Level;

        if (this.BaseLife*this.Level>this.TotalLife)
        {           
            this.TotalLife = this.BaseLife*this.Level;
        }

        Life = TotalLife;

        PosX = r.Next(1, Map.width-1);
        PosY = r.Next(1, Map.height-1);

        SetAttributes();
        EntityManager.AddNotSpawnedEnemy(this);

    }

    public Enemy(int x, int y)
    {

        this.Damage = this.BaseDamage*this.Level;

        if (this.BaseLife*this.Level>this.TotalLife)
        {           
            this.TotalLife = this.BaseLife*this.Level;
        }

        Life = TotalLife;

        PosX = x;
        PosY = y;

        SetAttributes();
        EntityManager.AddNotSpawnedEnemy(this);

    }
    public void SetAttributes()
    {
        // Set Attributes bassed on level
        this.Damage = this.BaseDamage*this.Level;

        if (this.BaseLife*this.Level>this.TotalLife)
        {           
            this.TotalLife = this.BaseLife*this.Level;
        }

        this.Life = this.TotalLife;
    }
    public override void Die()
    {   
        EntityManager.RemoveEntity(this);
        EntityManager.RemoveEnemy(this);
    }
    public void Spawn()
    {   
        this.SetAttributes();
        EntityManager.AddEntity(this);
        EntityManager.AddEnemy(this);
        EntityManager.RemoveNotSpawnedEnemy(this);
    }
    public virtual void WalkToPlayer()
    {    
        if (!this.CanWalk)
        {
            return;
        }

        Player? player = EntityManager.player;

        if (player==null)
        {
            return;
        }

        int diffX = player.PosX-this.PosX;
        int diffY = player.PosY-this.PosY;

        if (diffX == 0 && diffY == 0)
        {
            return;
        }

        var possibleMoves = new List<(int newX, int newY, string direction)>();

        if (diffX > 0 && this.PosX + 1 < Map.width)
        {
            possibleMoves.Add((this.PosX + 1, this.PosY, "right"));
        }
        if (diffX < 0 && this.PosX - 1 < Map.width)
        {
            possibleMoves.Add((this.PosX - 1, this.PosY, "left"));
        }
        if (diffY > 0 && this.PosY + 1 < Map.height)
        {
            possibleMoves.Add((this.PosX, this.PosY + 1, "down"));
        }
        if (diffY < 0 && this.PosY - 1 < Map.height)
        {
            possibleMoves.Add((this.PosX, this.PosY - 1, "up"));
        }

        var freeMoves = possibleMoves.Where(move => 
        !EntityManager.IsPositionOccupied(move.newX, move.newY, this)
        ).ToList();

        if (freeMoves.Count == 0)
        {
            return;
        }

        if (freeMoves.Count > 0)
        {
            var chosenMove = freeMoves[r.Next(freeMoves.Count)];

            switch (chosenMove.direction)
            {
                case "right": this.GoRight(); break;
                case "left": this.GoLeft(); break;
                case "down": this.GoDown(); break;
                case "up": this.GoUp(); break;
            }

        }


    }

    public virtual void Special(){}

    public virtual void Attack()
    {   
        Player? player = EntityManager.player;

        if (player == null)
        {
            return;
        }

        if (RoundAttackCount>=RoundsToAttack)
        {   
            RoundAttackCount=0;
            if (EntityManager.VerifySides(player, this, this.Range))
            {
                player.Life-=this.Damage;
                player.DamageReceiveInCurrentTurn+=this.Damage;
                player.EnemysHits.Add((this, this.Damage));
            }
        }
        else
        {
            RoundAttackCount++;
        }
    }
}