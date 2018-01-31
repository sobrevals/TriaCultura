using System;
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
using System.Windows;

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
            this.Dialogs.Add(new ProjectesViewModel());
        }

        public ICommand afegirAutor { get { return new RelayCommand(addAutor); } }
        protected virtual void addAutor()
        {
            author aux_author = new author();
            this.Dialogs.Add(new AutorViewModel
            {
                Author = aux_author,
                Titol = "Nou Autor",
                OnOk = (sender) =>
                {
                    context.SaveChanges();
                    FillAuthors(0);
                    sender.Close();
                },
                OnCancel = (sender) =>
                {
                    FillAuthors(0);
                    sender.Close();
                },
                OnCloseRequest = (sender) =>
                {
                    FillAuthors(0);
                    sender.Close();
                }
            });
        }

        public ICommand modificarAutor { get { return new RelayCommand(updAutor); } }

        protected virtual void updAutor()
        {
            author aux_author = SelectedAuthor;
            aux_author.dni = SelectedAuthor.dni;
            aux_author.address = SelectedAuthor.address;
            aux_author.disciplines = SelectedAuthor.disciplines;
            aux_author.name = SelectedAuthor.name;
            aux_author.surname = SelectedAuthor.surname;
            aux_author.emails = SelectedAuthor.emails;
            aux_author.phones = SelectedAuthor.phones;
            aux_author.projects = SelectedAuthor.projects;
            this.Dialogs.Add(new AutorViewModel(aux_author)
            {
                Titol = "Modificar Autor",
                Author = aux_author,
                OnOk = (sender) =>
                {
                    SelectedAuthor.dni = aux_author.dni;
                    SelectedAuthor.address = aux_author.address;
                    SelectedAuthor.disciplines = aux_author.disciplines;
                    SelectedAuthor.name = aux_author.name;
                    SelectedAuthor.surname = aux_author.surname;
                    SelectedAuthor.emails = aux_author.emails;
                    SelectedAuthor.phones = aux_author.phones;
                    SelectedAuthor.projects = aux_author.projects;
                    try
                    {
                        AuthorsL.Where(x => x.dni == SelectedAuthor.dni).FirstOrDefault().dni = SelectedAuthor.dni;
                        AuthorsL.Where(x => x.dni == SelectedAuthor.dni).FirstOrDefault().address = SelectedAuthor.address;
                        AuthorsL.Where(x => x.dni == SelectedAuthor.dni).FirstOrDefault().name = SelectedAuthor.name;
                        AuthorsL.Where(x => x.dni == SelectedAuthor.dni).FirstOrDefault().surname = SelectedAuthor.surname;
                        context.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString());
                    }
                    FillAuthors(0);
                    SelectedAuthor = AuthorsL.Where(x => x.dni == aux_author.dni).ToList()[0];
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
            AuthorsL = new ObservableCollection<author>(context.authors.OrderBy(x => x.dni).ToList());

            if (AuthorsL != null)
            {
                SelectedAuthor = AuthorsL[n];
            }
        }
        #endregion
    }
}
