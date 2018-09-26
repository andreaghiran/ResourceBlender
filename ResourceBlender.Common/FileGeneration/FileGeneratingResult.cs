using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace ResourceBlender.Common.FileGeneration
{
  public class FileGeneratingResult: FileResult
  {
    /// <summary>
    /// The delegate that will generate the file content.
    /// </summary>
    private readonly Action<Stream> content;

    private readonly bool bufferOutput;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileGeneratingResult" /> class.
    /// </summary>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="contentType">Type of the content.</param>
    /// <param name="content">Delegate with Stream parameter. This is the stream to which content should be written.</param>
    /// <param name="bufferOutput">use output buffering. Set to false for large files to prevent OutOfMemoryException.</param>
    public FileGeneratingResult(string fileName, string contentType, Action<Stream> content, bool bufferOutput = true)
        : base(contentType)
    {
      if (content == null)
        throw new ArgumentNullException("content");

      this.content = content;
      this.bufferOutput = bufferOutput;
      FileDownloadName = fileName;
    }

    /// <summary>
    /// Writes the file to the response.
    /// </summary>
    /// <param name="response">The response object.</param>
    protected override void WriteFile(HttpResponseBase response)
    {
      response.Buffer = bufferOutput;
      content(response.OutputStream);
    }
  }
}
