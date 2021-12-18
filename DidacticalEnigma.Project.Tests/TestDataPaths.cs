using System;
using System.IO;

namespace DidacticalEnigma.Project.Tests;

    public class TestDataPaths
    {
        // FIX THIS PATH SO IT POINTS TO THE ACTUAL DIRECTORY YOUR DATA IS
        // Various test runners put the executables in unrelated places,
        // and also make the current directory unrelated.
        public static readonly string BaseDir = Environment.GetEnvironmentVariable("APPVEYOR_BUILD_FOLDER") is { } envPath
            ? Path.Combine(envPath, "Data")
            : @"/home/milleniumbug/dokumenty/PROJEKTY/InDevelopment/DidacticalEnigma/Data";

        public static readonly string TestDir = Environment.GetEnvironmentVariable("APPVEYOR_BUILD_FOLDER") is { } envPath
            ? Path.Combine(envPath, "TestData")
            : @"/home/milleniumbug/dokumenty/PROJEKTY/InDevelopment/DidacticalEnigma/TestData";

        public static readonly string MagicTranslatorTestDir = Path.Combine(TestDir, "MTProjects");
    }