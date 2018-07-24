using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Encoder5000
{
    public partial class NewUserForm : Form
    {
        public NewUserForm()
        {
            InitializeComponent();
            
        }

        

        private byte[] gethashpas (string pas)
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

        private void encrypbd(string pas, string login)
        {
            AesManaged aesAlg = new AesManaged();
            

            aesAlg.KeySize = 256;
            aesAlg.Key = gethashpas(pas);
            aesAlg.IV = gethashlog(login);
            string resst = "";
            byte[] hashlog = gethashpas(login + pas);
            foreach (byte i in hashlog)
                resst += i.ToString();

            FileStream fileOut = new FileStream(login + ".sqlite", FileMode.Open);
            FileStream fileIn = new FileStream(resst.Substring(0, 10) + ".ECBD", FileMode.CreateNew);

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            CryptoStream csEncrypt = new CryptoStream(fileIn, encryptor, CryptoStreamMode.Write);

            byte[] bufferin = new byte[1048576];
            int readIn;

            while ((readIn = fileOut.Read(bufferin, 0, bufferin.Length)) > 0)
            {
                Application.DoEvents();
                csEncrypt.Write(bufferin, 0, readIn);
       
            }
            csEncrypt.Close();
            fileOut.Close();
            fileIn.Close();

            File.Delete(login + ".sqlite");
        }

        AesManaged aesAlg = new AesManaged(); //Генерируем ключи шифрования
        

        //СОздать нового пользователя
        private void button1_Click(object sender, EventArgs e)
        {
            if (passText.Text != passText2.Text)
                MessageBox.Show("Пароли не совпадают");
            else
            {

                SQLiteConnection.CreateFile(userText.Text + ".sqlite"); //Создаем SQLite 

                SQLiteConnection conn = new SQLiteConnection("Data Source=" + userText.Text + ".sqlite;Version=3;");
                conn.SetPassword(passText.Text); //Задаем пароль
                conn.Open();

                string sql = "create table keyuser (key blob, IV blob)"; //Задаем поля в базе

                SQLiteCommand command = new SQLiteCommand(sql, conn);
                command.ExecuteNonQuery();
                aesAlg.KeySize = 256;
                byte[] keyval = aesAlg.Key;
                byte[] IVvak = aesAlg.IV;

                command.CommandText = String.Format("INSERT INTO keyuser (key, IV) VALUES (@kk, @ii);"); //Записываем ключи и вектор в базу
                SQLiteParameter param = new SQLiteParameter("@kk", System.Data.DbType.Binary);
                SQLiteParameter param2 = new SQLiteParameter("@ii", System.Data.DbType.Binary);
                param.Value = keyval;
                param2.Value = IVvak;
                command.Parameters.Add(param);
                command.Parameters.Add(param2);

                command.ExecuteNonQuery();
                command.Dispose();
                conn.Close();
                
                encrypbd(passText.Text, userText.Text);
                Close();
            }
        }
    }
}
