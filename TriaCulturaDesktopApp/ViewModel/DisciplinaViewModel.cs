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

namespace TriaCulturaDesktopApp.ViewModel
{
    class DisciplinaViewModel : INotifyPropertyChanged, IUserDialogViewModel
    {
        triaculturaDBEntities context = new triaculturaDBEntities();
        #region BasicProperties

        private List<discipline> _disciplines;
        private discipline _selectedDiscipline;

        public List<discipline> Disciplines
        { get { return _disciplines; } set { _disciplines = value; } }

        public discipline SelectedDiscipline { get { return _selectedDiscipline; } set { _selectedDiscipline = value; } }

        public bool IsModal
        {
            get
            {
                return true;
            }
        }
        #endregion

        private ObservableCollection<IDialogViewModel> _dialogs = new ObservableCollection<IDialogViewModel>();

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler DialogClosing;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void RequestClose()
        {
            this.DialogClosing(this, null);
        }
        #endregion
        public DisciplinaViewModel ()
        {
            FillDisciplines(0); // crear model 
        }
        #region fill
        private void FillDisciplines (int index)
        {
            Disciplines = context.disciplines.OrderBy(x => x.type).ToList();
            if (Disciplines != null && index >= 0 && index < Disciplines.Count)
            {
                SelectedDiscipline = Disciplines[index];
            }
        }
        #endregion
        #region Commands
        public ICommand OkCommand { get { return new RelayCommand(Ok); } }
        protected virtual void Ok()
        {
            if (this.OnOk != null)
            {
                this.OnOk(this);
            } else
            {
               
            }
        }

        public Action<DisciplinaViewModel> OnOk { get; set; }
        public Action<DisciplinaViewModel> OnCancel { get; set; }
        public Action<DisciplinaViewModel> OnCloseRequest { get; set; }
        #endregion
    }
}
