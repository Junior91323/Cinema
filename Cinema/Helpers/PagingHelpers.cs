using Cinema.Models;
using Cinema.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Cinema.Helpers
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html,
          PageInfo pageInfo, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            if (pageInfo.TotalPages > ApplicationSettings.MoviesPagerCount)
            {
                int all = ApplicationSettings.MoviesPagerCount;
                int step = all / 2;
                if(pageInfo.PageNumber <= step)
                step = all - pageInfo.PageNumber;

                for (int i = pageInfo.PageNumber - step; i <= pageInfo.PageNumber + step; i++)
                {
                    if (i > 0)
                        result.Append(GenerateHTML(i, pageInfo.PageNumber, pageUrl));

                    if (i == pageInfo.TotalPages)
                        break;
                }
            }
            else
            {
                for (int i = 1; i <= pageInfo.TotalPages; i++)
                {
                    result.Append(GenerateHTML(i, pageInfo.PageNumber, pageUrl));
                }
            }

            return MvcHtmlString.Create(result.ToString());
        }

        private static string GenerateHTML(int i, int pageNumber, Func<int, string> func)
        {
            TagBuilder tag = new TagBuilder("a");
            tag.MergeAttribute("href", func(i));
            tag.InnerHtml = i.ToString();
            if (i == pageNumber)
            {
                tag.AddCssClass("selected");
                tag.AddCssClass("btn-primary");
            }
            tag.AddCssClass("btn btn-default");
            return tag.ToString();
        }
    }
}