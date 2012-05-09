using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace Devweb.MapEditor.Model
{
    /// <summary>
    /// Primary map object.
    /// </summary>
    public class Map : IMap
    {
        #region Constants

        private const ushort DEFAULT_MAP_WIDTH = 40;
        private const ushort DEFAULT_MAP_HEIGHT = 40;

        /// <summary>
        /// Null tile is always zero.
        /// </summary>
        public static readonly TileDefinition Null = new TileDefinition { Id = 0, Color = Colors.Black, Path = "data/tiles/null " };

        #endregion

        #region Private fields

        private readonly List<List<Tile>> _mapData;
        private readonly Dictionary<int, TileDefinition> _tileDefinitions;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Map"/> class.
        /// </summary>
        public Map() : this(DEFAULT_MAP_WIDTH, DEFAULT_MAP_HEIGHT)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Map"/> class.
        /// </summary>
        /// <param name="width">The map width.</param>
        /// <param name="height">The map height.</param>
        public Map(ushort width, ushort height)
        {
            _mapData = new List<List<Tile>>(Height);
            _tileDefinitions = new Dictionary<int, TileDefinition> { { 0, Null } };

            Height = height;
            Width = width;

            for (var i = 0; i < Height; i++)
            {
                var row = new List<Tile>(Width);
                for (var j = 0; j < Width; j++) row.Add(new Tile(Null));
                _mapData.Add(row);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the map data.
        /// </summary>
        public List<List<Tile>> MapData
        {
            get
            {
                return _mapData;
            }
        }

        /// <summary>
        /// Gets the tile types.
        /// </summary>
        public Dictionary<int, TileDefinition> TileDefinitions
        {
            get
            {
                return _tileDefinitions;
            }
        }

        #endregion

        #region Public Methods

        public TileDefinition GetTileDefinition(int id)
        {
            TileDefinition type;
            return _tileDefinitions.TryGetValue(id, out type) ? type : null;
        }

        #endregion

        #region Implementation of IMap

        /// <inheritdoc />
        public ushort Width { get; set; }

        /// <inheritdoc />
        public ushort Height { get; set; }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public string FilePath { get; set; }

        #endregion

        #region IMap Members

        /// <inheritdoc />
        IEnumerable<TileDefinition> IMap.TileDefinitions
        {
            get
            {
                return TileDefinitions.Values;
            }
        }

        /// <inheritdoc />
        IEnumerable<IEnumerable<TileDefinition>> IMap.MapData
        {
            get
            {
                return MapData.Select(m => m.Select(t => t.Definition));
            }
        }

        #endregion
    }
}
