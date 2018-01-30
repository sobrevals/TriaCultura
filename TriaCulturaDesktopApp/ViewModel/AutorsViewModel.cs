﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmDialogs.ViewModels;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TriaCulturaDesktopApp.Model;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace TriaCulturaDesktopApp.ViewModel
{
    public class AutorsViewModel : ViewModelBase, IUserDialogViewModel
    {
        private ObservableCollection<IDialogViewModel> _Dialogs = new ObservableCollection<IDialogViewModel>();
        public ObservableCollection<IDialogViewModel> Dialogs { get { return _Dialogs; } }
        triaculturaCTXEntities context = new triaculturaCTXEntities();

        #region BasicProperties

        private ObservableCollection<author> _authors;
        private int _selectedIndexAuthor;
        private author _selectedAuthor;

        public AutorsViewModel()
        {                  
            FillAuthors(0);
        }
        public ObservableCollection<author> AuthorsL
        {
            get { return (_authors); }
            set { _authors = value; NotifyPropertyChanged(); }
        }

        public int SelectedIndexAuthor
        {
            get { return _selectedIndexAuthor; }
            set { _selectedIndexAuthor = value; NotifyPropertyChanged(); }
        }

        public author SelectedAuthor
        {
            get { return (_selectedAuthor); }
            set
            {
                _selectedAuthor = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region IsModal
        public virtual bool IsModal
        {
            get
            {
                return true;
            }
        }

        #endregion IsModal

        #region ICommand
        public ICommand OkCommand { get { return new RelayCommand(Ok); } }
        protected virtual void Ok()
        {
            if (this.OnOk != null)
                this.OnOk(this);
            else
                Close();
        }

        public ICommand OpenProjecte { get { return new RelayCommand(opProject); } }
        protected virtual void opProject()
        {
            this.Dialogs.Add(new ProjecteViewModel());
        }

        public ICommand afegirAutor { get { return new RelayCommand(addAutor); } }
        protected virtual void addAutor()
        {
            this.Dialogs.Add(new AutorViewModel
            {
                Author = new author(),
                Titol = "Nou Autor",
                OnOk = (sender) =>
                {
                    FillAuthors(0);
                },
                OnCancel = (sender) =>
                {
                    FillAuthors(0);
                },
                OnCloseRequest = (sender) =>
                {
                    FillAuthors(0);
                }
            });
        }

        public ICommand modificarAutor { get { return new RelayCommand(updAutor); } }

        protected virtual void updAutor()
        {
            this.Dialogs.Add(new AutorViewModel(SelectedAuthor)
            {
                Titol = "Modificar Autor",
                Author = SelectedAuthor,
                OnOk = (sender) =>
                {
                    FillAuthors(0);
                    sender.Close();
                },
                OnCancel = (sender) =>
                {
                    FillAuthors(0);
                },
                OnCloseRequest = (sender) =>
                {
                    FillAuthors(0);
                }
            });
        }
        //public ICommand modificarAutor { get { return new RelayCommand(changeAutor); } }
        //protected virtual void changeAutor()
        //{
        //    this.Dialogs.Add(new AutorViewModel
        //    {
        //        Author = SelectedAuthor
        //    });
        //}
        public ICommand tornarEnrere { get { return new RelayCommand(Close); } }
        public Action<AutorsViewModel> OnOk { get; set; }
        #endregion ICommand

        #region DialogClosing

        public event EventHandler DialogClosing;

        public void Close()
        {
            if (this.DialogClosing != null)
                this.DialogClosing(this, new EventArgs());
        }

        #endregion

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region RequestClose
        public void RequestClose()
        {
            this.DialogClosing(this, null);
        }
        #endregion

        #region FillAutors
        private void FillAuthors(int n)
        {
            AuthorsL = new ObservableCollection<author> (context.authors.OrderBy(x => x.dni).ToList());
            
            if (AuthorsL != null)
            {
                SelectedAuthor = AuthorsL[n];
            }
        }
        #endregion
    }
}
