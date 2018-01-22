using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using TriaCulturaDesktopApp.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.ComponentModel;
using MvvmDialogs.ViewModels;
using System.Windows.Input;

namespace TriaCulturaDesktopApp.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<IDialogViewModel> _Dialogs = new ObservableCollection<IDialogViewModel>();
        public ObservableCollection<IDialogViewModel> Dialogs { get { return _Dialogs; } }

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {          
        }
        public ICommand CloseAllCommand { get { return new RelayCommand(OnCloseAll); } }
        public void OnCloseAll()
        {
            this.Dialogs.Clear();
        }

        public ICommand NewModalDialogCommand { get { return new RelayCommand(OnNewModalDialog); } }

        public void OnNewModalDialog()
        {
            this.Dialogs.Add(new AutorsViewModel
            {
                //Title = "Afegir Contacte",
                //Contacte = con_aux,
                //OkText = "Ok",
                //TextEnabled = true,
                //OnOk = (sender) =>


            });
        }
    }
}