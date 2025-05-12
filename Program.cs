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
        public CustomerTree CustomerTree { get; set; } = new CustomerTree();

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
            CustomerTree.Insert(customer.Id);
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
        private Random random = new Random();

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

            int id;
            do
            {
                id = random.Next(1000, 9999);
            } while (customerMap.ContainsKey(id));

            var customer = new Customer(id, name);
            customerQueue.Enqueue(customer);
            customerMap[id] = customer;
            customerByName[name] = customer;
            customerNameSet.Add(name);

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

        public void PrintAllCustomerTrees()
        {
            Console.WriteLine("\n--- Temsilci Müşteri ID Ağaçları (BST) ---");
            foreach (var rep in reps)
            {
                Console.WriteLine($"
Temsilci {rep.Name}:");
                rep.CustomerTree.PrintInOrder();
            }
        }

        public int GetCustomerCount()
        {
            return customerMap.Count;
        }

        public Dictionary<int, Customer> GetCustomerMap()
        {
            return customerMap;
        }

        public HashSet<string> GetCustomerNames()
        {
            return customerNameSet;
        }
    }

    class CustomerNode
    {
        public int Id;
        public CustomerNode Left;
        public CustomerNode Right;

        public CustomerNode(int id)
        {
            Id = id;
            Left = Right = null;
        }
    }

    class CustomerTree
    {
        public CustomerNode Root;

        public void Insert(int id)
        {
            Root = InsertRec(Root, id);
        }

        private CustomerNode InsertRec(CustomerNode node, int id)
        {
            if (node == null)
                return new CustomerNode(id);

            if (id < node.Id)
                node.Left = InsertRec(node.Left, id);
            else
                node.Right = InsertRec(node.Right, id);

            return node;
        }

        public void PrintInOrder()
        {
            Console.Write("ID sıralı: ");
            PrintInOrderRec(Root);
            Console.WriteLine();
        }

        private void PrintInOrderRec(CustomerNode node)
        {
            if (node == null) return;
            PrintInOrderRec(node.Left);
            Console.Write($"{node.Id} ");
            PrintInOrderRec(node.Right);
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

            Thread simThread = new Thread(sim.Run);
            simThread.Start();

            while (sim.HasWaitingCustomers())
                Thread.Sleep(500);

            sim.Stop();
            simThread.Join();

            sim.PrintRepresentativeHistories();
            sim.PrintAllCustomerTrees();

            Console.WriteLine("\nSimülasyon tamamlandı. Çıkmak için bir tuşa basın...");
            Console.ReadKey();
        }
    }
}