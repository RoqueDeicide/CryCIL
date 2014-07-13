using System.Collections.Generic;

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
				NativeInputMethods.RegisterAction(actionMap);

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
			int numRemoved = 0;

			foreach (var actionMap in actionmapDelegates)
				numRemoved += actionMap.Value.RemoveAll(x => x.Target == target);

			return numRemoved;
		}

		internal void Invoke(ActionMapEventArgs args)
		{
			List<ActionMapEventDelegate> eventDelegates;
			if (actionmapDelegates.TryGetValue(args.ActionName, out eventDelegates))
				eventDelegates.ForEach(x => x(args));
		}

		private Dictionary<string, List<ActionMapEventDelegate>> actionmapDelegates;
	}
}