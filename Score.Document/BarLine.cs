using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Score.Document
{
    public class BarLine : StaffSystemElement
    {
        public BarLineStyle Style;
    }

    public enum BarLineStyle
    {
        Single,
        Double,
        End,
        BeginRepeat,
        EndRepeat,
        BeginEndRepeat
    }
}
