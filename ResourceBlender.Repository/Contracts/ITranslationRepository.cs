using ResourceBlender.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceBlender.Repository.Contracts
{
  public interface ITranslationRepository
  {
    List<Translation> GetTranslations();
    Translation GetTranslationById(int id);
    void UpdateTranslation(Translation updatedTranslation);
  }
}
