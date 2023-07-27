using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;




namespace IK_kolko_krzyzyk_1
{
    public partial class Form1 : Form
    {
        string[] kik = { "", "", "", "", "", "", "", "", "", "0", "0", "0" };
        string[] kikMemory = { "", "", "", "", "", "", "", "", "", "0", "0", "0" };
        string turnMemory = "XO";
        List<Button> buttons;

        public Form1()
        {
            InitializeComponent();
            serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(port_DataReceived_1);

            buttons = new List<Button>()
                {
                    button2,button3,button4,button5,button6,button7,button8,button9,button10
                };
        }

        private void port_DataReceived_1(object sender, SerialDataReceivedEventArgs eventArgs)
        {

            string InputData = serialPort1.ReadExisting();

            kik = InputData.Split(',');


            textBox1.BeginInvoke(new Action(() =>
            {
                textBox2.Clear();
                textBox2.Text = InputData;

                for (int i = 0; i < buttons.Count; i++)// if the kik is cler dont du tis
                {
                    buttons[i].Text = kik[i];

                    if (buttons[i].Text != "")
                    {
                        buttons[i].Enabled = false;
                    }
                    else
                    {
                        buttons[i].Enabled = true;
                    }
                }

                label3.Text = "X: " + kik[10];
                label4.Text = "O: " + kik[11];

                checkIfWin();

                if (kik[9] == "0")
                {
                    label5.Visible = false;
                }
            }));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Connect")
            {
                serialPort1.PortName = "COM1";
                serialPort1.BaudRate = 600;
                serialPort1.DataBits = Convert.ToInt16(5);
                serialPort1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), "One");
                serialPort1.Handshake = (Handshake)Enum.Parse(typeof(Handshake), "None");
                try
                {
                    serialPort1.Open();

                    for (int i = 0; i < buttons.Count; i++)
                    {
                        buttons[i].Enabled = true;
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    MessageBox.Show(ex.Message);
                }

                button1.Text = "Connected";
                button1.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            send(0);
            checkIfWin();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            send(1);
            checkIfWin();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            send(2);
            checkIfWin();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            send(3);
            checkIfWin();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            send(4);
            checkIfWin();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            send(5);
            checkIfWin();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            send(6);
            checkIfWin();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            send(7);
            checkIfWin();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            send(8);
            checkIfWin();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            resetGame();
            send(15);
        }

        private void send(int nrButtons)
        {
            

            if (nrButtons == 15)//winO, tie or just unlocking buttons at the start info (the buttons r enabled for player X)
            {
                for (int i = 0; i < buttons.Count; i++)
                {
                    buttons[i].Enabled = true;
                }
            }
            else if (nrButtons == 14)//winX info (the buttons r disabled for player X)
            {
                for (int i = 0; i < buttons.Count; i++)
                {
                    buttons[i].Enabled = false;
                }
            }
            else if (nrButtons != 15)//usual info after move from player X
            {
                kik[nrButtons] = "X";
                buttons[nrButtons].Text = "X";

                for (int i = 0; i < buttons.Count; i++)
                {
                    buttons[i].Enabled = false;
                }
                kik[9] = "0";
                label5.Visible = false;
            }

            if (serialPort1.IsOpen)
            {
                serialPort1.Write(string.Join(",", kik));
                textBox1.Clear();
                textBox1.Text = string.Join(",", kik);
            }
        }

        private void checkIfWin()
        {
            string player = "X";

            if ((kik[0] == player && kik[1] == player && kik[2] == player)
                || (kik[3] == player && kik[4] == player && kik[5] == player)
                || (kik[6] == player && kik[7] == player && kik[8] == player)
                || (kik[0] == player && kik[3] == player && kik[6] == player)
                || (kik[1] == player && kik[4] == player && kik[7] == player)
                || (kik[2] == player && kik[5] == player && kik[8] == player)
                || (kik[0] == player && kik[4] == player && kik[8] == player)
                || (kik[2] == player && kik[4] == player && kik[6] == player))
            {
                kik[9] = "1";
                int x = int.Parse(kik[10]);
                x++;
                kik[10] = x.ToString();

                label3.Text = "X: " + kik[10];
                label4.Text = "O: " + kik[11];
                label5.Text = "Win!";
                label5.Visible = true;

                resetTurn();
                send(14);
                return;
            }

            player = "O";

            if ((kik[0] == player && kik[1] == player && kik[2] == player)
            || (kik[3] == player && kik[4] == player && kik[5] == player)
            || (kik[6] == player && kik[7] == player && kik[8] == player)
            || (kik[0] == player && kik[3] == player && kik[6] == player)
            || (kik[1] == player && kik[4] == player && kik[7] == player)
            || (kik[2] == player && kik[5] == player && kik[8] == player)
            || (kik[0] == player && kik[4] == player && kik[8] == player)
            || (kik[2] == player && kik[4] == player && kik[6] == player))
            {
                kik[9] = "2";
                int x = int.Parse(kik[11]);
                x++;
                kik[11] = x.ToString();

                label3.Text = "X: " + kik[10];
                label4.Text = "O: " + kik[11];
                label5.Text = "Lose!";
                label5.Visible = true;

                resetTurn();
                send(15);
                return;
            }
            if (kik[0] != "" && kik[1] != "" && kik[2] != "" && kik[3] != "" && kik[4] != "" && kik[5] != "" && kik[6] != "" && kik[7] != "" && kik[8] != "")
            {
                kik[9] = "3";

                label3.Text = "X: " + kik[10];
                label4.Text = "O: " + kik[11];
                label5.Text = "tie(fighter)";
                label5.Visible = true;

                resetTurn();
                send(15);
            }
        }

        private void resetTurn()
        {
            for (int i = 0; i < 9; i++)
            {
                kik[i] = "";
                buttons[i].Text = "";
            }
        }

        private void resetGame()
        {
            for (int i = 0; i < 9; i++)
            {
                kik[i] = "";
                buttons[i].Text = kik[i];
            }
            kik[9] = "0";
            kik[10] = "0";
            kik[11] = "0";

            label5.Visible = false;

            label3.Text = "X: " + kik[10];
            label4.Text = "O: " + kik[11];
        }
    }
}
