using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Es.Udc.DotNet.MiniPortal.Model.CommentService.Util
{

    
    public class StrNormalize
    {
        private static Regex a_replace;//, A_replace;
        private static Regex e_replace;//, E_replace;
        private static Regex i_replace;//, I_replace;
        private static Regex o_replace;//, O_replace;
        private static Regex u_replace;//, U_replace;

        static StrNormalize()
        {
            a_replace = new Regex("[á|à|ä|â|ã]", RegexOptions.Compiled);
            e_replace = new Regex("[é|è|ë|ê]", RegexOptions.Compiled);
            i_replace = new Regex("[í|ì|ï|î]", RegexOptions.Compiled);
            o_replace = new Regex("[ó|ò|ö|ô|õ]", RegexOptions.Compiled);
            u_replace = new Regex("[ú|ù|ü|û]", RegexOptions.Compiled);

            //A_replace = new Regex("[Á|À|Ä|Â|Ã]", RegexOptions.Compiled);
            //E_replace = new Regex("[É|È|Ë|Ê]", RegexOptions.Compiled);
            //I_replace = new Regex("[Í|Ì|Ï|Î]", RegexOptions.Compiled);
            //O_replace = new Regex("[Ó|Ò|Ö|Ô|Õ]", RegexOptions.Compiled);
            //U_replace = new Regex("[Ú|Ù|Ü|Û]", RegexOptions.Compiled);
        }

        private static string removeAccents(string str)
        {
            if (String.IsNullOrWhiteSpace(str))
                return str;

            str = a_replace.Replace(str, "a");
            str = e_replace.Replace(str, "e");
            str = i_replace.Replace(str, "i");
            str = o_replace.Replace(str, "o");
            str = u_replace.Replace(str, "u");

            //str = A_replace.Replace(str, "A");
            //str = E_replace.Replace(str, "E");
            //str = I_replace.Replace(str, "I");
            //str = O_replace.Replace(str, "O");
            //str = U_replace.Replace(str, "U");

            return str.Trim();
        }

        public static string strNormalize(string str)
        {
            return removeAccents(str.ToLower());
        }

    }
}
