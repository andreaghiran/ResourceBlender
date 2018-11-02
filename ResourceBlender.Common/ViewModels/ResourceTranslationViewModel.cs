using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceBlender.Common.ViewModels
{
  public class ResourceTranslationViewModel
  {
    public ResourceTranslationViewModel()
    {
      Translations = new List<ResourceTranslationViewModelElement>();
      AvailableLanguages = new List<string>();
    }

    public int ResourceId { get; set; }
    public string ResourceString { get; set; }
    public List<String> AvailableLanguages { get; set; }
    public List<ResourceTranslationViewModelElement> Translations { get; set; }
  }

  public class ResourceTranslationViewModelElement
  {
    public int TranslationId { get; set; }

    [Required]
    public string TranslationValue { get; set; }

    public string LanguageValue { get; set; }
  }
}
