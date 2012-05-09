using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace Devweb.MapEditor.Model
{
    /// <summary>
    /// Static class for loading / saving of map files into .g format.
    /// </summary>
    public static class FileOps
    {
        private static Regex _descriptionCommentRegex = new Regex(@"\{c:'#([^']+)'\}$", RegexOptions.Compiled);

        /// <summary>
        /// Loads the specified file path.
        /// </summary>
        /// <param name="filepath">The file path.</param>
        /// <returns></returns>
        public static Map Load(string filepath)
        {
            if (string.IsNullOrEmpty(filepath))
                throw new ArgumentNullException("filepath");

            Map map = null;

            using (var sr = new StreamReader(filepath))
            {
                string line;
                var lineNumber = 0;
                string lastComment = null;
                var readingData = false;
                var dataRow = 0;

                ushort width = 0, height = 0, unnamedCount = 0;

                while ((line = sr.ReadLine()) != null)
                {
                    lineNumber++;
                    line = line.Trim();
                    if(line.Length == 0) continue;
                    var tokens = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    if(map == null && width > 0 && height > 0)
                        map = new Map(width, height);

                    if (!readingData)
                    {
                        switch (line[0])
                        {
                            case '#':
                                lastComment = line.Length > 1 ? line.Substring(1).Trim() : null;
                                continue;

                            case 'x':
                                if (tokens.Length < 2 || !ushort.TryParse(tokens[1], out width)) throw new Exception(string.Format("Invalid x value line {0}.", lineNumber));
                                break;

                            case 'y':
                                if (tokens.Length < 2 || !ushort.TryParse(tokens[1], out height)) throw new Exception(string.Format("Invalid y value line {0}.", lineNumber));
                                break;

                            case 't':
                                {
                                    ushort id;
                                    if (tokens.Length < 3 || !ushort.TryParse(tokens[1], out id)) throw new Exception(string.Format("Invalid tile definition on line {0}.", lineNumber));

                                    // as null is always created ignore the file based one
                                    if (id == 0) break;

                                    if (map.GetTileDefinition(id) != null) throw new Exception(string.Format("Redefinition of tile #{0} on line {1}.", id, lineNumber));

                                    TileDefinition tileDef = null;
                                    if (string.IsNullOrEmpty(lastComment))
                                    {
                                        lastComment = "Unnamed " + (++unnamedCount);
                                    }
                                    else
                                    {
                                        var match = _descriptionCommentRegex.Match(lastComment);
                                        if (match.Success)
                                        {
                                            uint colorVal;
                                            if(uint.TryParse(match.Groups[1].Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out colorVal))
                                            {
                                                var color = Color.FromRgb((byte)(colorVal >> 16), (byte)((colorVal >> 8) & 0xff), (byte)(colorVal & 0xff));
                                                tileDef = new TileDefinition { Id = id, Path = tokens[2], Name = lastComment.Substring(0, match.Groups[0].Index - 1), Color = color };
                                            }
                                        }
                                    }

                                    map.TileDefinitions.Add(id, tileDef ?? new TileDefinition { Id = id, Path = tokens[2], Name = lastComment });
                                    break;
                                }
                        }

                        if (tokens[0] == "begin") readingData = true;
                        lastComment = null;
                    }
                    else
                    {
                        if (dataRow >= map.Height) throw new Exception(string.Format("Unexpected data row on line {0}.", lineNumber));

                        if(tokens.Length < map.Width)
                            throw new Exception(string.Format("Invalid data width, expected {0} got {1} on line {2}.", map.Width, tokens.Length, lineNumber));

                        var row = map.MapData[dataRow];
                        for (var i = 0; i < map.Width; i++)
                        {
                            ushort id;
                            TileDefinition type;
                            if (!ushort.TryParse(tokens[i], out id) || !map.TileDefinitions.TryGetValue(id, out type))
                                throw new Exception(string.Format("Invalid or unknown tile id on line {0} column {1}.", lineNumber, i));

                            row[i].Definition = type;
                        }

                        // stop reading data once we have all rows
                        if (++dataRow == map.Height) readingData = false;
                    }
                }

                // not enough data
                if(dataRow < map.Height)
                    throw new Exception("Unexpected end of file.");
            }

            map.Name = Path.GetFileName(filepath);
            map.FilePath = filepath;

            return map;
        }


        /// <summary>
        /// Saves the specified map.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <param name="filepath">The file path.</param>
        public static void Save(IMap map, string filepath)
        {
            if(map == null)
                throw new ArgumentNullException("map");

            if(string.IsNullOrEmpty(filepath))
                throw new ArgumentNullException("filepath");


            using (var sw = new StreamWriter(filepath))
            {
                sw.WriteLine("x {0}", map.Width);
                sw.WriteLine("y {0}", map.Height);

                sw.WriteLine();

                foreach (var tile in map.TileDefinitions)
                {
                    sw.WriteLine("# {0} {{c:'#{1:x2}{2:x2}{3:x2}'}}", string.IsNullOrEmpty(tile.Name) ? "unnamed" : tile.Name, tile.Color.R, tile.Color.G, tile.Color.B);
                    sw.WriteLine("t {0} {1}", tile.Id, string.IsNullOrEmpty(tile.Path) ? Map.Null.Path : tile.Path);
                }

                sw.WriteLine();
                sw.WriteLine("begin");

                foreach (var row in map.MapData)
                    sw.WriteLine(string.Join(" ", row.Select(t => t.Id).ToArray()));

                sw.WriteLine();
                sw.WriteLine("# Generated by CrapEditor 0.001.");
            }
        }
    }
}
