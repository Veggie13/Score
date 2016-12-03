using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Score.Document
{
    public class KeySignature : StaffSystemElement
    {
        public class Item
        {
            public int StaffIndex;
            public int Line;
            public NoteModifier Modifier;
        }

        public Item[] Items;
    }
}
