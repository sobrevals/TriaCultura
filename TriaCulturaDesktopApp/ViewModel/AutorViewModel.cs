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

namespace TriaCulturaDesktopApp.ViewModel
{
    class AutorViewModel : IUserDialogViewModel, INotifyPropertyChanged
    {
        #region Properties
        triaculturaCTXEntities context = new triaculturaCTXEntities();

        private author _author;
        private List<discipline> _disciplines;
        private List<phone> _telefons;
        private List<email> _emails;
        private Image _foto;

        public author Author { get { return _author; } set { _author = value; } }
        public List<discipline> Disciplines { get { return _disciplines; } set { _disciplines= value; } }
        public List<phone> Telefons { get { return _telefons; } set { _telefons= value; } }
        public List<email> Emails{ get { return _emails; } set { _emails= value; } }
        public Image Foto { get { return _foto; } set { _foto = value; } }

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
        #endregion
        public void RequestClose()
        {
            if (this.DialogClosing != null)
                this.DialogClosing(this, new EventArgs());
        }
    }
}
