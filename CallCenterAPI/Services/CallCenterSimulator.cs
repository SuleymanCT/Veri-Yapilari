using CallCenterAPI.Models;

namespace CallCenterAPI.Services
{
    public class CallCenterSimulator
    {
        private Queue<string> customerQueue = new();
        private List<Representative> representatives = new()
        {
            new Representative("A"),
            new Representative("B"),
            new Representative("C")
        };

        public void AddCustomer(string name)
        {
            if (!customerQueue.Contains(name))
                customerQueue.Enqueue(name);
        }

        public void AssignCustomers()
        {
            foreach (var rep in representatives)
                rep.UpdateStatus();

            foreach (var rep in representatives)
            {
                if (!rep.IsBusy && customerQueue.Count > 0)
                {
                    string customer = customerQueue.Dequeue();
                    rep.AssignCustomer(customer);
                }
            }
        }

        public List<string> GetCustomerNames()
        {
            return customerQueue.ToList();
        }

        public List<RepStatusDto> GetRepresentativeStatuses()
        {
            return representatives.Select(r => new RepStatusDto
            {
                Name = r.Name,
                IsBusy = r.IsBusy,
                CurrentCustomer = r.CurrentCustomerName
            }).ToList();
        }

        public Dictionary<string, TreeNodeDto?> GetRepresentativeTrees()
        {
            return representatives.ToDictionary(
                rep => rep.Name,
                rep => rep.CustomerTree.GetJson()
            );
        }
    }

    public class RepStatusDto
    {
        public string Name { get; set; }
        public bool IsBusy { get; set; }
        public string? CurrentCustomer { get; set; }
    }
}
