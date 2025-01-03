namespace MonkeyFinder.ViewModel;

public partial class BaseViewModel : ObservableObject
{

    private bool isBusy;
    private string title;

    public bool IsBusy
    {
        get => isBusy;
        set
        {
            if (isBusy != value)
            {
                isBusy = value;
                OnPropertyChanged("IsBusy");
                OnPropertyChanged(nameof(IsNotBusy));
            }
        }
    }

    public string Title
    {
        get => title;
        set
        {
            if (title != value)
            {
                title = value;
                OnPropertyChanged("Title");   
            }
        }
    }

    public bool IsNotBusy => !isBusy;

}
