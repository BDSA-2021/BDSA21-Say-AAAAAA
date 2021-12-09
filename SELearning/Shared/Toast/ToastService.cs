namespace SELearning.Shared.Toast;

public delegate void OnChangeHandler();

public class ToastService
{
    public IList<ToastNotification> Notifications { get; init; } = new List<ToastNotification>();
    private IList<OnChangeHandler> _onChangeHandlers = new List<OnChangeHandler>();

    public ToastService()
    {
    }

    public void AddToast(ToastNotification notification)
    {
        Notifications.Add(notification);
        notification.AddDismissHandler(() =>
        {
            Notifications.Remove(notification);
            NotifyHandlers();
        });

        NotifyHandlers();
    }

    public void AddHandler(OnChangeHandler handler)
    {
        _onChangeHandlers.Add(handler);
    }

    private void NotifyHandlers()
    {
        foreach (var handler in _onChangeHandlers)
        {
            handler.Invoke();
        }
    }
}