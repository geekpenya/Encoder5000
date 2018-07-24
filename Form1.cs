using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Globalization;
using System.Diagnostics;

namespace Encoder5000
{
    public partial class Form1 : Form
    {
       
        public Form1()
        {
            InitializeComponent();
            PopulateDriveList();
            InitListView();
            encod.Enabled = false;
            decode.Enabled = false;
            SelfRef = this;

        }

        byte[] key;
        byte[] IV;
        Encoder encoder;
        bool LogIn = false;

        

        public static Form1 SelfRef
        {
            get;
            set;
        }

        public void getkey(byte[] keys, byte[] IVs, string username)
        {
            key = keys;
            IV = IVs;
            userlabel.Text = username;
            encod.Enabled = true;
            decode.Enabled = true;
            encoder = new Encoder(key, IV, username);
            LogIn = true;
           
            
            
        }

        protected void InitListView()
        {
          
            viewFile.Clear();        
                                   
            viewFile.Columns.Add("Name", 150, HorizontalAlignment.Left);
            viewFile.Columns.Add("Size", 80, HorizontalAlignment.Right);
            viewFile.Columns.Add("Created", 140, HorizontalAlignment.Left);
            viewFile.Columns.Add("Path", 200, HorizontalAlignment.Left);

        }

        


        //Функция получает список дисков и вносит их в дерево
        private void PopulateDriveList()
        {
            viewFolder.Nodes.Clear();
            TreeNode startNode = new TreeNode("Компьютер", 1, 1);
            viewFolder.Nodes.Add(startNode);

            TreeNodeCollection nodeCol = startNode.Nodes;
            ManagementObjectCollection colletion = getDrives();
            foreach(ManagementObject mo in colletion)
            {
                startNode = new TreeNode(string.Format("{0}\\", mo["Name"]), 0, 0);
                nodeCol.Add(startNode);
            }

        
        }

        TreeViewEventArgs tempArgs; //Буферное дерево для обновления списка после шифрования/дешифрования

        //Функция для получение списка директорий, идущие следом в иерархии, после выбора
        private void treeExp_AfterSelect(object sender, TreeViewEventArgs e)
        {
            tempArgs = e;
            Cursor = Cursors.WaitCursor;
            TreeNode nodeCurrent = e.Node;
            nodeCurrent.Nodes.Clear();
            if (nodeCurrent.SelectedImageIndex == 1)
            {
                PopulateDriveList();
            }
            else
            {
                PopulateDirectory(nodeCurrent, nodeCurrent.Nodes); 
            }
            Cursor = Cursors.Default;
        }

        //Функция открытия директории
        protected void PopulateDirectory(TreeNode nodeCurrent, TreeNodeCollection nodeCurrentCollection)
        {
           
            if (nodeCurrent.SelectedImageIndex == 0)
            {
                try
                {
                    if (Directory.Exists(getFullPath(nodeCurrent.FullPath)) == false)
                        MessageBox.Show("Directory or path " + nodeCurrent + " does not exist.");
                    else
                    {
                        //Выводим список файлов в директории
                        PopulateFiles(nodeCurrent);

                        //Получаем список следующих директорий и заносим их в дерево
                        string[] stringDirectories = Directory.GetDirectories(getFullPath(nodeCurrent.FullPath));

                        
                        foreach (string stringDir in stringDirectories)
                        {
                            string stringFullPath = stringDir;
                            string stringPathName = GetPathName(stringFullPath);

                            
                            var nodeDir = new TreeNode(stringPathName, 0, 0);
                            nodeCurrentCollection.Add(nodeDir);
                        }
                    }
                }
                catch (IOException)
                {
                    MessageBox.Show("Error: Drive not ready or directory does not exist.");
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("Error: Drive or directory access denided.");
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error: " + e);
                }
            }
        }

        //Прасим имя файла из его полного пути
        protected string GetPathName(string stringPath)
        {
            
            string[] stringSplit = stringPath.Split('\\');
            int maxIndex = stringSplit.Length;
            if (stringSplit[maxIndex - 1].Contains("EC5"))
            {
                if (LogIn)
                    return ( encoder.GetDecrypName(stringPath));
                else
                    return stringSplit[maxIndex - 1];
            }

            else
                return stringSplit[maxIndex - 1];
        }

