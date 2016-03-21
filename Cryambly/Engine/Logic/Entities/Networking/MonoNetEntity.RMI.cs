using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using CryCil.Annotations;
using CryCil.Engine.DebugServices;
using CryCil.RunTime;

namespace CryCil.Engine.Logic
{
	public partial class MonoNetEntity
	{
		/// <summary>
		/// Calls a method of this type remotely via RMI framework.
		/// </summary>
		/// <param name="method"> Method to invoke.</param>
		/// <param name="where">  
		/// A set of flags that specifies game instances to which the call will be directed.
		/// </param>
		/// <param name="channel">
		/// Identifier of the channel to use when calling specific client, can be left equal to -1
		/// otherwise.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// Method that must be invoked via RMI wasn't specified.
		/// </exception>
		/// <exception cref="MissingMethodException">
		/// This type doesn't define the specified method.
		/// </exception>
		/// <exception cref="MissingAttributeException">
		/// Methods must be marked with attribute of type <see cref="RMIAttribute"/> to be invoked via RMI
		/// framework.
		/// </exception>
		/// <exception cref="RmiException">
		/// Specified method must be called from a client game instance.
		/// </exception>
		/// <exception cref="RmiException">Specified method call must be directed to the server.</exception>
		/// <exception cref="RmiException">
		/// Attempt was made to call RMI and specifying that it must not be directed to both local and
		/// remote game instances.
		/// </exception>
		/// <exception cref="RmiException">
		/// Attempt was made to call RMI on a specific client without specifying its identifier.
		/// </exception>
		/// <exception cref="RmiException">
		/// Calling RMI on a specific client and own client is not supported.
		/// </exception>
		/// <exception cref="RmiException">
		/// Cannot send RMI call to sender's client instance because it doesn't have client instance.
		/// </exception>
		/// <exception cref="AmbiguousMatchException">
		/// More than one method is found with the specified name.
		/// </exception>
		/// <exception cref="MemberAccessException">
		/// The caller does not have access to the method represented by the delegate (for example, if the
		/// method is private).
		/// </exception>
		/// <exception cref="TypeLoadException">The custom attribute type cannot be loaded.</exception>
		[ContractAnnotation("method:null => halt")]
		public void CallRmi(Func<bool> method, RmiTarget where, int channel = -1)
		{
			if (method == null)
			{
				throw new ArgumentNullException(nameof(method), "You must specify the method that must be invoked via RMI.");
			}
			Type type = this.GetType();
			MethodInfo methodInfo = method.Method;
			if (methodInfo != type.GetMethod(methodInfo.Name))
			{
				throw new MissingMethodException($"{type.FullName} type doesn't define method named {methodInfo.Name}.");
			}
			int rmiType = this.ValidateRmiMethod(methodInfo, null, where, false, false);

			this.ValidateRmiTarget(where, channel);

			InvokeRmi(this.Id, methodInfo.Name, null, where, channel, rmiType);
		}
		/// <summary>
		/// Calls a method of this type remotely via RMI framework.
		/// </summary>
		/// <typeparam name="RmiParametersType">A type of object that provides RMI arguments.</typeparam>
		/// <param name="method">    Method to invoke.</param>
		/// <param name="where">     
		/// A set of flags that specifies game instances to which the call will be directed.
		/// </param>
		/// <param name="parameters">
		/// An object that handles transfer of additional RMI data, if needed.
		/// </param>
		/// <param name="channel">   
		/// Identifier of the channel to use when calling specific client, can be left equal to -1
		/// otherwise.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// Method that must be invoked via RMI wasn't specified.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// Parameters for the method that must be invoked via RMI weren't specified.
		/// </exception>
		/// <exception cref="MissingMethodException">
		/// This type doesn't define the specified method.
		/// </exception>
		/// <exception cref="MissingAttributeException">
		/// Methods must be marked with attribute of type <see cref="RMIAttribute"/> to be invoked via RMI
		/// framework.
		/// </exception>
		/// <exception cref="RmiException">
		/// Specified method must be called from a client game instance.
		/// </exception>
		/// <exception cref="RmiException">Specified method call must be directed to the server.</exception>
		/// <exception cref="RmiException">
		/// Attempt was made to call RMI and specifying that it must not be directed to both local and
		/// remote game instances.
		/// </exception>
		/// <exception cref="RmiException">
		/// Attempt was made to call RMI on a specific client without specifying its identifier.
		/// </exception>
		/// <exception cref="RmiException">
		/// Calling RMI on a specific client and own client is not supported.
		/// </exception>
		/// <exception cref="RmiException">
		/// Cannot send RMI call to sender's client instance because it doesn't have client instance.
		/// </exception>
		/// <exception cref="AmbiguousMatchException">
		/// More than one method is found with the specified name.
		/// </exception>
		/// <exception cref="MemberAccessException">
		/// The caller does not have access to the method represented by the delegate (for example, if the
		/// method is private).
		/// </exception>
		/// <exception cref="TypeLoadException">The custom attribute type cannot be loaded.</exception>
		[ContractAnnotation("parameters:null => halt")]
		[ContractAnnotation("method:null => halt")]
		public void CallRmi<RmiParametersType>(Func<RmiParametersType, bool> method, RmiTarget where,
											   RmiParametersType parameters, int channel = -1)
			where RmiParametersType : RmiParameters
		{
			if (method == null)
			{
				throw new ArgumentNullException(nameof(method), "You must specify the method that must be invoked via RMI.");
			}
			if (parameters == null)
			{
				throw new ArgumentNullException(nameof(parameters),
												"You must specify the parameters for the method that must be invoked via RMI.");
			}
			Type type = this.GetType();
			MethodInfo methodInfo = method.Method;
			if (methodInfo != type.GetMethod(methodInfo.Name))
			{
				throw new MissingMethodException($"{type.FullName} type doesn't define method named {methodInfo.Name}.");
			}
			int rmiType = this.ValidateRmiMethod(methodInfo, parameters, where, false, false);

			this.ValidateRmiTarget(where, channel);

			InvokeRmi(this.Id, methodInfo.Name, parameters, where, channel, rmiType);
		}
		/// <summary>
		/// Calls a method of this type remotely via RMI framework.
		/// </summary>
		/// <param name="method">    Method to invoke.</param>
		/// <param name="where">     
		/// A set of flags that specifies game instances to which the call will be directed.
		/// </param>
		/// <param name="parameters">
		/// An object that handles transfer of additional RMI data, if needed.
		/// </param>
		/// <param name="channel">   
		/// Identifier of the channel to use when calling specific client, can be left equal to -1
		/// otherwise.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// Method that must be invoked via RMI wasn't specified.
		/// </exception>
		/// <exception cref="MissingMethodException">
		/// This type doesn't define the specified method.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Specified method must return boolean value to be invoked via RMI framework.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Specified method doesn't accept any arguments, but it was invoked with non-null
		/// <paramref name="parameters"/> object.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Specified method accepts parameters, but it was invoked without them.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Methods that accept more then 1 parameter cannot be invoked via RMI framework.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Methods must accept 1 parameter of type that derives from <see cref="RmiParameters"/> to be
		/// invoked via RMI framework.
		/// </exception>
		/// <exception cref="MissingAttributeException">
		/// Methods must be marked with attribute of type <see cref="RMIAttribute"/> to be invoked via RMI
		/// framework.
		/// </exception>
		/// <exception cref="RmiException">
		/// Specified method must be called from a client game instance.
		/// </exception>
		/// <exception cref="RmiException">Specified method call must be directed to the server.</exception>
		/// <exception cref="RmiException">
		/// Attempt was made to call RMI and specifying that it must not be directed to both local and
		/// remote game instances.
		/// </exception>
		/// <exception cref="RmiException">
		/// Attempt was made to call RMI on a specific client without specifying its identifier.
		/// </exception>
		/// <exception cref="RmiException">
		/// Calling RMI on a specific client and own client is not supported.
		/// </exception>
		/// <exception cref="RmiException">
		/// Cannot send RMI call to sender's client instance because it doesn't have client instance.
		/// </exception>
		/// <exception cref="AmbiguousMatchException">
		/// More than one method is found with the specified name.
		/// </exception>
		/// <exception cref="TypeLoadException">The custom attribute type cannot be loaded.</exception>
		[ContractAnnotation("method:null => halt")]
		public void CallRmi(MethodInfo method, RmiTarget where, RmiParameters parameters = null, int channel = -1)
		{
			if (method == null)
			{
				throw new ArgumentNullException(nameof(method), "You must specify the method that must be invoked via RMI.");
			}
			Type type = this.GetType();
			if (method != type.GetMethod(method.Name))
			{
				throw new MissingMethodException($"{type.FullName} type doesn't define method named {method.Name}.");
			}
			int rmiType = this.ValidateRmiMethod(method, parameters, where, true, true);

			this.ValidateRmiTarget(where, channel);

			InvokeRmi(this.Id, method.Name, parameters, where, channel, rmiType);
		}
		/// <summary>
		/// Calls a method of this type remotely via RMI framework.
		/// </summary>
		/// <param name="methodName">Name of the method to invoke.</param>
		/// <param name="where">     
		/// A set of flags that specifies game instances to which the call will be directed.
		/// </param>
		/// <param name="parameters">
		/// An object that handles transfer of additional RMI data, if needed.
		/// </param>
		/// <param name="channel">   
		/// Identifier of the channel to use when calling specific client, can be left equal to -1
		/// otherwise.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// Name of the method that must be invoked via RMI wasn't specified.
		/// </exception>
		/// <exception cref="MissingMethodException">
		/// This type doesn't define the method with given name.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Specified method must return boolean value to be invoked via RMI framework.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Specified method doesn't accept any arguments, but it was invoked with non-null
		/// <paramref name="parameters"/> object.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Specified method accepts parameters, but it was invoked without them.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Methods that accept more then 1 parameter cannot be invoked via RMI framework.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Methods must accept 1 parameter of type that derives from <see cref="RmiParameters"/> to be
		/// invoked via RMI framework.
		/// </exception>
		/// <exception cref="MissingAttributeException">
		/// Methods must be marked with attribute of type <see cref="RMIAttribute"/> to be invoked via RMI
		/// framework.
		/// </exception>
		/// <exception cref="RmiException">
		/// Specified method must be called from a client game instance.
		/// </exception>
		/// <exception cref="RmiException">Specified method call must be directed to the server.</exception>
		/// <exception cref="RmiException">
		/// Attempt was made to call RMI and specifying that it must not be directed to both local and
		/// remote game instances.
		/// </exception>
		/// <exception cref="RmiException">
		/// Attempt was made to call RMI on a specific client without specifying its identifier.
		/// </exception>
		/// <exception cref="RmiException">
		/// Calling RMI on a specific client and own client is not supported.
		/// </exception>
		/// <exception cref="RmiException">
		/// Cannot send RMI call to sender's client instance because it doesn't have client instance.
		/// </exception>
		/// <exception cref="AmbiguousMatchException">
		/// More than one method is found with the specified name.
		/// </exception>
		/// <exception cref="TypeLoadException">The custom attribute type cannot be loaded.</exception>
		[ContractAnnotation("methodName:null => halt")]
		public void CallRmi(string methodName, RmiTarget where, RmiParameters parameters = null, int channel = -1)
		{
			if (string.IsNullOrWhiteSpace(methodName))
			{
				throw new ArgumentNullException(nameof(methodName),
												"You must specify a name of the method that must be invoked via RMI.");
			}
			Type type = this.GetType();
			MethodInfo method = type.GetMethod(methodName);
			if (method == null)
			{
				throw new MissingMethodException($"{type.FullName} type doesn't define method named {methodName}.");
			}
			int rmiType = this.ValidateRmiMethod(method, parameters, where, true, true);

			this.ValidateRmiTarget(where, channel);

			InvokeRmi(this.Id, methodName, parameters, where, channel, rmiType);
		}
		/// <exception cref="ArgumentException">Method has failed validation.</exception>
		/// <exception cref="MissingAttributeException">Condition.</exception>
		/// <exception cref="RmiException">
		/// An error has occurred when checking Rmi configuration of the method.
		/// </exception>
		/// <exception cref="TypeLoadException">The custom attribute type cannot be loaded.</exception>
		private int ValidateRmiMethod(MethodInfo method, [CanBeNull] RmiParameters parameters,
									  [UsedImplicitly] RmiTarget where, bool checkReturnType, bool checkParameters)
		{
			Type type = this.GetType();
			string methodName = method.Name;
			if (checkReturnType && method.ReturnType != typeof(bool))
			{
				throw new ArgumentException(
					$"Method {methodName} of type {type.FullName} must return boolean value to be invoked via RMI framework.");
			}
			if (checkParameters)
			{
				var pars = method.GetParameters();
				if (pars.Length == 0 && parameters != null)
				{
					throw new ArgumentException(
						$"Method {methodName} of type {type.FullName} doesn't accept any parameters but it was " +
						"invoked with expectation that it accepts one parameter of type " + $"{parameters.GetType().FullName}");
				}
				if (pars.Length > 0 && parameters == null)
				{
					throw new ArgumentException(
						$"Method {methodName} of type {type.FullName} does accept parameters but it was " +
						"invoked with expectation that it doesn't accept anything.");
				}
				if (pars.Length > 1)
				{
					throw new ArgumentException(
						$"Method {methodName} of type {type.FullName} accepts more then 1 parameter but RMI methods must only accept " +
						$"one parameter of type that derives from {typeof(RmiParameters).FullName}.");
				}
				if (!pars[0].ParameterType.Implements<RmiParameters>())
				{
					throw new ArgumentException(
						$"Method {methodName} of type {type.FullName} accept one parameter of type that derives from {typeof(RmiParameters).FullName} to be called " +
						"via RMI framework.");
				}
			}
			RMIAttribute attribute = method.GetAttribute<RMIAttribute>();
			if (attribute == null)
			{
				Type emiAttributeType = typeof(RMIAttribute);
				throw new MissingAttributeException(emiAttributeType,
													$"Method {methodName} of type {type.FullName} must be marked by an attribute " +
													$"named {emiAttributeType.FullName} to be called via RMI framework.");
			}
			if (attribute.ToServer && !Game.IsClient)
			{
				throw
					new RmiException(RmiError.IsNotClient,
									 $"Method {methodName} of type {type.FullName} must be called from a client game instance.");
			}
			if (attribute.ToServer && !where.HasFlag(RmiTarget.ToServer))
			{
				throw
					new RmiException(RmiError.NotDirectedToServer,
									 $"RMI call of the method {methodName} of type {type.FullName} must be directed to the server.");
			}

			return (int)attribute.type;
		}
		/// <exception cref="RmiException">
		/// Method that is being invoked is not configured to be invoked in specified way.
		/// </exception>
		private void ValidateRmiTarget(RmiTarget where, int channel)
		{
			if (where.HasFlag(RmiTarget.NoCall))
			{
				throw new RmiException(RmiError.NoAllowedCalls, "Attempt was made to call RMI and specifying that it must " +
																"not be directed to both local and remote game instances.");
			}
			if (where.HasFlag(RmiTarget.ToClientChannel))
			{
				if (channel <= 0)
				{
					throw new RmiException(RmiError.ClientNotSpecified,
										   "Attempt was made to call RMI on a specific client without specifying its identifier.");
				}
				if (where.HasFlag(RmiTarget.ToOwnClient))
				{
					throw new RmiException(RmiError.SendingToClientAndItself,
										   "Calling RMI on a specific client and own client is not supported.");
				}
			}
			if (where.HasFlag(RmiTarget.ToOwnClient) && !this.channelId.IsValid)
			{
				throw new RmiException(RmiError.SendingToItselfWithoutOwnClient,
									   "Cannot send RMI call to sender's client instance because it doesn't have client instance.");
			}
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InvokeRmi(EntityId sender, string methodName, RmiParameters parameters, RmiTarget where,
											 int channel, int rmiType);
		[RawThunk("Invoked from underlying object when it receives a request to invoke RMI method " +
				  "from somewhere else.")]
		private bool ReceiveRmi(string methodName, RmiParameters parameters)
		{
			try
			{
				Type type = this.GetType();
				MethodInfo method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic);
				if (method == null)
				{
					Log.Error(
							  $"A request to invoke the method {methodName} of type {type.FullName} has been received from remote " +
							  "location, but the method is not defined.", true);
					return false;
				}
				RMIAttribute attribute = method.GetAttribute<RMIAttribute>();
				if (attribute == null)
				{
					Log.Warning(
							    $"A request to invoke the method {method.Name} of type {type.FullName} has been received from remote " +
								"location, but the method is not allowed to be invoked remotely.", false);
					return false;
				}
				if (attribute.ToServer && !Game.IsServer)
				{
					Log.Warning(
							    $"A request to invoke the method {method.Name} of type {type.FullName} has been received from remote " +
								"location, but the method is supposed to be invoked from the client to the server " +
								"and this game instance is not a server.", false);
					return false;
				}
				if (!attribute.ToServer && !Game.IsClient)
				{
					Log.Warning(
							    $"A request to invoke the method {method.Name} of type {type.FullName} has been received from remote " +
								"location, but the method is supposed to be invoked from the server to the client " +
								"and this game instance is not a client.", false);
					return false;
				}
				try
				{
					return (bool)method.Invoke(this, parameters != null ? new object[] {parameters} : null);
				}
				catch (ArgumentException ex)
				{
					if (attribute.Reliable)
					{
						var pars = method.GetParameters();
						string message;
						if (parameters == null)
						{
							message =
								$"Method {method.Name} of type {type.FullName} accepts parameters, but it was invoked with no arguments.";
						}
						else if (pars.Length == 0)
						{
							message =
								$"Method {method.Name} of type {type.FullName} doesn't accept any parameters, but was invoked with one.";
						}
						else
						{
							message =
								$"Method {method.Name} of type {type.FullName} must only accept one parameter of type {parameters.GetType().FullName}, " +
								$"but it actually accepts {pars.Length} parameters of types {pars.Select(t => t.ParameterType.FullName).ContentsToString(", ")}.";
						}
						RmiException rex = new RmiException(message, ex);
						MonoInterface.DisplayException(rex);
					}
					return false;
				}
				catch (Exception ex)
				{
					if (attribute.Reliable)
					{
						string message =
							$"An error has occurred when invoking method {method.Name} of type {type.FullName} via RMI mechanism.";
						RmiException rex = new RmiException(message, ex);
						MonoInterface.DisplayException(rex);
					}
					return false;
				}
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
				return false;
			}
		}
	}
}