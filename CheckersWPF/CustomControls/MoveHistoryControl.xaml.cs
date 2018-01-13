using System;

namespace CheckersWPF.CustomControls
{
    public sealed partial class MoveHistoryControl
    {
        public MoveHistoryControl()
        {
            InitializeComponent();
        }

        public event EventHandler OnMoveSelection;
        private void MoveSelected() =>
            OnMoveSelection?.Invoke(this, EventArgs.Empty);

        private void RadioButton_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //var senderElement = sender as RadioButton;
            //var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            //flyoutBase.ShowAt(senderElement);
        }

        private void RadioButton_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            MoveSelected();
        }
    }
}
