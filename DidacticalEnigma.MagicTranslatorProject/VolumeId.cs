using System;

namespace MagicTranslatorProject
{
    internal class VolumeId : IEquatable<VolumeId>
    {
        public bool Equals(VolumeId other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return VolumeNumber == other.VolumeNumber;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((VolumeId) obj);
        }

        public override int GetHashCode()
        {
            return VolumeNumber;
        }

        public static bool operator ==(VolumeId left, VolumeId right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(VolumeId left, VolumeId right)
        {
            return !Equals(left, right);
        }

        public int VolumeNumber { get; }

        public VolumeId(int volumeNumber)
        {
            VolumeNumber = volumeNumber;
        }

        public override string ToString()
        {
            return $"/VOL{VolumeNumber:D8}";
        }
    }
}