﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmDialogs.ViewModels;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TriaCulturaDesktopApp.Model;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Windows;

namespace TriaCulturaDesktopApp.ViewModel
{
    public class AutorsViewModel : ViewModelBase, IUserDialogViewModel
    {
        private ObservableCollection<IDialogViewModel> _Dialogs = new ObservableCollection<IDialogViewModel>();
        public ObservableCollection<IDialogViewModel> Dialogs { get { return _Dialogs; } }
        triaculturaCTXEntities context = new triaculturaCTXEntities();

        #region BasicProperties

        private ObservableCollection<author> _authors;
        private int _selectedIndexAuthor;
        private author _selectedAuthor;
        private project _project;

        public AutorsViewModel()
        {
            FillAuthors(0);
        }
        public ObservableCollection<author> AuthorsL
        {
            get { return (_authors); }
            set
            {
                _authors = value;
                NotifyPropertyChanged();
            }
        }

        public int SelectedIndexAuthor
        {
            get { return _selectedIndexAuthor; }
            set { _selectedIndexAuthor = value; NotifyPropertyChanged(); }
        }

        public author SelectedAuthor
        {
            get { return (_selectedAuthor); }
            set
            {
                _selectedAuthor = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region IsModal
        public virtual bool IsModal
        {
            get
            {
                return true;
            }
        }

        #endregion IsModal

        #region ICommand
        public ICommand OkCommand { get { return new RelayCommand(Ok); } }
        protected virtual void Ok()
        {
            if (this.OnOk != null)
                this.OnOk(this);
            else
                Close();
        }

        public ICommand OpenProjecte { get { return new RelayCommand(opProject); } }
        protected virtual void opProject()
        {
            if (SelectedAuthor!=null)
            {
                this.Dialogs.Add(new ProjectesViewModel(SelectedAuthor, true)
                {
                    Boto_afegir_enabled = true,
                    ProjectsL = SelectedAuthor.projects.ToList()
                });
            }
        }

        public ICommand afegirAutor { get { return new RelayCommand(addAutor); } }
        protected virtual void addAutor()
        {
            author aux_author = new author();
            this.Dialogs.Add(new AutorViewModel
            {
                Author = aux_author,
                Titol = "Nou Autor",
                OnOk = (sender) =>
                {
                    try
                    {
                        List<author> subList = context.authors.ToList();
                        if (!subList.Exists(x => x.dni == aux_author.dni))
                        {
                            context.authors.Add(aux_author);
                            context.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    FillAuthors(0);
                    sender.Close();
                },
                OnCancel = (sender) =>
                {
                    FillAuthors(0);
                    sender.Close();
                },
                OnCloseRequest = (sender) =>
                {
                    FillAuthors(0);
                    sender.Close();
                }
            });
        }

        public ICommand modificarAutor { get { return new RelayCommand(updAutor); } }

        protected virtual void updAutor()
        {
            if (SelectedAuthor != null)
            {
                author aux_author = SelectedAuthor;
                aux_author.dni = SelectedAuthor.dni;
                aux_author.address = SelectedAuthor.address;
                aux_author.disciplines = SelectedAuthor.disciplines;
                aux_author.name = SelectedAuthor.name;
                aux_author.surname = SelectedAuthor.surname;
                aux_author.emails = SelectedAuthor.emails;
                aux_author.phones = SelectedAuthor.phones;
                aux_author.projects = SelectedAuthor.projects;
                this.Dialogs.Add(new AutorViewModel(aux_author)
                {
                    Titol = "Modificar Autor",
                    Author = aux_author,
                    OnOk = (sender) =>
                    {
                        try
                        {
                            author a = context.authors.Where(x => x.dni == SelectedAuthor.dni).SingleOrDefault();
                            a.dni = aux_author.dni;
                            a.address = aux_author.address;
                            a.name = aux_author.name;
                            a.surname = aux_author.surname;
                            a.emails = aux_author.emails;
                            a.phones = aux_author.phones;
                            a.disciplines = aux_author.disciplines;
                            context.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.ToString());
                        }
                        FillAuthors(0);
                        SelectedAuthor = AuthorsL.Where(x => x.dni == aux_author.dni).ToList()[0];
                        sender.Close();
                    },
                    OnCancel = (sender) =>
                    {
                        FillAuthors(0);
                        sender.Close();
                    },
                    OnCloseRequest = (sender) =>
                    {
                        FillAuthors(0);
                        sender.Close();
                    }
                });
            }
        }

        public ICommand eliminarAutor { get { return new RelayCommand(delAutor); } }

        protected virtual void delAutor()
        {
            if (SelectedAuthor != null)
            {
                author aux_author = SelectedAuthor;
                aux_author.dni = SelectedAuthor.dni;
                aux_author.name = SelectedAuthor.name;

                List<project> aux_projectList = context.projects.Where(x => x.author_dni == aux_author.dni).ToList();

                if ((!(aux_author.projects.Count > 0)))
                {
                    this.Dialogs.Add(new Esborrar_AutorBuitDialogViewModel()
                    {
                        Title = "Esborrar Contacte",
                        Author = aux_author,
                        OkText = "Delete",
                        TextEnabled = false,
                        OnOk = (sender) =>
                        {
                            try
                            {
                                if (aux_author.phones.Count > 0)
                                {
                                    List<phone> phonesDelList = aux_author.phones.ToList();
                                    foreach (phone item in phonesDelList)
                                    {
                                        phone p = context.phones.Where(x => x.id_phone == item.id_phone).SingleOrDefault();
                                        context.phones.Remove(p);
                                    }
                                }

                                if (aux_author.emails.Count > 0)
                                {
                                    List<email> emailsDelList = aux_author.emails.ToList();
                                    foreach (email item in emailsDelList)
                                    {
                                        email e = context.emails.Where(x => x.id_email == item.id_email).SingleOrDefault();
                                        context.emails.Remove(e);
                                    }
                                }
                                author a = context.authors.Where(x => x.dni == SelectedAuthor.dni).SingleOrDefault();
                                context.authors.Remove(a);
                                context.SaveChanges();
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show(e.ToString());
                            }
                            FillAuthors(0);
                            sender.Close();
                        },
                        OnCancel = (sender) =>
                        {
                            FillAuthors(0);
                            sender.Close();
                        },
                        OnCloseRequest = (sender) =>
                        {
                            FillAuthors(0);
                            sender.Close();
                        }
                    });
                }
                else if ((aux_author.projects.Count > 0))
                {
                    this.Dialogs.Add(new ProjecteEsborrarDialogModel()
                    {
                        Title = "Esborrar Contacte",
                        Author = aux_author,
                        ProjectList = aux_projectList,
                        OkText = "Delete",
                        OnOk = (sender) =>
                        {
                            try
                            {
                                if (aux_author.projects.Count > 0)
                                {
                                    List<project> projectsDelList = aux_author.projects.ToList();

                                    foreach (project subitem in projectsDelList)
                                    {
                                        project projectDel = aux_author.projects.SingleOrDefault();
                                        List<request> requestDelList = projectDel.requests.ToList();

                                        foreach (request item in requestDelList)
                                        {
                                            request r = context.requests.Where(x => x.id_request == item.id_request).SingleOrDefault();

                                            context.requests.Remove(r);
                                        }

                                        project p = context.projects.Where(x => x.id_project == subitem.id_project).SingleOrDefault();

                                        context.projects.Remove(p);

                                    }
                                    if (aux_author.phones.Count > 0)
                                    {
                                        List<phone> phonesDelList = aux_author.phones.ToList();

                                        foreach (phone item in phonesDelList)
                                        {
                                            phone p = context.phones.Where(x => x.id_phone == item.id_phone).SingleOrDefault();

                                            context.phones.Remove(p);
                                        }
                                    }

                                    if (aux_author.emails.Count > 0)
                                    {
                                        List<email> emailsDelList = aux_author.emails.ToList();

                                        foreach (email item in emailsDelList)
                                        {
                                            email e = context.emails.Where(x => x.id_email == item.id_email).SingleOrDefault();

                                            context.emails.Remove(e);
                                        }
                                    }

                                    author a = context.authors.Where(x => x.dni == SelectedAuthor.dni).SingleOrDefault();

                                    context.authors.Remove(a);
                                    context.SaveChanges();
                                }
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show(e.ToString());
                            }
                            FillAuthors(0);
                            sender.Close();
                        },
                        OnCancel = (sender) =>
                        {
                            FillAuthors(0);
                            sender.Close();
                        },
                        OnCloseRequest = (sender) =>
                        {
                            FillAuthors(0);
                            sender.Close();
                        }
                    });
                }
            }
            else
            {

            }
        }


        public ICommand tornarEnrere { get { return new RelayCommand(Close); } }
        public Action<AutorsViewModel> OnOk { get; set; }

        public project Project
        {
            get
            {
                return _project;
            }

            set
            {
                _project = value;
                NotifyPropertyChanged();
            }
        }
        #endregion ICommand

        #region DialogClosing

        public event EventHandler DialogClosing;

        public void Close()
        {
            if (this.DialogClosing != null)
                this.DialogClosing(this, new EventArgs());
        }

        #endregion

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region RequestClose
        public void RequestClose()
        {
            this.DialogClosing(this, null);
        }
        #endregion

        #region FillAutors
        private void FillAuthors(int n)
        {

            if (AuthorsL != null && AuthorsL.Count == 0)
            {
                AuthorsL.Clear();
            }
            AuthorsL = new ObservableCollection<author>(context.authors.OrderBy(x => x.dni).ToList());
            if (AuthorsL != null && AuthorsL.Count < 0)
            {
                SelectedAuthor = AuthorsL[n];
            }

        }
        #endregion
    }
}
