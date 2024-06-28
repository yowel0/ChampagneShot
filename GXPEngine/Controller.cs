using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Printing;
using System.Security;
using System.ComponentModel;

namespace GXPEngine
{
    internal class Controller : GameObject
    {
        SerialPort port;
        char[] seperators;
        string[] inputs;
        float[] values;

        public float yaw;
        public float pitch;
        public float roll;
        public int button;
        int oldButton;
        public float acceleration;

        public bool buttonHELD;
        public bool buttonUP;
        public bool buttonDOWN;

        // quick (and ugly?)
        public static Controller main;


        public Controller() : base()
        {
            main = this; // improve!

            seperators = new[] { ',','\r' ,'\n'};
            port = new SerialPort();
            port.PortName = "COM3";
            port.BaudRate = 9600;
            port.RtsEnable = true;
            port.DtrEnable = true;
            
            //port.Open();

            values = new float[6]; // for efficiency: once, here
        }

        void Update()
        {
            string a = port.ReadExisting();
            if (a != "")
            {
                //Console.WriteLine(a);
                inputs = a.Split(seperators,StringSplitOptions.RemoveEmptyEntries);
                bool fail = false;
                //Console.WriteLine(inputs.Length);
                for (int i = 0; i < inputs.Length; i++)
                {
                    if (i < values.Length)
                    {
                        if (float.TryParse(inputs[i], out float floatValue))
                        {
                            values[i] = floatValue;
                        }

                        fail = true;
                        if (inputs.Length == 5 || inputs.Length == 10)
                        {
                            fail = false;
                        }
                    }
                }
                if (!fail)
                {
                    yaw = values[0];
                    pitch = values[1];
                    roll = values[2];
                    //Console.WriteLine("Pitch: " + pitch);
                    //Console.WriteLine("Yaw  : " + yaw);
                    button = (int)values[3];
                    acceleration = values[4];
                }
            }
            else
            {
                Console.WriteLine("EMPTY");
            }
            //Console.WriteLine("newframe");
            CheckButtonState();
        }

        void CheckButtonState()
        {
            if (button == 1 && oldButton == 1)
            {
                buttonHELD = true;
                buttonUP = false;
                buttonDOWN = false;
            }
            if (button > oldButton)
            {
                buttonHELD = false;
                buttonUP = false;
                buttonDOWN = true;
            }
            if (button < oldButton)
            {
                buttonHELD = false;
                buttonUP = true;
                buttonDOWN = false;
            }
            if (button == 0 && oldButton == 0)
            {
                buttonHELD = false;
                buttonUP = false;
                buttonDOWN = false;
            }

            oldButton = button;
        }
    }
}
