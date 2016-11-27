using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Diagnostics;
using Midi;

namespace Test
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var outDevice = OutputDevice.InstalledDevices[0];
            outDevice.Open();
            var defKeyMap = new Dictionary<Key, Pitch>
            {
                { Key.Q, Pitch.C3 },
                { Key.D2, Pitch.CSharp3 },
                { Key.W, Pitch.D3 },
                { Key.D3, Pitch.DSharp3 },
                { Key.E, Pitch.E3 },
                { Key.R, Pitch.F3 },
                { Key.D5, Pitch.FSharp3 },
                { Key.T, Pitch.G3 },
                { Key.D6, Pitch.GSharp3 },
                { Key.Y, Pitch.A3 },
                { Key.D7, Pitch.ASharp3 },
                { Key.U, Pitch.B3 },
                { Key.I, Pitch.C4 },
                { Key.D9, Pitch.CSharp4 },
                { Key.O, Pitch.D4 },
                { Key.D0, Pitch.DSharp4 },
                { Key.P, Pitch.E4 },
                { Key.OemOpenBrackets, Pitch.F4 },
                { Key.OemPlus, Pitch.FSharp4 },
                { Key.OemCloseBrackets, Pitch.G4 },

                { Key.Z, Pitch.C4 },
                { Key.S, Pitch.CSharp4 },
                { Key.X, Pitch.D4 },
                { Key.D, Pitch.DSharp4 },
                { Key.C, Pitch.E4 },
                { Key.V, Pitch.F4 },
                { Key.G, Pitch.FSharp4 },
                { Key.B, Pitch.G4 },
                { Key.H, Pitch.GSharp4 },
                { Key.N, Pitch.A4 },
                { Key.J, Pitch.ASharp4 },
                { Key.M, Pitch.B4 },
                { Key.OemComma, Pitch.C5 },
                { Key.L, Pitch.CSharp5 },
                { Key.OemPeriod, Pitch.D5 },
                { Key.OemSemicolon, Pitch.DSharp5 },
                { Key.OemQuestion, Pitch.E5 }
            };
            var shiftKeyMap = new Dictionary<Key, Pitch>
            {
                { Key.Q, Pitch.C2 },
                { Key.D2, Pitch.CSharp2 },
                { Key.W, Pitch.D2 },
                { Key.D3, Pitch.DSharp2 },
                { Key.E, Pitch.E2 },
                { Key.R, Pitch.F2 },
                { Key.D5, Pitch.FSharp2 },
                { Key.T, Pitch.G2 },
                { Key.D6, Pitch.GSharp2 },
                { Key.Y, Pitch.A2 },
                { Key.D7, Pitch.ASharp2 },
                { Key.U, Pitch.B2 },
                { Key.I, Pitch.C3 },
                { Key.D9, Pitch.CSharp3 },
                { Key.O, Pitch.D3 },
                { Key.D0, Pitch.DSharp3 },
                { Key.P, Pitch.E3 },
                { Key.OemOpenBrackets, Pitch.F3 },
                { Key.OemPlus, Pitch.FSharp3 },
                { Key.OemCloseBrackets, Pitch.G3 },

                { Key.Z, Pitch.C5 },
                { Key.S, Pitch.CSharp5 },
                { Key.X, Pitch.D5 },
                { Key.D, Pitch.DSharp5 },
                { Key.C, Pitch.E5 },
                { Key.V, Pitch.F5 },
                { Key.G, Pitch.FSharp5 },
                { Key.B, Pitch.G5 },
                { Key.H, Pitch.GSharp5 },
                { Key.N, Pitch.A5 },
                { Key.J, Pitch.ASharp5 },
                { Key.M, Pitch.B5 },
                { Key.OemComma, Pitch.C6 },
                { Key.L, Pitch.CSharp6 },
                { Key.OemPeriod, Pitch.D6 },
                { Key.OemSemicolon, Pitch.DSharp6 },
                { Key.OemQuestion, Pitch.E6 }
            };

            bool metronome = false;
            double bpm = 132 / 2f;
            long mspb = (long)Math.Round(60000 / bpm);
            Stopwatch watch = new Stopwatch();
            long beat = 0;

            var active = new HashSet<Key>();
            bool shifted = false;
            watch.Start();
            while (true)
            {
                bool newShifted = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift) || Keyboard.IsKeyToggled(Key.CapsLock);
                if (newShifted && !shifted)
                {
                    foreach (var key in defKeyMap.Keys)
                    {
                        outDevice.SendNoteOff(Channel.Channel1, defKeyMap[key], 127);
                    }
                    active.Clear();
                }
                else if (!newShifted && shifted)
                {
                    foreach (var key in shiftKeyMap.Keys)
                    {
                        outDevice.SendNoteOff(Channel.Channel1, shiftKeyMap[key], 127);
                    }
                    active.Clear();
                }
                shifted = newShifted;

                var keyMap = shifted ? shiftKeyMap : defKeyMap;
                var allActive = keyMap.Keys.Where(k => Keyboard.IsKeyDown(k));

                var inactive = active.Intersect(keyMap.Keys.Except(allActive)).ToList();
                foreach (var key in inactive)
                {
                    outDevice.SendNoteOff(Channel.Channel1, keyMap[key], 127);
                }
                active.ExceptWith(inactive);

                var newActive = allActive.Except(active).ToList();
                foreach (var key in newActive)
                {
                    outDevice.SendNoteOn(Channel.Channel1, keyMap[key], 127);
                }
                active.UnionWith(newActive);

                if (Keyboard.IsKeyDown(Key.F1))
                {
                    metronome = true;
                }
                if (Keyboard.IsKeyDown(Key.F2))
                {
                    metronome = false;
                }

                if (metronome && watch.ElapsedMilliseconds / mspb > beat)
                {
                    beat++;
                    outDevice.SendPercussion(Percussion.ClosedHiHat, 127);
                }
            }
        }
    }
}
