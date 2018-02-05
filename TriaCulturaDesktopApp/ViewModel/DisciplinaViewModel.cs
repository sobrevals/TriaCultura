﻿using System;
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
        private ObservableCollection<discipline> _disciplines_notAuthor;
        private ObservableCollection<discipline> _disciplines_fromAuthor;
        private discipline _selectedDiscipline_fromNotAuthor;
        private discipline _selectedDiscipline_fromAuthor;
        private author _selectedauthor;

        public discipline SelectedDiscipline_fromNotAuthor { get { return _selectedDiscipline_fromNotAuthor; } set { _selectedDiscipline_fromNotAuthor = value; NotifyPropertyChanged(); } }
        public discipline SelectedDiscipline_fromAuthor { get { return _selectedDiscipline_fromAuthor; } set { _selectedDiscipline_fromAuthor = value; NotifyPropertyChanged(); } }
        public ObservableCollection<discipline> Disciplines_fromAuthor { get { return _disciplines_fromAuthor; } set { _disciplines_fromAuthor = value; NotifyPropertyChanged(); } }
        public ObservableCollection<discipline> Disciplines_notAuthor { get { return _disciplines_notAuthor; } set { _disciplines_notAuthor = value; NotifyPropertyChanged(); } }
        public author SelectedAuthor { get { return _selectedauthor; } set { _selectedauthor = value; } }
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
        public DisciplinaViewModel()
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
        private void FillDisciplines_all(int index)
        {
            List<discipline> aux_list = context.disciplines.Distinct().ToList();
            if (SelectedAuthor != null)
            {
                aux_list.RemoveAll(x => x.author_dni.Equals(SelectedAuthor.dni));

                Disciplines_notAuthor = new ObservableCollection<discipline>(aux_list);
            }
            if (Disciplines_notAuthor != null && index >= 0 && index < Disciplines_notAuthor.Count)
            {
                SelectedDiscipline_fromNotAuthor = Disciplines_notAuthor[index];
            }
        }

        private void FillDisciplines_all(discipline d)
        {
            FillDisciplines_all(0);
            if (d != null && d.id_discipline != 0)
            {
                SelectedDiscipline_fromNotAuthor = Disciplines_notAuthor.Where(x => x.id_discipline == d.id_discipline).SingleOrDefault();
            }
        }

        private void FillDisciplines_author(int index)
        {
            if (SelectedAuthor != null)
            {
                Disciplines_fromAuthor = new ObservableCollection<discipline>(SelectedAuthor.disciplines.ToList());
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
            }
            else
            {
                this.Close();
            }
        }

        public ICommand CancelCommand { get { return new RelayCommand(Close); } }

        public void Close()
        {
            if (this.DialogClosing != null)
            {
                this.DialogClosing(this, new EventArgs());
            }
        }

        public ICommand AfegirDisciplina_autor { get { return new RelayCommand(AddDisciplina_autor); } }

        protected virtual void AddDisciplina_autor()
        {
            discipline d = SelectedDiscipline_fromNotAuthor;
            //context.disciplines.Where(x => x.id_discipline == d.id_discipline).SingleOrDefault().authors.Add(SelectedAuthor);
            SelectedAuthor.disciplines.Add(d);
            context.authors.Where(x => x.dni == SelectedAuthor.dni).SingleOrDefault().disciplines.Add(d);
            context.SaveChanges();
            SelectedAuthor = context.authors.Where(x => x.dni == SelectedAuthor.dni).SingleOrDefault();
            FillDisciplines_author(0);
            FillDisciplines_all(0);
        }

        public ICommand TreureDisciplina_autor { get { return new RelayCommand(RemDisciplina_autor); } }

        protected virtual void RemDisciplina_autor()
        {
            discipline d = SelectedDiscipline_fromAuthor;
            //context.disciplines.Where(x => x.id_discipline == d.id_discipline).SingleOrDefault().authors.Remove(SelectedAuthor);
            //context.authors.Where(x => x.dni == SelectedAuthor.dni).SingleOrDefault().disciplines.Remove(d);
            //context.disciplines.Where(x => x.id_discipline == d.id_discipline).SingleOrDefault().authors.Remove(SelectedAuthor);
            SelectedAuthor.disciplines.Remove(d);
            context.authors.Where(x => x.dni == SelectedAuthor.dni).SingleOrDefault().disciplines.Remove(d);
            context.SaveChanges();
            SelectedAuthor = context.authors.Where(x => x.dni == SelectedAuthor.dni).SingleOrDefault();
            FillDisciplines_author(0);
            FillDisciplines_all(0);
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
            if (SelectedDiscipline_fromNotAuthor != null)
            {
                discipline aux_disc = new discipline();
                aux_disc.id_discipline = SelectedDiscipline_fromNotAuthor.id_discipline;
                aux_disc.type = SelectedDiscipline_fromNotAuthor.type;
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
                            context.disciplines.Remove(SelectedDiscipline_fromNotAuthor);
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
