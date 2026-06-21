public static class ThreadingManager
{
    public static bool IsActive {get; set;} = false;
    private static HashSet<Thread> threads = new()
    {
        new Thread(x =>
        {   
            string saveMode="";
            int count=0;
            while (true)
            {
                // This thread Write the Save mode in the screen
                if (SaveManager.IsSaved) saveMode="Game Saved!";
                if (SaveManager.IsLoaded) saveMode="Game Loaded!";
                if (SaveManager.IsReseted) saveMode="Game Reseted!";

                if (SaveManager.IsLoaded || SaveManager.IsSaved || SaveManager.IsReseted)
                {
                    if (Map.IsMapDraw)
                    {
                        Console.SetCursorPosition(Map.width+14, Map.height+6);
                        Console.Write(saveMode);
                        Thread.Sleep(10);
                        count++;
                    }
                }

                if (count>150)
                {
                    SaveManager.IsSaved=false;
                    SaveManager.IsLoaded=false;
                    SaveManager.IsReseted=false;

                    count=0;
                }
            }

        }), // end
    };

    public static void StartThreads()
    {
        foreach (Thread thread in threads)
        {
            if (thread!=null) thread.Start();      
        }
        IsActive=true;
    }
}