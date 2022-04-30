using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constants
{
    public static class Messages
    {
        public static string NameMinLength = "İsim en az 2 karakter olmalıdır.";
        public static string SurnameMinLength = "Soyisim en az 2 karakter olmalıdır.";
        public static string UsernameMinLength = "Kullanıcı adı en az 3 karakter olmalıdır.";
        public static string AuthorizationDenied = "Yetki Hatası.";
    }
}
