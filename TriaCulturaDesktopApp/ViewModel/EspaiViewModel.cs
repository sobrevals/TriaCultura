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

        #region
        private List<place> _placeL;
        private List<place> _projectPlace;
        private place _selectedPlace_fromPlaces;
        private place _selectedPlace;
        private place _selectedPlace_fromProject;
        private project _project;
        private int _selectedIndexPlace;

        public List<string> PlaceNoms { get { return PlaceL.Select(x => x.name).ToList(); } }
        public List<string> ProjectPlaces { get { return ProjectPlace.Select(x => x.name).ToList(); } }

        public List<place> PlaceL
        {
            get
            {
                return _placeL;
            }

            set
            {
              
                _placeL = value;
                NotifyPropertyChanged();
            }
        }
        public List<place> ProjectPlace
        {
            get
            {
                return _projectPlace;
            }

            set
            {
                _projectPlace = value;
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

       

        #endregion

        public EspaiViewModel()
        {
            fillPlaces(0);
            fillProjectPlaces(0);
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

        public ICommand agregarEspai { get { return new RelayCommand(addPlace); } }

        public virtual void addPlace()
        {
            request aux = new request();
            aux.place_id = SelectedPlace_fromPlaces.id;
            aux.project_id = Project.id_project;
            context.requests.Add(aux);
            fillProjectPlaces(0);
           
        }

        public ICommand tornarEnrere { get { return new RelayCommand(Close); } }
        public void Close()
        {
            if (this.DialogClosing != null)
                this.DialogClosing(this, new EventArgs());
        }
        public ICommand CloseAllCommand { get { return new RelayCommand(OnCloseAll); } }

        public project Project
        {
            get
            {
                return _project;
            }

            set
            {
                _project = value;
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

        public void fillProjectPlaces(int n)
        {
            ProjectPlace = context.requests.Where(x => x.project_id == x.project.id_project).Select(x => x.place).ToList();
            if (ProjectPlace != null && n > 0)
            {
                Selectedplace_fromProject = ProjectPlace[n];
            }
        }
        
        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }


}
