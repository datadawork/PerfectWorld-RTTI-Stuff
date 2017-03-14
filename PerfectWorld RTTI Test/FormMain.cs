using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
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
                bWorkerMain.RunWorkerAsync();
                buttonStartStop.Text = "Stop";
            } else {
                if (bWorkerMain.CancellationPending) return;
                bWorkerMain.CancelAsync();
            }
        }

        private void bWorkerMain_DoWork(object sender, DoWorkEventArgs e) {
            //while(!bWorkerMain.CancellationPending) Thread.Sleep(10);
            var addr = Core.GetBaseAddress();
            if (e.Argument != null) addr = (IntPtr)e.Argument;
            Logging.Clear();
            var baseObj = new RttiObject(addr);
            Logging.Log($"Base: {baseObj.Name}");

            addr = Core.Memory.Read<IntPtr>(addr);
            var oList = new List<RttiObject>();
            for (var i = 0; i < 0x1FFF; i += 4) {
                var x = new RttiObject(addr + i);
                if(!x.isValid()) continue;
                oList.Add(x);
            }
            e.Result = oList;
        }

        private void bWorkerMain_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            buttonStartStop.Text = "Start";
            var oList = e.Result as List<RttiObject>;
            if (oList == null) return;
            foreach (var o in oList) {
                Logging.Log($"{o.Name}");
                foreach (var baseClass in o.BaseClassArray) {
                    Logging.Log($"\t{baseClass.Name}");
                }
            }
        }

    }
}
