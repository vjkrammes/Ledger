﻿using AutoMapper;

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

using System;
using System.Windows;

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

        private static void InitializeMapper(IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
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
            var mapper = new Mapper(config);
            services.AddSingleton<IMapper>(mapper);
        }

        private static void InitializeDAL(IServiceCollection services)
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

        private static void InitializeHistoryDAL(IServiceCollection services)
        {
            services.AddTransient<IAccountHistoryDAL, AccountHistoryDAL>();
            services.AddTransient<IAccountNumberHistoryDAL, AccountNumberHistoryDAL>();
            services.AddTransient<IAccountTypeHistoryDAL, AccountTypeHistoryDAL>();
            services.AddTransient<IAllotmentHistoryDAL, AllotmentHistoryDAL>();
            services.AddTransient<IPayeeHistoryDAL, PayeeHistoryDAL>();
            services.AddTransient<IPoolHistoryDAL, PoolHistoryDAL>();
            services.AddTransient<ITransactionHistoryDAL, TransactionHistoryDAL>();
        }

        private static void InitializeECL(IServiceCollection services)
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

        private static void InitializeViewModels(IServiceCollection services)
        {
            services.AddTransient<AboutViewModel>();
            services.AddTransient<AccountNumberViewModel>();
            services.AddTransient<AccountSummaryViewModel>();
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

        private static void InitializeHistoryViewModels(ServiceCollection services)
        {
            services.AddTransient<HistoryViewModel>();
            services.AddTransient<Ledger5PasswordViewModel>();
        }

        #endregion

        #region Properties

        public IConfiguration Configuration => _provider.GetRequiredService<IConfiguration>();
        public IExplorerService ExplorerService => _provider.GetRequiredService<IExplorerService>();
        public IPasswordManager PasswordManager => _provider.GetRequiredService<IPasswordManager>();
        public IPoolRecalculator PoolRecalculator => _provider.GetRequiredService<IPoolRecalculator>();
        public IServiceProvider Provider => _provider;
        public ISettingsService Settings => _provider.GetRequiredService<ISettingsService>();
        public IStringCypherService StringCypher => _provider.GetRequiredService<IStringCypherService>();
        public LedgerContext LedgerContext => _provider.GetRequiredService<LedgerContext>();

        #region ViewModels

        public AboutViewModel AboutViewModel => _provider.GetRequiredService<AboutViewModel>();
        public AccountNumberViewModel AccountNumberViewModel => _provider.GetRequiredService<AccountNumberViewModel>();
        public AccountSummaryViewModel AccountSummaryViewModel => _provider.GetRequiredService<AccountSummaryViewModel>();
        public AccountTypeViewModel AccountTypeViewModel => _provider.GetRequiredService<AccountTypeViewModel>();
        public AccountViewModel AccountViewModel => _provider.GetRequiredService<AccountViewModel>();
        public AllotmentViewModel AllotmentViewModel => _provider.GetRequiredService<AllotmentViewModel>();
        public BackupViewModel BackupViewModel => _provider.GetRequiredService<BackupViewModel>();
        public CompanyViewModel CompanyViewModel => _provider.GetRequiredService<CompanyViewModel>();
        public DateViewModel DateViewModel => _provider.GetRequiredService<DateViewModel>();
        public ExplorerViewModel ExplorerViewModel => _provider.GetRequiredService<ExplorerViewModel>();
        public IdentityViewModel IdentityViewModel => _provider.GetRequiredService<IdentityViewModel>();
        public MainViewModel MainViewModel => _provider.GetRequiredService<MainViewModel>();
        public NumberHistoryViewModel NumberHistoryViewModel => _provider.GetRequiredService<NumberHistoryViewModel>();
        public PalletteViewModel PalletteViewModel => _provider.GetRequiredService<PalletteViewModel>();
        public PasswordViewModel PasswordViewModel => _provider.GetRequiredService<PasswordViewModel>();
        public PoolViewModel PoolViewModel => _provider.GetRequiredService<PoolViewModel>();
        public PopupViewModel PopupViewModel => _provider.GetRequiredService<PopupViewModel>();
        public QAViewModel QAViewModel => _provider.GetRequiredService<QAViewModel>();
        public StatusbarViewModel StatusbarViewModel => _provider.GetRequiredService<StatusbarViewModel>();
        public TransactionViewModel TransactionViewModel => _provider.GetRequiredService<TransactionViewModel>();

        #endregion

        #region History View Models

        public HistoryViewModel HistoryViewModel => _provider.GetRequiredService<HistoryViewModel>();
        public Ledger5PasswordViewModel Ledger5PasswordViewModel => _provider.GetRequiredService<Ledger5PasswordViewModel>();

        #endregion

        #region DALs (Data Access Layer, interface to/from Entity Framework for database access)

        public IAccountDAL AccountDAL => _provider.GetRequiredService<IAccountDAL>();
        public IAccountNumberDAL AccountNumberDAL => _provider.GetRequiredService<IAccountNumberDAL>();
        public IAccountTypeDAL AccountTypeDAL => _provider.GetRequiredService<IAccountTypeDAL>();
        public IAllotmentDAL AllotmentDAL => _provider.GetRequiredService<IAllotmentDAL>();
        public ICompanyDAL CompanyDAL => _provider.GetRequiredService<ICompanyDAL>();
        public IIdentityDAL IdentityDAL => _provider.GetRequiredService<IIdentityDAL>();
        public IPoolDAL PoolDAL => _provider.GetRequiredService<IPoolDAL>();
        public ISettingsDAL SettingsDAL => _provider.GetRequiredService<ISettingsDAL>();
        public ITransactionDAL TransactionDAL => _provider.GetRequiredService<ITransactionDAL>();

        #endregion

        #region History DALs

        public IAccountHistoryDAL AccountHistoryDAL => _provider.GetRequiredService<IAccountHistoryDAL>();
        public IAccountNumberHistoryDAL AccountNumberHistoryDAL => _provider.GetRequiredService<IAccountNumberHistoryDAL>();
        public IAccountTypeHistoryDAL AccountTypeHistoryDAL => _provider.GetRequiredService<IAccountTypeHistoryDAL>();
        public IAllotmentHistoryDAL AllotmentHistoryDAL => _provider.GetRequiredService<IAllotmentHistoryDAL>();
        public IPayeeHistoryDAL PayeeHistoryDAL => _provider.GetRequiredService<IPayeeHistoryDAL>();
        public IPoolHistoryDAL PoolHistoryDAL => _provider.GetRequiredService<IPoolHistoryDAL>();
        public ITransactionHistoryDAL TransactionHistoryDAL => _provider.GetRequiredService<ITransactionHistoryDAL>();

        #endregion

        #region ECLs (Entity Conversion Layer, translates between Entity objects and Observable Data Transfer Objects / Models)

        public IAccountECL AccountECL => _provider.GetRequiredService<IAccountECL>();
        public IAccountNumberECL AccountNumberECL => _provider.GetRequiredService<IAccountNumberECL>();
        public IAccountTypeECL AccountTypeECL => _provider.GetRequiredService<IAccountTypeECL>();
        public IAllotmentECL AllotmentECL => _provider.GetRequiredService<IAllotmentECL>();
        public ICompanyECL CompanyECL => _provider.GetRequiredService<ICompanyECL>();
        public IIdentityECL IdentityECL => _provider.GetRequiredService<IIdentityECL>();
        public IPoolECL PoolECL => _provider.GetRequiredService<IPoolECL>();
        public ITransactionECL TransactionECL => _provider.GetRequiredService<ITransactionECL>();

        #endregion

        #endregion

        public Locator()
        {
            lock(_lockObject)
            {
                if (!_initialized)
                {
                    var services = new ServiceCollection();

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
