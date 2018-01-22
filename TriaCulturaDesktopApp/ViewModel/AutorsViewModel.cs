using System;
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


namespace TriaCulturaDesktopApp.ViewModel
{
    public class AutorsViewModel : ViewModelBase, IUserDialogViewModel
    {
        triaculturaCTXEntities context = new triaculturaCTXEntities();       
        public bool IsModal
        {
            get
            {
                return true;            }
        }

        #region Properties
        private List<author> _authors;
            private author _autorSelected;
public event EventHandler DialogClosing;

        public void RequestClose()
        {
            throw new NotImplementedException();
        }
    }
}
#endregion