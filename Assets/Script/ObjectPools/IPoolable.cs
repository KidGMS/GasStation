namespace Script.ObjectPools {
  public interface IPoolable<T> {
    IObjectPool<T> Pool { get; set; }

    void OnPool();

    void ReturnToPool();
  }
}