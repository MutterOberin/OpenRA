#region Copyright & License Information
/*
 * Copyright 2007-2015 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation. For more information,
 * see COPYING.
 */
#endregion

using System;
using System.Drawing;

namespace OpenRA
{
	public struct MPos : IEquatable<MPos>
	{
		public readonly int U, V;

		public MPos(int u, int v) { U = u; V = v; }
		public static readonly MPos Zero = new MPos(0, 0);

		public static bool operator ==(MPos me, MPos other) { return me.U == other.U && me.V == other.V; }
		public static bool operator !=(MPos me, MPos other) { return !(me == other); }

		public override int GetHashCode() { return U.GetHashCode() ^ V.GetHashCode(); }

		public bool Equals(MPos other) { return other == this; }
		public override bool Equals(object obj) { return obj is MPos && Equals((MPos)obj); }

		public MPos Clamp(Rectangle r)
		{
			return new MPos(Math.Min(r.Right, Math.Max(U, r.Left)),
							Math.Min(r.Bottom, Math.Max(V, r.Top)));
		}

		public override string ToString() { return U + "," + V; }

		public CPos ToCPos(Map map)
		{
			return ToCPos(map.TileShape);
		}

		public CPos ToCPos(TileShape shape)
		{
			if (shape == TileShape.Rectangle)
				return new CPos(U, V);

			// Convert from rectangular map position to diamond cell position
			//  - The staggered rows make this fiddly (hint: draw a diagram!)
			// (a) Consider the relationships:
			//  - +1u (even -> odd) adds (1, -1) to (x, y)
			//  - +1v (even -> odd) adds (1, 0) to (x, y)
			//  - +1v (odd -> even) adds (0, 1) to (x, y)
			// (b) Therefore:
			//  - au + 2bv adds (a + b) to (x, y)
			//  - a correction factor is added if v is odd
			var offset = (V & 1) == 1 ? 1 : 0;
			var y = (V - offset) / 2 - U;
			var x = V - y;
			return new CPos(x, y);
		}
	}
}