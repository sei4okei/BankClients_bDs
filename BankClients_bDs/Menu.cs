using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using static BankClients_bDs.Clients;
using static BankClients_bDs.Deposits;
using System.Data;

namespace BankClients_bDs
{
    internal class Menu
    {
        private static DateTime todayDate = DateTime.Now;
        private static List<Clients> DB = new List<Clients>();
        private static IEnumerator<int> sequenceDeposit = Enumerable.Range(1, int.MaxValue).GetEnumerator();
        private static IEnumerator<int> sequenceClient = Enumerable.Range(1, int.MaxValue).GetEnumerator();
        static string[] menuArray = new string[4] { "Меню", "1. Создание клиента", "2. Просмотр базы", "3. Выход" };
        static void Main()
        {
            while (true)
            {
                WriteLine("Введите действие: ");
                WriteLine("0. Выйти\n1. Добавить нового клиента\n2. Просмотр клиентов\n3. Максимальная сумма вклада\n4. Минимальная сумма вклада\n5. Провести транзакцию\n6. Удалить клиента");
                switch (Convert.ToInt32(ReadLine()))
                {
                    case 0:
                        Environment.Exit(0);
                        break;

                    case 1:
                        AddNewRecord();
                        break;

                    case 2:
                        ReadAllRecords();
                        break;

                    case 3:
                        MaxAmountofDepositProfit();
                        break;

                    case 4:
                        MinAmount();
                        break;

                    case 5:
                        Write("Введите номер счета списания: "); var min_depositID = Convert.ToInt32(ReadLine());
                        Write("Введите номер счета поступления: "); var plus_depositID = Convert.ToInt32(ReadLine());
                        Write("Введите сумму перевода "); var amount = Convert.ToDouble(ReadLine());
                        Transaction(min_depositID, plus_depositID, amount);
                        break;

                    case 6:
                        DeleteRecord();
                        break;

                    default:
                        WriteLine("Такой комманды не существует, попробуйте снова.");
                        break;
                }
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
                WriteLine("Дата открытия вклада:"); Write("Год: "); var depositYearOpen = Convert.ToInt32(ReadLine()); Write("Месяц: "); var depositMonthOpen = Convert.ToInt32(ReadLine()); Write("День: "); var depositDayOpen = Convert.ToInt32(ReadLine()); var depositDateOpen = new DateTime(depositYearOpen, depositMonthOpen, depositDayOpen);
                WriteLine("Дата закрытия вклада:"); Write("Год: "); var depositYearClose = Convert.ToInt32(ReadLine()); Write("Месяц: "); var depositMonthClose = Convert.ToInt32(ReadLine()); Write("День: "); var depositDayClose = Convert.ToInt32(ReadLine()); var depositDateClose = new DateTime(depositYearClose, depositMonthClose, depositDayClose);
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
        }

        static void ReadAllRecords()
        {
            foreach (var client in DB)
            {
                WriteLine($"ID\t Фамилия\t Номер паспорта\t Количество вкладов");
                WriteLine($"{client.ID}\t {client.surName}\t {client.passNumber}\t {client.amountDeposit}");
                foreach (var deposit in client.infoDeposit)
                {
                    WriteLine();
                    WriteLine($"\tID\t Ставка\t Размер\t Дата открытия\t\t Дата закрытия\t");
                    WriteLine($"\t{deposit.ID}\t {deposit.percent}\t {deposit.size}\t {deposit.dateOpen}\t {deposit.dateClose}\t");
                    WriteLine($"\tНомер паспорта\t Доход\t");
                    WriteLine($"\t{deposit.clientPassNumebrs}\t {CalculateDepositProfit(deposit)}");
                    WriteLine();
                }
            }
        }

        static void MaxAmountofDepositProfit()
        {
            int maxClientID = 0;
            int maxDepositID = 0;
            var max = 0.0;
            foreach (var client in DB)
            {
                foreach (var deposit in client.infoDeposit)
                {
                    if (CalculateDepositProfit(deposit) > max)
                    {
                        max = CalculateDepositProfit(deposit);
                        maxDepositID = deposit.ID;
                        maxClientID = client.ID;
                    }
                }
            }

            foreach (var client in DB)
            {
                if (client.ID == maxClientID)
                {
                    WriteLine($"ID\t Фамилия\t Номер паспорта\t Количество вкладов");
                    WriteLine($"{client.ID}\t {client.surName}\t {client.passNumber}\t {client.amountDeposit}");
                }
                foreach (var deposit in client.infoDeposit)
                {
                    if (deposit.ID == maxDepositID)
                    {
                        WriteLine();
                        WriteLine($"\tID\t Ставка\t Размер\t Дата открытия\t\t Дата закрытия\t");
                        WriteLine($"\t{deposit.ID}\t {deposit.percent}\t {deposit.size}\t {deposit.dateOpen}\t {deposit.dateClose}\t");
                        WriteLine($"\tНомер паспорта\t Доход\t");
                        WriteLine($"\t{deposit.clientPassNumebrs}\t {CalculateDepositProfit(deposit)}");
                        WriteLine();
                    }
                }
            }

        }

        static void MinAmount()
        {
            int minClientID = 0;
            int minDepositID = 0;
            var min = Double.MaxValue;
            foreach (var client in DB)
            {
                foreach (var deposit in client.infoDeposit)
                {
                    if (CalculateDepositProfit(deposit) < min)
                    {
                        min = CalculateDepositProfit(deposit);
                        minDepositID = deposit.ID;
                        minClientID = client.ID;
                    }
                }
            }

            foreach (var client in DB)
            {
                if (client.ID == minClientID)
                {
                    WriteLine($"ID\t Фамилия\t Номер паспорта\t Количество вкладов");
                    WriteLine($"{client.ID}\t {client.surName}\t {client.passNumber}\t {client.amountDeposit}");
                }
                WriteLine($"\tID\t Ставка\t Размер\t Дата открытия\t Дата закрытия\t Номер паспорта");
                foreach (var deposit in client.infoDeposit)
                {
                    if (deposit.ID == minDepositID)
                    {
                        WriteLine();
                        WriteLine($"\tID\t Ставка\t Размер\t Дата открытия\t\t Дата закрытия\t");
                        WriteLine($"\t{deposit.ID}\t {deposit.percent}\t {deposit.size}\t {deposit.dateOpen}\t {deposit.dateClose}\t");
                        WriteLine($"\tНомер паспорта\t Доход\t");
                        WriteLine($"\t{deposit.clientPassNumebrs}\t {CalculateDepositProfit(deposit)}");
                        WriteLine();
                    }
                }
            }
        }

        static void DeleteRecord()
        {
            Write("Введите пользователя для удаления: "); var userDeletedRecord = ReadLine();
            DB.RemoveAt(Convert.ToInt32(userDeletedRecord) - 1);
        }

        static void Transaction(int min_depositID, int plus_depositID, double amount)
        {
            var takenAmount = amount;
            foreach (var client in DB)
            {
                foreach (var deposit in client.infoDeposit)
                {
                    if (min_depositID == deposit.ID)
                    {
                        if (amount <= deposit.size)
                        {
                            deposit.size -= takenAmount;
                        }
                        else
                        {
                            WriteLine("На указанном счету недостаточно средств");
                        }
                    }
                }
            }
            foreach (var client in DB)
            {
                foreach (var deposit in client.infoDeposit)
                {
                    if (plus_depositID == deposit.ID)
                    {
                        deposit.size += takenAmount;
                    }
                }
            }
        }

        static double CalculateDepositProfit(Deposits deposit)
        {
            double amountOfDaysGone = (deposit.dateClose - todayDate).TotalDays;
            double amountOfDaysShouldGone = (deposit.dateClose - deposit.dateOpen).TotalDays;
            if (todayDate > deposit.dateClose)
            {
                return ((((deposit.dateClose - deposit.dateOpen).TotalDays / 365 * (deposit.percent / 100)) + 1) * deposit.size) - deposit.size;
            }
            return ((((todayDate - deposit.dateOpen).TotalDays / 365 * (deposit.percent / 100)) + 1) * deposit.size) - deposit.size;

        }
    }
}
