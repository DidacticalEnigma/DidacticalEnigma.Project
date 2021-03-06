﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Mime;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Optional;
using Optional.Collections;
using Utility.Utils;

namespace DidacticalEnigma.Project
{
    public class SimpleProject : IProject
    {
        private readonly string path;

        public void Dispose()
        {
            Save();
        }

        private void Save()
        {
            var tls = ((ContextList)Root).Children.Select(c => c.Translation);
            File.WriteAllText(path, JsonConvert.SerializeObject(tls, new OptionConverter()));
        }

        ITranslationContext IProject.Root => Root;

        public IModifiableTranslationContext Root { get; }

        public void Refresh(bool fullRefresh = false)
        {
            if (fullRefresh)
            {
                foreach (var context in ((ContextList)Root).Children)
                {
                    OnTranslationChanged(new TranslationChangedEventArgs(context, context.Translation, TranslationChangedReason.Unknown));
                }
            }
        }

        public event EventHandler<TranslationChangedEventArgs> TranslationChanged;

        public SimpleProject(string path)
        {
            this.path = path;
            IEnumerable<Context> tls;
            try
            {
                tls = JsonConvert.DeserializeObject<IEnumerable<Translation>>(File.ReadAllText(path), new OptionConverter())
                    .Select(t => new Context(t, this));
            }
            catch (FileNotFoundException)
            {
                tls = Enumerable.Empty<Context>();
            }
            Root = new ContextList(path, tls, this);
        }

        private class ContextList : IModifiableTranslationContext<Context>
        {
            private readonly SimpleProject project;
            private readonly ObservableBatchCollection<Context> children;
            private readonly string rootPath;

            IEnumerable<ITranslationContext> ITranslationContext.Children => children;

            IReadOnlyList<Context> IModifiableTranslationContext<Context>.Children => children;

            ITranslationContext IModifiableTranslationContext.AppendEmpty()
            {
                return AppendEmpty();
            }

            public Context AppendEmpty()
            {
                var tl = new Translation(Guid.NewGuid());
                var c = new Context(tl, project);
                children.Add(c);
                project.OnTranslationChanged(new TranslationChangedEventArgs(c, tl, TranslationChangedReason.New));
                return c;
            }

            public bool Remove(Guid guid)
            {
                return children
                    .Indexed()
                    .Where(c => c.element.Translation.Guid == guid.Some())
                    .FirstOrNone()
                    .Match(c =>
                    {
                        var (element, index) = c;
                        children.RemoveAt(index);
                        project.OnTranslationChanged(new TranslationChangedEventArgs(element, element.Translation, TranslationChangedReason.Removed));
                        return true;
                    }, () => false);
            }

            public bool Reorder(Guid translationId, Guid moveAt)
            {
                var sourceIndexOrNone = children
                    .Indexed()
                    .Where(c => c.element.Translation.Guid == translationId.Some())
                    .FirstOrNone()
                    .Map(m => m.index);
                var destinationIndexOrNone = children
                    .Indexed()
                    .Where(c => c.element.Translation.Guid == translationId.Some())
                    .FirstOrNone()
                    .Map(m => m.index);
                return sourceIndexOrNone
                    .FlatMap(s => destinationIndexOrNone.Map(d => (source: s, destination: d)))
                    .Match(c =>
                    {
                        children.Move(c.source, c.destination);
                        return true;
                    }, () => false);
            }

            public IEnumerable<Context> Children => children;

            IReadOnlyList<ITranslationContext> IModifiableTranslationContext.Children => children;

            public ContextList(string rootPath, IEnumerable<Context> children, SimpleProject project)
            {
                this.rootPath = rootPath;
                this.project = project;
                this.children = new ObservableBatchCollection<Context>(children);
            }

            public string ShortDescription => rootPath;
        }

        private class Context : IEditableTranslation
        {
            private readonly SimpleProject project;

            public IEnumerable<ITranslationContext> Children => Enumerable.Empty<ITranslationContext>();

            public Project.Translation Translation { get; private set; }

            public ModificationResult Modify(Project.Translation translation)
            {
                if (translation is Translation t)
                {
                    this.Translation = t;
                    project.Save();
                    project.OnTranslationChanged(new TranslationChangedEventArgs(this, t, TranslationChangedReason.InPlaceModification));
                    return ModificationResult.WithSuccess(Translation);
                }
                else
                {
                    return ModificationResult.WithUnsupported("attempted to save translation with extra data");
                }
            }

            public string ShortDescription => "";

            public Context(Project.Translation translation, SimpleProject project)
            {
                this.project = project;
                this.Translation = translation;
            }
        }

        public class Translation : Project.Translation
        {
            public override Option<Guid> Guid { get; }
            public override string OriginalText { get; }
            public override string TranslatedText { get; }
            public override IEnumerable<GlossNote> Glosses { get; }
            public override IEnumerable<TranslatorNote> Notes { get; }
            public override IEnumerable<TranslatedText> AlternativeTranslations { get; }

            public override Project.Translation With(
                string originalText = null,
                string translatedText = null,
                IEnumerable<GlossNote> glosses = null,
                IEnumerable<TranslatorNote> notes = null, IEnumerable<TranslatedText> alternativeTranslations = null)
            {
                return new Translation(
                    this.Guid,
                    originalText ?? this.OriginalText,
                    translatedText ?? this.TranslatedText,
                    glosses ?? this.Glosses,
                    notes ?? this.Notes,
                    alternativeTranslations ?? this.AlternativeTranslations);
            }

            [UsedImplicitly]
            public Translation(
                Option<Guid> guid,
                string originalText,
                string translatedText,
                IEnumerable<GlossNote> glosses,
                IEnumerable<TranslatorNote> notes,
                IEnumerable<TranslatedText> alternativeTranslations)
            {
                Guid = guid;
                OriginalText = originalText ?? throw new ArgumentNullException(nameof(originalText));
                TranslatedText = translatedText ?? throw new ArgumentNullException(nameof(translatedText));
                Glosses = glosses ?? throw new ArgumentNullException(nameof(glosses));
                Notes = notes ?? throw new ArgumentNullException(nameof(notes));
                AlternativeTranslations = alternativeTranslations ?? throw new ArgumentNullException(nameof(alternativeTranslations));
            }

            public Translation(
                Guid guid)
            {
                Guid = guid.Some();
                OriginalText = "";
                TranslatedText = "";
                Glosses = Enumerable.Empty<GlossNote>();
                Notes = Enumerable.Empty<TranslatorNote>();
                AlternativeTranslations = Enumerable.Empty<TranslatedText>();
            }
        }

        protected virtual void OnTranslationChanged(TranslationChangedEventArgs e)
        {
            TranslationChanged?.Invoke(this, e);
        }
    }
}
