using System;
using System.Threading;

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
            this.next = null;
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
}

class Program
{
    static void Main(string[] args)
    {
        var queue = new CallCenterQueueSimulation.CustomerQueue();

        queue.Enqueue(1001, "Müşteri 1", 5);
        queue.Enqueue(1002, "Müşteri 2", 3);
        queue.Enqueue(1003, "Müşteri 3", 7);

        queue.Display();

        while (true)
        {
            if (!queue.HasCustomers())
            {
                Console.WriteLine("Kuyruk boş. Simülasyon sona erdi.");
                break;
            }

            var customer = queue.PeekFront();
            Console.WriteLine($"\nMüşteri {customer.id} - {customer.name} işlemde ({customer.waitTime} sn)");

            Thread.Sleep(customer.waitTime * 1000);

            queue.Dequeue();
            queue.Display();
        }
    }
}
