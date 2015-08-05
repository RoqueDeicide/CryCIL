using System;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.RunTime;

namespace CryCil.Engine.Logic
{
	[Flags]
	internal enum RmiType
	{
		// Attachment flags.
		PreAttach = RmiDataTransfer.PreAttach,
		PostAttach = RmiDataTransfer.PostAttach,
		NoAttach = RmiDataTransfer.NoAttach,
		Urgent = RmiDataTransfer.Urgent,
		Independent = RmiDataTransfer.Independent,

		AttachmentMask = PreAttach | PostAttach | NoAttach | Urgent | Independent,
		// Reliability flags.
		Reliable = 1 << 3,
		// Is Server.
		ToServer = 1 << 4,
		// Is Fast
		LowDelay = 1 << 5,

		PreAttachServer = PreAttach | ToServer,
		PreAttachClient = PreAttach,
		PostAttachServer = PostAttach | ToServer,
		PostAttachClient = PostAttach,
		ReliableNoAttachServer = Reliable | NoAttach | ToServer,
		ReliableNoAttachClient = Reliable | NoAttach,
		UnreliableNoAttachServer = NoAttach | ToServer,
		UnreliableNoAttachClient = NoAttach,

		FastPreAttachServer = LowDelay | PreAttach | ToServer,
		FastPreAttachClient = LowDelay | PreAttach,
		FastPostAttachServer = LowDelay | PostAttach | ToServer,
		FastPostAttachClient = LowDelay | PostAttach,
		FastReliableNoAttachServer = LowDelay | Reliable | NoAttach | ToServer,
		FastReliableNoAttachClient = LowDelay | Reliable | NoAttach,
		FastUnreliableNoAttachServer = LowDelay | NoAttach | ToServer,
		FastUnreliableNoAttachClient = LowDelay | NoAttach,

		ReliableUrgentServer = Reliable | Urgent | ToServer,
		ReliableUrgentClient = Reliable | Urgent,
		UnreliableUrgentServer = Urgent | ToServer,
		UnreliableUrgentClient = Urgent,

		ReliableIndependentServer = Reliable | Independent | ToServer,
		ReliableIndependentClient = Reliable | Independent,
		UnreliableIndependentServer = Independent | ToServer,
		UnreliableIndependentClient = Independent
	}
	/// <summary>
	/// Marks methods that should be invoked remotely via CryEngine RMI (Remote Method Invocation)
	/// framework.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
	[StructLayout(LayoutKind.Explicit)]
	public sealed class RMIAttribute : Attribute
	{
		#region Statics
		private static readonly BitVector32.Section attachmentSection;
		private static readonly BitVector32.Section reliabilitySection;
		private static readonly BitVector32.Section isServerSection;
		private static readonly BitVector32.Section lowDelaySection;
		static RMIAttribute()
		{
			attachmentSection = BitVector32.CreateSection(5);
			reliabilitySection = BitVector32.CreateSection(1, attachmentSection);
			isServerSection = BitVector32.CreateSection(1, reliabilitySection);
			lowDelaySection = BitVector32.CreateSection(1, isServerSection);
		}
		#endregion
		#region Fields
		[FieldOffset(0)]
		[UsedImplicitly]
		private BitVector32 bits;
		[FieldOffset(0)]
		[UsedImplicitly]
		internal RmiType type;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether additional safety measures should be used when transferring RMI data for
		/// invocation of the method that is marked with this attribute.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Unreliably invoked methods should have more checks for errors in delivery and shouldn't crash
		/// the program when those happen.
		/// </para>
		/// <para>
		/// Reliability cannot be specified for methods whose RMI data is pre- or post- attached (When
		/// <see cref="DataTransfer"/> is either equal to <see cref="RmiDataTransfer.PreAttach"/> or
		/// <see cref="RmiDataTransfer.PostAttach"/>).
		/// </para>
		/// </remarks>
		public bool Reliable
		{
			get { return this.type.HasFlag(RmiType.Reliable); }
		}
		/// <summary>
		/// Specifies how and when RMI data is transferred via network.
		/// </summary>
		public RmiDataTransfer DataTransfer
		{
			get { return (RmiDataTransfer)(this.type & RmiType.AttachmentMask); }
		}
		/// <summary>
		/// Indicates whether marked method will be invoked from client to server (when value is
		/// <c>true</c>), or from server to client (when value is <c>false</c>).
		/// </summary>
		public bool ToServer
		{
			get { return this.type.HasFlag(RmiType.ToServer); }
		}
		/// <summary>
		/// Indicates whether marked method must be invoked without waiting for the next frame.
		/// </summary>
		public bool Immediate
		{
			get { return this.type.HasFlag(RmiType.LowDelay); }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="dataTransfer">
		/// Specifies the way the RMI data is transfered. <see cref="DataTransfer"/> for details.
		/// </param>
		/// <param name="toServer">    
		/// Specifies whether method will be invoked from the client. <see cref="ToServer"/> for details.
		/// </param>
		/// <param name="immediate">   
		/// Specifies whether method will be invoked before next frame. <see cref="Immediate"/> for
		/// details.
		/// </param>
		/// <param name="reliable">    
		/// Specifies whether RMI data will be transfered using more reliable way. <see cref="Reliable"/>
		/// for details.
		/// </param>
		public RMIAttribute(RmiDataTransfer dataTransfer, bool toServer, bool immediate = false, bool reliable = false)
		{
			this.bits[attachmentSection] = (int)dataTransfer;
			if (dataTransfer == RmiDataTransfer.PostAttach || dataTransfer == RmiDataTransfer.PreAttach && reliable)
			{
#if DEBUG
				MonoInterface.DisplayException(new ArgumentException("Methods that transfer RMI with pre- or post- attach " +
																	 "cannot be specified as reliable in RMIAttribute " +
																	 "constructor. This error is not fatal and can be " +
																	 "ignored."));
#endif
				reliable = false;
			}
			this.bits[reliabilitySection] = reliable ? 1 : 0;
			this.bits[isServerSection] = toServer ? 1 : 0;
			if (dataTransfer == RmiDataTransfer.Independent || dataTransfer == RmiDataTransfer.Urgent && immediate)
			{
#if DEBUG
				MonoInterface.DisplayException(new ArgumentException("Methods that transfer RMI urgently or independently " +
																	 "cannot be specified as immediate in RMIAttribute " +
																	 "constructor. This error is not fatal and can be " +
																	 "ignored."));
#endif
				immediate = false;
			}
			this.bits[lowDelaySection] = immediate ? 1 : 0;
		}
		#endregion
	}
}