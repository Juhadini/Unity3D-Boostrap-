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

namespace GameSpace
{
    /// <summary>
    /// This is a class that allows us to derive data classes. It's recommended that you always
    /// defined base classes for each project for easier OOP hierarchy.!--
    /// 
    /// Being abstract tells that this function is never used directly. Mearning only the classes
    /// that derive from this class are actually functional in game. For example
    /// 
    /// public class Unit : BaseClass is usable in game, but Baseclass isn't
    /// </summary>
    public abstract class BaseClass : System.IDisposable, IBaseClass
    {

        /// <summary>
        /// Region allows us to fold inside IDE the parts we don't want to see all the time
        /// It's a good pattern to make regions for specific types (like in this case we're
        /// creating a type for instance of the class private members
        /// </summary>
        #region Private Instance Members
        /// <summary>
        /// System.GUID Allows us to create unique identifiers for multiple purposes. In multiplayer
        /// these come handy (especially with Data Orientated Design) when RPC (remote procedure calls)
        /// can be called with specific identifiers
        /// </summary>
        System.Guid _guid;
        #endregion
        /// <summary>
        ///	Using a static list allows us to get both  Mono classes that are our own and the data classes that
        /// are our own to be saved to this. Note that the registeration / unregister has to be done in somewhere.!--
        /// In BaseClass this is done in constructor <see cref="BaseClass()">
        /// </summary>
        private static List<IBaseClass> s_baseClassInstance = new List<IBaseClass>();
        /// <summary>
        /// Returns the list of instance available. Notice internal: https://msdn.microsoft.com/fi-fi/library/7c5ka91b.aspx
		/// 
		/// using accessor get allows us to only send the list to request and other requests can add their own lists.
		/// However, notice that the list can be manipulated still with clear, add and so on in other classes
        /// </summary>
        /// <returns></returns>
        internal static List<IBaseClass> S_BaseClassInstance
        {
            get
            {
                return s_baseClassInstance;
            }
        }
        /// <summary>
        /// Constants differ from statics one final way, they can't be changed after they've been initialized
        /// (hence they're called constants). Defining <see cref="Log"> message <see cref="string.Format"> here
        /// allows us to save memory (each double quote always initialized memory and "s"+"aaa"+"asd" allocate 3+1 times memory)
        /// </summary>
        public const string c_logFormat = "{0} logs: {1}";
        /// <summary>
        /// <see cref="c_logFormat">
        /// </summary>
        public const string c_errFormat = "{0} error: {1}";
        #region Constructors
        /// <summary>
        /// This is a constructor for the data class. Notice how we're implementing this as a public
        /// function. This declaration allows us to use factory pattern
        /// 
        /// http://www.oodesign.com/factory-pattern.html
        /// </summary>
        public BaseClass()
        {
            _guid = new System.Guid();//implementing default GUID
            registerInstance();
        }
        /// <summary>
        /// A constructor that allows the guid to be set to new guid. Notice :this() syntax: it tells
        /// the constructor that the base constructor should be called before this function body (hence
        /// we can set the new guid)
        /// </summary>
        /// <param name="newGuid">The new <see cref="System.Guid"> to set for this object</param>
        public BaseClass(System.Guid newGuid) : this()
        {
            _guid = newGuid;
        }
        /// <summary>
        /// This is a deconstructor. Deconstructors are called when the object of the class is destroyed
        /// (Similar to unity3d's Destroy(something))
        /// </summary>
        ~BaseClass()
        {
            // Do something when <see cref="Dispose"> is called
        }
        #endregion
        /// <summary>
        /// This is a abstract function. All the classes that derive from <see cref="BaseClass"> Must implement
        /// this function.
        /// </summary>
        abstract public void Dispose();
        /// <summary>
        /// implement from <see cref="IBaseClass">. This allows us to add from <see cref="s_baseClassInstance"> instance list
        /// Using a boolean method instead of void allows us also to create easy tests as the function returns
        /// if the registeration was succesful. Note the difference between abstract and virtual: 
		/// 
		/// Abstract: Must be implemented in derived class (abstract method does not have functionalities)
		/// Virtual: Can be extended with override keyword. See <see cref="BaseClassChild"> for details
        /// </summary>
        /// <returns><c>true</c> if this instance was registered, otherwise false</returns>
        public virtual bool registerInstance()
        {
            if (s_baseClassInstance.Contains(this))//Lists can have duplicates and hence the check
                return false;
            s_baseClassInstance.Add(this);
            return true;
        }
        /// <summary>
        /// Same as <see cref="registerInstance">, but for unregistering 
        /// </summary>
        /// <returns><c>true</c> if this instance was removed from register, otherwise false<</returns>
        public virtual bool unregisterInstance()
        {
            if (!s_baseClassInstance.Contains(this))//Does not contains this object of class (luokan olio)
                return false;
            s_baseClassInstance.Remove(this);
            return true;
        }
        /// <summary>
        /// Using this extension from the <see cref="IBaseClass"> we can call data class updates, meaning
        /// we're allowing <see cref="MonoBehaviour.Update" style methods to be called inside of this. Neat?
        /// </summary>
        public virtual void GameUpdate()
        {
            return;
        }
        /// <summary>
        /// Implemented form <see cref="IBaseClass">. Allows a easy to log inside the classes that derive from this 
        /// </summary>
        public virtual void Log(object message)
        {
            // There's a debug functionalities inside <see cref="System.Analytics"> and hence we're using UnityEngine.Debug
            // instead Debug
            UnityEngine.Debug.Log(string.Format(c_logFormat, this, message.ToString()));
        }
        /// <summary>
        /// <see cref="Log"> 
        /// </summary>
        /// <param name="message"></param>
        public virtual void Error(object message)
        {
            UnityEngine.Debug.LogError(string.Format(c_errFormat, this, message.ToString()));
        }
    }
}