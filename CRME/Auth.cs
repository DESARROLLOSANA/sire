using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRME.Models;

namespace CRME
{
    public class Auth
    {
        private const string UserKey = "CRME.Auth.UserKey";

        public static cat_sistemas Usuario
        {
            get
            {
                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    return null;
                }
                var usuario = HttpContext.Current.Items[UserKey] as cat_sistemas;

                if (usuario == null)
                {
                    SIRE_Context db = new SIRE_Context();
                    usuario = db.cat_sistemas.FirstOrDefault(x => x.username == HttpContext.Current.User.Identity.Name);

                    if (usuario == null)
                    {
                        return null;
                    }
                    HttpContext.Current.Items[UserKey] = usuario;
                }
                return usuario;
            }
        }
    }
}