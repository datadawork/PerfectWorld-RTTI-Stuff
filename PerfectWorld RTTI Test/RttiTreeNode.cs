using System.Windows.Forms;

namespace PerfectWorld_RTTI_Test {
    public class RttiTreeNode: TreeNode {

        public RttiObject RttiObject { get; set; }

        public RttiTreeNode(RttiObject rttiObject) {
            Text = rttiObject.Offset > 0 ? $"{rttiObject.Offset:X4} {rttiObject.Name}" : rttiObject.Name;
            RttiObject = rttiObject;
            Nodes.Add(new TreeNode(Text) {Name = "dummy"});
        }

        public RttiTreeNode() {
            Nodes.Add(new TreeNode { Name = "dummy" });
        }

        public string GetPointerPath() {
            var parent = (RttiTreeNode)Parent;
            var path = parent == null ?
                $"0x{RttiObject.BaseAddress.ToInt32():X8}" :
                $"0x{RttiObject.Offset:X}";
            while (parent != null) {
                path = parent.RttiObject.Offset != 0 ?
                    $"0x{parent.RttiObject.Offset:X} + {path}" :
                    $"0x{parent.RttiObject.BaseAddress.ToInt32():X8} + {path}";
                parent = (RttiTreeNode)parent.Parent;
            }
            return path;
        }
    }
}