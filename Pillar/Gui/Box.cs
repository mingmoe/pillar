using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Pillar.Gui
{
    public struct Box : IEquatable<Box>
    {
        public Point Size { get; set; }

        public BoxExpansion Padding { get; set; }

        public BoxExpansion Margin { get; set; }

        public readonly override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is Box box)
            {
                return box.Size == Size && box.Padding == Padding && box.Margin == Margin;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Size,Padding,Margin);
        }

        public override string ToString()
        {
            return $"Box[Size:{Size} Padding:{Padding} Margin:{Margin}]";
        }

        public static bool operator ==(Box left,Box right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Box left, Box right)
        {
            return !left.Equals(right);
        }

        public bool Equals(Box other)
        {
            return other.Size == Size && other.Padding == Padding && other.Margin == Margin;
        }

    }
}

