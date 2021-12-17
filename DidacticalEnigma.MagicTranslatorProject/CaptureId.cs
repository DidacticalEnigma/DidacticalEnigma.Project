using System;
using JetBrains.Annotations;

namespace MagicTranslatorProject
{
    internal class CaptureId : IEquatable<CaptureId>
    {
        public bool Equals(CaptureId other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Page.Equals(other.Page) && CaptureNumber == other.CaptureNumber;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CaptureId)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Page.GetHashCode() * 397) ^ CaptureNumber.GetHashCode();
            }
        }

        public static bool operator ==(CaptureId left, CaptureId right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CaptureId left, CaptureId right)
        {
            return !Equals(left, right);
        }

        [NotNull] public PageId Page { get; }

        public long CaptureNumber { get; }

        public CaptureId([NotNull] PageId page, long captureNumber)
        {
            Page = page ?? throw new ArgumentNullException(nameof(page));
            CaptureNumber = captureNumber;
        }
        
        public override string ToString()
        {
            return $"{Page}/C{CaptureNumber:D8}";
        }
    }
}