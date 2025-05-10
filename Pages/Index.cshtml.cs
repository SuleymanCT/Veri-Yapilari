using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CallCenterWebApp.Models;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace CallCenterWebApp.Pages
{
    public class IndexModel : PageModel
    {
        public static RepresentativeGraph repGraph = new RepresentativeGraph();
        public static RepresentativeTree repTree = new RepresentativeTree();
        private static bool initialized = false;

        public Dictionary<string, List<string>> GraphData => repGraph.GetGraph();

        public class Node
        {
            public int id;
            public string name;
            public int waitTime;
            public DateTime enqueuedAt;
            public Node? next;
            public string assignedRep;

            public Node(int id, string name, int waitTime, string assignedRep)
            {
                this.id = id;
                this.name = name;
                this.waitTime = waitTime;
                this.enqueuedAt = DateTime.Now;
                this.assignedRep = assignedRep;
                this.next = null;
            }

            public int GetRemainingTime()
            {
                int elapsed = (int)(DateTime.Now - enqueuedAt).TotalSeconds;
                int remaining = waitTime - elapsed;
                return remaining > 0 ? remaining : 0;
            }
        }

        private static Node? front = null;
        private static Node? rear = null;

        public List<Node> QueueList { get; set; } = new();
        public CustomerHashTable hashTable = new CustomerHashTable(10);
        public string AssignedRepresentative { get; set; } = "Henüz atanmadı";

        public IndexModel()
        {
            if (!initialized)
            {
                // Temsilcileri sadece ilk seferde ekle
                repTree.Insert(1, "Temsilci A");
                repTree.Insert(2, "Temsilci B");
                repTree.Insert(3, "Temsilci C");

                // Bağlantı ağını kur
                var reps = repTree.GetInOrder();
                for (int i = 0; i < reps.Count; i++)
                {
                    for (int j = 0; j < reps.Count; j++)
                    {
                        if (i != j)
                        {
                            repGraph.AddConnection(reps[i], reps[j]);
                        }
                    }
                }

                initialized = true;
            }
        }

        public void OnGet()
        {
            RemoveExpiredCustomers();
            UpdateQueueList();
            UpdateRepresentative();
        }

        public IActionResult OnPost()
        {
            var form = Request.Form;
            string? action = form["action"];
            string? name = form["CustomerName"];
            string? idStr = form["CustomerID"];
            string? timeStr = form["WaitTime"];

            if (action == "add" && int.TryParse(idStr, out int id) && int.TryParse(timeStr, out int wait))
            {
                Enqueue(id, name ?? "Anonim", wait);
                hashTable.Add(id, name ?? "Anonim");
            }
            else if (action == "remove")
            {
                Dequeue();
            }

            RemoveExpiredCustomers();
            UpdateQueueList();
            UpdateRepresentative();

            return RedirectToPage();
        }

        public void Enqueue(int id, string name, int waitTime)
        {
            var rep = repTree.GetNextRepresentative();
            var newNode = new Node(id, name, waitTime, rep);
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

        public void Dequeue()
        {
            if (front != null)
            {
                front = front.next;
                if (front == null)
                    rear = null;
            }
        }

        private void RemoveExpiredCustomers()
        {
            while (front != null && front.GetRemainingTime() <= 0)
            {
                front = front.next;
                if (front == null)
                    rear = null;
            }
        }

        private void UpdateQueueList()
        {
            QueueList.Clear();
            var temp = front;
            while (temp != null)
            {
                QueueList.Add(temp);
                temp = temp.next;
            }
        }

        private void UpdateRepresentative()
        {
            AssignedRepresentative = QueueList.Count > 0
                ? repTree.PeekNextRepresentative()
                : "Henüz atanmadı";
        }

        public int HashTableSize => hashTable.GetSize();

        public List<KeyValuePair<int, string>> GetHashBucket(int index)
        {
            return new List<KeyValuePair<int, string>>(hashTable.GetBucket(index));
        }

        public List<string> GetInOrderRepresentatives() => repTree.GetInOrder();
    }
}
