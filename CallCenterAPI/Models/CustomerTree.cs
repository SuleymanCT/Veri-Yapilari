using CallCenterAPI.Models;

namespace CallCenterAPI.Models
{
    public class CustomerNode
    {
        public int Id;
        public CustomerNode? Left;
        public CustomerNode? Right;

        public CustomerNode(int id)
        {
            Id = id;
            Left = Right = null;
        }
    }

    public class CustomerTree
    {
        public CustomerNode? Root;

        public void Insert(int id)
        {
            Root = InsertRec(Root, id);
        }

        private CustomerNode InsertRec(CustomerNode? node, int id)
        {
            if (node == null)
                return new CustomerNode(id);

            if (id < node.Id)
                node.Left = InsertRec(node.Left, id);
            else
                node.Right = InsertRec(node.Right, id);

            return node;
        }

        public TreeNodeDto? GetJson()
        {
            return BuildJson(Root);
        }

        private TreeNodeDto? BuildJson(CustomerNode? node)
        {
            if (node == null) return null;

            return new TreeNodeDto
            {
                Id = node.Id,
                Left = BuildJson(node.Left),
                Right = BuildJson(node.Right)
            };
        }
    }
}
