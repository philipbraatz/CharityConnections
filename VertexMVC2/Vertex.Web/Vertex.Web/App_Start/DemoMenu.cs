using System.Web;
using System.Web.Mvc;
using Vertex.Web.Framework.UI;

namespace Vertex.Web
{
    public partial class DemoMenu
    {
        public static Nav GetDemoMenu()
        {
            var nav = new Nav();
            var url = new UrlHelper(HttpContext.Current.Request.RequestContext);

            var homes = new Dropdown("Home")
            {
                Dropdowns = {
                   new Dropdown("Home - Variation 1", url.Action("Index", "Home")),
                   new Dropdown("Home - Variation 2", url.Action("Index2", "Home")),
                   new Dropdown("Home - Variation 3", url.Action("Index3", "Home")),
                   new Dropdown("Home - Variation 4", url.Action("Index4", "Home")),
                   new Dropdown("Home - Variation 5", url.Action("Index5", "Home"))
                }
            };
            var pages = new Dropdown("Pages")
            {
                Dropdowns = {
                   //new Dropdown("Features"),
                   //new Dropdown(DropdownType.Divider),
                   new Dropdown("About Us", url.Action("About", "Page")),
                   new Dropdown("Contact Us", url.Action("Contact", "Page")),
                   new Dropdown("Team", url.Action("Team", "Page")),
                   new Dropdown("Jobs", url.Action("Jobs", "Page")),
                   new Dropdown("Page 404", url.Action("Category", "Blog")),
                   //new Dropdown("Team", ""),
                   //new Dropdown("Services", ""),
                   //new Dropdown("Careers", ""),
                   //new Dropdown("Job Page", ""),
                   //new Dropdown("Account", "") {
                   //    DropdownList = {
                   //       new Dropdown("Login", ""),
                   //       new Dropdown("Register", ""),
                   //       new Dropdown("Recovery", "")
                   //    }
                   //},
                   //new Dropdown("Help Page", ""),
                   //new Dropdown("Pricing Page", ""),
                   //new Dropdown(DropdownType.Divider),
                   //new Dropdown("Extra") {
                   //    DropdownList = {
                   //       new Dropdown("FAQ", ""),
                   //       new Dropdown("404 Error", "")
                   //    }
                   //},
                }
            };
            var blog = new Dropdown("Blog")
            {
                Dropdowns = {
                   new Dropdown("Card") {
                       Dropdowns = {
                          new Dropdown("Left Sidebar", url.Action("CardLeftSidebar", "Blog")),
                          new Dropdown("Right Sidebar", url.Action("CardRightSidebar", "Blog")),
                          new Dropdown("Full Width", url.Action("CardFullWidth", "Blog"))
                       }
                   },
                   new Dropdown("Grid") {
                       Dropdowns = {
                          new Dropdown("Left Sidebar", url.Action("GridLeftSidebar", "Blog")),
                          new Dropdown("Right Sidebar", url.Action("GridRightSidebar", "Blog")),
                          new Dropdown("Full Width", url.Action("GridFullWidth", "Blog"))
                       }
                   },
                   new Dropdown("List") {
                       Dropdowns = {
                          new Dropdown("Left Sidebar", url.Action("ListLeftSidebar", "Blog")),
                          new Dropdown("Right Sidebar", url.Action("ListRightSidebar", "Blog")),
                          new Dropdown("Full Width", url.Action("ListFullWidth", "Blog"))
                       }
                   },
                   new Dropdown("Single") {
                       Dropdowns = {
                          new Dropdown("Video", "/this-is-vide-post-example-with-default-view"),
                          new Dropdown("Audio", "/audio-post-example-with-default-view"),
                          new Dropdown("Quote", "/this-is-quote-post-with-center-view"),
                          new Dropdown(DropdownType.Divider),
                          new Dropdown("Center View", "/mobile-friendly-design"),
                          new Dropdown("Standard View", "/new-es2019-features-every-javascript-developer-should-know"),
                          new Dropdown("Fullwidth View", "/whats-new-in-life"),
                          new Dropdown("Fullwidth No Sidebar", "/10-best-games-for-console-2019")
                       }
                   }
                }
            };
            var portfolio = new Dropdown("Portfolio")
            {
                Dropdowns = {
                   //new Dropdown("Features"),
                   //new Dropdown(DropdownType.Divider),
                   new Dropdown("Agency", url.Action("Agency", "Portfolio")),
                   new Dropdown("Awesome Work", url.Action("AwesomeWork", "Portfolio")),
                   new Dropdown("Masonry", url.Action("Masonry", "Portfolio")),
                   //new Dropdown(DropdownType.Divider),
                   //new Dropdown("Single") {
                   //    DropdownList = {
                   //       new Dropdown("Default", "")
                   //    }
                   //},
                }
            };
            var features = new Dropdown("Features", url.Action("Index", "Features"))
            {
                Dropdowns = {
                    new Dropdown("<strong>Features</strong>", url.Action("Index", "Features")),
                    new Dropdown(DropdownType.Divider),
                    new Dropdown("Blog Post", url.Action("BlogPost", "Features")),
                    new Dropdown("Portfolio", url.Action("Portfolio", "Features")),
                    new Dropdown("Team Member", url.Action("TeamMember", "Features")),
                    new Dropdown("Clients", url.Action("Clients", "Features")),
                    new Dropdown("Testimonials", url.Action("Testimonials", "Features")),
                    new Dropdown("Jobs", url.Action("Jobs", "Features")),
                }
            };

            var doc = new Dropdown("Docs", "/Documentation/introduction.html");

            nav.Dropdowns.Add(homes);
            nav.Dropdowns.Add(pages);
            nav.Dropdowns.Add(blog);
            nav.Dropdowns.Add(portfolio); 
            nav.Dropdowns.Add(features);
            nav.Dropdowns.Add(doc);

            return nav;
        }
    }
}