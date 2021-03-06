﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DidacticalEnigma.Project;
using JetBrains.Annotations;
using MagicTranslatorProject.Json;
using Newtonsoft.Json;
using Optional;

namespace MagicTranslatorProject.Context
{
    public class PageContext : ITranslationContext<CaptureContext>
    {
        IEnumerable<ITranslationContext> ITranslationContext.Children => captures;

        public string ShortDescription => $"{name}: Volume {page.Chapter.Volume.VolumeNumber}, Chapter {page.Chapter.ChapterNumber}, Page {page.PageNumber}";

        internal PageContext([NotNull] MangaContext root, [NotNull] ProjectDirectoryListingProvider listing, [NotNull] PageId page)
        {
            this.name = root.Name;
            this.listing = listing;
            this.page = page;
            try
            {
                var pageJson = JsonConvert.DeserializeObject<PageJson>(File.ReadAllText(listing.GetCapturePath(page)),
                    new CharacterTypeConverter(root.IdNameMapping));
                captures = pageJson.Captures
                    .Select((c, i) =>
                    {
                        var guid = root.Map(page, c.Id).Some();
                        return new CaptureContext(this, c, j =>
                        {
                            pageJson.Captures[i] = j;
                            return ModificationResult.WithSuccess(new Translation(c, guid));
                        }, new Translation(c, guid));
                    })
                    .ToList();
            }
            catch (FileNotFoundException)
            {
                this.captures = Array.Empty<CaptureContext>();
            }
            catch (DirectoryNotFoundException)
            {
                this.captures = Array.Empty<CaptureContext>();
            }
        }

        public string PathToRaw => this.listing.GetRawPath(page);

        private readonly IList<CaptureContext> captures;

        private readonly string name;

        [NotNull] private readonly ProjectDirectoryListingProvider listing;
        
        private PageId page;

        public IEnumerable<CaptureContext> Children => captures;
    }
}
