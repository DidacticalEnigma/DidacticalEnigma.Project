using System.Collections.Generic;
using System.Linq;

namespace DidacticalEnigma.Project
{
    public partial class SimpleProject
    {
        private class Context : IEditableTranslation
        {
            private readonly SimpleProject project;

            public IEnumerable<ITranslationContext> Children => Enumerable.Empty<ITranslationContext>();
            
            public string ReadableIdentifier => "";

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
    }
}