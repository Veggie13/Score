using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Score.Document
{
    public class Clef : StaffElement
    {
        public ClefStyle Style;
        public int Line;
        public int OctaveShift;
    }

    public enum ClefStyle { G, F, C }
}
