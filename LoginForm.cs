using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Encoder5000
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            
        }
        byte[] key;
        byte[] IV;

        private byte[] gethashpas(string pas)
        {
            SHA256Managed hash = new SHA256Managed();
            byte[] pasbyte = Encoding.Unicode.GetBytes(pas);
            byte[] hashpas = hash.ComputeHash(pasbyte);
            return hashpas;
        }

        private byte[] gethashlog(string log)
        {
            MD5CryptoServiceProvider hash = new MD5CryptoServiceProvider();
            byte[] logbyte = Encoding.Unicode.GetBytes(log);
            byte[] hashlog = hash.ComputeHash(logbyte);
            return hashlog;
        }

        private void decrypBD (string log, string pas)
        {
            AesManaged aesAlg = new AesManaged();

            aesAlg.KeySize = 256;
            aesAlg.Key = gethashpas(pas);
            aesAlg.IV = gethashlog(log);
            string resst = "";
            byte[] hashlog = gethashpas(log+pas);
            foreach (byte i in hashlog)
                resst += i.ToString();

            Encoder encoder = new Encoder(aesAlg.Key, aesAlg.IV, log);
            try
            {

                FileStream fileOut = new FileStream(resst.Substring(0, 10) + ".ECBD", FileMode.Open);
                FileStream fileIn = new FileStream(log + ".sqlite", FileMode.CreateNew);

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                CryptoStream csDecrypt = new CryptoStream(fileOut, decryptor, CryptoStreamMode.Read);

                int readIn;
                byte[] bufferin = new byte[1048576]; //Буфер по 1МБ


                //Пока файл не кончится расшифровываем
            
                    while ((readIn = csDecrypt.Read(bufferin, 0, bufferin.Length)) > 0)
                    {
                        Application.DoEvents();
                        fileIn.Write(bufferin, 0, readIn);

                    }
                    csDecrypt.Close();

                    fileOut.Close();
                    fileIn.Close();
                }

            catch
            {
               
                
                
                File.Delete(log + ".sqlite");
                MessageBox.Show("Неверное Имя пользователя или пароль");
                
            }
            
        }

        //Создать нового пользователя
        private void button2_Click(object sender, EventArgs e)
        {
            NewUserForm formuser = new NewUserForm();
            formuser.Show();
        }

       

        //Кнопка "Вход"
        //Имя пользователя = имя зашифрованной SQLite базы, а пароль = пароль от этой базы
        private void button1_Click(object sender, EventArgs e)
        {

            

            decrypBD(loginText.Text, passText.Text);

            try
            {
                //Открываем базу данных
                using (var conn = new SQLiteConnection("Data Source=" + loginText.Text + ".sqlite;Version=3;Password=" + passText.Text + ";"))
                {

                    conn.Open();

                    string sql = "SELECT key FROM keyuser;"; //Вытаскиваем ключи
                    using (SQLiteCommand command = new SQLiteCommand(sql, conn))
                    {
                        using (var rdr = command.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                key = (System.Byte[])rdr[0];
                            }
                        }
                    }

                    sql = "SELECT IV FROM keyuser;"; //Вытаскиваем вектор
                    using (var command = new SQLiteCommand(sql, conn))
                    {
                        using (var rdr = command.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                IV = (System.Byte[])rdr[0];
                            }
                        }
                    }
                }


                File.Delete(loginText.Text + ".sqlite");
                Form1.SelfRef.getkey(key, IV, loginText.Text); //Передаем в главную форму
                Close();


            }

            catch
            {
                File.Delete(loginText.Text + ".sqlite");
               
            }
          
            
            




        }
    }
}
