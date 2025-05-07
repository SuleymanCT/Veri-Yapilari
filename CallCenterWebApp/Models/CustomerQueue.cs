namespace CallCenterWebApp.Models
{
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
        }

        public void Dequeue()
        {
            if (front != null)
                front = front.next;
        }

        public Node PeekFront() => front;

        public bool HasCustomers() => front != null;

        public List<Node> GetAll()
        {
            List<Node> list = new();
            Node temp = front;
            while (temp != null)
            {
                list.Add(temp);
                temp = temp.next;
            }
            return list;
        }
    }
}
