using System;
using System.Windows;

using AutoMapper;

using LedgerClient.ECL;
using LedgerClient.ECL.DTO;
using LedgerClient.ECL.Interfaces;
using LedgerClient.History.ViewModels;
using LedgerClient.Interfaces;
using LedgerClient.Models;
using LedgerClient.ViewModels;

using LedgerLib;
using LedgerLib.Entities;
using LedgerLib.HistoryDAL;
using LedgerLib.Infrastructure;
using LedgerLib.Interfaces;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LedgerClient.Infrastructure
{
    //
    // This class is the funnel through which all Dependency Injection flows
    //

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

        private void InitializeHistoryDAL(IServiceCollection services)
        {
            services.AddTransient<IAccountHistoryDAL, AccountHistoryDAL>();
            services.AddTransient<IAccountNumberHistoryDAL, AccountNumberHistoryDAL>();
            services.AddTransient<IAccountTypeHistoryDAL, AccountTypeHistoryDAL>();
            services.AddTransient<IAllotmentHistoryDAL, AllotmentHistoryDAL>();
            services.AddTransient<IPayeeHistoryDAL, PayeeHistoryDAL>();
            services.AddTransient<IPoolHistoryDAL, PoolHistoryDAL>();
            services.AddTransient<ITransactionHistoryDAL, TransactionHistoryDAL>();
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
            services.AddTransient<AboutViewModel>();
            services.AddTransient<AccountNumberViewModel>();
            services.AddTransient<AccountViewModel>();
            services.AddTransient<AccountTypeViewModel>();
            services.AddTransient<AllotmentViewModel>();
            services.AddTransient<BackupViewModel>();
            services.AddTransient<CompanyViewModel>();
            services.AddTransient<DateViewModel>();
            services.AddTransient<ExplorerViewModel>();
            services.AddTransient<IdentityViewModel>();
            services.AddSingleton<MainViewModel>();
            services.AddTransient<NumberHistoryViewModel>();
            services.AddTransient<PalletteViewModel>();
            services.AddTransient<PasswordViewModel>();
            services.AddTransient<PoolViewModel>();
            services.AddTransient<PopupViewModel>();
            services.AddTransient<QAViewModel>();
            services.AddSingleton<StatusbarViewModel>();
            services.AddTransient<TransactionViewModel>();
        }

        private void InitializeHistoryViewModels(ServiceCollection services)
        {
            services.AddTransient<HistoryViewModel>();
            services.AddTransient<Ledger5PasswordViewModel>();
        }

        #endregion

        #region Properties

        public IConfiguration Configuration { get => _provider.GetRequiredService<IConfiguration>(); }
        public IExplorerService ExplorerService { get => _provider.GetRequiredService<IExplorerService>(); }
        public IPasswordManager PasswordManager { get => _provider.GetRequiredService<IPasswordManager>(); }
        public IPoolRecalculator PoolRecalculator { get => _provider.GetRequiredService<IPoolRecalculator>(); }
        public IServiceProvider Provider { get => _provider; }
        public ISettingsService Settings { get => _provider.GetRequiredService<ISettingsService>(); }
        public IStringCypherService StringCypher { get => _provider.GetRequiredService<IStringCypherService>(); }
        public LedgerContext LedgerContext { get => _provider.GetRequiredService<LedgerContext>(); }

        #region ViewModels

        public AboutViewModel AboutViewModel { get => _provider.GetRequiredService<AboutViewModel>(); }
        public AccountNumberViewModel AccountNumberViewModel { get => _provider.GetRequiredService<AccountNumberViewModel>(); }
        public AccountTypeViewModel AccountTypeViewModel { get => _provider.GetRequiredService<AccountTypeViewModel>(); }
        public AccountViewModel AccountViewModel { get => _provider.GetRequiredService<AccountViewModel>(); }
        public AllotmentViewModel AllotmentViewModel { get => _provider.GetRequiredService<AllotmentViewModel>(); }
        public BackupViewModel BackupViewModel { get => _provider.GetRequiredService<BackupViewModel>(); }
        public CompanyViewModel CompanyViewModel { get => _provider.GetRequiredService<CompanyViewModel>(); }
        public DateViewModel DateViewModel { get => _provider.GetRequiredService<DateViewModel>(); }
        public ExplorerViewModel ExplorerViewModel { get => _provider.GetRequiredService<ExplorerViewModel>(); }
        public IdentityViewModel IdentityViewModel { get => _provider.GetRequiredService<IdentityViewModel>(); }
        public MainViewModel MainViewModel { get => _provider.GetRequiredService<MainViewModel>(); }
        public NumberHistoryViewModel NumberHistoryViewModel { get => _provider.GetRequiredService<NumberHistoryViewModel>(); }
        public PalletteViewModel PalletteViewModel { get => _provider.GetRequiredService<PalletteViewModel>(); }
        public PasswordViewModel PasswordViewModel { get => _provider.GetRequiredService<PasswordViewModel>(); }
        public PoolViewModel PoolViewModel { get => _provider.GetRequiredService<PoolViewModel>(); }
        public PopupViewModel PopupViewModel { get => _provider.GetRequiredService<PopupViewModel>(); }
        public QAViewModel QAViewModel { get => _provider.GetRequiredService<QAViewModel>(); }
        public StatusbarViewModel StatusbarViewModel { get => _provider.GetRequiredService<StatusbarViewModel>(); }
        public TransactionViewModel TransactionViewModel { get => _provider.GetRequiredService<TransactionViewModel>(); }

        #endregion

        #region History View Models

        public HistoryViewModel HistoryViewModel { get => _provider.GetRequiredService<HistoryViewModel>(); }
        public Ledger5PasswordViewModel Ledger5PasswordViewModel { get => _provider.GetRequiredService<Ledger5PasswordViewModel>(); }

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

        #region History DALs

        public IAccountHistoryDAL AccountHistoryDAL { get => _provider.GetRequiredService<IAccountHistoryDAL>(); }
        public IAccountNumberHistoryDAL AccountNumberHistoryDAL { get => _provider.GetRequiredService<IAccountNumberHistoryDAL>(); }
        public IAccountTypeHistoryDAL AccountTypeHistoryDAL { get => _provider.GetRequiredService<IAccountTypeHistoryDAL>(); }
        public IAllotmentHistoryDAL AllotmentHistoryDAL { get => _provider.GetRequiredService<IAllotmentHistoryDAL>(); }
        public IPayeeHistoryDAL PayeeHistoryDAL { get => _provider.GetRequiredService<IPayeeHistoryDAL>(); }
        public IPoolHistoryDAL PoolHistoryDAL { get => _provider.GetRequiredService<IPoolHistoryDAL>(); }
        public ITransactionHistoryDAL TransactionHistoryDAL { get => _provider.GetRequiredService<ITransactionHistoryDAL>(); }

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
                    services.AddDbContext<HistoryContext>(ServiceLifetime.Transient);

                    services.AddSingleton(ConfigurationFactory.Create());
                    services.AddTransient<IExplorerService, ExplorerService>();
                    services.AddSingleton<IPasswordManager, PasswordManager>();
                    services.AddTransient<IPoolRecalculator, PoolRecalculator>();
                    services.AddSingleton<ISettingsService, SettingsService>();
                    services.AddTransient<IStringCypherService, StringCypherService>();

                    InitializeMapper(services);
                    InitializeDAL(services);
                    InitializeHistoryDAL(services);
                    InitializeECL(services);
                    InitializeViewModels(services);
                    InitializeHistoryViewModels(services);

                    _provider = services.BuildServiceProvider();

                    Application.Current.Resources[Constants.Locator] = this;

                    _initialized = true;
                }
            }
        }
    }
}
