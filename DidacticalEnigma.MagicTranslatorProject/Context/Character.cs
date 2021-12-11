namespace MagicTranslatorProject.Context
{
    public class Character
    {
        public long Id { get; }
        
        public string Name { get; }

        public Character(long id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}