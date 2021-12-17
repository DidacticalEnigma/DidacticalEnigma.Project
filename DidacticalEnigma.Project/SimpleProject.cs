using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Mime;
using Newtonsoft.Json;
using Utility.Utils;

namespace DidacticalEnigma.Project
{
    public partial class SimpleProject : IProject
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

        protected virtual void OnTranslationChanged(TranslationChangedEventArgs e)
        {
            TranslationChanged?.Invoke(this, e);
        }
    }
}
