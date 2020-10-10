using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using Nethereum.Web3.Accounts.Managed;

namespace DropCoin.Model
{
    public static class Account
    {
        private const string RpcUrl = "http://127.0.0.1:8545";
        public static string AccountAddress { get; set; }
        public static string AccountPassword { get; set; }
        private static Web3 web3 { get; set; }

        public static void Login()
        {
            try
            {
                var account = new ManagedAccount(AccountAddress, AccountPassword);
                web3 = new Web3(account, RpcUrl);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static async void Registration(string password)
        {
            try
            {
                web3 = new Web3(RpcUrl);
                //var test = await web3.Personal.NewAccount.SendRequestAsync(password);
                //MessageBox.Show(test);

                var ecKey = Nethereum.Signer.EthECKey.GenerateKey();
                var privateKey = ecKey.GetPrivateKeyAsBytes().ToHex();
                var account = new Nethereum.Web3.Accounts.Account("tre");
                var t = account.PrivateKey;
                MessageBox.Show(t);
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
    }
}
