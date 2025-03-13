using Script.SaveSystem;

namespace Script.DI {
  public class Lifetime {
    public static readonly Lifetime Singleton = new Lifetime(DiConstant.SINGLETON);
    public static readonly Lifetime Transient = new Lifetime(DiConstant.TRANSIENT);
    public static readonly Lifetime Scoped = new Lifetime(DiConstant.SCOPED);

    public string Name { get; }

    private Lifetime (string name) {
      Name = name;
    }

    public override string ToString() {
      return Name;
    }
  }
}