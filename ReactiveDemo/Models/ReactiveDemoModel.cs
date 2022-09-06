namespace ReactiveDemo.Models
{
    using System;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;

    public class Coord
    {
        public int X { get; set; }
        public int Y { get; set; }
        public override string ToString()
        {
            return $"{X},{Y}";
        }
    }

    public class ReactiveDemoModel
    {
        public void TestReactiveXZip()
        {
            var mm = new Subject<Coord>();
            var s1 = mm.Skip(1);
            var delta = mm.Zip(s1, (prev, curr) => new Coord {
                X = curr.X - prev.X,
                Y = curr.Y - prev.Y
            });
            delta.Subscribe(Console.WriteLine, () => Console.WriteLine(@"Completed"));

            mm.OnNext(new Coord { X = 0, Y = 0 });
            mm.OnNext(new Coord { X = 1, Y = 0 }); //Move across 1
            mm.OnNext(new Coord { X = 3, Y = 2 }); //Diagonally up 2
            mm.OnNext(new Coord { X = 0, Y = 0 }); //Back to 0,0
            mm.OnCompleted();
        }

        public void TestReactiveXSample()
        {
            var interval = Observable.Interval(TimeSpan.FromMilliseconds(150));
            interval.Sample(TimeSpan.FromSeconds(1)).Subscribe(Console.WriteLine);
        }

        public void TestReactiveXMerge()
        {
            //Generate values 0,1,2 
            var s1 = Observable.Interval(TimeSpan.FromMilliseconds(250)).Take(3);

            //Generate values 100,101,102,103,104 
            var s2 = Observable.Interval(TimeSpan.FromMilliseconds(150)).Take(5).Select(i => i + 100);

            s1.Merge(s2).Subscribe(Console.WriteLine, () => Console.WriteLine(@"Completed"));
        }
    }
}