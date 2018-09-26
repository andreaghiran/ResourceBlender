using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ResourceBlender.Common.ViewModels
{
  public class ResourceViewModel
  {
    public int Id { get; set; }

    [DisplayName("Resource")]
    [Required]
    public string ResourceString { get; set; }

    [DisplayName("Romanian Translation")]
    [Required]
    public string RomanianTranslation { get; set; }

    [DisplayName("English Translation")]
    [Required]
    public string EnglishTranslation { get; set; }
  }
}
