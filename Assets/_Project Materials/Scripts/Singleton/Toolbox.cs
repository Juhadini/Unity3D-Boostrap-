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
    ///  A factory patternish for <see cref="Singleton"> class, usable when working with multiple singleton objects
    /// which can be case when using Unity3D. Notice DisallowMultipleComponent reference, it doesn't allow multiple
    /// toolboxes inside one game object
    /// </summary>
    [DisallowMultipleComponent]
    public class Toolbox : BaseMonoBehaviour
    {
        static List<Singleton> s_singletons = new List<Singleton>();
        /// <summary>
        /// As the logic is easy to handle inside toolbox we can use index i iterator as a private member 
        /// </summary>
        private int _i = 0;
        /// <summary>
        /// Overridden awake, notice how we leave this empty preventing toolbox being added to 
        /// <see cref="BaseClass.s_baseClassInstance">. This is useful when we want to prevent
        /// logic to implemented (iin this case GameUpdate would cause infinite loop <see cref="GameUpdate">)
        /// </summary>
        protected override void Awake()
        {

        }
        /// <summary>
        /// Extended from the <see cref="BaseClass"> . Calls every <see cref="Singleton.GameUpdate">
        /// Notice the try catch outside while loop. If one singleton update would cause an Error
        /// this update would end up in infinite loop as the i is incremented after update call
        /// </summary>
        public override void GameUpdate()
        {
            _i = 0;
            try
            {
                while (_i < s_singletons.Count)
                {
                    s_singletons[_i].GameUpdate();
                    _i++;
                }
            }
            catch (System.Exception e)
            {
                Error(e);
            }
        }
        /// <summary>
        /// Notice how we're not using 
        /// </summary>
        private void Update()
        {
            GameUpdate();//Update singletons first with their updates
            Log("Add toolbox logic after singletons are updated: Update base classes");
            try
            {
                _i = 0;//Remember to initialize parameters before entering a loop!
                while (_i < BaseClass.S_BaseClassInstance.Count)
                {
                    BaseClass.S_BaseClassInstance[_i].GameUpdate();//Update base classes after singleton updates have been iterated
                    _i++;
                }
            }
            catch (System.Exception e)
            {
                Error(e);
            }
        }

        /// <summary>
        /// Useful way to find a singleton. You can use for example: Toolbox.Get<Singleton>().DoSomething() instead of saving
        /// the isngleton pointer to variables (cachinng) or handling defualt inslgeton pattern: Singleton.Instance.DoSOmething() (this
        /// would require you to handle the singleton creation from other than factory. Not easy in large project)
        /// 
        /// T Marks a generic typeparam name="T". more about these: https://msdn.microsoft.com/fi-fi/library/0zk36dx2.aspx
        /// where T:Singleton marks that the generic type requires the type to be singleton or derice from it (for example you can't do
        /// Toolbox.Get<Toolbox>() because  toolbox doesn't derive from sngelton).
        /// </summary>
        /// <returns>Either existing or newly created singleton monobehavour casted to T</returns>
        public static T Get<T>() where T : Singleton
        {
            int i = 0;// Notice why we're not using _i here, as this local function should be thread safe the local paramter can be used
                      //Without going further into detail, the _i might be still be in use with two different threads
            try
            {
                while (i < s_singletons.Count)
                {
                    if (s_singletons[i].GetType() == typeof(T))
                        return s_singletons[i] as T;
                    i++;
                }
                //No instance found yet, use factory pattern to create one!
                Toolbox[] toolbox = Resources.FindObjectsOfTypeAll<Toolbox>();//Try to locate a existing toolbox. Singelton pattern could be used as well
                if (toolbox.Length == 0)
                {
                    GameObject g = new GameObject("Toolbox");//Create hte gameobject to handle component
                    toolbox = new Toolbox[] { g.AddComponent<Toolbox>() };//Attach the component to new game object and save it to the array for futher use
                                                                          //GameObject named Toolbox should now be viisible inside hierarchy root level
                }
                GameObject componentObject = new GameObject(typeof(T).ToString());//We could use nameof(T) here, but it's avaialable since .net 4.0
                componentObject.transform.SetParent(toolbox[0].transform);//Set the component game object to the child of the toolbox (easier to read)
                return componentObject.AddComponent<T>();//Attach the component and return it. After this the Singleton events will trigger
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogError(e);//Notice how we can't use <see cref="BaseClass.Error"> in this case as the error function is available
                                              //only to the instances of the class. TODO: Move the error and log to static functions
            }
            return default(T);//Usually default return null
        }
    }
}