using DrWndr.Models;
using DrWndr.Utils;
using MLToolkit.Forms.SwipeCardView.Core;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace DrWndr.ViewModels
{
    public class CardsViewModel: BasePageViewModel
    {
        #region Public member

        /// <summary>
        /// Determines the screen swipe distance
        /// threshold.
        /// </summary>
        public uint Threshold
        {
            get => threshold;
            set
            {
                threshold = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets the count information text.
        /// </summary>
        public string CountText
        {
            get => countText;
            set
            {
                countText = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Contains all posts to swipe.
        /// </summary>
        public ObservableCollection<Post> Posts
        {
            get => posts;
            set
            {
                posts = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Raised if user swiped.
        /// </summary>
        public ICommand SwipedCommand { get; }

        /// <summary>
        /// Raised if user dragged a card.
        /// </summary>
        public ICommand DraggingCommand { get; }

        #endregion

        #region Private member

        private ObservableCollection<Post> posts = new ObservableCollection<Post>();
        private uint threshold;
        private string countText;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public CardsViewModel()
        {
            Threshold = (uint)(App.ScreenWidth / 3);
            SwipedCommand = new Command<SwipedCardEventArgs>(OnSwipedCommand);
            DraggingCommand = new Command<DraggingCardEventArgs>(OnDraggingCommand);
            Posts = new ObservableCollection<Post>(Post.GetAll());
            CountText = $"Du spielst mit {Posts.Count} Artikeln";
        }

        #endregion

        #region Private helper

        private void OnSwipedCommand(SwipedCardEventArgs eventArgs)
        {
            var swipedPost = (Post)eventArgs.Item;
            var index = Posts.IndexOf(swipedPost);
            Posts[index].Status = eventArgs.Direction.ToStatus();
        }

        private void OnDraggingCommand(DraggingCardEventArgs eventArgs)
        {
            switch (eventArgs.Position)
            {
                case DraggingCardPosition.Start:
                    return;

                case DraggingCardPosition.UnderThreshold:
                    break;

                case DraggingCardPosition.OverThreshold:
                    break;

                case DraggingCardPosition.FinishedUnderThreshold:
                    return;

                case DraggingCardPosition.FinishedOverThreshold:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion
    }
}
