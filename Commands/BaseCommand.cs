/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  BaseCommand 
 * 
 *  implementing ICommand interface
 */
using System.Windows.Input;

namespace SharedLivingCostCalculator.Commands
{
    public abstract class BaseCommand : ICommand
    {

        // Event Properties & Fields
        #region Event Properties & Fields

        public event EventHandler? CanExecuteChanged;
        
        #endregion


        // Methods
        #region Methods

        public virtual bool CanExecute(object? parameter)
        {
            return true;
        }


        public abstract void Execute(object? parameter);


        protected void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
        
        #endregion


    }
}
// EOF