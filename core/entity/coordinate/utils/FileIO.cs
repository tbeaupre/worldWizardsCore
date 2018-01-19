using System.IO;

namespace WorldWizards.core.entity.coordinate.utils
{
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