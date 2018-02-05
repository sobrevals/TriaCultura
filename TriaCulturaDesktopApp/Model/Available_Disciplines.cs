using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriaCulturaDesktopApp.Model
{
    public class Available_Disciplines
    {
        triaculturaCTXEntities context = new triaculturaCTXEntities();
        public List<discipline> disciplines { get { return context.disciplines.Distinct().ToList(); } }
    }
}
