using System.Windows.Media;
using Devweb.MapEditor.Model;

namespace Devweb.MapEditor
{
    /// <summary>
    /// Simple design instance map.
    /// </summary>
    public class DesignMap : MapViewModel
    {
        private class DesignMapInner : Map
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DesignMapInner"/> class.
            /// </summary>
            public DesignMapInner()
                : base(5, 10)
            {
                TileDefinitions.Add(1, new TileDefinition { Id = 1, Name = "test 1", Color = Colors.Red });
                TileDefinitions.Add(2, new TileDefinition { Id = 2, Name = "test 2", Color = Colors.Green });
                TileDefinitions.Add(3, new TileDefinition { Id = 3, Name = "test 3", Color = Colors.Blue });

                for (var i = 0; i < MapData.Count; i++)
                {
                    foreach (var t in MapData[i]) t.Definition = TileDefinitions[i % 4];
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DesignMap"/> class.
        /// </summary>
        public DesignMap() : base(new DesignMapInner())
        {
        }
    }
}
