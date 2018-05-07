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

        #region BasicProperties

        private project _projecte;
        private ObservableCollection<file> _files;
        private request _selectedRequest;
        private file _selectedFile;
        private ObservableCollection<type> _available_types;

        private List<string> _types;
        private type _selectedType;
        private int _selectedIndexType;


        public request SelectedRequest { get { return _selectedRequest; } set { _selectedRequest = value; NotifyPropertyChanged(); RaisePropertyChanged("SelectedRequest"); } }

        public String titol { get; set; }

        public ObservableCollection<file> Files { get { return _files; } set { _files = value; NotifyPropertyChanged(); RaisePropertyChanged("Files"); } }
        public ObservableCollection<type> Available_Types { get { return _available_types; } set { _available_types = value; NotifyPropertyChanged(); RaisePropertyChanged("Types"); } }

        private triaculturaDBEntities context = new triaculturaDBEntities();

        public project Projecte
        {
            get
            { return _projecte; }

            set
            {
                _projecte = value;
                NotifyPropertyChanged();
            }
        }

        public file SelectedFile { get { return _selectedFile; } set { _selectedFile = value; NotifyPropertyChanged(); RaisePropertyChanged("SelectedFile"); } }

        public type SelectedType
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

        #endregion constructor

        #region fill
        private void FillRequests_all(int index)
        {
            if (Projecte != null)
            {
                RequestL = new ObservableCollection<request>(Projecte.requests.ToList());
                if (RequestL != null && index >= 0 && index < RequestL.Count)
                {
                    SelectedRequest = RequestL[index];
                }
            }
        }

        public void FillTipus()
        {

            Available_Types = new ObservableCollection<type>(context.types.ToList());
            if (Projecte.type != null)
            {
                string aux_type = Projecte.type;
                aux_type = Regex.Replace(aux_type, @"\s", "");
                int ind = 0;
                foreach (type t in Available_Types)
                {
                    if (t.description.Contains(aux_type))
                    {
                        SelectedType = t;
                        SelectedIndexType = ind;
                    }
                    ind++;
                }
            }
            else
            {
                SelectedType = Available_Types[0];
                SelectedIndexType = 0;
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

        public ICommand AfegirType { get { return new RelayCommand(add_type); } }

        protected virtual void add_type()
        {

            type aux_type = new Model.type();
            this.Dialogs.Add(new TypeDialogViewModel
            {
                Title = "Afegir tipus",
                Selected_type = aux_type,
                TextEnabled = true,
                OkText = "Afegir",
                OnOk = (sender) =>
                {
                    try
                    {
                        context.types.Add(aux_type);
                        context.SaveChanges();
                        FillTipus();
                        sender.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                },
                OnCancel = (sender) => { sender.Close(); },
                OnCloseRequest = (sender) => { sender.Close(); }
            });
        }

        public ICommand TreureType { get { return new RelayCommand(rem_type); } }

        protected virtual void rem_type()
        {
            type aux_type = SelectedType;
            this.Dialogs.Add(new TypeDialogViewModel
            {
                Title = "Esborrar Tipus",
                Selected_type = aux_type,
                OkText = "Confirmar",
                TextEnabled = false,
                OnOk = (sender) =>
                {
                    try
                    {
                        if (context.projects.Where(x => x.type.Equals(aux_type.description)).ToList().Count < 1)
                        {
                            context.types.Remove(aux_type);
                            context.SaveChanges();
                            FillTipus();
                        }
                        else
                        {
                            MessageBox.Show("Aquest tipus és utilitzat per altres projectes. No es pot eliminar.");
                        }
                        sender.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                },
                OnCancel = (sender) => { sender.Close(); },
                OnCloseRequest = (sender) => { sender.Close(); }
            });

        }
        public ICommand OkCommand { get { return new RelayCommand(GuardarIEnrere); } }
        protected virtual void GuardarIEnrere()
        {
            Projecte.type = Available_Types[SelectedIndexType].description;
            if (Projecte.title != null && Projecte.description != null && Projecte.topic != null && Projecte.type != null)
            {
                if (this.OnOk != null)
                    this.OnOk(this);
                else
                    Close();
            }
            else
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
        #region OpenFile

        public ICommand RemoveFile { get { return new RelayCommand(eliminarFile); } }
        protected virtual void eliminarFile()
        {
            if (SelectedFile != null)
            {
                file aux_fitxer = new file();
                aux_fitxer.extension = SelectedFile.extension;
                aux_fitxer.path = SelectedFile.path;
                aux_fitxer.name = SelectedFile.name;
                aux_fitxer.id_file = SelectedFile.id_file;
                aux_fitxer.project_id = SelectedFile.project_id;

                List<file> aux_list = Projecte.files.ToList();
                foreach (file f in aux_list)
                {
                    if (f.id_file.Equals(aux_fitxer.id_file))
                    {
                        Projecte.files.Remove(f);
                    }
                }
                FillFiles();
            }
        }

        public RelayCommand OpenFile { get; set; }
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
                fitxer.file_content = File.ReadAllBytes(SelectedPath);
                Projecte.files.Add(fitxer);
                FillFiles();
            }

        }
        #endregion

        #endregion

        public Action<ProjecteViewModel> OnOk { get; set; }
        public Action<ProjecteViewModel> OnReturn { get; set; }
        public Action<ProjecteViewModel> OnCloseRequest { get; set; }



    }
}