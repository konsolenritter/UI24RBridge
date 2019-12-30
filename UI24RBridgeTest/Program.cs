﻿using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using UI24RController;
using UI24RController.MIDIController;

namespace UI24RBridgeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create Bridge Object");
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();
            var address = configuration["UI24R-Url"];
            var midiInputDevice = configuration["MIDI-Input-Name"];
            var midiOutputDevice = configuration["MIDI-Output-Name"];
            //var controller = new BehringerUniversalMIDI();
            var controller = new MackieHUI();
            if (midiInputDevice == null || midiOutputDevice == null)
            {
                if (midiInputDevice == null)
                    Console.WriteLine("The input device name is mandantory in the config file. (MIDI-Input-Name)");
                if (midiOutputDevice == null)
                    Console.WriteLine("The ouput device name is mandantory in the config file. (MIDI-Output-Name)");
                WriteMIDIDeviceNames(controller);
            }
            else
            {
                controller.ConnectInputDevice(midiInputDevice);
                controller.ConnectOutputDevice(midiOutputDevice);

            }

            controller._messageReceived += (obj, e) => {
                Console.WriteLine(e.Message);
            };
            Action<string> messageWriter = (string message) =>
            {
                //Console.WriteLine(message);
            };
            using (UI24RBridge bridge = new UI24RBridge(address, controller, messageWriter))
            {
                while (!Console.KeyAvailable)
                {
                }
            }

        }

        /// <summary>
        /// Write the MIDI device names to the console. It help to set the config file.
        /// </summary>
        /// <param name="controller"></param>
        private static void WriteMIDIDeviceNames(IMIDIController controller)
        {
            var inputDevicenames = controller.GetInputDeviceNames();
            foreach (var inputDevice in inputDevicenames)
            {
                Console.WriteLine($"Input device name: {inputDevice}");
            }

            var outputDevicenames = controller.GetOutputDeviceNames();
            foreach (var outputDevice in outputDevicenames)
            {
                Console.WriteLine($"Output device name: {outputDevice}");
            }
            Console.WriteLine();
        }
    }
}