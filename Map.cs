public static class Map
{
    public const int height = 20;
    public const int width = 50;

    public static bool IsMapDraw {get; set;}

    public static char theme = '.';

    public static void DrawMap()
    {   
        // Draw the map
        
        IsMapDraw=false;

        Console.Clear();

        for (int x=0; x<width+2; x++)
        {
            Console.Write("-");
        }

        Console.WriteLine("");

        for (int y=0; y<height; y++)
        {
            Console.Write("|");

            for (int x=0; x<width; x++)
            {
                char apparence = VerifyEntitys(x, y);
                
                Console.Write(apparence);
            }

            Console.Write("|");

            Console.WriteLine("");
        }

        for (int x=0; x<width+2; x++)
        {
            Console.Write("-");
        }

        Console.WriteLine("");

        InfoManager.ShowInfo();

        IsMapDraw=true;
    }

    public static char VerifyEntitys(int x, int y)
    {
        // verify the entitys in map and put them in their positions
    
        foreach (var entity in EntityManager.EntityList)
        {   
            if (!entity.isDead)
            {
                if (entity.PosX == x && entity.PosY == y)
                {
                    return entity.Apparence;
                }
            }
        }

        return theme;
    }
}