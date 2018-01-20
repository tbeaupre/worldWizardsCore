using System.IO;

namespace WorldWizards.core.file.utils
{
    // @author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
    /// <summary>
    /// Utility for Saving and Loading JSON strings to and from files.
    /// </summary>
    public static class FileIO
    {
        public static string testPath = "Assets/Resources/test.json";

        public static void SaveJsonToFile(string json, string filePath)
        {
            File.WriteAllText(filePath, json);
        }


        public static string LoadJsonFromFile(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}