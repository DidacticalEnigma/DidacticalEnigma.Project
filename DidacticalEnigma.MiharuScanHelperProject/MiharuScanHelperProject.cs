using System.Diagnostics.CodeAnalysis;
using DidacticalEnigma.MiharuScanHelperProject.Context;
using DidacticalEnigma.MiharuScanHelperProject.Json;
using DidacticalEnigma.Project;

namespace DidacticalEnigma.MiharuScanHelperProject;

public class MiharuScanHelperProject : IProject
{
    private class MiharuScanHelperProjectRegistration : ProjectFormatHandlerRegistration
    {
        public MiharuScanHelperProjectRegistration() :
            base("Miharu Scan Helper", "*.scan", false)
        {
        }

        public override bool TryOpen(string path, [MaybeNullWhen(false)] out IProject project, [MaybeNullWhen(true)] out string failureReason)
        {
            try
            {
                project = new MiharuScanHelperProject(path);
                failureReason = null;
                return true;
            }
            catch (Exception ex)
            {
                project = null;
                failureReason = ex.Message;
                return false;
            }
        }
    }

    public MiharuScanHelperProject(string path)
    {
        using var textReader = File.OpenText(path);
        var chapter = ChapterIO.Read(textReader);
        Root = new ChapterContext(chapter, tl =>
        {
            tl.Guid.MatchSome(g =>
            {
                var page = chapter.Data.Pages.FirstOrDefault(p => p.TextEntries.Any(te => te.Uuid == g));
                if (page == null)
                {
                    return;
                }
                
                for (int i = 0; i < page.TextEntries.Count; ++i)
                {
                    if (page.TextEntries[i].Uuid == g)
                    {
                        page.TextEntries[i] = tl.GetJson();
                    }
                }

                using var file = File.OpenWrite(path);
                using var textWriter = new StreamWriter(file);
                ChapterIO.Write(textWriter, chapter);
            });
        });
    }

    public void Dispose()
    {
        
    }

    ITranslationContext IProject.Root => Root;

    public ChapterContext Root { get; }
    
    public void Refresh(bool fullRefresh = false)
    {
        throw new NotImplementedException();
    }

    public event EventHandler<TranslationChangedEventArgs>? TranslationChanged;
}