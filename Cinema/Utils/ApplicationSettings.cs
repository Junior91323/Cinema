using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Cinema.Utils
{
    public static class ApplicationSettings
    {
        public static string MovieImageSize
        {
            get
            {
                if (ConfigurationManager.AppSettings["MovieImageSize"] == null)
                    return "250*200";

                return ConfigurationManager.AppSettings["MovieImageSize"].ToString();
            }
        }
        public static int MoviesPageSize
        {
            get
            {
                int res = 0;
                if (ConfigurationManager.AppSettings["MoviesPageSize"] == null)
                    return 4;

                Int32.TryParse(ConfigurationManager.AppSettings["MoviesPageSize"].ToString(), out res);

                return res == 0 ? 4 : res;
            }
        }
        public static int MoviesPagerCount
        {
            get
            {
                int res = 0;
                if (ConfigurationManager.AppSettings["MoviesPagerCount"] == null)
                    return 5;

                Int32.TryParse(ConfigurationManager.AppSettings["MoviesPagerCount"].ToString(), out res);

                return res == 0 ?5 : res;
            }
        }
    }
}