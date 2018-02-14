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

        public triaculturaCTXEntities context { get; set; }



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
                NotifyPropertyChanged();
                RaisePropertyChanged("PlaceWithProject");
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
                NotifyPropertyChanged();
                RaisePropertyChanged("PlaceWithoutProject");
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

        public place SelectedPlace_fromProject
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

        public EspaiViewModel(project p, triaculturaCTXEntities context)
        {
            this.context = context;
            SelectedProject = p;
            if (context != null)
            {
                fillPlaces(0);
                fillProjectPlaces(0);
            }
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
            request r = new request();
            r.project_id = SelectedProject.id_project;
            r.place_id = SelectedPlace_fromPlaces.id_place;
            r.place = SelectedPlace_fromPlaces;
            SelectedProject.requests.Add(r);
            
            fillPlaces(0);
            fillProjectPlaces(0);
        }

        public ICommand treureEspai { get { return new RelayCommand(removePlace); } }

        public virtual void removePlace()
        {
            request r = new request();
            r.project_id = SelectedProject.id_project;
            r.place_id = SelectedPlace_fromProject.id_place;
            r.place = SelectedPlace_fromProject;
            List<request> aux_list = SelectedProject.requests.ToList();
            foreach (request req in aux_list)
            {
                if (req.place_id.Equals(r.place_id))
                {
                    SelectedProject.requests.Remove(req);
                }
            }
            //SelectedProject.requests.Remove(r);
            fillPlaces(0);
            fillProjectPlaces(0);
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
                //List<place> places_requested = context.places.Where(x => aux_project_request_list.Select(y => y.place_id).Equals(x.id_place)).ToList();
                List<place> all_places = context.places.ToList();
                List<place> aux_all_places = new List<place>();

                foreach (place item in all_places)
                {
                    if (places_requested.Select(x => x.id_place).ToList().Contains(item.id_place))
                    {
                        aux_all_places.Add(item);
                    }
                }

                foreach (place p in aux_all_places)
                {
                    if (places_requested.Select(x => x.id_place).ToList().Contains(p.id_place))
                    {
                        all_places.Remove(p);
                    }
                }

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
