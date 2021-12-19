using System.IO;
using System.Linq;
using Xunit;

namespace DidacticalEnigma.Project.Tests;

public class MiharuScanHelperProjectTests
{
    [Fact]
    public void Test()
    {
        using var project =
            new MiharuScanHelperProject.MiharuScanHelperProject(
                Path.Combine(
                    TestDataPaths.TestDir,
                    "asdf.scan"));

        Assert.Equal(
            new[] { "碇さーん", "それって\r\n女です?", "男です?" }.OrderBy(x => x),
            project.Root.Children.SelectMany(page => page.Children.Select(text => text.Translation.OriginalText)).OrderBy(x => x));
    }
    
    [Fact]
    public void ModificationTest()
    {
        try
        {
            File.Delete(Path.Combine(TestDataPaths.TestDir, "copy.scan"));
            
            File.Copy(
                Path.Combine(TestDataPaths.TestDir, "asdf.scan"),
                Path.Combine(TestDataPaths.TestDir, "copy.scan"));

            using var project =
                new MiharuScanHelperProject.MiharuScanHelperProject(
                    Path.Combine(TestDataPaths.TestDir, "copy.scan"));

            var entry = project.Root.Children
                .SelectMany(page =>
                    page.Children.Where(text => text.Translation.OriginalText == "碇さーん"))
                .First();

            entry.Modify(entry.Translation.With(originalText: "しんじくん"));
            
            var oldEntryLookupAttempt = project.Root.Children
                .SelectMany(page =>
                    page.Children.Where(text => text.Translation.OriginalText == "碇さーん"))
                .FirstOrDefault();
            
            var newEntryLookupAttempt = project.Root.Children
                .SelectMany(page =>
                    page.Children.Where(text => text.Translation.OriginalText == "しんじくん"))
                .FirstOrDefault();
            
            Assert.Null(oldEntryLookupAttempt);
            Assert.NotNull(newEntryLookupAttempt);
        }
        finally
        {
            File.Delete(Path.Combine(TestDataPaths.TestDir, "copy.scan"));
        }
    }
}