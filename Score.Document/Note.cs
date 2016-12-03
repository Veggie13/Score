using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Score.Document
{
    public class Note : StaffElement
    {
        public int BaseLength;
        public int DotCount;
        public NoteStyle Style;
        public StemStyle StemStyle;
        public int Tuplet;
    }

    public enum NoteStyle
    {
        Round,
        Drum,
        Cymbal,
        Block
    }

    public enum StemStyle
    {
        Up,
        Down,
        UpTied,
        DownTied
    }
}
