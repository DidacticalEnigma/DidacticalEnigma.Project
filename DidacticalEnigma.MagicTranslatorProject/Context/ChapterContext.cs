using System;
using System.Collections.Generic;
using System.Linq;
using DidacticalEnigma.Project;

namespace MagicTranslatorProject.Context
{
    public class ChapterContext : ITranslationContext<PageContext>
    {
        private readonly MangaContext root;

        internal ChapterContext(MangaContext root, ChapterId chapter, ProjectDirectoryListingProvider listing)
        {
            this.root = root;
            this.chapter = chapter;
            this.listing = listing;
        }

        private IReadOnlyCollection<PageContext> Load()
        {
            return listing.EnumeratePages(chapter)
                .Select(page => new PageContext(root, listing, page))
                .ToList();
        }

        IEnumerable<ITranslationContext> ITranslationContext.Children => Children;

        public string ShortDescription => $"{root.Name}: Volume {chapter.Volume.VolumeNumber}, Chapter {chapter.ChapterNumber}";

        private readonly WeakReference<IReadOnlyCollection<PageContext>> children = new WeakReference<IReadOnlyCollection<PageContext>>(null);
        private ProjectDirectoryListingProvider listing;
        private ChapterId chapter;

        public IEnumerable<PageContext> Children
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
    }
}