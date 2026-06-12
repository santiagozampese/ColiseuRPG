public static class EntityManager
{
    public static HashSet<Entity> EntityList = new();

    public static HashSet<Enemy> NotSpawnedEnemys = new();

    public static Player? player {get; set;}

    public static HashSet<Enemy> EnemyList = new();

    public static Random r = new();

    public static int MaxEnemys = 28;

    public static bool IsPositionOccupied(int x, int y, Entity? ignoreEntity = null)
    {
        foreach (var entity in EntityList)
        {
            if (entity == ignoreEntity) continue;
            if (!entity.isDead && entity.PosX ==x && entity.PosY == y)
            {
                return true;
            }
        }
        return false;
    }
    public static bool VerifySides(Entity entity, Entity target, int distance)
    {
        if (target == null) return false;
        int dx = Math.Abs(target.PosX-entity.PosX);
        int dy = Math.Abs(target.PosY-entity.PosY);
        return (dx <= distance && dy == 0) || (dy <= distance && dx == 0);
    }

    public static bool VerifyRight(Entity entity, Entity target, int distance)
    {
        if (target?.PosX-entity.PosX<=distance && target?.PosX>=entity.PosX && entity.PosY==target?.PosY)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool VerifyLeft(Entity entity, Entity target, int distance)
    {
        if (target?.PosX-entity.PosX>=-distance && target?.PosX<=entity.PosX && entity.PosY==target?.PosY)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool VerifyUp(Entity entity, Entity target, int distance)
    {
        if (target?.PosY-entity.PosY>=-distance && target?.PosY<=entity.PosY && entity.PosX==target?.PosX)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool VerifyDown(Entity entity, Entity target, int distance)
    {
        if (target?.PosY-entity.PosY<=distance && target?.PosY>=entity.PosY && entity.PosX==target?.PosX)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static void AddEntity(Entity entity)
    {
        EntityList.Add(entity);
    }
    public static void RemoveEntity(Entity entity)
    {
        EntityList.Remove(entity);
    }
    public static void AddEnemy(Enemy enemy)
    {
        EnemyList.Add(enemy);
    }
    public static void RemoveEnemy(Enemy enemy)
    {
        EnemyList.Remove(enemy);
    }
    public static void RemoveNotSpawnedEnemy(Enemy enemy)
    {
        NotSpawnedEnemys.Remove(enemy);
    }
    public static void AddNotSpawnedEnemy(Enemy enemy)
    {
        NotSpawnedEnemys.Add(enemy);
    }

}