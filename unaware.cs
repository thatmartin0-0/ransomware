using System;
using System.IO;
using System.Runtime.InteropServices;

class DecryptionTool
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
                // Decrypt the file using AES
                byte[] bytes = File.ReadAllBytes(file);
                byte[] decryptedBytes = Decrypt(bytes, "mysecretpassword");

                // Save the decrypted file
                File.WriteAllBytes(file, decryptedBytes);

                Console.WriteLine("File " + file + " has been decrypted.");
            }
        }

        Console.Write("Press any key to exit...");
        Console.ReadKey();
    }

    public static byte[] Decrypt(byte[] bytes, string password)
 {
        using (Aes aes = Aes.Create())
        {
            aes.Key = System.Text.Encoding.UTF8.GetBytes(password);
            aes.IV = new byte[16];

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream ms = new MemoryStream(bytes))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    return cs.ReadBytes(cs.Read(0x10000, 0, 4096));
                }
            }
        }
    }
}

