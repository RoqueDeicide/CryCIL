using System.Collections.Generic;
using System.Linq;
using CryEngine.Native;

namespace CryEngine
{
	public class ActionmapHandler
	{
		public ActionmapHandler()
		{
			actionmapDelegates = new Dictionary<string, List<ActionMapEventDelegate>>();
		}

		public void Add(string actionMap, ActionMapEventDelegate eventDelegate)
		{
			List<ActionMapEventDelegate> eventDelegates;
			if (!actionmapDelegates.TryGetValue(actionMap, out eventDelegates))
			{
				InputInterop.RegisterAction(actionMap);

				eventDelegates = new List<ActionMapEventDelegate>();
				actionmapDelegates.Add(actionMap, eventDelegates);
			}

			if (!eventDelegates.Contains(eventDelegate))
				eventDelegates.Add(eventDelegate);
		}

		public bool Remove(string actionMap, ActionMapEventDelegate eventDelegate)
		{
			List<ActionMapEventDelegate> eventDelegates;
			if (actionmapDelegates.TryGetValue(actionMap, out eventDelegates))
				return eventDelegates.Remove(eventDelegate);

			return false;
		}

		public int RemoveAll(object target)
		{
			return this.actionmapDelegates.Sum(actionMap => actionMap.Value.RemoveAll(x => x.Target == target));
		}

		internal void Invoke(ActionMapEventArgs args)
		{
			List<ActionMapEventDelegate> eventDelegates;
			if (actionmapDelegates.TryGetValue(args.ActionName, out eventDelegates))
				eventDelegates.ForEach(x => x(args));
		}

		private readonly Dictionary<string, List<ActionMapEventDelegate>> actionmapDelegates;
	}
}