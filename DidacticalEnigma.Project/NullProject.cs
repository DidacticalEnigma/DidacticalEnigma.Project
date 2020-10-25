using System;
using System.Collections.Generic;
using System.Linq;

namespace DidacticalEnigma.Project
{
    public class NullProject : IProject
    {
        public static NullProject Instance { get; } = new NullProject();

        public void Dispose()
        {
            // do nothing
        }

        public ITranslationContext Root { get; } = new Context();

        public void Refresh(bool fullRefresh = false)
        {
            // do nothing
        }

        public event EventHandler<TranslationChangedEventArgs> TranslationChanged;

        private class Context : ITranslationContext
        {
            public IEnumerable<ITranslationContext> Children => Enumerable.Empty<ITranslationContext>();

            public string ShortDescription => "";
        }
    }
}
