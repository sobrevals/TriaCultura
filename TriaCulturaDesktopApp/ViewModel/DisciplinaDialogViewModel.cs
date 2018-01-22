using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmDialogs.ViewModels;
using TriaCulturaDesktopApp.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace TriaCulturaDesktopApp.ViewModel
{
    class DisciplinaDialogViewModel : ViewModelBase, IUserDialogViewModel
    {
        #region Properties
        private string _okText;
        private bool _textEnabled;
        private string _title;
        private discipline _discipline;

        public string OkText
        {
            get { return _okText; }
            set { _okText = value; }
        }
        public bool TextEnabled
        {
            get { return _textEnabled; }
            set { _textEnabled = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        
        public discipline Discipline
        {
            get { return _discipline; }
            set { _discipline = value; }
        }
        public virtual bool IsModal { get { return true; } }
        public virtual void RequestClose() { this.DialogClosing(this, null); }
        public virtual event EventHandler DialogClosing;

        #endregion
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

        public Action<DisciplinaDialogViewModel> OnOk { get; set; }
        public Action<DisciplinaDialogViewModel> OnCancel { get; set; }
        public Action<DisciplinaDialogViewModel> OnCloseRequest { get; set; }

        #endregion Commands
    }
}
