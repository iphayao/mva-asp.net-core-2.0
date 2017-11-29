using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication
{
    public class RepeatTagHelper : TagHelper
    {
        public int Count { get; set; }
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            for(int i = 0; i < Count; i++)
            {
                output.Content.AppendHtml(await output.GetChildContentAsync(useCachedResult: false));
            }
        }
    }
}
