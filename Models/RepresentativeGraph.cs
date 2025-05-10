using System;
using System.Collections.Generic;

namespace CallCenterWebApp.Models
{
    public class RepresentativeGraph
    {
        private Dictionary<string, List<string>> graph = new();

        public RepresentativeGraph()
        {
            // Boş başlangıç, dinamik bağlantılar IndexModel içinde eklenecek
        }

        public void AddConnection(string from, string to)
        {
            if (!graph.ContainsKey(from))
                graph[from] = new List<string>();

            if (!graph[from].Contains(to))
                graph[from].Add(to);
        }

        public List<string> GetConnections(string representative)
        {
            return graph.ContainsKey(representative) ? graph[representative] : new List<string>();
        }

        public Dictionary<string, List<string>> GetGraph()
        {
            return graph;
        }
    }
}
