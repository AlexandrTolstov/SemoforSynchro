using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SemoforSynchro
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 20; i++)
            {
                Reader reader = new Reader(i);
            }
        }
    }
    public static class Count
    {
        public static int Num { get; set; } = 0;
    }
    class Reader
    {
        static Semaphore sem = new Semaphore(4, 4);
        static object locker = new object();
        Thread myThread;

        public Reader(int i)
        {
            myThread = new Thread(Read);
            myThread.Name = "Читатель " + i.ToString();
            myThread.Start();
        }

        private void Read()
        {
            for (int i = 0; i < 3; i++)
            {
                sem.WaitOne();
                lock (locker)
                {
                    Count.Num++;
                    Console.WriteLine("{0} входит в библиотеку; В библиотеке: {1}", Thread.CurrentThread.Name, Count.Num);
                    Console.WriteLine("{0} читает", Thread.CurrentThread.Name);
                }
                Thread.Sleep(100);
                lock (locker)
                {
                    Count.Num--;
                    Console.WriteLine("{0} покидает библиотеку; В библиотеке: {1}", Thread.CurrentThread.Name, Count.Num);
                    Console.WriteLine("--------------------------------------");
                }
                sem.Release();
                Thread.Sleep(1000);
            }
        }
    }
}
