using System;
using UnityEngine;

public abstract class CommandCandidate : ScriptableObject
{
    public abstract bool IsValidForInput(ICommandContext context);

    public abstract ICommand CreateCommand(ICommandContext context);
}

public abstract class CommandCandidate<TContext> : CommandCandidate
    where TContext : ICommandContext
{
    public override bool IsValidForInput(ICommandContext context)
    {
        if (context is not TContext typedContext)
            return false;

        return IsValidForInput(typedContext);
    }

    public override ICommand CreateCommand(ICommandContext context)
    {
        if (context is not TContext typedContext)
            throw new ArgumentException($"Invalid context type passed to {GetType().Name}. Expected {typeof(TContext).Name}, got {context?.GetType().Name}");

        return CreateCommand(typedContext);
    }

    protected abstract bool IsValidForInput(TContext context);
    protected abstract ICommand CreateCommand(TContext context);
}