using System.Collections.Generic;

namespace my_shoppinglist_api.Models
{
  public class ListResponse<T>
  {
    public IEnumerable<T> List { get; set; }
    public int Count { get; set; }
  }
}