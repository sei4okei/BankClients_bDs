using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using static BankClients_bDs.Clients;
using static BankClients_bDs.Deposits;

namespace BankClients_bDs
{
    internal class Menu
    {
        private static List<Clients> DB = new List<Clients>();
        private static IEnumerator<int> sequenceDeposit = Enumerable.Range(1, int.MaxValue).GetEnumerator();
        private static IEnumerator<int> sequenceClient = Enumerable.Range(1, int.MaxValue).GetEnumerator();
        static string[] menuArray = new string[4] { "Меню", "1. Создание клиента", "2. Просмотр базы", "3. Выход"};
        static void Main()
        {
            WriteLine("Введите действие: ");
            int userNum = Convert.ToInt32(ReadLine());
            if (userNum == 1)
            {
                AddNewRecord();
            }
            else if (userNum == 2)
            {
                ReadAllRecords();
            }
        }

        static void AddNewRecord()
        {
            Write("Фамилия: "); var clientSurName = ReadLine();
            Write("Номер паспорта: "); var clientPassNumber = ReadLine();
            Write("Кол-во вкладов: "); var clientAmountDeposits = Convert.ToInt32(ReadLine());
            if (DB.Count == sequenceClient.Current)
            {
                sequenceClient.MoveNext();
            }
            var clientId = sequenceClient.Current;
            var client = new Clients
            {
                ID = clientId,
                surName = clientSurName,
                passNumber = clientPassNumber,
                amountDeposit = clientAmountDeposits
            };
            client.infoDeposit = new List<Deposits>();
            for (int i = 0; i < clientAmountDeposits; i++)
            {
                Write("Процентная ставка: "); var depositPercent = Convert.ToDouble(ReadLine());
                Write("Размер вклада: "); var depositSize = Convert.ToDouble(ReadLine());
                Write("Время открытия вклада: "); var depositDateOpen = ReadLine();
                Write("Время закрытия вклада: "); var depositDateClose = ReadLine();
                sequenceDeposit.MoveNext();
                var depositID = sequenceDeposit.Current;
                var deposit = new Deposits
                {
                    ID = depositID,
                    percent = depositPercent,
                    size = depositSize,
                    dateOpen = depositDateOpen,
                    dateClose = depositDateClose,
                    clientPassNumebrs = clientPassNumber
                };
                client.infoDeposit.Add(deposit);
            }
            DB.Add(client);
            Main();
        }

        static void ReadAllRecords()
        { 
            foreach (var client in DB)
            {
                WriteLine($"ID\t Фамилия\t Номер паспорта\t Количество вкладов");
                WriteLine($"{client.ID}\t {client.surName}\t {client.passNumber}\t {client.amountDeposit}");
                WriteLine();
                WriteLine($"ID\t Ставка\t Размер\t Дата открытия\t Дата закрытия\t Номер паспорта");
                foreach (var deposit in client.infoDeposit)
                {
                    WriteLine($"{deposit.ID}\t {deposit.percent}\t {deposit.size}\t {deposit.dateOpen}\t {deposit.dateClose}\t {deposit.clientPassNumebrs}");
                    WriteLine();
                }
            }
            Main();
        }
    }
}
