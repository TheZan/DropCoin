using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Nethereum.Geth;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.Transactions;
using Nethereum.Web3;
using Nethereum.Web3.Accounts.Managed;

namespace DropCoin.Model
{
    public static class DropAccount
    {
        private const string RpcUrl = "http://127.0.0.1:8545";
        public static string AccountAddress { get; set; }
        public static string AccountPassword { get; set; }
        private static Web3Geth web3 { get; set; }
        private static ManagedAccount account { get; set; }

        public static async Task<bool> Login()
        {
            try
            {
                account = new ManagedAccount(AccountAddress, AccountPassword);
                web3 = new Web3Geth(account, RpcUrl);

                if (await web3.Personal.UnlockAccount.SendRequestAsync(account.Address, account.Password, 120))
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
                web3 = new Web3Geth(RpcUrl);

                var address = await web3.Personal.NewAccount.SendRequestAsync(password);
                using (var file = new FileStream("AccountData.txt", FileMode.Create))
                {
                    using(var writer = new StreamWriter(file))
                    {
                        writer.WriteLine($"Адрес учетной записи: {address}");
                        writer.WriteLine($"Пароль: {password}");
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
                var balance = await web3.Eth.GetBalance.SendRequestAsync(AccountAddress);
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
                if (Convert.ToInt32(await Task.Run(GetBalance)) > Convert.ToInt32(countSendToken))
                {
                    var transactionManagedAccount = await web3.TransactionManager.SendTransactionAsync(account.Address, accountAddressTo, new HexBigInteger(Convert.ToInt32(countSendToken)));
                    var test = await web3.Miner.Start.SendRequestAsync();
                    var test2 = await web3.Miner.Stop.SendRequestAsync();
                    MessageBox.Show(transactionManagedAccount);
                    return true;
                }
                else
                {
                    MessageBox.Show("Недостаточно средств!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static void GetTransactions()
        {
            
        }
    }
}
