using CallCenterAPI.Models;

namespace CallCenterAPI.Models
{
    public class Representative
    {
        public string Name { get; set; }
        public bool IsBusy { get; set; }
        public DateTime FreeAt { get; set; }
        public string? CurrentCustomerName { get; set; }
        public CustomerTree CustomerTree { get; set; } = new CustomerTree();

        public Representative(string name)
        {
            Name = name;
            IsBusy = false;
            FreeAt = DateTime.Now;
        }

        public void AssignCustomer(string name, int durationSeconds = 10)
        {
            IsBusy = true;
            FreeAt = DateTime.Now.AddSeconds(durationSeconds);
            CurrentCustomerName = name;

            int customerId = name.GetHashCode(); // örnek ID üretimi
            CustomerTree.Insert(Math.Abs(customerId % 10000));
        }

        public void UpdateStatus()
        {
            if (IsBusy && DateTime.Now >= FreeAt)
            {
                IsBusy = false;
                CurrentCustomerName = null;
            }
        }
    }
}
