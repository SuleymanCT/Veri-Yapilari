namespace CallCenterAPI.Models
{
    public class GraphRepresentative
    {
        public string Name { get; set; }
        public bool IsBusy { get; set; }
        private DateTime FreeAt;

        public GraphRepresentative(string name)
        {
            Name = name;
            IsBusy = false;
            FreeAt = DateTime.Now;
        }

        public void AssignCustomer(Customer customer, int duration)
        {
            IsBusy = true;
            FreeAt = DateTime.Now.AddSeconds(duration);
            Console.WriteLine($"[GRAPH] {customer.Name} temsilci {Name}'ye atandı. ({duration}s)");
        }

        public void UpdateStatus()
        {
            if (IsBusy && DateTime.Now >= FreeAt)
            {
                IsBusy = false;
                Console.WriteLine($"[GRAPH] Temsilci {Name} boşaldı.");
            }
        }
    }
}
