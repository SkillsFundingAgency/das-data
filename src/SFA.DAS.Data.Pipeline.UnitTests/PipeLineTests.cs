﻿using System;
using System.Linq;
using NUnit.Framework;

namespace SFA.DAS.Data.Pipeline.UnitTests
{
    [TestFixture]
    public class PipelineTests
    {
        public class TestMessage
        {
            public string Message { get; set; }
        }

        public class TestResult
        {
            public int Transformed { get; set; }
        }

        [Test]
        public void SingleStageSuccess()
        {
            var log = new LogToList();
            var m = new TestMessage { Message = "bob" };

            var result = m.Return(log.Log)
                .Step(x => Result.Win(
                   new TestMessage { Message = "hello " + x.Message },
                   "said hello"));

            Assert.IsInstanceOf<Success<TestMessage>>(result);
            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual("hello bob", result.Content.Message);
            Assert.AreEqual("Success: said hello", log.Messages.First());
        }

        [Test]
        public void SingleStageFailure()
        {
            var log = new LogToList();
            var m = new TestMessage { Message = "bob" };

            var result = m.Return(log.Log)
                .Step(x => Result.Fail<TestMessage>("it go bang"));

            Assert.IsInstanceOf<Failure<TestMessage>>(result);
            Assert.IsFalse(result.IsSuccess());
            Assert.IsNull(result.Content);
            Assert.AreEqual("Failure: it go bang", log.Messages.First());
        }

        [Test]
        public void SingleStageException()
        {
            var log = new LogToList();
            var m = new TestMessage { Message = "bob" };

            var result = m.Return(log.Log)
                .Step(x =>
                {
                    throw new Exception("big bang");
                    return Result.Win(
                        new TestMessage {Message = "hello " + x.Message},
                        "said hello");
                });

            Assert.IsInstanceOf<Failure<TestMessage>>(result);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual("Exception: big bang", log.Messages.First());
        }

        [Test]
        public void MultipleStageSuccess()
        {
            var m = new TestMessage { Message = "bob" };

            var result = m.Return()
                .Step(x => Result.Win(
                    new TestMessage {Message = "hello " + x.Message},
                    "said hello"))
                .Step(x =>
                {
                    var r = new TestResult {Transformed = x.Message.Length};
                    return Result.Win(r, "string to int");
                });

            Assert.IsTrue(result.IsSuccess());
        }

        [Test]
        public void FlowControll()
        {
            var m = new TestMessage { Message = "bob" };

            var result = m.Return()
                .Step(x => Result.Win(
                    new TestMessage { Message = "hello " + x.Message },
                    "said hello"))
                .Step<TestResult>(x =>
                {
                    if (x.Message.Length != 5)
                        return Result.Fail<TestResult>("not good");

                    var r = new TestResult {Transformed = x.Message.Length};
                    return Result.Win(r, "string to int");
                });

            Assert.IsFalse(result.IsSuccess());
        }

        [Test]
        public void EarlyFail()
        {
            var m = new TestMessage { Message = "bob" };

            bool beenRun = false;

            var result = m.Return()
                .Step(x => Result.Fail<TestMessage>("bang"))
                .Step(x =>
                {
                    beenRun = true;

                    var r = new TestResult { Transformed = x.Message.Length };
                    return Result.Win(r, "string to int");
                });

            Assert.IsFalse(result.IsSuccess());
            Assert.IsFalse(beenRun);
        }

        [Test]
        public void Rollback()
        {
            var m = new TestMessage { Message = "bob" };
            bool rolledback = false;

            var result = m.Return()
                .Step(x => Result.Win(
                    new TestMessage {Message = "hello " + x.Message},
                    "said hello"), () => { rolledback = true; })
                .Step(x => Result.Fail<TestMessage>("bang"));

            Assert.IsFalse(result.IsSuccess());
            Assert.IsTrue(rolledback);
        }
    }
}