
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Localization;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using System.Text.RegularExpressions;
using Connect.Conference.Core.Models.Comments;
using DotNetNuke.Collections;

namespace Connect.DNN.Modules.Conference.Common
{
    public static class Globals
    {

        public const string SharedResourceFileName = "~/DesktopModules/MVC/Connect/Conference/App_LocalResources/SharedResources.resx";

        public static string MinutesToString(this int minutes)
        {
            return ((int)minutes / 60).ToString() + ":" + (minutes % 60).ToString("00");
        }

        public static List<SelectListItem> StatusDropdownList(string locale)
        {
            return CBO.GetCachedObject<List<SelectListItem>>(new CacheItemArgs(string.Format("ConferenceStatusDropdownList-{0}", locale), 20, System.Web.Caching.CacheItemPriority.Default, locale), GetStatusDropdownList);
        }
        private static List<SelectListItem> GetStatusDropdownList(CacheItemArgs args)
        {
            var locale = (string)args.ParamList[0];
            var res = new List<SelectListItem>();
            foreach (var v in System.Enum.GetValues(typeof(Connect.Conference.Core.Common.SessionStatus)))
            {
                res.Add(new SelectListItem
                {
                    Text = Localization.GetString(v.ToString(), SharedResourceFileName),
                    Value = ((int)v).ToString()
                });
            }
            return res.OrderBy(i => i.Value).ToList();
        }

        public static string RemoveIllegalCharacters(this string filename)
        {
            return Regex.Replace(filename, "[\\:;,\\?]", "");
        }

        public static IEnumerable<Comment> FillStampLines(this IPagedList<Comment> commentList)
        {
            var res = new List<Comment>();
            foreach (var comment in commentList)
            {
                res.Add(comment.FillStampLine());
            }
            return res;
        }

        public static Comment FillStampLine(this Comment comment)
        {
            var res = comment;
            try
            {
                res.StampLine = string.Format(Localization.GetString("StampLine.Text", SharedResourceFileName), comment.Datime, comment.DisplayName);
            }
            catch { }
            return res;
        }

        public static string ToStatusString(this Connect.Conference.Core.Common.SessionStatus status)
        {
            return Localization.GetString(status.ToString(), SharedResourceFileName);
        }
    }
}