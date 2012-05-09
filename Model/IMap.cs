using System.Collections.Generic;

namespace Devweb.MapEditor.Model
{
    public interface IMap
    {
        ushort Width { get; }
        ushort Height { get; }

        /// <summary>
        /// Gets the filename.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the full path.
        /// </summary>
        string FilePath { get; }

        /// <summary>
        /// Gets the tile definitions.
        /// </summary>
        IEnumerable<TileDefinition> TileDefinitions { get; }

        /// <summary>
        /// Gets the map data.
        /// </summary>
        IEnumerable<IEnumerable<TileDefinition>> MapData { get; }
    }
}
