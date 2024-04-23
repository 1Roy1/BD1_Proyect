using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    class EncryptMD5
    {
        public string Encrypt (string Mensaje)
        {
            string hash = "coding";
            byte[] data = UTF8Encoding.UTF8.GetBytes (Mensaje);
            
            MD5 md5 = MD5.Create ();
            TripleDES tripledes = TripleDES.Create();

            tripledes.Key = md5.ComputeHash (UTF8Encoding.UTF8.GetBytes(hash));
            tripledes.Mode = CipherMode.ECB;

            ICryptoTransform transform = tripledes.CreateEncryptor();
            byte[] result = transform.TransformFinalBlock (data, 0, data.Length);

            return Convert.ToBase64String (result);
        }

        public string Decrypt (string MensajeEn)
        {
            string hash = "coding";
            byte[] data = Convert.FromBase64String(MensajeEn);

            MD5 md5 = MD5.Create();
            TripleDES tripledes = TripleDES.Create();

            tripledes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            tripledes.Mode = CipherMode.ECB;

            ICryptoTransform transform = tripledes.CreateDecryptor();
            byte[] result = transform.TransformFinalBlock(data, 0, data.Length);

            return UTF8Encoding.UTF8.GetString(result);
        }
    }
}
