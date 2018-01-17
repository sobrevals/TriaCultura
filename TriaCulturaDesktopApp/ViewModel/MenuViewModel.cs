using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using MvvmDialogs.ViewModels;



namespace TriaCulturaDesktopApp.ViewModel
{
    public class MenuViewModel : ViewModelBase
    {
        private ObservableCollection<IDialogViewModel> _Dialogs = new ObservableCollection<IDialogViewModel>();
        public ObservableCollection<IDialogViewModel> Dialogs { get { return _Dialogs; } }

        public event PropertyChangedEventHandler PropertyChanged;
    

    }
}
