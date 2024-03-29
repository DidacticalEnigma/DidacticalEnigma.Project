﻿using System;
using System.Collections.Generic;
using System.Linq;
using DidacticalEnigma.Project;

namespace MagicTranslatorProject.Context
{
    public class VolumeContext : ITranslationContext<ChapterContext>
    {
        private MangaContext root;

        internal VolumeContext(MangaContext root, VolumeId volume, ProjectDirectoryListingProvider listing)
        {
            this.volume = volume;
            this.root = root;
            this.listing = listing;
        }

        private IReadOnlyCollection<ChapterContext> Load()
        {
            return listing.EnumerateChapters(volume)
                .Select(ch =>
                {
                    return new ChapterContext(root, ch, listing);
                })
                .ToList();
        }

        private readonly WeakReference<IReadOnlyCollection<ChapterContext>> children = new WeakReference<IReadOnlyCollection<ChapterContext>>(null);

        private VolumeId volume;
        private ProjectDirectoryListingProvider listing;

        public IEnumerable<ChapterContext> Children
        {
            get
            {
                if (children.TryGetTarget(out var v))
                {
                    return v;
                }
                else
                {
                    v = Load();
                    children.SetTarget(v);
                    return v;
                }
            }
        }

        public string ShortDescription => $"{root.Name}: Volume {volume.VolumeNumber}";
        
        public string ReadableIdentifier => volume.ToString();

        IEnumerable<ITranslationContext> ITranslationContext.Children => Children;
    }
}