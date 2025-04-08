using WebAPISalesManagement.Helpers;
namespace WebAPISalesManagement.Services.URLPagination
{
    public interface IURLService
    {
        public Uri GetPageUri(PaginationFilterHelper filter, string route);
    }
}
