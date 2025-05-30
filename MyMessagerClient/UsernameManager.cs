using System.IO;

public static class UsernameManager
{
    private const string FilePath = "username.dat";

    public static void SaveUsername(string username)
    {
        File.WriteAllText(FilePath, username);
    }

    public static string LoadUsername()
    {
        return File.Exists(FilePath) ? File.ReadAllText(FilePath) : null;
    }

    public static void DeleteUsername()
    {
        if (File.Exists(FilePath))
            File.Delete(FilePath);
    }
}