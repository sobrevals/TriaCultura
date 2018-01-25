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


        public author Author { get { return _author; } set { _author = value; } }
        public List<discipline> Disciplines { get { return _disciplines; } set { _disciplines= value; } }
        public List<phone> Telefons { get { return _telefons; } set { _telefons= value; } }
        public List<email> Emails{ get { return _emails; } set { _emails= value; } }
        public Image Foto { get { return _foto; } set { _foto = value; } }

        public phone SelectedPhone { get { return _selectedPhone; } set { _selectedPhone = value; } }

        public email SelectedEmail { get { return _selectedEmail; } set { _selectedEmail = value; } }

        public String Titol { get; set; }

        public List<string> DisciplinesL { get { return Disciplines.Select(x => x.type).ToList(); } }
        public List<string> TelefonsL { get { return Telefons.Select(x => x.num).ToList(); } }
        public List<string> EmailsL { get { return Emails.Select(x => x.address).ToList(); } }

        private ObservableCollection<IDialogViewModel> _dialogs = new ObservableCollection<IDialogViewModel>();

        public ObservableCollection<IDialogViewModel> Dialogs { get { return _dialogs; } set { _dialogs = value; } }

        public bool IsModal
        {
            get
            {
                return true;
            }
        }

        public event EventHandler DialogClosing;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
        public AutorViewModel ()
        {
            author a = context.authors.Where(x => x.name.Contains("milc")).SingleOrDefault();
            Titol = "Modificar Autor";
            Author = a;
            Disciplines = a.disciplines.ToList();
            Telefons = a.phones.ToList();
            Emails = a.emails.ToList();
        }
        #region Fills
        public void FillDisciplines ()
        {
            Disciplines = Author.disciplines.ToList();
        }

        public void FillTelefons ()
        {
            Telefons = Author.phones.ToList();
        }

        public void FillEmails ()
        {
            Emails = Author.emails.ToList();
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
            this.Dialogs.Add(new DisciplinaViewModel(Author)
            {          
                SelectedAuthor = Author, // es torna null cada vegada...      
                OnOk = (sender) =>
                {
                    context.SaveChanges();
                    FillDisciplines();
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
            this.Dialogs.Add(new AutorDialogViewModel
            {
                Title = "Afegir Telefon",
                Telefon = aux_phone,
                DataText = "Numero",
                OkText = "Afegeix",
                OnOk = (sender) =>
                {
                    context.phones.Add(aux_phone);
                    try
                    {
                        context.SaveChanges();
                        FillTelefons();
                    } catch (Exception ex)
                    {
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
            this.Dialogs.Add(new AutorDialogViewModel
            {
                Title = "Afegir Telefon",
                Mail = aux_mail,
                DataText = "Adreça",
                OkText = "Afegeix",
                OnOk = (sender) =>
                {
                    context.emails.Add(aux_mail);
                    try
                    {
                        context.SaveChanges();
                        FillTelefons();
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
            if (SelectedPhone != null)
            {
                phone aux_phone = new phone();
                aux_phone.id_phone = SelectedPhone.id_phone;
                aux_phone.num = SelectedPhone.num;
                aux_phone.type = SelectedPhone.type;
                this.Dialogs.Add(new AutorDialogViewModel
                {
                    Title = "Esborrar Telèfon",
                    Telefon = aux_phone,
                    Id_item = aux_phone.id_phone,
                    Type_item = aux_phone.type,
                    DataText = "Numero",
                    Data_item = aux_phone.num,
                    TextEnabled = false,
                    OkText = "Esborra",
                    OnOk = (sender) =>
                    {
                        try
                        {
                            context.phones.Remove(aux_phone);
                            context.SaveChanges();
                            FillTelefons();
                        } catch (Exception ex)
                        {
                            MessageBox.Show("No es pot esborrar aquest telèfon.");
                        }
                    },
                    OnCancel = (sender) => { sender.Close(); },
                    OnCloseRequest = (sender) => { sender.Close(); }

                });
            }
        }

        public ICommand EsborrarEmail { get { return new RelayCommand(DeleteEmail); } }

        public void DeleteEmail()
        {
            if (SelectedEmail!= null)
            {
                email aux_email = new email();
                aux_email.id_email= SelectedEmail.id_email;
                aux_email.address= SelectedEmail.address;
                this.Dialogs.Add(new AutorDialogViewModel
                {
                    Title = "Esborrar Telèfon",
                    Mail = aux_email,
                    Id_item = aux_email.id_email,
                    Type_item = "Email",
                    DataText = "Adreça",
                    Data_item = aux_email.address,
                    TextEnabled = false,
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
