using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Microsoft.Ajax.Utilities;
using Shop.Infrastructure.Customer;
using Shop.Infrastructure.Interfaces;
using Shop.Infrastructure.Product;
using Shop.Infrastructure.Product.TableCreator;
using Shop.Infrastructure.Repository;
using Shop.Infrastructure.TableCreator;
using Shop.Infrastructure.TableCreator.Column;
using Shop.Order;
using Shop.Shared.Models.CommandHandler;

namespace Shop
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AreaRegistration.RegisterAllAreas();

            //// I want to REMOVE the ASP.NET ViewEngine...
            ViewEngines.Engines.Clear();
            //// and then add my own :)
            ViewEngines.Engines.Add(new CustomViewLocationRazorViewEngine());

            var container = InitiateAutofacContainerBuilder();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }

        public class CustomViewLocationRazorViewEngine : RazorViewEngine
        {
            public CustomViewLocationRazorViewEngine()
            {
                ViewLocationFormats = new[]
                {
                    "~/Pages/{1}/{0}.cshtml", "~/Pages/Management/{1}/{0}.cshtml", "~/Shared/Views/{0}.cshtml"
                };
                PartialViewLocationFormats = new[]
                {
                    "~/Pages/{1}/{0}.cshtml",
                    "~/Pages/Management/{1}/{0}.cshtml",
                    "~/Shared/Views/{0}.cshtml"
                };
                MasterLocationFormats = new[]
                {
                    "~/Pages/{1}/{0}.cshtml", "~/Pages/Management/{1}/{0}.cshtml", "~/Shared/Views/{0}.cshtml"
                };
            }
        }

        public IContainer InitiateAutofacContainerBuilder()
        {
            var builder = new ContainerBuilder();

            // Register your MVC controllers. (MvcApplication is the name of
            // the class in Global.asax.)
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // OPTIONAL: Register model binders that require DI.
            builder.RegisterModelBinders(typeof(MvcApplication).Assembly);
            builder.RegisterModelBinderProvider();

            // OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();

            // OPTIONAL: Enable property injection in view pages.
            builder.RegisterSource(new ViewRegistrationSource());

            // OPTIONAL: Enable property injection into action filters.
            builder.RegisterFilterProvider();

            // OPTIONAL: Enable action method parameter injection (RARE).
            //builder.InjectActionInvoker();

            // Set the dependency resolver to be Autofac.
            RegisterAutofacContainers(builder);

            var container = builder.Build();
            return container;
        }

        public void RegisterAutofacContainers(ContainerBuilder builder)
        {
            var repositoryAssembly = Assembly.GetAssembly(typeof(IRepository<>));

            //builder.RegisterAssemblyTypes(repositoryAssembly)
            //    .As(type => type.GetInterfaces()
            //        .Where(interfaceType => interfaceType.IsClosedTypeOf(typeof(IRepository<>)))
            //        .Select(interfaceType => new KeyedService("repository", interfaceType)))
            //    .InstancePerLifetimeScope();

            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            allAssemblies.ForEach(assembly =>
            {
                builder.RegisterAssemblyTypes(assembly)
                    .AsClosedTypesOf(typeof(IRepository<>))
                    .InstancePerDependency();
            });

            // Need to find the generic implementation of this.
            //builder.RegisterType<CustomerRepository>().As(typeof(IRepository<Customer>));
            //builder.RegisterType<OrderRepository>().As(typeof(IRepository<Order.Order>));
            //builder.RegisterType<ProductRepository>().As(typeof(IRepository<Product>));


            // External services mapping
            // Map shop.infrastructure.service
            builder.RegisterType<ProductService>().As<IProductService>();
            builder.RegisterType<CustomerService>().As<ICustomerService>();

            // Map Shop.Order
            builder.RegisterType<OrderService>().As<IOrderService>();

            _registerCommandHandlers(builder);
            _registerTableCreator(builder);
        }

        public void _registerCommandHandlers(ContainerBuilder builder)
        {
            builder.RegisterType<CommandHandlerFactory>().As<ICommandHandlerFactory>();

            // Register the open generic with a name so the
            // decorator can use it.
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .As(type => type.GetInterfaces()
                    .Where(interfaceType => interfaceType.IsClosedTypeOf(typeof(ICommandHandler<,>)))
                    .Select(interfaceType => new KeyedService("commandHandler", interfaceType)))
                .InstancePerLifetimeScope();

            // Register the generic decorator so it can wrap
            // the resolved named generics.
            builder.RegisterGenericDecorator(
                typeof(CommandHandlerLoggerDecorator<,>),
                typeof(ICommandHandler<,>),
                fromKey: "commandHandler");
        }

        private static void _registerTableCreator(ContainerBuilder builder)
        {
            builder.RegisterType<ProductAutoColumnRepository>().As(typeof(ITableColumnRepository<Product>));
            builder.RegisterType<ProductTableBuilder>().As(typeof(ITableBuilder<Product>));
        }

        private static IEnumerable<Type> FindDerivedTypes(Assembly assembly, Type baseType)
        {
            return assembly.GetTypes().Where(t => baseType.IsAssignableFrom(t));
        }

        private static IEnumerable<Type> FindSubClassesOf(Type baseType)
        {
            var assembly = baseType.Assembly;
            return assembly.GetTypes().Where(t => t.IsSubclassOf(baseType));
        }

        public static IEnumerable<Type> FindDerivedTypesFromAssembly(
            Assembly assembly, 
            Type baseType, bool classOnly)
        {
            // get all the types
            var types = assembly.GetTypes();

            // works out the derived types
            foreach (var type in types)
            {
                // if classOnly, it must be a class
                // useful when you want to create instance
                if (classOnly && !type.IsClass)
                    continue;

                if (type.IsAbstract)
                    continue;

                if (baseType.IsInterface)
                {
                    var it = type.GetInterface(baseType.FullName);

                    if (it != null)
                        // add it to result list
                        yield return type;
                }
                else if (type.IsSubclassOf(baseType))
                {
                    // add it to result list
                    yield return type;
                }
            }
        }
    }
}