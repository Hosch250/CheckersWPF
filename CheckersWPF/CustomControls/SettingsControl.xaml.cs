using CheckersWPF.VMs;

namespace CheckersWPF.CustomControls
{
    public sealed partial class SettingsControl
    {
        public SettingsControl()
        {
            InitializeComponent();
            DataContext = new SettingsViewModel();
        }
    }
}
