using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace PerfectWorld_RTTI_Test
{
    public partial class FormMain : Form {

        public FormMain() {
            InitializeComponent();
        }
        
        private void FormMain_Load(object sender, EventArgs e) {
            Logging.SetLogControl(textBoxLog);
            if (!Core.Load()) {
                Logging.Log("Failed to Load Core");
                return;
            }
            Logging.Log($"Process: {Core.Memory.Process.ProcessName}");
            var baseAddr = Core.GetBaseAddress(true);
            if(baseAddr != IntPtr.Zero) textBoxAddress.Text = $"{baseAddr.ToInt32():X8}";
        }
        
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e) {
            if(Core.isLoaded) Core.Unload();
        }

        private void buttonStartStop_Click(object sender, EventArgs e) {
            if (!bWorkerMain.IsBusy && buttonStartStop.Text != "Stop") {
                IntPtr addr;
                if (int.TryParse(textBoxAddress.Text, out int addrInt)) addr = (IntPtr) addrInt;
                else addr = Core.GetBaseAddress();
                bWorkerMain.RunWorkerAsync(addr);
                buttonStartStop.Text = "Stop";
            } else {
                if (bWorkerMain.CancellationPending) return;
                bWorkerMain.CancelAsync();
            }
        }

        private void bWorkerMain_DoWork(object sender, DoWorkEventArgs e) {
            if (e.Argument is null) {
                e.Result = null;
                return;
            }
            if (e.Argument is IntPtr addr) {
                var oList = new List<RttiObject>();
                for (var i = 0; i < 0x1FFF; i += 4) {
                    var x = new RttiObject(addr + i);
                    if (!x.isValid()) continue;
                    oList.Add(x);
                }
                e.Result = oList;
                return;
            }
            e.Result = null;
        }

        private void bWorkerMain_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            buttonStartStop.Text = "Start";
            var list = e.Result as List<RttiObject>;
            if (list == null) return;
            treeViewClassTree.Nodes.Clear();
            foreach (var obj in list) {
                treeViewClassTree.Nodes.Add(new RttiTreeNode(obj));
            }
        }

        private List<RttiObject> GetChildObjects(RttiObject parentObj) {
            var oList = new List<RttiObject>();
            for (var i = 0; i < 0x1FFF; i += 4) {
                var x = new RttiObject(parentObj.BaseAddress + i, i);
                oList.Add(x);
            }
            return oList.Where(r=>r.isValid()).ToList();
        }

        private void treeViewClassTree_BeforeExpand(object sender, TreeViewCancelEventArgs e) {
            var node = (RttiTreeNode)e.Node;
            if (node?.RttiObject == null) {
                node?.Nodes.Clear();
                return;
            }
            if(!node.Nodes.ContainsKey("dummy")) return;
            node.Nodes.Clear();
            foreach (var child in GetChildObjects(node.RttiObject)) {
                node.Nodes.Add(new RttiTreeNode(child));
            }
            Logging.Log(node.GetPointerPath());
        }
    }
}
