using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Encoder5000
{
    class Encoder
    {

        byte[] key;
        byte[] IV;
        string username;
        byte[] encrypName;
        public Encoder(byte[] key, byte[] IV, string username)
        {
            this.key = key;
            this.IV = IV;
            this.username = username;
        }

        public void Clean()
        {
            key = null;
            IV = null;
            username = null;
            encrypName = null;
        }



        public void EncrypFile(string pathFile, ProgressBar ProBar)
        {
           
            EncryptFileByte(pathFile, ProBar);
            

        }

        public bool DecrypFile(string pathFile, ProgressBar ProBar)
        {
            
           return DecryptFileByte(pathFile, ProBar);
           
        }

        public string GetDecrypName(string pathfile)
        {
            return GetRealName(pathfile);
        }

        public string GetEncrypName(string pathfile)
        {
            return EncrypFileName(pathfile);
        }

        
        //Зашифровать файл
       private void EncryptFileByte(string filepath, ProgressBar probar)
        {
            
            string[] temp = filepath.Split('\\'); //Сплитим путь к файлу
            string tempname = temp[temp.Length - 1]; //Получаем имя файла
            string temppath = filepath.Substring(0, filepath.Length - tempname.Length);//Путь до директории файла
            string tempname2 = EncrypFileName(tempname); //Шифруем имя файла
            
            FileStream fileOut = new FileStream(filepath, FileMode.Open);
            FileStream fileIn = new FileStream(temppath+"\\" + tempname2 + ".EC5", FileMode.CreateNew);
            probar.Maximum = (int)fileOut.Length;
            

            
            //Проверяем ключи шифрования на наличие
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            using (AesManaged aesAlg = new AesManaged())
            {
                //Задаем ключи шифрования пользователя
                aesAlg.KeySize = 256;
                aesAlg.Key = key;
                aesAlg.IV = IV;
                aesAlg.Padding = PaddingMode.ISO10126;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);             
                CryptoStream csEncrypt = new CryptoStream(fileIn, encryptor, CryptoStreamMode.Write);

                byte[] bufferin = new byte[1048576];    //Буфер считывания в 1МБ            
                int readIn;
                byte[] namebyte = Encoding.Default.GetBytes(username); //Шифруем имя пользователя

                csEncrypt.Write(namebyte, 0, namebyte.Length); //Записываем в файл шифрованное имя пользователя

                csEncrypt.WriteByte((byte)tempname.Length); //Записываем в файл шифрованную длину имени файла

                csEncrypt.Write(Encoding.Default.GetBytes(tempname), 0, Encoding.Default.GetBytes(tempname).Length); //Записываем в файл шифрованное имя файла


                      
                //Шифуем сам файл
                while ((readIn = fileOut.Read(bufferin, 0, bufferin.Length)) > 0)
                {
                    Application.DoEvents();
                    csEncrypt.Write(bufferin, 0, readIn);
                    probar.Value += readIn;
                }
                csEncrypt.Close();
                fileOut.Close();
                fileIn.Close();
                
            }

        }


        //Зашифровать имя файла
        private string EncrypFileName(string name)
        {
            
            AesManaged aesAlg = new AesManaged();
            aesAlg.KeySize = 256;
            aesAlg.Key = key;
            aesAlg.IV = IV;
            aesAlg.Padding = PaddingMode.ISO10126;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            MemoryStream msEncrypt = new MemoryStream();
            CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            StreamWriter swEncrypt = new StreamWriter(csEncrypt);

            swEncrypt.Write(name); //Шифруем имя файла
            csEncrypt.Close();

            encrypName = msEncrypt.ToArray(); //Сохраняем его в виде byte[] для дальнейшего записи в файл
            string resst = "";
            foreach (byte i in encrypName)
                resst += i.ToString(); //получаем его как набор байтов в виде строки

           
            return resst;
        }

        //Получить расшифрованное имя файла
        private string GetRealName(string filepath)
        {
            string[] temp = filepath.Split('\\'); //Сплитим путь файла
            string tempname = temp[temp.Length - 1]; //Берем имя файла
            string temppath = filepath.Substring(0, filepath.Length - tempname.Length); //Берем путь до директории файла

            FileStream fileOut = new FileStream(filepath, FileMode.Open);

            //Если ключи не корректны, вернем зашифрованное имя
            if (key == null || key.Length <= 0)
            {
                fileOut.Close();
                return tempname;
            }
            if (IV == null || IV.Length <= 0)
            {
                fileOut.Close();
                return tempname;
            }

            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.KeySize = 256;
                aesAlg.Key = key;
                aesAlg.IV = IV;
                aesAlg.Padding = PaddingMode.ISO10126;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                CryptoStream csDecrypt = new CryptoStream(fileOut, decryptor, CryptoStreamMode.Read);

                byte[] userbyte = new byte[username.Length];
                csDecrypt.Read(userbyte, 0, userbyte.Length); //Считываем имя пользователя из зашифрованного файла  
                string name = Encoding.Default.GetString(userbyte); //Расшифровываем имя пользователя
                if (name != username) //Если расшифрованное имя пользователя не совпадает с текущим, вернем зашифрованное имя файла
                {
                    fileOut.Close();
                    return tempname;                   
                }
                else 
                {
                    byte[] nameLen = new byte[1];
                    csDecrypt.Read(nameLen, 0, 1); //Расшифровываем длинну имени файла
                    byte[] bytename = new byte[nameLen[0]];
                    csDecrypt.Read(bytename, 0, nameLen[0]); //Считываем имя файла
                    string dename = Encoding.Default.GetString(bytename); //Расшифрованное имя
                    fileOut.Close();
                    return "[Cryp]" + dename;
                }
            }
        }

        //Расшифровать файл
        private bool DecryptFileByte(string filepath, ProgressBar ProBar)
        {
            string[] temp = filepath.Split('\\'); //Сплитим путь файла
            string tempname = temp[temp.Length - 1]; //Берем имя фалй
            string temppath = filepath.Substring(0, filepath.Length - tempname.Length); //Берем путь до директории файла

            FileStream fileOut = new FileStream(filepath, FileMode.Open);
            ProBar.Maximum = (int)fileOut.Length;
            
            //Проверяем ключи на сущестоввание 
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            using (AesManaged aesAlg = new AesManaged())
            {
                //Задаем ключи
                aesAlg.KeySize = 256;
                aesAlg.Key = key;
                aesAlg.IV = IV;
                aesAlg.Padding = PaddingMode.ISO10126;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                CryptoStream csDecrypt = new CryptoStream(fileOut, decryptor, CryptoStreamMode.Read);

                byte[] userbyte = new byte[username.Length];
                csDecrypt.Read(userbyte, 0, userbyte.Length); //Расшифровываем имя пользователя
                string name = Encoding.Default.GetString(userbyte);
                if (name != username) //Если имя пользоватея из файла и текущее не совпадает
                {
                    MessageBox.Show("Файл зашифрован другим пользователем");
                    return false;
                }
                else
                {
                    byte[] nameLen = new byte[1];
                    csDecrypt.Read(nameLen,0,1); //Расшифровываем длинну имени файла
                    byte[] bytename = new byte[nameLen[0]];
                    csDecrypt.Read(bytename, 0, nameLen[0]); //Расшифровываем имя файла
                    string dename = Encoding.Default.GetString(bytename);

                    FileStream fileIn = new FileStream(temppath + "\\" + dename, FileMode.CreateNew);


                    int readIn;
                    byte[] bufferin = new byte[1048576]; //Буфер по 1МБ

                    //Пока файл не кончится расшифровываем
                    while ((readIn = csDecrypt.Read(bufferin, 0, bufferin.Length)) > 0)
                    {
                        Application.DoEvents();
                        fileIn.Write(bufferin, 0, readIn);
                        ProBar.Value += readIn;
                    }
                    csDecrypt.Close();

                    fileOut.Close();
                    fileIn.Close();

                    return true;
                }

            }
        }
    }
}
