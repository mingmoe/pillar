using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pillar
{
    public struct BoxExpansion:IEquatable<BoxExpansion>
    {
        public int Left { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
        public int Bottom { get; set; }

        public readonly override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is BoxExpansion box)
            {
                return box.Left == Left && box.Right == Right && box.Top == Top && box.Bottom == Bottom;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Left, Right, Top, Bottom);
        }

        public override string ToString()
        {
            return $"{{Left:{Left} Right:{Right} Top:{Top} Bottom:{Bottom}}}";
        }

        public static bool operator ==(BoxExpansion left,BoxExpansion right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BoxExpansion left, BoxExpansion right)
        {
            return !left.Equals(right);
        }

        public bool Equals(BoxExpansion other)
        {
            return other.Left == Left && other.Right == Right && other.Top == Top && other.Bottom == Bottom;
        }
    }
}
