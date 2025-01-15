using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DidacticalEnigma.Project;
using JetBrains.Annotations;
using MagicTranslatorProject.Json;

namespace MagicTranslatorProject
{
    internal class ProjectDirectoryListingProvider
    {
        private MetadataJson metadata;
        
        [NotNull] private readonly IReadOnlyFileSystem readOnlyFileSystem;

        public TextReader FileOpen(string path)
        {
            return new StreamReader(readOnlyFileSystem.FileOpen(path));
        }

        public string GetCapturePath([NotNull] PageId page)
        {
            return FormatPath(
                metadata.Structure.Capture,
                page) + ".json";
        }

        public string GetRawPath([NotNull] PageId page)
        {
            return FormatPath(
                metadata.Structure.Raw,
                page);
        }
        
        public string GetTranslatedPath([NotNull] ChapterId chapter)
        {
            return FormatPath(
                metadata.Structure.Translated,
                chapter.Volume.VolumeNumber, chapter.ChapterNumber, null);
        }
        
        public string GetSavesPath([NotNull] ChapterId chapter)
        {
            return FormatPath(
                metadata.Structure.Save,
                chapter.Volume.VolumeNumber, chapter.ChapterNumber, null);
        }
        
        public string GetRawPath([NotNull] ChapterId chapter)
        {
            return FormatPath(
                metadata.Structure.Raw,
                chapter.Volume.VolumeNumber, chapter.ChapterNumber, null);
        }

        private IEnumerable<int> Enumerate(int volume, int chapter, [NotNull] string group)
        {
            var components = ("/" + metadata.Structure.Raw)
                .Split('/', '\\');
            var rest = components.Zip(components.Skip(1), (current, next) => (current, next))
                .TakeWhile(c => !c.current.Contains("{" + group + "}"))
                .Select(c => c.next)
                .ToList();
            var matcher = new Regex(Regex.Escape(rest[^1])
                .Replace("\\{volume}", GetRegexComponent(metadata.Structure.Volume, "volume"))
                .Replace("\\{chapter}", GetRegexComponent(metadata.Structure.Chapter, "chapter"))
                .Replace("\\{page}", GetRegexComponent(metadata.Structure.Page, "page"))
                .Replace("{volumeDigits}", GetDigitsRegexComponent(metadata.Structure.Volume, "volume"))
                .Replace("{chapterDigits}", GetDigitsRegexComponent(metadata.Structure.Chapter, "chapter"))
                .Replace("{pageDigits}", GetDigitsRegexComponent(metadata.Structure.Page, "page")));

            var rootVolumePath = Path.Combine(
                rest
                    .Take(rest.Count - 1)
                    .Select(c => c.Replace("{volume}", FillPlaceholder(metadata.Structure.Volume, volume)))
                    .Select(c => c.Replace("{chapter}", FillPlaceholder(metadata.Structure.Chapter, chapter)))
                    .ToArray());

            return readOnlyFileSystem.List(rootVolumePath)
                .Select(p => matcher.Match(p))
                .Where(m => m.Success)
                .Select(m => int.Parse(m.Groups[group].Value))
                .Distinct()
                .OrderBy(x => x);
        }

        public IEnumerable<VolumeId> EnumerateVolumes()
        {
            return Enumerate(0, 0, "volume")
                .Select(x => new VolumeId(x));
        }

        public IEnumerable<ChapterId> EnumerateChapters([NotNull] VolumeId volume)
        {
            return Enumerate(volume.VolumeNumber, 0, "chapter")
                .Select(x => new ChapterId(volume, x));
        }

        public IEnumerable<PageId> EnumeratePages([NotNull] ChapterId chapter)
        {
            return Enumerate(chapter.Volume.VolumeNumber, chapter.ChapterNumber, "page")
                .Select(x => new PageId(chapter, x));
        }

        private string FormatPath([NotNull] string path, int volume, int chapter, int? page)
        {
            path = path
                .Replace("{volume}", FillPlaceholder(metadata.Structure.Volume, volume))
                .Replace("{chapter}", FillPlaceholder(metadata.Structure.Chapter, chapter))
                .Replace("{page}", page != null ? FillPlaceholder(metadata.Structure.Page, page.Value) : "")
                .Replace("{volumeDigits}", FillDigitsPlaceholder(metadata.Structure.Volume, volume))
                .Replace("{chapterDigits}", FillDigitsPlaceholder(metadata.Structure.Chapter, chapter))
                .Replace("{pageDigits}", FillDigitsPlaceholder(metadata.Structure.Page, page ?? 0));
            return path;
        }
        
        private string FormatPath([NotNull] string path, [NotNull] PageId pageId)
        {
            path = path
                .Replace("{volume}", FillPlaceholder(metadata.Structure.Volume, pageId.Chapter.Volume.VolumeNumber))
                .Replace("{chapter}", FillPlaceholder(metadata.Structure.Chapter, pageId.Chapter.ChapterNumber))
                .Replace("{page}", FillPlaceholder(metadata.Structure.Page, pageId.PageNumber))
                .Replace("{volumeDigits}", FillDigitsPlaceholder(metadata.Structure.Volume, pageId.Chapter.Volume.VolumeNumber))
                .Replace("{chapterDigits}", FillDigitsPlaceholder(metadata.Structure.Chapter, pageId.Chapter.ChapterNumber))
                .Replace("{pageDigits}", FillDigitsPlaceholder(metadata.Structure.Page, pageId.PageNumber));
            return path;
        }

        private static readonly Regex numberPlaceholder = new Regex("(#+)");

        private int LengthOfNumberPlaceholder([NotNull] string format)
        {
            return numberPlaceholder.Match(format).Groups[1].Value.Length;
        }

        private string GetRegexComponent([NotNull] string format, [NotNull] string groupName)
        {
            var length = LengthOfNumberPlaceholder(format);
            return numberPlaceholder.Replace(format, $"(?<{groupName}>[0-9]{{{length}}})");
        }
        
        private string GetDigitsRegexComponent([NotNull] string format, [NotNull] string groupName)
        {
            var length = LengthOfNumberPlaceholder(format);
            return $"(?<{groupName}>[0-9]{{{length}}})";
        }

        private string FillPlaceholder([NotNull] string format, int value)
        {
            return numberPlaceholder.Replace(format, value.ToString().PadLeft(LengthOfNumberPlaceholder(format), '0'));
        }
        
        private string FillDigitsPlaceholder([NotNull] string format, int value)
        {
            return value.ToString().PadLeft(LengthOfNumberPlaceholder(format), '0');
        }

        public ProjectDirectoryListingProvider([NotNull] MetadataJson metadata, [NotNull] IReadOnlyFileSystem readOnlyFileSystem)
        {
            this.metadata = metadata;
            this.readOnlyFileSystem = readOnlyFileSystem;
        }

        public string GetCharactersPath()
        {
            return Path.Combine(metadata.Structure.Characters ?? "character", "characters.json");
        }
    }
}