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


namespace TriaCulturaDesktopApp.ViewModel
{
    class ProjecteViewModel : ViewModelBase, IUserDialogViewModel
    {
        #region BasicProperties
        private List<place> _place;
        private List<request> _request;
        private request _selectedRequest;
        private request _selectedRequest_place;
        private request _selectedPlace_fromProjects;
        private project _selectedproject;

        public List<place> Places
        { get { return _place; } set { _place = value; NotifyPropertyChanged(); } }

        public List<request> Request
        { get { return _request; } set { _request = value; } }

        public request SelectedRequest
        { get { return _selectedRequest; } set { _selectedRequest = value; } }

        public request SelectedRequest_fromPlace
        { get { return _selectedPlace_fromProjects; } set { _selectedPlace_fromProjects = value; } }

        public project SelectedProject
        { get { return _selectedproject; } set { _selectedproject = value; } }

        private ObservableCollection<IDialogViewModel> _Dialogs = new ObservableCollection<IDialogViewModel>();
        public ObservableCollection<IDialogViewModel> Dialogs { get { return _Dialogs; } }
        triaculturaCTXEntities context = new triaculturaCTXEntities();
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
        public ProjecteViewModel()
        {
            FillRequests_all(0);
        }
        #endregion constructor

        #region fill
        private void FillRequests_all(int index)
        {
            Request = context.requests.OrderBy(x => x.place_id).ToList();
            if (Places != null && index >= 0 && index < Places.Count)
            {
                _selectedRequest_place = Request[index];
            }
        }



        private void FillRequest_place(int index)
        {
            if (index >= 0 && index < SelectedProject.requests.Count)
            {
                SelectedRequest_fromPlace = _selectedproject.requests.ToList()[index];
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

        #region Commands

        public ICommand NewModalDialogCommand { get { return new RelayCommand(NewModalDialogEspai); } }
        public void NewModalDialogEspai()
        {
            this.Dialogs.Add(new EspaiViewModel());
        }

        public ICommand AfegirRequest_place { get { return new RelayCommand(afegirPlace); } }
        protected virtual void AddPlace_request()
        {
            request r = _selectedRequest_place;
            SelectedProject.requests.Add(r);
            context.SaveChanges();
            FillRequests_all(0);
        }
        public ICommand TreureRequest_place { get { return new RelayCommand(RemRequest_place); } }
        protected virtual void RemRequest_place()
        {
            request p = SelectedRequest_fromPlace;
            SelectedProject.requests.Remove(p);
            context.SaveChanges();
            FillRequest_place(0);
        }

        public ICommand Afegir_fitxer { get { return new RelayCommand(AddFitxer); } }
        protected virtual void AddFitxer()
        {

        }

        protected virtual void afegirPlace()
        {
            this.Dialogs.Add(new EspaiViewModel());
        }
        public ICommand finalitzar { get { return new RelayCommand(Close); } }
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