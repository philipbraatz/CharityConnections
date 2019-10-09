using System.Web.Mvc;
using Vertex.Web.Framework.Utilities;

namespace Vertex.Web.Framework.UI
{
    public class HeaderBuilder : ViewComponentBuilderBase<Header, HeaderBuilder>
    {
        public HeaderBuilder(HtmlHelper htmlHelper, Header model)
            : base(htmlHelper, model)
        {
            this.Component.ViewName = string.Format("{0}/Header", this.Component.GetViewName());
        }

        public HeaderBuilder Nav(Nav value)
        {
            Component.Nav = value;
            return this;
        }

        public HeaderBuilder Transparency(HeaderTransparency transparency)
        {
            Component.Transparency = transparency;
            return this;
        }

        public HeaderBuilder StickyType(HeaderStickyType stickyType)
        {
            Component.StickyType = stickyType;
            return this;
        }

        public HeaderBuilder BorderBottom(HeaderBorderBottom borderBottom)
        {
            Component.BorderBottom = borderBottom;
            return this;
        }

        public HeaderBuilder ShowLogo(bool value)
        {
            Component.ShowLogo = value;
            return this;
        }

        public HeaderBuilder ShowTopbar(bool value)
        {
            Component.ShowTopbar = value;
            return this;
        }

        public HeaderBuilder TextColor(BootstrapColor color)
        {
            Component.Nav.IsLightText = true;
            Component.Topbar.IsLightText = true;
            Component.Logo.WhiteLogoOnSticky = true;
            return this;
        }

        public HeaderBuilder TextColor(BootstrapColor color, bool whiteLogoOnSticky)
        {
            Component.Nav.IsLightText = true;
            Component.Topbar.IsLightText = true;
            Component.Logo.WhiteLogoOnSticky = whiteLogoOnSticky;
            return this;
        }

        public HeaderBuilder IsFullWidth(bool value)
        {
            Component.IsFullWidth = value;
            return this;
        }

        public HeaderBuilder IsStickyEnabled(bool value)
        {
            Component.IsStickyEnabled = value;
            return this;
        }

        public HeaderBuilder IsStickyEnableOnBoxed(bool value)
        {
            Component.IsStickyEnableOnBoxed = value;
            return this;
        }

        public HeaderBuilder ShowSearchIcon(bool value)
        {
            Component.ShowSearchIcon = value;
            return this;
        }

        public HeaderBuilder IsStickyEnableOnMobile(bool value)
        {
            Component.IsStickyEnableOnMobile = value;
            return this;
        }
    }
}