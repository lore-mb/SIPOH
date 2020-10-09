[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(PoderJudicial.SIPOH.WebApp.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(PoderJudicial.SIPOH.WebApp.App_Start.NinjectWebCommon), "Stop")]

namespace PoderJudicial.SIPOH.WebApp.App_Start
{
    using System;
    using System.Web;
    using AutoMapper;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using PoderJudicial.SIPOH.AccesoDatos;
    using PoderJudicial.SIPOH.AccesoDatos.Conexion;
    using PoderJudicial.SIPOH.AccesoDatos.Interfaces;
    using PoderJudicial.SIPOH.Entidades;
    using PoderJudicial.SIPOH.Negocio;
    using PoderJudicial.SIPOH.Negocio.Interfaces;
    using PoderJudicial.SIPOH.WebApp.Models;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            ServerConnection connection = ServerConnection.GetConnection();
           
            //Inyeccion de servicios para repositorios de conexion de datos
            kernel.Bind<ICuentaRepository>().To<CuentaRepository>().WithConstructorArgument("connection", connection);
            kernel.Bind<ICatalogosRepository>().To<CatalogosRepository>().WithConstructorArgument("connection", connection);
            kernel.Bind<IExpedienteRepository>().To<ExpedienteRepository>().WithConstructorArgument("connection", connection);
            kernel.Bind<IEjecucionRepository>().To<EjecucionRepository>().WithConstructorArgument("connection", connection);
            
            //Inyeccion de servicios para la logica de negocio
            kernel.Bind<ICuentaProcessor>().To<CuentaProcessor>();
            kernel.Bind<IInicialesProcessor>().To<InicialesProcessor>();
            kernel.Bind<IPromocionesProcessor>().To<PromocionesProcessor>();
            kernel.Bind<IBusquedasProcessor>().To<BusquedasProcessor>();
    
            //Mapers *********************************************************
            var mapperConfiguration = CreateConfiguration();
            kernel.Bind<MapperConfiguration>().ToConstant(mapperConfiguration).InSingletonScope();
            kernel.Bind<IMapper>().ToMethod(ctx => new Mapper(mapperConfiguration, type => ctx.Kernel.Get(type)));

        }

        private static MapperConfiguration CreateConfiguration()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Ejecucion, BeneficiarioDTO>();
                cfg.CreateMap<EjecucionModelView, Ejecucion>().ReverseMap();
                cfg.CreateMap<TocasModelView, Expediente>().ReverseMap();
                cfg.CreateMap<AnexosModelView, Anexo>().ReverseMap();
                cfg.CreateMap<Expediente, CausasModelView>();
            });
            return config;
        }
    }
}
