namespace ReactiveDemo.Events
{
    using Prism.Events;

    public class ReactiveDemoEvents
    {
        public class Test1 : PubSubEvent<int>
        {
        }
    }
}