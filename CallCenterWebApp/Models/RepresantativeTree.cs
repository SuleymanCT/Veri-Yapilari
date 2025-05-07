namespace CallCenterWebApp.Models
{
    public class RepresentativeTree
    {
        private RepresentativeNode root;

        public RepresentativeTree()
        {
            root = null!;
        }

        public void Insert(int priority, string name)
        {
            root = InsertRec(root, priority, name);
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

        public string GetLowestPriorityRep()
        {
            if (root == null)
                return "Temsilci yok";

            RepresentativeNode current = root;
            while (current.left != null)
                current = current.left;

            return current.name;
        }
    }
}
