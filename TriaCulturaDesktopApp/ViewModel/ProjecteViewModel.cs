using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MvvmDialogs.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using TriaCulturaDesktopApp.Model;
using System.Runtime.CompilerServices;
using System.Windows;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.IO;

namespace TriaCulturaDesktopApp.ViewModel
{
    class ProjecteViewModel : ViewModelBase, IUserDialogViewModel, INotifyPropertyChanged
    {
        //triaculturaCTXEntities context = new triaculturaCTXEntities();
        Available_Types tipus = new Available_Types();

        #region BasicProperties
        
        private project _projecte;
        private ObservableCollection<file> _files;
        private request _selectedRequest;
        private file _selectedFile;
        
        private List<string> _types;
        private string _selectedType;
        private int _selectedIndexType;


        public request SelectedRequest { get { return _selectedRequest; } set { _selectedRequest = value; NotifyPropertyChanged();RaisePropertyChanged("SelectedRequest"); } }

        public String titol { get; set; }

        public ObservableCollection<file> Files { get { return _files; } set { _files = value; NotifyPropertyChanged(); RaisePropertyChanged("Files"); } }

        public project Projecte
        {
            get
            { return _projecte; }

            set
            { _projecte = value;
                NotifyPropertyChanged();
            }
        }

        public file SelectedFile { get { return _selectedFile; } set { _selectedFile = value; NotifyPropertyChanged();RaisePropertyChanged("SelectedFile"); } }

        public List<string> Types
        {
            get
            {
                return _types;
            }

            set
            {
                _types = value;
            }
        }

        public string SelectedType
        {
            get
            {
                return _selectedType;
            }

            set
            {
                if (_selectedType == value) return;
                _selectedType = value;
                NotifyPropertyChanged();
                RaisePropertyChanged("SelectedType");
            }
        }

