using System;
using DrWndr.Models;
using DrWndr.Utils;
using DrWndr.ViewModels;
using MLToolkit.Forms.SwipeCardView.Core;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DrWndr
{
    public partial class CardsView : ContentPage
    {
        #region Contructor 

        /// <summary>
        /// Constructor
        /// </summary>
        public CardsView()
        {
            // Setup view.
            InitializeComponent();
            BindingContext = new CardsViewModel();

            // Set event handler.
            SwipeCardView.Dragging += SwipeCardView_Dragging;
        }

        #endregion

        #region Event handler

        /// <summary>
        /// Raised on card dragging.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event args.</param>
        private void SwipeCardView_Dragging(object sender, DraggingCardEventArgs e)
        {
            // Perform actions according to drag.
            switch (e.Position)
            {
                case DraggingCardPosition.OverThreshold:
                    switch (e.Direction)
                    {
                        case SwipeCardDirection.Left:
                            PageContent.BackgroundGradientStops = GetGradientForStatus(SwipeStatus.Disliked);
                            break;

                        case SwipeCardDirection.Right:
                            PageContent.BackgroundGradientStops = GetGradientForStatus(SwipeStatus.Liked);
                            break;

                        case SwipeCardDirection.Up:
                            break;

                        case SwipeCardDirection.Down:
                            break;
                    }
                    break;

                default:
                    PageContent.BackgroundGradientStops = GetGradientForStatus(SwipeStatus.Neutral);
                    break;
            }
        }

        /// <summary>
        /// Raised on button tap to visit drwindows.de
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event args.</param>
        private async void Button_Clicked(object sender, EventArgs e)
        {
            var post = (Post)SwipeCardView.TopItem;
            await Browser.OpenAsync(post.ArticleUrl, BrowserLaunchMode.SystemPreferred);
        }

        #endregion

        #region Private helper

        /// <summary>
        /// Gets the background gradient stops according for current status.
        /// </summary>
        /// <returns>Gradient stops according to if post is going to be liked.</returns>
        private Xamarin.Forms.PancakeView.GradientStopCollection GetGradientForStatus(SwipeStatus status)
        {
            switch(status)
            {
                case SwipeStatus.Neutral:
                    return new Xamarin.Forms.PancakeView.GradientStopCollection
                        {
                        new Xamarin.Forms.PancakeView.GradientStop { Color = Color.FromHex("213868"), Offset = 0 },
                        new Xamarin.Forms.PancakeView.GradientStop { Color = Color.FromHex("627294"), Offset = 0.5f },
                        new Xamarin.Forms.PancakeView.GradientStop { Color = Color.FromHex("8992A4"), Offset = 1.0f },
                        };
                case SwipeStatus.Liked:
                    return new Xamarin.Forms.PancakeView.GradientStopCollection
                        {
                            new Xamarin.Forms.PancakeView.GradientStop { Color = Color.DarkOliveGreen, Offset = 0 },
                            new Xamarin.Forms.PancakeView.GradientStop { Color = Color.DarkSeaGreen, Offset = 0.5f }
                        };

                case SwipeStatus.Disliked:
                    return new Xamarin.Forms.PancakeView.GradientStopCollection
                        {
                        new Xamarin.Forms.PancakeView.GradientStop { Color = Color.DarkRed, Offset = 0 },
                        new Xamarin.Forms.PancakeView.GradientStop { Color = Color.IndianRed, Offset = 0.5f }
                        };
                default:
                    throw new NotImplementedException("Swipe status not implemented");
            }
        }


        #endregion
    }
}
