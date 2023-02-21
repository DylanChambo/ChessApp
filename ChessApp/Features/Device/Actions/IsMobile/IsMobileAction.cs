using BlazorState;
using ChessApp.Data;

namespace ChessApp.Features.Device;

public partial class DeviceState
{
    public class IsMobileAction : IAction
    {
        public bool Mobile { get; set; }
    }
}

