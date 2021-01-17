using LedgerLib.HistoryEntities;
using LedgerLib.Infrastructure;

using Microsoft.EntityFrameworkCore;

namespace LedgerLib
{
    public class HistoryContext : DbContext
    {
        private readonly string _connectionString;

        #region DbSets

        public DbSet<AccountHistoryEntity> Accounts { get; set; }
        public DbSet<AccountNumberHistoryEntity> AccountNumbers { get; set; }
        public DbSet<AccountTypeHistoryEntity> AccountTypes { get; set; }
        public DbSet<AllotmentHistoryEntity> Allotments { get; set; }
        public DbSet<PayeeHistoryEntity> Payees { get; set; }
        public DbSet<PoolHistoryEntity> Pools { get; set; }
        public DbSet<TransactionHistoryEntity> Transactions { get; set; }

        #endregion

        #region Constructors

        public HistoryContext(DbContextOptions<HistoryContext> options) : base(options) => _connectionString = CSBuilder.BuildHistory();

        public HistoryContext() : base() => _connectionString = CSBuilder.BuildHistory();

        #endregion

        #region Overrides

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
            builder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AccountHistoryEntity>().HasIndex(x => x.PayeeId).IsClustered(false);
            builder.Entity<AccountHistoryEntity>().HasIndex(x => x.AccountTypeId).IsClustered(false);
            builder.Entity<AccountHistoryEntity>().HasOne(x => x.AccountType);
            builder.Entity<AccountHistoryEntity>().HasMany(x => x.AccountNumbers);

            builder.Entity<AccountNumberHistoryEntity>().HasIndex(x => x.AccountId).IsClustered(false);
            builder.Entity<AccountNumberHistoryEntity>().Property(x => x.StartDate).HasColumnType(Constants.Datetime2);
            builder.Entity<AccountNumberHistoryEntity>().Property(x => x.EndDate).HasColumnType(Constants.Datetime2);

            builder.Entity<AccountTypeHistoryEntity>().HasIndex(x => x.Description).IsUnique().IsClustered(false);

            builder.Entity<AllotmentHistoryEntity>().HasIndex(x => x.PoolId).IsClustered(false);
            builder.Entity<AllotmentHistoryEntity>().HasIndex(x => x.PayeeId).IsClustered(false);
            builder.Entity<AllotmentHistoryEntity>().HasOne(x => x.Payee);
            builder.Entity<AllotmentHistoryEntity>().Property(x => x.Date).HasColumnType(Constants.Datetime2);
            builder.Entity<AllotmentHistoryEntity>().Property(x => x.Amount).HasColumnType(Constants.HistoryMoneyFormat);

            builder.Entity<PayeeHistoryEntity>().HasIndex(x => x.Name).IsUnique().IsClustered(false);

            builder.Entity<PoolHistoryEntity>().HasIndex(x => x.Name).IsUnique().IsClustered(false);
            builder.Entity<PoolHistoryEntity>().Property(x => x.Date).HasColumnType(Constants.Datetime2);
            builder.Entity<PoolHistoryEntity>().Property(x => x.Amount).HasColumnType(Constants.HistoryMoneyFormat);

            builder.Entity<TransactionHistoryEntity>().HasIndex(x => x.AccountId).IsClustered(false);
            builder.Entity<TransactionHistoryEntity>().Property(x => x.Date).HasColumnType(Constants.Datetime2);
            builder.Entity<TransactionHistoryEntity>().Property(x => x.Balance).HasColumnType(Constants.HistoryMoneyFormat);
            builder.Entity<TransactionHistoryEntity>().Property(x => x.Payment).HasColumnType(Constants.HistoryMoneyFormat);

        }

        #endregion
    }
}
