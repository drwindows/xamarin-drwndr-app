using MLToolkit.Forms.SwipeCardView.Core;

namespace DrWndr.Utils
{
    public enum SwipeStatus
    {
        /// <summary>
        /// Neutral, aka not decided.
        /// </summary>
        Neutral,

        /// <summary>
        /// Liked.
        /// </summary>
        Liked,

        /// <summary>
        /// Disliked.
        /// </summary>
        Disliked
    }

    static class SwipeCardDirectionExtensions
    {
        /// <summary>
        /// Extension to get SwipeStatus dependent on a swipe direction.
        /// </summary>
        /// <param name="direction">Given swipe direction.</param>
        /// <returns>Resulting status</returns>
        public static SwipeStatus ToStatus(this SwipeCardDirection direction)
        {
            switch (direction)
            {
                case SwipeCardDirection.Left:
                    return SwipeStatus.Disliked;

                case SwipeCardDirection.Right:
                    return SwipeStatus.Liked;

                default:
                    return SwipeStatus.Neutral;
            }
        }
    }
}
