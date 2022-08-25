using CoreWCF;
using System;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web.Resource;
using System.Web.Services.Description;

namespace TileService
{
    [ServiceContract]
    public interface ITileService
    {
        [OperationContract]
        IList<GameTile> DrawTiles(int count);
    }

    public partial class TileBag : ITileService
    {
        private IList<GameTile> _gameTiles;
        private int _tileCount;
        private  Random _rnd;
        private ILogger<TileBag> _logger;
        private bool _isDev = false;

        // Parameterized constructor will be called by Dependency Injection
        public TileBag(ILogger<TileBag> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            if (env.IsDevelopment()) _isDev = true;
            _rnd = new Random();
            _gameTiles = initTileCollection();
        }


        [AuthorizeRole("AccessTileBag")]

        public IList<GameTile> DrawTiles(int count, [Injected] HttpContext ctx)
        {
            if (_isDev)
            {
                foreach (var claim in ctx.User.Claims)
                {
                    _logger.LogDebug("Claims {claim}", claim.ToString());
                }
                foreach (var h in ctx.Request.Headers)
                {
                    _logger.LogDebug("Request Header {name}={value}", h.Key, h.Value);
                }
            }
            if (count > 0)
            {
                var tiles = new List<GameTile>();
                for (var i = 0; i < count; i++)
                {
                    var index = _rnd.Next(_tileCount);
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


        private IList<GameTile> initTileCollection()
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

            _tileCount = (from t in tiles
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
