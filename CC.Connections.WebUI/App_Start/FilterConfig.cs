using System.Web;
using System.Web.Mvc;

namespace Doorfail.Connections.WebUI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
#pragma warning disable CA1062 // Validate arguments of public methods
            filters.Add(new HandleErrorAttribute());
#pragma warning restore CA1062 // Validate arguments of public methods
        }
    }
}
