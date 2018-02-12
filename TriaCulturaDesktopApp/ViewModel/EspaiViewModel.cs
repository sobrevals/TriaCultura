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

        #region BasicProperties
        private List<request> _allRequestInProject;
        private List<request> _allRequestOfProject;
        private List<place> _placeList;
        private ObservableCollection<place> _placeWithProject;
        private ObservableCollection<place> _placeWithoutProject;
        private place _selectedPlace_fromPlaces;
        private place _selectedPlace;
        private place _selectedPlace_fromProject;
        private project _selectedProject;
        private int _selectedIndexPlace;



        public List<request> AllRequestInProject
        {
            get
            {
                return _allRequestInProject;
            }

            set
            {
                _allRequestInProject = value;
                NotifyPropertyChanged();
            }
        }

        public List<request> AllRequestOfProject
        {
            get
            {
                return _allRequestOfProject;
            }

            set
            {
                _allRequestOfProject = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<place> PlaceWithProject
        {
            get
            {
                return _placeWithProject;
            }

            set
            {
                _placeWithProject = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<place> PlaceWithoutProject
        {
            get
            {
                return _placeWithoutProject;
            }

            set
            {
                _placeWithoutProject = value;
                RaisePropertyChanged();
            }
        }

        public List<place> PlaceList
        {
            get
            {
                return _placeList;
            }

            set
            {
                _placeList = value;
                NotifyPropertyChanged();
            }
        }

        public place SelectedPlace_fromPlaces
        {
            get
            {
                return _selectedPlace_fromPlaces;
            }

            set
            {
                _selectedPlace_fromPlaces = value;
                NotifyPropertyChanged();
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
                NotifyPropertyChanged();
            }
        }

        public place Selectedplace_fromProject
        {
            get
            {
                return _selectedPlace_fromProject;
            }

            set
            {
                _selectedPlace_fromProject = value;
                NotifyPropertyChanged();
            }
        }
        public project SelectedProject
        {
            get
            {
                return _selectedProject;
            }

            set
            {
                _selectedProject = value;
                NotifyPropertyChanged();
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
                NotifyPropertyChanged();
            }
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
        #endregion
        #region constructor

        public EspaiViewModel()
        {
        }

        public EspaiViewModel(project p)
        {
            SelectedProject = p;
            fillPlaces(0);
            fillProjectPlaces(0);
        }
        #endregion
        #region ICommand

        public void RequestClose()
        {
            this.DialogClosing(this, null);
        }

        public ICommand agregarEspai { get { return new RelayCommand(addPlace); } }

        public virtual void addPlace()
        {
            // MIRAR-SE DISCIPLINAVIEWMODEL I FER AQUEST
        }

        public ICommand treureEspai { get { return new RelayCommand(removePlace); } }

        public virtual void removePlace()
        {
            // MIRAR-SE DISCIPLINAVIEWMODEL I FER AQUEST
        }

        public ICommand tornarEnrere { get { return new RelayCommand(Close); } }
        public void Close()
        {
            if (this.DialogClosing != null)
                this.DialogClosing(this, new EventArgs());
        }

        public ICommand OkCommand { get { return new RelayCommand(Ok); } }

        public void Ok()
        {
            if (this.OnOk != null)
            {
                this.OnOk(this);
            }
            else
            {
                this.Close();

            }
        }
        #endregion
        #region filling
        public void fillPlaces(int n)
        {
            if (SelectedProject != null)
            {
                List<request> aux_project_request_list = SelectedProject.requests.ToList();
                List<place> places_requested = aux_project_request_list.Select(x => x.place).Distinct().ToList();
                List<place> all_places = context.places.ToList();
                all_places.RemoveAll(x => places_requested.Contains(x));

                PlaceWithoutProject = new ObservableCollection<place>(all_places);
            }
        }


        public void fillProjectPlaces(int n)
        {
            if (SelectedProject != null)
            {
                List<request> aux_request_list = SelectedProject.requests.ToList();
                List<place> aux_place_list = aux_request_list.Select(x => x.place).Distinct().ToList();
                
                PlaceWithProject = new ObservableCollection<place>(aux_place_list);
            }
        }

        #endregion

        #region propertychanged
        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
        public Action<EspaiViewModel> OnOk { get; set; }
        public Action<EspaiViewModel> OnCancel { get; set; }
        public Action<EspaiViewModel> OnCloseRequest { get; set; }

    }
}
