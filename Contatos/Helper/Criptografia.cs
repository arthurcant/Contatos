using System.Security.Cryptography;
using System.Text;

namespace Contatos.Helper
{
    public static class Criptografia
    {
        public static string Criptografar(this string objeto)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(objeto);
                byte[] hash = sha256.ComputeHash(bytes);
                StringBuilder stringBuilder = new StringBuilder();
                foreach (byte b in hash)
                {
                    stringBuilder.Append(b.ToString("x2"));
                }
                return stringBuilder.ToString();
            }
        }
    }
}
