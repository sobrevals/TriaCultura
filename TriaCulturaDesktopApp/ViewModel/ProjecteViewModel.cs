using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MvvmDialogs.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using TriaCulturaDesktopApp.Model;
using System.Runtime.CompilerServices;
using System.Windows;

namespace TriaCulturaDesktopApp.ViewModel
{
    class ProjecteViewModel : ViewModelBase, IUserDialogViewModel, INotifyPropertyChanged
    {
        triaculturaCTXEntities context = new triaculturaCTXEntities();

        #region BasicProperties
        private List<place> _place;
        private List<request> _request;
        private project _projecte;
        private request _selectedRequest;
        private request _selectedRequest_place;
        private request _selectedPlace_fromProjects;
        private List<type> _types;
        private type _selectedType;


        public String titol { get; set; }
        public List<place> Places
        { get { return _place; } set { _place = value; NotifyPropertyChanged(); } }

        public List<request> Request
        { get { return _request; } set { _request = value; NotifyPropertyChanged(); } }

        public request SelectedRequest
        { get { return _selectedRequest; } set { _selectedRequest = value; NotifyPropertyChanged(); } }

        public request SelectedRequest_fromPlace
        { get { return _selectedPlace_fromProjects; } set { _selectedPlace_fromProjects = value; NotifyPropertyChanged(); } }

        public project Projecte
        {
            get
            {
                return _projecte;
            }

            set
            {
                _projecte = value;
                NotifyPropertyChanged("Project");
            }
        }

        public List<type> Types
        {
            get
            {
                return _types;
            }

            set
            {
                _types = value;
            }
        }

        public type SelectedType
        {
            get
            {
                return _selectedType;
            }

            set
            {
                if (_selectedType == value) return;
                _selectedType = value;
                NotifyPropertyChanged();
                RaisePropertyChanged("SelectedType");
            }
        }

        private ObservableCollection<string> _requestL;
        private ObservableCollection<string> RequestL { get { return _requestL; } set { _requestL = value; NotifyPropertyChanged(); } }

        private ObservableCollection<IDialogViewModel> _Dialogs = new ObservableCollection<IDialogViewModel>();
        public ObservableCollection<IDialogViewModel> Dialogs { get { return _Dialogs; NotifyPropertyChanged(); } }

        #endregion BasicProperties

        #region isModal
        public bool IsModal
        {
            get
            {
                return true;
            }
        }

        #endregion isModal

        #region constructor
        public ProjecteViewModel(project p)
        {
            Projecte = p;
            titol = "Nou Projecte";

            FillRequests_all(0);
            FillTipus(0);
        }

        public ProjecteViewModel() { }

        public ProjecteViewModel(author a)
        {
            Projecte = new project();
            Projecte.author = a;

            FillRequests_all(0);

        }
        //public ProjecteViewModel(project p)
        //{
        //    Projecte = p;

        //    FillRequests_all(0);
        //}
        #endregion constructor

        #region fill
        private void FillRequests_all(int index)
        {
            if (Projecte != null)
            {
                Request = Projecte.requests.ToList();
                if (Places != null && index >= 0 && index < Places.Count)
                {
                    _selectedRequest_place = Request[index];
                }
            }
        }

        public void FillTipus(int index)
        {
            Types = context.types.ToList();          
        }



        private void FillRequest_place(int index)
        {
            // ProjectWithPlace != null && n > 0
            if (Projecte != null && index > 0)
            {
                SelectedRequest_fromPlace = Projecte.requests.ToList()[index];
            }
            else
            {
                Projecte = new project();
            }
        }
        #endregion

        #region PropertyChanged

        public virtual event EventHandler DialogClosing;
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void RequestClose()
        {
            this.DialogClosing(this, null);
        }
        #endregion

        //protected void save_changes()
        //{
        //    if (context.projects.Where(x => x.id_project == Projecte.id_project).ToList().Count == 1)
        //    {
        //        context.projects.Where(x => x.id_project == Projecte.id_project).ToList()[0] = Projecte;
        //    }
        //    else
        //    {
        //        context.projects.Add(Projecte);
        //    }
        //    context.SaveChanges();
        //    Projecte = context.projects.Where(x => x.id_project == Projecte.id_project).SingleOrDefault();
        //}
        #region Commands

