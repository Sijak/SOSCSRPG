using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Models;

namespace GameEngine.Action
{
    public abstract class BaseAction
    {
        protected readonly GameItem _itemInUse;
        protected BaseAction(GameItem ItemInUse)
        {
            _itemInUse = ItemInUse;
        }
        public event EventHandler<string> OnActionPerformed;
        protected void ReportResult(string result)
        {
            OnActionPerformed?.Invoke(this, result);
        }

    }
}
