﻿#region The MIT License - Copyright (C) 2012-2013 Pieter Geerkens
/////////////////////////////////////////////////////////////////////////////////////////
//                PG Software Solutions Inc. - Hex-Grid Utilities
/////////////////////////////////////////////////////////////////////////////////////////
// The MIT License:
// ----------------
// 
// Copyright (c) 2012-2013 Pieter Geerkens (email: pgeerkens@hotmail.com)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, 
// merge, publish, distribute, sublicense, and/or sell copies of the Software, and to 
// permit persons to whom the Software is furnished to do so, subject to the following 
// conditions:
//     The above copyright notice and this permission notice shall be 
//     included in all copies or substantial portions of the Software.
// 
//     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//     EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
//     OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
//     NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
//     HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
//     WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
//     FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
//     OTHER DEALINGS IN THE SOFTWARE.
/////////////////////////////////////////////////////////////////////////////////////////
#endregion
using System;
using System.Globalization;


namespace HexUtilities.Common
{
    /// <summary>TODO</summary>
    public struct NeighbourCoords : IEquatable<NeighbourCoords>
    {
        /// <summary>TODO</summary>
        public Hexside Hexside { get; private set; }
        /// <summary>TODO</summary>
        public HexCoords Coords { get; private set; }

        /// <summary>TODO</summary>
        public NeighbourCoords(HexCoords coords, Hexside hexside)
            : this()
        {
            Coords = coords; Hexside = hexside;
        }
        /// <inheritdoc/>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Neighbour: {0} at {1}", Coords.User, Hexside);
        }

        /// <summary>TODO</summary>
        public static Func<NeighbourCoords, T> Bind<T>(Func<HexCoords, T> f)
        {
            return n => f(n.Coords);
        }

        #region Value Equality - on Coords field only
        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is NeighbourCoords && this.Coords.Equals(((NeighbourCoords)obj).Coords);
        }
        /// <inheritdoc/>
        public override int GetHashCode() { return Coords.GetHashCode(); }

        bool IEquatable<NeighbourCoords>.Equals(NeighbourCoords obj) { return this.Equals(obj); }

        /// <summary>TODO</summary>
        public static bool operator !=(NeighbourCoords lhs, NeighbourCoords rhs) { return !(lhs == rhs); }
        /// <summary>TODO</summary>
        public static bool operator ==(NeighbourCoords lhs, NeighbourCoords rhs) { return lhs.Equals(rhs); }
        #endregion
    }
}
