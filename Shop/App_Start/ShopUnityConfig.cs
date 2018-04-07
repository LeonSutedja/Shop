using Shop.Infrastructure.Customer;
using Shop.Infrastructure.Interfaces;
using Shop.Infrastructure.Product;
using Shop.Infrastructure.Repository;
using Shop.Infrastructure.TableCreator;
using Shop.Infrastructure.TableCreator.Builder;
using Shop.Infrastructure.TableCreator.Column;
using Shop.Order;
using Shop.Shared.Models.CommandHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity;
using Unity.Lifetime;
using Unity.RegistrationByConvention;

namespace Shop
{
    public static class ShopUnityConfig
    {
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterTypes(AllClasses.FromLoadedAssemblies()
                    .Where(t => t.GetInterfaces().Any(i => i.IsGenericType &&
                                                           i.GetGenericTypeDefinition() == typeof(IRepository<>))),
                WithMappings.FromAllInterfaces,
                WithName.Default, WithLifetime.ContainerControlled);

            // External services mapping
            // Map shop.infrastructure.service
            container.RegisterType<IProductService, ProductService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICustomerService, CustomerService>(new ContainerControlledLifetimeManager());

            // Map Shop.Order
            container.RegisterType<IOrderService, OrderService>(new ContainerControlledLifetimeManager());

            _registerCommandHandlers(container);
            _registerTableCreator(container);
        }

        private static void _registerTableCreator(IUnityContainer container)
        {
            // TableCreator
            container.RegisterTypes(AllClasses.FromLoadedAssemblies()
                    .Where(t => t.GetInterfaces().Any(i => i.IsGenericType &&
                                                           i.GetGenericTypeDefinition() == typeof(ITableColumnRepository<>))),
                WithMappings.FromAllInterfaces,
                WithName.Default, WithLifetime.PerThread);

            var tColumnTypes = AllClasses.FromLoadedAssemblies()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType &&
                                                       i.GetGenericTypeDefinition() == typeof(ITableColumn<>))).ToList();

            container.RegisterTypes(tColumnTypes,
                WithMappings.FromAllInterfaces,
                WithName.TypeName, WithLifetime.PerThread);

            var ienumerable = typeof(IEnumerable<>);
            var itableColumnType = typeof(ITableColumn<Product>);
            Type[] typeArgs = { itableColumnType };
            ienumerable.MakeGenericType(typeArgs);

            container.RegisterType(typeof(IEnumerable<ITableColumn>), typeof(ITableColumn[]));

            container.RegisterTypes(AllClasses.FromLoadedAssemblies()
                    .Where(t => t.GetInterfaces().Any(i => i.IsGenericType &&
                                                           i.GetGenericTypeDefinition() == typeof(ITableBuilder<>))),
                WithMappings.FromAllInterfaces,
                WithName.Default, WithLifetime.PerThread);
        }

        private static void _registerCommandHandlers(IUnityContainer container)
        {
            // Handlers
            container.RegisterType<ICommandHandlerFactory, CommandHandlerFactory>(new ContainerControlledLifetimeManager());
            container.RegisterTypes(AllClasses.FromLoadedAssemblies()
                    .Where(t => t.GetInterfaces().Any(i => i.IsGenericType &&
                                                           i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>))), WithMappings.FromAllInterfaces,
                WithName.Default, WithLifetime.ContainerControlled);
        }
    }
}