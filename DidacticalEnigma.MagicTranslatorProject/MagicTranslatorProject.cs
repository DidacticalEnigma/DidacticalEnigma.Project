using System;
using System.IO;
using System.Text.RegularExpressions;
using DidacticalEnigma.Project;
using JetBrains.Annotations;
using MagicTranslatorProject.Context;
using MagicTranslatorProject.Json;
using Newtonsoft.Json;

namespace MagicTranslatorProject
{
    public class MagicTranslatorProject : IProject
    {
        [NotNull] private readonly IReadOnlyFileSystem rootFs;

        private class MagicTranslatorProjectRegistration : ProjectFormatHandlerRegistration
        {
            public MagicTranslatorProjectRegistration() :
                base("Magic Translator", "*", true)
            {
            }

            public override bool TryOpen([NotNull] string path, out IProject project, out string failureReason)
            {
                try
                {
                    var rootFs = new ReadOnlyFileSystem(path);
                    var metadata = Validate(rootFs);
                    var p = new MagicTranslatorProject();
                    p.Init(metadata, rootFs);
                    project = p;
                    failureReason = "";
                    return true;
                }
                catch (FileNotFoundException)
                {
                    failureReason = "This is not a valid Magic Translator project: metadata.json file is missing.";
                }
                catch (JsonSerializationException)
                {
                    failureReason = "This is not a valid Magic Translator project: the metadata.json is not valid.";
                }
                catch (InvalidDataException ex)
                {
                    failureReason = $"This is not a valid Magic Translator project: ${ex.Message}";
                }
                project = null;
                return false;
            }
        }

        public static ProjectFormatHandlerRegistration Registration { get; } = new MagicTranslatorProjectRegistration();

        public void Dispose()
        {

        }

        ITranslationContext IProject.Root => Root;

        public MangaContext Root { get; private set; }

        public void Refresh(bool fullRefresh = false)
        {
            if (fullRefresh)
            {
                foreach (var volume in Root.Children)
                {
                    foreach (var chapter in volume.Children)
                    {
                        foreach (var page in chapter.Children)
                        {
                            foreach (var capture in page.Children)
                            {
                                OnTranslationChanged(new TranslationChangedEventArgs(capture, capture.Translation, TranslationChangedReason.Unknown));
                            }
                        }
                    }
                }
            }
        }

        public event EventHandler<TranslationChangedEventArgs> TranslationChanged;
        
        private static MetadataJson Validate([NotNull] IReadOnlyFileSystem rootFs)
        {
            using var fileStream = rootFs.FileOpen("metadata.json");
            using var reader = new StreamReader(fileStream);
            using var jsonReader = new JsonTextReader(reader);
            var serializer = new JsonSerializer();
            
            var metadata = serializer.Deserialize<MetadataJson>(jsonReader);
            if (!HasContinuousDigitPlaceholders(metadata.Structure.Volume))
            {
                throw new InvalidDataException("Volume path cannot have separated digits");
            }

            if (!HasContinuousDigitPlaceholders(metadata.Structure.Chapter))
            {
                throw new InvalidDataException("Chapter path cannot have separated digits");
            }

            if (!HasContinuousDigitPlaceholders(metadata.Structure.Page))
            {
                throw new InvalidDataException("Page path cannot have separated digits");
            }

            var matcher = new Regex(@"^.*?\{volume}.*?\{chapter}.*?\{page}.*?$");
            foreach (var p in new[]{ metadata.Structure.Raw, metadata.Structure.Translated, metadata.Structure.Save, metadata.Structure.Capture })
            {
                if(!matcher.IsMatch(p))
                    throw new InvalidDataException("The {volume} placeholder must be before {chapter} placeholder, which must be before {page} placeholder");
            }

            return metadata;
        }

        private static bool HasContinuousDigitPlaceholders([NotNull] string input)
        {
            return true;
        }

        private MagicTranslatorProject()
        {

        }

        private void Init([NotNull] MetadataJson json, [NotNull] IReadOnlyFileSystem rootFs)
        {
            Root = new MangaContext(json, new ProjectDirectoryListingProvider(json, rootFs));
        }

        public MagicTranslatorProject([NotNull] string path)
        {
            this.rootFs = new ReadOnlyFileSystem(path);
            var json = Validate(rootFs);
            Init(json, rootFs);
        }
        
        public MagicTranslatorProject([NotNull] IReadOnlyFileSystem rootFs)
        {
            this.rootFs = rootFs;
            var json = Validate(rootFs);
            Init(json, rootFs);
        }

        protected virtual void OnTranslationChanged([NotNull] TranslationChangedEventArgs e)
        {
            TranslationChanged?.Invoke(this, e);
        }
    }
}