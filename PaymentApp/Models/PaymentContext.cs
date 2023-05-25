using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace PaymentApp.Models
{
    public partial class PaymentContext : DbContext
    {
        public PaymentContext()
        {
        }

        public PaymentContext(DbContextOptions<PaymentContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<BankAccount> BankAccounts { get; set; }
        public virtual DbSet<CardBank> CardBanks { get; set; }
        public virtual DbSet<CardVirtual> CardVirtuals { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Deposit> Deposits { get; set; }
        public virtual DbSet<DepositBank> DepositBanks { get; set; }
        public virtual DbSet<DepositCard> DepositCards { get; set; }
        public virtual DbSet<Transfer> Transfers { get; set; }
        public virtual DbSet<Wallet> Wallets { get; set; }
        public virtual DbSet<Withdraw> Withdraws { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=Payment;Integrated Security=True;Encrypt=false;TrustServerCertificate=true;MultipleActiveResultSets = True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<Bank>(entity =>
            {
                entity.Property(e => e.BankId).HasColumnName("Bank_id");

                entity.Property(e => e.BankName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Bank_Name");

                entity.Property(e => e.Iban)
                    .IsRequired()
                    .HasMaxLength(34)
                    .HasColumnName("IBAN");
            });

            modelBuilder.Entity<BankAccount>(entity =>
            {
                entity.ToTable("Bank_Account");

                entity.Property(e => e.BankAccountId).HasColumnName("Bank_Account_id");

                entity.Property(e => e.AccountName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Account_Name");

                entity.Property(e => e.BankId).HasColumnName("Bank_id");

                entity.Property(e => e.BankName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Bank_Name");

                entity.Property(e => e.CustomerId).HasColumnName("Customer_id");

                entity.Property(e => e.IbanCustomer)
                    .IsRequired()
                    .HasMaxLength(34)
                    .HasColumnName("IBAN_Customer");

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.BankAccounts)
                    .HasForeignKey(d => d.BankId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Bank_Account_Banks");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.BankAccounts)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Bank_Account_Customer");
            });

            modelBuilder.Entity<CardBank>(entity =>
            {
                entity.ToTable("Card_Bank");

                entity.Property(e => e.CardBankId).HasColumnName("Card_Bank_id");

                entity.Property(e => e.BankId).HasColumnName("Bank_id");

                entity.Property(e => e.CardName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Card_name");

                entity.Property(e => e.CardNumber)
                    .IsRequired()
                    .HasMaxLength(16)
                    .HasColumnName("Card_Number");

                entity.Property(e => e.CustomerId).HasColumnName("Customer_id");

                entity.Property(e => e.ExpireDate)
                    .HasColumnType("date")
                    .HasColumnName("Expire_Date");

                entity.Property(e => e.SecurityCode)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnName("Security_Code");

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.CardBanks)
                    .HasForeignKey(d => d.BankId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Card_Bank_Banks");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CardBanks)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Card_Bank_Customer");
            });

            modelBuilder.Entity<CardVirtual>(entity =>
            {
                entity.HasKey(e => e.CardIdVirtual);

                entity.ToTable("Card_Virtual");

                entity.Property(e => e.CardIdVirtual).HasColumnName("Card_id_Virtual");

                entity.Property(e => e.CardNumber)
                    .IsRequired()
                    .HasMaxLength(16)
                    .HasColumnName("Card_number");

                entity.Property(e => e.CustomerId).HasColumnName("Customer_id");

                entity.Property(e => e.ExpireDate)
                    .HasColumnType("date")
                    .HasColumnName("Expire_Date");

                entity.Property(e => e.NameCard)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Name_Card");

                entity.Property(e => e.SecurityCode)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnName("Security_Code");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CardVirtuals)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Card_Virtual_Customer");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Country");

                entity.Property(e => e.CountryId).HasColumnName("Country_id");

                entity.Property(e => e.CountryName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Country_Name");
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.ToTable("Currency");

                entity.Property(e => e.CurrencyId).HasColumnName("Currency_id");

                entity.Property(e => e.CurrencyDollarValue).HasColumnName("Currency_Dollar_Value");

                entity.Property(e => e.CurrencyName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Currency_Name");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.HasIndex(e => e.AccountNumber, "IX_Customer")
                    .IsUnique();

                entity.Property(e => e.CustomerId).HasColumnName("Customer_id");

                entity.Property(e => e.AccountNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Account_Number");

                entity.Property(e => e.CountryId).HasColumnName("Country_id");

                entity.Property(e => e.CustomerMail)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Customer_Mail");

                entity.Property(e => e.CustomerName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Customer_Name");

                entity.Property(e => e.CustomerPassword)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Customer_Password");

                entity.Property(e => e.CustomerTel)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Customer_Tel");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customer_Country");
            });

            modelBuilder.Entity<Deposit>(entity =>
            {
                entity.ToTable("Deposit");

                entity.Property(e => e.DepositId).HasColumnName("Deposit_id");

                entity.Property(e => e.AccountNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Account_Number");

                entity.Property(e => e.DepositTypeId).HasColumnName("Deposit_Type_id");

                entity.Property(e => e.TimeStamp)
                    .HasColumnType("date")
                    .HasColumnName("Time_Stamp");

                entity.HasOne(d => d.CurencyNavigation)
                    .WithMany(p => p.Deposits)
                    .HasForeignKey(d => d.Curency)
                    .HasConstraintName("FK_Deposit_Currency");

                entity.HasOne(d => d.DepositType)
                    .WithMany(p => p.Deposits)
                    .HasForeignKey(d => d.DepositTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Deposit_Deposit_Bank");

                entity.HasOne(d => d.DepositTypeNavigation)
                    .WithMany(p => p.Deposits)
                    .HasForeignKey(d => d.DepositTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Deposit_Deposit_Card");
            });

            modelBuilder.Entity<DepositBank>(entity =>
            {
                entity.ToTable("Deposit_Bank");

                entity.Property(e => e.DepositBankId).HasColumnName("Deposit_Bank_id");

                entity.Property(e => e.AccountNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Account_Number");

                entity.Property(e => e.BankId).HasColumnName("Bank_id");

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.DepositBanks)
                    .HasForeignKey(d => d.BankId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Deposit_Bank_Banks");
            });

            modelBuilder.Entity<DepositCard>(entity =>
            {
                entity.ToTable("Deposit_Card");

                entity.Property(e => e.DepositCardId).HasColumnName("Deposit_Card_id");

                entity.Property(e => e.AccountNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Account_Number");

                entity.Property(e => e.CardId).HasColumnName("Card_id");

                entity.HasOne(d => d.Card)
                    .WithMany(p => p.DepositCards)
                    .HasForeignKey(d => d.CardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Deposit_Card_Card_Bank");

                entity.HasOne(d => d.CardNavigation)
                    .WithMany(p => p.DepositCards)
                    .HasForeignKey(d => d.CardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Deposit_Card_Card_Virtual");
            });

            modelBuilder.Entity<Transfer>(entity =>
            {
                entity.HasKey(e => e.TransId);

                entity.Property(e => e.TransId)
                  
                    .HasColumnName("Trans_id");

                entity.Property(e => e.AccountNumberGiver)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Account_Number_Giver");

                entity.Property(e => e.AccountNumberTaker)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Account_Number_Taker");

                entity.HasOne(d => d.CurrencyNavigation)
                    .WithMany(p => p.Transfers)
                    .HasForeignKey(d => d.Currency)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transfers_Currency");
            });

            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.ToTable("Wallet");

                entity.Property(e => e.WalletId).HasColumnName("Wallet_id");

                entity.Property(e => e.AccountNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Account_Number");

                entity.Property(e => e.CustomerId).HasColumnName("Customer_id");

                entity.HasOne(d => d.CurrencyNavigation)
                    .WithMany(p => p.Wallets)
                    .HasForeignKey(d => d.Currency)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Wallet_Currency");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Wallets)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Wallet_Customer");
            });

            modelBuilder.Entity<Withdraw>(entity =>
            {
                entity.ToTable("Withdraw");

                entity.Property(e => e.WithdrawId).HasColumnName("Withdraw_id");

                entity.Property(e => e.BankAccountId).HasColumnName("Bank_Account_id");

                entity.Property(e => e.Currency).HasMaxLength(10);

                entity.Property(e => e.IbanCustomer)
                    .IsRequired()
                    .HasMaxLength(34)
                    .HasColumnName("IBAN_Customer");

                entity.HasOne(d => d.BankAccount)
                    .WithMany(p => p.Withdraws)
                    .HasForeignKey(d => d.BankAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Withdraw_Bank_Account");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
