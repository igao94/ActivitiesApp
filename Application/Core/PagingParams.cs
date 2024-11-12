namespace Application.Core;

public class PagingParams
{
    public const int MaxPageSize = 50;
    public const int MinPageNumber = 1;

    private int _pageSize = 10;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    private int _pageNumber = 1;

    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = (value < MinPageNumber) ? MinPageNumber : value;
    }
}
