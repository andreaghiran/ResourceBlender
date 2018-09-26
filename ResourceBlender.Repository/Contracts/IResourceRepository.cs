using ResourceBlender.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceBlender.Repository.Contracts
{
  public interface IResourceRepository
  {
    List<Resource> GetAllResources();
    void AddResource(Resource resource);
    Resource GetResourceById(int id);
    void EditResource(Resource resource);
    void DeleteResource(int id);
  }
}
