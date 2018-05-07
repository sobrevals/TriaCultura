using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MvvmDialogs.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TriaCulturaDesktopApp.Model;

namespace TriaCulturaDesktopApp.ViewModel
{
    public class PlaceDialogViewModel: ViewModelBase, IUserDialogViewModel
    {
        #region basic_properties
        private string _okText;
        private bool _textEnabled;
        private string _title;

        private place _selectedPlace;

        public string OkText
        {
            get
            {
                return _okText;
            }

            set
            {
                _okText = value;
            }
        }

        public bool TextEnabled
        {
            get
            {
                return _textEnabled;
            }

            set
            {
                _textEnabled = value;
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                _title = value;
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
            }
        }
        #endregion
        public virtual bool IsModal { get { return true; } }
        public virtual void RequestClose() { this.DialogClosing(this, null); }
        public virtual event EventHandler DialogClosing;
        #region Commands
        public ICommand OkCommand { get { return new RelayCommand(Ok); } }
        protected virtual void Ok()
        {
            if (this.OnOk != null)
                this.OnOk(this);
            else
                Close();
        }

        public ICommand CancelCommand { get { return new RelayCommand(Cancel); } }
        protected virtual void Cancel()
        {
            if (this.OnCancel != null)
                this.OnCancel(this);
            else
                Close();
        }

        public void Close()
        {
            if (this.DialogClosing != null)
                this.DialogClosing(this, new EventArgs());
        }

        public Action<PlaceDialogViewModel> OnOk { get; set; }
        public Action<PlaceDialogViewModel> OnCancel { get; set; }
        public Action<PlaceDialogViewModel> OnCloseRequest { get; set; }

    #endregion
    }
}
