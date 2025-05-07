namespace CallCenterWebApp.Models
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
}
