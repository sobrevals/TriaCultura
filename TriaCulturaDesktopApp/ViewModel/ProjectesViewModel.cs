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
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TriaCulturaDesktopApp.ViewModel
{
    public class ProjectesViewModel : ViewModelBase, IUserDialogViewModel

    {
        triaculturaCTXEntities context = new triaculturaCTXEntities();

        private ObservableCollection<IDialogViewModel> _Dialogs = new ObservableCollection<IDialogViewModel>();
        public ObservableCollection<IDialogViewModel> Dialogs { get { return _Dialogs; } }

        #region BasicProperties

        private ObservableCollection<project> _projectsL;
        private int _selectedIndexProject;
        private project _selectedProject;
        private bool _boto_afegir_enabled;
        private author _author;
        public String titol { get; set; }
        public ObservableCollection<project> ProjectsL { get { return _projectsL; } set { _projectsL = value; RaisePropertyChanged("ProjectsL"); } }

        public int SelectedIndexProject
        {
            get
            {
                return _selectedIndexProject;
            }

            set
            {
                _selectedIndexProject = value;
                NotifyPropertyChanged();
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
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region Contructor
        public ProjectesViewModel()
        {
            fillProjectes(0);
        }

        public ProjectesViewModel(author a, bool enable)
        {
            titol = "Projectes";
            Author = context.authors.Where(x => x.dni == a.dni).SingleOrDefault();
            fillProjectes(0);
        }


        #endregion

        #region FillProjectes

        public void fillProjectes(int n)
        {
            if (Author == null)
            {
                ProjectsL = new ObservableCollection<project>(context.projects.OrderBy(x => x.id_project).ToList());
            }
            else
            {
                ProjectsL = new ObservableCollection<project>(Author.projects.OrderBy(x => x.id_project).ToList());
            }
            if (ProjectsL != null && ProjectsL.Count != 0)
            {
                SelectedProject = ProjectsL[n];
            }
        }

        public void fillProjectes(project p)
        {
            fillProjectes(0);
            if (p != null && p.id_project != 0)
            {
                SelectedProject = ProjectsL.Where(x => x.id_project == p.id_project).SingleOrDefault();
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
        public ICommand tornarEnrere { get { return new RelayCommand(Close); } }
        public ICommand NewProjecte { get { return new RelayCommand(newProject); } }
        protected virtual void newProject()
        {
            project aux_project = new project();
            aux_project.author_dni = Author.dni;
            this.Dialogs.Add(new ProjecteViewModel(aux_project)
            {
                Projecte = aux_project,
                titol = "Nou Projecte",
                OnOk = (sender) =>
                {
                    context.projects.Add(aux_project);
                    context.SaveChanges();
                    fillProjectes(aux_project);
                    sender.Close();
                },
                OnCloseRequest = (sender) => { sender.Close(); },
                OnReturn = (sender) => { sender.Close(); }
            });
        }
        public ICommand ChangeProjecte { get { return new RelayCommand(modProjecte); } }
        protected virtual void modProjecte()
        {
            project aux_project = new project();
            aux_project.id_project = SelectedProject.id_project;
            aux_project.author_dni = SelectedProject.author_dni;
            aux_project.description = SelectedProject.description;
            aux_project.title = SelectedProject.title;
            aux_project.topic = SelectedProject.topic;
            aux_project.requests = SelectedProject.requests;
            aux_project.files = SelectedProject.files;
            aux_project.type = SelectedProject.type;
            this.Dialogs.Add(new ProjecteViewModel(aux_project)
            {
                Projecte = aux_project,
                titol = "Modificar Projecte",
                //SelectedType = aux_project.type,
                OnOk = (sender) =>
                {
                    context.projects.Where(x => x.id_project == SelectedProject.id_project).SingleOrDefault().author_dni = aux_project.author_dni;
                    context.projects.Where(x => x.id_project == SelectedProject.id_project).SingleOrDefault().title = aux_project.title;
                    context.projects.Where(x => x.id_project == SelectedProject.id_project).SingleOrDefault().topic = aux_project.topic;
                    context.projects.Where(x => x.id_project == SelectedProject.id_project).SingleOrDefault().description = aux_project.description;

                    //List<request> aux_request_list = aux_project.requests.ToList();
                    //foreach (request r in aux_request_list) { context.projects.Where(x => x.id_project == SelectedProject.id_project).SingleOrDefault().requests.Add(r); }
                    context.projects.Where(x => x.id_project == SelectedProject.id_project).SingleOrDefault().requests = aux_project.requests;

                    context.projects.Where(x => x.id_project == SelectedProject.id_project).SingleOrDefault().files = aux_project.files;
                    context.projects.Where(x => x.id_project == SelectedProject.id_project).SingleOrDefault().type = aux_project.type;
                    context.SaveChanges();
                    fillProjectes(SelectedProject);
                    sender.Close();
                },
                OnCloseRequest = (sender) => { sender.Close(); },
                OnReturn = (sender) => { sender.Close(); }
            });
        }
        public ICommand TreureProjecte_author { get { return new RelayCommand(RemProjecte_autor); } }
        protected virtual void RemProjecte_autor()
        {
            if (SelectedProject != null)
            {
                project aux_project = new project();

                aux_project = SelectedProject;
                aux_project.author_dni = SelectedProject.author_dni;

                this.Dialogs.Add(new EsborrarProjecteFromProjectes()
                {
                    Project = aux_project,
                    Title = "Eliminar Projecte",
                    OkText = "Delete",
                    OnOk = (sender) =>
                    {
                        List<request> requestDelList = aux_project.requests.ToList();

                        foreach (request item in requestDelList)
                        {
                            request r = context.requests.Where(x => x.id_request == item.id_request).SingleOrDefault();

                            context.requests.Remove(r);
                        }
                        context.projects.Remove(aux_project);
                        context.SaveChanges();
                        fillProjectes(0);
                        sender.Close();
                    },
                    OnCancel = (sender) =>
                    {
                        fillProjectes(0);
                        sender.Close();
                    },
                    OnCloseRequest = (sender) =>
                    {
                        fillProjectes(0);
                        sender.Close();
                    }
                });
            }
        }

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion PropertyChanged


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

        public bool Boto_afegir_enabled
        {
            get
            {
                return _boto_afegir_enabled;
            }

            set
            {
                _boto_afegir_enabled = value;
            }
        }

        public author Author
        {
            get
            {
                return _author;
            }

            set
            {
                _author = value;
            }
        }
        #endregion IsModal


    }


}
