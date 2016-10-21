using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace Utils
{
    public partial class ObsCrypt
    {
        public ObsCrypt()
		{
		}

        //public static string CreateMD5(string toEncode)
        //{
        //    MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

        //    byte[] input = ASCIIEncoding.ASCII.GetBytes(toEncode);
        //    byte[] output = md5.ComputeHash(input);	
		
        //    string hexOutput = BitConverter.ToString( output ).Replace("-", "");
        //    return hexOutput.ToLower();			
        //}

		//encrypt fun takes the string to be encrypted and passward as a parameter and
		//return encrypted string 
		public static string Encrypt(string   p_plainText, string   p_passPhrase )
		{
			string   t_salt					= "ertaehtxof";        // can be any string
			string   t_hashAlgorithm		= "SHA1";             // can be "MD5"
			int      t_passwordIterations	= 2;                  // can be any number
			string   t_initVector			= "51cko9wnc5F6gbc43"; // must be 16 bytes
			int      t_keySize				= 256;                // can be 192 or 128

			byte[] initVectorBytes = Encoding.ASCII.GetBytes(t_initVector);
			byte[] saltBytes  = Encoding.ASCII.GetBytes(t_salt);                 
      
			byte[] plainTextBytes  = Encoding.UTF8.GetBytes(p_plainText);

			PasswordDeriveBytes password = new PasswordDeriveBytes(
				p_passPhrase, 
				saltBytes, 
				t_hashAlgorithm, 
				t_passwordIterations);
      
			byte[] keyBytes = password.GetBytes(t_keySize / 8);
               
			RijndaelManaged symmetricKey = new RijndaelManaged();
      
			symmetricKey.Mode = CipherMode.CBC;    
   

       
			ICryptoTransform encryptor = symmetricKey.CreateEncryptor(
				keyBytes, 
				initVectorBytes);
          
			MemoryStream memoryStream = new MemoryStream();   
    
			CryptoStream cryptoStream = new CryptoStream(memoryStream, 
				encryptor,
				CryptoStreamMode.Write);
       
			cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
               
       
			cryptoStream.FlushFinalBlock();

       
			byte[] cipherTextBytes = memoryStream.ToArray();
               
     
			memoryStream.Close();
			cryptoStream.Close();
       
 
			string t_cipherText = Convert.ToBase64String(cipherTextBytes);
       
      
			return t_cipherText;
		}
   
 
		public static string Decrypt(string   p_cipherText, string   p_passPhrase )
		{
			string   t_salt					= "ertaehtxof";        // can be any string
			string   t_hashAlgorithm		= "SHA1";             // can be "MD5"
			int      t_passwordIterations	= 2;                  // can be any number
			string   t_initVector			= "51cko9wnc5F6gbc43"; // must be 16 bytes
			int      t_keySize				= 256;                // can be 192 or 128

			byte[] initVectorBytes = Encoding.ASCII.GetBytes(t_initVector);
			byte[] saltBytes  = Encoding.ASCII.GetBytes(t_salt);       
       
			byte[] cipherTextBytes = Convert.FromBase64String(p_cipherText);
       
			PasswordDeriveBytes password = new PasswordDeriveBytes(
				p_passPhrase, 
				saltBytes, 
				t_hashAlgorithm,  t_passwordIterations);

     
			byte[] keyBytes = password.GetBytes(t_keySize / 8);
     
			RijndaelManaged    symmetricKey = new RijndaelManaged();
       
       
			symmetricKey.Mode = CipherMode.CBC;
       
       
			ICryptoTransform decryptor = symmetricKey.CreateDecryptor(
				keyBytes, 
				initVectorBytes);
      
			MemoryStream  memoryStream = new MemoryStream(cipherTextBytes);
               
      
			CryptoStream  cryptoStream = new CryptoStream(memoryStream, 
				decryptor,
				CryptoStreamMode.Read);

       
			byte[] plainTextBytes = new byte[cipherTextBytes.Length];
       
      
			int decryptedByteCount = cryptoStream.Read(plainTextBytes, 
				0, 
				plainTextBytes.Length);
      
			memoryStream.Close();
			cryptoStream.Close();
       
       
			string t_plainText = Encoding.UTF8.GetString(plainTextBytes, 
				0, 
				decryptedByteCount);
       
      
			return t_plainText;
		}
	}
}
