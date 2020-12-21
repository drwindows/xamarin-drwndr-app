using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using DrWndr.Effects;
using DrWndr.Android.Effects;


[assembly: ResolutionGroupName("io.github.tscholze")]
[assembly: ExportEffect(typeof(LabelShadowEffect), "LabelShadowEffect")]
namespace DrWndr.Android.Effects
{
    /// <summary>
    /// Xamarn.Forms Effect for a Dropshadow Label for Android devices.
    /// 
    /// Based on:
    ///     - https://github.com/xamarin/xamarin-forms-samples/tree/master/Effects/ShadowEffect
    /// </summary>
    public class LabelShadowEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            // Get the underlying control and
            var control = Control as FormsTextView;
            var effect = (ShadowEffect)Element.Effects.FirstOrDefault(e => e is ShadowEffect);

            // Check if effect is available.
            if (effect == null) return;

            // Elsewise, apply the shadow layer with given properties.
            control.SetShadowLayer(effect.Radius, effect.DistanceX, effect.DistanceY, effect.Color.ToAndroid());
        }

        protected override void OnDetached()
        {
            // Do nothing, but the interface requires it.
        }
    }
}