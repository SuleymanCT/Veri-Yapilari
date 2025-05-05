using System;
using System.Threading;
using System.Collections.Generic; // Hash için gerekli

namespace CallCenterQueueSimulation
{
    public class Node
    {
        public int id;
        public string name;
        public int waitTime;
        public Node next;

        public Node(int id, string name, int waitTime)
        {
            this.id = id;
            this.name = name;
            this.waitTime = waitTime;
            this.next = null!;
        }
    }

    public class CustomerQueue
    {
        private Node front;
        private Node rear;

        public CustomerQueue()
        {
            front = null!;
            rear = null!;
        }

        public void Enqueue(int id, string name, int waitTime)
        {
            Node newNode = new Node(id, name, waitTime);

            if (rear == null)
            {
                front = rear = newNode;
            }
            else
            {
                rear.next = newNode;
                rear = newNode;
            }

            Console.WriteLine($"Müşteri {id} - {name} ({waitTime} sn) kuyruğa eklendi.");
        }

        public void Dequeue()
        {
            if (front == null)
            {
                Console.WriteLine("Kuyruk boş.");
                return;
            }

            Console.WriteLine($"Müşteri {front.id} - {front.name} kuyruktan çıkarıldı.");
            front = front.next;

            if (front == null)
            {
                rear = null;
            }
        }

        public Node PeekFront()
        {
            return front;
        }

        public bool HasCustomers()
        {
            return front != null;
        }

        public void Display()
        {
            if (front == null)
            {
                Console.WriteLine("Kuyruk boş.");
                return;
            }

            Console.WriteLine("Şu anki kuyruk:");
            Node temp = front;
            while (temp != null)
            {
                Console.WriteLine($"ID: {temp.id} | İsim: {temp.name} | Bekleme: {temp.waitTime} sn");
                temp = temp.next;
            }
        }
    }

    public class CustomerHashTable
    {
        private LinkedList<KeyValuePair<int, string>>[] table;
        private int size;

        public CustomerHashTable(int size)
        {
            this.size = size;
            table = new LinkedList<KeyValuePair<int, string>>[size];
        }

        private int GetHash(int key)
        {
            return key % size;
        }

        public void Add(int id, string name)
        {
            int hash = GetHash(id);
            if (table[hash] == null)
                table[hash] = new LinkedList<KeyValuePair<int, string>>();

            table[hash].AddLast(new KeyValuePair<int, string>(id, name));
        }

        public string? Get(int id)
        {
            int hash = GetHash(id);
            if (table[hash] != null)
            {
                foreach (var pair in table[hash])
                {
                    if (pair.Key == id)
                        return pair.Value;
                }
            }
            return null;
        }
    }

    // 🔵 Tree Sınıfı (Temsilciler için)
    public class RepresentativeNode
    {
        public int priority;
        public string name;
        public RepresentativeNode left;
        public RepresentativeNode right;

        public RepresentativeNode(int priority, string name)
        {
            this.priority = priority;
            this.name = name;
            this.left = null!;
            this.right = null!;
        }
    }

    public class RepresentativeTree
    {
        private RepresentativeNode root;

        public RepresentativeTree()
        {
            root = null!;
        }

        public void Insert(int priority, string name)
        {
            root = InsertRec(root, priority, name);
        }

        private RepresentativeNode InsertRec(RepresentativeNode node, int priority, string name)
        {
            if (node == null)
            {
                return new RepresentativeNode(priority, name);
            }

            if (priority < node.priority)
            {
                node.left = InsertRec(node.left, priority, name);
            }
            else
            {
                node.right = InsertRec(node.right, priority, name);
            }

            return node;
        }

        public string GetLowestPriorityRep()
        {
            if (root == null)
                return "Temsilci yok";

            RepresentativeNode current = root;
            while (current.left != null)
            {
                current = current.left;
            }
            return current.name;
        }
    }

}

class Program
{
    static void Main(string[] args)
    {
        var queue = new CallCenterQueueSimulation.CustomerQueue();
        var hashTable = new CallCenterQueueSimulation.CustomerHashTable(10);
        var repTree = new CallCenterQueueSimulation.RepresentativeTree();

        // Temsilciler ekleniyor
        repTree.Insert(1, "Temsilci A");
        repTree.Insert(3, "Temsilci B");
        repTree.Insert(2, "Temsilci C");

        queue.Enqueue(1001, "Müşteri 1", 5);
        hashTable.Add(1001, "Müşteri 1");

        queue.Enqueue(1002, "Müşteri 2", 3);
        hashTable.Add(1002, "Müşteri 2");

        queue.Enqueue(1003, "Müşteri 3", 7);
        hashTable.Add(1003, "Müşteri 3");

        queue.Display();

        Console.WriteLine("\nHashTable Testi -> ID 1002 olan müşteri: " + hashTable.Get(1002) + "\n");

        while (true)
        {
            if (!queue.HasCustomers())
            {
                Console.WriteLine("Kuyruk boş. Simülasyon sona erdi.");
                break;
            }

            var customer = queue.PeekFront();
            var assignedRep = repTree.GetLowestPriorityRep();

            Console.WriteLine($"\nMüşteri {customer.id} - {customer.name} işlemde ({customer.waitTime} sn)");
            Console.WriteLine($"Atanan Temsilci: {assignedRep}");

            for (int i = customer.waitTime; i > 0; i--)
            {
                Console.WriteLine($"{i}...");
                Thread.Sleep(1000);
            }

            queue.Dequeue();
            queue.Display();
        }
    }
}
