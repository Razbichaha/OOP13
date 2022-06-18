﻿using System;
using System.Collections.Generic;

namespace OOP13
{
    class Program
    {
        static void Main(string[] args)
        {
            ProgramCore programCore = new ProgramCore();
            programCore.StartAutoservis();
        }
    }

    class ProgramCore
    {
        private Autoservis _autoservis = new Autoservis();

        internal void StartAutoservis()
        {
            _autoservis.StartAutoservis();
            Console.ReadLine();
        }
    }

    class Autoservis
    {
        internal int Money { get; private set; }

        private Warehouse _warehouse = new Warehouse();
        private Menu _menu = new Menu();

        internal Autoservis()
        {
            Money = 100000;
        }

        internal void StartAutoservis()
        {
            bool continueGame = true;

            while (continueGame)
            {
                Auto auto = new Auto(_warehouse);
                List<Detail> BreakdownDetails = auto.GetCarBreakdown();

                _menu.ShowStartGame();
                _menu.ShowMoney(Money);
                _menu.ShowBreakdown(BreakdownDetails);

                bool thereBrokenParts = true;

                while (thereBrokenParts)
                {
                    Repair(auto);
                    BreakdownDetails = auto.GetCarBreakdown();
                    _menu.ShowBreakdown(BreakdownDetails);
                    _menu.ShowMoney(Money);

                    if (auto.ThereBrokenParts() == false)
                    {
                        thereBrokenParts = false;
                    }
                }
                continueGame = ContinueGame();
            }
        }

        private bool ContinueGame()
        {
            bool continueGame = true;
            _menu.MessagContinueGame();
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            if (keyInfo.Key==ConsoleKey.Escape)
            {
                continueGame = false;
            }
            return continueGame;
        }

        private void Repair(Auto auto)
        {
            string numberDetailString = _menu.SelektDetal();
            int numberDetail = 0;

            if (IsNumber(numberDetailString, ref numberDetail))
            {
                if (auto.InputIsCorrect(numberDetail))
                {
                    Money += auto.MakeRepairs(numberDetail);
                }
                else
                {
                    ErrorInput();
                }
            }
            else
            {
                ErrorInput();
            }
        }

        private void ErrorInput()
        {
            int fine = 10000;
            _menu.ErrorInput(fine);
            Money -= fine;
        }
        
        private bool IsNumber(string text, ref int number)
        {
            bool isNumber = int.TryParse(text, out number);
            return isNumber;
        }

    }

    class Menu
    {
        internal void MessagContinueGame()
        {
            Console.WriteLine();
            Console.WriteLine("Хотите продолжить ремонтировать автомобили нажмите Enter");
            Console.WriteLine("Если завершили работу нажмите ESC.");
        }

        internal void ShowMoney(int money)
        {
            Console.WriteLine($"На счету вашего автосервиса - {money} денег.");
        }

        internal void ErrorInput(int fine)
        {
            Console.WriteLine();
            Console.WriteLine($"Не правильный выбор получите штраф - {fine}");
        }

        internal void ShowStartGame()
        {
            Console.Clear();
            Console.WriteLine("В ремонт поступил новый автомобиль");
            Console.ReadLine();
        }

        internal void ShowBreakdown(List<Detail> details)
        {
            Console.WriteLine("Мастер приемщик предоставил список следующих неисправностей");

            foreach (Detail detail in details)
            {
                Console.WriteLine($"Наименование детали - {detail.Name}  стоимость - {detail.PartPrice}  стоимость работ по замене - {detail.WorkCoste}");
            }
        }

        internal string SelektDetal()
        {
            Console.WriteLine();
            Console.Write("Выберите номер детали для замены - ");
            string numberDetailString = Console.ReadLine();
            return numberDetailString;
        }
    }

    class Warehouse
    {
        private int _maximumDetails = 100;
        private int _minimumDetails = 50;
        private int _maximumQuantity = 10;
        private int _minimumQuantity = 2;

