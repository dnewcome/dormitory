using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using Djn.Testing;

namespace DormitoryTests
{
	public class Class1
	{
		[FestTest]
		public void Test1() {
			Djn.ORM.Dormitory.Persist( new { col1 = "foo" } );
			Djn.ORM.Dormitory.Persist( new TestObject() );
			List<object> objects = TableDriver.Row.GetByField( typeof( TestObject ), "col2", "bar" );
		}

		public static void Main() {
			Fest.Run();
			Console.ReadLine();
		}
	}
	public class TestObject
	{
		// Note that we must implement properties in order for the field to be
		// persisted - simple public members will not work.
		public string col2 {
			get { return m_col2; }
		} private string m_col2 = "bar";

		public int col3 {
			get { return m_col3;  }
		} private int m_col3 = 3;
	}
}
