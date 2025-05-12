namespace CallCenterAPI.Models
{
    public class TreeNodeDto
    {
        public int Id { get; set; }
        public TreeNodeDto? Left { get; set; }
        public TreeNodeDto? Right { get; set; }
    }

}
