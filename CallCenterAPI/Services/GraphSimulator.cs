using CallCenterAPI.Models;

namespace CallCenterAPI.Services
{
    public class GraphSimulator
    {
        public Graph graph = new();
        private List<(string from, string to, string customerName)> recentTransfers = new();

        private List<GraphRepresentative> reps = new()
        {
            new GraphRepresentative("G-A"),
            new GraphRepresentative("G-B"),
            new GraphRepresentative("G-C")
        };

        public GraphSimulator()
        {
            graph.AddEdge("G-A", "G-B");
            graph.AddEdge("G-B", "G-C");
            graph.AddEdge("G-C", "G-A");
        }

        public void AddCustomer(Customer customer)
        {
            queue.Enqueue(customer);
            Console.WriteLine($"[GRAPH] {customer.Name} (ID: {customer.Id}) graph kuyruğuna eklendi.");
        }

        private Queue<Customer> queue = new();
        private Random rand = new Random();
        private bool stopRequested = false;

        public void Run()
        {
            while (!stopRequested || queue.Count > 0 || reps.Exists(r => r.IsBusy))
            {
                foreach (var rep in reps)
                    rep.UpdateStatus();

                foreach (var rep in reps)
                {
                    if (!rep.IsBusy && queue.Count > 0)
                    {
                        var customer = queue.Dequeue();
                        rep.AssignCustomer(customer, rand.Next(5, 11));
                    }
                    else if (rep.IsBusy && queue.Count > 0)
                    {
                        foreach (var neighbor in graph.GetConnections(rep.Name))
                        {
                            var neighborRep = reps.Find(r => r.Name == neighbor);
                            if (neighborRep != null && !neighborRep.IsBusy)
                            {
                                var customer = queue.Dequeue();
                                Console.WriteLine($"[GRAPH] {rep.Name} meşgul. {customer.Name} yönlendirildi → {neighborRep.Name}");
                                recentTransfers.Add((rep.Name, neighborRep.Name, customer.Name));
                                neighborRep.AssignCustomer(customer, rand.Next(5, 11));
                                break;
                            }
                        }
                    }
                }

                Thread.Sleep(500);
            }
        }

        public void Stop()
        {
            stopRequested = true;
        }

        public Dictionary<string, List<string>> GetGraphStructure()
        {
            return graph.Connections;
        }

        public List<(string from, string to, string customerName)> GetAndClearTransfers()
        {
            var copy = new List<(string, string, string)>(recentTransfers);
            recentTransfers.Clear();
            return copy;
        }
    }
}