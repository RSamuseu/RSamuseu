using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AdvertisingAgency.Helpers
{
    public class FilterTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "input";
            output.Attributes.Add("type", "submit");
            output.Attributes.Add("value", "Filter");
        }
    }
}
