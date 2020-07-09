using PDCore.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCoreNew.Helpers.ExceptionHandling
{
    public static class DbActionWrapper
    {
        public static string Execute(Action action)
        {
            return DoExecuteAsync(null, action, true).Result;
        }

        public static Task<string> ExecuteAsync(Func<Task> task)
        {
            return DoExecuteAsync(task, null, false);
        }

        private async static Task<string> DoExecuteAsync(Func<Task> task, Action action, bool sync)
        {
            try
            {
                if (sync)
                    action();
                else
                    await task();
            }
            catch (DbEntityValidationException e)
            {
                return e.GetErrors();
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return WebUtils.ResultOkIndicator;
        }
    }
}
