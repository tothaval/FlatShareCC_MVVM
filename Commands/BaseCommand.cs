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

        // event properties & fields
        #region event properties

        public event EventHandler? CanExecuteChanged;
        
        #endregion event properties


        // methods
        #region methods

        public virtual bool CanExecute(object? parameter)
        {
            return true;
        }


        public abstract void Execute(object? parameter);


        protected void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
        
        #endregion methods


    }
}
// EOF