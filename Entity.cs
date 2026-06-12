public class Entity
{   

    public bool GiveXp {get; set;} = true;
    public bool isDead {get; set;} = false;
    public int PosX {get; set;}
    public int PosY {get; set;}

    public bool CanWalk = true;
    public int Level {get; set;} = 1;

    public double BaseLife {get; set;}
    public double TotalLife {get; set;}
    public double Life {get; set;}
    public double Damage {get; set;}
    public double BaseDamage {get; set;}

    public char Apparence {get; set;}


    public Entity()
    {   
        TotalLife = BaseLife;
        Life = TotalLife;

        Damage=BaseDamage;

        if (this.TotalLife*this.Level/1.5>this.TotalLife)
        {           
            this.TotalLife*=this.Level/1.5;
        }
    }

    public void VerifyDead()
    {
        if (this.Life<=0)
        {
            this.isDead=true;
        }
    }

    public virtual void Die()
    {}
    public void GoRight()
    {   


        if (PosX+1!=Map.width)
        {
            PosX++;
        }
    }

    public void GoLeft()
    {   

        if (PosX-1!=-1)
        {
            PosX--;
        }
    }

    public void GoUp()
    {   

        if (PosY-1!=-1)
        {
            PosY--;
        }   
    }

    public void GoDown()
    {   

        if (PosY+1!=Map.height)
        {
            PosY++;
        }
    }
}