using System;
using System.Web;
using Es.Udc.DotNet.MiniPortal.Web.Properties;
namespace Es.Udc.DotNet.MiniPortal.Web.HTTP.Util
{
    public class Pagination
    {
        private static string pattern = "<li {0} ><a href='{1}' >{2}</a></li>";//0->url, 1->class for active, 2->text
        private static int NUM_PAGINATION = Settings.Default.NumPagination;


        public static string showNextPrevious(int start, int size, int total, string url, HttpResponse Response)
        {
            if (size >= total)
            {
                return "";
            }
            

            string tmp;

            int index = (start % size != 0) ? start / size + 1 : start / size; // pagina en la que estamos

            int cnt = (index - NUM_PAGINATION / 2 < 0) ? 0 : index - NUM_PAGINATION / 2; // donde empezamos

            string active;
            //Se muestra prev
            active = ((start - size) >= 0) ? "" : "class='disabled'";
            tmp = String.Format(pattern,
                                active,
                                Response.ApplyAppPathModifier(String.Format(url, start - size)),
                                "&laquo;");
            //if (cnt > 0)
            //    tmp = String.Format(pattern,
            //                    "class='disabled'",
            //                    "#",
            //                    "...");
            //bool stop = false;
            for (int i = 0; i < NUM_PAGINATION; i++)
            {
                if ((cnt + i) * size >= total)
                    //{
                    //    stop = true;
                    break;
                //}
                active = ((cnt + i) == index) ? "class='active'" : "";
                tmp += String.Format(pattern,
                                     active,
                                     Response.ApplyAppPathModifier(String.Format(url, ((cnt + i) * size))),
                                     "" + (cnt + i + 1));
            }

            //if (!stop)
            //    tmp = String.Format(pattern,
            //                    "class='disabled'",
            //                    "#",
            //                    "...");

            //Se muestra next
            active = (total > (start + size)) ? "" : "class='disabled'";
            tmp += String.Format(pattern,
                                active,
                                Response.ApplyAppPathModifier(String.Format(url, start + size)),
                                "&raquo;");

            return "<div class='pagination'><ul>" + tmp + "</ul></div>";
        }
    }
}