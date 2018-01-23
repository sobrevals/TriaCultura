﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MvvmDialogs.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TriaCulturaDesktopApp.Model;
using System.Runtime.CompilerServices;


namespace TriaCulturaDesktopApp.ViewModel
{
    class ProjecteViewModel : ViewModelBase,  INotifyPropertyChanged, IUserDialogViewModel
    {

        private List<place> _place;
        private List<request> _request;
        private request _selectedRequest;
        private request _selectedRequest_place;
        private request _selectedPlace_fromProjects;
        private project _selectedproject;

        public List<place> Places
        { get { return _place; } set { _place = value; } }

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
        triaculturaDB_localEntities context = new triaculturaDB_localEntities();


        public bool IsModal
        {
            get
            {
                return true;
            }
        }


        ProjecteViewModel()
        {
            FillRequests_all(0);
        }




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
        public ICommand NewModalDialogCommand { get { return new RelayCommand(NewModalDialogEspai); } }
        public void NewModalDialogEspai()
        {
            this.Dialogs.Add(new EspaiViewModel());
        }


        #region Commands
        public ICommand AfegirRequest_place { get { return new RelayCommand(AddPlace_request); } }
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


        public ICommand OkCommand { get { return new RelayCommand(Ok); } }
        protected virtual void Ok()
        {
            if (this.OnOk != null)
            {
                this.OnOk(this);
            }
            else if (this.OnReturn != null)
            {
                this.OnReturn(this);
            } else
            {
                this.OnCloseRequest(this);
            }
        }

        public Action<ProjecteViewModel> OnOk { get; set; }
        public Action<ProjecteViewModel> OnReturn { get; set; }
        public Action<ProjecteViewModel> OnCloseRequest { get; set; }
        #endregion
    }
}