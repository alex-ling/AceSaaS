using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Data
{
    public class TreeNode
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string IconCls { get; set; }
        public bool? Checked { get; set; }
        public TreeNodeState? State { get; set; }
        public Dictionary<string, object> Attributes { get; set; }
        public List<TreeNode> Children { get; set; }

        public TreeNode()
        {
            Attributes = new Dictionary<string, object>();
            Children = new List<TreeNode>();
        }
    }
}
