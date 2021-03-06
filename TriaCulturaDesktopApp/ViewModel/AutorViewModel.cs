﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TriaCulturaDesktopApp.Model;
using MvvmDialogs.ViewModels;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace TriaCulturaDesktopApp.ViewModel
{
    class AutorViewModel : ViewModelBase, IUserDialogViewModel, INotifyPropertyChanged
    {
        #region Properties
        triaculturaDBEntities context = new triaculturaDBEntities();

        private bool __boto_afegir_enabled;
        private author _author;
        private ObservableCollection<discipline> _disciplines;
        private ObservableCollection<phone> _telefons;
        private ObservableCollection<email> _emails;
        private Image _foto;
        private phone _selectedPhone;
        private email _selectedEmail;
        private discipline _selectedDiscipline;
        #region PropertyChanged // DialogClosing
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler DialogClosing;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public bool Boto_afegir_enabled
        {
            get
            {
                return __boto_afegir_enabled;
            }

            set
            {
                __boto_afegir_enabled = value;
            }
        }
        #endregion
        private bool _isReadAuthor;
        private bool _isVisible;

        public bool IsVisible { get { return _isVisible; } set { _isVisible = value; NotifyPropertyChanged("IsVisible"); RaisePropertyChanged("IsVisible"); } }

        public bool IsReadAuthor { get { return _isReadAuthor; } set { _isReadAuthor = value; NotifyPropertyChanged(""); } }

        public author Author { get { return _author; } set { _author = value; NotifyPropertyChanged(); } }
        public discipline SelectedDiscipline { get { return _selectedDiscipline; } set { _selectedDiscipline = value; NotifyPropertyChanged(); } }
        public ObservableCollection<discipline> Disciplines { get { return _disciplines; } set { _disciplines = value; NotifyPropertyChanged(); } }
        public ObservableCollection<phone> Telefons { get { return _telefons; } set { _telefons = value; NotifyPropertyChanged(); } }
        public ObservableCollection<email> Emails { get { return _emails; } set { _emails = value; NotifyPropertyChanged(); } }
        public Image Foto { get { return _foto; } set { _foto = value; NotifyPropertyChanged(); } }

        public phone SelectedPhone { get { return _selectedPhone; } set { _selectedPhone = value; } }

        public email SelectedEmail { get { return _selectedEmail; } set { _selectedEmail = value; } }

        public String Titol { get; set; }


        public string SelectedTelefonNum { get; set; }
        public string SelectedEmailAddr { get; set; }

        private ObservableCollection<IDialogViewModel> _dialogs = new ObservableCollection<IDialogViewModel>();

        public ObservableCollection<IDialogViewModel> Dialogs { get { return _dialogs; } set { _dialogs = value; } }

        public bool IsModal
        {
            get
            {
                return true;
            }
        }


        #endregion
        public AutorViewModel()
        {
            IsVisible = true;
            IsReadAuthor = false;
            // de proves
            author a = new author();
            Titol = "Nou Autor";
            Author = a;
            FillDisciplines();
            FillTelefons();
            FillEmails();
        }

        public AutorViewModel(author a)
        {
            IsVisible = true;
            IsReadAuthor = true;
            Titol = "Modificar Autor";
            Author = context.authors.Select(x => x.dni).ToList().Contains(a.dni) ? context.authors.Where(x => x.dni == a.dni).SingleOrDefault() : new Model.author();
            FillDisciplines();
            FillTelefons();
            FillEmails();
        }

        #region Fills
        public void FillDisciplines()
        {
            Disciplines = new ObservableCollection<discipline>(Author.disciplines.ToList());
        }

        public void FillTelefons()
        {
            Telefons = new ObservableCollection<phone>(Author.phones.ToList());
        }

        public void FillEmails()
        {
            Emails = new ObservableCollection<email>(Author.emails.ToList());
        }

        #endregion
        public void RequestClose()
        {
            if (this.DialogClosing != null)
                this.DialogClosing(this, new EventArgs());
        }

        #region ICommand

        public ICommand OkCommand { get { return new RelayCommand(GuardarIEnrere); } }
        protected virtual void GuardarIEnrere()
        {

            if (this.OnOk != null)
                this.OnOk(this);
            else
                Close();
        }

        public ICommand EsborrarDisciplina { get { return new RelayCommand(DelDisciplina); } }

        public void DelDisciplina()
        {
            if (SelectedDiscipline != null)
            {
                discipline aux_discipline = new discipline();
                aux_discipline.author_dni = SelectedDiscipline.author_dni;
                aux_discipline.type = SelectedDiscipline.type;
                aux_discipline.id_discipline = SelectedDiscipline.id_discipline;
                this.Dialogs.Add(new DisciplinaDialogViewModel
                {
                    Title = "Esborrar Disciplina",
                    Discipline = aux_discipline,
                    OkText = "Esborra",
                    TextEnabled = false,
                    OnOk = (sender) =>
                    {
                        Author.disciplines.Remove(SelectedDiscipline);
                        FillDisciplines();
                        sender.Close();
                    },
                    OnCancel = (sender) => { sender.Close(); },
                    OnCloseRequest = (sender) => { sender.Close(); }
                });
            }
        }
        public ICommand AfegirDisciplina { get { return new RelayCommand(AddDisciplina); } }

        public void AddDisciplina()
        {
            discipline aux_discipline = new discipline();
            aux_discipline.author_dni = Author.dni;
            this.Dialogs.Add(new DisciplinaDialogViewModel
            {
                Title = "Afegir Disciplina",
                Discipline = aux_discipline,
                OkText = "Afegeix",
                TextEnabled = true,
                OnOk = (sender) =>
                {
                    Author.disciplines.Add(aux_discipline);
                    FillDisciplines();
                    sender.Close();
                },
                OnCancel = (sender) => { sender.Close(); },
                OnCloseRequest = (sender) => { sender.Close(); }
            });
        }

        public ICommand AfegirTelefon { get { return new RelayCommand(AddTelefon); } }
        public ICommand AfegirEmail { get { return new RelayCommand(AddEmail); } }

        public void AddTelefon()
        {
            phone aux_phone = new phone();
            aux_phone.author_dni = Author.dni;
            this.Dialogs.Add(new AutorDialogViewModel
            {
                Title = "Afegir Telèfon",
                Telefon = aux_phone,
                DataText = "Número",
                OkText = "Afegeix",
                TextEnabled = true,
                TextEnabled_type = true,
                OnOk = (sender) =>
                {
                    Author.phones.Add(aux_phone);
                    FillTelefons();
                    sender.Close();
                },
                OnCancel = (sender) => { sender.Close(); },
                OnCloseRequest = (sender) => { sender.Close(); }
            });
        }

        public void AddEmail()
        {
            email aux_mail = new email();
            aux_mail.author_dni = Author.dni;
            this.Dialogs.Add(new AutorDialogViewModel
            {
                Title = "Afegir Email",
                Mail = aux_mail,
                DataText = "Adreça",
                OkText = "Afegeix",
                Type_item = "Email",
                TextEnabled = true,
                TextEnabled_type = false,
                OnOk = (sender) =>
                {
                    if (this.validate_mail(aux_mail.address))
                    {
                        Author.emails.Add(aux_mail);
                        FillEmails();
                        sender.Close();
                    }
                    else
                    {
                        MessageBox.Show("Email incorrecte. Insereixi un correu vàlid.");
                    }
                },
                OnCancel = (sender) => { sender.Close(); },
                OnCloseRequest = (sender) => { sender.Close(); }
            });
        }

        public ICommand EsborrarTelefon { get { return new RelayCommand(DeleteTelefon); } }

        public void DeleteTelefon()
        {

            phone aux_tel = new Model.phone();

            if (SelectedPhone != null)
            {
                aux_tel.id_phone = SelectedPhone.id_phone;
                aux_tel.num = SelectedPhone.num;
                aux_tel.type = SelectedPhone.type;
                aux_tel.author_dni = SelectedPhone.author_dni;
                this.Dialogs.Add(new AutorDialogViewModel
                {
                    Title = "Esborrar Telèfon",
                    Telefon = aux_tel,
                    Id_item = aux_tel.id_phone,
                    Type_item = aux_tel.type,
                    DataText = "Número",
                    Data_item = aux_tel.num,
                    TextEnabled = false,
                    TextEnabled_type = false,
                    OkText = "Esborrar",
                    OnOk = (sender) =>
                    {
                        Author.phones.Remove(SelectedPhone);
                        FillTelefons();
                        sender.Close();
                    },
                    OnCancel = (sender) => { sender.Close(); },
                    OnCloseRequest = (sender) => { sender.Close(); }
                });
            }
        }

        public ICommand EsborrarEmail { get { return new RelayCommand(DeleteEmail); } }

        public void DeleteEmail()
        {
            email aux_email = new email();

            if (SelectedEmail != null)
            {
                aux_email.author_dni = Author.dni;
                aux_email.id_email = SelectedEmail.id_email;
                aux_email.address = SelectedEmail.address;
                this.Dialogs.Add(new AutorDialogViewModel
                {
                    Title = "Esborrar Email",
                    Mail = aux_email,
                    Id_item = aux_email.id_email,
                    Type_item = "Email",
                    DataText = "Adreça",
                    Data_item = aux_email.address,
                    TextEnabled = false,
                    TextEnabled_type = false,
                    OkText = "Esborrar",
                    OnOk = (sender) =>
                    {

                        Author.emails.Remove(SelectedEmail);
                        FillEmails();
                        sender.Close();
                    },
                    OnCancel = (sender) => { sender.Close(); },
                    OnCloseRequest = (sender) => { sender.Close(); }

                });
            }
        }

        public ICommand OpenProjecte { get { return new RelayCommand(opProject); } }
        protected virtual void opProject()
        {
            bool valid = validate_dni(Author.dni);
            if (valid)
            {
                if (context.authors.Select(x => x.dni).Contains(Author.dni))
                {
                    context.authors.Where(x => x.dni == Author.dni).ToList()[0] = Author;
                }
                else
                {
                    if (Author != null && Author.dni != null)
                    {
                        context.authors.Add(Author);
                    }
                    else
                    {
                        return;
                    }
                }
                context.SaveChanges();
                IsVisible = false;
                this.Dialogs.Add(new ProjectesViewModel(Author, true, context)
                {
                    context = context,
                    Boto_afegir_enabled = true,
                    ProjectsL = new ObservableCollection<project>(Author.projects.ToList())
                });
                this.Close();
            }
            else
            {
                MessageBox.Show("DNI Invàlid.");
            }
        }



        public ICommand tornarEnrere { get { return new RelayCommand(Close); } }
        public Action<AutorViewModel> OnOk { get; set; }
        public Action<AutorViewModel> OnCancel { get; set; }
        public Action<AutorsViewModel> OnCloseRequest { get; set; }




        #endregion ICommand

        #region DialogClosing

        public void Close()
        {
            if (this.DialogClosing != null)
                this.DialogClosing(this, new EventArgs());
        }
        #endregion
        public bool validate_dni(String dni)
        {
            if (dni != null)
            {
                string patron = "[A-HJ-NP-SUVW][0-9]{7}[0-9A-J]|\\d{8}[TRWAGMYFPDXBNJZSQVHLCKE]|[X]\\d{7}[TRWAGMYFPDXBNJZSQVHLCKE]|[X]\\d{8}[TRWAGMYFPDXBNJZSQVHLCKE]|[YZ]\\d{0,7}[TRWAGMYFPDXBNJZSQVHLCKE]";
                string sRemp = "";
                bool ret = false;
                System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex(patron);
                sRemp = rgx.Replace(dni, "OK");
                if (sRemp == "OK") ret = true;
                return ret;
            }
            else
            {
                return false;
            }
        }
        public bool validate_mail(String email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

    }

}
