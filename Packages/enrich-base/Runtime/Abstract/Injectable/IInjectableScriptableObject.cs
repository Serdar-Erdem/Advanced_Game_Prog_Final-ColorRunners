namespace Rich.Base.Runtime.Abstract.Injectable
{
    public interface IInjectableScriptableObject
    {
        string name { get;}
        /// Indicates whether the View can work absent a context
        /// 
        /// Leave this value true most of the time. If for some reason you want
        /// a view to exist outside a context you can set it to false. The only
        /// difference is whether an error gets generated.
        bool requiresContext{ get; set;}
		
        /// Indicates whether this View  has been registered with a Context
        bool registeredWithContext{get; set;}

        /// Exposure to code of the registerWithContext (Inspector) boolean. If false, the View won't try to register.
        bool autoRegisterWithContext{ get; }

        bool shouldRegister { get; }
    }
}