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

namespace TriaCulturaDesktopApp.ViewModel
{
    public class AutorsViewModel : ViewModelBase, IUserDialogViewModel
    {
        triaculturaCTXEntities context = new triaculturaCTXEntities();

        #region BasicProperties
             
        private List<author> _authors;
        private int _selectedIndexAuthor;
        private author _selectedAuthor;

        
        public List<author> AuthorsL
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

        public event EventHandler DialogClosing;

        public bool IsModal
        {
            get
            {
                return true;
            }
        }
        public virtual void RequestClose() { this.DialogClosing(this, null); }
        public ICommand OpenAuthor { get { return new RelayCommand(OpAuthor); } }
        protected virtual void OpAuthor()
        {
            if (this.OnOk != null)
                this.OnOk(this);
            else
                Close();
        }

        public void Close()
        {
            if (this.DialogClosing != null)
                this.DialogClosing(this, new EventArgs());
        }

        public Action<AutorsViewModel> OnOk { get; set; }

       

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
