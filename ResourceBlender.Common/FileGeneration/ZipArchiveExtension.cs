using System.IO;
using System.IO.Compression;

namespace ResourceBlender.Common.FileGeneration
{
  public static class ZipArchiveExtension
  {
    public static void ExtractToDirectory2(this ZipArchive archive, string destinationDirectory, bool overwrite)
    {
      if (!overwrite)
      {
        //archive.ExtractToDirectory(destinationDirectory);

      }
    }
  }
}
