using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DidacticalEnigma.Project;
using MagicTranslatorProject.Json;
using Newtonsoft.Json;
using Utility.Utils;

namespace MagicTranslatorProject.Context
{
    public class MangaContext : ITranslationContext<VolumeContext>
    {
        public IEnumerable<VolumeContext> Children
        {
            get
            {
                if (volumes.TryGetTarget(out var v))
                {
                    return v;
                }
                else
                {
                    v = Load();
                    volumes.SetTarget(v);
                    return v;
                }
            }
        }
        
        public string ReadableIdentifier => "";
        
        public string ShortDescription => Name;

        IEnumerable<ITranslationContext> ITranslationContext.Children => Children;

        private WeakReference<IReadOnlyCollection<VolumeContext>> volumes = new WeakReference<IReadOnlyCollection<VolumeContext>>(null);

        internal MangaContext(MetadataJson metadata, ProjectDirectoryListingProvider listing)
        {
            this.listing = listing;
            this.metadata = metadata;

            using var file = listing.FileOpen(listing.GetCharactersPath());
            using var jsonReader = new JsonTextReader(file);
            var serializer = new JsonSerializer();
            
            this.charactersJson = serializer.Deserialize<CharactersJson>(jsonReader);
            IdNameMapping = new DualDictionary<long, string>(charactersJson.Characters
                .ToDictionary(c => c.Id, c => c.Name));
            this.characterTypeConverter = new CharacterTypeConverter(IdNameMapping);
        }

        private readonly MetadataJson metadata;

        private IReadOnlyCollection<VolumeContext> Load()
        {
            return listing.EnumerateVolumes()
                .Select(vol =>
                {
                    return new VolumeContext(this, vol, listing);
                })
                .ToList();
        }

        public IEnumerable<CharacterType> AllCharacters => charactersJson.Characters
            .Select(c => (CharacterType)new NamedCharacter(c.Id, c.Name))
            .Concat(new CharacterType[]
            {
                new BasicCharacter("unknown"),
                new BasicCharacter("chapter-title"),
                new BasicCharacter("narrator"),
                new BasicCharacter("sfx")
            });

        internal IReadOnlyDualDictionary<long, string> IdNameMapping { get; }

        internal Guid Map(PageId page, long captureId)
        {
            return guidMap.GetOrAdd(
                (page, captureId),
                x => Guid.NewGuid());
        }

        private ConcurrentDictionary<(PageId page, long captureId), Guid> guidMap =
            new ConcurrentDictionary<(PageId page, long captureId), Guid>();

        private readonly ProjectDirectoryListingProvider listing;
        
        private readonly CharactersJson charactersJson;
        
        private readonly CharacterTypeConverter characterTypeConverter;

        public string Name => metadata.Name;
    }
}