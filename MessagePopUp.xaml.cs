using CommunityToolkit.Maui.Views;

namespace SmartCart;

public partial class MessagePopUp : Popup
{
    private readonly Action _onOk;
    private readonly Action _onCancel;

    public MessagePopUp(string message, bool showCancel = false, Action onOk = null, Action onCancel = null)
    {
        InitializeComponent();

        MessageLabel.Text = message;
        _onOk = onOk;
        _onCancel = onCancel;
        CancelButton.IsVisible = showCancel;
    }

    private void OnOkClicked(object sender, EventArgs e)
    {
        _onOk?.Invoke();
        Close();
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        _onCancel?.Invoke();
        Close();
    }
}