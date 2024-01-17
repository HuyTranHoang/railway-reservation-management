namespace Application.Common.Models.QueryParams;

public class QueryParams : PaginationParams
{
    public string SearchTerm { get; set; }
    public string Sort { get; set; }
}