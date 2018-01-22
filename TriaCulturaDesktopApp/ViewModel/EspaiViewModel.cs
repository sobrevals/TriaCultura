using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TriaCulturaDesktopApp.Model;
using MvvmDialogs.ViewModels;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight;

namespace TriaCulturaDesktopApp.ViewModel
{
    class EspaiViewModel : ViewModelBase, IUserDialogViewModel, INotifyPropertyChanged
    {
        triaculturaCTXEntities context = new triaculturaCTXEntities();

        public bool IsModal
        {
            get
            {
                return false;
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
