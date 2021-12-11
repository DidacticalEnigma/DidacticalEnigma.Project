using System;

namespace MagicTranslatorProject.Context
{
    public class NamedCharacter : CharacterType
    {
        protected bool Equals(NamedCharacter other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NamedCharacter)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(NamedCharacter left, NamedCharacter right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(NamedCharacter left, NamedCharacter right)
        {
            return !Equals(left, right);
        }

        public long Id { get; }
        
        public string Name { get; }

        public NamedCharacter(long id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public override bool Equals(CharacterType other)
        {
            return Equals((object)other);
        }

        public override string ToString()
        {
            return $"Character: {Name}";
        }
    }

    public class BasicCharacter : CharacterType
    {
        protected bool Equals(BasicCharacter other)
        {
            return Kind == other.Kind;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BasicCharacter)obj);
        }

        public override int GetHashCode()
        {
            return (Kind != null ? Kind.GetHashCode() : 0);
        }

        public static bool operator ==(BasicCharacter left, BasicCharacter right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BasicCharacter left, BasicCharacter right)
        {
            return !Equals(left, right);
        }

        public string Kind { get; }

        public BasicCharacter(string kind)
        {
            this.Kind = kind;
        }

        public override bool Equals(CharacterType other)
        {
            return Equals((object)other);
        }

        public override string ToString()
        {
            switch (Kind)
            {
                case "unknown":
                    return "N/A";
                case "chapter-title":
                    return "Chapter Title";
                case "narrator":
                    return "Narrator";
                case "sfx":
                    return "SFX";
                default:
                    return Kind;
            }
        }
    }

    public abstract class CharacterType : IEquatable<CharacterType>
    {
        public abstract bool Equals(CharacterType other);
        
        public abstract override string ToString();
    }
}