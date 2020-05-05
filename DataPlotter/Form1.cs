using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.WindowsForms;
using OxyPlot.Series;
using OxyPlot.Axes;
using OxyPlot.Annotations;
using System.Reflection;
using System.IO.Ports;

namespace DataPlotter
{
    public partial class Form1 : Form
    {
        public static readonly Protocol[] ImplementedProtocols = 
        {
            new MyOneWire_OxyPlot(""),
            new JustPlot()                       
        };
        protected Protocol SelectedProtocol;
        private Random randomGen = new Random();
        private Timer tim = new Timer() { Interval = 1000, Enabled = false };
        public Form1()
        {
            InitializeComponent();
            tim.Tick += DownloadData;
            //Fill About tab
            textBox1.Text = LoadAssembly(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
            ConfigureDialogs();
            ConfigureSettings();
        }
        static string formatter(double d)      
        {
            return ((uint)d).ToString();
        }

        #region "Configuration"

        void ConfigureChartForLogic(object ChartControl)
        {
            var myModel = new PlotModel();
            myModel.Axes.Add(new LinearAxis()  //X
            {
                Position = AxisPosition.Bottom,
                Title = "uS",
                IsZoomEnabled = true,
                IsPanEnabled = true,
                MinimumMinorStep = 1,
                MinimumMajorStep = 10,
                LabelFormatter = formatter,
                MinimumRange = 10,
                TicklineColor = OxyColors.White,
                AxislineColor = OxyColors.White,
                TitleColor = OxyColors.White,
                MinorTicklineColor = OxyColors.LightGray
            });
            myModel.Axes.Add(new LinearAxis()       //Y
            {
                Position = AxisPosition.Left,
                Title = "Level",
                IsZoomEnabled = false,
                IsPanEnabled = false,
                MinimumMinorStep = 1,
                MinimumMajorStep = 1,
                AbsoluteMaximum = 1.35,
                AbsoluteMinimum = -0.75,
                Minimum = -0.75,
                Maximum = 1.35,
                TicklineColor = OxyColors.White,
                AxislineColor = OxyColors.White,
                TitleColor = OxyColors.White,
                MinorTicklineColor = OxyColors.LightGray
            });
            myModel.Axes.Add(new LinearColorAxis
            {
                Position = AxisPosition.None,
                Palette = OxyPalettes.Jet(4),
                AbsoluteMaximum = 3,
                AbsoluteMinimum = -3,
                Maximum = 3,
                Minimum = -3
            });
            myModel.Background = OxyColors.Black;
            myModel.TextColor = OxyColors.White;
            myModel.TitleColor = OxyColors.White;
            myModel.PlotAreaBorderColor = OxyColors.LightGray;
            myModel.SelectionColor = OxyColors.LightBlue;
            myModel.DefaultFont = StandardFonts.Helvetica.BoldFont.BaseFont;
            (ChartControl as PlotView).Model = myModel;   
        }

        void ConfigureDialogs()
        {
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.Multiselect = false;
            saveFileDialog1.ValidateNames = true;
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.DefaultExt = ".txt";
            saveFileDialog1.CreatePrompt = false;
            saveFileDialog1.OverwritePrompt = true;                            
            colorDialog1.AllowFullOpen = true;
            colorDialog1.AnyColor = true;
        }

        void ConfigureSettings()
        {
            Properties.Settings.Default.Reload();
            //Load all implemented so far protocols
            cmbProtocol.DisplayMember = "HumanReadable";
            cmbProtocol.ValueMember = "Internal";
            foreach (var item in ImplementedProtocols)
            {
                cmbProtocol.Items.Add(item.Name);
            }
            //Select last used
            for (int i = 0; i < cmbProtocol.Items.Count; i++)
            {
                if (((Protocol.ProtocolName)(cmbProtocol.Items[i])).Internal == Properties.Settings.Default.LastProtocol)
                {
                    cmbProtocol.SelectedItem = cmbProtocol.Items[i];
                }
            }
            //Load last used colors
            lstColor.DisplayMember = "Index";
            LoadColors();
            //Add last used paths
            lstPath.Items.AddRange(Properties.Settings.Default.LastFiles.Cast<string>().ToArray());
        }

        #endregion

        #region "Methods"

        void LoadColors()
        {
            Properties.Settings.Default.Save();
            lstColor.Items.Clear();
            for (int i = 0; i < Properties.Settings.Default.LastColors.Count; i++)
            {
                lstColor.Items.Add(new SeriesColorCode(ColorTranslator.FromHtml(Properties.Settings.Default.LastColors[i]), i));
            }
            lstColor.Refresh();
        }
        void LoadFiles()
        {
            Properties.Settings.Default.Save();
            lstPath.Items.Clear();
            for (int i = 0; i < Properties.Settings.Default.LastFiles.Count; i++)
            {
                lstPath.Items.Add(Properties.Settings.Default.LastFiles[i]);
            }
            lstPath.Refresh();
        }
        string LoadAssembly(string path)
        {
            if (!File.Exists(path))
            {
                return "";
            }
            var ass = Assembly.LoadFile(path);
            return (ass.GetCustomAttributes(typeof(AssemblyTitleAttribute), false)
                    .OfType<AssemblyTitleAttribute>()
                    .FirstOrDefault()?
                    .Title ?? "") + " v" +
                (ass.GetName().Version.ToString()) + ", a part of " +
                (ass.GetCustomAttributes(typeof(AssemblyProductAttribute), false)
                    .OfType<AssemblyProductAttribute>()
                    .FirstOrDefault()?
                    .Product ?? "N/A") + " project." + Environment.NewLine +
                (ass.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false)
                    .OfType<AssemblyCompanyAttribute>()
                    .FirstOrDefault()?
                    .Company ?? "N/A") + ", " +
                (ass.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)
                    .OfType<AssemblyCopyrightAttribute>()
                    .FirstOrDefault()?
                    .Copyright ?? "N/A") + Environment.NewLine +
                (ass.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)
                    .OfType<AssemblyDescriptionAttribute>()
                    .FirstOrDefault()?
                    .Description ?? "N/A");
        }
        internal Color RandomColor()
        {
            KnownColor[] names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            KnownColor randomColorName = names[randomGen.Next(names.Length)];
            return Color.FromKnownColor(randomColorName);
        }

        #endregion

        #region "Events"

        private void Form1_Load(object sender, EventArgs e)
        {
            Location = Properties.Settings.Default.LastPosition;
            Size = Properties.Settings.Default.FormSize;
            txtBuffer.Text = Properties.Settings.Default.BufferSize.ToString();
            txtLoad.Text = Properties.Settings.Default.LoadCmd;
            txtSavePath.Text = Properties.Settings.Default.SavePath;
            if (SerialPort.GetPortNames().Contains(Properties.Settings.Default.LastPort))
            {
                cmbPorts.Items.Clear();
                cmbPorts.Items.AddRange(SerialPort.GetPortNames());
                cmbPorts.SelectedItem = Properties.Settings.Default.LastPort;
            }
            LoadColors();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnAddInput_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.LastFiles.AddRange(openFileDialog1.FileNames);
            }
            openFileDialog1.Multiselect = false;
            LoadFiles(); 
            while (lstPath.Items.Count > Properties.Settings.Default.LastColors.Count)
            {
                Properties.Settings.Default.LastColors.Add(ColorTranslator.ToHtml(RandomColor()));
            }
            LoadColors();                     
        }

        private void btnDeleteInput_Click(object sender, EventArgs e)
        {
            for (int i = lstPath.SelectedIndices.Count - 1; i >= 0; i--)
            {
                Properties.Settings.Default.LastFiles.RemoveAt(lstPath.SelectedIndices[i]);
            }
            LoadFiles();
        }

        private void cmbProtocol_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i;
            for (i = 0; i < ImplementedProtocols.Length; i++)
            {
                if (ImplementedProtocols[i].Name.Internal == ((Protocol.ProtocolName)cmbProtocol.SelectedItem).Internal)
                {
                    SelectedProtocol = ImplementedProtocols[i];
                    txtToolPath.Text = (Properties.Settings.Default.ToolPathes.Cast<string>()
                        .Where(x => (x.Split('|').FirstOrDefault() ?? "") == SelectedProtocol.Name.Internal).FirstOrDefault() ?? "")
                        .Split('|').Last();
                    txtToolArguments.Text = (Properties.Settings.Default.Commands.Cast<string>()
                        .Where(x => (x.Split('|').FirstOrDefault() ?? "") == SelectedProtocol.Name.Internal).FirstOrDefault() ?? "")
                        .Split('|').Last();
                    break;
                }
            }
            Properties.Settings.Default.LastProtocol = SelectedProtocol.Name.Internal;
            Properties.Settings.Default.Save();
            if (SelectedProtocol != null)  //Current parser assembly info
            {
                if (textBox1.Text.Contains(Environment.NewLine + Environment.NewLine))
                {
                    textBox1.Text = textBox1.Text.Remove(textBox1.Text.IndexOf(Environment.NewLine + Environment.NewLine));
                }

                textBox1.Text += Environment.NewLine + Environment.NewLine + "Current tool information:" + Environment.NewLine;
                textBox1.Text += LoadAssembly(SelectedProtocol.ToolPath);

            }
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            btnProcess.Enabled = false;
            Properties.Settings.Default.Save();
            tabControl1.TabPages[1].Controls.Clear();
            tabControl1.TabPages[2].Controls.Clear();
            PlotView plot;
            TextBox info;
            SplitContainer container;
            List<SplitContainer> plot_container = new List<SplitContainer>(lstPath.Items.Count);
            List<SplitContainer> info_container = new List<SplitContainer>(lstPath.Items.Count);
            for (int i = 0; i < lstPath.Items.Count; i++)
            {
                plot = new PlotView() { Dock = DockStyle.Fill };                 //Create new bunch of controls
                ConfigureChartForLogic(plot);
                info = new TextBox()
                {
                    ReadOnly = true, ScrollBars = ScrollBars.Both, Multiline = true, Dock = DockStyle.Fill, WordWrap = false
                };
                int code = SelectedProtocol.InvokeParser(lstPath.Items[i] as string);
                if (code == 0)    //Fill in the data
                {
                    string text = File.ReadAllText(lstPath.Items[i] as string);
                    string parsed = File.ReadAllText(SelectedProtocol.GetParsedFilePath(lstPath.Items[i] as string));
                    info.Text = SelectedProtocol.ExtractInfo(plot, parsed);
                    SelectedProtocol.Plot(plot, text, parsed, ((SeriesColorCode)(lstColor.Items[i])).Color);   //raw, parsed
                }
                else
                {
                    MessageBox.Show("Parsing file " + (lstPath.Items[i] as string) + " failed (code " + code.ToString() + ")." +
                        Environment.NewLine + "Parser returned following info:" + Environment.NewLine +
                        SelectedProtocol.GetCodeDescription(code));
                }
                container = new SplitContainer()
                {
                    Orientation = Orientation.Horizontal,
                    Panel2Collapsed = true,
                    Dock = DockStyle.Fill
                };
                container.Panel1.Controls.Add(plot);
                plot_container.Add(container);
                container = new SplitContainer() { Orientation = Orientation.Vertical, Panel2Collapsed = true, Dock = DockStyle.Fill };
                container.Panel1.Controls.Add(info);
                info_container.Add(container);
            }
            for (int i = lstPath.Items.Count - 1; i > 0; i--)
            {
                plot_container[i - 1].Panel2Collapsed = false;
                plot_container[i - 1].Panel2.Controls.Add(plot_container[i]);
                info_container[i - 1].Panel2Collapsed = false;
                info_container[i - 1].Panel2.Controls.Add(info_container[i]);
            }
            tabControl1.TabPages[1].Controls.Add(plot_container[0]);
            tabControl1.TabPages[2].Controls.Add(info_container[0]);
            tabControl1.SelectedIndex = 1;
            btnProcess.Enabled = true;
        }

        private void btnEditColor_Click(object sender, EventArgs e)
        {
            if (lstColor.SelectedIndices.Count > 0)
            {
                if (colorDialog1.ShowDialog() == DialogResult.OK)
                {
                    Properties.Settings.Default.LastColors[lstColor.SelectedIndices[0]] = ColorTranslator.ToHtml(colorDialog1.Color);
                    lstColor.Items[lstColor.SelectedIndex] = new SeriesColorCode(
                        colorDialog1.Color,
                        ((SeriesColorCode)(lstColor.Items[lstColor.SelectedIndex])).Index
                        );
                    lstColor.Refresh();
                }
            }
        }

        private void btnToolOpen_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtToolPath.Text = openFileDialog1.FileName;
            }
        }

        private void btnToolSave_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Properties.Settings.Default.ToolPathes.Count; i++)
            {
                if (Properties.Settings.Default.ToolPathes[i].Split('|').First() == SelectedProtocol.Name.Internal)
                {
                    Properties.Settings.Default.ToolPathes[i] = SelectedProtocol.Name.Internal + '|' + SelectedProtocol.ToolPath;
                }
            }
            for (int i = 0; i < Properties.Settings.Default.Commands.Count; i++)
            {
                if (Properties.Settings.Default.Commands[i].Split('|').First() == SelectedProtocol.Name.Internal)
                {
                    Properties.Settings.Default.Commands[i] = SelectedProtocol.Name.Internal + '|' + txtToolArguments.Text;
                }
            }
            Properties.Settings.Default.Save();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.LastPosition = Location;
            Properties.Settings.Default.FormSize = Size;
            Properties.Settings.Default.Save();
        }

        private void txtToolPath_TextChanged(object sender, EventArgs e)
        {
            if (SelectedProtocol != null)
            {
                SelectedProtocol.ToolPath = txtToolPath.Text;
            }
        }

        private void txtToolArguments_TextChanged(object sender, EventArgs e)
        {
            if (SelectedProtocol != null)
            {
                SelectedProtocol.CommandLineTemplate = Protocol.InputPlaceholder + (txtToolArguments.Text == "" ? "" : (' ' + txtToolArguments.Text));
            }
        }

        private void lstColor_DrawItem(object sender, DrawItemEventArgs e)
        {
            SolidBrush brush = new SolidBrush(((SeriesColorCode)(lstColor.Items[e.Index])).Color);
            e.Graphics.FillRectangle(brush, e.Bounds);
            brush.Color = brush.Color.GetBrightness() < 0.5 ? Color.White : Color.Black;
            e.Graphics.DrawString("Series #" + (e.Index + 1).ToString(), DefaultFont, brush, e.Bounds.Location);
            const int SelWidth = 1;
            if (lstColor.SelectedIndex == e.Index)
            {
                e.Graphics.DrawRectangle(new Pen(Color.Black, SelWidth), e.Bounds.X + SelWidth / 2, e.Bounds.Y + SelWidth / 2, e.Bounds.Width - SelWidth, e.Bounds.Height - SelWidth);
            }
        }

        private void lstColor_Leave(object sender, EventArgs e)
        {
            lstColor.Refresh();
        }

        private void lstColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstColor.Refresh();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.SavePath = txtSavePath.Text;
            Properties.Settings.Default.LoadCmd = txtLoad.Text;
            Properties.Settings.Default.BufferSize = serialPort1.ReadBufferSize;
            Properties.Settings.Default.LastPort = cmbPorts.SelectedItem as string;
        }

        private void cmbPorts_SelectedIndexChanged(object sender, EventArgs e)
        {
            serialPort1.PortName = cmbPorts.SelectedItem as string;
        }

        private void btnPort_Click(object sender, EventArgs e)
        {
            serialPort1.Open();
            btnOpenPort.Enabled = false;
            btnClosePort.Enabled = true;
        }

        private void btnClosePort_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            btnClosePort.Enabled = false;
            btnOpenPort.Enabled = true;
        }

        string SerialData;
        int ReadingAttempts;
        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                if (Directory.Exists(Path.GetDirectoryName(txtSavePath.Text)) || (txtSavePath.Text.Length == 0))
                {      
                    btnLoad.Enabled = false;
                    if (txtLoad.Text.Length > 0)
                    {                                
                        char[] send = txtLoad.Text.ToArray();
                        serialPort1.Write(send, 0, txtLoad.Text.Length);
                    }
                    SerialData = "";
                    ReadingAttempts = 0;
                    tim.Enabled = true;
                    tim.Start();
                }
                else
                {
                    MessageBox.Show("Invalid path.");
                }
            }
            else
            {
                MessageBox.Show("Port is not open.");
            }
        }

        void DownloadData(object sender, EventArgs e)
        {
            tim.Stop();
            try
            {
                while (serialPort1.BytesToRead > 0)
                {
                    SerialData += serialPort1.ReadExisting();
                }
                if (ReadingAttempts < 5)
                {
                    ReadingAttempts++;
                    tim.Start();
                }
                else
                {
                    if (SerialData == "")
                    {
                        MessageBox.Show("Nothing was received.");
                    }
                    else
                    {
                        if (txtSavePath.Text.Length > 0)
                        {
                            File.WriteAllText(txtSavePath.Text, SerialData);
                            MessageBox.Show("Data successfully saved.");
                        }
                        else
                        {
                            SerialData = "";
                            MessageBox.Show("Data successfully discarded (path is empty).");
                        }
                    }
                    btnLoad.Enabled = true;
                    tim.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during downloading: " + ex.Message);
                serialPort1.Close();
                btnOpenPort.Enabled = true;
                btnClosePort.Enabled = false;
                tim.Enabled = false;
                btnLoad.Enabled = true;
            }
        }

        private void btnSaveBrowse_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtSavePath.Text = saveFileDialog1.FileName;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            int buf;
            if (int.TryParse(txtBuffer.Text, out buf))
            {
                serialPort1.ReadBufferSize = buf;
            }
        }

        private void lstColor_DoubleClick(object sender, EventArgs e)
        {
            btnEditColor_Click(sender, e);
        }

        private void btnClearBuffer_Click(object sender, EventArgs e)
        {
            while (serialPort1.BytesToRead > 0)
            {
                serialPort1.DiscardInBuffer();
            }
        }

        private void cmbPorts_DropDown(object sender, EventArgs e)
        {
            
        }

        private void cmbPorts_Click(object sender, EventArgs e)
        {
            string sel = cmbPorts.SelectedItem as string;
            cmbPorts.Items.Clear();
            cmbPorts.Items.AddRange(SerialPort.GetPortNames());
            if (sel != null)
            {
                if (cmbPorts.Items.Contains(sel))
                {
                    cmbPorts.SelectedItem = sel;
                }
            }
        }
    }

    #endregion

    #region "Classes"

    public struct SeriesColorCode
    {
        public SeriesColorCode(Color color, int index)
        {
            _color = color;
            _html = ColorTranslator.ToHtml(color);
            Index = index;
        }
        private string _html;
        private Color _color;
        public string HTML
        {
            get { return _html; }
            set
            {
                _html = value;
                _color = ColorTranslator.FromHtml(value);
            }
        }
        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                _html = ColorTranslator.ToHtml(value);
            }
        }
        public int Index { get; }
    }

    public abstract class Protocol     //Works with human-readable parser output, parser output readability is intended
    {
        //Static
        public static readonly string InputPlaceholder = "{Input}";

        //Non-static
        public Protocol(string path, int timeout = 10000)
        {
            CommandLineTemplate = InputPlaceholder;
            ToolPath = path;                                  
            time.Tick += timeout_handler;
            time.Interval = timeout;
        }

        public struct ProtocolName
        {
            public ProtocolName(string serializable, string human)
            {
                HumanReadable = human;
                Internal = serializable;
            }
            public string HumanReadable { get; set; }
            public string Internal { get; set; }
        }
        public abstract string GetCodeDescription(int code);
        public delegate string ParsedFilepathRoutine(string InitialPath);
        public delegate bool PlotterRoutine(object ChartControl, object[] Data);
        public delegate string InfoExtractorRoutine(object Chart, string Data);
        public ParsedFilepathRoutine FilePathModifier { get; set; }
        public PlotterRoutine Plotter { get; set; }
        public InfoExtractorRoutine InfoExtractor { get; set; }
        public abstract ProtocolName Name { get; }
        public string ToolPath { get; set; }
        public string CommandLineTemplate { get; set; }
        public virtual int InvokeParser(string path)
        {
            try
            {
                time.Enabled = true;
                time.Start();
                Process parser = Process.Start(
                    ToolPath, 
                    CommandLineTemplate.Replace(InputPlaceholder, path.Contains(' ') ? ('"' + path + '"') : path) 
                    );
                while (time.Enabled && !parser.HasExited)
                {
                    System.Threading.Thread.Sleep(100);
                }
                return parser.ExitCode;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during invoking parser: " + ex.Message);
                return -1;   
            }
        }
        public bool Plot(object Chart, params object[] Data)
        {
            if (Plotter == null)
            {
                return false;
            }
            return Plotter(Chart, Data);
        }
        public string ExtractInfo(object Chart, string Data)
        {
            if (InfoExtractor == null)
            {
                return null;
            }
            return InfoExtractor(Chart, Data);
        }
        public string GetParsedFilePath(string InitialPath)
        {
            if (FilePathModifier == null)
            {
                return InitialPath;
            }
            return FilePathModifier(InitialPath);
        }

        private Timer time = new Timer();
        private void timeout_handler(object sender, EventArgs e)
        {
            time.Stop();
            time.Enabled = false;
        }
    }
    
    public class MyOneWire_OxyPlot : Protocol
    {
        public MyOneWire_OxyPlot(string tool_path) : base(tool_path)
        {
            Plotter = OneWirePlotter;
            InfoExtractor = OneWireInfoExtractor;
            FilePathModifier = OneWireNameModifier;
        }
        public static ProtocolName StaticName = new ProtocolName("MyOneWire", "OneWire (my custom parser)");
        public override ProtocolName Name
        {
            get { return StaticName; }
        }
        public override string GetCodeDescription(int code)
        {
            switch (code)
            {
                case 0:
                    return "OK.";
                case 1:
                    return "Incorrect file path.";
                case 2:
                    return "Exception caught during parsing.";
                case 3:
                    return "File contained multiple captures and was split. Please invoke parser again for each split fragment.";
                default:
                    return "N/A";
            }
        }
        internal bool OneWirePlotter(object ChartControl, object[] Data)
        {
            try
            {
                string[] lines;
                //Load points (Data[0])
                lines = (Data[0] as string).Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                List<DataPoint> points = new List<DataPoint>(lines.Length);
                foreach (var item in lines)
                {
                    string[] tmp = item.Split(':');
                    points.Add(new DataPoint(int.Parse(tmp[0]), int.Parse(tmp[1])));
                }
                //Load annotations  (Data[1])
                Data[1] = (Data[1] as string).Remove(0, (Data[1] as string).IndexOf("L:")).Trim('\r', '\n').Remove(0, 2).Trim('\r', '\n'); //Get the list of pulses
                lines = (Data[1] as string).Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                int last_coord = 0;
                int[] last_len = new int[] { 0, 0 };
                double[] pos = new double[] { 1.1, -0.15 };
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] tmp = lines[i].Split('@').Select(x => x.Trim()).ToArray();
                    string[] dur = tmp[0].Split('=').Select(x => x.Trim()).ToArray();
                    tmp[0] = dur[0];
                    if (tmp[0].Length == 0) continue;
                    int[] len = new int[] { tmp[0].Length, dur[1].Length };
                    int coord = int.Parse(tmp[1]);
                    double size = (ChartControl as PlotView).Model.DefaultFontSize;
                    if (((last_len[0] > 1) || (len[0] > 1) || (coord - last_coord < 25)) && (coord - last_coord < 600))
                    {
                        pos[0] = pos[0] > 1.2 ? 1.1 : 1.25;
                    }
                    else
                    {
                        pos[0] = 1.1;
                    }               
                    if ((len[0] > 6) && (coord - last_coord < 100)) //Try not to create a mess
                    {
                        size /= 1.5;
                    }
                    else
                    {
                        if (len[0] > 4) size /= 1.2;
                    }
                    (ChartControl as PlotView).Model.Annotations.Add(new ImageAnnotation()      //Type
                    {
                        ImageSource = DrawText(tmp[0], size),
                        X = new PlotLength(coord, PlotLengthUnit.Data),
                        Y = new PlotLength(pos[0], PlotLengthUnit.Data)
                    });
                    size = (ChartControl as PlotView).Model.DefaultFontSize / 1.5; 
                    if (((last_len[1] > 1) || (len[1] > 1) || (coord - last_coord < 25)) && (coord - last_coord < 150))
                    {
                        pos[1] = (pos[1] < -0.15) ? -0.15 : -0.3;
                    }
                    else
                    {
                        pos[1] = -0.15;
                    }                  
                    (ChartControl as PlotView).Model.Annotations.Add(new ImageAnnotation()      //Duration
                    {
                        ImageSource = DrawText(dur[1], size),
                        X = new PlotLength(coord, PlotLengthUnit.Data),
                        Y = new PlotLength(pos[1], PlotLengthUnit.Data)
                    });
                    last_coord = coord;
                    last_len = len;
                }
                //Setup the plot
                var mySeries = new StairStepSeries();
                mySeries.Points.AddRange(points);
                mySeries.Color = OxyColor.FromRgb(((Color)(Data[2])).R, ((Color)(Data[2])).G, ((Color)(Data[2])).B);
                (ChartControl as PlotView).Model.Axes[0].MaximumRange = points.Last().X - points.First().X + 1;
                (ChartControl as PlotView).Model.Axes[0].AbsoluteMaximum = points.Last().X + 1;
                (ChartControl as PlotView).Model.Axes[0].AbsoluteMinimum = points.First().X - 1;
                (ChartControl as PlotView).Model.Series.Add(mySeries);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during listing parsing: " + ex.Message);
                return false;
            }
            return true;
        }
        internal string OneWireInfoExtractor(object ChartControl, string Data)
        {
            try
            {
                //Parse data and setup annotations
                string info = Data.Substring(0, Data.IndexOf("L:")).Trim('\r', '\n');
                string[] lines = info.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                ScatterSeries transaction_starts = new ScatterSeries()
                {
                    MarkerType = MarkerType.Circle,    
                    MarkerSize = 2
                }; 
                double reduced_font = (ChartControl as PlotView).Model.DefaultFontSize / 1.5;
                double last_pos = -0.4;
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains('#'))
                    {
                        string[] sections = new string[3];
                        int start = lines[i].IndexOf('#');
                        int stop = lines[i].IndexOf('@');
                        sections[0] = lines[i].Substring(start, stop - start).Trim();  //0 = transaction number
                        start = lines[i].IndexOf('>');    //Upside down
                        sections[1] = lines[i].Substring(stop + 1, start - stop - 1).Trim();   //1 = transaction start time
                        sections[2] = lines[i].Substring(start + 1).Trim().Replace(", ", Environment.NewLine); //Info
                        start = int.Parse(sections[1]);
                        transaction_starts.Points.Add(new ScatterPoint(start, -0.05, 5, sections[2].Contains("E:") ? 1 : -1));
                        (ChartControl as PlotView).Model.Annotations.Add(new ImageAnnotation()
                        {
                            ImageSource = DrawText(sections[0] + Environment.NewLine + sections[2], reduced_font),
                            X = new PlotLength(start, PlotLengthUnit.Data),
                            Y = new PlotLength(-0.55, PlotLengthUnit.Data)
                        });
                        last_pos = -0.55;
                    }
                    else   //Error descriptor line
                    {
                        string[] tmp = lines[i].Split('@').Select(x => x.Trim('\t', ' ')).ToArray();
                        double pos = (last_pos < -0.4) ? -0.4 : -0.5;
                        (ChartControl as PlotView).Model.Annotations.Add(new ImageAnnotation()
                        {
                            ImageSource = DrawText(tmp[0], reduced_font),
                            X = new PlotLength(int.Parse(tmp[1]), PlotLengthUnit.Data),
                            Y = new PlotLength(pos, PlotLengthUnit.Data)
                        });
                        last_pos = pos;
                    }
                }
                (ChartControl as PlotView).Model.Series.Add(transaction_starts);
                //Return data section
                return info;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during info section extraction: " + ex.Message);
                return "";
            }
        }
        internal string OneWireNameModifier(string Initial)
        {
            return Initial.Replace(Path.GetFileName(Initial),
                Path.GetFileNameWithoutExtension(Initial) + "_parsed" + Path.GetExtension(Initial));
        }

        internal static OxyImage DrawText(string text, double size)
        {
            Font f = new Font("Arial", (uint)size);
            Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);
            var sz = drawing.MeasureString(text, f);
            img.Dispose();
            drawing.Dispose();
            img = new Bitmap((int)sz.Width, (int)sz.Height);
            drawing = Graphics.FromImage(img);
            drawing.Clear(Color.Transparent);
            Brush textBrush = new SolidBrush(Color.White);
            drawing.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            drawing.DrawString(text, f, textBrush, 0, 0);
            drawing.Save();
            textBrush.Dispose();
            drawing.Dispose();
            var str = new MemoryStream();
            img.Save(str, System.Drawing.Imaging.ImageFormat.Png);
            img.Dispose();
            OxyImage res = new OxyImage(str.GetBuffer());
            str.Close();
            str.Dispose();
            return res;
        }
    }

    class JustPlot : Protocol
    {
        public static ProtocolName StaticName = new ProtocolName("JustPlot", "Plot only");
        public JustPlot() : base("")
        {
            Plotter = JustPlotter;
        }
        public override ProtocolName Name
        {
            get { return StaticName; }
        }
        public override string GetCodeDescription(int code)
        {
            return "N/A";
        }
        private bool JustPlotter(object ChartControl, object[] Data)
        {
            try
            {
                string[] lines;        
                lines = (Data[0] as string).Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                List<DataPoint> points = new List<DataPoint>(lines.Length);
                foreach (var item in lines)
                {
                    string[] tmp = item.Split(':');
                    if (tmp.Length > 1)
                    {       
                        int i, j;
                        bool b = int.TryParse(tmp[0], out i);
                        b = int.TryParse(tmp[1], out j) && b;
                        if (b) points.Add(new DataPoint(i, j));
                    }
                }
                var mySeries = new StairStepSeries();
                mySeries.Points.AddRange(points);
                mySeries.Color = OxyColor.FromRgb(((Color)(Data[2])).R, ((Color)(Data[2])).G, ((Color)(Data[2])).B);
                (ChartControl as PlotView).Model.Axes[0].MaximumRange = points.Last().X - points.First().X + 1;
                (ChartControl as PlotView).Model.Axes[0].AbsoluteMaximum = points.Last().X + 1;
                (ChartControl as PlotView).Model.Axes[0].AbsoluteMinimum = points.First().X - 1;
                (ChartControl as PlotView).Model.Series.Add(mySeries);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during plotting: " + ex.Message);
                return false;    
            }
            return true;
        }
        public override int InvokeParser(string path)
        {
            return 0;
        }
    }
    #endregion
}
