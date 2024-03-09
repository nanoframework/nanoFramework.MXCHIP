//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using Iot.Device.Hts221;
using Iot.Device.Lis2Mdl;
using Iot.Device.Lps22Hb;
using Iot.Device.RgbDiode;
using Iot.Device.Ssd13xx;
using System.Device.Gpio;
using System.Device.I2c;

namespace nanoFramework.MxChip
{
    /// <summary>
    /// Class for the MXChip AZ3166 board.
    /// </summary>
    public class Az3166
    {
        private GpioController _gpioController;

        private static GpioPin _buttonA;
        private static GpioPin _buttonB;
        private static GpioPin _ledBlue;
        private static GpioPin _ledGreen;
        private static GpioPin _ledYellow;
        private static Lis2Mdl _magnetometer;
        private static Lps22Hb _barometer;
        private static Hts221 _tempAndHum;
        private static RgbDiode _rgb;
        private static Ssd1306 _oledDisplay;

        // GPIOs
        // Button A @ PA4
        private const int _gpioButtonA = 4;
        // Button B @ PA10
        private const int _gpioButtonB = 10;
        // Red LED @ PA15
        private const int _gpioLedBlue = 15;
        // Green LED @ PB2
        private const int _gpioLedGreen = 1 * 16 + 2;
        // Yellow LED @ PC13
        private const int _gpioLedYellow = 2 * 16 + 13;
        // PWM1 for RGB LED @ PB3
        private const int _rgbGreen = 1 * 16 + 3;
        // PWM2 for RGB LED @ PB4
        private const int _rgbRed = 1 * 16 + 4;
        // PWM3 for RGB LED @ PC7
        private const int _rgbBlue = 2 * 16 + 7;

        /// <summary>
        /// Button A.
        /// </summary>
        public GpioPin ButtonA => _buttonA;

        /// <summary>
        /// Button B.
        /// </summary>
        public GpioPin ButtonB => _buttonB;

        /// <summary>
        /// Lis2Mdl magnetoneter.
        /// </summary>
        public Lis2Mdl Magnetometer => _magnetometer;

        /// <summary>
        /// Lps22Hb barometer.
        /// </summary>
        public Lps22Hb Barometer => _barometer;

        /// <summary>
        /// OLED display.
        /// </summary>
        public Ssd1306 OledDisplay => _oledDisplay;

        /// <summary>
        /// HTS221 temperature and humidity sensor.
        /// </summary>
        public Hts221 TempAndHum => _tempAndHum;

        /// <summary>
        /// RGB LED.
        /// </summary>
        public RgbDiode RgbLed => _rgb;

        /// <summary>
        /// Blue LED.
        /// </summary>
        /// <remarks>
        /// Marked as Azure LED on the board.
        /// </remarks>
        public GpioPin LedBlue => _ledBlue;

        /// <summary>
        /// Green LED.
        /// </summary>
        /// <remarks>
        /// Marked as Wi-Fi LED on the board.
        /// </remarks>
        public GpioPin LedGreen => _ledGreen;

        /// <summary>
        /// Yellow LED.
        /// </summary>
        /// <remarks>
        /// Marked as User LED on the board.
        /// </remarks>
        public GpioPin LedYellow => _ledYellow;

        /// <summary>
        /// Class
        /// </summary>
        public Az3166()
        {
            // init and config GPIOs
            _gpioController = new GpioController();

            _buttonA = _gpioController.OpenPin(_gpioButtonA, PinMode.Input);
            _buttonB = _gpioController.OpenPin(_gpioButtonB, PinMode.Input);
            _ledBlue = _gpioController.OpenPin(_gpioLedBlue, PinMode.Output);
            _ledGreen = _gpioController.OpenPin(_gpioLedGreen, PinMode.Output);
            _ledYellow = _gpioController.OpenPin(_gpioLedYellow, PinMode.Output);

            // init sensors
            _magnetometer = new Lis2Mdl(CreateLis2MdlDevice());
            _barometer = new Lps22Hb(
                CreateLps22HbDevice(),
                FifoMode.Bypass);
            _tempAndHum = new Hts221(CreateHts221Device());

            // RGB LED
            _rgb = new RgbDiode(_rgbRed, _rgbGreen, _rgbBlue);
            // turn off RGB LED
            _rgb.SetColor(0, 0, 0);

            // init OLED screen
            _oledDisplay = CreateOledDisplay();
            _oledDisplay.ClearScreen();
            _oledDisplay.Font = new BasicFont();
        }

        private I2cDevice CreateHts221Device()
        {
            I2cConnectionSettings settings = new(1, Hts221.DefaultI2cAddress);
            return I2cDevice.Create(settings);
        }

        private Ssd1306 CreateOledDisplay()
        {
            I2cConnectionSettings settings = new(1, Ssd1306.DefaultI2cAddress);
            return new Ssd1306(
                I2cDevice.Create(settings),
                Ssd13xx.DisplayResolution.OLED128x64);
        }

        private I2cDevice CreateLis2MdlDevice()
        {
            I2cConnectionSettings settings = new(1, Lis2Mdl.DefaultI2cAddress);
            return I2cDevice.Create(settings);
        }

        I2cDevice CreateLps22HbDevice()
        {
            // using address 0x5C
            I2cConnectionSettings settings = new(1, 0b1011100);
            return I2cDevice.Create(settings);
        }
    }
}
