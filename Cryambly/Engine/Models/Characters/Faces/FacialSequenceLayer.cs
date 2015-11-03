namespace CryCil.Engine.Models.Characters.Faces
{
	/// <summary>
	/// Enumeration of identifiers of layers of facial animation.
	/// </summary>
	public enum FacialSequenceLayer
	{
		/// <summary>
		/// Layer that is used for facial animations in the preview section of facial editor.
		/// </summary>
		Preview,
		/// <summary>
		/// Layer that is used for facial animations in dialogue lines together with sound and lip-sync.
		/// </summary>
		Dialogue,
		/// <summary>
		/// Layer that is used for facial animations that are triggered via a TrackView sequence.
		/// </summary>
		Trackview,
		/// <summary>
		/// Unknown.
		/// </summary>
		AGStateAndAIAlertness,
		/// <summary>
		/// Layer that is used for facial animations requested through mannequin.
		/// </summary>
		Mannequin,
		/// <summary>
		/// Layer that is used for facial animations that are looped, requested/cleared via GoalPipes and
		/// GoalOps in AI subsystem.
		/// </summary>
		AIExpression,
		/// <summary>
		/// Layer that is used for facial animations requested through the FlowGraph.
		/// </summary>
		FlowGraph,
		/// <summary>
		/// Number of layers.
		/// </summary>
		Count
	}
}