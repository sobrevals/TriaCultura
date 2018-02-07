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
    class ProjecteEsborrarDialogModel : ViewModelBase, IUserDialogViewModel
    {
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

        public Action<ProjecteEsborrarDialogModel> OnOk { get; set; }
        public Action<ProjecteEsborrarDialogModel> OnCancel { get; set; }
        public Action<ProjecteEsborrarDialogModel> OnCloseRequest { get; set; }

    }
}

