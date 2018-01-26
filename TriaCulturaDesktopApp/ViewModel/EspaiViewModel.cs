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
        private List<place> _placeList;
        private List<place> _placeWithProject;
        private List<place> _placeWithoutProject;
        private place _selectedPlace_fromPlaces;
        private place _selectedPlace;
        private place _selectedPlace_fromProject;
        private project _selectedProject;
        private int _selectedIndexPlace;

        public List<string> PlaceNoms { get { return PlaceWithoutProject.Select(x => x.name).ToList(); } }
        public List<string> ProjectPlaces { get { return ProjectWithPlace.Select(x => x.name).ToList(); } }

        public List<place> PlaceL
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
        public List<place> ProjectWithPlace
        {
            get
            {
                return _placeWithProject;
            }

            set
            {
                _placeWithProject = value;
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

        public List<place> PlaceWithoutProject
        {
            get
            {
                return _placeWithoutProject;
            }

            set
            {
                _placeWithoutProject = value;
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
        }

        public EspaiViewModel(project p)
        {
            SelectedProject = context.projects.Where(x => x.id_project == p.id_project).SingleOrDefault();
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
            aux.place_id = SelectedPlace_fromPlaces.id_place;
            aux.project_id = SelectedProject.id_project;
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

        public void OnCloseAll()
        {
            this.Dialogs.Clear();
        }

        public void fillPlaces(int n)
        {
            List<request> llista_requests = context.requests.Where(x => x.project_id != SelectedProject.id_project).ToList();

            List<place> llista_espais = llista_requests.Select(x => x.place).ToList();
            PlaceWithoutProject = llista_espais;

            if (PlaceWithoutProject != null && n > 0)
            {
                SelectedPlace = PlaceWithoutProject[n];
            }
            else
            {
                PlaceWithoutProject = new List<place>();
            }
        }

        public void fillProjectPlaces(int n)
        {
            //ProjectPlace = context.requests.Where(x => x.project_id == x.project.id_project).Select(x => x.place).ToList();
            if (ProjectWithPlace != null && n > 0)
            {
                Selectedplace_fromProject = ProjectWithPlace[n];
            }
            else
            {
                ProjectWithPlace = new List<place>();
            }
        }

        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public Action<DisciplinaViewModel> OnOk { get; set; }
        public Action<DisciplinaViewModel> OnCancel { get; set; }
        public Action<DisciplinaViewModel> OnCloseRequest { get; set; }
    }
}
