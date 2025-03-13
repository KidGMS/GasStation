using System;
using System.Collections.Generic;

namespace Script.DI {
  public class DiContainer : IServiceProvider {
    private readonly Dictionary<Type, object> _singletons = new Dictionary<Type, object>();
    private readonly Dictionary<Type, Func<object>> _factories = new Dictionary<Type, Func<object>>();
    private readonly HashSet<Type> _scoped = new HashSet<Type>();
    private readonly Dictionary<Lifetime, Action<Type, Func<object>>> _registrationMethods;

    public DiContainer() {
      _registrationMethods = new Dictionary<Lifetime, Action<Type, Func<object>>> {
        {
          Lifetime.Singleton, RegisterSingleton
        }, {
          Lifetime.Transient, RegisterTransient
        }, {
          Lifetime.Scoped, RegisterScoped
        }
      };
    }

    public void Register<T> (T instance) where T : class {
      RegisterSingleton(typeof(T), () => instance);
    }

    public void Register<T> (Func<T> factory, Lifetime lifetime) where T : class {
      if (_registrationMethods.TryGetValue(lifetime, out Action<Type, Func<object>> method)) {
        method(typeof(T), () => factory());
      } else {
        throw new ArgumentException($"Unknown lifetime: {lifetime}");
      }
    }

    public T Resolve<T>() where T : class {
      return (T)CreateInstance(typeof(T));
    }

    public object Resolve (Type type) {
      if (_singletons.TryGetValue(type, out object singleton)) {
        return singleton;
      }

      if (_factories.TryGetValue(type, out Func<object> factory)) {
        return factory();
      }

      throw new Exception($"Service of type {type} is not registered.");
    }

    public bool IsRegistered<T>() where T : class {
      Type type = typeof(T);
      return _singletons.ContainsKey(type) || _factories.ContainsKey(type);
    }

    public void Unregister<T>() where T : class {
      Type type = typeof(T);
      _singletons.Remove(type);
      _factories.Remove(type);
      _scoped.Remove(type);
    }

    public void Clear() {
      _singletons.Clear();
      _factories.Clear();
      _scoped.Clear();
    }

    public void ClearScope() {
      foreach (Type type in _scoped) {
        _singletons.Remove(type);
      }
    }

    private void RegisterSingleton (Type type, Func<object> factory) {
      if (!_singletons.ContainsKey(type)) {
        _singletons[type] = factory();
      }
    }

    private void RegisterTransient (Type type, Func<object> factory) {
      _factories[type] = factory;
    }

    private void RegisterScoped (Type type, Func<object> factory) {
      _scoped.Add(type);
      _factories[type] = factory;
    }

    private object CreateInstance (Type type) {
      if (_singletons.TryGetValue(type, out object singleton)) {
        return singleton;
      }

      if (_factories.TryGetValue(type, out Func<object> factory)) {
        object instance = factory();

        if (_scoped.Contains(type)) {
          _singletons[type] = instance;
        }

        return instance;
      }

      throw new Exception($"Service of type {type} is not registered.");
    }
  }
}