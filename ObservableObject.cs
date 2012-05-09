using System.ComponentModel;

namespace Devweb.MapEditor
{
    /// <summary>
    /// This is the abstract base class for any object that provides property change notifications.  
    /// </summary>
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        #region RaisePropertyChanged

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        #endregion // RaisePropertyChanged

        #region INotifyPropertyChanged Members

        /// <inheritdoc/>
        public virtual event PropertyChangedEventHandler PropertyChanged;

        #endregion // INotifyPropertyChanged Members
    }
}
