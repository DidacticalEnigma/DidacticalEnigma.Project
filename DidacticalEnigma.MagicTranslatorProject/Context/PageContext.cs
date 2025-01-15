using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DidacticalEnigma.Project;
using JetBrains.Annotations;
using MagicTranslatorProject.Json;
using Newtonsoft.Json;
using Optional;
using Point = MagicTranslatorProject.Json.Point;

namespace MagicTranslatorProject.Context
{
    public class PageContext : ITranslationContext<CaptureContext>
    {
        IEnumerable<ITranslationContext> ITranslationContext.Children => captures;

        public string ShortDescription => $"{name}: Volume {page.Chapter.Volume.VolumeNumber}, Chapter {page.Chapter.ChapterNumber}, Page {page.PageNumber}";
        
        public string ReadableIdentifier => page.ToString();

        internal PageContext([NotNull] MangaContext root, [NotNull] ProjectDirectoryListingProvider listing, [NotNull] PageId page)
        {
            this.name = root.Name;
            this.listing = listing;
            this.page = page;
            try
            {
                var serializer = new JsonSerializer();
                serializer.Converters.Add(new CharacterTypeConverter(root.IdNameMapping));
                
                using var file = listing.FileOpen(listing.GetCapturePath(page));
                using var jsonReader = new JsonTextReader(file);
                var pageJson = serializer.Deserialize<PageJson>(jsonReader);
                
                captures = pageJson.Captures
                    .Select((c, i) =>
                    {
                        var guid = root.Map(page, c.Id).Some();
                        return new CaptureContext(this, c, new Translation(c, guid), new CaptureId(page, c.Id));
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
        
        public Rectangle<double> GetRelativeRectangle(CaptureContext captureContext, Size<int> pageSize)
        {
            if (captureContext.Version == 1)
            {
                // version 1's rectangles are in pixels
                return new Rectangle<double>(
                    x: captureContext.Position.X / pageSize.Width,
                    y: captureContext.Position.Y / pageSize.Height,
                    width: captureContext.Size.X / pageSize.Width,
                    height: captureContext.Size.Y / pageSize.Height);
            }
            else if (captureContext.Version == 2)
            {
                // version 2's rectangles is a [0; 1] float64 relative to the size of the image
                return new Rectangle<double>(
                    x: captureContext.Position.X,
                    y: captureContext.Position.Y,
                    width: captureContext.Size.X,
                    height: captureContext.Size.Y);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
        
        public Rectangle<int> GetAbsoluteRectangle(CaptureContext captureContext, Size<int> pageSize)
        {
            if (captureContext.Version == 1)
            {
                // version 1's rectangles are in pixels
                return new Rectangle<int>(
                    x: (int)captureContext.Position.X,
                    y: (int)captureContext.Position.Y,
                    width: (int)captureContext.Size.X,
                    height: (int)captureContext.Size.Y);
            }
            else if (captureContext.Version == 2)
            {
                // version 2's rectangles is a [0; 1] float64 relative to the size of the image
                return new Rectangle<int>(
                    x: (int)(captureContext.Position.X * pageSize.Width),
                    y: (int)(captureContext.Position.Y * pageSize.Height),
                    width: (int)(captureContext.Size.X * pageSize.Width),
                    height: (int)(captureContext.Size.Y * pageSize.Height));
            }
            else
            {
                throw new InvalidOperationException();
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
