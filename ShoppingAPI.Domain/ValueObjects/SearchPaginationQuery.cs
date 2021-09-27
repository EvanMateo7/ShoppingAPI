
namespace ShoppingAPI.Domain.ValueObjects
{
  public class SearchPaginationQuery : PaginationQuery
  {
    private string _searchTerm;
    public string SearchTerm
    {
      get => _searchTerm;
      set
      {
        _searchTerm = (value != null ? value.Trim() : "");
      }
    }

    public SearchPaginationQuery() : base()
    {
      SearchTerm = "";
    }
  }
}
