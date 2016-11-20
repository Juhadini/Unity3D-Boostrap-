/**
Copyright (c) 2016 Juha Möttönen

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be included
in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
**/

using System.Collections.Generic;
using UnityEngine;
namespace GameSpace
{
    /// <summary>
    /// Singleton. Anti Pattern. Or A possibility? Read mroe about singletons here:
    /// https://en.wikipedia.org/wiki/Singleton_pattern
	/// 
    /// (Why to use unity: allows a static class that can be used inside the inspector and static calss can't)
    /// </summary>
    public class Singleton : BaseMonoBehaviour
    {
        /// <summary>
        ///  Similar logix to <see cref="BaseClass.s_baseClassInstance"> but only for the singletons.!--
        /// Using dictionaries is expensive, but used correctly they can actually fasten the
        /// iteration counts (one to many, here the concepts not used as should)
        /// </summary>
        /// <returns></returns>
        static Dictionary<System.Type, Singleton> s_instances = new Dictionary<System.Type, Singleton>();
        /// <summary>
        /// 
        /// </summary>
        protected override void Awake()
        {
            base.Awake();//Allows us to call the functionalities of <see cref="BaseMonoBehaviour.Awake"> even with the extensions!

        }
        /// <summary>
        /// 
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
        /// <summary>
        /// By overriding the <see cref="BaseMonoBehaviour.registerInstance"> we're able to add more logic for singletons only
        /// </summary>
        /// <returns></returns>
        public override bool registerInstance()
        {
            // if dictionary contains this , bail out
            // We could also call Resources.FindObjectsOfTypeAll to see if there are nay monobehaviours
            // attached in scene, but for simplicity we're using only this. see Toolbox for FindObjectsOfTypeAll usage
            if (s_instances.ContainsKey(this.GetType()))
                return false;
            if (base.registerInstance())//the base class registeration was succesful, we're good to proceed futher
            {
                //Note how we can use base calls anywhere we like in function body
                s_instances.Add(this.GetType(), this);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Unregister this singleton from the lists
        /// </summary>
        /// <returns><c>true if the unregister was succeful both in <see cref="BaseClass.unregisterInstance"> and in singleton dictionary remove</returns>
        public override bool unregisterInstance()
        {
            if (s_instances.ContainsKey(this.GetType()))
            {
                s_instances.Remove(this.GetType());
                return base.unregisterInstance();
            }
            return false;
        }
    }
}