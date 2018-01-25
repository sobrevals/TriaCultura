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
    class AutorDialogViewModel : ViewModelBase, IUserDialogViewModel
    {
        #region Properties
        private string _okText;
        private bool _textEnabled;
        private bool _textEnabled_type;
        private string _title;

        private phone _telefon;
        private email _mail;

        private int _id_item;
        private string _data_item;
        private string _type_item;
        private string _dataText;

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
        public bool TextEnabled_type
        {
            get { return _textEnabled_type; }
            set { _textEnabled_type = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        public phone Telefon
        {
            get
            {
                return _telefon;
            }

            set
            {
                _telefon = value;
            }
        }

        public email Mail
        {
            get
            {
                return _mail;
            }

            set
            {
                _mail = value;
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
                if (Telefon!=null)
                {
                    Telefon.id_phone = value;
                } else if (Mail!=null)
                {
                    Mail.id_email = value;
                }
            }
        }

        public string Data_item
        {
            get
            {
                return _data_item;
            }

            set
            {
                _data_item = value;
                if (Telefon!=null)
                {
                    Telefon.num = value;
                } else if (Mail!=null)
                {
                    Mail.address = value;
                }
            }
        }

        public string Type_item
        {
            get
            {
                return _type_item;
            }

            set
            {
                _type_item = value;
                if (Telefon != null) Telefon.type = value;
            }
        }

        public string DataText
        {
            get
            {
                return _dataText;
            }

            set
            {
                _dataText = value;
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

        public Action<AutorDialogViewModel> OnOk { get; set; }
        public Action<AutorDialogViewModel> OnCancel { get; set; }
        public Action<AutorDialogViewModel> OnCloseRequest { get; set; }

        
    }

    #endregion
}

