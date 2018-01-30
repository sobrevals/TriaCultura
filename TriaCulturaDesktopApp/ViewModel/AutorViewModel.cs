using System;
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
        triaculturaCTXEntities context = new triaculturaCTXEntities();

        private author _author;
        private List<discipline> _disciplines;
        private List<phone> _telefons;
        private List<email> _emails;
        private Image _foto;
        private phone _selectedPhone;
        private email _selectedEmail;
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
        #endregion

        public author Author { get { return _author; } set { _author = value; NotifyPropertyChanged("Author"); } }
        public List<discipline> Disciplines { get { return _disciplines; } set { _disciplines = value; NotifyPropertyChanged("Disciplines"); } }
        public List<phone> Telefons { get { return _telefons; } set { _telefons = value; NotifyPropertyChanged("Telefons"); } }
        public List<email> Emails { get { return _emails; } set { _emails = value; NotifyPropertyChanged("Emails"); } }
        public Image Foto { get { return _foto; } set { _foto = value; NotifyPropertyChanged("Foto"); } }

        public phone SelectedPhone { get { return _selectedPhone; } set { _selectedPhone = value; } }

        public email SelectedEmail { get { return _selectedEmail; } set { _selectedEmail = value; } }

        public String Titol { get; set; }

        //public List<string> DisciplinesL { get { return Disciplines.Select(x => x.type).ToList(); }  }
        //public List<string> TelefonsL { get { return Telefons.Select(x => x.num).ToList(); }  }
        //public List<string> EmailsL { get { return Emails.Select(x => x.address).ToList(); }  }
        private ObservableCollection<string> _telefonsL;
        private ObservableCollection<string> _disciplinesL;
        private ObservableCollection<string> _emailsL;
        public ObservableCollection<string> DisciplinesL { get { return _disciplinesL; } set { _disciplinesL = value; NotifyPropertyChanged(); } }
        public ObservableCollection<string> TelefonsL { get { return _telefonsL; } set { _telefonsL = value; NotifyPropertyChanged(); } }
        public ObservableCollection<string> EmailsL { get { return _emailsL; } set { _emailsL = value; NotifyPropertyChanged(); } }

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
            Titol = "Modificar Autor";
            Author = context.authors.Where(x => x.dni == a.dni).SingleOrDefault();
            FillDisciplines();
            FillTelefons();
            FillEmails();
        }

        #region Fills
        public void FillDisciplines()
        {
            Disciplines = Author.disciplines.ToList();
            DisciplinesL = new ObservableCollection<string>(Disciplines.Select(x => x.type).ToList());
        }

        public void FillTelefons()
        {
            Telefons = Author.phones.ToList();
            TelefonsL = new ObservableCollection<string>(Telefons.Select(x => x.num).ToList());
        }

        public void FillEmails()
        {
            Emails = Author.emails.ToList();
            EmailsL = new ObservableCollection<string>(Emails.Select(x => x.address).ToList());
        }

        #endregion
        public void RequestClose()
        {
            if (this.DialogClosing != null)
                this.DialogClosing(this, new EventArgs());
        }

        #region ICommand
        public ICommand OkCommand { get { return new RelayCommand(Ok); } }
        protected virtual void Ok()
        {
            if (this.OnOk != null)
                this.OnOk(this);
            else
                Close();
        }

        public ICommand ObreDisciplines { get { return new RelayCommand(OpenDisciplines); } }

        public void OpenDisciplines()
        {
            List<discipline> disciplines_before = Author.disciplines.ToList();
            this.Dialogs.Add(new DisciplinaViewModel(Author)
            {
                SelectedAuthor = Author,
                OnOk = (sender) =>
                {
                    FillDisciplines();
                    sender.Close();
                },
                OnCancel = (sender) =>
                {
                    sender.Close();
                    Author.disciplines = disciplines_before;
                    context.authors.Where(x => x.dni == Author.dni).SingleOrDefault().disciplines = disciplines_before;
                    context.SaveChanges();
                },
                OnCloseRequest = (sender) =>
                {
                    sender.Close();
                    Author.disciplines = disciplines_before;
                    context.authors.Where(x => x.dni == Author.dni).SingleOrDefault().disciplines = disciplines_before;
                    context.SaveChanges();
                }
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
                Title = "Afegir Telefon",
                Telefon = aux_phone,
                DataText = "Numero",
                OkText = "Afegeix",
                TextEnabled = true,
                TextEnabled_type = true,
                OnOk = (sender) =>
                {
                    context.phones.Add(aux_phone);
                    try
                    {
                        context.SaveChanges();
                        FillTelefons();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (var validationException in ex.EntityValidationErrors)
                        {
                            foreach (var validationErrors in validationException.ValidationErrors)
                            {
                                Trace.TraceInformation("Property: {0} Error: {1}", validationErrors.PropertyName, validationErrors.ErrorMessage);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        MessageBox.Show("Error en escriure a la BBDD.");
                    }
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
                TextEnabled = true,
                TextEnabled_type = false,
                OnOk = (sender) =>
                {
                    context.emails.Add(aux_mail);
                    try
                    {
                        context.SaveChanges();
                        FillEmails();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error en escriure a la BBDD.");
                    }
                    sender.Close();
                },
                OnCancel = (sender) => { sender.Close(); },
                OnCloseRequest = (sender) => { sender.Close(); }
            });


        }
        public ICommand EsborrarTelefon { get { return new RelayCommand(DeleteTelefon); } }

        public void DeleteTelefon()
        {
            SelectedPhone = context.phones.Where(x => x.num.Equals(SelectedTelefonNum)).SingleOrDefault();
            if (SelectedPhone != null)
            {
                phone aux_phone = context.phones.Where(x => x.num.Equals(SelectedTelefonNum)).SingleOrDefault();
                //aux_phone.id_phone = SelectedPhone.id_phone;
                //aux_phone.num = SelectedPhone.num;
                //aux_phone.type = SelectedPhone.type;
                this.Dialogs.Add(new AutorDialogViewModel
                {
                    Title = "Esborrar Telèfon",
                    Telefon = aux_phone,
                    Id_item = aux_phone.id_phone,
                    Type_item = aux_phone.type,
                    DataText = "Numero",
                    Data_item = aux_phone.num,
                    TextEnabled = false,
                    TextEnabled_type = false,
                    OkText = "Esborra",
                    OnOk = (sender) =>
                    {
                        try
                        {
                            context.phones.Remove(aux_phone);
                            context.SaveChanges();
                            FillTelefons();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("No es pot esborrar aquest telèfon.");
                        }
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
            SelectedEmail = context.emails.Where(x => x.address.Equals(SelectedEmailAddr)).SingleOrDefault();
            if (SelectedEmail != null)
            {
                email aux_email = context.emails.Where(x => x.id_email == SelectedEmail.id_email).SingleOrDefault();
                //aux_email.id_email= SelectedEmail.id_email;
                //aux_email.address= SelectedEmail.address;
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
                    OkText = "Esborra",
                    OnOk = (sender) =>
                    {
                        try
                        {
                            context.emails.Remove(aux_email);
                            context.SaveChanges();
                            FillEmails();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("No es pot esborrar aquesta adreça.");
                        }
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
            this.Dialogs.Add(new ProjectesViewModel
            {
                ProjectsL = Author.projects.ToList()
            });
        }

        public ICommand tornarEnrere { get { return new RelayCommand(Close); } }
        public Action<AutorViewModel> OnOk { get; set; }


        #endregion ICommand

        #region DialogClosing

        public void Close()
        {
            if (this.DialogClosing != null)
                this.DialogClosing(this, new EventArgs());
        }
        #endregion
    }
}
