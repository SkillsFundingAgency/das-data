public class ServiceLocator
{
    private IDictionary<Type, object> _services;

    public ServiceLocator()
    {
        this._services = new Dictionary<Type, object>();
    }

    public void AddService(Type type, object obj)
    {
        this._services.Add(type, obj);
    }

    public IMyServiceClass GetService()
    {
        if (!this._services.ContainsKey(typeof(IMyServiceClass)))
        {
            this._services.Add(typeof(IMyServiceClass), null);
        }

        var service = this._services[typeof(IMyServiceClass)] as IMyServiceClass;
        if (service == null)
        {
            service = new MyServiceClass();
            this._services[typeof(IMyServiceClass)] = service;
        }

        return service;
    }

    public MyStatusClass GetStatus()
    {
        if (!this._services.ContainsKey(typeof(MyStatusClass)))
        {
            this._services.Add(typeof(MyStatusClass), null);
        }

        var status = this._services[typeof(MyStatusClass)] as MyStatusClass;
        if (status == null)
        {
            status = new MyStatusClass();
            this._services[typeof(MyStatusClass)] = status;
        }

        return status;
    }
}

public interface IMyServiceClass
{
    void Process(HttpRequestMessage req);
}

public class MyServiceClass : IMyServiceClass
{
    public void Process(HttpRequestMessage req)
    {
        return;
    }
}

public class MyStatusClass
{
    public string Status { get; set; }
}
