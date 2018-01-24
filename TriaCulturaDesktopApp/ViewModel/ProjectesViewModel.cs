using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TriaCulturaDesktopApp.Model;
using MvvmDialogs.ViewModels;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace TriaCulturaDesktopApp.ViewModel
{
    class ProjectesViewModel : ViewModelBase, IUserDialogViewModel

    {
        triaculturaCTXEntities context = new triaculturaCTXEntities();

        private ObservableCollection<IDialogViewModel> _Dialogs = new ObservableCollection<IDialogViewModel>();
        public ObservableCollection<IDialogViewModel> Dialogs { get { return _Dialogs; } }

        #region BasicProperties

        private List<project> _projectsL;
        private int _selectedIndexProject;
        private project _selectedProject;

        public List<project> ProjectsL
        {
            get
            {
                return _projectsL;
            }

            set
            {
                _projectsL = value;
            }
        }

        public int SelectedIndexProject
        {
            get
            {
                return _selectedIndexProject;
            }

            set
            {
                _selectedIndexProject = value;
            }
        }

        public project SelectedProject
        {
            get
            {
                return _selectedProject;
            }

            set
            {
                _selectedProject = value;
            }
        }
        #endregion

        #region Contructor
        public ProjectesViewModel()
        {
            fillProjectes(0);
        }

        #endregion

        #region FillProjectes

        public void fillProjectes(int n)
        {            
            ProjectsL= context.projects.OrderBy(x => x.id_project).ToList();

            if (ProjectsL != null)
            {
                SelectedProject = ProjectsL[n];
            }
        }

        #endregion

        #region RequestClose
        public void RequestClose()
        {
            this.DialogClosing(this, null);
        }

        public event EventHandler DialogClosing;

        public void Close()
        {
            if (this.DialogClosing != null)
                this.DialogClosing(this, new EventArgs());
        }

        #endregion

        #region ICommand
        public ICommand OpenProjecte { get { return new RelayCommand(opProject); } }
        protected virtual void opProject()
        {
                this.Dialogs.Add(new ProjecteViewModel());  
        }

        public Action<ProjectesViewModel> OnOk { get; set; }
        #endregion ICommand

        #region IsModal
        public virtual bool IsModal
        {
            get
            {
                return true;
            }
        }

        #endregion IsModal
    }


}
