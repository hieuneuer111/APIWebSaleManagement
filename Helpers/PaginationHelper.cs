using WebAPISalesManagement.Services.URLPagination;
using WebAPISalesManagement.Swagger;

namespace WebAPISalesManagement.Helpers
{
    public class PaginationHelper
    {
        public static PageResponse<List<T>> CreatePagedReponse<T>(List<T> pagedData, PaginationFilterHelper validFilter, int totalRecords, IURLService uriService, string route)
        {
            var respose = new PageResponse<List<T>>(pagedData, validFilter.PageNumber, validFilter.PageSize);
            var totalPages = ((double)totalRecords / (double)validFilter.PageSize);
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
            respose.NextPage =
                validFilter.PageNumber >= 1 && validFilter.PageNumber < roundedTotalPages
                ? uriService.GetPageUri(new PaginationFilterHelper(validFilter.PageNumber + 1, validFilter.PageSize), route)
                : null;
            respose.PreviousPage =
                validFilter.PageNumber - 1 >= 1 && validFilter.PageNumber <= roundedTotalPages
                ? uriService.GetPageUri(new PaginationFilterHelper(validFilter.PageNumber - 1, validFilter.PageSize), route)
                : null;
            respose.FirstPage = uriService.GetPageUri(new PaginationFilterHelper(1, validFilter.PageSize), route);
            respose.LastPage = uriService.GetPageUri(new PaginationFilterHelper(roundedTotalPages, validFilter.PageSize), route);
            respose.TotalPages = roundedTotalPages;
            respose.TotalRecords = totalRecords;
            return respose;
        }
        public static int GetMaxPage(int total, int PageSize)
        {
            if (total <= PageSize)
            {
                return 1;
            }
            else
            {
                return Convert.ToInt32(Math.Ceiling(total / Convert.ToDouble(PageSize)));
            }
        }
        public static ModelDataPageResponse<T> createPageDataResponse<T>(int total, int PageNumber, int PageSize, bool getLastPage)
        {
            ModelDataPageResponse<T> result = new ModelDataPageResponse<T>();
            result.totalItem = total;
            result.pageSize = PageSize;
            result.maxPage = GetMaxPage(result.totalItem, result.pageSize);
            result.currentPage = (PageNumber > result.maxPage) || getLastPage ? result.maxPage : PageNumber;
            return result;
        }
        public static List<Response> Paging<Response, ModelDataPage>(List<Response> items, ModelDataPageResponse<ModelDataPage> result, bool isPaging)
        {
            items = isPaging ? items.Skip((result.currentPage - 1) * result.pageSize).Take(result.pageSize).ToList() : items;
            return items;
        }
    }
}
