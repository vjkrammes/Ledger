using System;
using System.Windows;

using AutoMapper;

using LedgerClient.ECL;
using LedgerClient.ECL.DTO;
using LedgerClient.ECL.Interfaces;
using LedgerClient.Interfaces;
using LedgerClient.Models;
using LedgerClient.ViewModels;
using LedgerLib;
using LedgerLib.Entities;
using LedgerLib.Infrastructure;
using LedgerLib.Interfaces;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LedgerClient.Infrastructure
{
    public class Locator
    {
        private readonly ServiceProvider _provider;
        private static bool _initialized = false;
        private static readonly object _lockObject = new object();

        #region Initializers

        private void InitializeMapper(IServiceCollection services)
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AccountEntity, Account>().ReverseMap();
                cfg.CreateMap<AccountNumberEntity, AccountNumber>().ReverseMap();
                cfg.CreateMap<AccountTypeEntity, AccountType>().ReverseMap();
                cfg.CreateMap<AllotmentEntity, Allotment>().ReverseMap();
                cfg.CreateMap<CompanyEntity, Company>().ReverseMap();
                cfg.CreateMap<IdentityEntity, Identity>().ReverseMap();
                cfg.CreateMap<PoolEntity, Pool>().ReverseMap();
                cfg.CreateMap<TransactionEntity, Transaction>().ReverseMap();
            });
            Mapper mapper = new Mapper(config);
            services.AddSingleton<IMapper>(mapper);
        }

        private void InitializeDAL(IServiceCollection services)
        {
            services.AddTransient<IAccountDAL, AccountDAL>();
            services.AddTransient<IAccountNumberDAL, AccountNumberDAL>();
            services.AddTransient<IAccountTypeDAL, AccountTypeDAL>();
            services.AddTransient<IAllotmentDAL, AllotmentDAL>();
            services.AddTransient<ICompanyDAL, CompanyDAL>();
            services.AddTransient<IIdentityDAL, IdentityDAL>();
            services.AddTransient<IPoolDAL, PoolDAL>();
            services.AddTransient<ISettingsDAL, SettingsDAL>();
            services.AddTransient<ITransactionDAL, TransactionDAL>();
        }

        private void InitializeECL(IServiceCollection services)
        {
            services.AddTransient<IAccountECL, AccountECL>();
            services.AddTransient<IAccountNumberECL, AccountNumberECL>();
            services.AddTransient<IAccountTypeECL, AccountTypeECL>();
            services.AddTransient<IAllotmentECL, AllotmentECL>();
            services.AddTransient<ICompanyECL, CompanyECL>();
            services.AddTransient<IIdentityECL, IdentityECL>();
            services.AddTransient<IPoolECL, PoolECL>();
            services.AddTransient<ITransactionECL, TransactionECL>();
        }

        private void InitializeViewModels(IServiceCollection services)
        {
            services.AddTransient<AccountViewModel>();
            services.AddTransient<AccountTypeViewModel>();
            services.AddTransient<AllotmentViewModel>();
            services.AddTransient<CompanyViewModel>();
            services.AddTransient<HistoryViewModel>();
            services.AddTransient<IdentityViewModel>();
            services.AddSingleton<MainViewModel>();
            services.AddTransient<PasswordViewModel>();
            services.AddTransient<PoolViewModel>();
            services.AddTransient<PopupViewModel>();
            services.AddTransient<QAViewModel>();
            services.AddSingleton<StatusbarViewModel>();
        }

        #endregion

        #region Properties

        public IConfiguration Configuration { get => _provider.GetRequiredService<IConfiguration>(); }
        public IPasswordManager PasswordManager { get => _provider.GetRequiredService<IPasswordManager>(); }
        public IPoolRecalculator PoolRecalculator { get => _provider.GetRequiredService<IPoolRecalculator>(); }
        public IServiceProvider Provider { get => _provider; }
        public ISettingsService Settings { get => _provider.GetRequiredService<ISettingsService>(); }
        public IStringCypherService StringCypher { get => _provider.GetRequiredService<IStringCypherService>(); }
        public LedgerContext LedgerContext { get => _provider.GetRequiredService<LedgerContext>(); }

        #region ViewModels

        public AccountTypeViewModel AccountTypeViewModel { get => _provider.GetRequiredService<AccountTypeViewModel>(); }
        public AccountViewModel AccountViewModel { get => _provider.GetRequiredService<AccountViewModel>(); }
        public AllotmentViewModel AllotmentViewModel { get => _provider.GetRequiredService<AllotmentViewModel>(); }
        public CompanyViewModel CompanyViewModel { get => _provider.GetRequiredService<CompanyViewModel>(); }
        public HistoryViewModel HistoryViewModel { get => _provider.GetRequiredService<HistoryViewModel>(); }
        public IdentityViewModel IdentityViewModel { get => _provider.GetRequiredService<IdentityViewModel>(); }
        public MainViewModel MainViewModel { get => _provider.GetRequiredService<MainViewModel>(); }
        public PasswordViewModel PasswordViewModel { get => _provider.GetRequiredService<PasswordViewModel>(); }
        public PoolViewModel PoolViewModel { get => _provider.GetRequiredService<PoolViewModel>(); }
        public PopupViewModel PopupViewModel { get => _provider.GetRequiredService<PopupViewModel>(); }
        public QAViewModel QAViewModel { get => _provider.GetRequiredService<QAViewModel>(); }
        public StatusbarViewModel StatusbarViewModel { get => _provider.GetRequiredService<StatusbarViewModel>(); }

        #endregion

        #region DALs (Data Access Layer, interface to/from Entity Framework for database access)

        public IAccountDAL AccountDAL { get => _provider.GetRequiredService<IAccountDAL>(); }
        public IAccountNumberDAL AccountNumberDAL { get => _provider.GetRequiredService<IAccountNumberDAL>(); }
        public IAccountTypeDAL AccountTypeDAL { get => _provider.GetRequiredService<IAccountTypeDAL>(); }
        public IAllotmentDAL AllotmentDAL { get => _provider.GetRequiredService<IAllotmentDAL>(); }
        public ICompanyDAL CompanyDAL { get => _provider.GetRequiredService<ICompanyDAL>(); }
        public IIdentityDAL IdentityDAL { get => _provider.GetRequiredService<IIdentityDAL>(); }
        public IPoolDAL PoolDAL { get => _provider.GetRequiredService<IPoolDAL>(); }
        public ISettingsDAL SettingsDAL { get => _provider.GetRequiredService<ISettingsDAL>(); }
        public ITransactionDAL TransactionDAL { get => _provider.GetRequiredService<ITransactionDAL>(); }

        #endregion

        #region ECLs (Entity Conversion Layer, translates between Entity objects and Observable Data Transfer Objects / Models)

        public IAccountECL AccountECL { get => _provider.GetRequiredService<IAccountECL>(); }
        public IAccountNumberECL AccountNumberECL { get => _provider.GetRequiredService<IAccountNumberECL>(); }
        public IAccountTypeECL AccountTypeECL { get => _provider.GetRequiredService<IAccountTypeECL>(); }
        public IAllotmentECL AllotmentECL { get => _provider.GetRequiredService<IAllotmentECL>(); }
        public ICompanyECL CompanyECL { get => _provider.GetRequiredService<ICompanyECL>(); }
        public IIdentityECL IdentityECL { get => _provider.GetRequiredService<IIdentityECL>(); }
        public IPoolECL PoolECL { get => _provider.GetRequiredService<IPoolECL>(); }
        public ITransactionECL TransactionECL { get => _provider.GetRequiredService<ITransactionECL>(); }

        #endregion

        #endregion

        public Locator()
        {
            lock(_lockObject)
            {
                if (!_initialized)
                {
                    ServiceCollection services = new ServiceCollection();
                    services.AddDbContext<LedgerContext>(ServiceLifetime.Transient);
                    services.AddSingleton<ISettingsService, SettingsService>();
                    services.AddSingleton(ConfigurationFactory.Create());
                    services.AddSingleton<IPasswordManager, PasswordManager>();
                    services.AddTransient<IStringCypherService, StringCypherService>();
                    services.AddTransient<IPoolRecalculator, PoolRecalculator>();
                    InitializeMapper(services);
                    InitializeDAL(services);
                    InitializeECL(services);
                    InitializeViewModels(services);
                    _provider = services.BuildServiceProvider();
                    Application.Current.Resources[Constants.Locator] = this;
                    _initialized = true;
                }
            }
        }
    }
}
