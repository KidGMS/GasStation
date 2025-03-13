using System;

namespace Script.SaveSystem {
  public interface ISaveSystem {
    void Save<T> (string key, T data);

    T Load<T> (string key);

    bool Exists (string key);
  }
}