using System;
using System.Collections.Generic;


namespace CallCenterWebApp.Models
{
    public class RepresentativeNode
    {
        public int priority;
        public string name;
        public RepresentativeNode left;
        public RepresentativeNode right;

        public RepresentativeNode(int priority, string name)
        {
            this.priority = priority;
            this.name = name;
            this.left = null!;
            this.right = null!;
        }
    }
}