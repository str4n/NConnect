namespace NConnect.Shared.Abstractions.CQRS.Queries;

public interface IQuery;

public interface IQuery<TResult> : IQuery;