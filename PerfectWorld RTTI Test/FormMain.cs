using System;
using System.ComponentModel;
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

            for (var i = 0; i < 0x1FFF; i += 4) {
                var x = new RttiObject(addr+i);
                if(!x.isValid()) continue;
                Logging.Log($"{x.ObjectLocator.Type.Name}");
            }
        }

        private void bWorkerMain_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            buttonStartStop.Text = "Start";
        }

    }
}
