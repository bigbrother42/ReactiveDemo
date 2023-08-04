using System;
using System.Reactive.Subjects;
using System.Threading;
using ReactiveDemo.Util;

namespace ReactiveDemo.ViewModels.Observer
{
    public class ObserverViewModel : ViewModelBase
    {
        #region Models



        #endregion

        #region ReactiveProperty



        #endregion

        #region ReactiveCommand



        #endregion

        #region Override

        protected override void InitData()
        {
            base.InitData();

            // https://zhuanlan.zhihu.com/p/486899755

            SubjectTest1();

            var strVal = "222";
            var intVal = strVal.ToInt();
        }

        protected override void RegisterCommands()
        {
            base.RegisterCommands();
        }

        protected override void RegisterProperties()
        {
            base.RegisterProperties();
        }

        protected override void RegisterPubEvents()
        {
            base.RegisterPubEvents();
        }

        #endregion

        #region Functions

        #region Subject

        private void SubjectTest1()
        {
            var subject = new Subject<int>();
            subject.Subscribe(Console.WriteLine);
            subject.OnNext(1);
            subject.OnNext(2);
            subject.OnNext(3);
            subject.OnNext(4);
        }

        private void SubjectTest2()
        {
            var subject = new Subject<int>();
            subject.OnNext(1);
            subject.OnNext(2);
            subject.Subscribe(Console.WriteLine);
            subject.OnNext(3);
            subject.OnNext(4);
        }

        #endregion

        #region ReplaySubject

        private void ReplaySubjectTest1()
        {
            var subject = new ReplaySubject<int>();
            subject.OnNext(1);
            subject.OnNext(2);
            subject.Subscribe(Console.WriteLine);
            subject.OnNext(3);
            subject.OnNext(4);
        }

        private void ReplaySubjectTest2()
        {
            var subject = new ReplaySubject<int>(1);
            subject.OnNext(1);
            subject.OnNext(2);
            subject.Subscribe(Console.WriteLine);
            subject.OnNext(3);
            subject.OnNext(4);
        }

        private void ReplaySubjectTest3()
        {
            var subject = new ReplaySubject<int>(TimeSpan.FromMilliseconds(1000));
            subject.OnNext(1);
            Thread.Sleep(500);
            subject.OnNext(2);
            Thread.Sleep(200);
            subject.OnNext(3);
            Thread.Sleep(500);
            subject.Subscribe(Console.WriteLine);
            subject.OnNext(4);
            Thread.Sleep(500);
        }

        #endregion

        #region BehaviorSubject

        private void BehaviorSubjectTest1()
        {
            var subject = new BehaviorSubject<int>(0);
            subject.OnNext(1);
            subject.OnNext(2);
            subject.Subscribe(Console.WriteLine);
            subject.OnNext(3);
            subject.OnNext(4);
        }

        private void BehaviorSubjectTest2()
        {
            var subject = new BehaviorSubject<int>(99);
            subject.Subscribe(Console.WriteLine);
            subject.OnNext(1);
            subject.OnNext(2);
            subject.OnNext(3);
            subject.OnNext(4);
        }

        private void BehaviorSubjectTest3()
        {
            var subject = new BehaviorSubject<int>(0);
            subject.OnNext(1);
            subject.OnNext(2);
            subject.OnNext(3);
            subject.OnNext(4);
            subject.OnCompleted();
            subject.Subscribe(Console.WriteLine);
        }

        #endregion

        #region AsyncSubject

        private void AsyncSubjectTest1()
        {
            var subject = new AsyncSubject<int>();
            subject.Subscribe(Console.WriteLine);
            subject.OnNext(1);
            subject.OnNext(2);
            subject.OnNext(3);
            subject.OnNext(4);
            subject.OnCompleted();
        }

        private void AsyncSubjectTest2()
        {
            var subject = new AsyncSubject<int>();
            subject.Subscribe(Console.WriteLine);
            subject.OnNext(1);
            subject.OnNext(2);
            subject.OnNext(3);
            subject.OnNext(4);
        }

        #endregion

        #endregion
    }
}
