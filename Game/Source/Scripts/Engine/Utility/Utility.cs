namespace Dreadborne;

internal class Utility
{
    private const string XML_FILE_PATH = "Resources/XMLs/";

    /// <summary>
    ///Path = Resources/XMLs/...
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public static string UploadXML(string file) => XML_FILE_PATH + file;
}