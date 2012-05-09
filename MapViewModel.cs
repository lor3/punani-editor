using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Devweb.MapEditor.Model;

namespace Devweb.MapEditor
{
    /// <summary>
    /// View model for Map. 
    /// </summary>
    public class MapViewModel : ObservableObject, IMap
    {
        #region Private fields
        
        private readonly ObservableCollection<TileDefinition> _tileDefinitions;
        private readonly ObservableCollection<ObservableCollection<Tile>> _mapData;

        private TileDefinition _currentTile;
        private ushort _height;
        private ushort _width;
        private string _name;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MapViewModel"/> class.
        /// </summary>
        /// <param name="map">The map to initialise view model.</param>
        public MapViewModel(Map map)
        {
            _width = map.Width;
            _height = map.Height;

            FilePath = map.FilePath;
            Name = map.Name;

            _tileDefinitions = new ObservableCollection<TileDefinition>(map.TileDefinitions.Values);
            _mapData = new ObservableCollection<ObservableCollection<Tile>>(map.MapData.Select(l => new ObservableCollection<Tile>(l)));

            CurrentTile = _tileDefinitions[0];
        }

        #endregion

        #region Binding Properties

        /// <summary>
        /// Gets the window title.
        /// </summary>
        public string WindowTitle
        {
            get
            {
                var title = MainWindow.DEFAULT_TITLE;
                if (!string.IsNullOrEmpty(Name)) title += " : " + (string.IsNullOrEmpty(Name) ? "Unnamed file" : Name);
                return title;
            }
        }

        public ObservableCollection<TileDefinition> TileDefinitions
        {
            get
            {
                return _tileDefinitions;
            }
        }

        public ObservableCollection<ObservableCollection<Tile>> MapData
        {
            get
            {
                return _mapData;
            }
        }

        /// <summary>
        /// Gets or sets the current drawing tile.
        /// </summary>
        public TileDefinition CurrentTile
        {
            get
            {
                return _currentTile;
            }
            set
            {
                if (_currentTile == value) return;

                _currentTile = value;
                RaisePropertyChanged("CurrentTile");
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates the map data.
        /// </summary>
        private void UpdateMapData()
        {
            var updated = false;
            if (_mapData.Count != _height)
            {
                updated = true;
                ResizeList(_mapData, _height, () => new ObservableCollection<Tile>());
            }
            foreach (var row in _mapData.Where(r => r.Count != _width))
            {
                ResizeList(row, _width, () => new Tile(Map.Null));
                updated = true;
            }

            if (updated)
            {
                RaisePropertyChanged("MapData");
            }
        }

        /// <summary>
        /// Resizes the list.
        /// </summary>
        /// <typeparam name="T">list type</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="count">The new count.</param>
        /// <param name="createCallback">The create callback.</param>
        private void ResizeList<T>(ObservableCollection<T> list, int count, Func<T> createCallback)
        {
            if (list.Count > count)
            {
                while(list.Count > count)
                    list.RemoveAt(list.Count - 1);
            }
            else
                while (list.Count < count)
                {
                    list.Add(createCallback());
                }
        }

        #endregion

        #region Implementation of IMap

        /// <inheritdoc />
        public ushort Height
        {
            get
            {
                return _height;
            }
            set
            {
                if (_height == value) return;
                _height = value;
                UpdateMapData();
            }
        }

        /// <inheritdoc />
        public ushort Width
        {
            get
            {
                return _width;
            }
            set
            {
                if (_width == value) return;
                _width = value;
                UpdateMapData();
            }
        }

        /// <inheritdoc />
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name == value) return;

                _name = value;
                RaisePropertyChanged("Name");
                RaisePropertyChanged("WindowTitle");
            }
        }

        /// <inheritdoc />
        public string FilePath { get; set; }

        #endregion

        #region IMap Members

        /// <inheritdoc />
        IEnumerable<TileDefinition> IMap.TileDefinitions
        {
            get
            {
                return TileDefinitions;
            }
        }

        /// <inheritdoc />
        IEnumerable<IEnumerable<TileDefinition>> IMap.MapData
        {
            get
            {
                return MapData.Select(t  => t.Select(m => m.Definition));
            }
        }

        #endregion
    }
}
