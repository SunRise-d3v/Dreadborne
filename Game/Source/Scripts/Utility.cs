namespace Dreadborne;

internal class Utility
{
    private const string XML_FILE_PATH = "Resources/XMLs/";

    public static string UploadXML(string file) => XML_FILE_PATH + file;
}