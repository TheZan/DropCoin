using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Nethereum.Geth;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3.Accounts.Managed;

namespace DropCoin.Model
{
    public static class DropAccount
    {
        private const string RpcUrl = "http://192.168.1.58:8545";
        private static Web3Geth Web3 { get; set; }
        private static ManagedAccount Account { get; set; }
        private static DropCoinDbContext db;
        public static Users User { get; set; }

        public static async Task<bool> Login(string username, string password)
        {
            try
            {
                await using (db = new DropCoinDbContext())
                {
                    User = await db.Users.FirstOrDefaultAsync(p => p.UserName == username);
                    Account = new ManagedAccount(User.DrpAddress, password);
                    Web3 = new Web3Geth(Account, RpcUrl);

                    if (await Web3.Personal.UnlockAccount.SendRequestAsync(Account.Address, Account.Password, 120))
                    {
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static async void Registration(string username, string password)
        {
            try
            {
                Web3 = new Web3Geth(RpcUrl);

                var address = await Web3.Personal.NewAccount.SendRequestAsync(password);

                await using (db = new DropCoinDbContext())
                {
                    var newUser = new Users()
                    {
                        UserName = username,
                        Password = password,
                        DrpAddress = address
                    };

                    await db.Users.AddAsync(newUser);
                    await db.SaveChangesAsync();
                }

                MessageBox.Show($"Регистрация прошла успешно.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static async Task<string> GetBalance()
        {
            try
            {
                var balance = await Web3.Eth.GetBalance.SendRequestAsync(User.DrpAddress);
                return balance.Value.ToString();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public static async Task<bool> SendTransaction(string accountAddressTo, string countSendToken)
        {
            try
            {
                if (Convert.ToInt32(await Task.Run(GetBalance)) >= Convert.ToInt64(countSendToken))
                {
                    var transaction = await Web3.TransactionManager.SendTransactionAsync(Account.Address, accountAddressTo, new HexBigInteger(Convert.ToInt64(countSendToken)));

                    await Web3.Miner.Start.SendRequestAsync(1);

                    var receipt = await Web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transaction);

                    while (receipt == null)
                    {
                        await Task.Delay(1500);
                        receipt = await Web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transaction);
                    }

                    await Web3.Miner.Stop.SendRequestAsync();

                    await using (db = new DropCoinDbContext())
                    {
                        var fromUser = User.UserId;
                        var toUser = db.Users.FirstOrDefaultAsync(p => p.DrpAddress == accountAddressTo).Result.UserId;
                        var newTransaction = new Transactions()
                        {
                            TransactionHash = transaction,
                            From = fromUser,
                            To = toUser,
                            Count = Convert.ToInt32(countSendToken),
                            TransactionDate = DateTime.Now
                        };

                        await db.Transactions.AddAsync(newTransaction);
                        await db.SaveChangesAsync();
                    }

                    return true;
                }

                MessageBox.Show("Недостаточно средств!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
