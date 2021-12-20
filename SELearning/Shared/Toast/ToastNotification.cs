using System.Timers;

namespace SELearning.Shared.Toast;

public delegate void OnDismissHandler();

public class ToastNotification : IDisposable
{
    public DateTime Created { get; init; } = DateTime.Now;
    public string Title { get; init; }
    public string Body { get; init; }
    public ToastType Type { get; init; }

    private int Duration { get; init; }
    private Timer DismissTimer { get; init; }

    private readonly IList<OnDismissHandler> _onDismissHandlers = new List<OnDismissHandler>();

    public ToastNotification(string title, string body, ToastType type)
    {
        Title = title;
        Body = body;
        Type = type;
    }

    public ToastNotification(string body, ToastType type) : this(body, body, type) {}

    /// <summary>
    /// Creates an auto dismissing toast notification. It will dismiss after the given duration.
    /// </summary>
    /// <param name="title">Title of notification</param>
    /// <param name="body">Title of body</param>
    /// <param name="type">Error level of notification</param>
    /// <param name="duration">Amount of milliseconds that should pass before dismissing</param>
    public ToastNotification(string title, string body, ToastType type, int duration)
        : this(title, body, type)
    {
        Duration = duration;
        DismissTimer = new Timer();
        DismissTimer.Interval = duration;
        DismissTimer.Elapsed += (_, _) => { Dismiss(); };
        DismissTimer.Start();
    }

    public ToastNotification(string body, ToastType type, int duration) : this(body, body, type, duration) {}

    public void AddDismissHandler(OnDismissHandler handler)
    {
        _onDismissHandlers.Add(handler);
    }

    public void Dismiss()
    {
        foreach (var handler in _onDismissHandlers)
        {
            handler.Invoke();
        }
    }

    public void Dispose()
    {
        DismissTimer.Dispose();
        GC.SuppressFinalize(this);
    }

    public static ToastNotification CreateNoAccessToastNotification() =>
        new("Access not available!", "Unfortunately, our authorization handler was not able " +
                                     "to find your access token. Please contact the admin", ToastType.Error, 10_000);

    public static ToastNotification CreateGenericErrorToastNotification(string message) =>
        new("Fatal error!", $"An error occurred: {message}", ToastType.Error, 10_000);

    public static ToastNotification CreateUnauthorized(string message) =>
        new("Unauthorized!", $"You are not authorized to {message}", ToastType.Error, 10_000);

    public static ToastNotification CreateLoggedError(string message)
    {
        Console.WriteLine(message);
        return CreateGenericErrorToastNotification(message);
    }
}
