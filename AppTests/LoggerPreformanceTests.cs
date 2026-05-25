using System;
using System.Collections.Generic;
using System.Diagnostics;
using App.Data;
using App.Logic;
using Xunit;

namespace AppTests
{
    public class LoggerPerformanceTests
    {
        private class NoOpLogger : ILogger
        {
            public void Log(string message) { }
        }

        private class FakeBallRepository : IBallRepository
        {
            public IReadOnlyList<IBall> GetInitialBalls(int count, double width, double height)
            {
                var list = new List<IBall>();

                for (int i = 0; i < count; i++)
                {
                    var b = new Ball
                    {
                        Radius = 10,
                        Mass = 1
                    };

                    b.SetPosition(i * 20, i * 20);
                    b.ApplyVelocity(10, 10);

                    list.Add(b);
                }

                return list;
            }
        }

        [Fact]
        public void Logger_Should_Not_Significantly_Affect_Performance()
        {
            var repo = new FakeBallRepository();

            var withLogger = new BallService(repo, new FileLogger("test_log.log"));
            var noLogger = new BallService(repo, new NoOpLogger());

            var balls1 = repo.GetInitialBalls(50, 500, 500);
            var balls2 = repo.GetInitialBalls(50, 500, 500);

            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 200; i++)
                withLogger.UpdatePositions(balls1, 0.016, 500, 500);
            sw.Stop();
            var t1 = sw.ElapsedMilliseconds;

            sw.Restart();
            for (int i = 0; i < 200; i++)
                noLogger.UpdatePositions(balls2, 0.016, 500, 500);
            sw.Stop();
            var t2 = sw.ElapsedMilliseconds;

            Assert.True(t2 <= t1 * 1.5);
        }
    }
}