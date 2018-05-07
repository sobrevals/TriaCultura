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
    public class TypeDialogViewModel : ViewModelBase, IUserDialogViewModel
    {

        #region basic_properties
        private string _okText;
        private bool _textEnabled;
        private string _title;

        private type _selected_type;

        public type Selected_type
        {
            get
            {
                return _selected_type;
            }

            set
            {
                _selected_type = value;
            }
        }
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

        public Action<TypeDialogViewModel> OnOk { get; set; }
        public Action<TypeDialogViewModel> OnCancel { get; set; }
        public Action<TypeDialogViewModel> OnCloseRequest { get; set; }

    #endregion
    }
}
