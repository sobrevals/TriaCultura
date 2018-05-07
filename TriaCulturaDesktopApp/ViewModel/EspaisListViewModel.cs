using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MvvmDialogs.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TriaCulturaDesktopApp.Model;

namespace TriaCulturaDesktopApp.ViewModel
{
    public class EspaisListViewModel : ViewModelBase, IUserDialogViewModel
    {
        private ObservableCollection<IDialogViewModel> _Dialogs = new ObservableCollection<IDialogViewModel>();
        public ObservableCollection<IDialogViewModel> Dialogs { get { return _Dialogs; } }
        triaculturaDBEntities context = new triaculturaDBEntities();

        #region BasicProperties

        private ObservableCollection<place> _places;
        private place _selectedPlace;

        public EspaisListViewModel()
        {
            FillEspais();
        }
        public ObservableCollection<place> Places
        {
            get { return (_places); }
            set
            {
                _places = value;
                RaisePropertyChanged("Places");
            }
        }
        public place SelectedPlace { get { return _selectedPlace; } set { _selectedPlace = value; RaisePropertyChanged("SelectedPlace"); } }
        #endregion

        #region IsModal
        public bool IsModal
        {
            get
            {
                return true;
            }
        }
        #endregion
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public event EventHandler DialogClosing;
        #region fill

        private void FillEspais()
        {
            Places = null;
            Places = new ObservableCollection<place>(context.places.ToList());
            if (Places != null && Places.Count > 0)
            {
                SelectedPlace = Places[0];
            }
        }

        #endregion
        #region Close
        public void RequestClose()
        {
            this.DialogClosing(this, null);
        }
        public void Close()
        {
            if (this.DialogClosing != null)
                this.DialogClosing(this, new EventArgs());
        }
        #endregion

        #region commands

        public ICommand tornarEnrere { get { return new RelayCommand(Close); } }

        public ICommand afegirPlace { get { return new RelayCommand(add_place); } }
        public ICommand eliminarPlace { get { return new RelayCommand(del_place); } }
        public ICommand modificarPlace { get { return new RelayCommand(mod_place); } }
        public ICommand OpenProjecte { get { return new RelayCommand(opProject); } }
        protected virtual void opProject()
        {
            place p = SelectedPlace;

            if (SelectedPlace != null)
            {
                this.Dialogs.Add(new ProjectesViewModel(p, context)
                {
                    context = context,
                    Boto_afegir_enabled = true,
                });
            }
        }

        private void add_place()
        {
            place aux_place = new place();
            this.Dialogs.Add(new PlaceDialogViewModel
            {
                Title = "Afegir un Espai",
                SelectedPlace = aux_place,
                OkText = "Afegir",
                TextEnabled = true,
                OnOk = (sender) =>
                {
                    try
                    {
                        context.places.Add(aux_place);
                        context.SaveChanges();
                        FillEspais();
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
        private void mod_place()
        {
            place aux_place = SelectedPlace;
            this.Dialogs.Add(new PlaceDialogViewModel {
                Title = "Modificar Espai",
                SelectedPlace = aux_place,
                OkText = "Modificar",
                TextEnabled = true,
                OnOk = (sender) =>
                {
                    try
                    {
                        context.places.Where(x => x.id_place == aux_place.id_place).SingleOrDefault().name = aux_place.name;
                        context.places.Where(x => x.id_place == aux_place.id_place).SingleOrDefault().address = aux_place.address;
                        context.places.Where(x => x.id_place == aux_place.id_place).SingleOrDefault().capacity = aux_place.capacity;
                        context.SaveChanges();
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

        private void del_place()
        {
            place aux_place = SelectedPlace;
            this.Dialogs.Add(new PlaceDialogViewModel
            {
                Title = "Esborrar Espai",
                SelectedPlace = aux_place,
                OkText = "Confirmar",
                TextEnabled = false,
                OnOk = (sender) =>
                {
                    try
                    {
                        if (aux_place.requests.Count == 0)
                        {
                            context.places.Remove(aux_place);
                            context.SaveChanges();
                        } else
                        {
                            MessageBox.Show("Aquest Espai té projectes assignats, si us plau, esborra primer els projectes.");
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

        #endregion
    }
}