        private List<Detail> _details = new List<Detail>();
        private List<int> _quantityDetails = new List<int>();

        internal Warehouse()
        {
            Create();
        }

        internal Detail GetRandomDetail()
        {
            Random random = new Random();
            int randomNumber = random.Next(/*_minimumDetails*/0, _details.Count);
            Detail detail;

            if (randomNumber == _minimumDetails)
            {
                detail = _details[_details.Count - 1];
            }
            else
            {
                detail = _details[randomNumber];
            }

            return detail;
        }

        internal int Count()
        {
            return _details.Count;
        }

        private void Create()
        {
            Random random = new Random();

            for (int i = 0; i < random.Next(_minimumDetails, _maximumDetails); i++)
            {
                string nameDetails = "Деталь_";
                nameDetails += i.ToString();
                Detail detail = new Detail(nameDetails);
                _details.Add(detail);
                _quantityDetails.Add(random.Next(_minimumQuantity, _maximumQuantity));
            }
        }
    }

    class Auto
    {
        private int _maximumQuantityBreakdown = 6;
        private int _minimumQuantityBreakdown = 1;

        private List<Detail> _details = new List<Detail>();

        internal Auto(Warehouse warehouse)
        {
            Create(warehouse);
        }

        internal List<Detail> GetCarBreakdown()
        {
            Random random = new Random();
            List<Detail> detailBreakdown = new List<Detail>();

            foreach (Detail detail in _details)
            {
                detailBreakdown.Add(detail);
            }

            return detailBreakdown;
        }

        internal bool ThereBrokenParts()
        {
            bool thereBrokenParts = true;

            if (_details.Count == 0)
            {
                thereBrokenParts = false;
            }
            return thereBrokenParts;
        }

        internal int MakeRepairs(int numberDetails)
        {
            string nameDetails = "Деталь_" + numberDetails;
            int payment = 0;

            foreach (Detail detail in _details)
            {
                if (detail.Name == nameDetails)
                {
                    payment = detail.PartPrice + detail.WorkCoste;
                    _details.Remove(detail);
                    break;
                }
            }
            return payment;
        }

        internal bool InputIsCorrect(int numberDetail)
        {
            bool isCorrect = false;
            string nameDetails = "Деталь_" + numberDetail;

            foreach (Detail detail in _details)
            {
                if (detail.Name == nameDetails)
                {
                    isCorrect = true;
                }
            }
            return isCorrect;
        }

        private bool IsRepeat(int[] massiv, int number)
        {
            bool isRepeat = false;

            foreach (var item in massiv)
            {
                if (item == number)
                {
                    isRepeat = true;
                    break;
                }
            }
            return isRepeat;
        }

        private void Create(Warehouse warehouse)
        {
            Random random = new Random();

            for (int i = 0; i < random.Next(_minimumQuantityBreakdown, _maximumQuantityBreakdown); i++)
            {
                _details.Add(warehouse.GetRandomDetail());
            }
        }
    }

    class Detail
    {
        internal string Name { get; private set; }
        internal int PartPrice { get; private set; }
        internal int WorkCoste { get; private set; }

        private int _maximumRateDetail = 5000;
        private int _minimumRateDetail = 10;
        private int _maximumCosteProcent = 150;
        private int _minimumCosteProcent = 50;

        internal Detail(string name)
        {
            Name = name;
            GenerateRate();
        }

        private void GenerateRate()
        {
            Random random = new Random();
            PartPrice = random.Next(_minimumRateDetail, _maximumRateDetail);
            WorkCoste = ProcentCoste(PartPrice);
        }

        private int ProcentCoste(int partPrace)
        {
            Random random = new Random();
            int procent100 = 100;
            int procentCost = partPrace / procent100 * random.Next(_minimumCosteProcent, _maximumCosteProcent);
            return procentCost;
        }
    }
}