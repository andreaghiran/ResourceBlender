using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceBlender.Common.Exceptions
{
  public class ResourceDoesNotExistException: Exception
  {
    public ResourceDoesNotExistException()
    {

    }

    public ResourceDoesNotExistException(string message)
      :base(message)
    {

    }

    public ResourceDoesNotExistException(string message, Exception innerException)
      :base(message, innerException)
    {

    }
  }
}
