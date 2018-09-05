using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Shop.Infrastructure.Product;
using Shop.Infrastructure.Product.TableCreator;
using Shop.Infrastructure.Repository;
using Shop.Infrastructure.TableCreator;
using Shop.Shared.Models.CommandHandler;
using System;
using System.Linq;
using System.Reflection;

namespace Shop
{
    public class AutofacConfig
    {
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

            _registerAutofacContainers(builder);
            var container = builder.Build();
            return container;
        }

        private void _registerAutofacContainers(ContainerBuilder builder)
        {
            // this is all current assemblies
            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            builder.RegisterAssemblyTypes(allAssemblies)
                .AsClosedTypesOf(typeof(IRepository<>))
                .SingleInstance();

            // Registering Services by convention
            builder.RegisterAssemblyTypes(allAssemblies)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .SingleInstance();

            _registerCommandHandlers(builder);
            _registerTableCreator(builder);
        }

        private void _registerCommandHandlers(ContainerBuilder builder)
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

        private void _registerTableCreator(ContainerBuilder builder)
        {
            builder.RegisterType<ProductAutoColumnRepository>().As(typeof(ITableColumnRepository<Product>));
            builder.RegisterType<ProductTableBuilder>().As(typeof(ITableBuilder<Product>));
        }
    }
}