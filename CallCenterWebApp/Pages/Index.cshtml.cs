using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CallCenterWebApp.Models;

namespace CallCenterWebApp.Pages
{
    public class IndexModel : PageModel
    {
        // --- Veri Yapıları ---
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

        // Kuyruk
        static Node? front = null;
        static Node? rear = null;

        // Kuyruğun görsel listesi
        public List<Node> QueueList = new();

        // --- HashTable ---
        public CustomerHashTable hashTable = new CustomerHashTable(10);

        // --- Tree ---
        public RepresentativeTree repTree = new RepresentativeTree();

        // Atanan temsilci
        public string AssignedRepresentative { get; set; } = "Henüz atanmadı";

        public IndexModel()
        {
            // Temsilcileri ekle
            repTree.Insert(1, "Temsilci A");
            repTree.Insert(3, "Temsilci B");
            repTree.Insert(2, "Temsilci C");
        }

        public void OnGet()
        {
            UpdateQueueList();
            UpdateRepresentative();
        }

        public void OnPost(string action, int CustomerID, string CustomerName, int WaitTime)
        {
            if (action == "add")
            {
                Enqueue(CustomerID, CustomerName, WaitTime);
                hashTable.Add(CustomerID, CustomerName);
            }
            else if (action == "remove")
            {
                Dequeue();
            }

            UpdateQueueList();
            UpdateRepresentative();
        }

        // Kuyruğa ekleme
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
        }

        // Kuyruktan çıkarma
        public void Dequeue()
        {
            if (front != null)
            {
                front = front.next;
                if (front == null)
                    rear = null;
            }
        }

        // Kuyruk listesini güncelle
        private void UpdateQueueList()
        {
            QueueList.Clear();
            Node? temp = front;
            while (temp != null)
            {
                QueueList.Add(temp);
                temp = temp.next;
            }
        }

        // Temsilciyi güncelle
        private void UpdateRepresentative()
        {
            if (QueueList.Count > 0)
            {
                AssignedRepresentative = repTree.GetLowestPriorityRep();
            }
            else
            {
                AssignedRepresentative = "Henüz atanmadı";
            }
        }
    }
}
