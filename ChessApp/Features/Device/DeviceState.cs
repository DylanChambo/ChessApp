using BlazorState;

namespace ChessApp.Features.Device;

public partial class DeviceState : State<DeviceState>
{
    public bool Mobile { get; private set; }

    public override void Initialize()
    {
        Mobile = false;
    }
}