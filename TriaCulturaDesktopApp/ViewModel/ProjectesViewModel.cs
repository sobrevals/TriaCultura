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
using System.Windows;

namespace TriaCulturaDesktopApp.ViewModel
{
    public class ProjectesViewModel : ViewModelBase, IUserDialogViewModel

    {
        public triaculturaDBEntities context { get; set; }

        private ObservableCollection<IDialogViewModel> _Dialogs = new ObservableCollection<IDialogViewModel>();
        public ObservableCollection<IDialogViewModel> Dialogs { get { return _Dialogs; } }

        #region BasicProperties

        private ObservableCollection<project> _projectsL;
        private int _selectedIndexProject;
        private project _selectedProject;
        private bool _boto_afegir_enabled;
        private author _author;
        private string _searchBoxText;

        public string SearchBoxText { get { return _searchBoxText; } set { _searchBoxText = value;  RaisePropertyChanged("SearchBoxText"); } }
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
            context = new triaculturaDBEntities();
            fillProjectes(0);
        }

        public ProjectesViewModel(author a, bool enable, triaculturaDBEntities ctx)
        {
            context = ctx;
            titol = "Projectes";
            Author = context.authors.Where(x => x.dni == a.dni).SingleOrDefault();
            fillProjectes(0);
        }

        public ProjectesViewModel(place p, triaculturaDBEntities ctx)
        {
            context = ctx;
            titol = "Projectes";
            fillProjectes(p);
        }

        #endregion

        #region FillProjectes

        public void fillProjectes (place p)
        {
            List<request> lr = context.requests.Where(x => x.place_id == p.id_place).ToList();
            List<project> lp = new List<project>(400);
            foreach (request r in lr)
            {
                lp.Add(r.project);
            }
            ProjectsL = new ObservableCollection<project>(lp);

        }

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

        public void fillProjectes (string filter)
        {
            ProjectsL = null;
            ProjectsL = new ObservableCollection<project>(context.projects
                .Where(x => x.title.Contains(filter)).OrderBy(x => x.id_project).ToList());

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

        public ICommand filtrarProjectes { get { return new RelayCommand(filtre); } }
        public ICommand tornarEnrere { get { return new RelayCommand(Close); } }
        public ICommand NewProjecte { get { return new RelayCommand(newProject); } }

        protected virtual void filtre()
        {
            fillProjectes(SearchBoxText);
        }

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
                    if (aux_project.files.Count > 0)
                    {
                        List<file> files_to_add = aux_project.files.ToList();
                        foreach (file f in files_to_add)
                        {
                            context.files.Add(f);
                        }
                    }

                    try
                    {
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error en esciure a la BBDD");
                    }
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
            if (SelectedIndexProject != null)
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
                    OnOk = (sender) =>
                    {
                        context.projects.Where(x => x.id_project == SelectedProject.id_project).SingleOrDefault().author_dni = aux_project.author_dni;
                        context.projects.Where(x => x.id_project == SelectedProject.id_project).SingleOrDefault().title = aux_project.title;
                        context.projects.Where(x => x.id_project == SelectedProject.id_project).SingleOrDefault().topic = aux_project.topic;
                        context.projects.Where(x => x.id_project == SelectedProject.id_project).SingleOrDefault().description = aux_project.description;

                        context.projects.Where(x => x.id_project == SelectedProject.id_project).SingleOrDefault().files = aux_project.files;
                        context.projects.Where(x => x.id_project == SelectedProject.id_project).SingleOrDefault().type = aux_project.type;
                        try
                        {
                            context.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error de validació");
                        }
                        fillProjectes(SelectedProject);
                        sender.Close();
                    },
                    OnCloseRequest = (sender) => { sender.Close(); },
                    OnReturn = (sender) => { sender.Close(); }
                });
            }
        }
        public ICommand TreureProjecte_author { get { return new RelayCommand(RemProjecte_autor); } }
        protected virtual void RemProjecte_autor()
        {
            if (SelectedProject != null)
            {
                project aux_project = new project();

                aux_project = SelectedProject;
                aux_project.id_project = SelectedProject.id_project;
                aux_project.author_dni = SelectedProject.author_dni;

                this.Dialogs.Add(new EsborrarProjecteFromProjectes()
                {
                    Project = aux_project,
                    Title = "Eliminar Projecte",
                    OkText = "Esborrar",
                    OnOk = (sender) =>
                    {
                        if (aux_project.requests.Count > 0)
                        {
                            List<request> requestDelList = aux_project.requests.ToList();

                            foreach (request item in requestDelList)
                            {
                                request r = context.requests.Where(x => x.id_request == item.id_request).SingleOrDefault();

                                context.requests.Remove(r);
                            }
                        }
                        if (aux_project.files.Count > 0)
                        {
                            List<file> fileDelList = aux_project.files.ToList();

                            foreach (file f in fileDelList)
                            {
                                file file = context.files.Where(x => x.id_file == f.id_file).SingleOrDefault();
                                context.files.Remove(file);
                            }
                        }
                        project p = context.projects.Where(x => x.id_project == aux_project.id_project).SingleOrDefault();
                        context.projects.Remove(p);
                        try
                        {
                            context.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error escrivint a la BBDD");
                        }
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

        public ICommand Veure_Espais { get { return new RelayCommand(veureEspais); } }

        public virtual void veureEspais()
        {

            if (SelectedProject != null)
            {
                project aux_project = new project();
                aux_project.id_project = SelectedProject.id_project;
                aux_project.requests = SelectedProject.requests;
                aux_project.author_dni = SelectedProject.author_dni;

                this.Dialogs.Add(new EspaiViewModel(aux_project, context)
                {
                    SelectedProject = aux_project,
                    context = context,
                    OnOk = (sender) =>
                    {
                        List<request> existing_requests = context.requests.Where(x => x.project_id == SelectedProject.id_project).ToList();
                        List<request> project_requests = aux_project.requests.ToList();
                        foreach (request r in project_requests)
                        {
                            if (!existing_requests.Select(x => x.place_id).Contains(r.place_id))
                            {
                                context.requests.Add(r);
                            }
                        }
                        try
                        {
                            context.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error en escriure a la BBDD");
                        }
                        sender.Close();
                    },
                    OnCancel = (sender) =>
                    {
                        sender.Close();
                    },
                    OnCloseRequest = (sender) =>
                    {
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
