using MvvmDialogs.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriaCulturaDesktopApp.ViewModel
{
    class Esborrar_AutorBuitDialogViewModel : INotifyPropertyChanged, IUserDialogViewModel
    {
        public bool IsModal
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public event EventHandler DialogClosing;
        public event PropertyChangedEventHandler PropertyChanged;

        public void RequestClose()
        {
            throw new NotImplementedException();
        }
    }
}
