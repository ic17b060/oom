using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using static System.Console;

namespace Task6
{
    public static class VideoGamePush
    {
        public static void Run()
        {
            var source = new Subject<int>();

            source
                .Sample(TimeSpan.FromSeconds(2.0))
                .Subscribe(x => WriteLine($"received {x}"))
                ;
            var t = new Thread(() =>
            {
                var i = 0;
                while (true)
                {
                    Thread.Sleep(250);
                    source.OnNext(i);
                    WriteLine($"sent {i}");
                    i++;
                }
            });
            t.Start();
        }
    }
}
