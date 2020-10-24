using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Nethereum.Geth;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3.Accounts.Managed;

namespace DropCoin.Model
{
    public static class DropAccount
    {
        private const string RpcUrl = "http://127.0.0.1:8545";
        public static string AccountAddress { get; set; }
        public static string AccountPassword { get; set; }
        private static Web3Geth Web3 { get; set; }
        private static ManagedAccount Account { get; set; }

        public static async Task<bool> Login()
        {
            try
            {
                Account = new ManagedAccount(AccountAddress, AccountPassword);
                Web3 = new Web3Geth(Account, RpcUrl);

                if (await Web3.Personal.UnlockAccount.SendRequestAsync(Account.Address, Account.Password, 120))
                {
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static async void Registration(string password)
        {
            try
            {
                Web3 = new Web3Geth(RpcUrl);

                var address = await Web3.Personal.NewAccount.SendRequestAsync(password);
                await using (var file = new FileStream("AccountData.txt", FileMode.Create))
                {
                    await using(var writer = new StreamWriter(file))
                    {
                        await writer.WriteLineAsync($"Адрес учетной записи: {address}");
                        await writer.WriteLineAsync($"Пароль: {password}");
                    }
                }

                MessageBox.Show($"Регистрация прошла успешно. Файл AccountData.txt с данными учетной записи создан.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
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
                var balance = await Web3.Eth.GetBalance.SendRequestAsync(AccountAddress);
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
