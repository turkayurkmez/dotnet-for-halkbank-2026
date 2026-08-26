namespace DependencyInjectionLifeCyycle.Services
{
    public interface IGuidGenerator
    {
        Guid Guid { get; set; }
    }

    public interface ISingleton : IGuidGenerator { }
    public interface ITransient : IGuidGenerator { }
    public interface IScoped : IGuidGenerator { }

    public class Singleton : ISingleton
    {
        public Guid Guid { get; set; }

        public Singleton()
        {
             Guid = Guid.NewGuid();   
        }
    }

    public class Transient : ITransient
    {
        public Guid Guid { get; set; }
        public Transient()
        {
            Guid = Guid.NewGuid();
        }
    }

    public class Scoped : IScoped
    {
        public Guid Guid { get; set; }
        public Scoped()
        {
            Guid = Guid.NewGuid();
        }
    }


    public class GuidService
    {
        private readonly ISingleton _singleton;
        private readonly ITransient _transient;
        private readonly IScoped _scoped;

        public GuidService(ISingleton singleton, ITransient transient, IScoped scoped)
        {
            _singleton = singleton;
            _transient = transient;
            _scoped = scoped;
        }

        public ISingleton Singleton { get=> _singleton; }
        public IScoped Scoped { get=> _scoped; }
        public ITransient Transient { get=> _transient; }
    } 
}
