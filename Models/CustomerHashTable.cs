using System;
using System.Collections.Generic;

namespace CallCenterWebApp.Models
{
    public class CustomerHashTable
    {
        private LinkedList<KeyValuePair<int, string>>[] table;
        private int size;

        public CustomerHashTable(int size)
        {
            this.size = size;
            table = new LinkedList<KeyValuePair<int, string>>[size];
        }

        private int GetHash(int key) => key % size;

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

        public int GetSize() => size;

        public LinkedList<KeyValuePair<int, string>> GetBucket(int index)
        {
            if (index < 0 || index >= size) return new LinkedList<KeyValuePair<int, string>>();
            return table[index] ?? new LinkedList<KeyValuePair<int, string>>();
        }
    }
}
