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

namespace IK_kolko_krzyzyk_2
{
    public partial class Form1 : Form
    {
        string[] kik = { "", "", "", "", "", "", "", "", "", "0", "0", "0" };
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

        private void port_DataReceived_1(object sender, SerialDataReceivedEventArgs e)
        {
            string InputData = serialPort1.ReadExisting();

            kik = InputData.Split(',');

            this.BeginInvoke(new Action(() =>

            {

                label3.Text = "X: " + kik[10].ToString();
                label4.Text = "O: " + kik[11].ToString();

                switch (kik[9])
                {
                    case "1":
                        label5.Text = "Lose!";
                        label5.Visible = true;

                        for (int i = 0; i < buttons.Count; i++)
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
                        break;
                    case "2":
                        label5.Text = "Win!";
                        label5.Visible = true;

                        for (int i = 0; i < buttons.Count; i++)
                        {
                            buttons[i].Text = kik[i];
                            buttons[i].Enabled = false;
                        }
                        break;
                    case "3":
                        label5.Text = "tie(fighter)";
                        label5.Visible = true;

                        for (int i = 0; i < buttons.Count; i++)
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
                        break;
                    case "0":
                        label5.Visible = false;

                        for (int i = 0; i < buttons.Count; i++)
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
                        break;
                }
            } ));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Connect")
            {
                serialPort1.PortName = "COM2";
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
        }

        private void button3_Click(object sender, EventArgs e)
        {
            send(1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            send(2);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            send(3);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            send(4);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            send(5);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            send(6);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            send(7);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            send(8);
        }

        private void send(int nrButtons)
        {
            if(nrButtons == 15)// the bts r enabled for playerO
            {
                for (int i = 0; i < buttons.Count; i++)
                {
                    buttons[i].Enabled = true;
                }
            }
            else if (nrButtons == 14)//sending info after dc
            {
                for (int i = 0; i < buttons.Count; i++)
                {
                    buttons[i].Enabled = false;
                }
            }
            else if (nrButtons != 15)//usual info after move from player O
            {
                kik[nrButtons] = "O";
                buttons[nrButtons].Text = "O";

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
                //textBox1.Clear();
                //textBox1.Text = string.Join(",", kik);
            }
        }
    }
}
