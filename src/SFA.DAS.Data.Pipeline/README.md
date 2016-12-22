# Data Pipeleine Helpers

This project exists to reduce the amound of boiler plate code needed to extrct data from DAS services and insert it into the Reporting Data Store. It should be a trivial task to add more workers where necessary refenrecing this library.

The helper in this project are a form of [monad](http://mikhail.io/2016/01/monads-explained-in-csharp/) sometimes used in functional programming. Monads are essentially a box that allows creation of operations that manipulate the boxes. In this case a pipleine of operations. These helpers are an extension modeled on [Railway Oriented Programming](http://fsharpforfunandprofit.com/rop/) that embodys the idea of a sucess and a falure track in the pipeline.

The core of the library is the abstract PipelineResult class, it has two derived classes Success and Failure that represent the two tracks that can be taken. Each each operation in the pipeline is expected to return one or the other. helper methods exist to create the response ```Result.Win``` and ```Result.Fail``` they also add a message to the log.

Rollback operations can also be added to the pipleine so that if any failures occure further down the pipeline the rollback methods will be called.

```
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
```

The example above shows the basic syntax that returns Success. the exmple below performs exactly the same operation but uses the ```Transform``` extension method to streamline the operation.

```
[Test]
public void SingleStageTransform()
{
    var log = new LogToList();
    var m = new TestMessage { Message = "bob" };

    var result = m.Return(log.Log)
        .Transform(x => new TestMessage { Message = "hello " + x.Message }, "said hello");

    Assert.IsInstanceOf<Success<TestMessage>>(result);
    Assert.IsTrue(result.IsSuccess());
    Assert.AreEqual("hello bob", result.Content.Message);
    Assert.AreEqual("Success: said hello", log.Messages.First());
}
```

Any exceptons thrown by operations in the pipeline will be interpreted as a failure but a failure can be returned directly as below.

```
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
```

The helpers to attatch a pipelein to a messsage expects a function to be passed in that returns a message task.

```
public async Task<Message<Test>> GetMessage()
{
    return await Task.Run(() => new TestMessage<Test>(new Test {Value = "bob"}));
}

[Test]
public void FetchMessage()
{
    var result = MessageQueue
        .WaitFor(GetMessage)
        .Step(m => Result.Win(new Test {Value = "hello " + m.Value}, "recived bob"));

    Assert.IsTrue(result.IsSuccess());
    Assert.AreEqual("hello bob", result.Content.Value);
}
```

This should fit with the existng messaging frameworks used by DAS.

Extension methods are also provided for storage, see the tests for more details.
