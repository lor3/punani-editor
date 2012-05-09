namespace Devweb.MapEditor.Model
{
    /// <summary>
    /// Map tile object.
    /// </summary>
    public class Tile : ObservableObject
    {
        private TileDefinition _definition;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tile"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public Tile(TileDefinition type)
        {
            _definition = type;
        }


        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public TileDefinition Definition
        {
            get
            {
                return _definition;
            }
            set
            {
                if (_definition == value) return;

                _definition = value;
                RaisePropertyChanged("Definition");
            }
        }
    }
}
