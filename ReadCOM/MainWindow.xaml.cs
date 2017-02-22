using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ReadCOM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SerialPort mySerialPort;

        //COM config robot.
        private SerialPort BluetoothConnection = new SerialPort();

        static private int[] dataMinimumValue = new int[15];// luu lai du lieu khi tay thang
        static private int[] dataMaximumValue = new int[15];// luu lai du lieu khi tay nam
        private int[] datafromglover = new int[15];
        private int Mode;//=0 run,=1 set min,=2 set max, = 3 control robot

        private DispatcherTimer timer;
        private DispatcherTimer timerAddText;
        private DispatcherTimer timerRobot;

        List<string> DataFromGlover4times;

        private double[,] distance;
        private double[,] f;

        private static string OldText = "";

        public MainWindow()
        {
            InitializeComponent();
            dataMinimumValue = new int[]{622,613,647,605,0,120,576,616,622,610,0,626,-40,34,-127};
            dataMaximumValue = new int[]{412,421,455,406,0,180,465,446,465,455,0,513,-36,43,-126};

            DataFromGlover4times = new List<string>();
            DataFromGlover4times.Add("0#0#0#0#0#0#0#0#0#0#0#0#0#0#0");
            DataFromGlover4times.Add("0#0#0#0#0#0#0#0#0#0#0#0#0#0#0");
            DataFromGlover4times.Add("0#0#0#0#0#0#0#0#0#0#0#0#0#0#0");
            DataFromGlover4times.Add("0#0#0#0#0#0#0#0#0#0#0#0#0#0#0");

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += timer_Tick;

            timerAddText = new DispatcherTimer();
            timerAddText.Interval = TimeSpan.FromMilliseconds(1000);
            timerAddText.Tick += timerAddText_Tick;

            timerRobot = new DispatcherTimer();
            timerRobot.Interval = TimeSpan.FromMilliseconds(10);
            timerRobot.Tick += timerRbot_Tick;
           
        }

        private void timerAddText_Tick(object sender, EventArgs e)
        {
            
            //throw new NotImplementedException();
            string TextNow = textBoxLetter.Text;
            if (TextNow == OldText && !TextNow.Equals('\0'.ToString()))
            {
                textBoxString.AppendText(TextNow);
            }

            OldText = TextNow;
        }

        

        /// <summary>
        /// ham cu, tam chua dung
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>    
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {

        }

        /// <summary>
        /// Ham dung ve 3D
        /// </summary>
        /// <param name="datainput"></param>
        private void ThreeDChange(string datainput)
        {
            string[] indatabuffer = datainput.Split('#');

            for (int i = 0; i < indatabuffer.Length; i++)
            {
                datafromglover[i] = Int32.Parse(indatabuffer[i]);
            }
            try
            {
                xMod.Dispatcher.Invoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                        new Action(() =>
                        {
                            if (dataMinimumValue[11] != 0)
                                xMod.Value = 90.00 * (datafromglover[11] - dataMinimumValue[11]) / (dataMaximumValue[11] - dataMinimumValue[11]);
                        }));

                yMod.Dispatcher.Invoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                        new Action(() =>
                        {
                            if (dataMinimumValue[1] != 0)
                                yMod.Value = 90.00 * (datafromglover[1] - dataMinimumValue[1]) / (dataMaximumValue[1] - dataMinimumValue[1]);
                        }));

                xMod2.Dispatcher.Invoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                        new Action(() =>
                        {
                            if (dataMinimumValue[7] != 0)
                                xMod2.Value = 90.00 * (datafromglover[7] - dataMinimumValue[7]) / (dataMaximumValue[7] - dataMinimumValue[7]);
                        }));

                yMod2.Dispatcher.Invoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                        new Action(() =>
                        {
                            if (dataMinimumValue[2] != 0)
                                yMod2.Value = 90.00 * (datafromglover[2] - dataMinimumValue[2]) / (dataMaximumValue[2] - dataMinimumValue[2]);
                        }));

                xMod3.Dispatcher.Invoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                        new Action(() =>
                        {
                            if (dataMinimumValue[8] != 0)
                                xMod3.Value = 90.00 * (datafromglover[8] - dataMinimumValue[8]) / (dataMaximumValue[8] - dataMinimumValue[8]);
                        }));

                yMod3.Dispatcher.Invoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                        new Action(() =>
                        {
                            if (dataMinimumValue[3] != 0)
                                yMod3.Value = 90.00 * (datafromglover[3] - dataMinimumValue[3]) / (dataMaximumValue[3] - dataMinimumValue[3]);
                        }));

                xMod4.Dispatcher.Invoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                        new Action(() =>
                        {
                            if (dataMinimumValue[9] != 0)
                                xMod4.Value = 90.00 * (datafromglover[9] - dataMinimumValue[9]) / (dataMaximumValue[9] - dataMinimumValue[9]);
                        }));

                yMod4.Dispatcher.Invoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                        new Action(() =>
                        {
                            if (dataMinimumValue[4] != 0)
                                yMod4.Value = 90.00 * (datafromglover[4] - dataMinimumValue[4]) / (dataMaximumValue[4] - dataMinimumValue[4]);
                        }));

                xMod5.Dispatcher.Invoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                        new Action(() =>
                        {
                            if (dataMinimumValue[10] != 0)
                                xMod5.Value = 90.00 * (datafromglover[10] - dataMinimumValue[10]) / (dataMaximumValue[10] - dataMinimumValue[10]);
                        }));

                yMod5.Dispatcher.Invoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                        new Action(() =>
                        {
                            if (dataMinimumValue[0] != 0)
                                yMod5.Value = 90.00 * (datafromglover[0] - dataMinimumValue[0]) / (dataMaximumValue[0] - dataMinimumValue[0]);
                        }));




                double Ax, Ay, Az;
                Ax = (datafromglover[12]);
                Ay = (datafromglover[13]);
                Az = (datafromglover[14]);
                //Ax = Ax / Math.Sqrt(Ax * Ax + Ay * Ay + Az * Az);
                //Ay = Ay / Math.Sqrt(Ax * Ax + Ay * Ay + Az * Az);
                //Az = Az / Math.Sqrt(Ax * Ax + Ay * Ay + Az * Az);

                //double CHuanHoa = Math.Sqrt(Ax * Ax + Ay * Ay + Az * Az);

                double Ax2, Ay2, Az2;
                Ax2 = (datafromglover[3]);
                Ay2 = (datafromglover[4]);
                Az2 = (datafromglover[5]);

                xCam.Dispatcher.Invoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                        new Action(() =>
                        {

                            if (Az < 0)
                                xCam.Value = -Ax * 0.6;
                            else
                            {
                                if (Ax < 0)
                                    xCam.Value = 180 + (Ax * 0.6);
                                else xCam.Value = -180 + (Ax * 0.6);
                            }

                        }));

                // goc xoay ngon cai, cai nay hoi dac biet ti
                // phai dua vao ca goc quay cua ca truc Ax nua
                zMod5.Dispatcher.Invoke(
                    System.Windows.Threading.DispatcherPriority.Normal,new Action(() =>
                        {
                            if (dataMinimumValue[5] != 0)
                            {
                                double tempData5 = 0.50 * (dataMaximumValue[5] + dataMinimumValue[5]);
                                double gocNgonCaiSolo = (90.00 * (datafromglover[5] - tempData5) / (dataMaximumValue[5] - tempData5));
                                zMod5.Value = gocNgonCaiSolo - (-180 + (Ax * 0.6));
                            }

                        }));

                zCam.Dispatcher.Invoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                        new Action(() =>
                        {
                            // bac tu do the truc nay bi a mat, boi vay se can den cai acclerometer thu 2 de thu nhan :D
                            zCam.Value = 0;
                        }));

                yCam.Dispatcher.Invoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                        new Action(() =>
                        {
                            if (Az < 0)
                                yCam.Value = -Ay * 0.6;
                            else
                            {
                                if (Ay < 0)
                                    yCam.Value = 180 + (Ay * 0.6);
                                else yCam.Value = -180 + (Ay * 0.6);
                            }
                        }));

            }
            catch(Exception _ex)
            {
                MessageBox.Show(_ex.ToString());
            }
        }
        


        /// <summary>
        /// Timer test
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            if (Mode == 0)
            {
                //string indataarray = mySerialPort.ReadBufferSize();
                string indata = mySerialPort.ReadLine();

                //string indataarray = mySerialPort.ReadExisting();
                //string[] indatastrings = indataarray.Split('\n');

                for (int i = 0; i < 3; i++)
                {
                    DataFromGlover4times[i] = DataFromGlover4times[i + 1];
                }

                DataFromGlover4times[3] = indata;

                Moniter.Dispatcher.Invoke(
                    System.Windows.Threading.DispatcherPriority.Normal, new Action(() => { Moniter.AppendText(indata); }));

                indata = indata.Replace("\r", "");

                ThreeDChange(indata);

                char KetQua = toLeter(DataFromGlover4times);

                textBoxLetter.Dispatcher.Invoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                        new Action(() => { textBoxLetter.Text = KetQua.ToString(); }));

            }
            else if (Mode == 1)
            {
                string indata = mySerialPort.ReadLine();

                Moniter.Dispatcher.Invoke(
                            System.Windows.Threading.DispatcherPriority.Normal,
                            new Action(() => { Moniter.AppendText(indata); }));

                indata = indata.Replace("\r", "");

                string[] indatabuffer = indata.Split('#');

                for (int i = 0; i < indatabuffer.Length; i++)
                {
                    dataMinimumValue[i] = Int32.Parse(indatabuffer[i]);
                }

                mySerialPort.Close();
                buttonOpen.Dispatcher.Invoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                            new Action(() => { buttonOpen.Content = "Open"; }));
            }
            else if (Mode == 2)
            {
                string indata = mySerialPort.ReadLine();

                Moniter.Dispatcher.Invoke(
                            System.Windows.Threading.DispatcherPriority.Normal,
                            new Action(() => { Moniter.AppendText(indata); }));

                indata = indata.Replace("\r", "");

                string[] indatabuffer = indata.Split('#');

                for (int i = 0; i < indatabuffer.Length; i++)
                {
                    dataMaximumValue[i] = Int32.Parse(indatabuffer[i]);
                }

                mySerialPort.Close();

                buttonOpen.Dispatcher.Invoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                            new Action(() => { buttonOpen.Content = "Open"; }));

            }

        }


        #region NhanDang
        /// <summary>
        /// dua va dau vao, tinh gia tri inpt se ung voi gia tri nao trong bang co san
        /// </summary>
        /// <param name="input">gia tri thu dc tu he thong cam bien</param>
        /// <returns></returns>
        private char toLeter(List<string> input)
        {
            DataCharacter datacharacter = new DataCharacter();

            double[] MangSaiSo = new double[datacharacter.listdataCha.Count];

            List<string> DataFromFile = new List<string>();
            
            
            for (int i = 0; i < datacharacter.listdataCha.Count; i++)
            {
                // lay tam 4 cai
                DataFromFile.Clear();

                string[] datatemp = datacharacter.listdataCha[i].Split(';');

                for (int j = 0; j < datatemp.Length; j++)
                {
                    DataFromFile.Add(datatemp[j]);
                    //DataFromFile.Add(datacharacter.listdataCha[i]);
                    //DataFromFile.Add(datacharacter.listdataCha[i]);
                    //DataFromFile.Add(datacharacter.listdataCha[i]);
                }
                MangSaiSo[i] = computerDynamic(input, DataFromFile);
            }

            int NhoNhat = 0;
            double SaisoNhoNhat = MangSaiSo[0];

            for (int i = 0; i < MangSaiSo.Length;i++ )
            {
                if (SaisoNhoNhat > MangSaiSo[i])
                {
                    SaisoNhoNhat = MangSaiSo[i];
                    NhoNhat = i;
                }
            }

            if (MangSaiSo[NhoNhat] < 200000)
                return datacharacter.returnCha(NhoNhat);
            else
                return '\0'; // day chinh la char rong
        }

        /// <summary>
        /// tinh tong do lech cua gia tri input voi gia tri data
        /// </summary>
        /// <param name="input">gia tri thu dc</param>
        /// <param name="Data">gia tri trong data</param>
        /// <returns></returns>
        private double KhoangCach(string _input, string _Data)
        {
            int[] input = Array.ConvertAll(_input.Split('#'), Int32.Parse);
            int[] Data = Array.ConvertAll(_Data.Split('#'), Int32.Parse);

            double SaiSo = 0;
            for (int i = 0; i < input.Length;i++ )
            {
                //if(Data[i]!=0)
                SaiSo = SaiSo + (1.0 / input.Length) * Math.Abs((Data[i] - input[i]));
            }

            //SaiSo = SaiSo / input.Length;

            return SaiSo;
        }

        public double min(double x,double y,double z)
        {
            double min = x;
            if (min > y) min = y;
            if (min > z) min = z;

            return min;
        }


        /// <summary>
        /// tim toan dong dung cho DTW
        /// </summary>
        /// <param name="m">chieu ngang</param>
        /// <param name="n">chieu doc</param>
        /// <returns></returns>
        public double computerDynamic(List<string> m_str, List<string> n_str)
        {

            int m = m_str.Count;
            int n = n_str.Count;


            //init cho quy hoach dong.
            distance = new double[m, n];
            f = new double[m + 1, n + 1];

            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    distance[i, j] = KhoangCach(m_str[i], n_str[j]);
                }
            }

            for (int i = 0; i <= m; ++i)
            {
                for (int j = 0; j <= n; ++j)
                {
                    f[i, j] = -1.0;
                }
            }

            //for (int i = 1; i <= m; ++i)
            //{
            //    f[i, 0] = double.PositiveInfinity;
            //}
            //for (int j = 1; j <= n; ++j)
            //{
            //    f[0, j] = double.PositiveInfinity;
            //}

            f[0, 0] = 0.0;

            // bat dau quy hoach dong
            f[0, 0] = distance[0, 0];

            for (int i = 1; i < m; i++ )
            {
                f[i, 0] = f[i - 1, 0] + distance[i, 0]; 
            }

            for (int j = 1; j < n; j++)
            {
                f[0, j] = f[0, j - 1] + distance[0, j];
            }

            for (int g = 1; g < n; g++)
            {
                for (int k = 1; k < m; k++)
                {

                    f[k, g] = min(f[k, g - 1], f[k - 1, g - 1], f[k - 1, g]) + distance[k, g];
                }
            }

            return f[m-1, n-1];
        }

        #endregion

        #region robot cotrol
        private void buttonOpenRobot_Click(object sender, RoutedEventArgs e)
        {
            this.buttonOpenRobot.IsEnabled = false;
            if (BluetoothConnection.IsOpen)
            {           
                BluetoothConnection.Close();
                this.buttonOpenRobot.Content = "Robot:Connect";
            }
            else
            {
                this.buttonOpenRobot.Content = "Robot:Disconnect";
                this.BluetoothConnection.PortName = this.comboBoxRobotPort.Text.Trim();
                BluetoothConnection.Open();
                BluetoothConnection.ReadTimeout = 1500;
            }
            this.buttonOpenRobot.IsEnabled = true;
        }

        private void NXTSendCommandAndGetReply(byte[] Command)
        {

            Byte[] MessageLength = { 0x00, 0x00 };

            MessageLength[0] = (byte)Command.Length;
            //this.textBox2.Text += "TX:";
            //for (int i = 0; i < Command.Length; i++)
                //this.textBox2.Text += Command[i].ToString("X2") + " ";
            //this.textBox2.Text += Environment.NewLine;
            //this.textBox2.Select(this.textBox2.Text.Length, 0);
            //this.textBox2.ScrollToCaret();

            BluetoothConnection.Write(MessageLength, 0, MessageLength.Length);
            BluetoothConnection.Write(Command, 0, Command.Length);
            int length = BluetoothConnection.ReadByte() + 256 * BluetoothConnection.ReadByte();
            //this.textBox2.Text += "RX:";
            //for (int i = 0; i < length; i++)
                //this.textBox2.Text += BluetoothConnection.ReadByte().ToString("X2") + " ";
            //this.textBox2.Text += Environment.NewLine;
            //this.textBox2.Select(this.textBox2.Text.Length, 0);
            //this.textBox2.ScrollToCaret();
        }

        private void timerRbot_Tick(object sender, EventArgs e)
        {
            string indata = mySerialPort.ReadLine();

            for (int i = 0; i < 3; i++)
            {
                DataFromGlover4times[i] = DataFromGlover4times[i + 1];
            }

            DataFromGlover4times[3] = indata;

            char KetQua = toLeter(DataFromGlover4times);

            textBoxLetter.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                           new Action(() => { textBoxLetter.Text = KetQua.ToString(); }));

            if (KetQua.Equals('^'))
            {
                string s = "up";
                byte[] NxtMessage = new byte[5 + s.Length];
                NxtMessage[0] = 0x00;
                NxtMessage[1] = 0x09;
                NxtMessage[2] = 0x00;
                NxtMessage[3] = (byte)(s.Length + 1);
                byte[] array = Encoding.ASCII.GetBytes(s);
                for (int ByteCtr = 0; ByteCtr < array.Length; ByteCtr++)
                {
                    NxtMessage[4 + ByteCtr] = array[ByteCtr];
                }
                NxtMessage[4 + s.Length] = 0x00;
                NXTSendCommandAndGetReply(NxtMessage);

            }
            else if (KetQua.Equals('*'))
            {
                string s = "down";
                byte[] NxtMessage = new byte[5 + s.Length];
                NxtMessage[0] = 0x00;
                NxtMessage[1] = 0x09;
                NxtMessage[2] = 0x00;
                NxtMessage[3] = (byte)(s.Length + 1);
                byte[] array = Encoding.ASCII.GetBytes(s);
                for (int ByteCtr = 0; ByteCtr < array.Length; ByteCtr++)
                {
                    NxtMessage[4 + ByteCtr] = array[ByteCtr];
                }
                NxtMessage[4 + s.Length] = 0x00;
                NXTSendCommandAndGetReply(NxtMessage);
            }
            else if (KetQua.Equals('<'))
            {
                string s = "left";
                byte[] NxtMessage = new byte[5 + s.Length];
                NxtMessage[0] = 0x00;
                NxtMessage[1] = 0x09;
                NxtMessage[2] = 0x00;
                NxtMessage[3] = (byte)(s.Length + 1);
                byte[] array = Encoding.ASCII.GetBytes(s);
                for (int ByteCtr = 0; ByteCtr < array.Length; ByteCtr++)
                {
                    NxtMessage[4 + ByteCtr] = array[ByteCtr];
                }
                NxtMessage[4 + s.Length] = 0x00;
                NXTSendCommandAndGetReply(NxtMessage);
            }
            else if (KetQua.Equals('>'))
            {
                string s = "right";
                byte[] NxtMessage = new byte[5 + s.Length];
                NxtMessage[0] = 0x00;
                NxtMessage[1] = 0x09;
                NxtMessage[2] = 0x00;
                NxtMessage[3] = (byte)(s.Length + 1);
                byte[] array = Encoding.ASCII.GetBytes(s);
                for (int ByteCtr = 0; ByteCtr < array.Length; ByteCtr++)
                {
                    NxtMessage[4 + ByteCtr] = array[ByteCtr];
                }
                NxtMessage[4 + s.Length] = 0x00;
                NXTSendCommandAndGetReply(NxtMessage);
            }
            else
            {
                // do nothing
            }
        }

        private void buttonRunRobot_Click(object sender, RoutedEventArgs e)
        {
            timerRobot.Start();
        }
       
        #endregion

        #region From Event

        private void buttonOpen_Click(object sender, RoutedEventArgs e)
        {
            if (buttonOpen.Content.Equals("Open"))
            {
                try
                {
                    // khong dc dat thuoc tinh cong trong ham khai bao, neu ko se ko chay
                    //cha hieu tai sao
                    mySerialPort = new SerialPort();
                    mySerialPort.PortName = comboBoxNameCOM.Text;
                    mySerialPort.BaudRate = Int32.Parse(comboBoxBaund.Text);
                    mySerialPort.Handshake = Handshake.None;
                    mySerialPort.Parity = Parity.None;
                    mySerialPort.DataBits = Int32.Parse(comboBoxDataSize.Text);
                    mySerialPort.StopBits = StopBits.One;
                    mySerialPort.RtsEnable = true;
                    mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                    mySerialPort.Open();
                    mySerialPort.DiscardInBuffer();
                    Moniter.Dispatcher.Invoke(
                        System.Windows.Threading.DispatcherPriority.Normal,
                        new Action(() => { Moniter.Text = comboBoxNameCOM.Text + " is open\n"; }));
                    buttonOpen.Content = "Close";

                    timer.Start();
                    timerAddText.Start();
                }
                catch (Exception _ex)
                {
                    Moniter.Text = _ex.ToString();
                }

            }
            else
            {
                timer.Stop();
                timerAddText.Stop();
                mySerialPort.Close();
                buttonOpen.Content = "Open";
            }

        }

        private void Moniter_TextChanged(object sender, TextChangedEventArgs e)
        {
            Moniter.ScrollToEnd();
        }

        private void butonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void comboBoxMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Mode = comboBoxMode.SelectedIndex;
        }

        #endregion
    }
}
