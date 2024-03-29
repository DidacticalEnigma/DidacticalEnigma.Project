﻿using System;
using System.Collections.Generic;

namespace DidacticalEnigma.Project
{
    public interface ITranslationContext
    {
        IEnumerable<ITranslationContext> Children { get; }
        
        string ReadableIdentifier { get; }

        string ShortDescription { get; }
    }

    public interface ITranslationContext<out TContext> : ITranslationContext
        where TContext : ITranslationContext
    {
        new IEnumerable<TContext> Children { get; }
    }

    public interface IModifiableTranslationContext : ITranslationContext
    {
        new IReadOnlyList<ITranslationContext> Children { get; }

        ITranslationContext AppendEmpty();

        bool Remove(Guid guid);

        bool Reorder(Guid translationId, Guid moveAt);
    }

    public interface IModifiableTranslationContext<out TContext> :
        ITranslationContext<TContext>,
        IModifiableTranslationContext
        where TContext : ITranslationContext
    {
        new IReadOnlyList<TContext> Children { get; }

        new TContext AppendEmpty();
    }
    
    public interface IReadOnlyTranslation : ITranslationContext
    {
        Translation Translation { get; }
    }

    public interface IEditableTranslation : IReadOnlyTranslation
    {
        ModificationResult Modify(Translation translation);
    }

    public interface IEditableTranslation<out TContext> : ITranslationContext<TContext>, IEditableTranslation
        where TContext : ITranslationContext
    {
        
    }
}