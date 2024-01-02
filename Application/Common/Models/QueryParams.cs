namespace Application.Common.Models;

public class QueryParams : PaginationParams
{
    public string SearchTerm { get; set; }
    public string Sort { get; set; }
}