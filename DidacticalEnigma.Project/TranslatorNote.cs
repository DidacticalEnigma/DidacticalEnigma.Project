using System;

namespace DidacticalEnigma.Project
{
    public class TranslatorNote : IEquatable<TranslatorNote>
    {
        public bool Equals(TranslatorNote other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return SideText == other.SideText && Text == other.Text;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TranslatorNote)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((SideText != null ? SideText.GetHashCode() : 0) * 397) ^ (Text != null ? Text.GetHashCode() : 0);
            }
        }

        public static bool operator ==(TranslatorNote left, TranslatorNote right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TranslatorNote left, TranslatorNote right)
        {
            return !Equals(left, right);
        }

        public string SideText { get; }

        public string Text { get; }

        public TranslatorNote(string sideText, string text)
        {
            SideText = sideText;
            Text = text;
        }

        public override string ToString()
        {
            return $"{SideText}: {Text}";
        }
    }
}