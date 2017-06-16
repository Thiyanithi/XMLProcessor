namespace XMLProcessor_Assessment2_
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml;
    using System;

    public partial class XMLProcessorform : Form
    {
        SqlConnection connection = null;
        SqlCommand command, insertcommand = null;
        SqlDataReader reader = null;
        string xmldocument = string.Empty;
        string loggerdocument = string.Empty;
        public string success = string.Empty;
        DateTime date;
        

        public XMLProcessorform()
        {
            InitializeComponent();
            StopXMLGenerator.Enabled = false;
            StopXMLExecutor.Enabled = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled=false;
            try
            {
                string xmlgeneratorpath = XMLProcessor_Assessment2_.Properties.Settings.Default.xmlgeneratorpath;
                textBox1.Text = xmlgeneratorpath.ToString();

                string xmlexecutorpath = XMLProcessor_Assessment2_.Properties.Settings.Default.xmlexecutorpath;
                textBox2.Text = xmlexecutorpath.ToString();

                string loggerpath = XMLProcessor_Assessment2_.Properties.Settings.Default.loggerpath;
                textBox3.Text = loggerpath.ToString();

                string servername = XMLProcessor_Assessment2_.Properties.Settings.Default.servername;
                textBox5.Text = servername.ToString();

                int time = XMLProcessor_Assessment2_.Properties.Settings.Default.timen;
                textBox4.Text = time.ToString();

                string login = XMLProcessor_Assessment2_.Properties.Settings.Default.login;
                textBox6.Text = login.ToString();

                string password = XMLProcessor_Assessment2_.Properties.Settings.Default.password;
                textBox7.Text = password.ToString();

                XMLProcessor_Assessment2_.Properties.Settings.Default.xmlgeneratorpath = textBox1.Text;
                XMLProcessor_Assessment2_.Properties.Settings.Default.xmlexecutorpath = textBox2.Text;
                XMLProcessor_Assessment2_.Properties.Settings.Default.loggerpath = textBox3.Text;
                Properties.Settings.Default.Save();
                textBox7.PasswordChar = char.Parse("*");
            }
            catch (Exception objexception)
            {
                MessageBox.Show(objexception.ToString());
            }
        }

        public string Createloggerfile(string loggerdocument)
        {
            success = "False";
            string xmlloggerpath = textBox3.Text;
            xmlloggerpath = xmlloggerpath + "/Logger";
            Directory.CreateDirectory(xmlloggerpath);
            xmlloggerpath = xmlloggerpath + "/logger.txt";
            using (StreamWriter sw = File.AppendText(xmlloggerpath))
            {
                sw.WriteLine(loggerdocument);
                success = "True";
            }
            return success;
        }

        public string Createxmldocument(string xmldocument)
        {
            success = "False";
            try
            {
                string xmlgeneratorpath = textBox1.Text;
                xmlgeneratorpath = xmlgeneratorpath + "/GeneratorXMLs";
                Directory.CreateDirectory(xmlgeneratorpath);
                date = DateTime.Now;
                xmlgeneratorpath = xmlgeneratorpath + "/GeneratorXML" + date.Second + "s.xml";
                System.IO.File.Create(xmlgeneratorpath).Dispose();
                System.IO.File.AppendAllText(xmlgeneratorpath, xmldocument);
                success = "True";
            }
            catch (Exception)
            {
                MessageBox.Show("IO Exception Raise in XML Generator Path");
            }
            return success;
        }

        private void StartXMLGenerator_Click(object sender, EventArgs e)
        {
            success = "False";
            StopXMLGenerator.Enabled = true;
            xmldocument = string.Empty;
            bool checksome = false;
            try
            {
                string connectionstring = "Data Source=" + (textBox5.Text).ToString() + ";Initial Catalog=XMLProcessor;User ID=" + (textBox6.Text).ToString() + ";Password=" + (textBox7.Text).ToString();
                connection = new SqlConnection(connectionstring);
                command = new SqlCommand("select ItemId,ItemName from XMLItems where GenerateXML = 1", connection);
                connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    xmldocument = "<?xml version='1.0'?><Items><Item><ItemId>" + reader.GetValue(0).ToString() + "</ItemId>" + "<ItemName>" + reader.GetValue(1).ToString() + "</ItemName></Item></Items>".ToString();
                    loggerdocument = loggerdocument + "\r Process: XML Generator\n\tItemId:" + reader.GetValue(0).ToString() + "\tItem Name:" + reader.GetValue(1).ToString() + "\t Time to Write:" + System.DateTime.Now + "\t\t\t\t".ToString();
                    checksome = true;
                    while (checksome)
                    {
                        Thread.Sleep(int.Parse(textBox4.Text));
                        this.Createxmldocument(xmldocument);
                        checksome = false;
                        xmldocument = string.Empty;
                    }
                    success = "True";
                }
                this.Createloggerfile(loggerdocument);
                loggerdocument = "\n\n\t-------------END XML Generator---------------\n\n\n".ToString();
                this.Createloggerfile(loggerdocument);
            }
            catch (Exception)
            {
                MessageBox.Show("Error for SQL Connecntion");
            }
            //return success;
        }
        public object xmlgeneratorpath { get; set; }

        private void StopXMLGenerator_Click(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
            //return "True";
        }

        private void StartXMLExecutor_Click(object sender, EventArgs e)
        {
            success = "False";
            StopXMLExecutor.Enabled = true;
            try
            {
                string xmlexecutorpath = textBox2.Text;
                string xmlgeneratorpath = textBox1.Text;
                bool checksome = false;
                string loggdocument = string.Empty;
                xmlgeneratorpath = xmlgeneratorpath + "\\GeneratorXMLs";
                xmlexecutorpath = xmlexecutorpath + "\\ExecutedXMLs";
                DirectoryInfo sourceinfo = new DirectoryInfo(@xmlgeneratorpath);
                DirectoryInfo target = new DirectoryInfo(@xmlexecutorpath);
                string connectionstring = "Data Source=" + (textBox5.Text).ToString() + ";Initial Catalog=XMLProcessor;User ID=" + (textBox6.Text).ToString() + ";Password=" + (textBox7.Text).ToString();
                connection = new SqlConnection(connectionstring);
                connection.Open();
                if (!Directory.Exists(target.FullName))
                {
                    Directory.CreateDirectory(target.FullName);
                }
                foreach (FileInfo file in sourceinfo.GetFiles())
                {
                    string filewithdesignationpath = xmlgeneratorpath + "\\" + file;
                    XmlDataDocument xmldoc = new XmlDataDocument();
                    XmlNodeList xmlnodes;
                    int node = 0;
                    int ItemId = 0;
                    date = DateTime.Now;
                    string ItemName = string.Empty;
                    FileStream fs = new FileStream(filewithdesignationpath, FileMode.Open, FileAccess.Read);
                    xmldoc.Load(fs);
                    xmlnodes = xmldoc.GetElementsByTagName("Item");
                    for (node = 0; node <= xmlnodes.Count - 1; node++)
                    {
                        xmlnodes[node].ChildNodes.Item(0).InnerText.Trim();
                        ItemId = int.Parse(xmlnodes[node].ChildNodes.Item(0).InnerText.Trim());
                        ItemName = xmlnodes[node].ChildNodes.Item(1).InnerText.Trim();
                        fs.Close();
                        var time = date.ToString("H:mm:ss");
                        command = connection.CreateCommand();
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "CalculateTimeStamp";
                        SqlCommand cmd = new SqlCommand("CalculateTimeStamp", connection);
                        command.Parameters.AddWithValue("@Hours", date.Hour);
                        command.Parameters.AddWithValue("@Minits",date.Minute);
                        command.Parameters.AddWithValue("@Seconds",date.Second);
                        var result = command.Parameters.Add("@TimeStampValue", SqlDbType.Int);
                        result.Direction = ParameterDirection.ReturnValue;
                        command.ExecuteNonQuery();
                        int timestamp = Convert.ToInt32(result.Value);    
                        string insertQuery = "insert into XMLMessages(ItemId,ItemName,Timestamp,Value) values(" + ItemId + ",'" + ItemName + "','" + time + "',"+ timestamp +");";
                        loggdocument = "\r Process: XML Executor\n\tItemId:" + ItemId + "\tItem Name:" + ItemName + "\t Time to Write:" + System.DateTime.Now + "\t\t\t\t".ToString();
                        this.Createloggerfile(loggdocument);
                        insertcommand = new SqlCommand(insertQuery, connection);
                        insertcommand.ExecuteNonQuery();
                        File.Move(filewithdesignationpath, xmlexecutorpath + "\\" + file);
                        checksome = true;
                    }
                    while (checksome)
                    {
                        Thread.Sleep(int.Parse(textBox4.Text));
                        checksome = false;
                    }
                    success = "True";
                }
                loggerdocument = "\n\n\t---------------- END XML EXecutor ---------------\n\n\n".ToString();
                this.Createloggerfile(loggerdocument);
                connection.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("check the ItemId already exists or not.");
            }
            //return success;
        }

        private void StopXMLExecutor_Click(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
            //return "True";
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog objFolderBrowserDialog = new FolderBrowserDialog();
            objFolderBrowserDialog.ShowDialog();
            textBox1.Text = objFolderBrowserDialog.SelectedPath;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog objFolderBrowserDialog = new FolderBrowserDialog();
            objFolderBrowserDialog.ShowDialog();
            textBox2.Text = objFolderBrowserDialog.SelectedPath;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog objFolderBrowserDialog = new FolderBrowserDialog();
            objFolderBrowserDialog.ShowDialog();
            textBox3.Text = objFolderBrowserDialog.SelectedPath;
        }
    }
}




    