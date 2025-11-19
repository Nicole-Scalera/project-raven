/// <summary>
/// This class manages the current save directory for saving and loading game data.
/// It inherits from ES3SlotManager and provides a static property to get or set
/// the current save directory used throughout the game session.
/// </summary>

public class CurrentSaveDirectory : ES3SlotManager
{
    private static string directory = "";

    // Property to get or set the current save directory
    public static string CurrentDirectory
    { 
        get { return directory; }
        set { directory = value; }
    }
}