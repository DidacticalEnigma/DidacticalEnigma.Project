using System;
using JetBrains.Annotations;

namespace MagicTranslatorProject
{
    internal class PageId : IEquatable<PageId>
    {
        public bool Equals(PageId other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Chapter.Equals(other.Chapter) && PageNumber == other.PageNumber;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PageId) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Chapter.GetHashCode() * 397) ^ PageNumber;
            }
        }

        public static bool operator ==(PageId left, PageId right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PageId left, PageId right)
        {
            return !Equals(left, right);
        }

        [NotNull] public ChapterId Chapter { get; }

        public int PageNumber { get; }

        public PageId([NotNull] ChapterId chapter, int pageNumber)
        {
            Chapter = chapter ?? throw new ArgumentNullException(nameof(chapter));
            PageNumber = pageNumber;
        }
        
        public override string ToString()
        {
            return $"{Chapter}/P{PageNumber:D8}";
        }
    }
}