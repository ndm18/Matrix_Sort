using System.Threading;
using NUnit.Framework;
using Moq;
using observer;

namespace observer_tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void NotifyAll_ShouldExecuteRunMethodOfAllSubscribers()
        {
            var subscriberMock1 = new Mock<ISubscriber>();
            var subscriberMock2 = new Mock<ISubscriber>();
            var countdown = new Countdown("Test");
            
            countdown.Subscribe(subscriberMock1.Object);
            countdown.Subscribe(subscriberMock2.Object);
            
            countdown.NotifyAll();
            
            subscriberMock1.Verify(mock => mock.Run(countdown));
            subscriberMock2.Verify(mock => mock.Run(countdown));
        }
        
        [Test]
        public void Subscriber_ShouldUnsubscribeIfTimeIsExpired()
        {
            var subscriber = new Subscriber("Test subscriber", 1);
            var countdown = new Countdown("Test");
            
            countdown.Subscribe(subscriber);

            Thread.Sleep(2000);
            countdown.NotifyAll();

            Assert.False(countdown.Unsubscribe(subscriber)); 
        }
        
        
        [Test]
        public void Subscriber_ShouldNotUnsubscribeIfTimeIsNotExpired()
        {
            var subscriber = new Subscriber("Test subscriber", 5);
            var countdown = new Countdown("Test");
            
            countdown.Subscribe(subscriber);

            Thread.Sleep(2000);
            countdown.NotifyAll();

            Assert.True(countdown.Unsubscribe(subscriber)); 
        }
    }
}