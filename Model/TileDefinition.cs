using System;
using System.Diagnostics;
using System.Windows.Media;

namespace Devweb.MapEditor.Model
{
    /// <summary>
    /// The tile definitions.
    /// </summary>
    [DebuggerDisplay("{Name} : {Color}")]
    public class TileDefinition : ObservableObject
    {
        #region Private fields

        private int _id;
        private Color? _color;
        private string _name;
        private string _path;

        #endregion

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                if(_id == value) return;
                
                _id = value;
                RaisePropertyChanged("Id");
            }
        }
     
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if(_name == value) return;
                
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                if(_path == value) return;
                
                _path = value;
                RaisePropertyChanged("Path");
            }
        }

        /// <summary>
        /// Gets or sets the colour.
        /// </summary>
        public Color Color
        {
            get
            {
                return _color.HasValue ? _color.Value : (_color = ColourFromString(Name)).Value;
            }
            set
            {
                _color = value;
            }
        }

        /// <summary>
        /// Generates colour from string.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>a colour object</returns>
        private static Color ColourFromString(string str)
        {
            var bytes = BitConverter.GetBytes(str.GetHashCode());
            return Color.FromRgb(bytes[0], bytes[1], bytes[2]);
        }
    }
}
