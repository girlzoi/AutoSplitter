using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Diagnostics;
using System.IO;


namespace AutoSplitterGUI
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            Definitions def = new Definitions();
            InitializeComponent();

            string[] configs = DefineConfigs().ToArray();
            comboBox1.Items.AddRange(configs);

            checkedListBox1 = new CheckedListBox();
            Controls.Add(checkedListBox1);
            checkedListBox1.Font = new Font("Segoe UI", 12F);
            checkedListBox1.FormattingEnabled = true;
            checkedListBox1.Items.AddRange(def.checkListItems);

            checkedListBox1.Location = new Point(17, 133);
            checkedListBox1.Name = "checkedListBox1";
            checkedListBox1.Size = new Size(236, 484);
            checkedListBox1.TabIndex = 5;
            checkedListBox1.Visible = false;


        }
        private CheckedListBox checkedListBox1;

        public void CheckBoxMaker(string selectedConfig)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void panel1_DragEnter(object sender, DragEventArgs e)
        {
            Definitions def = new Definitions();
            Program pgr = new Program();
            var selCfg = comboBox1.Text;
            string everyPage = textBox1.Text;
            //int pgDiff = 0;

            string[] inputFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (comboBox1.Visible == true)
                {
                    this.BeginInvoke(new Action(() => {
                        processWithCFG(inputFiles.ToArray(), selCfg);
                }));
                
                }
            else if (textBox1.Visible == true)
                {
                this.BeginInvoke(new Action(() => {
                    processWithEvery(inputFiles.ToArray(), int.Parse(everyPage));
                }));
                
                }
            
        }
        private void processWithCFG(string[] files, string selCfg)
        {
            Program pgr = new Program();
            Definitions def = new Definitions();
            foreach (var file in files)
            {
                int retry = 0;
                Console.WriteLine(file);
                var lines = File.ReadLines(def.configFolder + selCfg);
                foreach (var line in lines)
                {
                    try
                    {
                        Console.WriteLine(line);
                        int pgDiff = 0;
                        var a = file.ToString();
                        int ext = a.IndexOf(".");
                        var b = a.Substring(0, ext);
                        string[] aLine = line.Split(',');


                        if (!checkedListBox1.CheckedItems.Contains(aLine[0]))
                        {
                            pgr.SplitPDF(aLine[0], int.Parse(aLine[1]) - pgDiff, int.Parse(aLine[2]) - pgDiff, file, b);
                            //Console.WriteLine(aLine[0]);
                            //Console.WriteLine(aLine[2]);
                            //Console.WriteLine(aLine[2]);
                            //Console.WriteLine(file + " " + b + " (b)");
                            //Console.WriteLine(pgDiff);
                            label2.Text = aLine[0];
                        }
                        else if (checkedListBox1.CheckedItems.Contains(aLine[0]))
                        {
                            label2.Text = "Skipping " + aLine[0];
                            pgDiff = pgDiff + (int.Parse(aLine[2]) - int.Parse(aLine[1])) + 1;
                            //Console.WriteLine(pgDiff + " and is Contains[0]");
                        }
                    }
                    catch (Exception e) {
                        if (retry < 5)
                        {
                            retry++;
                            Console.WriteLine("I'm Old!");
                            System.Threading.Thread.Sleep(200);
                        }
                        else
                        {
                            throw e;
                        }
                    }
                    label2.Text = "Done!";
                }
            }
        }
        private void processWithEvery(string[] files, int everyPage)
        {
            Definitions def = new Definitions();
            Program pgr = new Program();
            
            foreach (var file in files)
            {
                int retry = 0;
                //System.Threading.Thread.Sleep(150);
                try {
                    int howMany = 1;
                    int firstPage = 1;
                    int lastPage = everyPage;
                    Console.WriteLine(file);
                    int length = PdfReader.Open(file, PdfDocumentOpenMode.Import).PageCount;
                    for (int i = 0; i < length; i = i + everyPage)
                    {
                        var a = file.ToString();
                        int ext = a.IndexOf(".");
                        var b = a.Substring(0, ext);

                        pgr.SplitPDF(howMany.ToString(), firstPage, lastPage, file, b);
                        firstPage = lastPage + 1;
                        lastPage = lastPage + everyPage;
                        howMany++;
                        label2.Text = howMany.ToString();
                        label2.Text = firstPage.ToString();
                        label2.Text = lastPage.ToString();
                        Console.WriteLine(howMany);
                        Console.WriteLine(firstPage);
                        Console.WriteLine(lastPage);
                    }
                } catch (Exception e) {
                    if (retry < 5) {
                        retry++;
                        System.Threading.Thread.Sleep(200);
                    }
                    else
                    {
                        throw e;
                    }
                }
                label2.Text = "Done!";
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Missing Forms") {
                missingForms();
            } else {
                openCfgFolder();
            }
        }
        private void missingForms()
        {
            Definitions def = new Definitions();
            var selCfg = comboBox1.Text;
            var selCfgPath = def.configFolder + selCfg;
            int j = 0;
            if (comboBox1.Text != "")
            {
                checkedListBox1.Items.Clear();
                checkedListBox1.Visible = true;
                var lines = File.ReadLines(selCfgPath);
                foreach (var line in lines)
                {
                    string[] aLine = line.Split(',');
                    Array.Resize(ref def.checkListItems, j + 1);
                    def.checkListItems[j] = aLine[0];
                    j++;
                }
            }
            checkedListBox1.Items.AddRange(def.checkListItems);
        }
        private void openCfgFolder()
        {
            Process.Start("explorer.exe", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\AutoSplitter\template\");
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Text = "Missing Forms";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Visible == true)
            {
                button2.Text = "Split Every X Page";
                button1.Text = "Open Config Folder";
                textBox1.Text = null;
            }
            else if (textBox1.Visible == false)
            {
                checkedListBox1.Visible = false;
                comboBox1.Text = null;
                button2.Text = "Select Config";
                button1.Text = "Open Config Folder";
            }
            textBox1.Visible = !textBox1.Visible;

            comboBox1.Visible = !comboBox1.Visible;

        }

    }
    public class Definitions 
    {
        //public string configFolder = @"..\..\..\template\";
        //public string[] cfgFiles = System.IO.Directory.GetFiles(@"..\..\..\template\", "*.cfg");
        static string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public string configFolder = @appData.ToString() + @"\AutoSplitter\template\";
        public string[] cfgFiles = System.IO.Directory.GetFiles(@appData.ToString() + @"\AutoSplitter\template\", "*.cfg");
        public string[] checkListItems = [];

    }
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            string[] exampleCFG = { "form-a, 1, 1", "form-b, 2, 4", "form-c, 5, 6", "form-d, 7, 10", "form-e, 11, 11", "form-f, 12, 16" };
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (!Directory.Exists(@appData + @"\AutoSplitter\")) {
                Directory.CreateDirectory(@appData + @"\AutoSplitter\");
            }
            if (!Directory.Exists(@appData + @"\AutoSplitter\template\"))
            {
                Directory.CreateDirectory(@appData + @"\AutoSplitter\template\");
            }
            Definitions def = new Definitions();
            if (def.cfgFiles.Length == 0) {
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(@appData + @"\AutoSplitter\template\", "example.cfg")))
                {
                    foreach (string line in exampleCFG)
                        outputFile.WriteLine(line);
                }
            }
            
            //putting these down here so hopefully cfg are stored to list configs before list is drawn
            ApplicationConfiguration.Initialize(); //run ui
            Application.Run(new Form1()); //run form code
        }
        public void SplitPDF(string formNa, int pStart, int pEnd, string fileNa, string fileLoc)
        {
                Directory.CreateDirectory(fileLoc);
                PdfDocument inputPDFFile = PdfReader.Open(fileNa, PdfDocumentOpenMode.Import);

                //Get the total pages in the PDF
                var totalPagesInInputPDFFile = inputPDFFile.PageCount; //set this to max page count from .cfg

                while (totalPagesInInputPDFFile != 0)
                {
                    //Create an instance of the PDF document in memory
                    PdfDocument outputPDFDocument = new PdfDocument();

                    // Add a specific page to the PdfDocument instance
                    if (pStart == pEnd)
                    {
                        outputPDFDocument.AddPage(inputPDFFile.Pages[pStart - 1]);
                    }
                    else
                    {
                        for (int i = pStart; i <= pEnd; i++)
                        {
                            outputPDFDocument.AddPage(inputPDFFile.Pages[i - 1]);
                        }
                    }

                    //save the PDF document
                    SaveOutputPDF(outputPDFDocument, formNa, fileLoc);

                    totalPagesInInputPDFFile--;
                //Console.WriteLine(fileLoc + " " + fileNa);
                }
            
        }
        private static void SaveOutputPDF(PdfDocument outputPDFDocument, string fileNa, string folderNa)
        {
            // Output file path
            string outputPDFFilePath = Path.Combine(folderNa, fileNa + ".pdf");

            //Save the document
            outputPDFDocument.Save(outputPDFFilePath);
        }

    }
}