using System;
using System.Collections.Generic;

using CryEngine.Extensions;

using CryEngine.Flowgraph;

namespace CryEngine.Initialization
{
    /// <summary>
    /// Represents a given class.
    /// </summary>
    public class CryScript
    {
        private CryScript() { }

        private CryScript(Type type, ScriptType scriptType)
        {
            Type = type;
            ScriptName = type.Name;

            ScriptType = scriptType;
        }

        static ScriptType GetScriptType(Type type)
        {
            if (type.IsAbstract || type.IsEnum)
                return ScriptType.Any;

            var scriptType = ScriptType.Any;
            if (type.Implements<CryScriptInstance>())
            {
                scriptType |= ScriptType.CryScriptInstance;

                if (type.Implements<EntityBase>())
                {
                    scriptType |= ScriptType.Entity;

                    if (type.Implements<ActorBase>())
                    {
                        scriptType |= ScriptType.Actor;

                        if (type.Implements<AIActor>())
                            scriptType |= ScriptType.AIActor;
                    }
                    else if (type.Implements<GameRules>())
                        scriptType |= ScriptType.GameRules;
                }
                else if (type.Implements<FlowNode>())
                {
                    if (type.ImplementsGeneric(typeof(EntityFlowNode<>)))
                        scriptType |= ScriptType.EntityFlowNode;

                    scriptType |= ScriptType.FlowNode;
                }
            }

            return scriptType;
        }

        public static bool TryCreate(Type type, out CryScript script)
        {
            if (type.IsAbstract || type.IsEnum)
            {
                script = null;
                return false;
            }

            var scriptType = GetScriptType(type);
            if ((scriptType & (scriptType - 1)) == 0) // only had Any set.
            {
                script = null;
                return false;
            }

            script = new CryScript(type, scriptType);
            return true;
        }

        public ScriptType ScriptType { get; private set; }

        public Type Type { get; private set; }
        /// <summary>
        /// The script's name, not always type name!
        /// </summary>
        public string ScriptName { get; set; }

        /// <summary>
        /// Stores all instances of this class.
        /// </summary>
        public List<CryScriptInstance> ScriptInstances { get; internal set; }

        public IScriptRegistrationParams RegistrationParams { get; set; }

        /// <summary>
        /// True when the script has been registered in native code, e.g. entity system registration.
        /// </summary>
        public bool Registered { get; set; }

        #region Operators
        public static bool operator ==(CryScript script1, CryScript script2)
        {
            if (object.ReferenceEquals(script2, null))
                return object.ReferenceEquals(script1, null);

            return script1.Type == script2.Type;
        }

        public static bool operator !=(CryScript script1, CryScript script2)
        {
            return !(script1 == script2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            return GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;

                hash = hash * 23 + ScriptType.GetHashCode();
                hash = hash * 23 + Type.GetHashCode();

                return hash;
            }
        }

        #endregion
    }

    [Flags]
    public enum ScriptType
    {
        /// <summary>
        /// All scripts have this flag applied.
        /// </summary>
        Any = 1 << 0,
        /// <summary>
        /// Scripts deriving from CryScriptInstance.
        /// </summary>
        CryScriptInstance = 1 << 1,
        /// <summary>
        /// Scripts deriving from GameRules.
        /// </summary>
        GameRules = 1 << 2,
        /// <summary>
        /// Scripts deriving from FlowNode.
        /// </summary>
        FlowNode = 1 << 3,
        /// <summary>
        /// Scripts deriving from EntityBase.
        /// </summary>
        Entity = 1 << 4,
        /// <summary>
        /// Scripts deriving from Actor.
        /// </summary>
        Actor = 1 << 5,
        /// <summary>
        /// Scripts deriving from AIActor
        /// </summary>
        AIActor = 1 << 6,
        /// <summary>
        /// Scripts deriving from EntityFlowNode.
        /// </summary>
        EntityFlowNode = 1 << 7,
    }
}
