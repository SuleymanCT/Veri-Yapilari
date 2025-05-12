namespace CallCenterAPI.Models
{
    public class Graph
    {
        public Dictionary<string, List<string>> Connections = new();

        public void AddEdge(string from, string to)
        {
            if (!Connections.ContainsKey(from))
                Connections[from] = new List<string>();
            Connections[from].Add(to);
        }

        public List<string> GetConnections(string name)
        {
            return Connections.ContainsKey(name) ? Connections[name] : new List<string>();
        }
    }

}
