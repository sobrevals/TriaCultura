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

        private List<place> _placeL;
        private List<string> _placeNoms;
        private place _selectedPlace;
        private int _selectedIndexPlace;

        public EspaiViewModel()
        {
            fillPlaces(0);
        }

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

        public List<place> PlaceL
        {
            get
            {
                return _placeL;
            }

            set
            {
                _placeL = value;
            }
        }

        public place SelectedPlace
        {
            get
            {
                return _selectedPlace;
            }

            set
            {
                _selectedPlace = value;
            }
        }

        public int SelectedIndexPlace
        {
            get
            {
                return _selectedIndexPlace;
            }

            set
            {
                _selectedIndexPlace = value;
            }
        }

        public List<string> PlaceNoms
        {
            get
            {
                return PlaceL.Select(x => x.name).ToList();
            }

            set
            {
                _placeNoms = value;
            }
        }

        public void OnCloseAll()
        {
            this.Dialogs.Clear();
        }

        public void fillPlaces(int n)
        {
            PlaceL = context.places.OrderBy(x => x.name).ToList();

            if (PlaceL != null)
            {
                SelectedPlace = PlaceL[n];
            }
        }

    }


}
