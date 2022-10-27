using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CRME.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Net.Mail;
using System.Configuration;
using System.Net;
using System.IO;

namespace CRME.Helpers
{
    public class HelpersController
    {

        public string RemoveDiacritics(string text)
        {
            string formD = text.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            foreach (char ch in formD)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(ch);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

    }
}