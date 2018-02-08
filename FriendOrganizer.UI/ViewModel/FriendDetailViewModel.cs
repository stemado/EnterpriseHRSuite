using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Event;
using Prism.Events;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using FriendOrganizer.UI.Wrapper;

namespace FriendOrganizer.UI.ViewModel
{
    public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
  {
    private IFriendDataService _dataService;
    private IEventAggregator _eventAggregator;
    private FriendWrapper _friend;

    public FriendDetailViewModel(IFriendDataService dataService,
      IEventAggregator eventAggregator)
    {
      _dataService = dataService;
      _eventAggregator = eventAggregator;
      _eventAggregator.GetEvent<OpenFriendDetailViewEvent>()
        .Subscribe(OnOpenFriendDetailView);

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
    }

    public async Task LoadAsync(int friendId)
        {
            var friend = await _dataService.GetByIdAsync(friendId);
            Friend = new FriendWrapper(friend);
            Friend.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(Friend.HasErrors))
                    {
                        ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                    }
                };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

    public FriendWrapper Friend
        {
            get { return _friend; }
            private set
            {
                _friend = value;
                OnPropertyChanged();
            }
        }

    public ICommand SaveCommand { get; }

    private async void OnSaveExecute()
        {
           await  _dataService.SaveAsync(Friend.Model);
            _eventAggregator.GetEvent<AfterFriendSavedEvent>().Publish(
                new AfterFriendSavedEventArgs
                {
                    Id = Friend.Id,
                    DisplayMember = $"{Friend.FirstName} {Friend.LastName}"
                });
        }

    private bool OnSaveCanExecute()
        {
            //TODO: Check in addition if friend has changes
            return Friend != null && !Friend.HasErrors;
        }

    private async void OnOpenFriendDetailView(int friendId)
    {
      await LoadAsync(friendId);
    }
    }
}
