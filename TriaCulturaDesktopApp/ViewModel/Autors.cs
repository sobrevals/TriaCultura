using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmDialogs.ViewModels;

namespace TriaCulturaDesktopApp.ViewModel
{
   public class Autors : IUserDialogViewModel
    {
        #region IUserDialogViewModel Implementation

        public bool IsModal { get; private set; }
        public virtual void RequestClose()
        {
            if (this.OnCloseRequest != null)
                this.OnCloseRequest(this);
            else
                Close();
        }
        public event EventHandler DialogClosing;


        #endregion IUserDialogViewModel Implementation
    }
}
