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

namespace TriaCulturaDesktopApp.ViewModel
{
    class DisciplinaViewModel : INotifyPropertyChanged
    {
        triaculturaEntities context = new Model.triaculturaEntities();
        #region BasicProperties

        private List<discipline> _disciplines;
        private discipline _selectedDiscipline;

        public List<discipline> Disciplines
        { get { return _disciplines; } set { _disciplines = value; } }

        public discipline SelectedDiscipline { get { return _selectedDiscipline; } set { _selectedDiscipline = value; } }
        #endregion

        private ObservableCollection<IDialogViewModel> _dialogs = new ObservableCollection<IDialogViewModel>();

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;


       
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
        public DisciplinaViewModel ()
        {
            FillDisciplines(0); // crear model 
        }
    }
}
