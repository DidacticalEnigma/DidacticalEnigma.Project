using System;
using JetBrains.Annotations;

namespace MagicTranslatorProject
{
    internal class ChapterId
    {
        [NotNull] public VolumeId Volume { get; }

        public int ChapterNumber { get; }

        public ChapterId([NotNull] VolumeId volume, int chapterNumber)
        {
            Volume = volume ?? throw new ArgumentNullException(nameof(volume));
            ChapterNumber = chapterNumber;
        }
        
        public override string ToString()
        {
            return $"{Volume}/CH{ChapterNumber:D8}";
        }
    }
}