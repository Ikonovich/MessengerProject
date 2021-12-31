using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Messenger_Client
{
    // This class is used for all security-related tasks.
    // It uses the RandomNumberGenerator class, which is cryptographically sound.
    static class Security
    {
        // The characters available for randomly generated codes.
        private static readonly char[] AvailChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

        public static string GenerateCode(int length)
        {
            string code = "";

            for (int i = 0; i < length; i++)
            {
                int randInt = RandomNumberGenerator.GetInt32(AvailChars.Length);
                code += AvailChars[randInt];
            }
            return code;

        }


    }
}
