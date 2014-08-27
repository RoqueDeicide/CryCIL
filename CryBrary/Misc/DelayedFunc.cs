using System;
using CryEngine.Initialization;

namespace CryEngine
{
	public abstract class DelayedFuncBase : CryScriptInstance
	{
		protected void Init(Delegate _delegate, float delay)
		{
			Delegate = _delegate;
			Delay = delay;

			Reset();

			ScriptManager.Instance.AddScriptInstance(this, ScriptType.CryScriptInstance);
		}

		public void Reset()
		{
			FrameStartTime = Time.FrameStartTime;

			ReceiveUpdates = true;
		}

		public override void OnUpdate()
		{
			if ((Time.FrameStartTime - FrameStartTime) > Delay)
			{
				ReceiveUpdates = false;

				Delegate.DynamicInvoke(Params);
			}
		}

		#region Overrides
		public override bool Equals(object obj)
		{
			return obj is DelayedFuncBase && this == obj;
		}

		public override int GetHashCode()
		{
			// Overflow is fine, just wrap
			unchecked
			{
				int hash = 17;

				if (Params != null)
					hash = hash * 29 + Params.GetHashCode();

				hash = hash * 29 + Delay.GetHashCode();
				hash = hash * 29 + FrameStartTime.GetHashCode();
				hash = hash * 29 + Delegate.GetHashCode();

				return hash;
			}
		}
		#endregion

		public abstract object[] Params { get; }

		public float Delay { get; set; }
		public float FrameStartTime { get; set; }
		public Delegate Delegate { get; set; }
	}

	public sealed class DelayedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : DelayedFuncBase
	{
		public DelayedFunc() { }

		/// <summary></summary>
		public DelayedFunc(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, float delay, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8, T9 param9, T10 param10)
		{
			_params = new object[] { param1, param2, param3, param4, param5, param6, param7, param8, param9, param10 };

			Init(action, delay);
		}

		private readonly object[] _params;
		public override object[] Params { get { return _params; } }
	}

	public sealed class DelayedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9> : DelayedFuncBase
	{
		public DelayedFunc() { }

		/// <summary></summary>
		public DelayedFunc(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, float delay, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8, T9 param9)
		{
			_params = new object[] { param1, param2, param3, param4, param5, param6, param7, param8, param9 };

			Init(action, delay);
		}

		private readonly object[] _params;
		public override object[] Params { get { return _params; } }
	}

	public sealed class DelayedFunc<T1, T2, T3, T4, T5, T6, T7, T8> : DelayedFuncBase
	{
		public DelayedFunc() { }

		/// <summary></summary>
		public DelayedFunc(Action<T1, T2, T3, T4, T5, T6, T7, T8> action, float delay, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8)
		{
			_params = new object[] { param1, param2, param3, param4, param5, param6, param7, param8 };

			Init(action, delay);
		}

		private readonly object[] _params;
		public override object[] Params { get { return _params; } }
	}

	public sealed class DelayedFunc<T1, T2, T3, T4, T5, T6, T7> : DelayedFuncBase
	{
		public DelayedFunc() { }

		/// <summary></summary>
		public DelayedFunc(Action<T1, T2, T3, T4, T5, T6, T7> action, float delay, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7)
		{
			_params = new object[] { param1, param2, param3, param4, param5, param6, param7 };

			Init(action, delay);
		}

		private readonly object[] _params;
		public override object[] Params { get { return _params; } }
	}

	public sealed class DelayedFunc<T1, T2, T3, T4, T5, T6> : DelayedFuncBase
	{
		public DelayedFunc() { }

		/// <summary></summary>
		public DelayedFunc(Action<T1, T2, T3, T4, T5, T6> action, float delay, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6)
		{
			_params = new object[] { param1, param2, param3, param4, param5, param6 };

			Init(action, delay);
		}

		private readonly object[] _params;
		public override object[] Params { get { return _params; } }
	}

	public sealed class DelayedFunc<T1, T2, T3, T4, T5> : DelayedFuncBase
	{
		public DelayedFunc() { }

		/// <summary></summary>
		public DelayedFunc(Action<T1, T2, T3, T4, T5> action, float delay, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5)
		{
			_params = new object[] { param1, param2, param3, param4, param5 };

			Init(action, delay);
		}

		private readonly object[] _params;
		public override object[] Params { get { return _params; } }
	}

	public sealed class DelayedFunc<T1, T2, T3, T4> : DelayedFuncBase
	{
		public DelayedFunc() { }

		/// <summary></summary>
		public DelayedFunc(Action<T1, T2, T3, T4> action, float delay, T1 param1, T2 param2, T3 param3, T4 param4)
		{
			_params = new object[] { param1, param2, param3, param4 };

			Init(action, delay);
		}

		private readonly object[] _params;
		public override object[] Params { get { return _params; } }
	}

	public sealed class DelayedFunc<T1, T2, T3> : DelayedFuncBase
	{
		public DelayedFunc() { }

		/// <summary></summary>
		public DelayedFunc(Action<T1, T2, T3> action, float delay, T1 param1, T2 param2, T3 param3)
		{
			_params = new object[] { param1, param2, param3 };

			Init(action, delay);
		}

		private readonly object[] _params;
		public override object[] Params { get { return _params; } }
	}

	public sealed class DelayedFunc<T1, T2> : DelayedFuncBase
	{
		public DelayedFunc() { }

		/// <summary></summary>
		public DelayedFunc(Action<T1, T2> action, float delay, T1 param1, T2 param2)
		{
			_params = new object[] { param1, param2 };

			Init(action, delay);
		}

		private readonly object[] _params;
		public override object[] Params { get { return _params; } }
	}

	public sealed class DelayedFunc<T1> : DelayedFuncBase
	{
		public DelayedFunc() { }

		/// <summary></summary>
		public DelayedFunc(Action<T1> action, float delay, T1 param1)
		{
			_params = new object[] { param1 };

			Init(action, delay);
		}

		private readonly object[] _params;
		public override object[] Params { get { return _params; } }
	}

	public sealed class DelayedFunc : DelayedFuncBase
	{
		public DelayedFunc() { }

		/// <summary></summary>
		public DelayedFunc(Action action, float delay)
		{
			Init(action, delay);
		}

		public override object[] Params { get { return null; } }
	}
}