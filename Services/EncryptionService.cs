using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Hosting;

namespace ApplicationSecurity.Services
{
    public class EncryptionService
    {
        private readonly Aes _aes = Aes.Create();
        
        public EncryptionService(IWebHostEnvironment hostingEnvironment)
        {
            var keyRingDirectory = Path.Combine(hostingEnvironment.ContentRootPath, "Encryption");
            var keyFileName = Path.Combine(keyRingDirectory, "encryption.key");

            if (File.Exists(keyFileName))
            {
                _aes.Key = Encoding.ASCII.GetBytes(string.Join("", File.ReadAllLines(keyFileName)));
            }
            else
            {
                var key = Encoding.ASCII.GetString(_aes.Key);
                using var file = File.CreateText(keyFileName);
                file.WriteLine(key);
            }
        }
        
        public string Encrypt(string plaintextString)
        {
            _aes.GenerateIV(); // generate new IV each encryption
            var ivString = Convert.ToBase64String(_aes.IV);
            
            var encryptor = _aes.CreateEncryptor();

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plaintextString);
                    }
                    return ivString + ";" + Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public string Decrypt(string encryptedString)
        {
            var splitString = encryptedString.Split(";");
            var ivBytes = Convert.FromBase64String(splitString[0]);
            var encryptedBytes = Convert.FromBase64String(splitString[1]);

            _aes.IV = ivBytes;
            var decryptor = _aes.CreateDecryptor(_aes.Key, ivBytes);
            
            using var msDecrypt = new MemoryStream(encryptedBytes);
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);
            
            return srDecrypt.ReadToEnd();
        }
    }
}