using System.IO;
using System.Linq;
using Xunit;
namespace DidacticalEnigma.Project.Tests;

public class MagicTranslatorProjectTests
{
    [Fact]
    public void Listing()
    {
        using var project =
            new MagicTranslatorProject.MagicTranslatorProject(
                Path.Combine(
                    TestDataPaths.MagicTranslatorTestDir,
                    "Basic"));
        
        Assert.Equal(new []{"/VOL00000010", "/VOL00000020"}, project.Root.Children.Select(vol => vol.ReadableIdentifier).OrderBy(x => x));
        Assert.Equal(
            new []{
                "Character: Invincible Super Hero",
                "Character: Lame Guy",
                "N/A",
                "Chapter Title",
                "Narrator",
                "SFX" }.OrderBy(x => x),
            project.Root.AllCharacters.Select(ch => ch.ToString()).OrderBy(x => x));
        Assert.Equal(
            new []{"/VOL00000010/CH00000001", "/VOL00000010/CH00000002"}.OrderBy(x => x),
            project.Root.Children
                .First(vol => vol.ReadableIdentifier == "/VOL00000010")
                .Children
                .Select(ch => ch.ReadableIdentifier)
                .OrderBy(x => x));
        
        Assert.Equal(
            new []{"/VOL00000010/CH00000001/P00000001", "/VOL00000010/CH00000001/P00000002"}.OrderBy(x => x),
            project.Root.Children
                .First(vol => vol.ReadableIdentifier == "/VOL00000010")
                .Children
                .First(ch => ch.ReadableIdentifier == "/VOL00000010/CH00000001")
                .Children
                .Select(p => p.ReadableIdentifier)
                .OrderBy(x => x));
        
        Assert.Equal(
            new []{"UNLIMITED POWER!!", "menacing"}.OrderBy(x => x),
            project.Root.Children
                .First(vol => vol.ReadableIdentifier == "/VOL00000010")
                .Children
                .First(ch => ch.ReadableIdentifier == "/VOL00000010/CH00000001")
                .Children
                .First(p => p.ReadableIdentifier == "/VOL00000010/CH00000001/P00000002")
                .Children
                .Select(capture => capture.Translation.TranslatedText)
                .OrderBy(x => x));
    }
    
    [Fact]
    public void ListingDigits()
    {
        using var project =
            new MagicTranslatorProject.MagicTranslatorProject(
                Path.Combine(
                    TestDataPaths.MagicTranslatorTestDir,
                    "BasicDigits"));
        
        Assert.Equal(new []{"/VOL00000010", "/VOL00000020"}, project.Root.Children.Select(vol => vol.ReadableIdentifier).OrderBy(x => x));
        Assert.Equal(
            new []{
                "Character: Invincible Super Hero",
                "Character: Lame Guy",
                "N/A",
                "Chapter Title",
                "Narrator",
                "SFX" }.OrderBy(x => x),
            project.Root.AllCharacters.Select(ch => ch.ToString()).OrderBy(x => x));
        Assert.Equal(
            new []{"/VOL00000010/CH00000001", "/VOL00000010/CH00000002"}.OrderBy(x => x),
            project.Root.Children
                .First(vol => vol.ReadableIdentifier == "/VOL00000010")
                .Children
                .Select(ch => ch.ReadableIdentifier)
                .OrderBy(x => x));
        
        Assert.Equal(
            new []{"/VOL00000010/CH00000001/P00000001", "/VOL00000010/CH00000001/P00000002"}.OrderBy(x => x),
            project.Root.Children
                .First(vol => vol.ReadableIdentifier == "/VOL00000010")
                .Children
                .First(ch => ch.ReadableIdentifier == "/VOL00000010/CH00000001")
                .Children
                .Select(p => p.ReadableIdentifier)
                .OrderBy(x => x));
        
        Assert.Equal(
            new []{"UNLIMITED POWER!!", "menacing"}.OrderBy(x => x),
            project.Root.Children
                .First(vol => vol.ReadableIdentifier == "/VOL00000010")
                .Children
                .First(ch => ch.ReadableIdentifier == "/VOL00000010/CH00000001")
                .Children
                .First(p => p.ReadableIdentifier == "/VOL00000010/CH00000001/P00000002")
                .Children
                .Select(capture => capture.Translation.TranslatedText)
                .OrderBy(x => x));
    }
}