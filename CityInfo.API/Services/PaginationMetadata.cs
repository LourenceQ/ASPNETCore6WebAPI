namespace CityInfo.API.Services;

public class PaginationMetadata
{
    public PaginationMetadata(int totalItemCount
        , int pageSize
        , int currrntPage)
    {
        TotalItemCount = totalItemCount;
        TotalPageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);
        PageSize = pageSize;
        CurrrntPage = currrntPage;
    }

    public int TotalItemCount { get; set; }
    public int TotalPageCount { get; set; }
    public int PageSize { get; set; }
    public int CurrrntPage { get; set; }
}
