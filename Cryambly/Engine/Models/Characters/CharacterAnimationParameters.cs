namespace CryCil.Engine.Models.Characters
{
	/// <summary>
	/// Encapsulates a set of parameters that are used to start animations.
	/// </summary>
	public unsafe struct CharacterAnimationParameters
	{
		/// <summary>
		/// Transition time between 2 animations.
		/// </summary>
		public float TransitionTime;
		/// <summary>
		/// A value between 0 and 1 that can be used to specify when to start the transition animation.
		/// </summary>
		/// <remarks>
		/// This value is probably ignored if the flag <see cref="AnimationFlags.StartAtKeyTime"/> is not
		/// set in <see cref="Flags"/>.
		/// </remarks>
		public float KeyTime;
		/// <summary>
		/// Animation playback speed scale.
		/// </summary>
		public float PlaybackSpeed;
		private float allowMultiLayerAnimation;
		/// <summary>
		/// Indicates whether animations can be player on higher layers and overwrite the channels on lower
		/// layers.
		/// </summary>
		public bool AllowMultiLayerAnimation
		{
			get { return this.allowMultiLayerAnimation == 1.0f; }
			set { this.allowMultiLayerAnimation = value ? 1.0f : 0; }
		}
		/// <summary>
		/// Identifier of the layer where to play the animation.
		/// </summary>
		public int LayerId;
		/// <summary>
		/// Animation specific weight multiplier, applied on top of the existing layer weight.
		/// </summary>
		public float PlaybackWeight;
		/// <summary>
		/// A set of flags that specifies how to handle the animation.
		/// </summary>
		public AnimationFlags Flags;
		/// <summary>
		/// Token specified by the animation calling code for it's own benefit.
		/// </summary>
		public uint UserToken;
		/// <summary>
		/// A set of weights that are blended together just like the animation is, for calling code's
		/// benefit.
		/// </summary>
		public fixed float UserData [8];
		/// <summary>
		/// Creates a new instance of this type.
		/// </summary>
		/// <param name="layerId">            Used to initialize <see cref="LayerId"/>.</param>
		/// <param name="flags">              Used to initialize <see cref="Flags"/>.</param>
		/// <param name="transitionTime">     Used to initialize <see cref="TransitionTime"/>.</param>
		/// <param name="keyTime">            Used to initialize <see cref="KeyTime"/>.</param>
		/// <param name="playbackSpeed">      Used to initialize <see cref="PlaybackSpeed"/>.</param>
		/// <param name="allowMultilayerAnim">
		/// Used to initialize <see cref="allowMultiLayerAnimation"/>.
		/// </param>
		/// <param name="userToken">          Used to initialize <see cref="UserToken"/>.</param>
		/// <param name="playbackWeight">     Used to initialize <see cref="PlaybackWeight"/>.</param>
		public CharacterAnimationParameters(int layerId, AnimationFlags flags = 0, float transitionTime = -1,
											float keyTime = -1.0f, float playbackSpeed = 1.0f,
											bool allowMultilayerAnim = true, uint userToken = 0,
											float playbackWeight = 1.0f)
			: this()
		{
			this.LayerId = layerId;
			this.Flags = flags;
			this.TransitionTime = transitionTime;
			this.KeyTime = keyTime;
			this.PlaybackSpeed = playbackSpeed;
			this.PlaybackWeight = playbackWeight;
			this.allowMultiLayerAnimation = allowMultilayerAnim ? 1.0f : 0.0f;
			this.UserToken = userToken;
		}
	}
}