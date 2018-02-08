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
    class EsborrarProjecteFromProjectes : ViewModelBase, IUserDialogViewModel
    {
        
        #region Basic Properties
        private string _okText;
        private string _title;
        private project _project;       
      
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

        public project Project
        {
            get
            {
                return _project;
            }

            set
            {
                _project = value;
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

        #endregion

        public Action<EsborrarProjecteFromProjectes> OnOk { get; set; }
        public Action<EsborrarProjecteFromProjectes> OnCancel { get; set; }
        public Action<EsborrarProjecteFromProjectes> OnCloseRequest { get; set; }

       
    }
}