        public int SelectedIndexType
        {
            get
            {
                return _selectedIndexType;
            }

            set
            {
                _selectedIndexType = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<request> _requestL;
        public ObservableCollection<request> RequestL { get { return _requestL; } set { _requestL = value; NotifyPropertyChanged(); } }

        private ObservableCollection<IDialogViewModel> _Dialogs = new ObservableCollection<IDialogViewModel>();
        public ObservableCollection<IDialogViewModel> Dialogs { get { return _Dialogs; NotifyPropertyChanged(); } }

        #endregion BasicProperties

        #region isModal
        public bool IsModal
        {
            get
            {
                return true;
            }
        }

        #endregion isModal

        #region constructor
        public ProjecteViewModel(project p)
        {
            Projecte = p;
            titol = "Nou Projecte";
            FillRequests_all(0);
            FillTipus();
            FillFiles();
            RegisterCommands();
        }

        public ProjecteViewModel() { RegisterCommands(); }

        //public ProjecteViewModel(project p)
        //{
        //    Projecte = p;

        //    FillRequests_all(0);
        //}
        #endregion constructor

        #region fill
        private void FillRequests_all(int index)
        {
            if (Projecte != null)
            {
                RequestL = new ObservableCollection<request>(Projecte.requests.ToList());
                if (RequestL!= null && index >= 0 && index < RequestL.Count)
                {
                    SelectedRequest= RequestL[index];
                }
            }
        }

        public void FillTipus()
        {
           
            Types = tipus.types;
            if (Projecte.type != null)
            {
                string aux_type = Projecte.type;
                aux_type = Regex.Replace(aux_type, @"\s", "");
                for (int i = 0; i < Types.Count; i++)
                {
                    if (Types[i].Contains(aux_type))
                    {
                        SelectedType = Types[i];
                        SelectedIndexType = i;
                    }
                }
            } else
            {
                SelectedType = Types[0];
                SelectedIndexType=0;
            }
        }


        private void FillFiles()
        {
            Files = new ObservableCollection<file>(Projecte.files.ToList());
        }
        #endregion

        #region PropertyChanged

        public virtual event EventHandler DialogClosing;
        public event PropertyChangedEventHandler PropertyChanged;
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

        #region Commands

        public ICommand AfegirRequest_place { get { return new RelayCommand(AddPlace_request); } }

        protected virtual void AddPlace_request()
        {
            project aux_project = new project();
            aux_project.requests = Projecte.requests;
            aux_project.id_project = Projecte.id_project;
            //aux_project.author_dni = Projecte.author_dni;
            //aux_project.description = Projecte.description;
            //aux_project.files = Projecte.files;
            //aux_project.title= Projecte.title;
            //aux_project.topic= Projecte.topic;
            //aux_project.type= Projecte.type;
            
            this.Dialogs.Add(new EspaiViewModel(aux_project)
            {
               SelectedProject = aux_project,
               OnOk = (sender) =>
               {
                   Projecte.requests = aux_project.requests;
                   FillRequests_all(0);
                   sender.Close();
               },
               OnCancel = (sender) =>
               {
                   FillRequests_all(0);
                   sender.Close();
               },
               OnCloseRequest = (sender) =>
               {
                   FillRequests_all(0);
                   sender.Close();
               }

            });
        }
        public ICommand TreureRequest_place { get { return new RelayCommand(RemRequest_place); } }
        protected virtual void RemRequest_place()
        {

            //if (SelectedRequest != null)
            //{
            //    request aux_request = context.requests.Where(x => x.id_request == SelectedRequest.id_request).SingleOrDefault();
            //    try
            //    {
            //        //request p = SelectedRequest_fromPlace;
            //        Projecte.requests.Remove(aux_request);
            //        context.SaveChanges();
            //        FillRequest_place(0);
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("No es pot esborrar aquestsa place.");
            //    }
            //}
        }



       public ICommand Afegir_fitxer { get { return new RelayCommand(AddFitxer); } }
        protected virtual void AddFitxer()
        {

        }
        public ICommand Finalitzar { get { return new RelayCommand(SaveProject); } }

        public void SaveProject()
        {
            //if (context.projects.Select(x => x.id_project).ToList().Contains(Projecte.id_project))
            //{
            //    context.projects.Where(x => x.id_project == Projecte.id_project).ToList()[0] = Projecte;
            //}
            //else
            //{
            //    context.projects.Add(Projecte);
            //}
            //context.SaveChanges();
            //Close();
        }


        public ICommand OkCommand { get { return new RelayCommand(GuardarIEnrere); } }
        protected virtual void GuardarIEnrere()
        {
            Projecte.type = Types[SelectedIndexType];
            if (Projecte.title != null && Projecte.description != null && Projecte.topic != null && Projecte.type != null)
            {
                if (this.OnOk != null)
                    this.OnOk(this);
                else
                    Close();
            }else
            {
                MessageBox.Show("Falta omplir algun camp.", "Alert");
            }
        }

        public ICommand tornarEnrere { get { return new RelayCommand(Close); } }
        public void Close()
        {
            if (this.DialogClosing != null)
                this.DialogClosing(this, new EventArgs());
        }


        #endregion

        public Action<ProjecteViewModel> OnOk { get; set; }
        public Action<ProjecteViewModel> OnReturn { get; set; }
        public Action<ProjecteViewModel> OnCloseRequest { get; set; }


        #region OpenFile
        public ICommand RemoveFile { get { return new RelayCommand(eliminarFile); } }
        protected virtual void eliminarFile()
        {
            if (SelectedFile != null)
            {
                file aux_fitxer = new file();
                aux_fitxer.extension = SelectedFile.extension;
                aux_fitxer.path = SelectedFile.path;
                aux_fitxer.name= SelectedFile.name;
                aux_fitxer.id_file= SelectedFile.id_file;
                aux_fitxer.project_id= SelectedFile.project_id;

                List<file> aux_list = Projecte.files.ToList();
                foreach (file f in aux_list)
                {
                    if (f.id_file.Equals(aux_fitxer.id_file))
                    {
                        Projecte.files.Remove(f);
                    }
                }
                //Projecte.files.Remove(aux_fitxer);
                //context.SaveChanges();
                FillFiles();
            }
        }

        public RelayCommand OpenFile{ get; set; }
        private string _selectedPath;
        public string SelectedPath
        {
            get { return _selectedPath; }
            set
            {
                _selectedPath = value;
                RaisePropertyChanged("SelectedPath");
            }
        }

        private string _defaultPath;


        private void RegisterCommands()
        {
            OpenFile = new RelayCommand(ExecuteOpenFileDialog);
        }

        private void ExecuteOpenFileDialog()
        {
            var dialog = new OpenFileDialog { InitialDirectory = _defaultPath };
            dialog.ShowDialog();

            SelectedPath = dialog.FileName;
            afegirPaths(Projecte);



        }
        public void afegirPaths(project p)
        {
            if (Projecte != null)
            {
                file fitxer = new file();
                fitxer.project_id = p.id_project;
                fitxer.name = Path.GetFileNameWithoutExtension(SelectedPath);
                fitxer.extension = Path.GetExtension(SelectedPath);
                fitxer.path = SelectedPath;
                Projecte.files.Add(fitxer);
                FillFiles();
            }

        }
        #endregion
    }
}