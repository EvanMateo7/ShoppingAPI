
namespace ShoppingAPI.Data.Util
{
  public class PaginationQuery
  {
    private const int defaultPageNumer = 1;
    private const int defaultPageSize = 15;

    private int _pageNumber;
    public int PageNumber { 
      get => _pageNumber;
      set {
        _pageNumber = (value >= 1) ? value : defaultPageNumer;
      }
    }
    
    private int _pageSize;
    public int PageSize { 
      get => _pageSize;
      set {
        _pageSize = (value >= 2 && value <= 50) ? value : defaultPageSize;
      }
    }

    public PaginationQuery()
    {
      PageNumber = defaultPageNumer;
      PageSize = defaultPageSize;
    }
  }
}
