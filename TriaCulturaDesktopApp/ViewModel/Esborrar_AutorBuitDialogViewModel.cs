using GalaSoft.MvvmLight;
using MvvmDialogs.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriaCulturaDesktopApp.Model;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace TriaCulturaDesktopApp.ViewModel
{
    class Esborrar_AutorBuitDialogViewModel : ViewModelBase, IUserDialogViewModel
    {
        private string _okText;
        private bool _textEnabled;
        private bool _textEnabled_type;
        private string _title;
        private author _author;

        public virtual bool IsModal { get { return true; } }
        public virtual void RequestClose() { this.DialogClosing(this, null); }
        public virtual event EventHandler DialogClosing;

       
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

        public Action<Esborrar_AutorBuitDialogViewModel> OnOk { get; set; }
        public Action<Esborrar_AutorBuitDialogViewModel> OnCancel { get; set; }
        public Action<Esborrar_AutorBuitDialogViewModel> OnCloseRequest { get; set; }

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

        public bool TextEnabled_type
        {
            get
            {
                return _textEnabled_type;
            }

            set
            {
                _textEnabled_type = value;
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

        public author Author
        {
            get
            {
                return _author;
            }

            set
            {
                _author = value;
            }
        }
    }
}
