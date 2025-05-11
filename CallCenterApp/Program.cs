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
        private Dictionary<int, Customer> customerMap = new Dictionary<int, Customer>();
        private Dictionary<string, Customer> customerByName = new Dictionary<string, Customer>();
        private Dictionary<string, List<Customer>> repHistories = new Dictionary<string, List<Customer>>();
        private HashSet<string> customerNameSet = new HashSet<string>();
        private List<Representative> reps = new List<Representative>();
        private bool stopRequested = false;

        public CallCenterSimulator()
        {
            reps.Add(new Representative("A"));
            reps.Add(new Representative("B"));
            reps.Add(new Representative("C"));
        }

        public void AddCustomer(string name)
        {
            if (customerNameSet.Contains(name))
            {
                Console.WriteLine($"{name} zaten sırada, tekrar eklenmedi.");
                return;
            }

            var customer = new Customer(customerMap.Count + 1, name);
            customerQueue.Enqueue(customer);
            customerMap[customer.Id] = customer;
            customerByName[name] = customer;
            customerNameSet.Add(name);

            Console.WriteLine($"{name} sıraya eklendi.");
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

                    if (!repHistories.ContainsKey(rep.Name))
                        repHistories[rep.Name] = new List<Customer>();

                    repHistories[rep.Name].Add(customer);
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

        public void PrintRepresentativeHistories()
        {
            Console.WriteLine("\n--- Temsilci Müşteri Geçmişleri ---");
            foreach (var entry in repHistories)
            {
                Console.WriteLine($"Temsilci {entry.Key} şu müşterilere hizmet verdi:");
                foreach (var customer in entry.Value)
                {
                    Console.WriteLine($" - {customer.Name} (ID: {customer.Id})");
                }
            }
        }

        public int GetCustomerCount()
        {
            return customerMap.Count;
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
                int beforeCount = sim.GetCustomerCount();
                sim.AddCustomer(name);
                if (sim.GetCustomerCount() > beforeCount)
                    added++;
            }

            // Simülasyonu başlat
            Thread simThread = new Thread(sim.Run);
            simThread.Start();

            // Kuyruk ve işlemler bitene kadar bekle
            while (sim.HasWaitingCustomers())
                Thread.Sleep(500);

            // Simülasyonu düzgün durdur
            sim.Stop();
            simThread.Join();

            // Temsilci müşteri geçmişini yazdır
            sim.PrintRepresentativeHistories();

            Console.WriteLine("\nSimülasyon tamamlandı. Çıkmak için bir tuşa basın...");
            Console.ReadKey();
        }
    }
}
