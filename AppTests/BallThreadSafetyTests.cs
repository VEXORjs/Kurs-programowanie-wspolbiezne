using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Data;
using Xunit;

namespace AppTests
{
    public class BallThreadSafetyTests
    {
        [Fact]
        public void Ball_Should_Be_Thread_Safe_Under_Concurrent_Access()
        {
            var ball = new Ball
            {
                Radius = 10,
                Mass = 1
            };

            ball.SetPosition(0, 0);
            ball.ApplyVelocity(1, 1);

            Parallel.For(0, 5000, _ =>
            {
                ball.Move(0.01);
                var state = ball.GetState();

                Assert.True(double.IsFinite(state.X));
                Assert.True(double.IsFinite(state.Y));
            });
        }

        [Fact]
        public void Ball_State_Should_Be_Consistent_Between_Calls()
        {
            var ball = new Ball();
            ball.SetPosition(10, 20);
            ball.ApplyVelocity(5, 5);

            var s1 = ball.GetState();
            var s2 = ball.GetState();

            Assert.Equal(s1.X, s2.X);
            Assert.Equal(s1.Y, s2.Y);
            Assert.Equal(s1.VX, s2.VX);
            Assert.Equal(s1.VY, s2.VY);
        }
    }
}