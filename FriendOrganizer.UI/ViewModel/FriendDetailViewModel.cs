using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Event;
using Prism.Events;
using System.Threading.Tasks;
using System;
using System.Windows.Input;
using Prism.Commands;

namespace FriendOrganizer.UI.ViewModel
{
  public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
  {
    private IFriendDataService _dataService;
    private IEventAggregator _eventAggregator;

    public FriendDetailViewModel(IFriendDataService dataService,
      IEventAggregator eventAggregator)
    {
      _dataService = dataService;
      _eventAggregator = eventAggregator;
      _eventAggregator.GetEvent<OpenFriendDetailViewEvent>()
        .Subscribe(OnOpenFriendDetailView);

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
    }

        private async void OnSaveExecute()
        {
           await  _dataService.SaveAsync(Friend);
            _eventAggregator.GetEvent<AfterFriendSavedEvent>().Publish(
                new AfterFriendSavedEventArgs
                {
                    Id = Friend.Id,
                    DisplayMember = $"{Friend.FirstName} {Friend.LastName}"
                });
        }


        private bool OnSaveCanExecute()
        {
            //TODO Check if friend is valid
            return true;
        }

        private async void OnOpenFriendDetailView(int friendId)
    {
      await LoadAsync(friendId);
    }

    public async Task LoadAsync(int friendId)
    {
      Friend = await _dataService.GetByIdAsync(friendId);
    }

    private Friend _friend;

    public Friend Friend
    {
      get { return _friend; }
      private set
      {
        _friend = value;
        OnPropertyChanged();
      }
    }
        public ICommand SaveCommand { get; }
    }
}
