using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriaCulturaDesktopApp.Model;

namespace TriaCulturaDesktopApp.ViewModel
{
    class DisciplinaViewModel : INotifyPropertyChanged
    {
        triaculturaEntities context = new Model.triaculturaEntities();

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
