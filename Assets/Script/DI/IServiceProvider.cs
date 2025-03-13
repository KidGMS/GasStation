using System;
namespace Script.DI {
  public interface IServiceProvider {
    void Register<T> (T instance) where T : class;

    void Register<T> (Func<T> factory, Lifetime lifetime) where T : class;

    T Resolve<T>() where T : class;

    bool IsRegistered<T>() where T : class;

    void Unregister<T>() where T : class;

    void Clear();

    void ClearScope();
  }
}