using NUnit.Framework;
using System.Linq;
using System;

namespace PlayingWithContainers
{
	public abstract class FillMeIn { }    

	[TestFixture ()]
	public class Test
	{
		public static object FILL_ME_IN = new Object();

		[Test]
		public void ArraysTests_A_CreatingAnArray()
		{
			var array = new int[3];
			Assert.AreEqual(typeof(FillMeIn), array.GetType());
			Assert.AreEqual(FILL_ME_IN, array.Length);
		}

		[Test]
		public void ArraysTests_B_ArrayLiterals()
		{
			// You can create an array and fill it with elements in one go,
			// in that case you don't need to specify the length.
			int[] array1 = new int[] { 42, 50 };
			Assert.AreEqual(typeof(int), array1[0].GetType());

			//You don't have to specify a type if the arguments can be inferred
			var array2 = new[] { 42, 50 };
			Assert.AreEqual(typeof(int), array2[0].GetType());

			// or even simpler:
			int[] array3 = { 42, 50 };
			Assert.AreEqual(typeof(int), array3[0].GetType());
		}

		[Test]
		public void ArraysTests_C_AccessingElements()
		{
			int[] array1 = { 42, 50 };

			//Are arrays 0-based or 1-based?
			Assert.AreEqual(42, array1[((int)FILL_ME_IN)]);

			//This is important because...
			Assert.AreEqual(array1.IsFixedSize, FILL_ME_IN);

			//...it means we can't do this: array1[2] = 13;
			//array1[2] = 13;
			//Uncomment the following assert if using nUinit
			Assert.Throws(typeof(FillMeIn), delegate() { array1[2] = 13; });

			//This is because the array is fixed at length 1. You could write a function
			//which created a new array bigger than the last, copied the elements over, and
			//returned the new array. Or you could use a container, which we'll see later.
		}

		[Test]
		public void ArraysTests_D_AccessingArrayElements()
		{
			var array = new[] { "peanut", "butter", "and", "jelly" };

			Assert.AreEqual(FILL_ME_IN, array[0]);
			Assert.AreEqual(FILL_ME_IN, array[3]);
		}

		[Test]
		public void ArraysTests_E_SlicingArrays()
		{
			var array = new[] { "peanut", "butter", "and", "jelly" };

			Assert.AreEqual(new string[] { (string)FILL_ME_IN, (string)FILL_ME_IN }, array.Take(2).ToArray());
			Assert.AreEqual(new string[] { (string)FILL_ME_IN, (string)FILL_ME_IN }, array.Skip(1).Take(2).ToArray());
		}

		[Test]
		public void ArraysTests_F_CreatingATwoDimensionalArray()
		{
			int[,] array = new int[3, 2];
			array[2, 1] = 5;
			Assert.AreEqual(FILL_ME_IN, array[2, 1]);
		}

		[Test]
		public void ArraysTests_G_CreatingATwoDimensionalArrayLiteral()
		{
			int[,] array = new int[3, 2]
			{
				{ 1, 2 },
				{ 3, 4 },
				{ 5, 6 }
			};
			Assert.AreEqual(FILL_ME_IN, array[2, 1]);
		}

		[Test]
		public void ArraysTests_H_CreatingAnArrayOfArrays()
		{
			// If you need a non-rectangular two dimensional array,
			// you need to create an array of arrays:
			int[][] array = new int[3][];
			array[0] = new int[1];
			array[1] = new int[2];
			array[2] = new int[3];

			array[2][2] = 5;
			Assert.AreEqual(FILL_ME_IN, array[2][2]);
		}


	}
}

