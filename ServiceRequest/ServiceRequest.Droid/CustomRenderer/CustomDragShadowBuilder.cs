using System;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Graphics;
using ServiceRequest.AppContext;

namespace ServiceRequest.Droid.CustomRenderer
{
    public class CustomDragShadowBuilder : View.DragShadowBuilder
    {
        private Drawable _shadow;

        /// ------------------------------------------------------------------------------------------------
        #region Constructor
        public CustomDragShadowBuilder(View v)
            : base(v)
        {
            try
            {
                v.DrawingCacheEnabled = true;
                var bitmap = v.DrawingCache;
                _shadow = new BitmapDrawable(bitmap);
                _shadow.SetColorFilter(Color.ParseColor("#FDFEFE"), PorterDuff.Mode.Clear);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Public Function
        public override void OnProvideShadowMetrics(Point size, Point touch)
        {
            try
            {
                int width = View.Width;
                int height = View.Height;
                _shadow.SetBounds(0, 0, width, height);
                size.Set(width, height);
                touch.Set(width / 2, height / 2);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        public override void OnDrawShadow(Canvas canvas)
        {
            try
            {
                base.OnDrawShadow(canvas);
                _shadow.Draw(canvas);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}