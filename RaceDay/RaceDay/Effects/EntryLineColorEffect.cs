using Xamarin.Forms;

namespace RaceDay.Effects
{
    /// <summary>
    /// Placeholder for the device dependent EntryLineColorEffect graphics operations.  Used to draw the line of an text entry field
    /// in the specified color as the Custom Validation behaviors will change the line color to red on an invalid rule.
    /// </summary>
    /// 
    public class EntryLineColorEffect : RoutingEffect
    {
        public EntryLineColorEffect() : base("RaceDay.EntryLineColorEffect")
        {
        }
    }
}
