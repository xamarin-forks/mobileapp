namespace Toggl.Daneel.Extensions
{
    public class Reactive<TBase>
    {
        public TBase Base { get; set; }

        public Reactive(TBase _base)
        {
            Base = _base;
        }
    }

    public static class NSObjectReactiveCompatibleExtensions
    {
        public static Reactive<NSObject> Rx<NSObject>(this NSObject nsObject)
            => new Reactive<NSObject>(nsObject);
    }
}


