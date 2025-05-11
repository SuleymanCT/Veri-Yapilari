using System;
using System.Collections.Generic;
using System.Threading;

namespace CallCenterApp
{
    class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Customer(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    class Representative
    {
        public string Name { get; set; }
        public bool IsBusy { get; set; }
        public DateTime FreeAt { get; set; }

        public Representative(string name)
        {
            Name = name;
            IsBusy = false;
            FreeAt = DateTime.Now;
        }

        public void AssignCustomer(Customer customer)
        {
            IsBusy = true;
            FreeAt = DateTime.Now.AddSeconds(10);
            Console.WriteLine($"{customer.Name} temsilci {Name}'ye atandı.");
        }

        public void UpdateStatus()
        {
            if (IsBusy && DateTime.Now >= FreeAt)
            {
                IsBusy = false;
                Console.WriteLine($"Temsilci {Name} artık boşta.");
            }
        }
    }

    class CallCenterSimulator
    {
        private Queue<Customer> customerQueue = new Queue<Customer>();
        private List<Representative> reps = new List<Representative>();
        private bool stopRequested = false;
        private Random random = new Random();

        public CallCenterSimulator()
        {
            reps.Add(new Representative("A"));
            reps.Add(new Representative("B"));
            reps.Add(new Representative("C"));
        }

        public void AddCustomer(string name)
        {
            int id = random.Next(1000, 9999);
            var customer = new Customer(id, name);
            customerQueue.Enqueue(customer);
            Console.WriteLine($"{name} sıraya eklendi. (ID: {id})");
        }

        public void AssignCustomers()
        {
            foreach (var rep in reps)
                rep.UpdateStatus();

            foreach (var rep in reps)
            {
                if (!rep.IsBusy && customerQueue.Count > 0)
                {
                    var customer = customerQueue.Dequeue();
                    rep.AssignCustomer(customer);
                }
            }
        }

        public void Run()
        {
            while (!stopRequested || HasWaitingCustomers())
            {
                AssignCustomers();
                Thread.Sleep(500);
            }
        }

        public void Stop()
        {
            stopRequested = true;
        }

        public bool HasWaitingCustomers()
        {
            return customerQueue.Count > 0 || reps.Exists(r => r.IsBusy);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var sim = new CallCenterSimulator();

            Console.WriteLine("10 farklı müşteri ismi girin:");
            int added = 0;
            while (added < 10)
            {
                string name = Console.ReadLine();
                sim.AddCustomer(name);
                added++;
            }

            Thread simThread = new Thread(sim.Run);
            simThread.Start();

            while (sim.HasWaitingCustomers())
                Thread.Sleep(500);

            sim.Stop();
            simThread.Join();

            Console.WriteLine("\nSimülasyon tamamlandı. Çıkmak için bir tuşa basın...");
            Console.ReadKey();
        }
    }
}