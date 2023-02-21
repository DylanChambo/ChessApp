using BlazorState;
using MediatR;

namespace ChessApp.Features.Device;

public partial class DeviceState
{
    public class IsMobileHandler : ActionHandler<IsMobileAction>
    {
        public IsMobileHandler(IStore store): base(store) { }

        DeviceState deviceState => Store.GetState<DeviceState>();

        public override Task<Unit> Handle(IsMobileAction isMobileAction, CancellationToken cancellationToken)
        {
            deviceState.Mobile = isMobileAction.Mobile;
            return Unit.Task;
        }
    }
}

