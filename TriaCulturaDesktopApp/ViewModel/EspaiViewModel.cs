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
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace TriaCulturaDesktopApp.ViewModel
{
    class EspaiViewModel : ViewModelBase, IUserDialogViewModel, INotifyPropertyChanged
    {
        private ObservableCollection<IDialogViewModel> _Dialogs = new ObservableCollection<IDialogViewModel>();
        public ObservableCollection<IDialogViewModel> Dialogs { get { return _Dialogs; } }
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
            this.DialogClosing(this, null);
        }

        public ICommand tornarEnrere { get { return new RelayCommand(Close); } }
        public void Close()
        {
            if (this.DialogClosing != null)
                this.DialogClosing(this, new EventArgs());
        }
        public ICommand CloseAllCommand { get { return new RelayCommand(OnCloseAll); } }
        public void OnCloseAll()
        {
            this.Dialogs.Clear();
        }



    }


}
