using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortal.CoreBusiness.Services
{
    public class PasswordHashService : IPasswordHashService
    {
        public string GeneratePasswordHash(string password)
        {
            MD5 MD5Generator = MD5.Create();
            // Convert the input string to a byte array and compute the hash.
            byte[] data = MD5Generator.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder builder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                builder.Append(data[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
