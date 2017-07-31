using System.Web.Mvc;

namespace ProjectyMcProjectface.Mvc
{
    public class FilterConfig
    {
        public static void RegisterglobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthorizeAttribute());
        }
    }
}