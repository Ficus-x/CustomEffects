using Exiled.API.Features;

namespace CustomEffects
{
    using System;
    using System.Collections.Generic;
    using CustomEffects.Effects;
    
    public static class Extensions
    {
        public static void Register(this IEnumerable<CustomEffect> customEffects)
        {
            if (customEffects == null)
                throw new ArgumentNullException(nameof(customEffects));
            
            foreach (CustomEffect customEffect in customEffects)
                customEffect.TryRegister();
        }
        
        public static void Unregister(this IEnumerable<CustomEffect> customEffects)
        {
            if (customEffects == null)
                throw new ArgumentNullException(nameof(customEffects));

            foreach (CustomEffect customEffect in customEffects)
                customEffect.TryUnregister();
        }
    }
}