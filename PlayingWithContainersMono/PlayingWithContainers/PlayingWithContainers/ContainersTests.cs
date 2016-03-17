using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace UnitTestProject
{

	public abstract class FillMeIn { }    

	[TestFixture]
	public class ContainersTests
	{
		public static object FILL_ME_IN = new Object();

		[Test]
		public void ContainersTests_A_ArrayListSizeIsDynamic()
		{
			//When you worked with Array, the fact that Array is fixed size was glossed over.
			//The size of an array cannot be changed after you allocate it. To get around that
			//you need a class from the System.Collections namespace such as ArrayList
			ArrayList list = new ArrayList();
			Assert.AreEqual(FILL_ME_IN, list.Count);

			list.Add(42);
			Assert.AreEqual(FILL_ME_IN, list.Count);
		}

		[Test]
		public void ContainersTests_B_ArrayListHoldsObjects()
		{
			ArrayList list = new ArrayList();
			System.Reflection.MethodInfo method = list.GetType().GetMethod("Add");
			Assert.AreEqual(typeof(FillMeIn), method.GetParameters()[0].ParameterType);
		}

		[Test]
		public void ContainersTests_C_MustCastWhenRetrieving()
		{
			//There are a few problems with ArrayList holding object references. The first 
			//is that you must cast the items you fetch back to the original type.
			ArrayList list = new ArrayList();
			list.Add(42);
			int x = 0;
			//x = (int)list[0];
			Assert.AreEqual(x, 42);
		}

		[Test]
		public void ContainersTests_D_ArrayListIsNotStronglyTyped()
		{
			//Having to cast everywhere is tedious. But there is also another issue lurking
			//ArrayList can hold more than one type. 
			ArrayList list = new ArrayList();
			list.Add(42);
			list.Add("fourty two");
			Assert.AreEqual(FILL_ME_IN, list[0]);
			Assert.AreEqual(FILL_ME_IN, list[1]);

			//While there are a few cases where it could be nice, instead what it means is that 
			//anytime your code works with an array list you have to check that the element is 
			//of the type you expect.
		}

		[Test]
		public void ContainersTests_E_Boxing()
		{
			short s = 5;
			object os = s;
			Assert.AreEqual(s.GetType(), os.GetType());
			Assert.AreEqual(s, os);

			//While this it is true that everything is an object and all the above passes. Not everything is quite as it seems.
			//Under the covers .Net allocates memory for all value type objects (int, double, bool,...) on the stack. This is 
			//considerably more efficient than a heap allocation. .Net also has the ability to put a value type onto the heap.
			//(for calling methods and other reasons). The process of putting stack data into the heap is called "boxing" the 
			//process of taking the value type off the heap is called "unboxing". We won't go into the details (see Jeffrey 
			//Richter's book if you want details). This subject comes up because every time you put a value type into an 
			//ArrayList it must be boxed. Every time you read it from the ArrayList it must be unboxed. This can be a significat
			//cost.
		}

		[Test]
		public void ContainersTests_F_ABetterDynamicSizeContainer()
		{
			//ArrayList is a .Net 1.0 container. With .Net 2.0 generics were introduced and with it a new set of collections in
			//System.Collections.Generic The array like container is List<T>. List<T> (read "list of T") is a generic class. 
			//The "T" in the definition of List<T> is the type argument. You cannot declare an instace of List<T> without also
			//supplying a type in place of T.
			var list = new List<int>();
			Assert.AreEqual(FILL_ME_IN, list.Count);

			list.Add(42);
			Assert.AreEqual(FILL_ME_IN, list.Count);

			//Now just like int[], you can have a type safe dynamic sized container
			//list.Add("fourty two"); //<--Unlike ArrayList this is illegal.

			//List<T> also solves the boxing/unboxing issues of ArrayList. Unfortunately, you'll have to take Microsoft's word for it
			//as I can't find a way to prove it without some ugly MSIL beyond the scope of these Koans.
		}
		public class Widget
		{
		}

		[Test]
		public void ContainersTests_G_ListWorksWithAnyType()
		{
			//Just as with Array, list will work with any type
			List<Widget> list = new List<Widget>();
			list.Add(new Widget());
			Assert.AreEqual(FILL_ME_IN, list.Count);
		}

		[Test]
		public void ContainersTests_H_InitializingWithValues()
		{
			//Like array you can create a list with an initial set of values easily
			var list = new List<int> { 1, 2, 3 };
			Assert.AreEqual(FILL_ME_IN, list.Count);
		}

		[Test]
		public void ContainersTests_I_AddMultipleItems()
		{
			//You can add multiple items to a list at once
			List<int> list = new List<int>();
			list.AddRange(new[] { 1, 2, 3 });
			Assert.AreEqual(FILL_ME_IN, list.Count);
		}

		[Test]
		public void ContainersTests_J_RandomAccess()
		{
			//Just as with array, you can use the subscript notation to access any element in a list.
			// No casting is needed with generic containers, because the compiler knows it can only have one type,
			// in this case int:
			List<int> list = new List<int> { 5, 6, 7 };
			Assert.AreEqual(FILL_ME_IN, list[2]);
		}

		[Test]
		public void ContainersTests_K_BeyondTheLimits()
		{
			List<int> list = new List<int> { 1, 2, 3 };
			//You cannot attempt to get data that doesn't exist
			//int x = list[3];
			//Uncomment the following assert if using nUinit
			Assert.Throws(typeof(FillMeIn), delegate() { int x = list[3]; });
		}

		[Test]
		public void ContainersTests_L_ConvertingToFixedSize()
		{
			List<int> list = new List<int> { 1, 2, 3 };
			Assert.AreEqual(FILL_ME_IN, list.ToArray());
		}

		[Test]
		public void ContainersTests_M_InsertingInTheMiddle()
		{
			List<int> list = new List<int> { 1, 2, 3 };
			list.Insert(1, 6);
			Assert.AreEqual(FILL_ME_IN, list.ToArray());
		}

		[Test]
		public void ContainersTests_N_RemovingItems()
		{
			// The method Remove removes the first occurance of its argument
			List<int> list = new List<int> { 2, 1, 2, 3 };
			list.Remove(2);
			Assert.AreEqual(FILL_ME_IN, list.ToArray());
		}

		[Test]
		public void ContainersTests_O_RemovingItemsAtACertainPosition()
		{
			// The method RemoveAt removes the element at the given position
			List<int> list = new List<int> { 2, 1, 2, 3 };
			list.RemoveAt(2);
			Assert.AreEqual(FILL_ME_IN, list.ToArray());
		}

		[Test]
		public void ContainersTests_P_OldStack()
		{
			// Stack is another container type that can contain elements of different types
			var array = new[] { 1, 2 };
			Stack stack = new Stack(array);
			stack.Push("last");
			Assert.AreEqual(FILL_ME_IN, stack.ToArray());
		}

		[Test]
		public void ContainersTests_Q_StackPushPop()
		{
			// The generic version of stack is restricted to having elements of only one type
			var stack = new Stack<int>();
			Assert.AreEqual(FILL_ME_IN, stack.Count);

			stack.Push(42);
			Assert.AreEqual(FILL_ME_IN, stack.Count);

			int x = stack.Pop();
			Assert.AreEqual(FILL_ME_IN, x);

			Assert.AreEqual(FILL_ME_IN, stack.Count);
		}

		[Test]
		public void ContainersTests_R_StackOrder()
		{
			// Stack is a first in, last out container (FILO).
			var stack = new Stack<int>();
			stack.Push(1);
			stack.Push(2);
			stack.Push(3);

			Assert.AreEqual(FILL_ME_IN, stack.Pop());
			Assert.AreEqual(FILL_ME_IN, stack.Pop());
			Assert.AreEqual(FILL_ME_IN, stack.Pop());
			Assert.AreEqual(FILL_ME_IN, stack.Count);
		}

		[Test]
		public void ContainersTests_S_PeekingIntoAStack()
		{
			// You can peek at the element that was last added, without removing it as Pop would do.
			var stack = new Stack<int>();
			stack.Push(1);
			stack.Push(2);
			Assert.AreEqual(FILL_ME_IN, stack.Peek());
			Assert.AreEqual(FILL_ME_IN, stack.Count);
		}

		[Test]
		public void ContainersTests_T_Queue()
		{
			// A Queue is a first in first out container (FIFO).
			Queue<string> queue = new Queue<string>();
			queue.Enqueue("one");
			queue.Enqueue("two");
			queue.Enqueue("three");

			Assert.AreEqual(FILL_ME_IN, queue.Dequeue());
			Assert.AreEqual(FILL_ME_IN, queue.Dequeue());
			Assert.AreEqual(FILL_ME_IN, queue.Dequeue());
			Assert.AreEqual(FILL_ME_IN, queue.Count);
		}

		[Test]
		public void ContainersTests_U_PeekingIntoAQueue()
		{
			// Peek allows you to see which element Dequeue would return, but without removing it.
			Queue<string> queue = new Queue<string>();
			queue.Enqueue("one");
			queue.Enqueue("two");
			Assert.AreEqual(FILL_ME_IN, queue.Peek());
		}

		[Test]
		public void ContainersTests_V_AddingToADictionary()
		{
			//Dictionary<TKey, TValue> is .Net's key value store. The key and the value do not need to be the same types.
			Dictionary<int, string> dictionary = new Dictionary<int, string>();
			Assert.AreEqual(FILL_ME_IN, dictionary.Count);
			dictionary[1] = "one";
			Assert.AreEqual(FILL_ME_IN, dictionary.Count);
		}

		[Test]
		public void ContainersTests_W_AccessingData()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary["one"] = "uno";
			dictionary["two"] = "dos";
			//The most common way to locate data is with the subscript notation.
			Assert.AreEqual(FILL_ME_IN, dictionary["one"]);
			Assert.AreEqual(FILL_ME_IN, dictionary["two"]);
		}

		[Test]
		public void ContainersTests_X_AccessingDataNotAdded()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary["one"] = "uno";
			//string s = dictionary["two"];
			Assert.Throws(typeof(FillMeIn), delegate() { string s = dictionary["two"]; });
		}

		[Test]
		public void ContainersTests_Y_CatchingMissingData()
		{
			//To deal with the throw when data is not there, you could wrap the data access in a try/catch block...
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary["one"] = "uno";
			string result;
			try
			{
				result = dictionary["two"];
			}
			catch (Exception)
			{
				result = "dos";
			}
			Assert.AreEqual(FILL_ME_IN, result);
		}

		[Test]
		public void ContainersTests_Z_PreCheckForMissingData()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary["one"] = "uno";
			string result;
			if (dictionary.ContainsKey("two"))
			{
				result = dictionary["two"];
			}
			else
			{
				result = "dos";
			}
			Assert.AreEqual(FILL_ME_IN, result);
		}

		[Test]
		public void ContainersTests_ZA_TryGetValueForMissingData()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary["one"] = "uno";
			string result;
			if (!dictionary.TryGetValue("two", out result))
			{
				result = "dos";
			}
			Assert.AreEqual(FILL_ME_IN, result);
		}

		[Test]
		public void ContainersTests_ZB_InitializingADictionary()
		{
			//Although it is not common, you can initialize a dictionary...
			var dictionary = new Dictionary<string, string> { { "one", "uno" }, { "two", "dos" } };
			Assert.AreEqual(FILL_ME_IN, dictionary["one"]);
			Assert.AreEqual(FILL_ME_IN, dictionary["two"]);
		}

		[Test]
		public void ContainersTests_ZC_ModifyingData()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary["one"] = "uno";
			dictionary["two"] = "dos";
			dictionary["one"] = "ein";
			Assert.AreEqual(FILL_ME_IN, dictionary["one"]);
		}

		[Test]
		public void ContainersTests_ZD_KeyExists()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary["one"] = "uno";
			Assert.AreEqual(FILL_ME_IN, dictionary.ContainsKey("one"));
			Assert.AreEqual(FILL_ME_IN, dictionary.ContainsKey("two"));
		}

		[Test]
		public void ContainersTests_ZE_ValueExists()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary["one"] = "uno";
			Assert.AreEqual(FILL_ME_IN, dictionary.ContainsValue("uno"));
			Assert.AreEqual(FILL_ME_IN, dictionary.ContainsValue("dos"));
		}

		[Test]
		public void ContainersTests_ZF_f()
		{
			Dictionary<string, int> one = new Dictionary<string, int>();
			one["jim"] = 53;
			one["amy"] = 20;
			one["dan"] = 23;

			Dictionary<string, int> two = new Dictionary<string, int>();
			two["jim"] = 54;
			two["jenny"] = 26;

			foreach (KeyValuePair<string, int> item in two)
			{
				one[item.Key] = item.Value;
			}

			Assert.AreEqual(FILL_ME_IN, one["jim"]);
			Assert.AreEqual(FILL_ME_IN, one["jenny"]);
			Assert.AreEqual(FILL_ME_IN, one["amy"]);
		}

	}
}
