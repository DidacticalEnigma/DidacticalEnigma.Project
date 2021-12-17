using System;
using System.Collections.Generic;
using System.Linq;
using Optional;
using Optional.Collections;
using Utility.Utils;

namespace DidacticalEnigma.Project
{
    public partial class SimpleProject
    {
        private class ContextList : IModifiableTranslationContext<Context>
        {
            private readonly SimpleProject project;
            private readonly ObservableBatchCollection<Context> children;
            private readonly string rootPath;

            IEnumerable<ITranslationContext> ITranslationContext.Children => children;

            public string ReadableIdentifier => rootPath;

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
    }
}