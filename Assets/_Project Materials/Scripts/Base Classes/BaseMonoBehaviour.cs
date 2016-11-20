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
using UnityEngine;

namespace GameSpace
{
    /// <summary>
    /// Abstract MonoBehaviour, for full documentation why to use abstract <see cref="BaseClass">.
    /// Implements <see cref="IBaseClass">
	/// 
	/// Oh, and have you noticed that there's only one or two using something at the start of the scripts?
	/// The more of using (some namsppace); you have in your classes usually means the less portable your code will be
	/// . Unity namespace is the largest components...
    /// </summary>
    public abstract class BaseMonoBehaviour : MonoBehaviour, IBaseClass
    {
        /// <summary>
        /// Just a constructor. Allows anything to be done before the unity events 
        /// are handled. Notice! Inspector values will be set in <see cref="BaseMonoBehaviour.Awake"> and
        /// thus this should not have any data changing actions
        /// </summary>
        protected BaseMonoBehaviour()
        {
            Log(string.Format("I've been created in time {0}", System.DateTime.Now));
        }
        /// <summary>
        /// This is called only when the script is generated for the first time. Using virtual allows
        /// us to create nice OOP patterns. See <see cref="Singleton"> for more information
        /// </summary>
        protected virtual void Awake()
        {
            registerInstance();
        }
        /// <summary>
        /// Works similar to <see cref="System.IDisposable"> but as unity does. All the functionalities
        /// that must be done when this game object or script is destroyed must be located here!
        /// </summary>
        protected virtual void OnDestroy()
        {

        }
        /// <summary>
        /// implement from <see cref="IBaseClass">. This allows us to add from <see cref="S_baseClassInstance"> instance list from
		/// <see cref="BaseClass">
        /// Using a boolean method instead of void allows us also to create easy tests as the function returns
        /// if the registeration was succesful. Note the difference between abstract and virtual: 
		/// 
		/// Abstract: Must be implemented in derived class (abstract method does not have functionalities)
		/// Virtual: Can be extended with override keyword. See <see cref="BaseClassChild"> for details
        /// </summary>
        /// <returns><c>true</c> if this instance was registered, otherwise false</returns>
        public virtual bool registerInstance()
        {
            if (BaseClass.S_BaseClassInstance.Contains(this))//Lists can have duplicates and hence the check
                return false;
            BaseClass.S_BaseClassInstance.Add(this);
            return true;
        }
        /// <summary>
        /// Same as <see cref="registerInstance">, but for unregistering 
        /// </summary>
        /// <returns><c>true</c> if this instance was removed from register, otherwise false<</returns>
        public virtual bool unregisterInstance()
        {
            if (!BaseClass.S_BaseClassInstance.Contains(this))//Does not contains this object of class (luokan olio)
                return false;
            BaseClass.S_BaseClassInstance.Remove(this);
            return true;
        }
        /// <summary>
        /// Using this extension from the <see cref="IBaseClass"> we can call data class updates, meaning
        /// we're allowing <see cref="MonoBehaviour.Update" style methods to be called inside of this. Neat?
        /// 
        /// See the benefits: <see cref="Toolbox"> and https://blogs.unity3d.com/2015/12/23/1k-update-calls/
        /// </summary>
        public virtual void GameUpdate()
        {
            return;
        }
        /// <summary>
        /// Implemented form <see cref="IBaseClass">. Allows a easy to log inside the classes that derive from this 
        /// Notice how we can use format of the <see cref="BaseClass.c_logFormat"> instead of creating new one
        /// to this class
        /// </summary>
        public virtual void Log(object message)
        {
            // There's a debug functionalities inside <see cref="System.Analytics"> and hence we're using UnityEngine.Debug
            // instead Debug
            UnityEngine.Debug.Log(string.Format(BaseClass.c_logFormat, this, message.ToString()));
        }
        /// <summary>
        /// <see cref="Log"> 
        /// </summary>
        /// <param name="message"></param>
        public virtual void Error(object message)
        {
            UnityEngine.Debug.LogError(string.Format(BaseClass.c_errFormat, this, message.ToString()));
        }
    }
}