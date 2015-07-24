using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HexUtilities;
using HexUtilities.ShadowCasting;

namespace Terrain
{
    public class Fov : IFov
    {
        private readonly object _syncLock = new object();

        public Fov(IFovBoard<IHex> board)
        {
            _isOnboard = h => board.IsOnboard(h);
            _fovBacking = new BitArray[board.MapSizeHexes.Width];
            for (var i = 0; i < board.MapSizeHexes.Width; i++)
                _fovBacking[i] = new BitArray(board.MapSizeHexes.Height);
        }

        public bool this[HexCoords coords]
        {
            get
            {
                return _isOnboard(coords) && _fovBacking[coords.User.X][coords.User.Y];
            }
            internal set
            {
                lock (_syncLock)
                {
                    if (_isOnboard(coords)) { _fovBacking[coords.User.X][coords.User.Y] = value; }
                }
            }
        } BitArray[] _fovBacking;

        Func<HexCoords, bool> _isOnboard;
    }
}
