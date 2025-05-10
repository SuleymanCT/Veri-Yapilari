using System;
using System.Collections.Generic;

namespace CallCenterWebApp.Models
{
    public class RepresentativeTree
    {
        private RepresentativeNode root;
        private Queue<string> roundRobinQueue = new();

        public RepresentativeTree()
        {
            root = null!;
        }

        public void Insert(int priority, string name)
        {
            root = InsertRec(root, priority, name);
            roundRobinQueue.Enqueue(name);
        }

        private RepresentativeNode InsertRec(RepresentativeNode node, int priority, string name)
        {
            if (node == null)
                return new RepresentativeNode(priority, name);

            if (priority < node.priority)
                node.left = InsertRec(node.left, priority, name);
            else
                node.right = InsertRec(node.right, priority, name);

            return node;
        }

        public string GetNextRepresentative()
        {
            if (roundRobinQueue.Count == 0)
                return "Temsilci yok";

            string rep = roundRobinQueue.Dequeue();
            roundRobinQueue.Enqueue(rep);
            return rep;
        }

        public string PeekNextRepresentative()
        {
            return roundRobinQueue.Count > 0 ? roundRobinQueue.Peek() : "Temsilci yok";
        }

        public List<string> GetInOrder()
        {
            List<string> result = new();
            InOrder(root, result);
            return result;
        }

        private void InOrder(RepresentativeNode node, List<string> list)
        {
            if (node == null) return;
            InOrder(node.left, list);
            list.Add(node.name);
            InOrder(node.right, list);
        }
    }
}
