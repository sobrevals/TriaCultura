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
        private bool _textEnabled;
        private bool _textEnabled_type;
        private string _title;
        private project _projecte;

        private int _id_item;
        private string _title_item;
        private string _dni_item;

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
        public project Projecte
        {
            get
            {
                return _projecte;
            }

            set
            {
                _projecte = value;
            }
        }

        public int Id_item
        {
            get
            {
                return _id_item;
            }

            set
            {
                _id_item = value;
                if (Projecte != null)
                {
                    Projecte.id_project = value;
                }
            }
        }

        public string Title_item
        {
            get
            {
                return _title_item;
            }

            set
            {
                _title_item = value;
                if (Projecte != null)
                {
                    Projecte.title = value;
                }
            }
        }

        public string Dni_item
        {
            get
            {
                return _dni_item;
            }

            set
            {
                _dni_item = value;
                if (Projecte != null) Projecte.author_dni = value;
            }
        }



        public virtual bool IsModal { get { return true; } }
        public virtual void RequestClose() { this.DialogClosing(this, null); }
        public virtual event EventHandler DialogClosing;
        #endregion
        /* #region Commands
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

         public Action<AutorDialogViewModel> OnOk { get; set; }
         public Action<AutorDialogViewModel> OnCancel { get; set; }
         public Action<AutorDialogViewModel> OnCloseRequest { get; set; }


     }

     #endregion
     */
    }
}

