using Caliburn.Micro;

namespace PDCore.Helpers.WPF.CaliburnMicro
{
    public interface IHaveShutdownTask
    {
        IResult GetShutdownTask();
    }
}
