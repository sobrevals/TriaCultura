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
        #region Properties
        private string _okText;       
        private string _title;
        private author _author;
        private List<project> _projectList;

        public string OkText
        {
            get { return _okText; }
            set { _okText = value; }
        }    
        public string Title
        {
            get { return _title; }
            set { _title = value; }
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
        public List<project> ProjectList
        {
            get
            {
                return _projectList;
            }

            set
            {
                _projectList = value;
               
            }
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

        public Action<ProjecteEsborrarDialogModel> OnOk { get; set; }
        public Action<ProjecteEsborrarDialogModel> OnCancel { get; set; }
        public Action<ProjecteEsborrarDialogModel> OnCloseRequest { get; set; }

     
    }

    #endregion

}

