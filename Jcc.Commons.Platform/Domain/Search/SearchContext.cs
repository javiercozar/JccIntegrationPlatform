namespace Jcc.Commons.Platform.Domain.Search;

public sealed class SearchContext : IValidateSupplierSearchRequest, IValidateSupplierSearchResponse
{
    public bool ValidRequest { get; set; }
    public bool ValidResponse { get; set; }
}