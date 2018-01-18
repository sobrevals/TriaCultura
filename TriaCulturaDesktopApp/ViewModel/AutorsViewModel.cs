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


namespace TriaCulturaDesktopApp.ViewModel
{
    public class AutorsViewModel : ViewModelBase, IUserDialogViewModel
    {
        #region IUserDialogViewModel Implementation

        #region Commands

        public bool IsModal { get; private set; }
        public virtual void RequestClose()
        {
            if (this.OnCloseRequest != null)
                this.OnCloseRequest(this);
            else
                Close();
        }
        public event EventHandler DialogClosing;


        #endregion IUserDialogViewModel Implementationpublic ICommand OkCommand { get { return new RelayCommand(Ok); } }
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

        #endregion Commands


        private string _Message;
        public string Message
        {
            get { return _Message; }
            set { _Message = value; RaisePropertyChanged(() => this.Message); }
        }

        private string _Caption;
        public string Caption
        {
            get { return _Caption; }
            set { _Caption = value; RaisePropertyChanged(() => this.Caption); }
        }

        public Action<AutorsViewModel> OnOk { get; set; }
        public Action<AutorsViewModel> OnCancel { get; set; }
        public Action<AutorsViewModel> OnCloseRequest { get; set; }

        public AutorsViewModel(bool isModal = true)
        {
            this.IsModal = isModal;
        }

        public void Close()
        {
            if (this.DialogClosing != null)
                this.DialogClosing(this, new EventArgs());
        }

        public void Show(IList<IDialogViewModel> collection)
        {
            collection.Add(this);
        }

    }
}
