using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RaceDay
{
    public enum ToastDuration
    {
        Short = 0,
        Long = 1
    };

    public class ToastOptions
    {
        public string Text { get; set; }
        public ToastDuration Duration { get; set; }
    };

    public interface IToast
    {
        void Show(ToastOptions options);
        int Duration();
    }
}
