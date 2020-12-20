using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using DrWndr.Models;
using MLToolkit.Forms.SwipeCardView.Core;
using Xamarin.Forms;

namespace DrWndr.ViewModels
{
    public class MainPageViewModel: BasePageViewModel
    {

        #region Public member

        public uint Threshold
        {
            get => threshold;
            set
            {
                threshold = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Post> Posts
        {
            get => posts;
            set
            {
                posts = value;
                RaisePropertyChanged();
            }
        }

        public ICommand SwipedCommand { get; }

        public ICommand DraggingCommand { get; }

        #endregion

        #region Private member

        private ObservableCollection<Post> posts = new ObservableCollection<Post>();
        private uint threshold;

        #endregion

        #region Constructor

        public MainPageViewModel()
        {
            Threshold = 300;

            SwipedCommand = new Command<SwipedCardEventArgs>(OnSwipedCommand);
            DraggingCommand = new Command<DraggingCardEventArgs>(OnDraggingCommand);
            Posts = new ObservableCollection<Post>(Post.GetAll());
        }

        #endregion

        #region Private helper

        private void OnSwipedCommand(SwipedCardEventArgs eventArgs)
        {
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
