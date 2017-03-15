using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime;
using System.Windows.Forms;

namespace PerfectWorld_RTTI_Test
{
    public partial class FormMain : Form {

        private RttiTreeNode RootNode = new RttiTreeNode();
        private int MaxOffset = 0x1FFFF;

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
            if(bWorkerMain.IsBusy) bWorkerMain.CancelAsync();
        }
        
        private void textBoxLog_TextChanged(object sender, EventArgs e) {
            textBoxLog.SelectionStart = textBoxLog.Text.Length;
            textBoxLog.ScrollToCaret();
        }

        private void buttonStartStop_Click(object sender, EventArgs e) {
            if(!Core.isLoaded) return;
            if (!bWorkerMain.IsBusy && buttonStartStop.Text != "Stop") {
                IntPtr addr;
                if (int.TryParse(textBoxAddress.Text, out int addrInt)) addr = (IntPtr) addrInt;
                else addr = Core.GetBaseAddress();

                if (textBoxOffset.Text.Contains("0x")) textBoxOffset.Text = textBoxOffset.Text.Replace("0x", "");
                if (int.TryParse(textBoxOffset.Text, NumberStyles.HexNumber, new NumberFormatInfo(), out int maxoff))
                    MaxOffset = maxoff;
                else {
                    MaxOffset = 0x1FFFF;
                    textBoxOffset.Text = "1FFFF";
                }
                RootNode = new RttiTreeNode {
                    RttiObject = new RttiObject(addr, -1, false),
                    Text = $"{addr.ToInt32():X8} Base Address"
                };
                treeViewClassTree.Nodes.Clear();
                treeViewClassTree.Nodes.Add(RootNode);
                RootNode.Expand();
                buttonStartStop.Text = "Stop";
            } else {
                if (bWorkerMain.CancellationPending) return;
                bWorkerMain.CancelAsync();
            }
        }

        private void bWorkerMain_DoWork(object sender, DoWorkEventArgs e) {
            if(!Core.isLoaded) return;
            if (e.Argument is null) {
                e.Result = null;
                return;
            }
            if (e.Argument is RttiTreeNode node) {
                var nodeList = new List<RttiTreeNode>();
                Logging.Log($"Searching {MaxOffset / 4} addresses ...");
                var sw = Stopwatch.StartNew();
                for (var i = 0; i < MaxOffset; i += 4) {
                    if (bWorkerMain.CancellationPending) break;
                    bWorkerMain.ReportProgress(i);
                    var x = new RttiObject(node.RttiObject.BaseAddress + i, i);
                    if (x.isValid()) nodeList.Add(new RttiTreeNode(x));
                }
                sw.Stop();
                Logging.Log($"Found {nodeList.Count} classes");
                Logging.Log($"Time: {sw.Elapsed.TotalMinutes:00}:{sw.Elapsed.Seconds:00}.{sw.Elapsed.Milliseconds:000}");
                Logging.Log($"{new string('-', 50)}");
                bWorkerMain.ReportProgress(MaxOffset);
                e.Result = new object[]{node, nodeList};
                return;
            }
            e.Result = null;
        }

        private void bWorkerMain_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            buttonStartStop.Text = "Init";
            var res = e.Result as object[];
            if (res?.Length != 2) return;
            var node = (RttiTreeNode)res[0];
            var list = (List<RttiTreeNode>)res[1];
            foreach (var treeNode in list) {
                node.Nodes.Add(treeNode);
            }
            node.FirstNode.Remove();
            node.Expand();
        }

        private void treeViewClassTree_BeforeExpand(object sender, TreeViewCancelEventArgs e) {
            var node = (RttiTreeNode)e.Node;
            if (node?.RttiObject == null) {
                node?.Nodes.Clear();
                return;
            }

            //Logging.Log(node.GetPointerPath());
            if (!node.Nodes.ContainsKey("dummy") || bWorkerMain.IsBusy) return;
            node.FirstNode.Text = "Loading ...";
            buttonStartStop.Text = "Stop";
            bWorkerMain.RunWorkerAsync(node);
        }

        private void bWorkerMain_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            var progress = decimal.ToInt32((decimal)e.ProgressPercentage / MaxOffset * 100);
            progressBarWorker.Value = progress;
        }
    }
}
