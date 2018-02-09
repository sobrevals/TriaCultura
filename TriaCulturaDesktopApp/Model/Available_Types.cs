using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriaCulturaDesktopApp.Model
{
    public class Available_Types
    {
        public List<string> types
        {
            get
            {
                return new List<string>(new string[] { "Exposició", "Film", "Concert", "Teatre", "Performance", "Circ", "Cercavila" });
            }
        }
    }
}

