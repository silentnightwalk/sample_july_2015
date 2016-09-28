using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MetrologyAdmin
{
    public class PasswordGenerator
    {
        public static string GeneratePassword(
            int passwordLength = 6
            //,string characterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_"
            )
        {
            var result = "";
            using (RNGCryptoServiceProvider cryptRNG = new RNGCryptoServiceProvider())
            {
                byte[] tokenBuffer = new byte[passwordLength*2];
                cryptRNG.GetBytes(tokenBuffer);
                var charSequence = Convert
                    .ToBase64String(tokenBuffer)
                    .ToUpper()
                    .Except(
                        new char[] { 
                            '!','@','#','$','%','^','&','*','(',')','-' 
                            ,'+','=','`','~','"','\'','\\','|','/','.',','
                            ,'?','<','>',':',';','{','}','[',']'
                            //,'','','','','','',''
                            }
                        );
                Array.ForEach<char>(
                    charSequence.Take(passwordLength).ToArray(),
                    c=>result=result + c.ToString()
                    );
            }
            return result;
        }
    }
}
