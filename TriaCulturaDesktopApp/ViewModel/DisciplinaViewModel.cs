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

namespace TriaCulturaDesktopApp.ViewModel
{
    class DisciplinaViewModel : INotifyPropertyChanged, IUserDialogViewModel
    {
        triaculturaCTXEntities context = new triaculturaCTXEntities();
        #region BasicProperties

        private List<discipline> _disciplines;
        private List<discipline> _disciplines_fromAuthor;
        private discipline _selectedDiscipline_fromDisciplines;
        private discipline _selectedDiscipline_fromAuthor;
        private author _selectedauthor;

        public List<discipline> Disciplines
        { get { return _disciplines; } set { _disciplines = value; NotifyPropertyChanged(); } }

        public discipline SelectedDiscipline_fromDisciplines { get { return _selectedDiscipline_fromDisciplines; } set { _selectedDiscipline_fromDisciplines = value;NotifyPropertyChanged(); } }
        public discipline SelectedDiscipline_fromAuthor { get { return _selectedDiscipline_fromAuthor; } set { _selectedDiscipline_fromAuthor = value;NotifyPropertyChanged(); } }

        public author SelectedAuthor { get { return _selectedauthor; } set { _selectedauthor = value; }  }
        public List<discipline> Disciplines_fromAuthor
        {
            get
            {
                return _disciplines_fromAuthor;
            }

            set
            {
                _disciplines_fromAuthor = value;
            }
        }
        public bool IsModal
        {
            get
            {
                return true;
            }
        }


        private ObservableCollection<IDialogViewModel> _dialogs = new ObservableCollection<IDialogViewModel>();

        public ObservableCollection<IDialogViewModel> Dialogs { get { return _dialogs; } set { _dialogs = value; } }
        #endregion
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
            FillDisciplines_all(0);
            FillDisciplines_author(0);
            
        }
        public DisciplinaViewModel(author a)
        {
            SelectedAuthor = context.authors.Where(x => x.dni == a.dni).SingleOrDefault();
            FillDisciplines_all(0);
            FillDisciplines_author(0);
        }
        #region fill
        private void FillDisciplines_all (int index)
        {
            Disciplines = context.disciplines.OrderBy(x => x.type).ToList();
            if (Disciplines != null && index >= 0 && index < Disciplines.Count)
            {
                SelectedDiscipline_fromDisciplines = Disciplines[index];
            }
        }

        private void FillDisciplines_all(discipline d)
        {
            FillDisciplines_all(0);
            if (d!=null && d.id_discipline!=0)
            {
                SelectedDiscipline_fromDisciplines = Disciplines.Where(x => x.id_discipline == d.id_discipline).SingleOrDefault();
            }
        }

        private void FillDisciplines_author (int index)
        {
            if (SelectedAuthor != null)
            {
                Disciplines_fromAuthor = SelectedAuthor.disciplines.ToList();
                if (Disciplines_fromAuthor != null && index >= 0 && index < Disciplines_fromAuthor.Count)
                {
                    SelectedDiscipline_fromAuthor = Disciplines_fromAuthor[index];
                }
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
                this.Close();
            }
        }

        public ICommand CancelCommand { get { return new RelayCommand(Close); } }

        public void Close()
        {
            if (this.DialogClosing != null)
            {
                this.DialogClosing(this, new EventArgs ());
            }
        }

        public ICommand AfegirDisciplina_autor { get { return new RelayCommand(AddDisciplina_autor); } }

        protected virtual void AddDisciplina_autor()
        {
            discipline d = SelectedDiscipline_fromDisciplines;
            //context.disciplines.Where(x => x.id_discipline == d.id_discipline).SingleOrDefault().authors.Add(SelectedAuthor);
            context.authors.Where(x => x.dni == SelectedAuthor.dni).SingleOrDefault().disciplines.Add(d);
            //context.SaveChanges();
            FillDisciplines_author(0);
        }

        public ICommand TreureDisciplina_autor { get { return new RelayCommand(RemDisciplina_autor); } }

        protected virtual void RemDisciplina_autor()
        {
            discipline d = SelectedDiscipline_fromAuthor;
            context.disciplines.Where(x => x.id_discipline == d.id_discipline).SingleOrDefault().authors.Remove(SelectedAuthor);
            SelectedAuthor.disciplines.Remove(d);
            //context.SaveChanges();
            FillDisciplines_author(0);
        }

        public ICommand CrearDisciplina { get { return new RelayCommand(CreaDisciplina); } }

        protected void CreaDisciplina()
        {
            discipline aux_disc = new discipline();
            this.Dialogs.Add(new DisciplinaDialogViewModel
            {
                Title = "Crear Disciplina",
                Discipline = aux_disc,
                OkText = "Confirmar",
                TextEnabled = true,
                OnOk = (sender) =>
                {
                    context.disciplines.Add(aux_disc);
                    try
                    {
                        context.SaveChanges();
                        FillDisciplines_all(aux_disc);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    sender.Close();
                },
                    OnCancel = (sender) => { sender.Close(); },
                    OnCloseRequest = (sender) => { sender.Close(); }
            });
            
        }

        public ICommand EsborrarDisciplina { get { return new RelayCommand(EsborraDisciplina); } }

        protected void EsborraDisciplina()
        {
            if (SelectedDiscipline_fromDisciplines != null)
            {
                discipline aux_disc = new discipline();
                aux_disc.id_discipline = SelectedDiscipline_fromDisciplines.id_discipline;
                aux_disc.type = SelectedDiscipline_fromDisciplines.type;
                this.Dialogs.Add(new DisciplinaDialogViewModel
                {
                    Title = "Esborrar Disciplina",
                    Discipline = aux_disc,
                    OkText = "Confirmar",
                    TextEnabled = false,
                    OnOk = (sender) =>
                    {
                        try
                        {
                            context.disciplines.Remove(SelectedDiscipline_fromDisciplines);
                            context.SaveChanges();
                            FillDisciplines_all(0);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("No es pot esborrar la Disciplina ja que està vinculada a algun(s) autor(s).");
                        }
                        sender.Close();
                    },
                    OnCancel = (sender) => { sender.Close(); },
                    OnCloseRequest = (sender) => { sender.Close(); }

                });
            }
        }

        public Action<DisciplinaViewModel> OnOk { get; set; }
        public Action<DisciplinaViewModel> OnCancel { get; set; }
        public Action<DisciplinaViewModel> OnCloseRequest { get; set; }


        #endregion
    }
}
