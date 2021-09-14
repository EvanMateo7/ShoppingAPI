

using System;

namespace ShoppingAPI.Domain.Exceptions
{
  public class DomainException : Exception
  {
    public string DomainExceptionType { get; init; }
    public DomainException(string domainExceptionType) : base(domainExceptionType)
    {
      DomainExceptionType = domainExceptionType;
    }
  }

  public static class DomainExceptionTypes
  {
    public static string ProductNegativeQuantity => "product_negative_quantity";
    public static string OrderProductInvalidQuantity => "order_product_invalid_quantity";
  }
}
