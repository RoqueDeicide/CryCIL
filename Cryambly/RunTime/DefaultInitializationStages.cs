using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryCil.RunTime
{
	/// <summary>
	/// Enumeration of indices of default initialization stages.
	/// </summary>
	public enum DefaultInitializationStages
	{
		/// <summary>
		/// Index of the initialization stage during which types that represent objects that handle
		/// transfer of data for RMI calls defined in CryCIL are registered.
		/// </summary>
		RmiRegistrationStage = 1000000 - 1,
		/// <summary>
		/// Index of the initialization stage during which entities defined in CryCIL are registered.
		/// </summary>
		EntityRegistrationStage = 1000000,
		/// <summary>
		/// Index of the initialization stage during which actors defined in CryCIL are registered.
		/// </summary>
		ActorsRegistrationStage = 2000000,
		/// <summary>
		/// Index of the initialization stage during which game modes defined in CryCIL are registered.
		/// </summary>
		GameModeRegistrationStage = 3000000,
		/// <summary>
		/// Index of the initialization stage during which data required to register CryCIL flow nodes is
		/// gathered.
		/// </summary>
		FlowNodeRecognitionStage = 4000000
	}
}