using SharpDX.Multimedia;
using SharpDX.RawInput;

namespace HexaEngine.Core.Input
{
    public partial class RawInput
    {
        public void InitializeDirectRawInput()
        {
            Device.RegisterDevice(UsagePage.Generic, UsageId.GenericMouse, DeviceFlags.None);
            Device.RegisterDevice(UsagePage.Generic, UsageId.GenericKeyboard, DeviceFlags.None);
        }
    }
}
