using nanoFramework.MxChip;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace AZ3166_Sample
{
    public class Program
    {
        private static Az3166 _iotDevice;
        public static void Main()
        {

           _iotDevice = new Az3166();

            _iotDevice.OledDisplay.DrawString(2, 2, "IoT", 2, true);
            Thread.Sleep(1000);

            _iotDevice.OledDisplay.DrawString(1, 32, "nanoFramework", 1, true);
            _iotDevice.OledDisplay.DrawString(1, 44, "ROCKS!", 1, true);
            _iotDevice.OledDisplay.Display();
            Thread.Sleep(150);

            _iotDevice.OledDisplay.DrawString(1, 44, "      ", 1, true);
            _iotDevice.OledDisplay.Display();
            Thread.Sleep(150);

            _iotDevice.OledDisplay.DrawString(3, 44, "ROCKS!", 1, true);
            _iotDevice.OledDisplay.Display();
            Thread.Sleep(150);

            _iotDevice.OledDisplay.DrawString(1, 44, "      ", 1, true);
            _iotDevice.OledDisplay.Display();
            Thread.Sleep(150);

            _iotDevice.OledDisplay.DrawString(1, 44, "ROCKS!", 1, true);
            _iotDevice.OledDisplay.Display();

            Thread.Sleep(1000);

            _iotDevice.ButtonB.DebounceTimeout = TimeSpan.FromMilliseconds(50);
            _iotDevice.ButtonB.ValueChanged += (s, e) =>
            {
                if(e.ChangeType == System.Device.Gpio.PinEventTypes.Falling)
                {
                    _iotDevice.LedYellow.Toggle();

                    _iotDevice.OledDisplay.ClearScreen();
                    _iotDevice.OledDisplay.DrawString(1, 20, $"Temp: {_iotDevice.Barometer.Temperature.DegreesCelsius:F1}C", 1);
                    _iotDevice.OledDisplay.DrawString(1, 32, $"Pres: {_iotDevice.Barometer.Pressure.Hectopascals:F0}hPa", 1);
                    _iotDevice.OledDisplay.Display();

                    Thread.Sleep(3000);
                    _iotDevice.LedYellow.Toggle();

                    RestoreScreen();
                }
            };

            _iotDevice.ButtonA.DebounceTimeout = TimeSpan.FromMilliseconds(50);
            _iotDevice.ButtonA.ValueChanged += (s, e) =>
            {
                if (e.ChangeType == System.Device.Gpio.PinEventTypes.Falling)
                {
                    // demo RGB
                    Thread.Sleep(1000);
                    _iotDevice.RgbLed.SetColor(255, 0, 0);
                    Thread.Sleep(250);
                    _iotDevice.RgbLed.SetColor(0, 255, 0);
                    Thread.Sleep(250);
                    _iotDevice.RgbLed.SetColor(0, 0, 255);
                    Thread.Sleep(250);

                    _iotDevice.RgbLed.SetColor(Color.LawnGreen);
                    Thread.Sleep(250);
                    _iotDevice.RgbLed.SetColor(Color.OrangeRed);
                    Thread.Sleep(250);
                    _iotDevice.RgbLed.SetColor(Color.Teal);
                    Thread.Sleep(500);

                    _iotDevice.RgbLed.Transition(Color.FromArgb(0, 255, 0));
                    _iotDevice.RgbLed.Transition(Color.FromArgb(255, 0, 0));

                    Thread.Sleep(1000);
                    _iotDevice.RgbLed.SetColor(0, 0, 0);
                }
            };

            Thread.Sleep(Timeout.Infinite);
        }

        // restore main "screen"
        private static void RestoreScreen()
        {
            _iotDevice.OledDisplay.ClearScreen();

            _iotDevice.OledDisplay.DrawString(2, 2, "IoT", 2, true);
            _iotDevice.OledDisplay.DrawString(1, 32, "nanoFramework", 1, true);
            _iotDevice.OledDisplay.DrawString(1, 44, "ROCKS!", 1, true);
            _iotDevice.OledDisplay.Display();
        }
    }
}
