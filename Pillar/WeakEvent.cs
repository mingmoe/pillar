namespace Pillar;

/// <summary>
///     非线程安全的,使用弱引用的事件源.
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly struct WeakEvent<T>() where T : EventArgs
{
	private readonly List<WeakReference<EventHandler<T>>> _handlers = [];

	public void ClearAllHandlers()
	{
        _handlers.Clear();
	}

	public IEnumerable<Exception> Fire(object? source, T @event, bool ignoreError = false)
	{
		List<Exception> exceptions = new(2);
		for (var index = 0; index < _handlers.Count; index++)
		{
			try
			{
				var handler = _handlers[index];

				if (handler.TryGetTarget(out var target))
				{
					target.Invoke(source, @event);
					continue;
				}

                _handlers.RemoveAt(index);
				index--;
			}
			catch (Exception ex)
			{
				if (!ignoreError)
					throw;
				exceptions.Add(ex);
			}
		}

		return exceptions;
	}

	public void Register(EventHandler<T> handler)
	{
        _handlers.Add(new WeakReference<EventHandler<T>>(handler));
	}

	public void Unregister(EventHandler<T> handler)
	{
        _handlers.RemoveAll(e =>
		{
			if (!e.TryGetTarget(out var obj)) return true;
			return obj == handler;
		});
	}
}
