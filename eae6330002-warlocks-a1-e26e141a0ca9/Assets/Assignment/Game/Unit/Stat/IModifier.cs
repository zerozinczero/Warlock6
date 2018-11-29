using System;

public interface IModifier<T> {
    T Modify(T t, object context = null);
    int Priority { get; }
    // TODO: OnChangedEvent
}
