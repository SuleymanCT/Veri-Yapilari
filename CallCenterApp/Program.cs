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
        public Queue<Customer> History = new Queue<Customer>();

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
            History.Enqueue(customer);
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
        private List<Representative> reps = new List<Representative>();

        public CallCenterSimulator()
        {
            reps.Add(new Representative("A"));
            reps.Add(new Representative("B"));
            reps.Add(new Representative("C"));
        }

        public void AddCustomer(string name)
        {
            var customer = new Customer(customerMap.Count + 1, name);
            customerQueue.Enqueue(customer);
            customerMap[customer.Id] = customer;
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
                }
            }
        }

        public void Run()
        {
            while (true)
            {
                AssignCustomers();
                Thread.Sleep(500);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var sim = new CallCenterSimulator();

            Console.WriteLine("10 müşteri ismi girin:");
            for (int i = 0; i < 10; i++)
            {
                string name = Console.ReadLine();
                sim.AddCustomer(name);
            }

            sim.Run();
        }
    }
}