        //Получаем список файлом в директории и выводим их в ListView
        protected void PopulateFiles(TreeNode nodeCurrent)
        {
            
            var lvData = new string[4];

            
            InitListView();

            if (nodeCurrent.SelectedImageIndex == 0)
            {
                
                if (Directory.Exists(getFullPath(nodeCurrent.FullPath)) == false)
                {
                    MessageBox.Show("Directory or path " + nodeCurrent + " does not exist.");
                }
                else
                {
                    try
                    {
                        string[] stringFiles = Directory.GetFiles(getFullPath(nodeCurrent.FullPath));
                        DateTime dtCreateDate;

                        //Пока не выведим все файлы
                        foreach (string stringFile in stringFiles)
                        {
                            string stringFileName = stringFile;
                            var objFileSize = new FileInfo(stringFileName);
                            Int64 lFileSize = objFileSize.Length;
                            dtCreateDate = objFileSize.CreationTime; 
                            

                            //Имя файла и его размер
                            lvData[0] = GetPathName(stringFileName);
                            lvData[1] = formatSize(lFileSize);

                            //Проверяем в каком формате было задано время создания файла
                            if (TimeZone.CurrentTimeZone.IsDaylightSavingTime(dtCreateDate) == false)
                            {
                                //Без настройки на летнее время
                                lvData[2] = formatDate(dtCreateDate.AddHours(1));
                            }
                            else
                            {
                                //С настройкой на летнее время
                                lvData[2] = formatDate(dtCreateDate);
                            }

                            
                            //Путь к файлу
                            lvData[3] = stringFileName;

                            //Создает и добавляем элемент в ListView
                            var lvItem = new ListViewItem(lvData, 0);
                            viewFile.Items.Add(lvItem);


                        }
                    }
                    catch (IOException)
                    {
                        MessageBox.Show("Error: Drive not ready or directory does not exist.");
                    }
                    catch (UnauthorizedAccessException)
                    {
                        MessageBox.Show("Error: Drive or directory access denided.");
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Error: " + e);
                    }
                }
            }
        }

        //Получаем корректный путь до файла
        protected static string getFullPath(string stringPath)
        {
            
            string stringParse = stringPath.Replace("Компьютер\\", "");

            return stringParse;
        }

        //Получаем корректный размер файла
        protected static string formatSize(Int64 lSize)
        {
            
            string stringSize;
            var myNfi = new NumberFormatInfo();

            Int64 lKBSize;

            if (lSize < 1024)
            {
                stringSize = lSize == 0 ? "0" : "1";
            }
            else
            {
                //Конвертируем в КБ
                lKBSize = lSize / 1024;
                //Стандартный формат 
                stringSize = lKBSize.ToString("n", myNfi);
                //Удаляем десятичную часть
                stringSize = stringSize.Replace(".00", "");
            }

            return string.Format("{0} KB", stringSize);

        }

        //Получить дату и время в коротком формате
        protected string formatDate(DateTime dtDate)
        {
            
            string stringDate;

            stringDate = dtDate.ToShortDateString() + " " + dtDate.ToShortTimeString();

            return stringDate;
        }

        //Получаем список дисков
        private static ManagementObjectCollection getDrives()
        {
            
            ManagementObjectCollection queryCollection;
            using (var query = new ManagementObjectSearcher("SELECT * From Win32_LogicalDisk "))
            {
                queryCollection = query.Get();
            }

            return queryCollection;
        }

        //Запускаем файл при двойном нажатию мыши
        private void viewFile_DoubleClick(object sender, EventArgs e)
        {
            Process.Start(viewFile.SelectedItems[0].SubItems[3].Text);
        }

        //Кнопка "Зашифровать"
        private void encod_Click(object sender, EventArgs e)
        {
            
            encoder.EncrypFile(viewFile.SelectedItems[0].SubItems[3].Text, progressBar1);
            progressBar1.Value = 0;
            MessageBox.Show("Файл успешно зашифрован");
            File.Delete(viewFile.SelectedItems[0].SubItems[3].Text);
            treeExp_AfterSelect(sender, tempArgs);
        }

        //Кнопка "Расшифровать"
        private void decode_Click(object sender, EventArgs e)
        {
            if (encoder.DecrypFile(viewFile.SelectedItems[0].SubItems[3].Text, progressBar1))
            {
                MessageBox.Show("Файл успешно расшифрован");
                progressBar1.Value = 0;
                File.Delete(viewFile.SelectedItems[0].SubItems[3].Text);
                treeExp_AfterSelect(sender, tempArgs);
            }
            

        }    
       
        //Вызов окна входа пользователя
        private void входToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            login.Show();
        }

        //Выход из пользователя
        private void выходToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            key = null;
            IV = null;
            userlabel.Text = "Вход не выполнен";
            encod.Enabled = false;
            decode.Enabled = false;
            encoder.Clean();
            LogIn = false;
        }

        private void новыйПользовательToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewUserForm formuser = new NewUserForm();
            formuser.Show();
        }

       
    }
}
