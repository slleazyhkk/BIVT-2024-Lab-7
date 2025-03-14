﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_7
{
    public class Blue_3
    {
        public class Participant
        {
            private string _name;
            private string _surname;
            protected int[] _penalties;

            public string Name => _name;
            public string Surname => _surname;
            public int[] Penalties
            {
                get
                {
                    if (_penalties == null) return null;
                    int[] copy = new int[_penalties.Length];
                    Array.Copy(_penalties, copy, copy.Length);
                    return copy;
                }
            }

            public int Total //сумма штрафов
            {
                get
                {
                    int s = 0;
                    if (_penalties == null) return 0;
                    for (int i = 0; i < _penalties.Length; i++)
                    {
                        s += _penalties[i];
                    }
                    return s;
                }
            }

            public virtual bool IsExpelled //исключен, если штраф 10 хотя бы в одном матче
            {
                get
                {
                    bool ex = false;
                    if (_penalties == null) return false;
                    for (int i = 0; i < _penalties.Length; i++)
                    {
                        if (_penalties[i] == 10)
                        {
                            ex = true;
                            break;
                        }
                    }
                    return ex;
                }
            }
            //конструктор
            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _penalties = new int[0];
            }
            //методы
            public virtual void PlayMatch(int time) //добавляет штрафное время в массив штрафов
            {
                if (_penalties == null) return;

                int[] newpenaltytimes = new int[_penalties.Length + 1];

                for (int i = 0; i < newpenaltytimes.Length - 1; i++)
                {
                    newpenaltytimes[i] = _penalties[i];
                }

                newpenaltytimes[newpenaltytimes.Length - 1] = time;

                _penalties = newpenaltytimes;
            }

            public static void Sort(Participant[] array) //пузырьком <3  по возрастанию штрафного времени
            {
                if (array == null || array.Length == 0) return;
                for (int i = 0; i < array.Length; i++)
                {
                    for (int j = 0; j < array.Length - i - 1; j++)
                    {
                        if (array[j].Total > array[j + 1].Total)
                        {
                            var temp = array[j];
                            array[j] = array[j + 1];
                            array[j + 1] = temp;
                        }
                    }
                }
            }

            public void Print()
            {
                Console.WriteLine(_name + " " + _surname + " " + $"Общее штрафное время: {Total} Исключение спортсмена: {IsExpelled}");
            }
        }
        public class BasketballPlayer : Participant
        {
           
            public override bool IsExpelled
            {
                get
                {
                    if (_penalties == null || _penalties.Length == 0) return false;
                    int cnt=_penalties.Length;
                    int falls = 0;
                    foreach (int penalty in _penalties)
                    {
                        if (penalty >= 5) falls++;
                    }
                    if (falls > 0.1 * cnt || this.Total >= 2 * cnt) return true;
                    return false;
                }
            }

            public BasketballPlayer(string name, string surname) : base(name, surname)
            {
                _penalties = new int[0];
            }


            public override void PlayMatch(int fallsCount)
            {
                if (_penalties == null || fallsCount<0 || fallsCount>5) return;
                base.PlayMatch(fallsCount);
            }
        }

        public class HockeyPlayer : Participant
        {

            private int totalPenaltyMinutes;
            private static int countPlayers;
            private static int totalPenaltyMinutesAll;
            public override bool IsExpelled
            {
                get
                {
                    if (_penalties == null || _penalties.Length == 0) return false;
                    foreach (int penalty in _penalties)
                    {
                        if (penalty >= 10)
                        {
                            return true;
                        }
                    }
                    if (totalPenaltyMinutes > (totalPenaltyMinutesAll / countPlayers) *0.1) return true;
                    return false;
                }
            }

            public HockeyPlayer(string name, string surname) : base(name, surname)
            {
                _penalties = new int[0];
                countPlayers++;
            }
            public override void PlayMatch(int penaltyMinutes)
            {
                if (_penalties == null || _penalties.Length==0) return;
                base.PlayMatch(penaltyMinutes);
                if (penaltyMinutes >= 0)
                {
                    totalPenaltyMinutesAll += penaltyMinutes;
                    
                }
            }
        }
    }
}