        public ICommand NewModalDialogCommand { get { return new RelayCommand(NewModalDialogEspai); } }
        public void NewModalDialogEspai()
        {
            this.Dialogs.Add(new EspaiViewModel());
        }

        public void FillTipus()
        {
            Types = context.types.ToList();
        }

        protected virtual void AddPlace_request()
        {
            request r = _selectedRequest_place;
            Projecte.requests.Add(r);
            context.SaveChanges();
            FillRequests_all(0);
        }
        public ICommand TreureRequest_place { get { return new RelayCommand(RemRequest_place); } }
        protected virtual void RemRequest_place()
        {
            if (SelectedRequest != null)
            {
                request aux_request = context.requests.Where(x => x.id_request == SelectedRequest.id_request).SingleOrDefault();
                try
                {
                    //request p = SelectedRequest_fromPlace;
                    Projecte.requests.Remove(aux_request);
                    context.SaveChanges();
                    FillRequest_place(0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No es pot esborrar aquestsa place.");
                }
            }
        }


        public ICommand Afegir_fitxer { get { return new RelayCommand(AddFitxer); } }
        protected virtual void AddFitxer()
        {

        }
        public ICommand Finalitzar { get { return new RelayCommand(SaveProject); } }

        public void addType_toProject()
        {
            if (!(Projecte.types.Select(x=>x.name).ToList()[0].Equals(SelectedType.name)))
            {
                Projecte.types.Add(SelectedType);
            }
        }

        public void SaveProject()
        {
            Projecte.types.Add(SelectedType);
            //if (context.projects.Select(x => x.id_project).ToList().Contains(Projecte.id_project))
            //{
            //    context.projects.Where(x => x.id_project == Projecte.id_project).ToList()[0] = Projecte;
            //}
            //else
            //{
            //    context.projects.Add(Projecte);
            //}
            //context.SaveChanges();
            //Close();
        }


        public void deleteProject()
        {

            project aux_project = new Model.project();

            if (Projecte != null)
            {
                aux_project.id_project = Projecte.id_project;
                aux_project.author_dni = Projecte.author_dni;
                aux_project.title = Projecte.title;
                aux_project.types = Projecte.types;
                aux_project.topic = Projecte.topic;
                aux_project.requests = Projecte.requests;
                aux_project.files = Projecte.files;
                aux_project.author = Projecte.author;
              /*  this.Dialogs.Add(new ProjectesViewModel
                {
                    Title = "Esborrar Projecte",
                    Telefon = aux_project,
                    Id_item = aux_project.id_phone,
                    Type_item = aux_tel.type,
                    DataText = "Numero",
                    Data_item = aux_tel.num,
                    TextEnabled = false,
                    TextEnabled_type = false,
                    OkText = "Esborra",
                    OnOk = (sender) =>
                    {
                        Author.phones.Remove(SelectedPhone);
                        FillTelefons();
                        sender.Close();
                    },
                    OnCancel = (sender) => { sender.Close(); },
                    OnCloseRequest = (sender) => { sender.Close(); }
                });*/
            }
        }


        public ICommand AfegirRequest_place { get { return new RelayCommand(afegirPlace); } }
        protected virtual void afegirPlace()
        {
            //save_changes();
            request aux_request = new request();
            aux_request.project_id = Projecte.id_project;

            this.Dialogs.Add(new EspaiViewModel(Projecte)
            {
                SelectedProject = Projecte,
                OnOk = (sender) =>
                {
                    context.SaveChanges();
                    FillRequest_place(0);
                    sender.Close();
                },
                OnCancel = (sender) => { sender.Close(); },
                OnCloseRequest = (sender) => { sender.Close(); }
            });
        }
        public ICommand OkCommand { get { return new RelayCommand(GuardarIEnrere); } }
        protected virtual void GuardarIEnrere()
        {
            addType_toProject();
            if (this.OnOk != null)
                this.OnOk(this);
            else
                Close();
        }

        public ICommand tornarEnrere { get { return new RelayCommand(Close); } }
        public void Close()
        {
            if (this.DialogClosing != null)
                this.DialogClosing(this, new EventArgs());
        }


        #endregion

        public Action<ProjecteViewModel> OnOk { get; set; }
        public Action<ProjecteViewModel> OnReturn { get; set; }
        public Action<ProjecteViewModel> OnCloseRequest { get; set; }

      
    }
}