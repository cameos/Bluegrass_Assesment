using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using Ninject;
using Ninject.Extensions.ChildKernel;
using System.Configuration;
using System.Web.Routing;
using DataAccess.Entities;
using Abstraction.Abstract;
using Abstraction.Concrete;

namespace Blue_API.Custom
{
    public class CustomDependency : IDependencyResolver
    {
        private IKernel kernel;

        public CustomDependency() : this(new StandardKernel()) { }

        public CustomDependency(IKernel ninjectKernel, bool scope = false)
        {
            kernel = ninjectKernel;
            if (!scope)
                AddBindings(kernel);

        }

        public IDependencyScope BeginScope()
        {
            return new CustomDependency(AddRequestBindings(new ChildKernel(kernel)), true);
        }

        public void Dispose()
        {

        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }


        public void AddBindings(IKernel kernel)
        {
            kernel.Bind<ICountry>().To<CountryConcrete>();
            kernel.Bind<IProvince>().To<ProvinceConcrete>();
            kernel.Bind<ICity>().To<CityConcrete>();
            kernel.Bind<IAddress>().To<AddressConcrete>();
            kernel.Bind<IUser>().To<UserConcrete>();
            kernel.Bind<IAdmin>().To<AdminConcrete>();
        }

        private IKernel AddRequestBindings(IKernel kernel)
        {
            kernel.Bind<ICountry>().To<CountryConcrete>();
            kernel.Bind<IProvince>().To<ProvinceConcrete>();
            kernel.Bind<ICity>().To<CityConcrete>();
            kernel.Bind<IAddress>().To<AddressConcrete>();
            kernel.Bind<IUser>().To<UserConcrete>();
            kernel.Bind<IAdmin>().To<AdminConcrete>();

            return kernel;
        }
    }
}