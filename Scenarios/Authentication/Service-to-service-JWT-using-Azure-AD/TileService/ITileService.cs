using CoreWCF;
using System;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web.Resource;

namespace TileService
{
    [ServiceContract]
    public interface ITileService
    {
        [OperationContract]
        IList<GameTile> DrawTiles(int count);
    }

    public class TileBag : ITileService
    {
        private static IList<GameTile> _gameTiles = initTileCollection();
        private static int TileCount;
        private static Random rnd = new Random();

     //  [AuthorizeRole("AccessTileBag")]
        public IList<GameTile> DrawTiles(int count)
        {

            // TODO: Remove this block
            var ctxa = new HttpContextAccessor();
            var ctx = ctxa.HttpContext;
            if (ctx == null) { Console.WriteLine("no http context"); }
            else
            {
                foreach (var claim in ctx.User.Claims)
                {
                    Console.WriteLine("***** BEGIN CLAIM *****");
                    Console.WriteLine(claim.ToString());
                    Console.WriteLine("***** END CLAIM *****");
                }
                Console.WriteLine("***** BEGIN HEADERS *****");
                foreach (var h in ctx.Request.Headers)
                {
                    Console.WriteLine(h.Key + ":" + h.Value);
                }
                Console.WriteLine("***** END HEADERS *****");
            }
            if (count > 0)
            {
                var tiles = new List<GameTile>();
                for (var i = 0; i < count; i++)
                {
                    var index = rnd.Next(TileCount);
                    var t_offset = 0;
                    foreach (var t in _gameTiles)
                    {
                        if (t_offset + t.Weight > index)
                        {
                            tiles.Add(t);
                            break;
                        }
                        t_offset += t.Weight;
                    }
                }
                return tiles;
            }
            throw new ArgumentException("DrawTiles needs to be given a positive number of tiles to return");
        }


        private static IList<GameTile> initTileCollection()
        {
            var tiles = new List<GameTile>();
            tiles.Add(new GameTile('A', 1, 9));
            tiles.Add(new GameTile('B', 3, 2));
            tiles.Add(new GameTile('C', 3, 2));
            tiles.Add(new GameTile('D', 2, 4));
            tiles.Add(new GameTile('E', 1, 12));
            tiles.Add(new GameTile('F', 4, 2));
            tiles.Add(new GameTile('G', 2, 3));
            tiles.Add(new GameTile('H', 4, 2));
            tiles.Add(new GameTile('I', 1, 9));
            tiles.Add(new GameTile('J', 7, 1));
            tiles.Add(new GameTile('K', 5, 1));
            tiles.Add(new GameTile('L', 1, 4));
            tiles.Add(new GameTile('M', 3, 2));
            tiles.Add(new GameTile('N', 1, 6));
            tiles.Add(new GameTile('O', 1, 8));
            tiles.Add(new GameTile('P', 3, 2));
            tiles.Add(new GameTile('Q', 10, 1));
            tiles.Add(new GameTile('R', 1, 9));
            tiles.Add(new GameTile('S', 1, 4));
            tiles.Add(new GameTile('T', 1, 6));
            tiles.Add(new GameTile('U', 1, 4));
            tiles.Add(new GameTile('V', 4, 2));
            tiles.Add(new GameTile('W', 1, 9));
            tiles.Add(new GameTile('X', 8, 1));
            tiles.Add(new GameTile('Y', 4, 2));
            tiles.Add(new GameTile('Z', 10, 1));
            tiles.Add(new GameTile('\0', 0, 2));

            TileCount = (from t in tiles
                         select t.Weight).Sum();
            return tiles;
        }
    }

    [DataContract]
    public class GameTile
    {
        [DataMember]
        public char Letter { get; init; }
        [DataMember]
        public int Score { get; init; }
        [DataMember]
        public int Weight { get; init; }

        public GameTile(char letter, int score, int weight)
        {
            Letter = letter;
            Score = score;
            Weight = weight;
        }
    }
}
