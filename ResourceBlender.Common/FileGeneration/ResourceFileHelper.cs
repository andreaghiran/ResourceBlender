namespace ResourceBlender.Common.FileGeneration
{
  public static class ResourceFileHelper
  {
    public static string GetLanguageCode(string resourceFileName)
    {
      string[] resourceFileNameArray = resourceFileName.Split('.');

      if(resourceFileNameArray.Length != 3)
      {
        return "en";
      }
      else
      {
        return resourceFileNameArray[1];
      }
    }
  }
}
