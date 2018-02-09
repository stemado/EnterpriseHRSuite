using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Event;
using Prism.Events;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using FriendOrganizer.UI.Wrapper;
using FriendOrganizer.UI.Data.Repositories;

namespace FriendOrganizer.UI.ViewModel
{

    public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
    {
        private IFriendRepository _friendRepository;
        private IEventAggregator _eventAggregator;
        private FriendWrapper _friend;
        private bool _hasChanges;

        //Using _friendRepository to load and save a friend.
        public FriendDetailViewModel(IFriendRepository friendRepository,
      IEventAggregator eventAggregator)
        {
            _friendRepository = friendRepository;
            _eventAggregator = eventAggregator;


            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
        }

        public async Task LoadAsync(int friendId)
        {
            var friend = await _friendRepository.GetByIdAsync(friendId);

            Friend = new FriendWrapper(friend);
            Friend.PropertyChanged += (s, e) =>
                {
                    if (!HasChanges)
                    {
                        HasChanges = _friendRepository.HasChanges();
                    }
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



        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }


        public ICommand SaveCommand { get; }

        private async void OnSaveExecute()
        {
            await _friendRepository.SaveAsync();
            HasChanges = _friendRepository.HasChanges();
            _eventAggregator.GetEvent<AfterFriendSavedEvent>().Publish(
                new AfterFriendSavedEventArgs
                {
                    Id = Friend.Id,
                    DisplayMember = $"{Friend.FirstName} {Friend.LastName}"
                });
        }

        private bool OnSaveCanExecute()
        {
            return Friend != null && !Friend.HasErrors && HasChanges;
        }


    }
}
